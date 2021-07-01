function readMore(postId) {
    console.log(postId);
    var x = document.getElementById(`short-content ${postId}`);
    var y = document.getElementById(`whole-content ${postId}`);
    if (y.style.display === "none") {
        x.style.display = "none";
        y.style.display = "block";
    } else {
        x.style.display = "block";
        y.style.display = "none";
    }
}