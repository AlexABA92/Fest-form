// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(
    function () {
        $('#option1').click(function () {
            if (!$('#option1').hasClass('active')) {
                $('#option2').removeClass('active')
                $('#option1').addClass('active')
                organizationFildToggle()
            }
        })
        $('#option2').click(function () {
            if (!$('#option2').hasClass('active')) {
                $('#option1').removeClass('active')
                $('#option2').addClass('active')
                organizationFildToggle()
            }
        })
        $("#parNum").change(function () {
            var sOption = $(this).find("option:selected");
            var num = Math.abs(sOption.val());  // Проверяем значение

            $('#par1').addClass("d-none");
            $('#par2').addClass("d-none");
            $('#par3').addClass("d-none");

            if (num == 1 ) {
                $('#par1').removeClass("d-none");
            }
            if (num == 2) {
                $('#par2').removeClass("d-none");
                $('#par1').removeClass("d-none");
            }
            if (num == 3) {
                $('#par2').removeClass("d-none");
                $('#par1').removeClass("d-none");
                $('#par3').removeClass("d-none");
            }
            //if (num === 1) {
            //    if ($('#par1').hasClass("d-none")) $('#par1').removeClass("d-none")
            //} else if (num === 2) {
            //    $('#par1').hasClass("d-none")
            //    $('#par1').removeClass("d-none")
            //    $('#par2').hasClass("d-none")
            //    $('#par2').removeClass("d-none")
            //} else if (num === 3) {
            //    $('#par1').hasClass("d-none")
            //    $('#par1').removeClass("d-none")
            //    $('#par2').hasClass("d-none")
            //    $('#par2').removeClass("d-none").removeClass("d-none")
            //    $('#par3').hasClass("d-none")
            //    $('#par3').removeClass("d-none")
            //} else {
            //    $('#par1').addClass("d-none")
            //    $('#par2').addClass("d-none")
            //    $('#par3').addClass("d-none")
            //}
         
        });


    })

var organizationFildToggle = function () {
    $("#organizationField").hasClass("invisible")
        ? $("#organizationField").removeClass("invisible").addClass("visible")
        : $("#organizationField").removeClass("visible").addClass("invisible")
}
