var ws = null;

window.onload = function () {
    if (Modernizr.websockets) {
        WriteMessage('support', 'да ');
    }
}

function WriteMessage(idspan, txt) {
    console.log(idspan + " " +  txt)
}

function exe_start(pageId) {
    if (ws == null) {

        try {
            ws = new WebSocket("wss://" + location.host + "/videostatus/1");
        }
        catch(e) {
            ws = new WebSocket("ws://" + location.host + "/videostatus/1");
        }
        
        progressBlock = document.getElementById('progressBlock');
        progressBlock.style.visibility = 'visible';
        ws.onopen = function () { ws.send(pageId); }
        ws.onclose = function (s) { console.log('onclose', s); }
        ws.onmessage = function (evt) { status_answer(evt.data) }
    }
}

function exe_stop() {
    ws.close(3001, 'stopapplication');
    ws = null;
    bstart.disabled = false;
    bstop.disabled = true;
}

function status_answer(data) {
    fileJson = JSON.parse(data);
    handlingText = document.getElementById('videoHandlingText');
    progress = document.getElementById('videoProgress');

    if (fileJson['HandlingAction'].length == 0)
        handlingText.innerHTML = fileJson['LastDoneAction'];
    else
        handlingText.innerHTML = fileJson['LastDoneAction'] + " -> " + fileJson['HandlingAction'];

    progress.style.width = (fileJson['Progress']) + '%';
    progress.innerHTML = (fileJson['Progress']) + '%'

    if (fileJson['Body'] != null) {
        thumbnailImg = document.getElementById('videoThumbnail');
        thumbnailImg.src = fileJson['Body']
    }
}