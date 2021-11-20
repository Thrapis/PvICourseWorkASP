var ws = null;

window.onload = function () {
	if (Modernizr.websockets) {
		WriteMessage('support', 'да ');
	}
}

function WriteMessage(idspan, txt) {
	console.log(idspan + " " +  txt)
}

function exe_start(pageId, lastCommentId) {
	if (ws == null) {

		try {
			ws = new WebSocket("wss://" + location.host + "/newcomments/1");
		}
		catch(e) {
			ws = new WebSocket("ws://" + location.host + "/newcomments/1");
		}
		
		ws.onopen = function () { ws.send(pageId + ';' + (lastCommentId)); }
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

var stringToHTML = function (str) {
	var parser = new DOMParser();
	var doc = parser.parseFromString(str, 'text/html');
	return doc.body;
};

function status_answer(data) {
	let fileJson = JSON.parse(data);

	let container = document.getElementById('commentsContainer');
	var template = document.querySelector('#commentrow');
	const commentClone = template.content.cloneNode(true);

	let authorName = fileJson['AuthorName'];
	let commentDate = fileJson['CommentDate'];
	let text = fileJson['Text'];

	setTimeTemplate = `<div class='video-set-time' onclick="setUserTime('TIME')">TIME</div>`;
	let pattern = /([0-5]?\d:[0-5]?\d)/;
	let newText = (text);

	var mySet = new Set();
	const regexp = RegExp(pattern, 'g');

	while ((matches = regexp.exec(text)) !== null) {
		console.log(`Found ${matches[0]}. Next starts at ${regexp.lastIndex}.`);
		if (mySet.has(matches[0])) continue;
		mySet.add(matches[0]);
		var copy = (setTimeTemplate);
		copy = copy.replaceAll("TIME", matches[0]);
		newText = newText.replaceAll(matches[0], copy);
	}

	let date = new Date(commentDate);
	let formatted = (date.getDate()) + "." + (date.getMonth() + 1) + "." + (date.getFullYear()) +
		" " + (date.getHours()) + ":" + (date.getMinutes()) + ":" + (date.getSeconds());

	commentClone.querySelector('#authorNameField').textContent = authorName;
	commentClone.querySelector('#commentDateField').textContent = formatted;
	commentClone.querySelector('#textField').insertAdjacentHTML('afterbegin', newText)

	container.prepend(commentClone);
}