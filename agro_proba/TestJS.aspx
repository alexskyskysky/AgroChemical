<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestJS.aspx.cs" Inherits="agro_proba.TestJS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link type="text/css" href="css/ol.css" rel="stylesheet" />
    <link href="css/jquery-ui.structure.min.css" rel="stylesheet" />
    <link href="css/jquery-ui.theme.min.css" rel="stylesheet" />
    <style type="text/css">
        html { 
            height: 100%;
            overflow:  hidden;
        }  
        body { height: 100%; margin: 0; padding:0; }
        .map:-moz-full-screen {
          height: 100%;
        }
        .map:-webkit-full-screen {
          height: 100%;
        }
        .map:-ms-fullscreen {
          height: 100%;
        }
        .map:fullscreen {
          height: 100%;
        }
        .ol-rotate {
          top: 3em;
        }
        .fullpagewindow {
            height: 100%;
            width: 100%;
        }
        #unlicensed {
            visibility: hidden;
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="js/ol-debug.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="map" class="map"></div>
        <script type="text/javascript">
            var view = new ol.View({
                center: [4151201, 6550197],
                zoom: 9,
                minZoom: 2,
                maxZoom: 20
            });
            var bing_layer = new ol.layer.Tile({ source: new ol.source.BingMaps({ key: 'Ak-dzM4wZjSqTlzveKz5u0d4IQ4bRzVI309GxmkgSVr1ewS6iPSrOvOKhA-CJlm3', imagerySet: 'AerialWithLabels' }) });
            var osm_layer = new ol.layer.Tile({ source: new ol.source.OSM() });
            var format = new ol.format.WKT();
            var map = new ol.Map({ controls: ol.control.defaults().extend([new ol.control.FullScreen()]), layers: [osm_layer, bing_layer], target: 'map', view: view });

            var soil_point_source = new ol.source.Vector();
            var soil_point_feature;
            soil_point_feature = format.readFeature('POINT (38.347265078000078 50.827828742000065)');
            soil_point_feature.name = '12305012014128';
            soil_point_feature.setProperties({ 'id_type_point': '2', 'code_type_point': '2', 'title_type_point': 'Полуяма', 'name_point': 'Пя 35', 'soil_point_geo_json': 'POINT (38.347265078000078 50.827828742000065)', 'date_point': '', 'id_plot': '118719', 'code_plot': '12305012014128', 'number_plot': '128', 'area': '33', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.35668144300007 50.894821725000043)');
            soil_point_feature.name = '1230501201478';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 29', 'soil_point_geo_json': 'POINT (38.35668144300007 50.894821725000043)', 'date_point': '', 'id_plot': '119157', 'code_plot': '1230501201478', 'number_plot': '78', 'area': '84', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.289031603000069 50.851522104000026)');
            soil_point_feature.name = '12305012014140';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 45', 'soil_point_geo_json': 'POINT (38.289031603000069 50.851522104000026)', 'date_point': '', 'id_plot': '120102', 'code_plot': '12305012014140', 'number_plot': '140', 'area': '22', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.264200561000052 50.826385219000031)');
            soil_point_feature.name = '12305012014167';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 62', 'soil_point_geo_json': 'POINT (38.264200561000052 50.826385219000031)', 'date_point': '', 'id_plot': '122477', 'code_plot': '12305012014167', 'number_plot': '167', 'area': '24', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.23995487600007 50.809954800000071)');
            soil_point_feature.name = '1230501201437';
            soil_point_feature.setProperties({ 'id_type_point': '2', 'code_type_point': '2', 'title_type_point': 'Полуяма', 'name_point': 'Пя 67', 'soil_point_geo_json': 'POINT (38.23995487600007 50.809954800000071)', 'date_point': '', 'id_plot': '124602', 'code_plot': '1230501201437', 'number_plot': '37', 'area': '66', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.383853141000031 50.829834253000058)');
            soil_point_feature.name = '1230501201459';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 23', 'soil_point_geo_json': 'POINT (38.383853141000031 50.829834253000058)', 'date_point': '', 'id_plot': '127348', 'code_plot': '1230501201459', 'number_plot': '59', 'area': '38', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.423469457000067 50.856269385000076)');
            soil_point_feature.name = '12305012014101';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 10', 'soil_point_geo_json': 'POINT (38.423469457000067 50.856269385000076)', 'date_point': '', 'id_plot': '128264', 'code_plot': '12305012014101', 'number_plot': '101', 'area': '31', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.427884482000024 50.868711321000035)');
            soil_point_feature.name = '12305012014115';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 3', 'soil_point_geo_json': 'POINT (38.427884482000024 50.868711321000035)', 'date_point': '', 'id_plot': '128478', 'code_plot': '12305012014115', 'number_plot': '115', 'area': '32', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.359600292000039 50.837866648000045)');
            soil_point_feature.name = '12305012014152';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 26', 'soil_point_geo_json': 'POINT (38.359600292000039 50.837866648000045)', 'date_point': '', 'id_plot': '128924', 'code_plot': '12305012014152', 'number_plot': '152', 'area': '16', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.314420889000075 50.838754044000041)');
            soil_point_feature.name = '12305012014138';
            soil_point_feature.setProperties({ 'id_type_point': '3', 'code_type_point': '3', 'title_type_point': 'Прикопка', 'name_point': 'Пр 46', 'soil_point_geo_json': 'POINT (38.314420889000075 50.838754044000041)', 'date_point': '', 'id_plot': '129558', 'code_plot': '12305012014138', 'number_plot': '138', 'area': '23', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.276006077000034 50.79297396100003)');
            soil_point_feature.name = '1230501201431';
            soil_point_feature.setProperties({ 'id_type_point': '1', 'code_type_point': '1', 'title_type_point': 'Разрез', 'name_point': 'Рз 56', 'soil_point_geo_json': 'POINT (38.276006077000034 50.79297396100003)', 'date_point': '', 'id_plot': '130759', 'code_plot': '1230501201431', 'number_plot': '31', 'area': '35', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            soil_point_feature = format.readFeature('POINT (38.419857132000061 50.861105365000071)');
            soil_point_feature.name = '12305012014183';
            soil_point_feature.setProperties({ 'id_type_point': '2', 'code_type_point': '2', 'title_type_point': 'Полуяма', 'name_point': 'Пя 9', 'soil_point_geo_json': 'POINT (38.419857132000061 50.861105365000071)', 'date_point': '', 'id_plot': '133586', 'code_plot': '12305012014183', 'number_plot': '183', 'area': '51', 'tour': '9', 'year': '2014', 'id_department': '961', 'code_department': '1230501', 'title_department': 'Отделение №1', 'id_organization': '653', 'code_organization': '12305', 'title_organization': 'СПК "Большевик"', 'id_region': '12', 'code_region': '12' });
            soil_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
            soil_point_source.addFeature(soil_point_feature);
            var count_soil_point = soil_point_source.getFeatures().length;
            if (count_soil_point > 0) {
                var vector_soil_points = new ol.layer.Vector({ source: soil_point_source });
                map.addLayer(vector_soil_points);
                vector_soil_points.setZIndex(2);
            }

            function SetStyles_soil_points__Page() {
                var soil_points_styleCache = {};
                var soil_points_style = function (feature, resolution) {
                    var text_soil = feature.name;
                    if (!soil_points_styleCache[text_soil]) {
                        switch (feature.getProperties().code_type_point) {
                            case '1': {
                                var getText = function (feature, resolution) {
                                    var t = feature.getProperties().name_point;
                                    if (resolution > 300) { t = ''; }
                                    return t;
                                }
                                var canvas = (document.createElement('canvas')); var render = ol.render.toContext((canvas.getContext('2d')), { size: [14, 14], pixelRatio: 1 }); render.setFillStrokeStyle(new ol.style.Fill({
                                    color: 'rgba(0, 255, 197, 1)'
                                }), new ol.style.Stroke({
                                    color: 'rgba(255, 255, 255, 1)',
                                    width: 2
                                }));
                                render.drawPolygonGeometry(new ol.geom.Polygon([[[0, 0], [0, 14], [14, 14], [14, 0], [0, 0]]]));
                                soil_points_styleCache[text_soil] = [new ol.style.Style({
                                    image: new ol.style.Icon({ img: canvas, imgSize: [canvas.width, canvas.height] }),
                                    text: new ol.style.Text({
                                        offsetX: 25,
                                        offsetY: -8,
                                        font: '12px Arial',
                                        text: getText(feature, resolution),
                                        fill: new ol.style.Fill({
                                            color: 'rgba(0, 0, 0, 1)'
                                        }),
                                        stroke: new ol.style.Stroke({
                                            color: 'rgba(0, 0, 0, 1)',
                                            width: 1
                                        })
                                    })
                                })];
                                break;
                            }
                            case '2': {
                                var getText = function (feature, resolution) {
                                    var t = feature.getProperties().name_point;
                                    if (resolution > 300) { t = ''; }
                                    return t;
                                }
                                var canvas = (document.createElement('canvas')); var render = ol.render.toContext((canvas.getContext('2d')), { size: [14, 14], pixelRatio: 1 }); render.setFillStrokeStyle(new ol.style.Fill({
                                    color: 'rgba(0, 255, 197, 1)'
                                }), new ol.style.Stroke({
                                    color: 'rgba(255, 255, 255, 1)',
                                    width: 2
                                }));
                                render.drawPolygonGeometry(new ol.geom.Polygon([[[0, 14], [7, 0], [14, 14], [0, 14]]]));
                                soil_points_styleCache[text_soil] = [new ol.style.Style({
                                    image: new ol.style.Icon({ img: canvas, imgSize: [canvas.width, canvas.height] }),
                                    text: new ol.style.Text({
                                        offsetX: 25,
                                        offsetY: -8, 
                                        font: '12px Arial',
                                        text: getText(feature, resolution),
                                        fill: new ol.style.Fill({
                                            color: 'rgba(0, 0, 0, 1)'
                                        }),
                                        stroke: new ol.style.Stroke({
                                            color: 'rgba(0, 0, 0, 1)',
                                            width: 1
                                        })
                                    })
                                })];
                                break;
                            }
                            case '3': {
                                var getText = function (feature, resolution) {
                                    var t = feature.getProperties().name_point;
                                    if (resolution > 300) { t = ''; }
                                    return t;
                                }
                                soil_points_styleCache[text_soil] = [new ol.style.Style({
                                    image: new ol.style.Circle({
                                        fill: new ol.style.Fill({
                                            color: 'rgba(0, 255, 197, 1)'
                                        }),
                                        stroke: new ol.style.Stroke({
                                            color: 'rgba(255, 255, 255, 1)',
                                            width: 2
                                        }),
                                        radius: 7
                                    }),
                                    text: new ol.style.Text({
                                        offsetX: 25,
                                        offsetY: -8,
                                        font: '12px Arial',
                                        text: getText(feature, resolution),
                                        fill: new ol.style.Fill({
                                            color: 'rgba(0, 0, 0, 1)'
                                        }),
                                        stroke: new ol.style.Stroke({
                                            color: 'rgba(0, 0, 0, 1)',
                                            width: 1
                                        })
                                    })
                                })];
                                break;
                            }
                        }
                    }
                    return soil_points_styleCache[text_soil];
                };
                vector_soil_points.setStyle(soil_points_style);
            }
            SetStyles_soil_points__Page();
            map.on('moveend', function () {
                SetStyles_soil_points__Page();
            });
            
        </script>
                <script type="text/javascript">
                    /*var view = new ol.View({ center: [4151201, 6550197], zoom: 9 });
                    var bing_layer = new ol.layer.Tile({ source: new ol.source.BingMaps({ key: 'Ak-dzM4wZjSqTlzveKz5u0d4IQ4bRzVI309GxmkgSVr1ewS6iPSrOvOKhA-CJlm3', imagerySet: 'AerialWithLabels' }) });
                    var format = new ol.format.WKT();
                    var map = new ol.Map({ controls: ol.control.defaults().extend([new ol.control.FullScreen()]), layers: [bing_layer], target: 'map', view: view });
                    var feature_source = new ol.source.Vector();
                    var vector_plots = new ol.layer.Vector({ source: feature_source, styles:[] });
                    //map.addControl(new ol.control.ZoomToExtent({ extent: feature_source.getExtent() }));
                    //feature_source.getFeatures().length
                    var plots_styleCache = {};
                    var plots_style = function (feature)
                    {
                        var text = feature.get('name');
                        if (!plots_styleCache[text])
                        {
                            if (feature.getProperties().number_plot < 10) {
                                plots_styleCache[text] = [new ol.style.Style({
                                    fill: new ol.style.Fill({
                                        color: 'rgba(0, 255, 100, 0.8)'
                                    }),
                                    stroke: new ol.style.Stroke({
                                        color: 'rgba(255, 0, 0, 1)',
                                        width: 2
                                    })
                                })];
                            }
                            else
                            {
                                plots_styleCache[text] = [new ol.style.Style({
                                    fill: new ol.style.Fill({
                                        color: 'rgba(200, 100, 100, 0.8)'
                                    }),
                                    stroke: new ol.style.Stroke({
                                        color: 'rgba(0, 255, 0, 1)',
                                        width: 1
                                    })
                                })];
                            }
                        }
                        return plots_styleCache[text];
                    };
                    vector_plots.setStyle(plots_style);
                    
                    select.on('select', function(evt){
                        var selected = evt.selected;
                        var deselected = evt.deselected;
            
                        if (selected.length) {
                            selected.forEach(function(feature){
                            console.info(feature);
                            feature.setStyle(style_modify);
                            });
                        } else {
                            deselected.forEach(function(feature){
                            console.info(feature);
                            feature.setStyle(null);
                            });
                        }
                    });
                    
                    var style = new ol.style.Style({
                        image: new ol.style.Circle({
                            fill: new ol.style.Fill({
                                color: 'rgb(255,200,77)'
                            }),
                            stroke: new ol.style.Stroke({
                                color: 'rgba(0,0,0,.2)',
                                width: 1
                            }),
                            radius: 14
                        }),
                        text: new ol.style.Text({
                            font: 'light 10px Arial',
                            text: '1',
                            fill: new ol.style.Fill({ color: 'black' }),
                            stroke: new ol.style.Stroke({ color: 'black', width: 0.5 })
                        })
                    });

                    var selectClick = new ol.interaction.Select({condition: ol.events.condition.click});

                    if (selectClick != null)
                    {
                        map.on("click", function (e) {
                            map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {
                                if (feature.getGeometry().getType() == 'Point')
                                {
                                    CallServer(('soil_point:' + feature.getProperties().code_plot), 'null');
                                }
                                else if (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')
                                {
                                    CallServer(('code_plot:' + feature.getProperties().code_plot), 'null');
                                }
                            });
                        });
                    }*/

                    /*var gs = new GroupStyle();
        
                    var plots_styleCache = {};
                    var plots_style = function (feature) {
                        var text = feature.get('name');
                        if (!plots_styleCache[text]) {
                            switch (feature.getProperties().number_plot){
                                case 1:{plots_styleCache[text] = [new ol.style.Style({
                                            fill: new ol.style.Fill({
                                                color: 'rgba(0, 255, 100, 0.8)'
                                            }),
                                            stroke: new ol.style.Stroke({
                                                color: 'rgba(255, 0, 0, 1)',
                                                width: 2
                                            }),
                                            text: new ol.style.Text({
                                                font: 'light 10px Arial',
                                                text: '1',
                                                fill: new ol.style.Fill({ color: 'black' }),
                                                stroke: new ol.style.Stroke({ color: 'black', width: 0.5 })
                                            })
                                        })];
                                        break;}}
        
                        }
                        return plots_styleCache[text];
                    };
                    for (var i = 1; i < map.getLayers().getLength() ; i++){map.removeLayer(map.getLayers()[i]);}*/
        </script>
    </form>
</body>
</html>
