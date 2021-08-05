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
function viewMore() {
    let posts = document.getElementsByClassName("col-lg-6 offset-lg-3");
    let btn = document.getElementById("btn");
        if (postsShown + 5 <= posts.length) {
            btn.disabled = false;
            for (var i = postsShown; i < postsShown + 5; i++) {
                console.log(posts[i]);
                posts[i].style.display = "block";
            }
            postsShown += 5;
        } else if (postsShown + 5 > posts.length && postsShown < posts.length) {
            btn.disabled = false;
            for (var i = postsShown; i < posts.length; i++) {
                posts[i].style.display = "block";
                postsShown++;
            }
            btn.disabled = true;
        } else if (posts.length <= postsShown) {
            btn.disabled = true;
        }
}