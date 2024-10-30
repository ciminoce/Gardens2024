var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = new DataTable('#orderTable', {
        "ajax": {
            "url":"/Customer/Orders/GetAll"
        },
        "columns": [
            { "data": "orderHeaderId" },
            { "data": "orderDate" },
            { "data": "shippingDate" },
            { "data": "orderTotal" },

        ]
    });
}