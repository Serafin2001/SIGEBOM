//=========================================
// VARIABLES GLOBALES
//=========================================

let inputIdDestino = "";
let inputNombreDestino = "";
let inputCedulaDestino = "";

let seleccionMultiple = false;
let callbackMultiple = null;

//=========================================
// ABRIR MODAL (SELECCIÓN ÚNICA)
//=========================================

function abrirModalBomberos(idDestino, nombreDestino, cedulaDestino = "") {

    seleccionMultiple = false;

    inputIdDestino = idDestino;
    inputNombreDestino = nombreDestino;
    inputCedulaDestino = cedulaDestino;

    cargarBomberos("");

    const modal = new bootstrap.Modal(
        document.getElementById("modalBomberos")
    );

    modal.show();
}

//=========================================
// ABRIR MODAL (SELECCIÓN MÚLTIPLE)
//=========================================

function abrirModalBomberosMultiple(callback) {

    seleccionMultiple = true;

    callbackMultiple = callback;

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

    $("#contenidoBomberos").load(
        "/Usuarios/BuscarBomberos?buscar=" + encodeURIComponent(buscar),
        function () {

            if (seleccionMultiple) {

                $(".seleccionarBombero").each(function () {

                    let boton = $(this);

                    boton.replaceWith(`
                        <input type="checkbox"
                               class="form-check-input chkBombero"
                               data-id="${boton.data("id")}"
                               data-nombre="${boton.data("nombre")}"
                               data-cedula="${boton.data("cedula")}" />
                    `);

                });

            }

        });

}

//=========================================
// BUSCAR EN TIEMPO REAL
//=========================================

$(document).on("keyup", "#txtBuscarBombero", function () {

    cargarBomberos($(this).val());

});

//=========================================
// BOTÓN BUSCAR
//=========================================

$(document).on("click", "#btnBuscarBombero", function () {

    cargarBomberos($("#txtBuscarBombero").val());

});

//=========================================
// SELECCIÓN ÚNICA
//=========================================

$(document).on("click", ".seleccionarBombero", function () {

    if (seleccionMultiple)
        return;

    $("#" + inputIdDestino).val($(this).data("id"));

    $("#" + inputNombreDestino).val($(this).data("nombre"));

    if (inputCedulaDestino !== "") {

        $("#" + inputCedulaDestino).val($(this).data("cedula"));

    }

    bootstrap.Modal
        .getInstance(document.getElementById("modalBomberos"))
        .hide();

});

//=========================================
// ACEPTAR SELECCIÓN MÚLTIPLE
//=========================================

$(document).on("click", "#btnAceptarBomberos", function () {

    if (!seleccionMultiple)
        return;

    let seleccionados = [];

    $(".chkBombero:checked").each(function () {

        seleccionados.push({

            id: $(this).data("id"),
            nombre: $(this).data("nombre"),
            cedula: $(this).data("cedula")

        });

    });

    if (callbackMultiple != null) {

        callbackMultiple(seleccionados);

    }

    bootstrap.Modal
        .getInstance(document.getElementById("modalBomberos"))
        .hide();

});