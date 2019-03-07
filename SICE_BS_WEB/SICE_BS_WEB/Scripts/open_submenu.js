//jQuery for menu submenu click to open 
$(document).ready(main);

function main() {

    //Muestra y oculta submenú
    $('.dropdown').click(function () {
        
        $(this).children('.dropdown-InformesGenerales').slideToggle();
    });

}
