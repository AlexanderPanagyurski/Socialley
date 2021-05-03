    //$(document).ready(function () {
    //    $('.paragraph-popover').popover();
    //});
$('.paragraph-popover').popover({
    html: true,
    trigger: 'hover',
}).on('hide.bs.popover', function () {
    if ($(".popover:hover").length) {
        return false;
    }
});

$('body').on('mouseleave', '.popover', function () {
    $('.popover').popover('hide');
});