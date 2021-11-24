function onFileChoice() {
    var fullPath = document.getElementById('videoFile').value;
    if (fullPath) {
        var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
        document.getElementById('videoFileLabel').innerHTML = filename;
    }
}

function disableForm() {
    document.getElementById('videoFile').disabled = true;
    document.getElementById('videoName').disabled = true;
    document.getElementById('videoSubmit').disabled = true;
    document.getElementById('tagsControllerBox').style.pointerEvents = 'none';
};

function onlyLettersKetEvent(evt) {
    var ASCIICode = (evt.which) ? (evt.which) : evt.keyCode;
    console.log(ASCIICode)
    if (ASCIICode => parseInt('a') && ASCIICode <= parseInt('z') || ASCIICode == parseInt('_')) {
        return true
    }
    return false
}

$(function () {
    $("textarea[id='tagText']").on('input', function (e) {
        $(this).val($(this).val().replace(/[^0-9a-zA-Z_]/g, ''));
    });
});

function newTag() {

    let tagTextControl = document.getElementById('tagText');
    let tagTextControlValue = tagTextControl.value;
    let container = document.getElementById('tagsContainer');
    var template = document.querySelector('#tagrow');
    const tagClone = template.content.cloneNode(true);

    if ($("button[id='" + tagTextControlValue + "_tag']").length > 0) {
        tagTextControl.value = "";
        $('#addtag').modal('hide');
        return;
    }
       

    tagClone.getElementById('tagValue').value = tagTextControlValue;
    tagClone.getElementById('tagName').innerHTML = '#' + tagTextControlValue;
    tagClone.querySelector('button').id = tagTextControlValue + '_tag';
    tagClone.querySelector('button').onclick = () => deleteTag(tagTextControlValue + '_tag');

    container.append(tagClone);

    tagTextControl.value = "";
    $('#addtag').modal('hide');
}

function deleteTag(tagId) {
    $("button[id='" + tagId + "']").remove();
}