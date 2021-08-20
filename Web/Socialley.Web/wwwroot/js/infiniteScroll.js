let postsShown = 5;
window.addEventListener('load', () => {
    let posts = document.getElementsByClassName("col-lg-6 offset-lg-3");
    if (posts.length <= 5) {
        for (let i = 0; i < posts.length; i++) {
            posts[i].style.display = "block";
        }
    } else {
        for (let i = 0; i < 5; i++) {
            posts[i].style.display = "block";
        }
        for (let i = 6; i < posts.length; i++) {
            posts[i].style.display = "none";
        }
    }
});
window.addEventListener('scroll', () => {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
        //User is currently at the bottom of the page
        viewMore();
    }
});
function viewMore() {
    let posts = document.getElementsByClassName("col-lg-6 offset-lg-3");
    if (postsShown + 5 <= posts.length) {
        for (var i = postsShown; i < postsShown + 5; i++) {
            console.log(posts[i]);
            posts[i].style.display = "block";
        }
        postsShown += 5;
    } else if (postsShown + 5 > posts.length && postsShown < posts.length) {
        for (var i = postsShown; i < posts.length; i++) {
            posts[i].style.display = "block";
            postsShown++;
        }
    } else if (posts.length <= postsShown) {
    }
}