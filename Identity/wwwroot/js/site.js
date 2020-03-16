// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function shake() {
    var all = $(".input-validation-error").map(function () {
        return this;
    }).get();
    console.log(all);
    $.each(all, function (i, val) {
        val.classList.add("shake-animation");
    });
    this.setTimeout(function () {
        $.each(all, function (i, val) {
            val.classList.remove("shake-animation");
        })
    }, 1000);
}