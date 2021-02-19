$("#darkThemeSwitch").change(function () {
    if (this.checked) {
        document.cookie = "DarkTheme=True; path=/"
    }
    else {
        document.cookie = "DarkTheme=False; path=/";
    }
    location.reload();
});