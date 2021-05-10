$(function () {
    var availableTags = {};
    $.ajax({
        url: "/api/searches",
        type: "GET",
        data: JSON.stringify(availableTags),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#tags").autocomplete({
                source: data
            });
        }
    });
});