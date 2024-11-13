var FileBytes = null;
var FileID;
var FileName;

var MainText = document.getElementById("filenameLbl");
var DownloadButton = document.getElementById("downloader");
var LoadingGif = document.getElementById("loadingGif");

document.body.onload = function(){
  FileID = window.location.href.split("?").pop();
  if (!window.location.href.includes("?") || FileID.length<36) {
    UnexpectedResponse("This is an invalid download link");
  }
  else {
  FileName = decodeURIComponent(FileID.slice(36));
  document.getElementById("filenameLbl").innerHTML = FileName;
  GetFile();
  }
}

async function GetFile() {
    const endpoint = "https://api.snakefile.com/files/download/" + FileID;
    try {
      const response = await fetch(endpoint, {
        method:"GET"
    });
      if (!response.ok) {
        return UnexpectedResponse("Unable to download - Response Status: " + response.status)
      }
      FileBytes = await response.text();
    } catch (error) {
      UnexpectedResponse(error.message)
    }
}

function UnexpectedResponse(message) {
  MainText.innerHTML = message;
  console.log(message);
  LoadingGif.style.visibility="hidden";
  DownloadButton.remove();
}

function CreateDownloadable() {
  // utifall användaren försöker ladda ner när filen inte är hämtad
  if (FileBytes==null) {
    LoadingGif.style.visibility="visible";
    setInterval(function() {
      if (FileBytes!=null) {
        CreateDownloadable();
        LoadingGif.style.visibility="hidden";
        clearInterval();
      }
    }, 500);
    return;
  }
  // blob från base64 byte[]
  const byteCharacters = atob(FileBytes);
  const byteNumbers = new Array(byteCharacters.length);
  for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
  }
  const byteArray = new Uint8Array(byteNumbers);

  const blob = new Blob([byteArray], { type: 'application/octet-stream' });

  // skapa a element för att trigga blob nedladdning
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = FileName || 'downloaded-file';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}
