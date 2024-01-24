namespace PinturaImageEditor.Data
{
	public static class Extension
	{
		public static IEnumerable<TSource> Update<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
		{
			foreach (var item in source)
			{
				func(item);
			}
			return source;

		}
	}
}
