//// REQUERIDO
function mostrarCargando(mensaje = "Cargando . . .") {
    let mensajeCargando = document.getElementById("mensajeCargando");
    mensajeCargando.textContent = mensaje;
    const elementoAnimacionCargando = document.querySelector("#elemento-animacioncargando");
    elementoAnimacionCargando.classList.remove("hidden");
    document.body.style.overflow = 'hidden';
}


// REQUERIDO
function ocultarCargando() {
    const elementoAnimacionCargando = document.querySelector("#elemento-animacioncargando");
    elementoAnimacionCargando.classList.add("hidden");
    document.body.style.overflow = '';
}

// Util para retirar loadings colgados entre cambios de vistas. Eliminar si lo considera necesario.
//window.addEventListener("pageshow", () => ocultarCargando());
document.addEventListener("load", () => ocultarCargando());


// Se puede eliminar. funcionalidad para agregar por default animacion cargando a todos los elementos link y form del documento.
const listaLinksDocumento = document.querySelectorAll('a[href*="/"]:not([target="_blank"]):not([href*="#"])');
if (listaLinksDocumento) listaLinksDocumento.forEach(link => link.addEventListener('click', () => mostrarCargando()));
const listaFormsDocumento = document.querySelectorAll("form");
if (listaFormsDocumento) listaFormsDocumento.forEach(form => form.addEventListener('submit', () => mostrarCargando()));


// Logica para el carrusel de swiper
document.addEventListener('DOMContentLoaded', function () {
    const carruseles = document.querySelectorAll('.swiper');
    carruseles.forEach((carrusel) => {
        const contenedorCarrusel = carrusel.querySelector('.swiper-wrapper');
        const itemsCarrusel = contenedorCarrusel ? contenedorCarrusel.querySelectorAll('.swiper-slide ') : [];
        let slidesPerViewValue = 1;
        let xxl = 1;
        let xl = 1;
        let lg = 1;
        let md = 1;
        let sm = 1;
        let xs = 1;
        let xxs = 1;
        let spaceBetweenValue = 20;
        let spaceBetweenValueXxl = 30;
        let spaceBetweenValueXl = 30;
        let spaceBetweenValueLg = 30;
        let spaceBetweenValueMd = 30;
        let spaceBetweenValueSm = 20;
        let spaceBetweenValueXs = 20;
        let spaceBetweenValueXxs = 20;
        let loopValue = true;
        let delayValue = 3000;
        let disableOnInteractionValue = false;
        let swiperPagination = ".swiperPagination";
        let nextElValue = '.swiperNext';
        let prevElValue = '.swiperPrev';
        let centeredSlidesValue = false;
        if (itemsCarrusel.length > 1) {
            loopValue = true;
        } else {
            loopValue = false;
        }
        if (carrusel.classList.contains('carruselPrincipal')) {
            nextElValue = ".nextPrincipal";
            prevElValue = ".prevPrincipal";
            swiperPagination = ".paginationPrincipal"
        }
        if(carrusel.classList.contains('carruselMarcoPrincipal') || carrusel.classList.contains('carruselMarcoMiniatura')){
            return;
        }
        if(carrusel.classList.contains('swiperCuadros')){
            slidesPerViewValue = 2;
            xxl = 2;
            xl = 2;
            lg = 2;
            md = 2;
            sm = 2;
            xs = 2;
            xxs = 2;
            spaceBetweenValue = 16;
            spaceBetweenValueXxl = 16;
            spaceBetweenValueXl = 16;
            spaceBetweenValueLg = 16;
            spaceBetweenValueMd = 16;
            spaceBetweenValueSm = 16;
            spaceBetweenValueXs = 16;
            spaceBetweenValueXxs = 16;
            centeredSlidesValue = true;
        }

        if (itemsCarrusel.length > 0) {
            var swiper = new Swiper(carrusel, {
                grabCursor: true,
                slidesPerView: slidesPerViewValue,
                spaceBetween: spaceBetweenValue,
                loop: loopValue,
                centeredSlides: centeredSlidesValue,
                autoplay: {
                    delay: delayValue,
                    disableOnInteraction: disableOnInteractionValue,
                },
                pagination: {
                    el: swiperPagination,
                    clickable: true,
                },
                navigation: {
                    nextEl: nextElValue,
                    prevEl: prevElValue,
                },
                breakpoints: {
                    1536: {
                        slidesPerView: xxl,
                        spaceBetween: spaceBetweenValueXxl,
                    },
                    1280: {
                        slidesPerView: xl,
                        spaceBetween: spaceBetweenValueXl,
                    },
                    1024: {
                        slidesPerView: lg,
                        spaceBetween: spaceBetweenValueLg,
                    },
                    768: {
                        slidesPerView: md,
                        spaceBetween: spaceBetweenValueMd,
                    },
                    640: {
                        slidesPerView: sm,
                        spaceBetween: spaceBetweenValueSm,
                    },
                    520: {
                        slidesPerView: xs,
                        spaceBetween: spaceBetweenValueXs,
                    },
                    390: {
                        slidesPerView: xxs,
                        spaceBetween: spaceBetweenValueXxs,
                    },
                },
            });
        }
    });


    // Instalaciones: thumbs + principal
    var thumbsEl = document.querySelector('.carruselMarcoMiniatura');
    var principalEl = document.querySelector('.carruselMarcoPrincipal');
    if (thumbsEl && principalEl) {
        var swiperThumbs = new Swiper(thumbsEl, {
            spaceBetween: 16,
            slidesPerView: 4,
            freeMode: true,
            watchSlidesProgress: true,
        });
        new Swiper(principalEl, {
            spaceBetween: 0,
            loop: true,
            effect: 'fade',
            autoplay: {
                delay: 5000,
                disableOnInteraction: false,
            },
            pagination: {
                el: '.instalacionesPaginacion',
                clickable: true,
            },
            navigation: {
                nextEl: '.prevInstalaciones',
                prevEl: '.nextInstalaciones',
            },
            thumbs: {
                swiper: swiperThumbs,
            },
        });
    }
});

