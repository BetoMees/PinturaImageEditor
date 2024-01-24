using Microsoft.JSInterop;
using PinturaImageEditor.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static PinturaImageEditor.Data.ArchiveModel;

namespace PinturaImageEditor.Services
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
	public class ArchiveService(ILogger<ArchiveService> logger, IJSRuntime js, IWebHostEnvironment env)
	{
		ILogger<ArchiveService> Logger = logger;
		IJSRuntime JS = js;
		IWebHostEnvironment Env = env;

		const string ImagesFolder = "/Images/";
		const int bufferSize = 32256;
		const long maxAllowedSize = 1024 * 1024 * 5;
		ImageFormat imageFormat = ImageFormat.Png;

		static string GetMD5(byte[] bytes)
		{
			var hash = MD5.HashData(bytes);
			var checksum = BitConverter.ToString(hash)
				.Replace("-", String.Empty)
				.ToLower();
			return checksum;
		}
		async Task<string> SaveNewImg(string UrlBlob)
		{
			if (!UrlBlob.StartsWith("blob"))
			{
				throw new Exception("Your url must be Blob URL");
			}
			var _jsStreamReference = await JS.InvokeAsync<IJSStreamReference>("ImageEditor.GetBlobFromUrlBlob", UrlBlob);
			var stream = await _jsStreamReference.OpenReadStreamAsync(maxAllowedSize);
			MemoryStream ms = new MemoryStream(Convert.ToInt32(stream.Length));
			var buffer = new byte[bufferSize];
			int bytesRead;
			while ((bytesRead = await stream.ReadAsync(buffer).AsTask()) != 0)
			{
				ms.Write(buffer, 0, bytesRead);
			}

			Image img = Image.FromStream(ms, true);

			if (!Directory.Exists(Env.WebRootPath + ImagesFolder))
			{
				Directory.CreateDirectory(Env.WebRootPath + ImagesFolder);
			}

			var md5Name = GetMD5(ms.GetBuffer());

			String ImageDiretorio = Path.Combine(ImagesFolder, md5Name + "." + imageFormat.ToString());
			img.Save(Env.WebRootPath + ImageDiretorio, imageFormat);
			return ImageDiretorio;
		}

		async Task SaveEditedImg(ArchiveModel item)
		{
			item.Url = await SaveNewImg(item.Url);
			DeleteImg(item.OldUrl);
			item.OldUrl = String.Empty;
		}

		void DeleteImg(string name)
		{
			if (name.StartsWith("blob"))
			{
				return;
			}
			string fileName = Env.WebRootPath + name;
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			else
			{
				Logger.LogError("Archive not found on: " + name);
			}
		}

		public async Task<ArchiveResult> SaveArchive(ArchiveModel item)
		{
			try
			{
				switch (item.Status)
				{
					case EArchiveStatus.New:
						item.Url = await SaveNewImg(item.Url);
						break;
					case EArchiveStatus.Edited:
						await SaveEditedImg(item);
						break;
					case EArchiveStatus.Deleted:
						var name = string.IsNullOrEmpty(item.OldUrl) ? item.Url : item.OldUrl;
						DeleteImg(name);
						break;
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, item.ToString());
				return new() { Success = false, Message = ex.Message };
			}
			return new();
		}
	}
	public class ArchiveResult
	{
		public bool Success { get; set; } = true;
		public string? Message { get; set; }
	}
}
