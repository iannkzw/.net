
$(document).ready(function () {
    $("#tel").inputmask({ "mask": "(99) 99999-9999" }); 

    //Input mask
    /*$('#money').inputmask('decimal', {
        rightAlign: true,
        groupSeparator: '',
        autoGroup: false,
        digits: 2,
        radixPoint: ",",
        digitsOptional: false,
        allowMinus: false,
        prefix: "R$ ",
        placeholder: '',
        autoUnmask: true,
        removeMaskOnSubmit: true*/

    $('#money').inputmask('decimal', {
        radixPoint: ",",
        groupSeparator: ".",
        autoGroup: true,
        digits: 2,
        digitsOptional: false,
        placeholder: '0',
        rightAlign: false,
        removeMaskOnSubmit: true,
        onBeforeMask: function (value, opts) {
            return value;
        }
    });
   
});