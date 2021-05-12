$(document).ready(function () {
    $('#tags').autocomplete({
        minLength: 1,
        source: function (request, response) {
            $.ajax({
                url: '/api/searches',
                method: 'get',
                data: { title: request.term },
                dataType: 'json',
                success: function (data) {
                    response(data);
                }
            });
        }
    })
        .autocomplete('instance')._renderItem = function (ul, item) {
            return $(`<a  href="/Users/UserProfile?userId=${item.id}"<li class="list-group-item d-flex align-items-sm-center autocomplete-list-element">`)
                .append(`<div class="img-fluid autocomoplete-profile-image"><img src="${item.profileImageUrl}" class="rounded-circle" width="50px"></div>`)
                .append(`<div class="flex-fill pl-3 pr-3"><div class="text-dark font-weight-600">${item.userName}</div></div>`)
                .append('</a>')
                .appendTo(ul);
        }
});