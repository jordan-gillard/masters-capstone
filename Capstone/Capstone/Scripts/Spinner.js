function ShowSpinnerLoading() {
    $("#loading").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('loading');
    var spinner = new Spinner(opts).spin(target);
    $(target).data('spinner', spinner);
}

function HideSpinnerLoading() {
    var target = document.getElementById('loading');
    $(target).data('spinner').stop();
    $("#loading").css("display", "none")
    $("#loading").fadeOut;
}

function ShowSpinnerSaving() {
    $("#saving").fadeIn();
    var opts = {
        lines: 24, // The number of lines to draw
        length: 8, // The length of each line
        width: 8, // The line thickness
        radius: 20, // The radius of the inner circle
        color: '#123', // #rgb or #rrggbb
        speed: 2, // Rounds per second
        trail: 75, // Afterglow percentage
        shadow: true, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('saving');
    var spinner = new Spinner(opts).spin(target);
    $(target).data('spinner', spinner);
}

function ShowSpinnerMessage(message) {
    $("#messagespinner").fadeIn();
    var opts = {
        lines: 24, // The number of lines to draw
        length: 8, // The length of each line
        width: 8, // The line thickness
        radius: 20, // The radius of the inner circle
        color: '#123', // #rgb or #rrggbb
        speed: 2, // Rounds per second
        trail: 75, // Afterglow percentage
        shadow: true, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('messagespinner');
    var spinner = new Spinner(opts).spin(target);
    $(target).data(message, spinner);
}

function HideSpinner() {
    var target = document.getElementById('saving');
    $(target).data('spinner').stop();
    $("#saving").css("display", "none")
    $("#saving").fadeOut;
}

function HideSpinnerSaving() {
    var target = document.getElementById('saving');
    $(target).data('spinner').stop();
    $("#saving").css("display", "none")
    $("#saving").fadeOut;
}

function HideSpinnerLoading() {
    var target = document.getElementById('loading');
    $(target).data('spinner').stop();
    $("#loading").css("display", "none")
    $("#loading").fadeOut;
}

function ShowSpinnerSearching() {
    $("#searching").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('searching');
    var spinner = new Spinner(opts).spin(target);
}

function ShowSpinnerExecuting() {
    $("#executing").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('executing');
    var spinner = new Spinner(opts).spin(target);
}

function ShowSpinnerWithdrawing() {
    $("#withdrawing").fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var target = document.getElementById('withdrawing');
    var spinner = new Spinner(opts).spin(target);
}