import { appendDefaultEditor } from './PinturaJS/pintura.js';
import locale from './PinturaJS/locale/pt_PT/index.js';

window.ImageEditor = {
    Open: function (file, el, ref) {
        const editor = appendDefaultEditor(el, {
            src: file,
            locale,
            enableDropImage: false,
            enablePasteImage: false,
            imageWriter: {
                targetSize: {
                    width: 1280,
                    height: 720,
                    fit: 'contain',
                    upscale: true,
                },
            }
        });
        editor?.on('process', (res) => {
            ref.invokeMethodAsync('DialogImageEditor.OnSave', { Url: URL.createObjectURL(res.dest), Name: res.dest.name});
        });
    },
    OpenByElement: function (input, el, ref) {
        this.Open(input.files[0], el, ref);
    },
    GetBlobFromUrlBlob: async function (url) {
        const response = await fetch(url)
        return await response.blob()
    }
}

