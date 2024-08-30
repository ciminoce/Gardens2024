function Delete(url, id, nameToDelete) {
    Swal.fire({
        title: `Are you sure you want to delete ${nameToDelete}?`,        text: "You won't be able to revert this!",
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
                        var row = document.getElementById(id);
                        row.remove();
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
