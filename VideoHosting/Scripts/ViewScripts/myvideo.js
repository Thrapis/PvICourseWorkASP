function onFileChoice() {
    var fullPath = document.getElementById('imageFile').value;
    if (fullPath) {
        var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
        document.getElementById('imageFileLabel').innerHTML = filename;
    }
}

$(function () {
    $("textarea[id='tagText']").on('input', function (e) {
        $(this).val($(this).val().replace(/[^0-9a-zA-Z_]/g, ''));
    });
});

function onAddTag(pageId) {
    document.getElementById('addTagPageId').value = pageId;
    document.getElementById('addTagForm').setAttribute('data-ajax-update', '#' + pageId + '-tags');
}

function afterNewTag() {
    document.getElementById('tagText').value = '';
    $('#addtag').modal('hide');
}

function onRemoveTag(pageId, name) {
    document.getElementById('removeTagPageId').value = pageId;
    document.getElementById('removeTagName').value = name;
    document.getElementById('removeTagNameQuestion').innerText = '#' + name;
    document.getElementById('removeTagForm').setAttribute('data-ajax-update', '#' + pageId + '-tags');
}

function afterDeleteTag() {
    $('#removetag').modal('hide');
}

function onUpdateVideoName(pageId, videoName) {
    document.getElementById('updateVideoNamePageId').value = pageId;
    document.getElementById('updateVideoNameText').value = videoName;
    document.getElementById('updateVideoNameForm').setAttribute('data-ajax-update', '#' + pageId + '-videoname');
}

function afterUpdateVideoName() {
    $('#updatevideoname').modal('hide');
}

function onUpdateThumbnail(pageId) {
    document.getElementById('updateThumbnailPageId').value = pageId;
    document.getElementById('updateThumbnailForm').setAttribute('data-ajax-update', '#' + pageId + '-thumbnail');
}

function afterUpdateThumbnail() {
    $('#updatethumbnail').modal('hide');
}


function onRemoveVideo(pageId, videoName) {
    console.log(pageId + " (" + videoName + ")");
    document.getElementById('removeVideoQuestion').innerHTML = videoName + " (" + pageId + ")";
    document.getElementById('removeVideoPageId').value = pageId;
    document.getElementById('removeVideoForm').setAttribute('data-ajax-update', '#' + pageId + '-editblock');
}

function afterRemoveVideo() {
    $('#removevideo').modal('hide');
}