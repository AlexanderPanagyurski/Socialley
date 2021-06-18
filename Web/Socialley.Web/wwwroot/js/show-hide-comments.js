function commentsInteract() {
    var x = document.getElementsByClassName("container-comments")[0];
    var y = document.getElementsByClassName("show-hide")[0];
    if (x.style.display === "none") {
        x.style.display = "block";
        y.textContent = "Hide Comments";
    } else {
        x.style.display = "none";
        y.textContent = "Show Comments";
    }
}