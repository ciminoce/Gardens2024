var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = new DataTable('#countriesTable', {
        "ajax": {
            "url":"/Admin/Countries/GetAll"
        },
        "columns": [
            { "data": "countryName" },
            {
                "data": "countryId",
                "render": function (data,type,row) {
                    return `
                            <a class="btn btn-warning" href="/Admin/Countries/UpSert?id=${data}" >
                                <i class="bi bi-pencil-square"></i>&nbsp;
                                Edit
                            </a>
                            <a onclick="Delete('/Admin/Countries/Delete/${data}','${row.countryName}')" class="btn btn-danger">
                                <i class="bi bi-trash-fill"></i>
                                Delete
                            </a>

                    `
                }
            }
        ]
    });
}
function Delete(url, nameToDelete) {
    Swal.fire({
        title: `Are you sure you want to delete ${nameToDelete}?`, text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },

                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message)
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    });
}
