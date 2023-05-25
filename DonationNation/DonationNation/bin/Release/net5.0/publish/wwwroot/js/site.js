// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {

    var element = $("#footer-country");
    var strings = ["Maldives", "Indonesia", "Sudan"];
    var present = $.inArray(element.text(), strings);

    setInterval(function advance() {
        element.text(strings[++present % strings.length]);
    }, 1500);
}());