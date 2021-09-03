$.noConflict();
jQuery(document).ready(function ($) {
    let uploadCrop;
    let tempFilename;
    let rawImg;
    let imageId;
    function readFile(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('.upload-demo').addClass('ready');
                $('#cropImagePop').modal('show');
                rawImg = e.target.result;
            }
            reader.readAsDataURL(input.files[0]);
        }
        else {
            swal("Sorry - you're browser doesn't support the FileReader API");
        }
    }

    uploadCrop = $('#upload-demo').croppie({
        viewport: {
            width: 250,
            height: 250,
        },
        enforceBoundary: false,
        enableExif: true
    });
    $('#cropImagePop').on('shown.bs.modal', function () {
        // alert('Shown pop');
        uploadCrop.croppie('bind', {
            url: rawImg
        }).then(function () {
            console.log('jQuery bind complete');
        });
    });

    $('.item-img').on('change', function () {
        imageId = $(this).data('id'); tempFilename = $(this).val();
        $('#cancelCropBtn').data('id', imageId); readFile(this);
    });
    $('#cropImageBtn').on('click', function (ev) {
        uploadCrop.croppie('result', {
            type: 'base64',
            format: 'jpeg',
            size: { width: 1000, height: 1000 }
        }).then(function (resp) {
            $('#item-img-output').attr('src', resp);
            $('#cropImagePop').modal('hide');
        });
    });
});