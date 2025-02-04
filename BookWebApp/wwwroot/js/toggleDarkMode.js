﻿$(document).ready(function () {

    // This code uses `localStorage` to save "user prefers color" persistently
    // This key used is `user-prefers-color`, and should be one of:
    //  0 = only used at first trigger, to indicate firts time selection
    //  1 = user wants light mode
    //  2 = user wants dark mode
    // the key can also be deleted to indicate user has no preference.

    // function to toggle the css
    function toggle_color_scheme_css($mode) {
        // amend the body classes
        if ($mode == 'dark') {
            $("html").removeClass('light').addClass("dark");
        } else {
            $("html").removeClass("dark").addClass('light');
        }
        // if on user prefers color then update stored value
        $upc = window.localStorage.getItem('user-prefers-color');
        if ($upc !== null) {
            if ($upc == 0) $("#css-save-btn").prop("checked", true);  // first time click
            window.localStorage.setItem('user-prefers-color', ($mode == 'dark') ? 2 : 1);
        }
    }

    // function / listener action to toggle the button
    function update_color_scheme_css() {
        $upc = window.localStorage.getItem('user-prefers-color');
        if (($upc === null) || ($upc == 0)) {
            $mode = (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches) ? 'dark' : 'light';
        } else {
            $mode = ($upc == 2) ? 'dark' : 'light';
        }
        $("#css-toggle-btn").prop("checked", ('dark' == $mode));
        toggle_color_scheme_css($mode);
    }

    // initial mode discovery & update button
    update_color_scheme_css();
    if (window.localStorage.getItem('user-prefers-color') !== null)
        $("#css-save-btn").prop("checked", true);

    // update every time it changes
    if (window.matchMedia) window.matchMedia("(prefers-color-scheme: dark)").addListener(update_color_scheme_css);

    // toggle button click code
    $("#css-toggle-btn").bind("click", function () {
        // disable further automatic toggles
        if (window.localStorage.getItem('user-prefers-color') === null)
            window.localStorage.setItem('user-prefers-color', 0);
        // get current mode, i.e. does body have the `.dark`` classname
        $mode = $("html").hasClass("dark") ? 'light' : 'dark';
        toggle_color_scheme_css($mode);
    });

    // toggle button click code
    $("#css-save-btn").bind("click", function () {
        $chk = $("#css-save-btn").prop("checked");
        if ($chk) {
            // user wants persistance, save current state
            $upc = $("html").hasClass("dark") ? 2 : 1;
            window.localStorage.setItem("user-prefers-color", $upc);
        } else {
            // user doesn't want pesistace, delete saved key
            window.localStorage.removeItem("user-prefers-color");
        }
    });

});