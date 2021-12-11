var xmlRequest;
function CreateXMLHttpRequest() {
    try {
        // Этот код работает, если XMLHttpRequest является частью JavaScript
        xmlRequest = new XMLHttpRequest();
    }
    catch (err) {
        // В противном случае требуется объект ActiveX
        xmlRequest = new ActiveXObject("Microsoft.XMLHTTP");
    }
}

function ClientCallback(arg, context) {
    //alert(arg);
    eval(arg);
}