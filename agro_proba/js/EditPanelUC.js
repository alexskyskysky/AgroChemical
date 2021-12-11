$(function () {
    $('#EditPanel').draggable();

    $('#CursorRB').checkboxradio({ icon: false });
    $('#HandRB').checkboxradio({ icon: false });
    $('#EditingRB').checkboxradio({ icon: false });
    $('#AddingRB').checkboxradio({ icon: false });

    $('#PointRB').checkboxradio({ icon: false, disabled: true });
    $('#LineStringRB').checkboxradio({ icon: false, disabled: true });
    $('#PolygonRB').checkboxradio({ icon: false, disabled: true });

    $('#DeleteFeatureB').button({ icon: false });
    $('#SaveChangesB').button({ icon: false });
    $('#CancelChangesB').button({ icon: false });

    function ShowHideObjectsButtons() {
        if ($('#AddingRB').prop('checked')) {
            $('#PointRB').checkboxradio("enable");
            $('#LineStringRB').checkboxradio("enable");
            $('#PolygonRB').checkboxradio("enable");
        }
        else {
            $('#PointRB').checkboxradio("disable");
            $('#LineStringRB').checkboxradio("disable");
            $('#PolygonRB').checkboxradio("disable");
        }
    }

    var draw_type = "none";

    $('#CursorRB').on('change', function () {
        ShowHideObjectsButtons();
        draw_type = "none";
        Draw.setActive(false);
        Modify.setActive(false);
        Translate.setActive(false);
    });
    $('#HandRB').on('change', function () {
        ShowHideObjectsButtons();
        draw_type = "none";
        Draw.setActive(false);
        Modify.setActive(false);
        Translate.setActive(true);
    });
    $('#EditingRB').on('change', function () {
        ShowHideObjectsButtons();
        draw_type = "none";
        Draw.setActive(false);
        Modify.setActive(true);
        Translate.setActive(false);
    });
    $('#AddingRB').on('change', function () {
        ShowHideObjectsButtons();
        draw_type = "Polygon";
        $('#PolygonRB').prop('checked', true);
        Draw.setActive(true);
        Modify.setActive(false);
        Translate.setActive(false);
    });

    $('#PointRB').on('change', function () {
        if ($('#PointRB').prop('checked'))
        {
            draw_type = "Point";
        }
        Draw.getActive() && Draw.setActive(true);
    });
    $('#LineStringRB').on('change', function () {
        if ($('#LineStringRB').prop('checked')) {
            draw_type = "LineString";
        }
        Draw.getActive() && Draw.setActive(true);
    });
    $('#PolygonRB').on('change', function () {
        if ($('#PolygonRB').prop('checked')) {
            draw_type = "Polygon";
        }
        Draw.getActive() && Draw.setActive(true);
    });

    $('#DeleteFeaturesB').on('click', function () {
        DeleteSelectedFeatures();
    });

    var edit_vector = new ol.layer.Vector();

    //изменение объектов
    var Modify = {
        init: function () {
            this.select = selectClick;

            this.modify = new ol.interaction.Modify({
                features: this.select.getFeatures()
            });
            map.addInteraction(this.modify);

            this.setEvents();
        },
        setEvents: function () {
            var selectedFeatures = this.select.getFeatures();

            this.select.on('change:active', function () {
                selectedFeatures.forEach(selectedFeatures.remove, selectedFeatures);
            });
        },
        setActive: function (active) {
            this.modify.setActive(active);
        },
        destroy: function () {
            map.removeInteraction(this.modify);
        }
    };
    //Modify.init();
    //---------------------------------------

    //рисование объектов
    var Draw = {
        init: function () {
            map.addInteraction(this.Point);
            this.Point.setActive(false);
            map.addInteraction(this.LineString);
            this.LineString.setActive(false);
            map.addInteraction(this.Polygon);
            this.Polygon.setActive(false);
        },
        Point: new ol.interaction.Draw({
            source: edit_vector.getSource(),
            type: ('Point')
        }),
        LineString: new ol.interaction.Draw({
            source: edit_vector.getSource(),
            type: ('LineString')
        }),
        Polygon: new ol.interaction.Draw({
            source: edit_vector.getSource(),
            type: ('Polygon')
        }),
        getActive: function () {
            return this.activeType ? this[this.activeType].getActive() : false;
        },
        setActive: function (active) {
            var type = draw_type;
            if (active && type != "none") {
                this.activeType && this[this.activeType].setActive(false);
                this[type].setActive(true);
                this.activeType = type;
            } else {
                this.activeType && this[this.activeType].setActive(false);
                this.activeType = null;
            }
        },
        destroy: function () {
            map.removeInteraction(this.Point);
            map.removeInteraction(this.LineString);
            map.removeInteraction(this.Polygon);
        }
    };
    //Draw.init();
    //---------------------------------------

    //перемещение объектов
    var Translate = {
        init: function () {
            this.select = selectClick;

            this.translate = new ol.interaction.Translate({
                features: this.select.getFeatures()
            });
            map.addInteraction(this.translate);
        },
        setActive: function (active) {
            this.translate.setActive(active);
        },
        destroy: function () {
            map.removeInteraction(this.translate);
        }
    };
    //Translate.init();
    //---------------------------------------

    /*Draw.setActive(false);
    Modify.setActive(false);
    Translate.setActive(false);*/

    var snap = new ol.interaction.Snap();

    function DeleteSelectedFeatures() {
        var selectedFeatures = selectClick.getFeatures();
        selectedFeatures.forEach(function (feature) {
            edit_vector.getSource().removeFeature(feature);
        });
    }

    function StartEdit() {
        edit_vector = vector_plots;

        Modify.init();
        Draw.init();
        Translate.init();

        Draw.setActive(false);
        Modify.setActive(false);
        Translate.setActive(false);

        snap = new ol.interaction.Snap({
            source: edit_vector.getSource()
        });
        map.addInteraction(snap);
    }

    function FinishEdit() {
        map.removeInteraction(this.snap);

        Modify.destroy();
        Draw.destroy();
        Translate.destroy();
    }

    $('#EditPanel').on("click", StartEdit);
});