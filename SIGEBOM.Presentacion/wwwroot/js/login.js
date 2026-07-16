//=========================================
// MOSTRAR / OCULTAR CONTRASEÑA
//=========================================

document.addEventListener("DOMContentLoaded", function () {

    const boton = document.getElementById("mostrarPassword");

    const password = document.getElementById("password");

    if (!boton || !password)
        return;

    boton.addEventListener("click", function () {

        if (password.type === "password") {

            password.type = "text";

            boton.innerHTML = '<i class="bi bi-eye-slash"></i>';

        }
        else {

            password.type = "password";

            boton.innerHTML = '<i class="bi bi-eye"></i>';

        }

    });

});