<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestJS_new.aspx.cs" Inherits="agro_proba.TestJS_new" %>

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

            var current_extent;
            var zoom_to_extent = new ol.control.ZoomToExtent();
            map.addControl(zoom_to_extent);

            var vector_territory = new ol.layer.Vector();
            var format = new ol.format.WKT();
            var features_str = [];
            var features_array = [];
            <% agro_proba.TestJS_new helper = new agro_proba.TestJS_new();
                foreach (var item in helper.GetSpatialData())  
               { %>
                    features_str.push('<% =item %>');
            <% } %>

            for (var i = 0; i < features_str.length; i++) {
                features_array[i] = new Array();
                for (var j = 0; j < features_str[i].split('|').length; j++) {
                    features_array[i][features_str[i].split('|')[j].split(':')[0]] = features_str[i].split('|')[j].split(':')[1];
                }
            }

            var territory_source = new ol.source.Vector();
            var territory_feature;
            for (var i = 0; i < features_array.length; i++) {
                territory_feature = format.readFeature(features_array[i]['territory_geo_json']);
                territory_feature.name = features_array[i]['code_territory'];
                for (key in features_array[i])
                {
                    if (key != 'territory_geo_json')
                    {
                        territory_feature.set(key, features_array[i][key]);
                    }
                }
                territory_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
                territory_source.addFeature(territory_feature);
            }
            var count_territory = territory_source.getFeatures().length;
            if (count_territory > 0) {
                vector_territory = new ol.layer.Vector({title: 'Области', source: territory_source, minResolution: 300 });
                current_extent = territory_source.getExtent();
                map.addLayer(vector_territory);
                map.getView().fit(current_extent, map.getSize());
                map.removeControl(zoom_to_extent);
                zoom_to_extent = new ol.control.ZoomToExtent({ extent: current_extent });
                map.addControl(zoom_to_extent);
            }
           
            var selectClick = new ol.interaction.Select({ condition: ol.events.condition.click });
            if (selectClick != null) {
                map.addInteraction(selectClick);
                map.on('singleclick', function (e) {
                    var selectedFeatures = selectClick.getFeatures();
                    if (selectedFeatures.getLength() == 1) {
                        selectedFeatures.forEach(function (feature) {
                            if (feature.getProperties().layer == "soil_points" && feature.getGeometry().getType() == 'Point')
                            {
                                CallServer(('soil_point:' + feature.name), 'null');
                            }
                            if (feature.getProperties().layer == "plots" && (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')) {
                                CallServer(('code_plot:' + feature.name), 'null');
                            }
                        }, this);
                    }
                    else if (selectedFeatures.getLength() > 1) {
                        var unique_id_plots = "";
                        var numbers_plots = "";
                        var sum_area = 0;
                        selectedFeatures.forEach(function (feature) {
                            if (feature.getProperties().layer == "plots" && (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')) {
                                if (unique_id_plots.length > 0) {
                                    if (unique_id_plots[unique_id_plots.length - 1] != ',') { unique_id_plots += ','; }
                                }
                                unique_id_plots += feature.getProperties().unique_id_plot;
                                if (numbers_plots.length > 0) {
                                    if (numbers_plots[numbers_plots.length - 1] != ',') { numbers_plots += ','; }
                                }
                                numbers_plots += feature.getProperties().number_plot;
                                sum_area += feature.getProperties().area;
                            }
                        }, this);
                        if (sum_area > 0) {
                            CallServer('selected_plots:' + unique_id_plots + '|' + numbers_plots + '|' + sum_area, 'null');
                        }
                    }
                });
            }
        </script>
    </form>
</body>
</html>