let verMensaje = document.getElementById("verMensaje")
function verMensajeUno(){
    verMensaje.classList.remove("hidden");
    verMensaje.classList.add("block")
}
document.addEventListener('DOMContentLoaded', function (){

    if(verMensaje){
        setTimeout(()=>{
            verMensajeUno();
        }, 5000)
    }
})

function mostarOcultarContraseña(elemento) {
    const contenedor = elemento.closest('.inputContraseña');
    const input = contenedor.querySelector('input');
    const iconShow = contenedor.querySelector('.iconShowPassword');
    const iconHidden = contenedor.querySelector('.iconHiddenPassword');

    
    if (input.type === 'password') {
        input.type = 'text';

        iconShow.classList.add('hidden');
        iconHidden.classList.remove('hidden');
    } else {
        input.type = 'password';

        iconShow.classList.remove('hidden');
        iconHidden.classList.add('hidden');
    }
}


/* function AbrirModalEditarFamilia() {
    const EditarFamilia = document.getElementById('ModalEditarFamilia');
    const contenidoEditarFamilia = document.getElementById('contenidoEditarFamilia');

    EditarFamilia.classList.remove('hidden');

    setTimeout(() => {
        contenidoEditarFamilia.classList.remove('translate-y-[100vh]');
    }, 300);

    // Click fuera del contenido
    EditarFamilia.addEventListener('click', cerrarEditarFamiliaFuera);
} */
function AbrirModalEditarFamilia(familia) {
    const EditarFamilia = document.getElementById('ModalEditarFamilia');
    const contenidoEditarFamilia = document.getElementById('contenidoEditarFamilia');

    // 1. Asignar datos principales
    document.getElementById('edit_Id').value = familia.id;
    document.getElementById('familia').value = familia.nombreFamilia;
    // Evalúa ambos casos por si C# lo envió en PascalCase (Username)
    const inputUsername = document.getElementById('edit_Username');
    if (inputUsername) {
        inputUsername.value = familia.username || familia.Username || '';
    }
    document.getElementById('contrasenaFamilia').value = ''; // Se deja vacío para no sobrescribir si no se cambia
    document.getElementById('boletos').value = familia.cantidadBoletos;
    document.getElementById('invitadoCabana').value = familia.opcionCabana ? "true" : "false";

    const inputFecha = document.getElementById('editFechaLimite');
    if (inputFecha) {
        inputFecha.value = familia.fechaLimite ? familia.fechaLimite : '';
    }

    // 2. Renderizar inputs de invitados dinámicamente
    const contenedor = document.getElementById('contenedorInvitadosModal');
    contenedor.innerHTML = ''; // Limpiar contenedor

    const invitados = familia.invitados || familia.Invitados || [];

    invitados.forEach((invitado, index) => {
        const div = document.createElement('div');
        div.className = 'flex flex-col gap-1';

        div.innerHTML = `
            <label class="label">Boleto ${index + 1}</label>
            <!-- Input oculto para conservar el ID del invitado -->
            <input type="hidden" name="Invitados[${index}].Id" value="${invitado.id || invitado.Id}" />
            
            <!-- Input con el nombre completo -->
            <input type="text" 
                   name="Invitados[${index}].NombreCompleto" 
                   value="${invitado.nombreCompleto || invitado.NombreCompleto || ''}" 
                   class="input" 
                   placeholder="Nombre del invitado" />
        `;

        contenedor.appendChild(div);
    });
    
    // 3. Animación de apertura
    EditarFamilia.classList.remove('hidden');

    setTimeout(() => {
        contenidoEditarFamilia.classList.remove('translate-y-[100vh]');
    }, 300);

    EditarFamilia.addEventListener('click', cerrarEditarFamiliaFuera);
}
function cerrarEditarFamilia() {
    const EditarFamilia = document.getElementById('ModalEditarFamilia');
    const contenidoEditarFamilia = document.getElementById('contenidoEditarFamilia');

    contenidoEditarFamilia.classList.add('translate-y-[100vh]');

    setTimeout(() => {
        EditarFamilia.classList.add('hidden');
        EditarFamilia.removeEventListener('click', cerrarEditarFamiliaFuera);
    }, 300);
}

