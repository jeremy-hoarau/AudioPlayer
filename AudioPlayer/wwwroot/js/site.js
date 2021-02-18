// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#darkThemeSwitch").change(function () {
    if (this.checked) {
        document.cookie = "DarkTheme=True"
    }
    else {
        document.cookie = "DarkTheme=False";
    }
    location.reload();
});