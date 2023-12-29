// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Change the text of the payment dropdown button
$(document).ready(function(){
    $(".dropdown-item").click(function(){
        var selectedItemText = $(this).text();
        $("#paymentMethod").text(selectedItemText);
    });
});


// Hide/show table fields based on radio button selection
document.addEventListener('DOMContentLoaded', function () {
    var radioButtons = document.querySelectorAll('input[name="EntityType"]');
    var company = document.querySelectorAll('.company');
    var person = document.querySelectorAll('.person');

    function toggleVisibility() {
        var selectedValue = document.querySelector('input[name="EntityType"]:checked').value;
        if (selectedValue === 'company') {
            company.forEach(function (content) {
                content.style.display = 'table-row';
            });
            person.forEach(function (content) {
                content.style.display = 'none';
            });
        } else if (selectedValue === 'person') {
            company.forEach(function (content) {
                content.style.display = 'none';
            });
            person.forEach(function (content) {
                content.style.display = 'table-row';
            });
        }
    }

    // Trigger the display logic on each radio button change
    radioButtons.forEach(function (radio) {
        radio.addEventListener('change', toggleVisibility);
    });

    // Also trigger the display logic immediately on page load
    toggleVisibility();
});
