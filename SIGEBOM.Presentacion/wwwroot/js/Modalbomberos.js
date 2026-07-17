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


let tiempoBusqueda;

$(document).on("keyup",
    "#txtCedula, #txtNombre, #txtApellido",
    function () {

        clearTimeout(tiempoBusqueda);

        tiempoBusqueda = setTimeout(function () {

            buscarBomberos();

        }, 300);

    });

function buscarBomberos() {

    let cedula = $("#txtCedula").val();

    let nombre = $("#txtNombre").val();

    let apellido = $("#txtApellido").val();

    let rango = $("#cmbRango").val();

    $("#contenidoBomberos").load(

        "/Usuarios/BuscarBomberos?" +

        $.param({

            cedula: cedula,

            nombre: nombre,

            apellido: apellido,

            idRango: rango

        })

    );

}

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


$(document).on("click",
    "#btnLimpiarBusqueda",
    function () {

        $("#txtCedula").val("");

        $("#txtNombre").val("");

        $("#txtApellido").val("");

        $("#cmbRango").val("");

        buscarBomberos();

    });