    //$(document).ready(function () {
    //    $('.paragraph-popover').popover();
//});
var $y = jQuery.noConflict();
$y('.paragraph-popover').popover({
    html: true,
    trigger: 'hover',
}).on('hide.bs.popover', function () {
    if ($y(".popover:hover").length) {
        return false;
    }
});

$y('body').on('mouseleave', '.popover', function () {
    $y('.popover').popover('hide');
});
