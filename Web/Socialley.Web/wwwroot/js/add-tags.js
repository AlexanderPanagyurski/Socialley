const tagContainer = document.querySelector('.tag-container');
const input = document.querySelector('.tag-container input');
let tagsArea = document.querySelector('.tags-area');
console.log(tagsArea.value);

let tags = [];

function createTag(label) {
    const div = document.createElement('div');
    div.setAttribute('class', 'tag');
    const span = document.createElement('span');
    span.innerHTML = label;
    const closeIcon = document.createElement('i');
    closeIcon.innerHTML = 'close';
    closeIcon.setAttribute('class', 'material-icons');
    closeIcon.setAttribute('data-item', label);
    div.appendChild(span);
    div.appendChild(closeIcon);
    return div;
}

function clearTags() {
    document.querySelectorAll('.tag').forEach(tag => {
        tag.parentElement.removeChild(tag);
    });
}

function addTags() {
    clearTags();
    tags.slice().reverse().forEach(tag => {
        tagContainer.prepend(createTag(tag));
    });
}

input.addEventListener('keyup', (e) => {
    let evtobj = window.event ? event : e;
    if (evtobj.keyCode == 13 && evtobj.ctrlKey) {
        e.target.value.split(',').forEach(tag => {
            tags.push(tag);
            tagsArea.value = tags.toString();
        });

        addTags();
        input.value = '';
    }
});
document.addEventListener('click', (e) => {
    console.log(e.target.tagName);
    if (e.target.tagName === 'I') {
        const tagLabel = e.target.getAttribute('data-item');
        const index = tags.indexOf(tagLabel);
        tags = [...tags.slice(0, index), ...tags.slice(index + 1)];
        addTags();
    }
})
for (let i = 0; i < tags.length; i++) {
   
}
input.focus();