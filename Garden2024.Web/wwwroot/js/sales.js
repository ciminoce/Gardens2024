var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = new DataTable('#salesTable', {
        "ajax": {
            "url":"/Admin/Orders/GetAll"
        },
        "columns": [
            {
                "data":"orderHeaderId"
            },
            {
                "data": "applicationUser",
                "render": function (data) {
                    return data.firstName + ' ' + data.lastName;
                }
            },
            {
                "data": "orderDate",
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "orderTotal",
                "render": function (data) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "data": "orderHeaderId",
                "render": function (data) {
                    return `
                             <a class="btn btn-info" href="/Admin/Orders/Details?id=${data}">
                                <i class="bi bi-card-list"></i>&nbsp;
                                Details
                            </a>

                    `
                }
            }
        ]
    });
}