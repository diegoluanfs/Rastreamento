
var url_base = 'http://localhost:5291';

var token = localStorage.getItem('Token-Located');
var headers = {
    'Authorization': 'Bearer ' + token,
    'Content-Type': 'application/json'
};

$('.btn-target').on('click', function () {
    $.ajax({
        url: url_base + '/api/target',
        type: 'POST',
        headers: headers,
        success: function (result) {
            console.log(result)
            location.reload(true);
        },
        error: function (error) {
            reject(error);
            Swal.fire({
                position: "top-end",
                icon: "error",
                title: error.responseJSON.message,
                showConfirmButton: false,
                timer: 2500
            });
        }
    });
});

function GetPoints() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url_base + '/api/targets',
            type: 'GET',
            headers: headers,
            success: function (result) {
                // Chame a função para construir a tabela
                buildTable(result);
            },
            error: function (error) {
                reject(error);
                Swal.fire({
                    position: "top-end",
                    icon: "error",
                    title: error.responseJSON.message,
                    showConfirmButton: false,
                    timer: 2500
                });
            }
        });
    });
}

function buildTable(data) {
    var tableHtml = '<table class="table"><thead><tr><th>Check</th><th>ID</th><th>Latitude Start</th><th>Longitude Start</th><th>Latitude End</th><th>Longitude End</th><th>Color</th><th>Marker</th><th>Excluir</th></tr></thead><tbody>';

    for (var i = 0; i < data.length; i++) {
        var row = data[i];
        var markerIcon = '<i class="fa fa-map-marker" style="color: ' + row.color + '"></i>';

        tableHtml += '<tr>';
        tableHtml += '<td><input type="checkbox" class="check-item" data-id="' + row.id + '"></td>';
        tableHtml += '<td>' + row.id + '</td>';
        tableHtml += '<td>' + row.latitudeStart + '</td>';
        tableHtml += '<td>' + row.longitudeStart + '</td>';
        tableHtml += '<td>' + row.latitudeEnd + '</td>';
        tableHtml += '<td>' + row.longitudeEnd + '</td>';
        tableHtml += '<td>' + row.color + '</td>';
        tableHtml += '<td>' + markerIcon + '</td>';
        tableHtml += '<td><button class="btn-delete" data-id="' + row.id + '" disabled><i class="fa fa-trash"></i></button></td>';
        tableHtml += '</tr>';
    }

    tableHtml += '</tbody></table>';
    $('#table-container').html(tableHtml);

    // Adicione um evento de clique para os botões de exclusão
    $('.btn-delete').on('click', function () {
        var idToDelete = $(this).data('id');
        // Adicione a lógica para exclusão com o ID correspondente
        deleteItem(idToDelete);
    });

    // Adicione um evento de alteração aos checkboxes
    $('.check-item').on('change', function () {
        // Selecione o botão de exclusão correspondente
        var btnDelete = $(this).closest('tr').find('.btn-delete');

        // Habilitar ou desabilitar o botão com base no estado do checkbox
        btnDelete.prop('disabled', !this.checked);
    });
}

function deleteItem(id) {
    $.ajax({
        url: url_base + '/api/target/' + id,
        type: 'DELETE',
        headers: headers,
        success: function (result) {
            console.log("result: ", result)
            if (result.statusCode == 200) {
                GetPoints();
            }
        },
        error: function (error) {
            console.log(error);
            Swal.fire({
                position: "top-end",
                icon: "error",
                title: error.responseJSON.message,
                showConfirmButton: false,
                timer: 2500
            });
        }
    });
}

// Chame a função GetPoints após o carregamento completo do DOM
$(document).ready(function () {
    GetPoints();
});

