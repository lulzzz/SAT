(function ($) {
    $.fn.validationEngineLanguage = function () {
    };
    $.validationEngineLanguage = {
        newLang: function () {
            $.validationEngineLanguage.allRules = {
                "required": { // Add your regex rules here, you can take telephone as an example
                    "regex": "none",
                    "alertText": "* Este campo es obligatorio",
                    "alertTextCheckboxMultiple": "* Por favor seleccione una opción",
                    "alertTextCheckboxe": "* Este checkbox es obligatorio"
                },
                "requiredInFunction": {
                    "func": function (field, rules, i, options) {
                        return (field.val() == "test") ? true : false;
                    },
                    "alertText": "* Field must equal test"
                },
                "minSize": {
                    "regex": "none",
                    "alertText": "* Mínimo de ",
                    "alertText2": " caracteres autorizados"
                },
                "groupRequired": {
                    "regex": "none",
                    "alertText": "* Debe de rellenar al menos uno de los siguientes campos"
                },
                "maxSize": {
                    "regex": "none",
                    "alertText": "* Máximo de ",
                    "alertText2": " caracteres autorizados"
                },
                "min": {
                    "regex": "none",
                    "alertText": "* El valor mínimo es "
                },
                "max": {
                    "regex": "none",
                    "alertText": "* El valor máximo es "
                },
                "past": {
                    "regex": "none",
                    "alertText": "* Fecha anterior a "
                },
                "future": {
                    "regex": "none",
                    "alertText": "* Fecha posterior a "
                },
                "maxCheckbox": {
                    "regex": "none",
                    "alertText": "* Se ha excedido el número de opciones permitidas"
                },
                "minCheckbox": {
                    "regex": "none",
                    "alertText": "* Por favor seleccione ",
                    "alertText2": " opciones"
                },
                "equals": {
                    "regex": "none",
                    "alertText": "* Los campos no coinciden"
                },
                "creditCard": {
                    "regex": "none",
                    "alertText": "* La tarjeta de crédito no es válida"
                },
                "phone": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([\+][0-9]{1,3}[ \.\-])?([\(]{1}[0-9]{2,6}[\)])?([0-9 \.\-\/]{3,20})((x|ext|extension)[ ]?[0-9]{1,4})?$/,
                    "alertText": "* Número de teléfono inválido"
                },
                "email": {
                    // Shamelessly lifted from Scott Gonzalez via the Bassistance Validation plugin http://projects.scottsplayground.com/email_address_validation/
                    "regex": /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i,
                    "alertText": "* Correo inválido"
                },
                "integer": {
                    "regex": /^[\-\+]?\d+$/,
                    "alertText": "* No es un valor entero válido"
                },
                "number": {
                    // Number, including positive, negative, and floating decimal. credit: orefalo
                    "regex": /^[\-\+]?((([0-9]{1,3})([,][0-9]{3})*)|([0-9]+))?([\.]([0-9]+))?$/,
                    "alertText": "* No es un valor decimal válido"
                },
                "positiveNumber": {
                    // Numeros positivos (máx 10) con o sin decimales(máx 2) (Elaboró: Gustavo Luna)
                    "regex": /^[0-9]{1,10}([\.][0-9]{1,2})?$/,
                    "alertText": "* El valor debe ser un número de hasta 10 digitos enteros y 2 decimales"
                },
                "positiveNumber3": {
                    // Numeros positivos (máx 10) con o sin decimales(máx 4) (Elaboró: Carlos Limón)
                    "regex": /^[0-9]{1,10}([\.][0-9]{1,3})?$/,
                    "alertText": "* El valor debe ser un número de hasta 10 digitos enteros y 3 decimales"
                },
                "positiveNumber4": {
                    // Numeros positivos (máx 10) con o sin decimales(máx 4) (Elaboró: Carlos Limón)
                    "regex": /^[0-9]{1,10}([\.][0-9]{1,4})?$/,
                    "alertText": "* El valor debe ser un número de hasta 10 digitos enteros y 4 decimales"
                },
                "positiveNumber6": {
                    // Numeros positivos (máx 10) con o sin decimales(máx 6) (Elaboró: Carlos Limón)
                    "regex": /^[0-9]{1,10}([\.][0-9]{1,6})?$/,
                    "alertText": "* El valor debe ser un número de hasta 10 digitos enteros y 6 decimales"
                },
                "date": {//Fecha dd/MM/aaaa (Elaboró: Gustavo Luna)
                    "regex": /^([0][1-9]|[1-2][0-9]|[3][0-1])[/]([0][1-9]|[1][0-2])[/]([1][9]|[2][0])[0-9][0-9]$/,
                    "alertText": "* Fecha inválida, por favor utiliza el formato dd/MM/aaaa"
                },
                "time24": {//Hora HH:mm (Elaboró: Gustavo Luna)
                    "regex": /^([0-1][0-9]|[2][0-3])[:][0-5][0-9]$/,
                    "alertText": "* Hora inválida, por favor utiliza el formato HH:mm (24 Horas)"
                },
                "dateTime24": {// Fecha y Hora dd/MM/aaaa HH:mm (Elaboró: Gustavo Luna)
                    "regex": /^([0][1-9]|[1-2][0-9]|[3][0-1])[/]([0][1-9]|[1][0-2])[/]([1][9]|[2][0])[0-9][0-9][\s]([0-1][0-9]|[2][0-3])[:][0-5][0-9]$/,
                    "alertText": "* Fecha y hora inválida, por favor utiliza el formato dd/MM/aaaa HH:mm"
                },
                "ipv4": {
                    "regex": /^((([01]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))[.]){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))$/,
                    "alertText": "* Direccion IP inválida"
                },
                "url": {
                    "regex": /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,
                    "alertText": "* URL Inválida"
                },
                "onlyNumberSp": {
                    "regex": /^[0-9\ ]+$/,
                    "alertText": "* Sólo números"
                },
                "onlyLetterSp": {
                    "regex": /^[a-zA-Z\ \']+$/,
                    "alertText": "* Sólo letras"
                },
                "onlyLetterNumber": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "* No se permiten caracteres especiales"
                },                
                "IdCatalogo": {//Formato de catálogo Ej. [Nombre ID: 1] (Elaborado por Gustavo Luna)
                    "regex": /^[^:]+[ID:]([0-9]{1,10})$/,
                    "alertText": "* Selecciona un elemento de la lista de sugerencias"
                },
                "RFC": {//Formato de catálogo Ej. [XAXX010101000] (Elaborado por Carlos Limón)
                    "regex": /^[A-Z&a-z]{3,4}[0-9]{6}[A-Za-z0-9]{3}$/,
                    "alertText": "* RFC Inválido"
                },
                "CURP": {//Formato de catálogo Ej. [XAXX010101AXAXAXX0] (Elaborado por Gustavo Luna)
                    "regex": /^[A-Za-z]{4}[0-9]{6}[A-Za-z]{6}[0-9A-Za-z]{1}[0-9]{1}$/,
                    "alertText": "* CURP Inválido"
                },
                "CLAVENOMINA12": {//Formato clave de Nomina 1.2(Elaborado por Vanessa Ramos)
                    "regex": /^([A-Z]|[a-z]|[0-9]|Ñ|ñ|!|&quot;|%|&amp;|&apos;|´|-  |:|;|&gt;|=|&lt;|@|_|,|\{|\}|`|~|á|é|í|ó|ú|Á|É|Í|Ó|Ú|ü|Ü){3,15}$/,
                "alertText": "* Clave Inválida (Longitud minima 3 , Longitud maxima 15)"
                },
                "CONCEPTONOMINA12": {//Concepto de  Npómina 1.2 (Elaborado por Vanessa Ramos)
                    "regex": /^([A-Z]|[a-z]|[0-9]| |Ñ|ñ|!|&quot;|%|&amp;|&apos;|´|-|:|;|&gt;|=|&lt;|@|_|,|\{|\}|`|~|á|é|í|ó|ú|Á|É|Í|Ó|Ú|ü|Ü){1,100}$/,
                    "alertText": "* Concepto Inválid0 (Longitud minima 1 , Longitud maxima 100)"
                },
            };

        }
    };
    $.validationEngineLanguage.newLang();
})(jQuery);