function cerrarEditarFamiliaFuera(e) {
    const contenidoEditarFamilia = document.getElementById('contenidoEditarFamilia');

    // Si el click NO fue dentro del contenido
    if (!contenidoEditarFamilia.contains(e.target)) {
        cerrarEditarFamilia();
    }
}

function confirmarEliminarFamilia() {
    // Tomamos el Id y Nombre del modal activo
    const idFamilia = document.getElementById('edit_Id').value;
    const nombreFamilia = document.getElementById('familia').value;

    if (!idFamilia) return;

    if (confirm(`¿Estás seguro de que deseas eliminar a la familia "${nombreFamilia}"? Esta acción borrará también todos sus boletos asignados.`)) {
        document.getElementById('delete_Id').value = idFamilia;
        document.getElementById('formEliminarFamilia').submit();
    }
}

function abrirMenuMovil() {
    const menuMovil = document.getElementById('menuMovil');
    const ctnMenuMovil = document.getElementById('ctnMenuMovil');

    menuMovil.classList.remove('hidden');

    setTimeout(() => {
        ctnMenuMovil.classList.remove('-translate-x-[500px]');
    }, 10);

    // Click fuera del contenido
    menuMovil.addEventListener('click', cerrarMenuMovilFuera);
}

function cerrarMenuMovil() {
    const menuMovil = document.getElementById('menuMovil');
    const ctnMenuMovil = document.getElementById('ctnMenuMovil');

    ctnMenuMovil.classList.add('-translate-x-[500px]');

    setTimeout(() => {
        menuMovil.classList.add('hidden');
        menuMovil.removeEventListener('click', cerrarMenuMovilFuera);
    }, 300);
}

function cerrarMenuMovilFuera(e) {
    const ctnMenuMovil = document.getElementById('ctnMenuMovil');

    // Si el click NO fue dentro del menú
    if (!ctnMenuMovil.contains(e.target)) {
        cerrarMenuMovil();
    }
}