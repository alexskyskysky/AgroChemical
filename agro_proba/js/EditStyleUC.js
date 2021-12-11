$(function () {
    $("#EditStyleTabs").tabs();
    $(".spinner").spinner({ min: 0, max: 255 });

    $(window).resize(function () {
        var heigth = $("#EditStyleTabs").height();
        var nav_height = $("#EditStyleTabs").find(".ui-tabs-nav").eq(0).outerHeight(true);
        $("#EditStyleTabs").find(".ui-tabs-panel").css("height", heigth - nav_height)
    });

    $("#EditStyleGP").GridPanel({
        width: "700",

        service: {
            select_name: "OLGISMapService.asmx",
            select_command: "GetColors",
            insert_name: "OLGISMapService.asmx",
            insert_command: "InsertColor",
            update_name: "OLGISMapService.asmx",
            update_command: "UpdateColor",
            delete_name: "OLGISMapService.asmx",
            delete_command: "DeleteColor"
        },

        fields: [
            { name: "id_color", title: "Id", type: "text", width: 50, inserting: false },
            { name: "red", title: "Красный", type: "number", width: 100 },
            { name: "green", title: "Зелёный", type: "number", width: 100 },
            { name: "blue", title: "Синий", type: "number", width: 100 },
            { name: "opacity", title: "Непрозрачность", type: "number", width: 150 },
            { name: "description", title: "Описание", type: "text", width: 200 },
            { type: "control" }
        ]
    });
});