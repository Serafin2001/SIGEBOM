// ======================================
// FECHA Y HORA
// ======================================

function actualizarFechaHora() {

    const ahora = new Date();

    const fecha = ahora.toLocaleDateString("es-DO", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric"
    });

    const hora = ahora.toLocaleTimeString("es-DO", {
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: true
    });

    const fechaElemento = document.getElementById("fecha");
    const horaElemento = document.getElementById("hora");

    if (fechaElemento) {
        fechaElemento.textContent = fecha;
    }

    if (horaElemento) {
        horaElemento.textContent = hora;
    }
}

actualizarFechaHora();

setInterval(actualizarFechaHora, 1000);