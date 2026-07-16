//=========================================
// VARIABLES GLOBALES
//=========================================

let inputIdDestino = "";
let inputNombreDestino = "";

//=========================================
// ABRIR MODAL
//=========================================

function abrirModalBomberos(idDestino, nombreDestino) {

    inputIdDestino = idDestino;
    inputNombreDestino = nombreDestino;

    cargarBomberos("");

    const modal = new bootstrap.Modal(
        document.getElementById("modalBomberos")
    );

    modal.show();
}

//=========================================
// CARGAR BOMBEROS
//=========================================

function cargarBomberos(buscar) {

    $("#contenidoBomberos")
        .load("/Usuarios/BuscarBomberos?buscar=" + encodeURIComponent(buscar));

}

//=========================================
// BUSCAR
//=========================================

$(document).on("keyup", "#txtBuscarBombero", function () {

    cargarBomberos($(this).val());

});

//=========================================
// SELECCIONAR BOMBERO
//=========================================

$(document).on("click", ".seleccionarBombero", function () {

    $("#" + inputIdDestino).val($(this).data("id"));

    $("#" + inputNombreDestino).val($(this).data("nombre"));

    bootstrap.Modal
        .getInstance(document.getElementById("modalBomberos"))
        .hide();

});

