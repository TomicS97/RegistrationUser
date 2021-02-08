$(document).ready(function () {
    getUser();
});

$("#registrationUser").on("click", function () {
    var registrationData = {
        FirstName: $("#firstName").val(),
        LastName: $("#lastName").val(),
        Address: $("#address").val(),
        City: $("#city").val(),
        Country: $("#country").val(),
        Email: $("#email").val(),
        Password: $("#password").val(),
        Continent: $("#country").find(':selected').attr('data-name')
    };
    $.ajax({
        url: "/Registration/SaveNewUser/",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(registrationData),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            initRegistrationGrid();
            reload();
        },
        error: function (e) {
            console.log(e);
        }
    })
});


var registarData;
function getUser() {
    $.ajax({
        url: "/Registration/GetUser",
        type: "POST",
        dataType: "json",
        success: function (data) {
            registarData = data;
            initRegistrationGrid();
        }
    });
};

$(document).on('click', '.delteRow', function () {
    var uniqueEmail = $(this)[0].id;
    deleteUser(uniqueEmail);
});

var deleteUser = function (uniqueEmail) {
    $.ajax({
        type: "POST",
        url: "/Registration/DeleteUser",
        data: {
            'Email': uniqueEmail
        },
        success: function (data) {
            initRegistrationGrid(data);
            reload();
        },
        error: function (e) {
            console.log(e);
        }
    });
}

var reload = function () {
    location.reload();
};

var initRegistrationGrid = function () {
    dataGrid = $("#registrationGrid").dxDataGrid({
        dataSource: registarData,
        showColumnLines: true,
        rowAlternationEnabled: true,
        wordWrapEnabled: true,
        remoteOperations: {
            sorting: true,
            paging: true,
            filtering: true
        },
        columns: [
            {
                capton: "Action",
                cellTemplate: function (container, options) {
                    var j = options.row.data;
                    $("<button id='" + j.Email + "' class='btn btn-primary btn-xs red-intense delteRow'  data-toggle='tooltip' data-placement='top' title='Deactivate'>Delete</button>").appendTo(container);
                }
            },
            {
                dataField: "FirstName",
                caption: "First Name",

            },
            {
                dataField: "LastName",
                caption: "Last Name",
            },
            {
                dataField: "Address",
                caption: "Address",
            },
            {
                dataField: "City",
                caption: "City",

            },
            {
                dataField: "Country",
                caption: "Country",

            },
            {
                dataField: "Email",
                caption: "Email",
            }
        ]
    }).dxDataGrid("instance");
};



