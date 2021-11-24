function likeActive() {
    let icon = document.getElementById('like-icon');
    if (icon.classList.contains('fa-thumbs-o-up')) {
        return false;
    }
    else if (icon.classList.contains('fa-thumbs-up')) {
        return true;
    }
}

function dislikeActive() {
    let icon = document.getElementById('dislike-icon');
    if (icon.classList.contains('fa-thumbs-o-down')) {
        return false;
    }
    else if (icon.classList.contains('fa-thumbs-down')) {
        return true;
    }
}

function toggleLike() {
    let icon = document.getElementById('like-icon');
    let counter = document.getElementById('like-counter');
    let count = parseInt(counter.innerText);
    if (!likeActive()) {
        icon.classList.remove('fa-thumbs-o-up');
        icon.classList.add('fa-thumbs-up');
        counter.innerText = count + 1;
    }
    else if (likeActive()) {
        icon.classList.remove('fa-thumbs-up');
        icon.classList.add('fa-thumbs-o-up');
        counter.innerText = count - 1;
    }
}

function toggleDislike() {
    let icon = document.getElementById('dislike-icon');
    let counter = document.getElementById('dislike-counter');
    let count = parseInt(counter.innerText);
    if (!dislikeActive()) {
        icon.classList.remove('fa-thumbs-o-down');
        icon.classList.add('fa-thumbs-down');
        counter.innerText = count + 1;
    }
    else if (dislikeActive()) {
        icon.classList.remove('fa-thumbs-down');
        icon.classList.add('fa-thumbs-o-down');
        counter.innerText = count - 1;
    }
}

function rateResult(data, status, xhr) {
    if (data == 'like') {
        toggleLike();
        if (dislikeActive())
            toggleDislike();
    }
    else if (data == 'dislike') {
        toggleDislike();
        if (likeActive())
            toggleLike();
    }
}

function afterComment() {
    document.getElementById('commentText').value = "";
    $('#addcomment').modal('hide')
}