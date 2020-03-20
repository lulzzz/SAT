/* Global JQuery */
/*
	Page Validator - TECTOS
	by Carlos Limón
	http://www.tectos.com.mx
 */
(function ($) {
    "use strict";
    
	/**
	 * Función de Validación de Forma
     * @param {String}
     * forma (optional) action
	 */
    $.fn.validaPaginaActual = function (forma) {
        var form = $(this);
        if (!form[0])
            return form; // Validando si la forma no existe

        if (typeof (forma) == 'string') {
            var operacion, mensaje;
            //Validando forma
            switch (forma) {
				/** Diccionario de Datos**/
                case 'DespachoSimplificado':
                    operacion = true;
                    mensaje = 'Usted no esta en el Despacho Simplificado, debe mantener la sesión en una sola pestaña.';
                    break;
                case 'Planeacion':
                    operacion = true;
                    mensaje = 'Usted no esta en Planeación, debe mantener la sesión en una sola pestaña.';
                    break;
                default:
                    operacion = false;
                    mensaje = 'Debe especificar una forma.';
                    break;
            }

            //Validando operación y mensaje
            if ((operacion != undefined && operacion != null) && (mensaje != undefined && mensaje != null)) {
				//Mostrando Excepción
                alert(mensaje);
                if (operacion) {
                    window.top.close();
                }
            }
        }
    }

    $.validaPaginaActual = {};
})(jQuery);