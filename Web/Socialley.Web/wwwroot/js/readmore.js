function readMore(postId) {
    console.log(postId);
    var x = document.getElementById(`short-content ${postId}`);
    var y = document.getElementById(`whole-content ${postId}`);
    var hyperlink = document.getElementById(`read-more ${postId}`);
    if (y.style.display === "none") {
        x.style.display = "none";
        y.style.display = "block";
        hyperlink.innerHTML = "Read Less";
    } else {
        x.style.display = "block";
        y.style.display = "none";
        hyperlink.innerHTML = "Read More";
    }
}