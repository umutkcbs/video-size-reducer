function dropHandler(ev) {
    console.log('File(s) dropped');
    document.getElementById("degisecekyazi").innerHTML = 'HARİKA! BİR DOSYA YÜKLEDİNİZ!';

    // Prevent default behavior (Prevent file from being opened)
    ev.preventDefault();

    if (ev.dataTransfer.items) {
        // Use DataTransferItemList interface to access the file(s)
        [...ev.dataTransfer.items].forEach((item, i) => {
            // If dropped items aren't files, reject them
            if (item.kind === 'file') {
                const file = item.getAsFile();
                console.log(`… file[${i}].name = ${file.name}`);
                document.getElementById("degisecekyazi").innerHTML = 'HARİKA! BİR DOSYA YÜKLEDİNİZ !';  //

                //let data = document.getElementById("image-file").files[0];
                let formData = new FormData();

                formData.append("file", file);
                fetch('/api/upload', { method: "POST", body: formData });
            }
        });
    } else {
        // Use DataTransfer interface to access the file(s)
        [...ev.dataTransfer.files].forEach((file, i) => {
            console.log(`… file[${i}].name = ${file.name}`);
            document.getElementById("degisecekyazi").innerHTML = 'HARİKA! SADECE BİR DOSYA YÜKLEDİNİZ!';
        });
    }
}
function dragOverHandler(ev) {
    console.log('File(s) in drop zone');

    // Prevent default behavior (Prevent file from being opened)
    ev.preventDefault();
}
