
$(document).ready(function () {
    $("#tel").inputmask({ "mask": "(99) 99999-9999" }); 

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

    $("#cpf").inputmask({
        mask: ["999.999.999-99"],
        keepStatic: true
    });
   
});