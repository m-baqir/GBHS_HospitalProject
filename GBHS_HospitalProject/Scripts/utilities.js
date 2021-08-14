$(document).ready(function () {
    $('#PatientInfo').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Patient/AutoCompleteLoad",
                type: "POST",
                dataType: "json",
                data: {
                    keyword: request.term
                },
                success: function (data) {
                    console.log('Success !' + data);
                    response($.map(data, function (item) {
                        return {
                            label: item.label,
                            value: item.label,
                            key: item.value,
                        };
                    }))
                },


                error: function () {
                    alert('something went wrong !');
                }
            })
        },
        select: function (event, ui) {
            $('#PatientID').val(ui.item.key);
        }
        });
    })