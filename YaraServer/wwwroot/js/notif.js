"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/scanhub").build();

connection.on("ReceiveMessage", function (json) {
    var result = JSON.parse(json);
    var iconVar = '';
    var titleVar = '';
    var typeVar = '';
    switch (result.Tag) {
        case 0:
            iconVar = 'glyphicon glyphicon - info - sign';
            titleVar = 'Information';
            typeVar = 'info';
            break;
        case 1:
            iconVar = 'glyphicon glyphicon-warning-sign';
            titleVar = 'Warning';
            typeVar = 'warning';
            break;
        case 2:
            iconVar = 'glyphicon glyphicon-alert';
            titleVar = 'Alert';
            typeVar = 'danger';
            break;
    }

    $.notify({
        icon: iconVar,
        title: titleVar,
        message: result.SystemName + '; ' + result.OsName,
        url: 'https://localhost:44311/Admin/Report/Details/' + result.ReportId
    }, {
            allow_dismiss: true,
            type: typeVar,
            delay: 5000,
            timer: 1000,
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
    });
});

connection.start().then(function () {
    console.log("salut");
}).catch(function (err) {
    return console.error(err.toString());
});
