using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml;
using System.Text;

namespace agro_proba
{
    public partial class OLGISMap : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        SqlConnection conn;
        Boolean connection_try = false;
        String connStr, id_organization, year, tour, code_plot, querry_string;

        public static double Round(double value, int digits)
        {
            double scale = Math.Pow(10.0, digits);
            double round = Math.Floor(Math.Abs(value) * scale + 0.5);
            return (Math.Sign(value) * round / scale);
        }

        public static void quickSort(int[] a, int l, int r)
        {
            int temp;
            int x = a[l + (r - l) / 2];
            //запись эквивалентна (l+r)/2,
            //но не вызввает переполнения на больших данных
            int i = l;
            int j = r;
            //код в while обычно выносят в процедуру particle
            while (i <= j)
            {
                while (a[i] < x) i++;
                while (a[j] > x) j--;
                if (i <= j)
                {
                    temp = a[i];
                    a[i] = a[j];
                    a[j] = temp;
                    i++;
                    j--;
                }
            }
            if (i < r)
                quickSort(a, i, r);

            if (l < j)
                quickSort(a, l, j);
        }

        public String NotNull(String value)
        {
            if (value != null && String.Compare(value, "null") != 0 && value != String.Empty)
            {
                value = value.Replace('.', ',');
            }
            return ((value == String.Empty || value == null || String.Compare(value, "null") == 0) ? "0" : value);
        }

        /*private double CountDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            latitude1 *= Math.PI / 180;
            latitude2 *= Math.PI / 180;
            longitude1 *= Math.PI / 180;
            longitude2 *= Math.PI / 180;
            double a = Math.Pow(Math.Cos(latitude2) * Math.Sin(longitude2 - longitude1), 2);
            double b = Math.Pow(Math.Cos(latitude1) * Math.Sin(latitude2) - Math.Sin(latitude1) * Math.Cos(latitude2) * Math.Cos(longitude2 - longitude1), 2);
            double c = Math.Sin(latitude1) * Math.Sin(latitude2) + Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Cos(longitude2 - longitude1);
            double d = Math.Atan2(Math.Sqrt(a + b), c);
            double R = 6371200;
            d *= R;
            return d / 1000;
        }*/

        private double CountDistance(double latitude1, double longitude1, double altitude1, double latitude2, double longitude2, double altitude2)
        {
            latitude1 *= Math.PI / 180;
            latitude2 *= Math.PI / 180;
            longitude1 *= Math.PI / 180;
            longitude2 *= Math.PI / 180;
            double a = Math.Pow(Math.Cos(latitude2) * Math.Sin(longitude2 - longitude1), 2);
            double b = Math.Pow(Math.Cos(latitude1) * Math.Sin(latitude2) - Math.Sin(latitude1) * Math.Cos(latitude2) * Math.Cos(longitude2 - longitude1), 2);
            double c = Math.Sin(latitude1) * Math.Sin(latitude2) + Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Cos(longitude2 - longitude1);
            double d = Math.Atan2(Math.Sqrt(a + b), c);
            double R = 6371200;
            d *= R;
            double h = Math.Abs(altitude2 - altitude1);
            double l = Math.Sqrt(Math.Pow(d, 2) + Math.Pow(h, 2));
            return l / 1000;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            connStr = SetConnectionString();
            conn = new SqlConnection(connStr);
            connection_try = TryConnection(connStr);
            HttpCookie for_map;
            if (IsPostBack)
            {
                for_map = Request.Cookies["Agrochim31_For_Map"];
                if (for_map != null)
                {
                    id_organization = for_map["id_organization"].ToString();
                    year = for_map["year"].ToString();
                    tour = for_map["tour"].ToString();
                    code_plot = for_map["code_plot"].ToString();
                }
            }
            
            if (!IsPostBack)
            {
                //реализация обратного вызова
                String callbackRef = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientCallback", "context", true);
                String callbackScript = "function CallServer(arg, context) {" + callbackRef + ";}";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //--------------------------
                String base_js_script;
                //base_js_script = "var view = new ol.View({center: [4151201, 6550197], zoom: 9});" +
                //11799431.182326088,9558909.009231001|4
                base_js_script = //"\nproj4.defs('EPSG:3395', '+proj=merc +lon_0=0 +k=1 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs');" +
                            "\nvar current_projection = 'EPSG:3857';" +
                            "\nvar view = new ol.View({center: [11799431.182326088, 9558909.009231001], zoom: 4});" +
                            "\nvar bing_road_layer = new ol.layer.Tile({source: new ol.source.BingMaps({key: 'Akl-9E6rCVj7WMYXPMiVwv32zVlPwuRUR7aElzBOE2v328UvXmvH9vyVCbc9OGQF',imagerySet: 'Road'}), zIndex: -1, visible: false});" +
                            "\nvar bing_sat_layer = new ol.layer.Tile({source: new ol.source.BingMaps({key: 'Akl-9E6rCVj7WMYXPMiVwv32zVlPwuRUR7aElzBOE2v328UvXmvH9vyVCbc9OGQF',imagerySet: 'Aerial'}), zIndex: -1, visible: false});" +
                            "\nvar bing_hyb_layer = new ol.layer.Tile({source: new ol.source.BingMaps({key: 'Akl-9E6rCVj7WMYXPMiVwv32zVlPwuRUR7aElzBOE2v328UvXmvH9vyVCbc9OGQF',imagerySet: 'AerialWithLabels'}), zIndex: -1, visible: false});" +
                            "\nvar osm_layer =  new ol.layer.Tile({source: new ol.source.OSM(), zIndex: -1});" +
                            //06.05.2016 добавлены слои
                            //старый кадастр http://maps.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer/
                            //новый кадастр http://pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer/
                            "\nvar rr_kadastr_layer = new ol.layer.Tile({projection: 'EPSG:900913', source: new ol.source.TileArcGISRest({url: 'http://pkk5.rosreestr.ru/arcgis/rest/services/Cadastre/Cadastre/MapServer/'}), zIndex: 8, visible: false});" +
                            //"\nvar yandex_sat_layer = new ol.layer.Tile({projection: 'EPSG:3395', source: new ol.source.XYZ({url: 'http://sat02.maps.yandex.net?l=sat&v=2.8.3&z={z}&x={x}&y={y}'}), zIndex: -1, visible: false});" +
                            //"\nvar yandex_map_layer = new ol.layer.Tile({type: 'base', projection: 'EPSG:3395', source: new ol.source.XYZ({url: 'http://vec02.maps.yandex.net?l=map&v=2.8.3&z={z}&x={x}&y={y}'}), zIndex: -1, visible: false});" +
                            //"\nvar yandex_layers_group = new ol.layer.Group({layers: [yandex_map_layer, yandex_sat_layer]});" +
                            "\nvar format = new ol.format.WKT();" +
                            //layers:[bing_layer]
                            "\nvar map = new ol.Map({controls: ol.control.defaults().extend([new ol.control.FullScreen()]), layers:[osm_layer, bing_road_layer, bing_sat_layer, bing_hyb_layer, rr_kadastr_layer], target: 'map', view: view});";
                base_js_script += "\nvar element = document.getElementById('popup');" +
                                  "\nvar popup = new ol.Overlay({element: element, stopEvent: false, offset: [10, 0]});" +
                                  "\nmap.addOverlay(popup);";
                base_js_script += "\nvar id_farm; \nvar backup_content_lagoon; \nfunction SP(lagoon) { CallServer('get_protocols_table:' + lagoon,'null'); }";
                base_js_script += "\nfunction BackupLagoonContent() { document.getElementById('PopupW').innerHTML = backup_content_lagoon; };";
                base_js_script += "\nvar selectClick = new ol.interaction.Select({condition: ol.events.condition.click});";
                base_js_script += "\nif (selectClick != null) {" +
                             "\nmap.addInteraction(selectClick);" +
                             "\nmap.on('singleclick', function (e) {" +
                                 "\nvar selectedFeatures = selectClick.getFeatures();" +
                                 "\nif (selectedFeatures.getLength() == 1) {" +
                                     "\nselectedFeatures.forEach(function (feature) {" +
                                         "\nif (feature.getProperties().layer == 'soil_points' && feature.getGeometry().getType() == 'Point')" +
                                         "\n{CallServer(('soil_point:' + feature.name), 'null');}" +
                                         "\nif (feature.getProperties().layer == 'plots' && (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')) {" +
                                             "\nif($('#Year3CB').val() == null) {SetDateHistoryBook();}" +
                                             "\nCallServer(('code_plot:' + feature.name + '|' + $('#Year3CB').val() + '|' + $('#CultureZoneCB').val()), 'null');}" +
                                         "\nif (feature.getProperties().layer == 'farms')" +
                                            "\n{var coordinates;" +
                                            "\nif(feature.getGeometry().getType() == 'Polygon'  || feature.getGeometry().getType() == 'MultiPolygon')" +
                                            "\n{coordinates = ol.extent.getCenter(feature.getGeometry().getExtent());}" +
                                            "\nelse if(feature.getGeometry().getType() == 'Point') {coordinates = feature.getGeometry().getCoordinates();}" +
                                            "\npopup.setPosition(coordinates);" +
                                            "\nid_farm = feature.getProperties().id_farm;" +
                                            "\nvar content = '<table width=\"500\">' + " +
                                            "'<tr><td width=\"200\">№:</td><td width=\"300\">' + feature.getProperties().number_farm + '</td></tr>' + " +
                                            "'<tr><td>Наименование:</td><td>' + feature.getProperties().title_farm + '</td></tr>' + " +
                                            "'<tr><td>Месторасположение:</td><td>' + feature.getProperties().location_farm + '</td></tr>' + " +
                                            "'<tr><td>Среднегодовое поголовье животных:</td><td>' + feature.getProperties().animal_population + '</td></tr>' + " +
                                            "'<tr><td>Расчетный объем:</td><td>' + feature.getProperties().lagoons_volume + '</td></tr>' + " +
                                            "'</table>';" +
                                            "\ndocument.getElementById('PopupW').innerHTML = ''; document.getElementById('PopupW').innerHTML = content; $(\"#PopupW\").dialog(\"open\");" +
                                            "CallServer('get_lagoons_table:' + id_farm,'null');}" +
                                         //---------------------Лесополосы-----------------------
                                         "\nif (feature.getProperties().layer == 'woodland_belts')" +
                                            "\n{var coordinates;" +
                                            "\nvar start_date = '';" +
                                            "\nif(feature.getGeometry().getType() == 'Polygon'  || feature.getGeometry().getType() == 'MultiPolygon' || feature.getGeometry().getType() == 'LineString')" +
                                            "\n{coordinates = ol.extent.getCenter(feature.getGeometry().getExtent());}" +
                                            "\nelse if(feature.getGeometry().getType() == 'Point') {coordinates = feature.getGeometry().getCoordinates();}" +
                                            "\npopup.setPosition(coordinates);" +
                                            "\nif(feature.getProperties().start_date != null && feature.getProperties().start_date != ''){start_date = feature.getProperties().start_date.split('-')[2] + '.'" +
                                            " + feature.getProperties().start_date.split('-')[1]  + '.' + feature.getProperties().start_date.split('-')[0];}" +
                                            "\nvar content = '<div  style=\"background-color:#ffffff; padding: 10px; border-radius: 7px;\"><table width=\"450\">' + " +
                                            "'<tr><td width=\"200\">Идентификационный номер:</td><td width=\"250\">' + feature.getProperties().title_woodland_belt + '</td></tr>' + " +
                                            "'<tr><td>Тип лесополосы:</td><td>' + feature.getProperties().title_type_woodland_belt + '</td></tr>' + " +
                                            "'<tr><td>Количество рядов:</td><td>' + feature.getProperties().number_of_ranks + '</td></tr>' + " +
                                            "'<tr><td>Основная порода:</td><td>' + feature.getProperties().basic_species + '</td></tr>' + " +
                                            "'<tr><td>Сопутствующая порода:</td><td>' + feature.getProperties().related_species + '</td></tr>' + " +
                                            "'<tr><td>Длина, м:</td><td>' + feature.getProperties().length + '</td></tr>' + " +
                                            "'<tr><td>Ширина, м:</td><td>' + feature.getProperties().width + '</td></tr>' + " +
                                            "'<tr><td>Площадь, га:</td><td>' + feature.getProperties().area + '</td></tr>' + " +
                                            "'<tr><td>Примерные затраты, руб.:</td><td>' + feature.getProperties().cost + '</td></tr>' + " +
                                            "'<tr><td>Дата начала закладки:</td><td>' + start_date + '</td></tr>' + " +
                                            "'</table></div>'; \nelement.innerHTML = content;}" +
                                          //----------------Залужение, выход пород-----------------
                                          "\nif (feature.getProperties().layer == 'grassing')" +
                                            "\n{var coordinates;" +
                                            "\nvar start_date = '';" +
                                            "\nif(feature.getGeometry().getType() == 'Polygon'  || feature.getGeometry().getType() == 'MultiPolygon' || feature.getGeometry().getType() == 'LineString')" +
                                            "\n{coordinates = ol.extent.getCenter(feature.getGeometry().getExtent());}" +
                                            "\nelse if(feature.getGeometry().getType() == 'Point') {coordinates = feature.getGeometry().getCoordinates();}" +
                                            "\npopup.setPosition(coordinates);" +
                                            "\nif(feature.getProperties().start_date != null && feature.getProperties().start_date != ''){start_date = feature.getProperties().start_date.split('-')[2] + '.'" +
                                            " + feature.getProperties().start_date.split('-')[1]  + '.' + feature.getProperties().start_date.split('-')[0];}" +
                                            "\nvar content = '<div  style=\"background-color:#ffffff; padding: 10px; border-radius: 7px;\"><table width=\"450\">' + " +
                                            "'<tr><td width=\"200\">Идентификационный номер:</td><td width=\"250\">' + feature.getProperties().title_grassing + '</td></tr>' + " +
                                            "'<tr><td>Основная культура:</td><td>' + feature.getProperties().basic_culture + '</td></tr>' + " +
                                            "'<tr><td>Сопутствующая культура:</td><td>' + feature.getProperties().related_culture + '</td></tr>' + " +
                                            "'<tr><td>Площадь, га:</td><td>' + feature.getProperties().area + '</td></tr>' + " +
                                            "'<tr><td>Примерные затраты, руб.:</td><td>' + feature.getProperties().cost + '</td></tr>' + " +
                                            "'<tr><td>Дата начала закладки:</td><td>' + start_date + '</td></tr>' + " +
                                            "'</table></div>'; \nelement.innerHTML = content;}" +
                                         //---------------------------------------------------------
                                         "\nif(feature.getProperties().layer == 'territory'){" +
                                            "\n$(\"#TerritoryCB option:selected\").each(function(){$(this).prop('selected', false);});" +
                                            "\n$(\"#TerritoryCB option[value='\" + feature.getProperties().id_territory + \"']\").prop('selected', true);" +
                                            "\nvar values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1];" +
                                            "\nCallServer('select_territory:' + feature.getProperties().id_territory +'|'+ values, 'null');" +
                                            "\nmap.removeInteraction(selectClick); selectClick = new ol.interaction.Select({condition: ol.events.condition.click}); map.addInteraction(selectClick);}" +
                                         "\nif(feature.getProperties().layer == 'regions'){" +
                                            "\n$(\"#RegionCB option:selected\").each(function(){$(this).prop('selected', false);});" +
                                            "\n$(\"#RegionCB option[value='\" + feature.getProperties().id_region + \"']\").prop('selected', true);" +
                                            "\nvar values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1];" +
                                            "\nCallServer('select_region:' + feature.getProperties().id_region +'|'+ values, 'null');" +
                                            "\nmap.removeInteraction(selectClick); selectClick = new ol.interaction.Select({condition: ol.events.condition.click}); map.addInteraction(selectClick);}" +
                                         "\nif(feature.getProperties().layer == 'organizations'){" +
                                            "\n$(\"#OrganizationCB option:selected\").each(function(){$(this).prop('selected', false);});" +
                                            "\n$(\"#OrganizationCB option[value='\" + feature.getProperties().id_organization + \"']\").prop('selected', true);" +
                                            "\nvar values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] + '|' + get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1];" +
                                            "\nCallServer('select_organization:' + feature.getProperties().id_organization +'|'+ values, 'null');" +
                                            "\nmap.removeInteraction(selectClick); selectClick = new ol.interaction.Select({condition: ol.events.condition.click}); map.addInteraction(selectClick);}" +
                                      "}, this);}" +
                                 //"\n$(element).popover({'placement': 'right','html': true,'content': content});" +
                                 //"\n$(element).popover('show');}}, this);}" +
                                 "\nelse if (selectedFeatures.getLength() > 1) {" +
                                     "\nvar unique_id_plots = '';" +
                                     "\nvar numbers_plots = '';" +
                                     "\nvar sum_area = 0;" +
                                     "\nselectedFeatures.forEach(function (feature) {" +
                                         "\nif (feature.getProperties().layer == 'plots' && (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon')) {" +
                                             "\nif (unique_id_plots.length > 0) {" +
                                                 "\nif (unique_id_plots[unique_id_plots.length - 1] != ',') { unique_id_plots += ','; }}" +
                                             "\nunique_id_plots += feature.getProperties().unique_id_plot;" +
                                             "\nif (numbers_plots.length > 0) {" +
                                                 "\nif (numbers_plots[numbers_plots.length - 1] != ',') { numbers_plots += ','; }}" +
                                             "\nnumbers_plots += feature.getProperties().number_plot;" +
                                             "\nsum_area += Number(feature.getProperties().area.replace(/,/,'.'));}}, this);" +
                                     "\nif (sum_area > 0) {" +
                                         "CallServer('selected_plots:' + unique_id_plots + '|' + numbers_plots + '|' + sum_area, 'null');}}" +
                                 "\nelse if (selectedFeatures.getLength() == 0) {element.innerHTML = '';}});}"; //$(element).popover('destroy');

                //запись лог файлов
                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "base_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                SW.Write(base_js_script);
                SW.Close();*/

                base_js_script += "\nvar vector_territory = new ol.layer.Vector();";
                base_js_script += "\nvar vector_regions = new ol.layer.Vector();";
                base_js_script += "\nvar vector_organizations = new ol.layer.Vector();";
                base_js_script += "\nvar vector_plots = new ol.layer.Vector();";
                base_js_script += "\nvar vector_plots_single = new ol.layer.Vector();";
                base_js_script += "\nvar vector_soil = new ol.layer.Vector();";
                base_js_script += "\nvar vector_soil_points = new ol.layer.Vector();";
                base_js_script += "\nvar vector_erosion = new ol.layer.Vector();";
                base_js_script += "\nvar vector_erosion_intersection = new ol.layer.Vector();";
                base_js_script += "\nvar vector_slope = new ol.layer.Vector();";
                base_js_script += "\nvar vector_exposure = new ol.layer.Vector();";
                base_js_script += "\nvar vector_typing = new ol.layer.Vector();";
                base_js_script += "\nvar vector_project_plots = new ol.layer.Vector();";
                base_js_script += "\nvar vector_grassing = new ol.layer.Vector();";
                base_js_script += "\nvar vector_water_objects = new ol.layer.Vector();";
                base_js_script += "\nvar vector_woodland_belts = new ol.layer.Vector();";
                base_js_script += "\nvar vector_track_points = new ol.Collection();";
                base_js_script += "\nvar vector_farms = new ol.layer.Vector();";
                base_js_script += "\nvar vector_lagoons = new ol.layer.Vector();";

                base_js_script += "\nvar track_point_source = new ol.Collection();";
                base_js_script += "\nvar vector_track_lines = new ol.Collection();";
                base_js_script += "\nvar track_point_ln_source = new ol.Collection();";
                base_js_script += "\nvar vector_track_last_point = new ol.Collection();";
                base_js_script += "\nvar track_last_point_source = new ol.Collection();";
                base_js_script += "\nvar track_current_date_rt = {};";
                base_js_script += "\nvar D = {};"; //расстояния по треку
                base_js_script += "\nvar realtime_tracking;";
                base_js_script += "\nvar days_timer = {};";

                base_js_script += "\nvar plots_source = new ol.source.Vector();";
                base_js_script += "\nvar soil_source = new ol.source.Vector();";
                base_js_script += "\nvar slope_source = new ol.source.Vector();";
                base_js_script += "\nvar exposure_source = new ol.source.Vector();";
                base_js_script += "\nvar erosion_source = new ol.source.Vector();";
                base_js_script += "\nvar typing_source = new ol.source.Vector();";
                base_js_script += "\nvar territory_source = new ol.source.Vector();";
                base_js_script += "\nvar regions_source = new ol.source.Vector();";
                base_js_script += "\nvar organizations_source = new ol.source.Vector();";
                base_js_script += "\nvar farms_source = new ol.source.Vector();";
                base_js_script += "\nvar lagoons_source = new ol.source.Vector();";

                base_js_script += "\nvar current_extent;";
                base_js_script += "\nvar zoom_to_extent = new ol.control.ZoomToExtent();";
                base_js_script += "\nmap.addControl(zoom_to_extent);";
                base_js_script += "$(\"#TypePlotCB option:selected\").each(function () { $(this).prop('selected', false); });\n$(\"#TypePlotCB option[value='0']\").prop('selected', true);";
                base_js_script += GetLegendJS("null", "", "", "");

                //"var map = new ol.Map({layers:[bing_layer], target: 'map', view: view});";

                ClientScript.RegisterStartupScript(this.GetType(), "BaseMap", base_js_script, true);

                DataSet layersDS = new DataSet();
                if (connection_try)
                {
                    conn.Open();
                    querry_string = "exec [dbo].[GetTerritoryGeoJSON];";
                    SqlDataAdapter get_territory_geo_data = new SqlDataAdapter(querry_string, conn);
                    get_territory_geo_data.Fill(layersDS, "Territory");
                    conn.Close();

                    String territory_feature_string = String.Empty, territory_feature_name = String.Empty, territory_properties_string = String.Empty;

                    if (CheckRowsCount(layersDS,"Territory"))
                    {
                        String territory_js_script = String.Empty;
                        territory_js_script = "\nterritory_source = new ol.source.Vector();\nvar territory_feature;";
                        for (int i = 0; i < layersDS.Tables["Territory"].Rows.Count; i++)
                        {
                            territory_feature_string = "\nterritory_feature = format.readFeature('" + layersDS.Tables["Territory"].Rows[i]["territory_geo_json"] + "');";
                            territory_feature_name = "\nterritory_feature.name = '" + layersDS.Tables["Territory"].Rows[i]["code_territory"] + "';";
                            for (int j = 0; j < layersDS.Tables["Territory"].Columns.Count; j++)
                            {
                                if (j == 0)
                                {
                                    //properties_string = "\nterritory_feature.setProperties({'" + layersDS.Tables["Territory"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Territory"].Rows[i][j].ToString() + "'";
                                    territory_properties_string = "\nterritory_feature.setProperties({'layer':'territory','id_feature':'" + i.ToString() + "'";
                                }
                                if (layersDS.Tables["Territory"].Columns[j].ColumnName.ToString() != "territory_geo_json")
                                {
                                    if (territory_properties_string[territory_properties_string.Length - 1] != '{' && territory_properties_string[territory_properties_string.Length - 1] != ',')
                                    {
                                        territory_properties_string += ",";
                                    }
                                    territory_properties_string += ("'" + layersDS.Tables["Territory"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Territory"].Rows[i][j].ToString() + "'");
                                }
                                if (j == (layersDS.Tables["Territory"].Columns.Count - 1))
                                {
                                    territory_properties_string += "});";
                                }
                            }
                            territory_js_script += (territory_feature_string + territory_feature_name + territory_properties_string + "\nterritory_feature.getGeometry().transform('EPSG:4326', current_projection);");
                            territory_js_script += "\nterritory_source.addFeature(territory_feature);";
                        }
                        territory_js_script += "\nvar count_territory = territory_source.getFeatures().length; \nif(count_territory > 0){";
                        territory_js_script += "\nvector_territory = new ol.layer.Vector({source: territory_source, projection: 'EPSG:4326', minResolution:600});";
                        territory_js_script += "\ncurrent_extent = territory_source.getExtent();";
                        territory_js_script += "\nmap.addLayer(vector_territory);";
                        //territory_js_script += "\nmap.getView().fit(current_extent, map.getSize());";
                        //territory_js_script += "\nmap.on('moveend', function () {alert(map.getView().getCenter() + '|' + map.getView().getZoom());});";
                        territory_js_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";

                        ClientScript.RegisterStartupScript(this.GetType(), "TerritoryMap", territory_js_script, true);

                        //запись лог файлов
                        /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "territory_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                        SW.Write(territory_js_script);
                        SW.Close();*/

                        //задание стиля для областей
                        String js_script_style = String.Empty;
                        js_script_style = "\nvar territory_colors = {};";
                        js_script_style += "\nSetColors(territory_colors, " + layersDS.Tables["Territory"].Rows.Count + ", 0.2);";
                        js_script_style += ("\nfunction SetStyles_Territory" + ClientID + "(){");
                        js_script_style += "\nvar territory_styleCache = {};";
                        js_script_style += "\nvar territory_style = function (feature, resolution){";
                        js_script_style += ("\nvar text = feature.name;");
                        js_script_style += "\nif (!territory_styleCache[text]){";
                        String fill_str, stroke_str;// font_str;
                        js_script_style += "territory_styleCache[text] = [new ol.style.Style({";
                        fill_str = "\nfill: new ol.style.Fill({";
                        fill_str += "\ncolor: territory_colors[feature.getProperties().id_feature]})";
                        stroke_str = "\nstroke: new ol.style.Stroke({";
                        stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                        stroke_str += "\nwidth: 1})";
                        /*font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                        font_str += "\ntext: feature.getProperties().title_territory,";
                        font_str += "\nfill: new ol.style.Fill({";
                        font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                        font_str += "\nstroke: new ol.style.Stroke({";
                        font_str += "\ncolor: 'rgba(255,255,255,1)',";
                        font_str += "\nwidth: 2})})";*/

                        js_script_style += fill_str;
                        js_script_style += ("," + stroke_str);
                        //js_script_style += ("," + font_str);
                        js_script_style += "})];";

                        js_script_style += "\n}\nreturn territory_styleCache[text];};";
                        js_script_style += "\nvector_territory.setStyle(territory_style);}";
                        js_script_style += "\nSetStyles_Territory" + ClientID + "();";

                        ClientScript.RegisterStartupScript(this.GetType(), "TerritoryStyle", js_script_style, true);

                        //запись лог файлов
                        /*SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "territory_style_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                        SW.Write(js_script_style);
                        SW.Close();*/
                    }
                }
                
                //проверяем куки
                if (Request.Browser.Cookies)
                {
                    HttpCookie map_user = Request.Cookies["Agrochim31_Map_User"];
                    String login_js_script = String.Empty;
                    if (map_user != null)
                    {
                        if (map_user["id_user"].ToString() != "0")
                        {
                            login_js_script = "$(function (){";
                            login_js_script += "\n$(\"#LoginW\").dialog(\"close\");";
                            //js_script += GetRegionsJS(map_user["id_user"].ToString(), Convert.ToInt32(map_user["type_user"].ToString()));
                            login_js_script += GetTerritoryJS(map_user["id_user"].ToString(), Convert.ToInt32(map_user["type_user"].ToString()));
                            login_js_script += "});";
                        }
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "CloseLoginW", login_js_script, true);

                    //SetSecurity(user_reg_data);

                    for_map = Request.Cookies["Agrochim31_For_Map"];
                    if (for_map != null && map_user != null)
                    {
                        id_organization = for_map["id_organization"].ToString();
                        year = for_map["year"].ToString();
                        tour = for_map["tour"].ToString();
                        code_plot = for_map["code_plot"].ToString();

                        if (connection_try)
                        {
                            conn.Open();
                            //querry_string = "exec [dbo].[GetPlotGeoJSON] " + id_organization + ", " + year;
                            querry_string = "exec [dbo].[GetPlotGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter plots_geo_data = new SqlDataAdapter(querry_string, conn);
                            plots_geo_data.Fill(layersDS, "Plots");

                            //String get_plot_tree_str = "SELECT TOP 1 * FROM View_Plots_Tree WHERE id_organization=" + id_organization + " AND [year]=" + year;
                            String get_plot_tree_str = "SELECT TOP 1 * FROM View_Plots_Tree WHERE id_organization=" + id_organization + " AND [tour]=" + tour;
                            SqlDataAdapter get_plot_tree = new SqlDataAdapter(get_plot_tree_str, conn);
                            get_plot_tree.Fill(layersDS, "CurrentData");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "CurrentData"))
                            {
                                String select_js_script = String.Empty;
                                select_js_script = "$(function (){";
                                select_js_script += "\n$(\"#TerritoryCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                select_js_script += ("\n$(\"#TerritoryCB option[value='" + layersDS.Tables["CurrentData"].Rows[0]["id_territory"].ToString() + "']\").prop('selected', true);");
                                select_js_script += GetRegionsJS(layersDS.Tables["CurrentData"].Rows[0]["id_territory"].ToString(), map_user["id_user"].ToString(), Convert.ToInt32(Convert.ToInt32(map_user["type_user"].ToString())));
                                select_js_script += "\n$(\"#RegionCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                select_js_script += ("\n$(\"#RegionCB option[value='" + layersDS.Tables["CurrentData"].Rows[0]["id_region"].ToString() + "']\").prop('selected', true);");
                                select_js_script += GetOrganizationsJS(layersDS.Tables["CurrentData"].Rows[0]["id_region"].ToString(), map_user["id_user"].ToString(), Convert.ToInt32(Convert.ToInt32(map_user["type_user"].ToString())));
                                select_js_script += "\n$(\"#OrganizationCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                select_js_script += ("\n$(\"#OrganizationCB option[value='" + layersDS.Tables["CurrentData"].Rows[0]["id_organization"].ToString() + "']\").prop('selected', true);");
                                select_js_script += "});";

                                ClientScript.RegisterStartupScript(this.GetType(), "SelectData", select_js_script, true);

                                conn.Open();
                                String get_regions_str = "exec [dbo].[GetRegionsGeoJSON] " + layersDS.Tables["CurrentData"].Rows[0]["id_territory"].ToString();
                                SqlDataAdapter get_regions = new SqlDataAdapter(get_regions_str, conn);
                                get_regions.Fill(layersDS, "Regions");

                                String get_organization_str = "exec [dbo].[GetOrganizationsGeoJSON] " + layersDS.Tables["CurrentData"].Rows[0]["id_region"].ToString();
                                SqlDataAdapter get_organization = new SqlDataAdapter(get_organization_str, conn);
                                get_organization.Fill(layersDS, "Organizations");
                                conn.Close();

                                String delete_js_script = String.Empty;
                                delete_js_script = "\nif(vector_regions != null) { map.removeLayer(vector_regions); }";
                                delete_js_script += "\nif(vector_organizations != null) { map.removeLayer(vector_organizations); }";
                                delete_js_script += "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";

                                ClientScript.RegisterStartupScript(this.GetType(), "DeleteLayers", delete_js_script, true);

                                String regions_feature_string = String.Empty, regions_feature_name = String.Empty, regions_properties_string = String.Empty;
                                if (CheckRowsCount(layersDS, "Regions"))
                                {
                                    String regions_js_script = String.Empty;
                                    regions_js_script = "\nregions_source = new ol.source.Vector();\nvar regions_feature;";
                                    for (int i = 0; i < layersDS.Tables["Regions"].Rows.Count; i++)
                                    {
                                        regions_feature_string = "\nregions_feature = format.readFeature('" + layersDS.Tables["Regions"].Rows[i]["region_geo_json"] + "');";
                                        regions_feature_name = "\nregions_feature.name = '" + layersDS.Tables["Regions"].Rows[i]["code_region"] + "';";
                                        for (int j = 0; j < layersDS.Tables["Regions"].Columns.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                regions_properties_string = "\nregions_feature.setProperties({'layer':'regions','id_feature':'" + i.ToString() + "'";
                                            }
                                            if (layersDS.Tables["Regions"].Columns[j].ColumnName.ToString() != "region_geo_json")
                                            {
                                                if (regions_properties_string[regions_properties_string.Length - 1] != '{' && regions_properties_string[regions_properties_string.Length - 1] != ',')
                                                {
                                                    regions_properties_string += ",";
                                                }
                                                regions_properties_string += ("'" + layersDS.Tables["Regions"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Regions"].Rows[i][j].ToString() + "'");
                                            }
                                            if (j == (layersDS.Tables["Regions"].Columns.Count - 1))
                                            {
                                                regions_properties_string += "});";
                                            }
                                        }
                                        regions_js_script += (regions_feature_string + regions_feature_name + regions_properties_string + "\nregions_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                        regions_js_script += "\nregions_source.addFeature(regions_feature);";
                                    }
                                    regions_js_script += "\nvar count_regions = regions_source.getFeatures().length; \nif(count_regions > 0){";
                                    regions_js_script += "\nvector_regions = new ol.layer.Vector({source: regions_source, minResolution:100, maxResolution:600});";
                                    regions_js_script += "\ncurrent_extent = regions_source.getExtent();";
                                    regions_js_script += "\nmap.addLayer(vector_regions); map.getView().fit(current_extent, map.getSize());";
                                    regions_js_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";

                                    ClientScript.RegisterStartupScript(this.GetType(), "RegionMap", regions_js_script, true);

                                    //задание стиля для районов
                                    String regions_js_script_style = String.Empty;
                                    regions_js_script_style = "\nvar regions_colors = {};";
                                    regions_js_script_style += "\nSetColors(regions_colors, " + layersDS.Tables["Regions"].Rows.Count + ", 0.2);";
                                    regions_js_script_style += ("\nfunction SetStyles_Regions" + ClientID + "(){");
                                    regions_js_script_style += "\nvar regions_styleCache = {};";
                                    regions_js_script_style += "\nvar regions_style = function (feature, resolution){";
                                    regions_js_script_style += ("\nvar text = feature.name;");
                                    regions_js_script_style += "\nif (!regions_styleCache[text]){";
                                    String fill_str, stroke_str, font_str;
                                    regions_js_script_style += "regions_styleCache[text] = [new ol.style.Style({";
                                    fill_str = "\nfill: new ol.style.Fill({";
                                    fill_str += "\ncolor: regions_colors[feature.getProperties().id_feature]})";
                                    stroke_str = "\nstroke: new ol.style.Stroke({";
                                    stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                                    stroke_str += "\nwidth: 1})";
                                    font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                    font_str += "\ntext: feature.getProperties().title_region,";
                                    font_str += "\nfill: new ol.style.Fill({";
                                    font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                    font_str += "\nstroke: new ol.style.Stroke({";
                                    font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                    font_str += "\nwidth: 2})})";

                                    regions_js_script_style += fill_str;
                                    regions_js_script_style += ("," + stroke_str);
                                    regions_js_script_style += ("," + font_str);
                                    regions_js_script_style += "})];";

                                    regions_js_script_style += "\n}\nreturn regions_styleCache[text];};";
                                    regions_js_script_style += "\nvector_regions.setStyle(regions_style);}";
                                    regions_js_script_style += "\nSetStyles_Regions" + ClientID + "();";

                                    ClientScript.RegisterStartupScript(this.GetType(), "RegionStyle", regions_js_script_style, true);
                                }

                                String organizations_feature_string = String.Empty, organizations_feature_name = String.Empty, organizations_properties_string = String.Empty;
                                if (CheckRowsCount(layersDS, "Organizations"))
                                {
                                    String organizations_js_script = String.Empty;
                                    organizations_js_script = "\norganizations_source = new ol.source.Vector();\nvar organizations_feature;";
                                    for (int i = 0; i < layersDS.Tables["Organizations"].Rows.Count; i++)
                                    {
                                        organizations_feature_string = "\norganizations_feature = format.readFeature('" + layersDS.Tables["Organizations"].Rows[i]["organization_geo_json"] + "');";
                                        organizations_feature_name = "\norganizations_feature.name = '" + layersDS.Tables["Organizations"].Rows[i]["code_organization"] + "';";
                                        for (int j = 0; j < layersDS.Tables["Organizations"].Columns.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                organizations_properties_string = "\norganizations_feature.setProperties({'layer':'organizations','id_feature':'" + i.ToString() + "'";
                                            }
                                            if (layersDS.Tables["Organizations"].Columns[j].ColumnName.ToString() != "organization_geo_json")
                                            {
                                                if (organizations_properties_string[organizations_properties_string.Length - 1] != '{' && organizations_properties_string[organizations_properties_string.Length - 1] != ',')
                                                {
                                                    organizations_properties_string += ",";
                                                }
                                                organizations_properties_string += ("'" + layersDS.Tables["Organizations"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Organizations"].Rows[i][j].ToString() + "'");
                                            }
                                            if (j == (layersDS.Tables["Organizations"].Columns.Count - 1))
                                            {
                                                organizations_properties_string += "});";
                                            }
                                        }
                                        organizations_js_script += (organizations_feature_string + organizations_feature_name + organizations_properties_string + "\norganizations_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                        organizations_js_script += "\norganizations_source.addFeature(organizations_feature);";
                                    }
                                    organizations_js_script += "\nvar count_organizations = organizations_source.getFeatures().length; \nif(count_organizations > 0){";
                                    organizations_js_script += "\nvector_organizations = new ol.layer.Vector({source: organizations_source, minResolution:50, maxResolution:300});";
                                    organizations_js_script += "\ncurrent_extent = organizations_source.getExtent();";
                                    organizations_js_script += "\nmap.addLayer(vector_organizations); map.getView().fit(current_extent, map.getSize());";
                                    organizations_js_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";

                                    ClientScript.RegisterStartupScript(this.GetType(), "OrganizationMap", organizations_js_script, true);

                                    //задание стиля для организаций
                                    String organizations_js_script_style = String.Empty;
                                    organizations_js_script_style = "\nvar organization_colors = {};";
                                    organizations_js_script_style += "\nSetColors(organization_colors, " + layersDS.Tables["Organizations"].Rows.Count + ", 0.2);";
                                    organizations_js_script_style += ("\nfunction " + "SetStyles_Organizations" + ClientID + "(){");
                                    organizations_js_script_style += "\nvar organization_styleCache = {};";
                                    organizations_js_script_style += "\nvar organization_style = function (feature, resolution){";
                                    organizations_js_script_style += ("\nvar text = feature.name;");
                                    organizations_js_script_style += "\nif(!organization_styleCache[text]){";
                                    String fill_str, stroke_str, font_str;
                                    organizations_js_script_style += "organization_styleCache[text] = [new ol.style.Style({";
                                    fill_str = "\nfill: new ol.style.Fill({";
                                    fill_str += "\ncolor: organization_colors[feature.getProperties().id_feature]})";
                                    stroke_str = "\nstroke: new ol.style.Stroke({";
                                    stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                                    stroke_str += "\nwidth: 1})";
                                    font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                    font_str += "\ntext: feature.getProperties().title_organization,";
                                    font_str += "\nfill: new ol.style.Fill({";
                                    font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                    font_str += "\nstroke: new ol.style.Stroke({";
                                    font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                    font_str += "\nwidth: 2})})";

                                    organizations_js_script_style += fill_str;
                                    organizations_js_script_style += ("," + stroke_str);
                                    organizations_js_script_style += ("," + font_str);
                                    organizations_js_script_style += "})];";

                                    organizations_js_script_style += "\n}\nreturn organization_styleCache[text];};";
                                    organizations_js_script_style += "\nvector_organizations.setStyle(organization_style);}";
                                    organizations_js_script_style += "\nSetStyles_Organizations" + ClientID + "();";

                                    ClientScript.RegisterStartupScript(this.GetType(), "OrganizationStyle", organizations_js_script_style, true);
                                }
                            }

                            String feature_string = String.Empty,
                                   feature_name = String.Empty,
                                   properties_string = String.Empty;

                            if (CheckRowsCount(layersDS, "Plots"))
                            {
                                String plots_js_script = String.Empty;
                                plots_js_script = "\nplots_source = new ol.source.Vector(); \nvar feature;";
                                for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                {
                                    feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                    feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                    for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                        {
                                            if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                            {
                                                properties_string += ",";
                                            }
                                            properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                        {
                                            properties_string += "});";
                                        }
                                    }
                                    plots_js_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                    plots_js_script += "\nplots_source.addFeature(feature);";
                                }
                                plots_js_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                plots_js_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:100});";

                                plots_js_script += "\ncurrent_extent = plots_source.getExtent();";
                                plots_js_script += "\nmap.addLayer(vector_plots); vector_plots.setZIndex(1); map.getView().fit(current_extent, map.getSize());";
                                plots_js_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                //js_script += "\nvector_plots.on('select', function (){if(selectClick.getFeatures().getLength() > 0){var code_plot = selectClick.getFeatures()[0].getProperties().code_plot; CallServer(('code_plot:' + code_plot + '|' + $('#Year3CB').val()), 'null');}});";

                                /*plots_js_script += ("\nif (selectClick != null){map.on('singleclick', function (e) {" +
                                             "map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {" +
                                             "if (feature.getGeometry().getType() == 'Point'){" +
                                             "CallServer(('soil_point:' + feature.name), 'null');}" +
                                             "else if (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon'){" +
                                             "CallServer(('code_plot:' + feature.name + '|' + $('#Year3CB').val()), 'null');}});});}");*/
                                plots_js_script += "\nCallServer('get_tours_years_by_filter:id_organization=" + id_organization + "', 'null');";

                                ClientScript.RegisterStartupScript(this.GetType(), "PlotsMap", plots_js_script, true);
                            }
                        }
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "AddPlots", "alert('Включите поддержку cookies!!!');", true);
                }

                if (connection_try)
                {
                    //задание стиля для всех с/х угодий и контура
                    conn.Open();
                    DataSet stylesDS = new DataSet();
                    String get_styles_str = "SELECT * FROM ViewDefaultStyle";
                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                    get_styles.Fill(stylesDS, "DefaultStyles");

                    String get_plot_dep_styles_str = "SELECT * FROM ViewPlotDepStyle";
                    SqlDataAdapter get_plot_dep_styles = new SqlDataAdapter(get_plot_dep_styles_str, conn);
                    get_plot_dep_styles.Fill(stylesDS, "PlotDepStyles");
                    conn.Close();

                    //все с/х угодия
                    String fill_str, stroke_str, font_str;
                    String plots_js_script_style = String.Empty;

                    if (CheckRowsCount(stylesDS, "PlotDepStyles"))
                    {
                        plots_js_script_style += ("\nfunction SetStyles_Arable" + ClientID + "(){");
                        plots_js_script_style += "\nvar plots_styleCache = {};";
                        plots_js_script_style += "\nvar plots_style = function (feature, resolution){";
                        plots_js_script_style += ("\nvar plot_text = feature.getProperties().number_plot;");
                        if (stylesDS.Tables["PlotDepStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                        {
                            /*plots_js_script_style += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null && " +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                        stylesDS.Tables["PlotDepStyles"].Rows[0]["maxresolution"].ToString() +
                                        ") { t = ''; }\nreturn t;};");*/
                            /*plots_js_script_style += ("\nvar getText = function (feature, resolution) {\nvar t = document.createElement('span'); \nif(feature.getProperties().unique_id_plot != null && " +
                                          "feature.getProperties().unique_id_plot != '')\n{t.innerHTML = feature.getProperties().unique_id_plot + ' <sup>' + feature.getProperties().number_plot + '</sup>';}" +
                                          "else {t.innerHTML = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    stylesDS.Tables["PlotDepStyles"].Rows[0]["maxresolution"].ToString() +
                                                ") { t.innerHTML = ''; }\n" +
                                                "for (var i = 0; i < t.children.length; i++){var child = t.children[i]; if (child.tagName === 'SUP'){\n" +
                                                "if (child.textContent <= 3 && child.textContent > 1){child.textContent = String.fromCharCode(child.textContent.charCodeAt(0) + 128);}" +
                                                "else if (child.textContent > 3 && child.textContent <= 9){child.textContent = String.fromCharCode(child.textContent.charCodeAt(0) + 8256);}}}" +
                            "\nreturn t.textContent;};");*/
                            plots_js_script_style += ("\nvar getText = function (feature, resolution) {var t;" +
                                                      "\nif (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '' && feature.getProperties().id_type_label_text == 1)" +
                                                      "\n{ t = feature.getProperties().unique_id_plot; }" +
                                                      "\nelse if (feature.getProperties().number_plot != null && feature.getProperties().number_plot != '' && feature.getProperties().id_type_label_text == 2)" +
                                                      "\n{ t = feature.getProperties().number_plot; }" +
                                                      "\nelse if (feature.getProperties().area != null && feature.getProperties().area != '' && feature.getProperties().id_type_label_text == 3)" +
                                                      "\n{ t = feature.getProperties().area; }" +
                                                      "\nelse if (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '')" +
                                                      "\n{ t = feature.getProperties().unique_id_plot; }" +
                                                      "\nelse { t = feature.getProperties().number_plot; };" +
                                                      "\nif (resolution > " + stylesDS.Tables["PlotDepStyles"].Rows[0]["maxresolution"].ToString() + ") { t = ''; } \nreturn t;};");
                        }
                        plots_js_script_style += "\nif (!plots_styleCache[plot_text]){";
                        plots_js_script_style += ("\nswitch (feature.getProperties().id_department){");

                        for (int i = 0; i < stylesDS.Tables["PlotDepStyles"].Rows.Count; i++)
                        {
                            plots_js_script_style += ("\ncase '" + stylesDS.Tables["PlotDepStyles"].Rows[i]["id_department"].ToString() + "':{plots_styleCache[plot_text] = [new ol.style.Style({");
                            fill_str = String.Empty;
                            if (stylesDS.Tables["PlotDepStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["PlotDepStyles"].Rows[i]["red"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["green"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["PlotDepStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_width"].ToString());
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["PlotDepStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["PlotDepStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["PlotDepStyles"].Rows[i]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["PlotDepStyles"].Rows[i]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["PlotDepStyles"].Rows[i]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["PlotDepStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["PlotDepStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                plots_js_script_style += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += font_str;
                            }
                            plots_js_script_style += "})];\nbreak;}";
                        }

                        if (CheckRowsCount(stylesDS, "DefaultStyles"))
                        {
                            plots_js_script_style += "\ndefault:{\nplots_styleCache[plot_text] = [new ol.style.Style({";
                            fill_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["DefaultStyles"].Rows[0]["size_font"].ToString() + "px " + stylesDS.Tables["DefaultStyles"].Rows[0]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["DefaultStyles"].Rows[0]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["DefaultStyles"].Rows[0]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                plots_js_script_style += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += font_str;
                            }
                            //добавление отображение точки
                            plots_js_script_style += (",\nimage: new ol.style.Circle({ radius: 3, fill: new ol.style.Fill({ color: 'rgba(" +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_opacity"].ToString().Replace(',', '.') + ")' })})");

                            plots_js_script_style += "})];\nbreak;}";
                        }

                        plots_js_script_style += "}\n}\nreturn plots_styleCache[plot_text];};";
                        plots_js_script_style += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                        plots_js_script_style += ("\nSetStyles_Arable" + ClientID + "();");
                        plots_js_script_style += ("\nmap.on('moveend', function () {\nSetStyles_Arable" + ClientID + "();});");
                    }
                    else
                    {
                        if (CheckRowsCount(stylesDS, "DefaultStyles"))
                        {
                            plots_js_script_style = "\nfunction SetStyles_Arable" + ClientID + "(){";
                            plots_js_script_style += "\nvar plots_styleCache = {};";
                            plots_js_script_style += "\nvar plots_style = function (feature, resolution){";
                            plots_js_script_style += "\nvar plot_text = feature.name;";
                            plots_js_script_style += "\nif (!plots_styleCache[plot_text]){";
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                            {
                                plots_js_script_style += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                          "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                          "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                            stylesDS.Tables["DefaultStyles"].Rows[0]["maxresolution"].ToString() +
                                            ") { t = ''; }\nreturn t;};");
                            }
                            plots_js_script_style += "\nplots_styleCache[plot_text] = [new ol.style.Style({";
                            fill_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["DefaultStyles"].Rows[0]["size_font"].ToString() + "px " + stylesDS.Tables["DefaultStyles"].Rows[0]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["DefaultStyles"].Rows[0]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["DefaultStyles"].Rows[0]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[0]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                plots_js_script_style += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += font_str;
                            }
                            //добавление отображение точки
                            plots_js_script_style += (",\nimage: new ol.style.Circle({ radius: 3, fill: new ol.style.Fill({ color: 'rgba(" +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[0]["stroke_opacity"].ToString().Replace(',', '.') + ")' })})");

                            plots_js_script_style += "})];";

                            plots_js_script_style += "\n}\nreturn plots_styleCache[plot_text];};";
                            plots_js_script_style += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style); vector_plots.setVisible(true);}";
                            plots_js_script_style += ("\nSetStyles_Arable" + ClientID + "();");
                            plots_js_script_style += ("\nmap.on('moveend', function () {\nSetStyles_Arable" + ClientID + "();});");
                        }
                    }

                    //контур
                    if (CheckRowsCount(stylesDS, "DefaultStyles"))
                    {
                        if (stylesDS.Tables["DefaultStyles"].Rows.Count > 1)
                        {
                            plots_js_script_style += "\nfunction SetStyles_Outline" + ClientID + "(){";
                            plots_js_script_style += "\nvar plots_styleCache = {};";
                            plots_js_script_style += "\nvar plots_style = function (feature, resolution){";
                            plots_js_script_style += "\nvar text = feature.name;";
                            plots_js_script_style += "\nif (!plots_styleCache[text]){";
                            if (stylesDS.Tables["DefaultStyles"].Rows[1]["id_text_style"].ToString() != String.Empty)
                            {
                                plots_js_script_style += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                          "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                          "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                            stylesDS.Tables["DefaultStyles"].Rows[1]["maxresolution"].ToString() +
                                            ") { t = ''; }\nreturn t;};");
                            }
                            plots_js_script_style += "\nplots_styleCache[text] = [new ol.style.Style({";
                            fill_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[1]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[1]["red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[1]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["DefaultStyles"].Rows[1]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["DefaultStyles"].Rows[1]["size_font"].ToString() + "px " + stylesDS.Tables["DefaultStyles"].Rows[1]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["DefaultStyles"].Rows[1]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["DefaultStyles"].Rows[1]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[1]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["DefaultStyles"].Rows[1]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                plots_js_script_style += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    plots_js_script_style += ",";
                                }
                                plots_js_script_style += font_str;
                            }
                            plots_js_script_style += "})];";

                            plots_js_script_style += "\n}\nreturn plots_styleCache[text];};";
                            plots_js_script_style += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style); vector_plots.setVisible(true);}";
                        }

                        //запись лога в файл
                        /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "plot_styles_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                        SW.Write(plots_js_script_style);
                        SW.Close();*/

                        ClientScript.RegisterStartupScript(this.GetType(), "PlotsStyle", plots_js_script_style, true);
                    }
                }


                List<object> Themes = new List<object>();
                Themes.Add(new { name_theme = "null", title_theme = "Сельскохозяйственные угодия" });
                Themes.Add(new { name_theme = "p2o5", title_theme = "Подвижный фосфор" });
                Themes.Add(new { name_theme = "k2o", title_theme = "Подвижный калий" });
                Themes.Add(new { name_theme = "ph_s", title_theme = "Степень кислотности" });
                Themes.Add(new { name_theme = "humus", title_theme = "Органическое вещество" });
                Themes.Add(new { name_theme = "soil", title_theme = "Почвенная карта" });
                Themes.Add(new { name_theme = "erosion_soil", title_theme = "Карта эрозии" });
                Themes.Add(new { name_theme = "slope", title_theme = "Карта крутизны склонов" });
                Themes.Add(new { name_theme = "exposure", title_theme = "Карта экспозиции" });
                Themes.Add(new { name_theme = "typing", title_theme = "Карта типизации" });
                Themes.Add(new { name_theme = "project", title_theme = "Карта АЛСЗ" });
                Themes.Add(new { name_theme = "farmland", title_theme = "Тип сельскохозяйственных угодий" });
                Themes.Add(new { name_theme = "watering", title_theme = "Орошение" });
                Themes.Add(new { name_theme = "using_plot", title_theme = "Использование" });
                Themes.Add(new { name_theme = "report", title_theme = "По отчёту" });

                MapThemeCB.DataSource = Themes;
                MapThemeCB.DataBind();

                List<object> Type_plots = new List<object>();
                Type_plots.Add(new { id_type_plot = "0", title_type_plot = "Все с/х угодия" });
                Type_plots.Add(new { id_type_plot = "1", title_type_plot = "Только пашня" });
                Type_plots.Add(new { id_type_plot = "2", title_type_plot = "Многолетние насаждения" });

                TypePlotCB.DataSource = Type_plots;
                TypePlotCB.DataBind();

                /*AcceptLoginB.Listeners.Click.Handler = "App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());";
                UsernameTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
                UserPassword.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
                ToolBarW.Listeners.AfterRender.Handler = "App.direct.LoadComboBoxData();";
                CenterMapB.Listeners.Click.Handler = "if(map != null && extent_plots != null && count_plots > 0){map.getView().fit(extent_plots, map.getSize());};";
                ShowPlotInfoB.Listeners.Click.Handler = "var plots_styleCache = {};var plots_style = function (feature){var text = feature.name; if (!plots_styleCache[text]){if (feature.getProperties().number_plot < 10) {" +
                                                        "plots_styleCache[text] = [new ol.style.Style({fill: new ol.style.Fill({color: 'rgba(0, 255, 100, 0.8)'}),stroke: new ol.style.Stroke({color: 'rgba(255, 0, 0, 1)',width: '5'})})];}" +
                                                        "else{plots_styleCache[text] = [new ol.style.Style({fill: new ol.style.Fill({color: 'rgba(200, 100, 100, 0.8)'}),stroke: new ol.style.Stroke({color: 'rgba(0, 255, 0, 1)',width: '1'})})];}}" +
                                                        "return plots_styleCache[text];};vector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);";*/

                if (connection_try)
                {
                    //загрузка справочников для книги историии полей
                    conn.Open();
                    DataSet tablesDS = new DataSet();

                    String get_sort_crop_rotation_str = "SELECT * FROM Sort_crop_rotation";
                    String get_culture_str = "SELECT * FROM Culture ORDER BY title_culture";
                    String get_cross_culture_str = "SELECT * FROM Cross_culture ORDER BY title_cross_culture";
                    String get_mineral_fertilizer_str = "SELECT * FROM Fertilizer WHERE id_kind_fertilizer = 1 ORDER BY title_fertilizer";
                    String get_organic_fertilizer_str = "SELECT * FROM Fertilizer WHERE id_kind_fertilizer = 2 ORDER BY title_fertilizer";
                    String get_tillage_str = "SELECT * FROM Type_tillage ORDER BY title_type_tillage";
                    String get_type_drug_str = "SELECT * FROM Type_drug ORDER BY title_type_drug";
                    String get_drugs_str = "SELECT * FROM Drugs WHERE id_type_drug = 1 ORDER BY title_drug";
                    String get_ameliorator_str = "SELECT * FROM Ameliorators ORDER BY title_ameliorator";
                    String get_type_property_str = "SELECT * FROM Type_property ORDER BY title_type_property";

                    SqlDataAdapter get_sort_crop_rotation = new SqlDataAdapter(get_sort_crop_rotation_str, conn);
                    SqlDataAdapter get_culture = new SqlDataAdapter(get_culture_str, conn);
                    SqlDataAdapter get_cross_culture = new SqlDataAdapter(get_cross_culture_str, conn);
                    SqlDataAdapter get_mineral_fertilizer = new SqlDataAdapter(get_mineral_fertilizer_str, conn);
                    SqlDataAdapter get_organic_fertilizer = new SqlDataAdapter(get_organic_fertilizer_str, conn);
                    SqlDataAdapter get_tillage = new SqlDataAdapter(get_tillage_str, conn);
                    SqlDataAdapter get_type_drug = new SqlDataAdapter(get_type_drug_str, conn);
                    SqlDataAdapter get_drugs = new SqlDataAdapter(get_drugs_str, conn);
                    SqlDataAdapter get_ameliorators = new SqlDataAdapter(get_ameliorator_str, conn);
                    SqlDataAdapter get_type_property = new SqlDataAdapter(get_type_property_str, conn);

                    get_sort_crop_rotation.Fill(tablesDS, "Sort_crop_rotation");
                    get_culture.Fill(tablesDS, "Culture");
                    get_cross_culture.Fill(tablesDS, "Cross_culture");
                    get_mineral_fertilizer.Fill(tablesDS, "MineralFertilizer");
                    get_organic_fertilizer.Fill(tablesDS, "OrganicFertilizer");
                    get_tillage.Fill(tablesDS, "Type_tillage");
                    get_type_drug.Fill(tablesDS, "Type_drug");
                    get_drugs.Fill(tablesDS, "Drugs");
                    get_ameliorators.Fill(tablesDS, "Ameliorators");
                    get_type_property.Fill(tablesDS, "Type_property");
                    conn.Close();

                    if (CheckRowsCount(tablesDS, "Sort_crop_rotation"))
                    {
                        SortCRCB.DataSource = tablesDS.Tables["Sort_crop_rotation"];
                        SortCRCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Culture"))
                    {
                        CultureCB.DataSource = tablesDS.Tables["Culture"];
                        OldCultureCB.DataSource = tablesDS.Tables["Culture"];
                        CultureCB.DataBind();
                        OldCultureCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Cross_culture"))
                    {
                        CrossCultureCB.DataSource = tablesDS.Tables["Cross_culture"];
                        CrossCultureCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "MineralFertilizer"))
                    {
                        BasicFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        SowingFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        DressingFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        OldBasicFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        OldSowingFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        OldDressingFertCB.DataSource = tablesDS.Tables["MineralFertilizer"];
                        BasicFertCB.DataBind();
                        SowingFertCB.DataBind();
                        DressingFertCB.DataBind();
                        OldBasicFertCB.DataBind();
                        OldSowingFertCB.DataBind();
                        OldDressingFertCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "OrganicFertilizer"))
                    {
                        OrganicFertCB.DataSource = tablesDS.Tables["OrganicFertilizer"];
                        OldOrganicFertCB.DataSource = tablesDS.Tables["OrganicFertilizer"];
                        OrganicFertCB.DataBind();
                        OldOrganicFertCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Type_tillage"))
                    {
                        TillageCB.DataSource = tablesDS.Tables["Type_tillage"];
                        TillageCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Type_drug"))
                    {
                        TypeDrugCB.DataSource = tablesDS.Tables["Type_drug"];
                        TypeDrugCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Drugs"))
                    {
                        DrugCB.DataSource = tablesDS.Tables["Drugs"];
                        DrugCB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Ameliorators"))
                    {
                        Ameliorator1CB.DataSource = tablesDS.Tables["Ameliorators"];
                        Ameliorator1CB.DataBind();
                        Ameliorator2CB.DataSource = tablesDS.Tables["Ameliorators"];
                        Ameliorator2CB.DataBind();
                    }
                    if (CheckRowsCount(tablesDS, "Type_property"))
                    {
                        TypePropertyCB.DataSource = tablesDS.Tables["Type_property"];
                        TypePropertyCB.DataBind();
                    }
                }

                /*MapThemeCB.Attributes["onChange"] = "if($(\"#OrganizationCB\").val() != 0){CallServer(this.value, 'null');}" +
                                                    "else if($(\"#RegionCB\").val() == 0 && $(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + this.value, 'null');}" +
                                                    "else {CallServer('territory_theme:' + this.value, 'null');}";*/
                MapThemeCB.Attributes["onChange"] = "\nCallServer('legend:' + this.value, 'null');" +
                                                    "\nif($(\"#OrganizationCB\").val() != 0){CallServer(this.value, 'null');}" +
                                                    "\nif($('#OrganizationCB').val() == 0 && $('#RegionCB').val() != 0)" +
                                                    "{CallServer('get_region_plots:' + $('#RegionCB').val() + '|' + this.value, null);}" +
                                                    "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + this.value, 'null');}" +
                                                    "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + this.value, 'null');}";

                TypeDrugCB.Attributes["onChange"] = "CallServer('change_type_drugs:' + this.value, 'null');";
                CultureCB.Attributes["onChange"] = "CallServer('change_culture:' + this.value + '|' + $(\"#UniqNumberTB\").val(), 'null');";
                OldCultureCB.Attributes["onChange"] = "CallServer('change_old_culture:' + this.value, 'null');";
                Year3CB.Attributes["onChange"] = "$(\"#Year4CB option:selected\").each(function(){$(this).prop('selected', false);});" +
                                                 " $(\"#Year4CB option[value=\"+this.value+\"]\").prop('selected', true);" +
                                                 " CallServer('change_crop_rotation_year:' + this.value +'|'+ $(\"#UniqNumberTB\").val(), 'null');";
                Year4CB.Attributes["onChange"] = "$(\"#Year3CB option:selected\").each(function(){$(this).prop('selected', false);});" +
                                                 " $(\"#Year3CB option[value=\"+this.value+\"]\").prop('selected', true);" +
                                                 " CallServer('change_crop_rotation_year:' + this.value +'|'+ $(\"#UniqNumberTB\").val(), 'null');";
                TerritoryCB.Attributes["onChange"] = "var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                  "CallServer('select_territory:' + this.value +'|'+ values, 'null');";
                RegionCB.Attributes["onChange"] = "var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                  "CallServer('select_region:' + this.value +'|'+ values, 'null');";
                OrganizationCB.Attributes["onChange"] = "var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                        "CallServer('select_organization:' + this.value +'|'+ values , 'null');";
                SurveyTourCB.Attributes["onChange"] = "if($(\"#OrganizationCB\").val() != 0){var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                        "CallServer('get_plots_by_filter:' + CreateFilter() +'|'+ $(\"#OrganizationCB\").val() +'|'+ values , 'null');}";
                SurveyYearCB.Attributes["onChange"] = "if($(\"#OrganizationCB\").val() != 0){var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                        "CallServer('get_plots_by_filter:' + CreateFilter() +'|'+ $(\"#OrganizationCB\").val() +'|'+ values , 'null');}";
                TypePlotCB.Attributes["onChange"] = "if($(\"#OrganizationCB\").val() != 0){var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                                        "CallServer('get_plots_by_filter:' + CreateFilter() +'|'+ $(\"#OrganizationCB\").val() +'|'+ values , 'null');}";

                PercentCaCO3_1TB.Attributes["onKeyUp"] = "$(\"#FactCaCO3TB\").val(GetFactCaCO3());";
                DoseAmeliorator1TB.Attributes["onKeyUp"] = "$(\"#FactCaCO3TB\").val(GetFactCaCO3());";
                PercentCaCO3_2TB.Attributes["onKeyUp"] = "$(\"#FactCaCO3TB\").val(GetFactCaCO3());";
                DoseAmeliorator2TB.Attributes["onKeyUp"] = "$(\"#FactCaCO3TB\").val(GetFactCaCO3());";

                SortCRCB.Attributes["onChange"] = "CallServer('change_sort_crop_rotation:' + this.value, 'null');";

                //пересчёт мин. удобрений в д.в.
                String total_dose = "CallServer('total_dose:' + $(\"#BasicFertCB\").val() + '|' + $(\"#DoseBasicFertTB\").val() + '|' + $(\"#SowingFertCB\").val()" +
                              " + '|' + $(\"#DoseSowingFertTB\").val() + '|' + $(\"#DressingFertCB\").val() + '|' + $(\"#DoseDressingFertTB\").val(), 'null');";
                String old_total_dose = "CallServer('old_total_dose:' + $(\"#OldBasicFertCB\").val() + '|' + $(\"#OldDoseBasicFertTB\").val() + '|' + $(\"#OldSowingFertCB\").val()" +
                              " + '|' + $(\"#OldDoseSowingFertTB\").val() + '|' + $(\"#OldDressingFertCB\").val() + '|' + $(\"#OldDoseDressingFertTB\").val(), 'null');";

                BasicFertCB.Attributes["onChange"] = total_dose;
                DoseBasicFertTB.Attributes["onKeyUp"] = total_dose;
                SowingFertCB.Attributes["onChange"] = total_dose;
                DoseSowingFertTB.Attributes["onKeyUp"] = total_dose;
                DressingFertCB.Attributes["onChange"] = total_dose;
                DoseDressingFertTB.Attributes["onKeyUp"] = total_dose;

                OldBasicFertCB.Attributes["onChange"] = old_total_dose;
                OldDoseBasicFertTB.Attributes["onKeyUp"] = old_total_dose;
                OldSowingFertCB.Attributes["onChange"] = old_total_dose;
                OldDoseSowingFertTB.Attributes["onKeyUp"] = old_total_dose;
                OldDressingFertCB.Attributes["onChange"] = old_total_dose;
                OldDoseDressingFertTB.Attributes["onKeyUp"] = old_total_dose;

                //пересчёт орг. удобрений в д.в.
                String org_total_dose = "CallServer('org_total_dose:' + $(\"#OrganicFertCB\").val() + '|' + $(\"#DoseOrganicFertTB\").val() + '|' + $(\"#IdProtocolTB\").val(), 'null');";
                String old_org_total_dose = "CallServer('old_org_total_dose:' + $(\"#OldOrganicFertCB\").val() + '|' + $(\"#OldDoseOrganicFertTB\").val(), 'null');";

                OrganicFertCB.Attributes["onChange"] = org_total_dose;
                DoseOrganicFertTB.Attributes["onKeyUp"] = org_total_dose;

                OldOrganicFertCB.Attributes["onChange"] = old_org_total_dose;
                OldDoseOrganicFertTB.Attributes["onKeyUp"] = old_org_total_dose;

                UniqueNumberSearchB.Attributes["onClick"] = "CallServer('code_single_plot:' + $(\"#UniqueNumberSearchTB\").val() +'|' + $(\"#Year3CB\").val(), 'null');";

                PhasePestsCB.Attributes["onChange"] = "CallServer('change_phase_pest:' + this.value + '|' + $(\"#CultureCB\").val(), 'null');";
                PhaseDiseasesCB.Attributes["onChange"] = "CallServer('change_phase_disease:' + this.value + '|' + $(\"#CultureCB\").val(), 'null');";

                CountWeedinessTB.Attributes["onKeyUp"] = "CallServer('change_weediness:' + $(\"#CountWeedinessTB\").val() + '|' + $(\"#PercentWeedinessTB\").val(), 'null');";
                PercentWeedinessTB.Attributes["onKeyUp"] = "CallServer('change_weediness:' + $(\"#CountWeedinessTB\").val() + '|' + $(\"#PercentWeedinessTB\").val(), 'null');";
            }
        }
        
        public Boolean TryConnection(String connection_stirng)
        {
            SqlConnection connection = new SqlConnection(connection_stirng);
            Boolean rez = false;
            try
            {
                connection.Open();
                rez = true;
            }
            catch 
            {
                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка соединения с БД",
                    Message = exc.Message + "<br />" + connection.ConnectionString + "<br />Настройте параметры подключения к базе данных!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
                rez = false;
            }
            finally
            {
                connection.Dispose();
            }
            return rez;
        }

        public String SetConnectionString()
        {
            String connString = String.Empty;
            try
            {
                //Создаём переменную Xml
                XmlDocument connect_to_server = new XmlDocument();
                //Загружаем параметры с файла
                connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "connection.xml"));
                //Выбираем отдельные элементы и выводим их
                XmlNodeList connection = connect_to_server.GetElementsByTagName("item");
                //"Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N"
                connString = "Data Source=" + GetValueFromXml(connect_to_server, "item", "server", "value")
                                          + "; Initial Catalog=" + GetValueFromXml(connect_to_server, "item", "database", "value")
                                          + "; Persist Security Info=True; User ID=" + GetValueFromXml(connect_to_server, "item", "login", "value")
                                          + "; Password=" + GetValueFromXml(connect_to_server, "item", "password", "value")
                                          + "; Type System Version=SQL Server 2012;";
            }
            catch 
            {
            }
            return connString;
        }

        //выбор значения из xml файла
        public String GetValueFromXml(XmlDocument doc, String tag, String id_item, String attr_item)
        {
            String ret_value = String.Empty;
            XmlNodeList list = doc.GetElementsByTagName(tag);
            foreach (XmlNode x in list)
            {
                foreach (XmlAttribute y in x.Attributes)
                {
                    if (y.Name == "id" && y.Value == id_item)
                    {
                        foreach (XmlAttribute z in x.Attributes)
                        {
                            if (z.Name == "value")
                            {
                                ret_value = z.Value.ToString();
                            }
                        }
                    }
                }
            }
            if (ret_value == String.Empty)
            {
                ret_value = "0";
            }
            return ret_value;
        }

        //Создание MD5 хеша
        private String GetMd5Hash(String str)
        {
            String strHash = String.Empty;
            foreach (byte b in new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str)))
            {
                strHash += b.ToString("x2");
            }
            return strHash;
        }
        //Проверка MD5 хеша
        private Boolean VerifyMd5Hash(String str, String strHash)
        {
            String new_strHash = GetMd5Hash(str);
            //сравнение с учётом регистра
            StringComparer comparer = StringComparer.Ordinal;
            if (0 == comparer.Compare(new_strHash, strHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region ICallbackEventHandler

        private string eventArgument;

        public void RaiseCallbackEvent(string eventArg)
        {
            this.eventArgument = eventArg;
        }

        public string GetCallbackResult()
        {
            String j_script = String.Empty;

            if (connection_try)
            {
                switch (eventArgument.Split(':')[0])
                {
                    case "null":
                        {
                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Arable" + ClientID + "();";
                            j_script += "\nmap.on('moveend', function () {\nSetStyles_Arable" + ClientID + "();});";
                            //запись лог файлов
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "plots_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/
                            break;
                        }
                    case "soil":
                        {
                            DataSet layersDS = new DataSet();

                            conn.Open();
                            //String soil_querry_string = "exec [dbo].[GetSoilGeoJSON] " + id_organization + ", " + year;
                            String soil_querry_string = "exec [dbo].[GetSoilGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter soil_geo_data = new SqlDataAdapter(soil_querry_string, conn);
                            soil_geo_data.Fill(layersDS, "Soil");
                            conn.Close();

                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Outline" + ClientID + "();";
                            Int32 count_parts = 0;
                            if (CheckRowsCount(layersDS, "Soil"))
                            {
                                count_parts = Convert.ToInt32(Round(layersDS.Tables["Soil"].Rows.Count / 50, 0));
                                j_script += "\nsoil_source = new ol.source.Vector();";
                                j_script += "\nvector_soil = new ol.layer.Vector({source: soil_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_soil);";
                                for (int i = 1; i <= count_parts + 1; i++)
                                {
                                    j_script += "\nCallServer('soil_part:" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                }
                            }
                            j_script += "\nCallServer('soil_points', 'null');";
                            break;
                        }
                    case "soil_points":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();
                            //String soil_points_querry_string = "exec [dbo].[GetSoilPointsGeoJSON] " + id_organization + ", " + year;
                            String soil_points_querry_string = "exec [dbo].[GetSoilPointsGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter soil_points_geo_data = new SqlDataAdapter(soil_points_querry_string, conn);
                            soil_points_geo_data.Fill(layersDS, "SoilPoints");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "SoilPoints"))
                            {
                                //добавление почвенных точек
                                String soil_point_feature_string = String.Empty;
                                String soil_point_feature_name = String.Empty;
                                String soil_point_properties_string = String.Empty;
                                j_script += "\nvar soil_point_source = new ol.source.Vector();\nvar soil_point_feature;";
                                for (int i = 0; i < layersDS.Tables["SoilPoints"].Rows.Count; i++)
                                {
                                    soil_point_feature_string = "\nsoil_point_feature = format.readFeature('" + layersDS.Tables["SoilPoints"].Rows[i]["soil_point_geo_json"] + "');";
                                    soil_point_feature_name = "\nsoil_point_feature.name = '" + layersDS.Tables["SoilPoints"].Rows[i]["id_geographic_soil_point"] + "';";
                                    for (int j = 0; j < layersDS.Tables["SoilPoints"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            soil_point_properties_string = "\nsoil_point_feature.setProperties({'layer':'soil_points','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["SoilPoints"].Columns[j].ColumnName.ToString() != "soil_point_geo_json")
                                        {
                                            if (soil_point_properties_string[soil_point_properties_string.Length - 1] != '{' && soil_point_properties_string[soil_point_properties_string.Length - 1] != ',')
                                            {
                                                soil_point_properties_string += ",";
                                            }
                                            soil_point_properties_string += ("'" + layersDS.Tables["SoilPoints"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["SoilPoints"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["SoilPoints"].Columns.Count - 1))
                                        {
                                            soil_point_properties_string += "});";
                                        }
                                    }
                                    j_script += (soil_point_feature_string + soil_point_feature_name + soil_point_properties_string + "\nsoil_point_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nsoil_point_source.addFeature(soil_point_feature);";
                                }
                                j_script += "\nvar count_soil_point = soil_point_source.getFeatures().length; \nif(count_soil_point > 0){";
                                j_script += "\nvector_soil_points = new ol.layer.Vector({source: soil_point_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_soil_points);}; vector_soil_points.setZIndex(2);";
                            }

                            break;
                        }
                    case "soil_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();
                            //String soil_querry_string = "exec [dbo].[GetSoilGeoJSON] " + id_organization + ", " + year;
                            String soil_querry_string = "exec [dbo].[GetSoilGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter soil_geo_data = new SqlDataAdapter(soil_querry_string, conn);
                            soil_geo_data.Fill(layersDS, "Soil");

                            String get_soil_styles_str = "SELECT * FROM ViewSoilStyle";
                            SqlDataAdapter get_soil_styles = new SqlDataAdapter(get_soil_styles_str, conn);
                            get_soil_styles.Fill(stylesDS, "SoilStyles");

                            String get_soil_points_styles_str = "SELECT * FROM ViewSoilPointsStyle";
                            SqlDataAdapter get_soil_points_styles = new SqlDataAdapter(get_soil_points_styles_str, conn);
                            get_soil_points_styles.Fill(stylesDS, "SoilPointsStyles");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "Soil"))
                            {
                                //добавление почвенной карты
                                String soil_feature_string = String.Empty;
                                String soil_feature_name = String.Empty;
                                String soil_properties_string = String.Empty;
                                j_script += "\nsoil_source = vector_soil.getSource();\nvar soil_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) * 50;
                                if (to_i > layersDS.Tables["Soil"].Rows.Count) { to_i = layersDS.Tables["Soil"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    soil_feature_string = "\nsoil_feature = format.readFeature('" + layersDS.Tables["Soil"].Rows[i]["soil_geo_json"] + "');";
                                    soil_feature_name = "\nsoil_feature.name = '" + layersDS.Tables["Soil"].Rows[i]["id_geographic_soil"] + "';";
                                    for (int j = 0; j < layersDS.Tables["Soil"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            soil_properties_string = "\nsoil_feature.setProperties({'layer':'soil','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Soil"].Columns[j].ColumnName.ToString() != "soil_geo_json")
                                        {
                                            if (soil_properties_string[soil_properties_string.Length - 1] != '{' && soil_properties_string[soil_properties_string.Length - 1] != ',')
                                            {
                                                soil_properties_string += ",";
                                            }
                                            soil_properties_string += ("'" + layersDS.Tables["Soil"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Soil"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Soil"].Columns.Count - 1))
                                        {
                                            soil_properties_string += "});";
                                        }
                                    }
                                    j_script += (soil_feature_string + soil_feature_name + soil_properties_string + "\nsoil_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nsoil_source.addFeature(soil_feature);";
                                }

                                if (CheckRowsCount(stylesDS, "SoilStyles"))
                                {
                                    //приеменение стилей для почвенной карты
                                    j_script += "\nfunction " + "SetStyles_soil" + ClientID + "(){";
                                    j_script += "\nvar soil_styleCache = {};";
                                    j_script += "\nvar soil_style = function (feature, resolution){";
                                    j_script += "\nvar soil_text = feature.name;";
                                    j_script += "\nif (!soil_styleCache[soil_text]){";
                                    j_script += ("\nswitch (feature.getProperties().code_type_soil){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["SoilStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["SoilStyles"].Rows[i]["code_type_soil"].ToString() + "':{");
                                        if (stylesDS.Tables["SoilStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().coding_soil;\nif (resolution > " +
                                                        stylesDS.Tables["SoilStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nsoil_styleCache[soil_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["SoilStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["SoilStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["SoilStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["SoilStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["SoilStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["SoilStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["SoilStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["SoilStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["SoilStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn soil_styleCache[soil_text];};";
                                    j_script += "\nvector_soil.setStyle(soil_style);}";
                                    j_script += ("\nSetStyles_soil" + ClientID + "();");
                                }
                            }

                            if (CheckRowsCount(stylesDS, "SoilPointsStyles"))
                            {
                                //приеменение стилей для почвенных точек
                                j_script += ("\nfunction " + "SetStyles_soil_points" + ClientID + "(){");
                                j_script += "\nvar soil_points_styleCache = {};";
                                j_script += "\nvar soil_points_style = function (feature, resolution){";
                                j_script += "\nvar soil_point_text = feature.name;";
                                j_script += "\nif (!soil_points_styleCache[soil_point_text]){";
                                j_script += ("\nswitch (feature.getProperties().code_type_point){");
                                String image_points_str, font_points_str;
                                for (int i = 0; i < stylesDS.Tables["SoilPointsStyles"].Rows.Count; i++)
                                {
                                    j_script += ("\ncase '" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["code_type_point"].ToString() + "':{");
                                    if (stylesDS.Tables["SoilStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().name_point;\nif (resolution > " +
                                                    stylesDS.Tables["SoilPointsStyles"].Rows[i]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    image_points_str = String.Empty;
                                    if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["image_src"].ToString() != String.Empty)
                                    {
                                        image_points_str += ("\nsoil_points_styleCache[soil_point_text] = [new ol.style.Style({");
                                        image_points_str += "\nimage: new ol.style.Icon({";
                                        image_points_str += ("\nsrc: '" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["image_src"].ToString() + "', " +
                                                                      "anchor: [0.5, 0.5], " +
                                                                      "anchorXUnits: 'fraction', " +
                                                                      "anchorYUnits: 'fraction'})");
                                    }
                                    else
                                    {
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["canvas"].ToString() != String.Empty && stylesDS.Tables["SoilPointsStyles"].Rows[i]["canvas"].ToString().Split(';').Length > 3)
                                        {
                                            //создаём полигон
                                            String[] poligon_points = stylesDS.Tables["SoilPointsStyles"].Rows[i]["canvas"].ToString().Split(';');
                                            String poligon = "[[";
                                            for (int j = 0; j < poligon_points.Length - 1; j++)
                                            {
                                                poligon += ("[" + poligon_points[j] + "],");
                                            }
                                            poligon += ("[" + poligon_points[poligon_points.Length - 1] + "]]]");
                                            image_points_str += ("\nvar canvas = (document.createElement('canvas')); var render = ol.render.toContext((canvas.getContext('2d')),{size: [14, 14], pixelRatio: 1});");
                                            //было setFillStrokeStyle - не работало с файлом ol.js, а только с debug
                                            image_points_str += "\nrender.setStyle(new ol.style.Style({ fill: new ol.style.Fill({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            image_points_str += "\nstroke: new ol.style.Stroke({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            image_points_str += ("\nwidth: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                image_points_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            //render.drawPolygonGeometry заменено на render.drawGeometry
                                            image_points_str += "\n}) }));\nrender.drawGeometry(new ol.geom.Polygon(" + poligon + "));";
                                            image_points_str += ("\nsoil_points_styleCache[soil_point_text] = [new ol.style.Style({");
                                            image_points_str += "\nimage: new ol.style.Icon({img: canvas, imgSize: [canvas.width, canvas.height]})";
                                        }
                                        else
                                        {
                                            //делаем окружность
                                            image_points_str += "\nsoil_points_styleCache[soil_point_text] = [new ol.style.Style({image: new ol.style.Circle({fill: new ol.style.Fill({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            image_points_str += "\nstroke: new ol.style.Stroke({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            image_points_str += ("\nwidth: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                image_points_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            image_points_str += ("}),\nradius: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["canvas"].ToString().Split(';')[0].Split(',')[0] + "})");
                                        }
                                    }
                                    font_points_str = String.Empty;
                                    if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                    {

                                        font_points_str += "\ntext: new ol.style.Text({font: '";
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                        {
                                            font_points_str += "bold ";
                                        }
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                        {
                                            font_points_str += "italic ";
                                        }
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                        {
                                            font_points_str += "underline ";
                                        }
                                        font_points_str += (stylesDS.Tables["SoilPointsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_name"].ToString() + "',");
                                        font_points_str += "\ntext: getText(feature, resolution),";
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                        {
                                            font_points_str += ("\noffsetX: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                        }
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                        {
                                            font_points_str += ("\noffsetY: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                        }
                                        font_points_str += "\nfill: new ol.style.Fill({";
                                        font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                        font_points_str += "\nstroke: new ol.style.Stroke({";
                                        font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        font_points_str += ("\nwidth: " + stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_width"].ToString());
                                        if (stylesDS.Tables["SoilPointsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                        {
                                            font_points_str += ",\nlineDash: [0.5, 4]";
                                        }
                                        font_points_str += "\n})})";
                                    }

                                    if (image_points_str != String.Empty)
                                    {
                                        j_script += image_points_str;
                                        if (font_points_str != String.Empty)
                                        {
                                            if (image_points_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_points_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    else
                                    {
                                        j_script += "\nsoil_points_styleCache[soil_point_text] = null;\nbreak;}";
                                    }

                                }
                                j_script += "}\n}\nreturn soil_points_styleCache[soil_point_text];};";
                                j_script += "\nvector_soil_points.setStyle(soil_points_style);}";
                                j_script += ("\nSetStyles_soil_points" + ClientID + "();");
                            }
                            if (CheckRowsCount(layersDS, "Soil") && CheckRowsCount(stylesDS, "SoilStyles") && CheckRowsCount(stylesDS, "SoilPointsStyles"))
                            {
                                j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_soil" + ClientID + "();\nSetStyles_soil_points" + ClientID + "();});");
                            }
                            else if (CheckRowsCount(layersDS, "Soil") && CheckRowsCount(stylesDS, "SoilStyles"))
                            {
                                j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_soil" + ClientID + "();});");
                            }
                            else if (CheckRowsCount(stylesDS, "SoilPointsStyles"))
                            {
                                j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_soil_points" + ClientID + "();});");
                            }

                            break;
                        }
                    case "erosion_soil":
                        {
                            DataSet layersDS = new DataSet();

                            conn.Open();
                            //String erosion_querry_string = "exec [dbo].[GetErosionGeoJSON] " + id_organization + ", " + year;
                            String erosion_querry_string = "exec [dbo].[GetErosionGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter erosion_geo_data = new SqlDataAdapter(erosion_querry_string, conn);
                            erosion_geo_data.Fill(layersDS, "Erosion");
                            conn.Close();

                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Outline" + ClientID + "();";
                            Int32 count_parts = 0;
                            if (CheckRowsCount(layersDS, "Erosion"))
                            {
                                count_parts = Convert.ToInt32(Round(layersDS.Tables["Erosion"].Rows.Count / 50, 0));
                                j_script += "\nerosion_source = new ol.source.Vector();";
                                j_script += "\nvector_erosion = new ol.layer.Vector({source: erosion_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_erosion);";
                                for (int i = 1; i <= count_parts + 1; i++)
                                {
                                    j_script += "\nCallServer('erosion_soil_part:" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                }
                            }
                            j_script += "\nCallServer('erosion_change:" + id_organization + "|" + tour + "', 'null');";
                            break;
                        }
                    case "erosion_soil_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            //String erosion_querry_string = "exec [dbo].[GetErosionGeoJSON] " + id_organization + ", " + year;
                            String erosion_querry_string = "exec [dbo].[GetErosionGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter erosion_geo_data = new SqlDataAdapter(erosion_querry_string, conn);
                            erosion_geo_data.Fill(layersDS, "Erosion");

                            String get_erosion_styles_str = "SELECT * FROM ViewErosionStyle";
                            SqlDataAdapter get_erosion_styles = new SqlDataAdapter(get_erosion_styles_str, conn);
                            get_erosion_styles.Fill(stylesDS, "ErosionStyles");

                            conn.Close();

                            if (CheckRowsCount(layersDS, "Erosion"))
                            {
                                //добавление эрозионной карты
                                String erosion_feature_string = String.Empty;
                                String erosion_feature_name = String.Empty;
                                String erosion_properties_string = String.Empty;
                                j_script += "\nerosion_source = vector_erosion.getSource();\nvar erosion_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) * 50;
                                if (to_i > layersDS.Tables["Erosion"].Rows.Count) { to_i = layersDS.Tables["Erosion"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    erosion_feature_string = "\nerosion_feature = format.readFeature('" + layersDS.Tables["Erosion"].Rows[i]["erosion_geo_json"] + "');";
                                    erosion_feature_name = "\nerosion_feature.name = '" + layersDS.Tables["Erosion"].Rows[i]["id_geographic_erosion"] + "';";
                                    for (int j = 0; j < layersDS.Tables["Erosion"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            erosion_properties_string = "\nerosion_feature.setProperties({'layer':'erosion','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Erosion"].Columns[j].ColumnName.ToString() != "erosion_geo_json")
                                        {
                                            if (erosion_properties_string[erosion_properties_string.Length - 1] != '{' && erosion_properties_string[erosion_properties_string.Length - 1] != ',')
                                            {
                                                erosion_properties_string += ",";
                                            }
                                            erosion_properties_string += ("'" + layersDS.Tables["Erosion"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Erosion"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Erosion"].Columns.Count - 1))
                                        {
                                            erosion_properties_string += "});";
                                        }
                                    }
                                    j_script += (erosion_feature_string + erosion_feature_name + erosion_properties_string + "\nerosion_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nerosion_source.addFeature(erosion_feature);";
                                }

                                //приеменение стилей для эрозионной карты
                                if (CheckRowsCount(stylesDS, "ErosionStyles"))
                                {
                                    j_script += ("\nfunction " + "SetStyles_erosion_soil" + ClientID + "(){");
                                    j_script += "\nvar erosion_styleCache = {};";
                                    j_script += "\nvar erosion_style = function (feature, resolution){";
                                    j_script += "\nvar erosion_text = feature.getProperties().code_erosion_soil;";
                                    j_script += "\nif (!erosion_styleCache[erosion_text]){";
                                    j_script += ("\nswitch (feature.getProperties().code_erosion_soil){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["ErosionStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["ErosionStyles"].Rows[i]["code_erosion_soil"].ToString() + "':{");
                                        if (stylesDS.Tables["ErosionStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().number_erosion_soil;\nif (resolution > " +
                                                        stylesDS.Tables["ErosionStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nerosion_styleCache[erosion_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["ErosionStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["ErosionStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["ErosionStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["ErosionStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["ErosionStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["ErosionStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["ErosionStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["ErosionStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn erosion_styleCache[erosion_text];};";
                                    j_script += "\nvector_erosion.setStyle(erosion_style);}";
                                    j_script += ("\nSetStyles_erosion_soil" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_erosion_soil" + ClientID + "();});");
                                }
                            }
                            break;
                        }
                    case "slope":
                        {
                            DataSet layersDS = new DataSet();

                            conn.Open();
                            //String slope_querry_string = "exec [dbo].[GetSlopeGeoJSON] " + id_organization + ", " + year;
                            String slope_querry_string = "exec [dbo].[GetSlopeGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter slope_geo_data = new SqlDataAdapter(slope_querry_string, conn);
                            slope_geo_data.Fill(layersDS, "Slope");
                            conn.Close();

                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Outline" + ClientID + "();";
                            Int32 count_parts = 0;
                            if (CheckRowsCount(layersDS, "Slope"))
                            {
                                count_parts = Convert.ToInt32(Round(layersDS.Tables["Slope"].Rows.Count / 50, 0));
                                j_script += "\nslope_source = new ol.source.Vector();";
                                j_script += "\nvector_slope = new ol.layer.Vector({source: slope_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_slope);";
                                for (int i = 1; i <= count_parts + 1; i++)
                                {
                                    j_script += "\nCallServer('slope_part:" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                }

                            }
                            break;
                        }
                    case "slope_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            //String slope_querry_string = "exec [dbo].[GetSlopeGeoJSON] @id_org = " + id_organization + ", @year = " + year;
                            String slope_querry_string = "exec [dbo].[GetSlopeGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter slope_geo_data = new SqlDataAdapter(slope_querry_string, conn);
                            slope_geo_data.Fill(layersDS, "Slope");

                            String get_slope_styles_str = "SELECT * FROM ViewSlopeStyle";
                            SqlDataAdapter get_slope_styles = new SqlDataAdapter(get_slope_styles_str, conn);
                            get_slope_styles.Fill(stylesDS, "SlopeStyles");

                            conn.Close();

                            if (CheckRowsCount(layersDS, "Slope"))
                            {
                                //добавление карты уклонов
                                String slope_feature_string = String.Empty;
                                String slope_feature_name = String.Empty;
                                String slope_properties_string = String.Empty;
                                j_script += "\nslope_source = vector_slope.getSource();\nvar slope_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) * 50;
                                if (to_i > layersDS.Tables["Slope"].Rows.Count) { to_i = layersDS.Tables["Slope"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    slope_feature_string = "\nslope_feature = format.readFeature('" + layersDS.Tables["Slope"].Rows[i]["slope_geo_json"].ToString() + "');";
                                    slope_feature_name = "\nslope_feature.name = '" + layersDS.Tables["Slope"].Rows[i]["id_geographic_slope"].ToString() + "';";
                                    for (int j = 0; j < layersDS.Tables["Slope"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            slope_properties_string = "\nslope_feature.setProperties({'layer':'slope','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Slope"].Columns[j].ColumnName.ToString() != "slope_geo_json")
                                        {
                                            if (slope_properties_string[slope_properties_string.Length - 1] != '{' && slope_properties_string[slope_properties_string.Length - 1] != ',')
                                            {
                                                slope_properties_string += ",";
                                            }
                                            slope_properties_string += ("'" + layersDS.Tables["Slope"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Slope"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Slope"].Columns.Count - 1))
                                        {
                                            slope_properties_string += "});";
                                        }
                                    }
                                    j_script += (slope_feature_string + slope_feature_name + slope_properties_string + "\nslope_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nslope_source.addFeature(slope_feature);";
                                }

                                if (CheckRowsCount(stylesDS, "SlopeStyles"))
                                {
                                    //приеменение стилей для карты уклонов
                                    j_script += ("\nfunction " + "SetStyles_slope" + ClientID + "(){");
                                    j_script += "\nvar slope_styleCache = {};";
                                    j_script += "\nvar slope_style = function (feature, resolution){";
                                    j_script += "\nvar slope_text = feature.getProperties().code_slope;";
                                    j_script += "\nif (!slope_styleCache[slope_text]){";
                                    j_script += ("\nswitch (feature.getProperties().code_slope){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["SlopeStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["SlopeStyles"].Rows[i]["code_slope"].ToString() + "':{");
                                        if (stylesDS.Tables["SlopeStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_slope;\nif (resolution > " +
                                                        stylesDS.Tables["SlopeStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nslope_styleCache[slope_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["SlopeStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SlopeStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["SlopeStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["SlopeStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["SlopeStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["SlopeStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["SlopeStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["SlopeStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SlopeStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["SlopeStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn slope_styleCache[slope_text];};";
                                    j_script += "\nvector_slope.setStyle(slope_style);}";
                                    j_script += ("\nSetStyles_slope" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_slope" + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "slope_log_" + current_part.ToString() + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "exposure":
                        {
                            DataSet layersDS = new DataSet();

                            conn.Open();
                            String exposure_querry_string = "exec [dbo].[GetExposureGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter exposure_geo_data = new SqlDataAdapter(exposure_querry_string, conn);
                            exposure_geo_data.Fill(layersDS, "Exposure");
                            conn.Close();

                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Outline" + ClientID + "();";
                            Int32 count_parts = 0;
                            if (CheckRowsCount(layersDS, "Exposure"))
                            {
                                count_parts = Convert.ToInt32(Round(layersDS.Tables["Exposure"].Rows.Count / 50, 0));
                                j_script += "\nexposure_source = new ol.source.Vector();";
                                j_script += "\nvector_exposure = new ol.layer.Vector({source: exposure_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_exposure);";
                                for (int i = 1; i <= count_parts + 1; i++)
                                {
                                    j_script += "\nCallServer('exposure_part:" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                }

                            }
                            break;
                        }
                    case "exposure_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            String exposure_querry_string = "exec [dbo].[GetExposureGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter exposure_geo_data = new SqlDataAdapter(exposure_querry_string, conn);
                            exposure_geo_data.Fill(layersDS, "Exposure");

                            String get_exposure_styles_str = "SELECT * FROM ViewExposureStyle";
                            SqlDataAdapter get_exposure_styles = new SqlDataAdapter(get_exposure_styles_str, conn);
                            get_exposure_styles.Fill(stylesDS, "ExposureStyles");

                            conn.Close();

                            if (CheckRowsCount(layersDS, "Exposure"))
                            {
                                //добавление карты экспозиции
                                String exposure_feature_string = String.Empty;
                                String exposure_feature_name = String.Empty;
                                String exposure_properties_string = String.Empty;
                                j_script += "\nexposure_source = vector_exposure.getSource();\nvar slope_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) * 50;
                                if (to_i > layersDS.Tables["Exposure"].Rows.Count) { to_i = layersDS.Tables["Exposure"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    exposure_feature_string = "\nexposure_feature = format.readFeature('" + layersDS.Tables["Exposure"].Rows[i]["exposure_geo_json"].ToString() + "');";
                                    exposure_feature_name = "\nexposure_feature.name = '" + layersDS.Tables["Exposure"].Rows[i]["id_geographic_exposure"].ToString() + "';";
                                    for (int j = 0; j < layersDS.Tables["Exposure"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            exposure_properties_string = "\nexposure_feature.setProperties({'layer':'exposure','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Exposure"].Columns[j].ColumnName.ToString() != "exposure_geo_json")
                                        {
                                            if (exposure_properties_string[exposure_properties_string.Length - 1] != '{' && exposure_properties_string[exposure_properties_string.Length - 1] != ',')
                                            {
                                                exposure_properties_string += ",";
                                            }
                                            exposure_properties_string += ("'" + layersDS.Tables["Exposure"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Exposure"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Exposure"].Columns.Count - 1))
                                        {
                                            exposure_properties_string += "});";
                                        }
                                    }
                                    j_script += (exposure_feature_string + exposure_feature_name + exposure_properties_string + "\nexposure_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nexposure_source.addFeature(exposure_feature);";
                                }

                                if (CheckRowsCount(stylesDS, "ExposureStyles"))
                                {
                                    //приеменение стилей для карты уклонов
                                    j_script += ("\nfunction SetStyles_exposure" + ClientID + "(){");
                                    j_script += "\nvar exposure_styleCache = {};";
                                    j_script += "\nvar exposure_style = function (feature, resolution){";
                                    j_script += "\nvar exposure_text = feature.getProperties().code_exposure;";
                                    j_script += "\nif (!exposure_styleCache[exposure_text]){";
                                    j_script += ("\nswitch (feature.getProperties().code_exposure){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["ExposureStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["ExposureStyles"].Rows[i]["code_exposure"].ToString() + "':{");
                                        if (stylesDS.Tables["ExposureStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_exposure;\nif (resolution > " +
                                                        stylesDS.Tables["ExposureStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nexposure_styleCache[exposure_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["ExposureStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ExposureStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["ExposureStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["ExposureStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["ExposureStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["ExposureStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["ExposureStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["ExposureStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ExposureStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["ExposureStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn exposure_styleCache[exposure_text];};";
                                    j_script += "\nvector_exposure.setStyle(exposure_style);}";
                                    j_script += ("\nSetStyles_exposure" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_exposure" + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "exposure_log_" + current_part.ToString() + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "typing":
                        {
                            DataSet layersDS = new DataSet();

                            conn.Open();
                            //String typing_querry_string = "exec [dbo].[GetTypingGeoJSON] " + id_organization + ", " + year;
                            String typing_querry_string = "exec [dbo].[GetTypingGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter typing_geo_data = new SqlDataAdapter(typing_querry_string, conn);
                            typing_geo_data.Fill(layersDS, "Typing");
                            conn.Close();

                            j_script = RemoveLayers();
                            j_script += "\nSetStyles_Outline" + ClientID + "();";
                            Int32 count_parts = 0;
                            if (CheckRowsCount(layersDS, "Typing"))
                            {
                                count_parts = Convert.ToInt32(Round(layersDS.Tables["Typing"].Rows.Count / 50, 0));
                                j_script += "\ntyping_source = new ol.source.Vector();";
                                j_script += "\nvector_typing = new ol.layer.Vector({source: typing_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_typing);";
                                for (int i = 1; i <= count_parts + 1; i++)
                                {
                                    j_script += "\nCallServer('typing_part:" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                }
                            }
                            break;
                        }
                    case "typing_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            //String typing_querry_string = "exec [dbo].[GetTypingGeoJSON] " + id_organization + ", " + year;
                            String typing_querry_string = "exec [dbo].[GetTypingGeoJSON] " + id_organization + ", " + tour;
                            SqlDataAdapter typing_geo_data = new SqlDataAdapter(typing_querry_string, conn);
                            typing_geo_data.Fill(layersDS, "Typing");

                            String get_typing_styles_str = "SELECT * FROM ViewTypingStyle";
                            SqlDataAdapter get_typing_styles = new SqlDataAdapter(get_typing_styles_str, conn);
                            get_typing_styles.Fill(stylesDS, "TypingStyles");

                            conn.Close();

                            if (CheckRowsCount(layersDS, "Typing"))
                            {
                                //добавление карты типизации
                                String typing_feature_string = String.Empty;
                                String typing_feature_name = String.Empty;
                                String typing_properties_string = String.Empty;
                                j_script += "\ntyping_source = vector_typing.getSource();\nvar typing_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) * 50;
                                if (to_i > layersDS.Tables["Typing"].Rows.Count) { to_i = layersDS.Tables["Typing"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    typing_feature_string = "\ntyping_feature = format.readFeature('" + layersDS.Tables["Typing"].Rows[i]["typing_geo_json"] + "');";
                                    typing_feature_name = "\ntyping_feature.name = '" + layersDS.Tables["Typing"].Rows[i]["id_geographic_typing"] + "';";
                                    for (int j = 0; j < layersDS.Tables["Typing"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            typing_properties_string = "\ntyping_feature.setProperties({'layer':'typing','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Typing"].Columns[j].ColumnName.ToString() != "typing_geo_json")
                                        {
                                            if (typing_properties_string[typing_properties_string.Length - 1] != '{' && typing_properties_string[typing_properties_string.Length - 1] != ',')
                                            {
                                                typing_properties_string += ",";
                                            }
                                            typing_properties_string += ("'" + layersDS.Tables["Typing"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Typing"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Typing"].Columns.Count - 1))
                                        {
                                            typing_properties_string += "});";
                                        }
                                    }
                                    j_script += (typing_feature_string + typing_feature_name + typing_properties_string + "\ntyping_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\ntyping_source.addFeature(typing_feature);";
                                }

                                if (CheckRowsCount(stylesDS, "TypingStyles"))
                                {
                                    //приеменение стилей для карты типизации
                                    j_script += ("\nfunction " + "SetStyles_typing" + ClientID + "(){");
                                    j_script += "\nvar typing_styleCache = {};";
                                    j_script += "\nvar typing_style = function (feature, resolution){";
                                    j_script += "\nvar typing_text = feature.getProperties().code_typing;";
                                    j_script += "\nif (!typing_styleCache[typing_text]){";
                                    j_script += ("\nswitch (feature.getProperties().code_typing){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["TypingStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["TypingStyles"].Rows[i]["code_typing"].ToString() + "':{");
                                        if (stylesDS.Tables["TypingStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().number_typing;\nif (resolution > " +
                                                        stylesDS.Tables["TypingStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\ntyping_styleCache[typing_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["TypingStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypingStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["TypingStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypingStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["TypingStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["TypingStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["TypingStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["TypingStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["TypingStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["TypingStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypingStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["TypingStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn typing_styleCache[typing_text];};";
                                    j_script += "\nvector_typing.setStyle(typing_style);}";
                                    j_script += ("\nSetStyles_typing" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_typing" + ClientID + "();});");
                                }
                            }
                            break;
                        }
                    case "project":
                        {
                            j_script = RemoveLayers();
                            //j_script += "\nSetStyles_Outline" + ClientID + "();";
                            j_script += "vector_plots.setVisible(false);";
                            j_script += "\nCallServer('project_plots', 'null');";
                            j_script += "\nCallServer('grassing', 'null');";
                            j_script += "\nCallServer('water_objects', 'null');";
                            j_script += "\nCallServer('woodland_belts', 'null');";
                            break;
                        }
                    case "project_plots":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            //в проектах нет тура, поэтому выборка по последнему году
                            String project_plots_querry_string = "exec [dbo].[GetProjectPlotsGeoJSON] " + id_organization + ", " + year;
                            SqlDataAdapter project_plots_geo_data = new SqlDataAdapter(project_plots_querry_string, conn);
                            project_plots_geo_data.Fill(layersDS, "ProjectPlots");

                            String get_project_plots_styles_str = "SELECT * FROM ViewDefaultStyle";
                            SqlDataAdapter get_project_plots_styles = new SqlDataAdapter(get_project_plots_styles_str, conn);
                            get_project_plots_styles.Fill(stylesDS, "ProjectPlotsStyle");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "ProjectPlots"))
                            {
                                //добавление проектных участков
                                String project_feature_string = String.Empty;
                                String project_feature_name = String.Empty;
                                String project_properties_string = String.Empty;
                                j_script += "\nvar project_plots_source = new ol.source.Vector();\nvar project_plots_feature;";

                                for (int i = 0; i < layersDS.Tables["ProjectPlots"].Rows.Count; i++)
                                {
                                    project_feature_string = "\nproject_plots_feature = format.readFeature('" + layersDS.Tables["ProjectPlots"].Rows[i]["project_plot_geo_json"] + "');";
                                    project_feature_name = "\nproject_plots_feature.name = '" + layersDS.Tables["ProjectPlots"].Rows[i]["id_geographic_project_plot"] + "';";
                                    for (int j = 0; j < layersDS.Tables["ProjectPlots"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            project_properties_string = "\nproject_plots_feature.setProperties({'layer':'project_plots','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["ProjectPlots"].Columns[j].ColumnName.ToString() != "project_plot_geo_json")
                                        {
                                            if (project_properties_string[project_properties_string.Length - 1] != '{' && project_properties_string[project_properties_string.Length - 1] != ',')
                                            {
                                                project_properties_string += ",";
                                            }
                                            project_properties_string += ("'" + layersDS.Tables["ProjectPlots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["ProjectPlots"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["ProjectPlots"].Columns.Count - 1))
                                        {
                                            project_properties_string += "});";
                                        }
                                    }
                                    j_script += (project_feature_string + project_feature_name + project_properties_string + "\nproject_plots_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nproject_plots_source.addFeature(project_plots_feature);";
                                }

                                j_script += "\nvar count_project_plots = project_plots_source.getFeatures().length; \nif(count_project_plots > 0){";
                                j_script += "\nvector_project_plots = new ol.layer.Vector({source: project_plots_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_project_plots); vector_project_plots.setZIndex(1);};";

                                //приеменение стилей для проектных участков
                                String fill_str, stroke_str, font_str;
                                if (CheckRowsCount(stylesDS, "ProjectPlotsStyle"))
                                {
                                    j_script += ("\nfunction SetStyles_project_plots" + ClientID + "(){");
                                    j_script += "\nvar project_plots_styleCache = {};";
                                    j_script += "\nvar project_plots_style = function (feature, resolution){";
                                    j_script += "\nvar project_plot_text = feature.name;";
                                    j_script += "\nif (!project_plots_styleCache[project_plot_text]){";
                                    if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_project_plot != null &&" +
                                                      "feature.getProperties().unique_id_project_plot != '')\n{t = feature.getProperties().unique_id_project_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    j_script += "\nproject_plots_styleCache[project_plot_text] = [new ol.style.Style({";
                                    fill_str = String.Empty;
                                    if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["id_color"].ToString() != String.Empty)
                                    {
                                        fill_str += "\nfill: new ol.style.Fill({";
                                        fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["red"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["green"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                    }
                                    stroke_str = String.Empty;
                                    if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["id_stroke_style"].ToString() != String.Empty)
                                    {
                                        stroke_str += "\nstroke: new ol.style.Stroke({";
                                        stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        stroke_str += ("\nwidth: " + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_width"].ToString());
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["stroke_dash_type"].ToString() == "1")
                                        {
                                            stroke_str += ",\nlineDash: [0.5, 4]";
                                        }
                                        stroke_str += "\n})";
                                    }
                                    font_str = String.Empty;
                                    if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        font_str += "\ntext: new ol.style.Text({font: '";
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["bold_font"].ToString() == "1")
                                        {
                                            font_str += "bold ";
                                        }
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["italic_font"].ToString() == "1")
                                        {
                                            font_str += "italic ";
                                        }
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["underline_font"].ToString() == "1")
                                        {
                                            font_str += "underline ";
                                        }
                                        font_str += (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["size_font"].ToString() + "px " + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_name"].ToString() + "',");
                                        font_str += "\ntext: getText(feature, resolution),";
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["offset_x"].ToString() != String.Empty)
                                        {
                                            font_str += ("\noffsetX: " + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["offset_x"].ToString() + ",");
                                        }
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["offset_y"].ToString() != String.Empty)
                                        {
                                            font_str += ("\noffsetY: " + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["offset_y"].ToString() + ",");
                                        }
                                        font_str += "\nfill: new ol.style.Fill({";
                                        font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                        font_str += "\nstroke: new ol.style.Stroke({";
                                        font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        font_str += ("\nwidth: " + stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_width"].ToString());
                                        if (stylesDS.Tables["ProjectPlotsStyle"].Rows[0]["font_stroke_dash_type"].ToString() == "1")
                                        {
                                            font_str += ",\nlineDash: [0.5, 4]})";
                                        }
                                        font_str += "\n})})";
                                    }

                                    if (fill_str != String.Empty)
                                    {
                                        j_script += fill_str;
                                    }
                                    if (stroke_str != String.Empty)
                                    {
                                        if (fill_str != String.Empty)
                                        {
                                            j_script += ",";
                                        }
                                        j_script += stroke_str;
                                    }
                                    if (font_str != String.Empty)
                                    {
                                        if (fill_str != String.Empty || stroke_str != String.Empty)
                                        {
                                            j_script += ",";
                                        }
                                        j_script += font_str;
                                    }
                                    j_script += "})];";

                                    j_script += "\n}\nreturn project_plots_styleCache[project_plot_text];};";
                                    j_script += "\nvector_project_plots.setStyle(project_plots_style); vector_plots_single.setStyle(project_plots_style);}";
                                    j_script += "\nSetStyles_" + eventArgument + ClientID + "();";
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_project_plots" + ClientID + "();});");

                                    //запись лог файлов
                                    /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "project_plots_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                    SW.Write(j_script);
                                    SW.Close();*/
                                }
                            }
                            break;
                        }
                    case "grassing":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();
                            String grassing_querry_string = "exec [dbo].[GetGrassingGeoJSON] " + id_organization + ", " + year;
                            SqlDataAdapter grassing_geo_data = new SqlDataAdapter(grassing_querry_string, conn);
                            grassing_geo_data.Fill(layersDS, "Grassing");

                            String get_grassing_styles_str = "SELECT * FROM ViewGrassingStyle";
                            SqlDataAdapter get_grassing_styles = new SqlDataAdapter(get_grassing_styles_str, conn);
                            get_grassing_styles.Fill(stylesDS, "GrassingStyle");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "Grassing"))
                            {
                                //добавление залужения
                                String grassing_feature_string = String.Empty;
                                String grassing_feature_name = String.Empty;
                                String grassing_properties_string = String.Empty;
                                j_script += "\nvar grassing_source = new ol.source.Vector();\nvar grassing_feature;";

                                for (int i = 0; i < layersDS.Tables["Grassing"].Rows.Count; i++)
                                {
                                    grassing_feature_string = "\ngrassing_feature = format.readFeature('" + layersDS.Tables["Grassing"].Rows[i]["grassing_geo_json"] + "');";
                                    grassing_feature_name = "\ngrassing_feature.name = '" + layersDS.Tables["Grassing"].Rows[i]["id_geographic_grassing"] + "';";
                                    for (int j = 0; j < layersDS.Tables["Grassing"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            grassing_properties_string = "\ngrassing_feature.setProperties({'layer':'grassing','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Grassing"].Columns[j].ColumnName.ToString() != "grassing_geo_json")
                                        {
                                            if (grassing_properties_string[grassing_properties_string.Length - 1] != '{' && grassing_properties_string[grassing_properties_string.Length - 1] != ',')
                                            {
                                                grassing_properties_string += ",";
                                            }
                                            grassing_properties_string += ("'" + layersDS.Tables["Grassing"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Grassing"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Grassing"].Columns.Count - 1))
                                        {
                                            grassing_properties_string += "});";
                                        }
                                    }
                                    j_script += (grassing_feature_string + grassing_feature_name + grassing_properties_string + "\ngrassing_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\ngrassing_source.addFeature(grassing_feature);";
                                }

                                j_script += "\nvar count_grassing = grassing_source.getFeatures().length; \nif(count_grassing > 0){";
                                j_script += "\nvector_grassing = new ol.layer.Vector({source: grassing_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_grassing); vector_grassing.setZIndex(2);};";

                                if (CheckRowsCount(stylesDS, "GrassingStyle"))
                                {
                                    //приеменение стилей для залужения
                                    j_script += ("\nfunction " + "SetStyles_" + eventArgument + ClientID + "(){");
                                    j_script += "\nvar grassing_styleCache = {};";
                                    j_script += "\nvar grassing_style = function (feature, resolution){";
                                    j_script += "\nvar grassing_text = feature.name;";
                                    j_script += "\nif (!grassing_styleCache[grassing_text]){";
                                    j_script += ("\nswitch (feature.getProperties().id_type_grassing){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["GrassingStyle"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["GrassingStyle"].Rows[i]["id_type_grassing"].ToString() + "':{");
                                        if (stylesDS.Tables["GrassingStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_grassing;\nif (resolution > " +
                                                        stylesDS.Tables["GrassingStyle"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\ngrassing_styleCache[grassing_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["GrassingStyle"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GrassingStyle"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["GrassingStyle"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["GrassingStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["GrassingStyle"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["GrassingStyle"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["GrassingStyle"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["GrassingStyle"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GrassingStyle"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["GrassingStyle"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn grassing_styleCache[grassing_text];};";
                                    j_script += "\nvector_grassing.setStyle(grassing_style);}";
                                    j_script += ("\nSetStyles_" + eventArgument + ClientID + "();");
                                    //j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_" + eventArgument + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "grassing_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "water_objects":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();
                            String water_objects_querry_string = "exec [dbo].[GetWaterObjectsGeoJSON] " + id_organization + ", " + year;
                            SqlDataAdapter water_objects_geo_data = new SqlDataAdapter(water_objects_querry_string, conn);
                            water_objects_geo_data.Fill(layersDS, "WaterObjects");

                            String get_water_objects_styles_str = "SELECT * FROM ViewWaterObjectsStyle";
                            SqlDataAdapter get_water_objects_styles = new SqlDataAdapter(get_water_objects_styles_str, conn);
                            get_water_objects_styles.Fill(stylesDS, "WaterObjectsStyle");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "WaterObjects"))
                            {
                                //добавление водных объеков
                                String water_objects_feature_string = String.Empty;
                                String water_objects_feature_name = String.Empty;
                                String water_objects_properties_string = String.Empty;
                                j_script += "\nvar water_objects_source = new ol.source.Vector();\nvar water_objects_feature;";

                                for (int i = 0; i < layersDS.Tables["WaterObjects"].Rows.Count; i++)
                                {
                                    water_objects_feature_string = "\nwater_objects_feature = format.readFeature('" + layersDS.Tables["WaterObjects"].Rows[i]["water_object_geo_json"] + "');";
                                    water_objects_feature_name = "\nwater_objects_feature.name = '" + layersDS.Tables["WaterObjects"].Rows[i]["id_geographic_water_object"] + "';";
                                    for (int j = 0; j < layersDS.Tables["WaterObjects"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            water_objects_properties_string = "\nwater_objects_feature.setProperties({'layer':'water_objects','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["WaterObjects"].Columns[j].ColumnName.ToString() != "water_object_geo_json")
                                        {
                                            if (water_objects_properties_string[water_objects_properties_string.Length - 1] != '{' && water_objects_properties_string[water_objects_properties_string.Length - 1] != ',')
                                            {
                                                water_objects_properties_string += ",";
                                            }
                                            water_objects_properties_string += ("'" + layersDS.Tables["WaterObjects"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["WaterObjects"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["WaterObjects"].Columns.Count - 1))
                                        {
                                            water_objects_properties_string += "});";
                                        }
                                    }
                                    j_script += (water_objects_feature_string + water_objects_feature_name + water_objects_properties_string + "\nwater_objects_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nwater_objects_source.addFeature(water_objects_feature);";
                                }

                                j_script += "\nvar count_water_objects = water_objects_source.getFeatures().length; \nif(count_water_objects > 0){";
                                j_script += "\nvector_water_objects = new ol.layer.Vector({source: water_objects_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_water_objects); vector_water_objects.setZIndex(2);};";

                                if (CheckRowsCount(stylesDS, "WaterObjectsStyle"))
                                {
                                    //приеменение стилей для водных объектов
                                    j_script += ("\nfunction " + "SetStyles_" + eventArgument + ClientID + "(){");
                                    j_script += "\nvar water_objects_styleCache = {};";
                                    j_script += "\nvar water_objects_style = function (feature, resolution){";
                                    j_script += "\nvar water_objects_text = feature.name;";
                                    j_script += "\nif (!water_objects_styleCache[water_objects_text]){";
                                    j_script += ("\nswitch (feature.getProperties().id_type_water_object){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["WaterObjectsStyle"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["id_type_water_object"].ToString() + "':{");
                                        if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_water_object;\nif (resolution > " +
                                                        stylesDS.Tables["WaterObjectsStyle"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nwater_objects_styleCache[water_objects_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["WaterObjectsStyle"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn water_objects_styleCache[water_objects_text];};";
                                    j_script += "\nvector_water_objects.setStyle(water_objects_style);}";
                                    j_script += ("\nSetStyles_" + eventArgument + ClientID + "();");
                                    //j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_" + eventArgument + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "water_objects_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "woodland_belts":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();
                            String woodland_belts_querry_string = "exec [dbo].[GetWoodlandBeltsGeoJSON] " + id_organization + ", " + year;
                            SqlDataAdapter woodland_belts_geo_data = new SqlDataAdapter(woodland_belts_querry_string, conn);
                            woodland_belts_geo_data.Fill(layersDS, "WoodlandBelts");

                            String get_woodland_belts_styles_str = "SELECT * FROM ViewWoodlandBeltsStyle";
                            SqlDataAdapter get_woodland_belts_styles = new SqlDataAdapter(get_woodland_belts_styles_str, conn);
                            get_woodland_belts_styles.Fill(stylesDS, "WoodlandBeltsStyle");
                            conn.Close();

                            if (CheckRowsCount(layersDS, "WoodlandBelts"))
                            {
                                //добавление лесополос
                                String woodland_belts_feature_string = String.Empty;
                                String woodland_belts_feature_name = String.Empty;
                                String woodland_belts_properties_string = String.Empty;
                                j_script += "\nvar woodland_belts_source = new ol.source.Vector();\nvar woodland_belts_feature;";

                                for (int i = 0; i < layersDS.Tables["WoodlandBelts"].Rows.Count; i++)
                                {
                                    woodland_belts_feature_string = "\nwoodland_belts_feature = format.readFeature('" + layersDS.Tables["WoodlandBelts"].Rows[i]["woodland_belt_geo_json"] + "');";
                                    woodland_belts_feature_name = "\nwoodland_belts_feature.name = '" + layersDS.Tables["WoodlandBelts"].Rows[i]["id_geographic_woodland_belt"] + "';";
                                    for (int j = 0; j < layersDS.Tables["WoodlandBelts"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            woodland_belts_properties_string = "\nwoodland_belts_feature.setProperties({'layer':'woodland_belts','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["WoodlandBelts"].Columns[j].ColumnName.ToString() != "woodland_belt_geo_json")
                                        {
                                            if (woodland_belts_properties_string[woodland_belts_properties_string.Length - 1] != '{' && woodland_belts_properties_string[woodland_belts_properties_string.Length - 1] != ',')
                                            {
                                                woodland_belts_properties_string += ",";
                                            }
                                            woodland_belts_properties_string += ("'" + layersDS.Tables["WoodlandBelts"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["WoodlandBelts"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["WoodlandBelts"].Columns.Count - 1))
                                        {
                                            woodland_belts_properties_string += "});";
                                        }
                                    }
                                    j_script += (woodland_belts_feature_string + woodland_belts_feature_name + woodland_belts_properties_string + "\nwoodland_belts_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nwoodland_belts_source.addFeature(woodland_belts_feature);";
                                }

                                j_script += "\nvar count_woodland_belts = woodland_belts_source.getFeatures().length; \nif(count_woodland_belts > 0){";
                                j_script += "\nvector_woodland_belts = new ol.layer.Vector({source: woodland_belts_source, maxResolution:100});";
                                j_script += "\nmap.addLayer(vector_woodland_belts); vector_woodland_belts.setZIndex(2);};";

                                if (CheckRowsCount(stylesDS, "WoodlandBeltsStyle"))
                                {
                                    //приеменение стилей для лесополос
                                    j_script += ("\nfunction " + "SetStyles_" + eventArgument + ClientID + "(){");
                                    j_script += "\nvar woodland_belts_styleCache = {};";
                                    j_script += "\nvar woodland_belts_style = function (feature, resolution){";
                                    j_script += "\nvar woodland_belts_text = feature.name;";
                                    j_script += "\nif (!woodland_belts_styleCache[woodland_belts_text]){";
                                    j_script += ("\nswitch (feature.getProperties().id_type_woodland_belt){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["WoodlandBeltsStyle"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["id_type_woodland_belt"].ToString() + "':{");
                                        if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_woodland_belt;\nif (resolution > " +
                                                        stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nwoodland_belts_styleCache[woodland_belts_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["WoodlandBeltsStyle"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn woodland_belts_styleCache[woodland_belts_text];};";
                                    j_script += "\nvector_woodland_belts.setStyle(woodland_belts_style);}";
                                    j_script += ("\nSetStyles_" + eventArgument + ClientID + "();");
                                    //j_script += ("\nmap.on('moveend', function () {\nSetStyles_Outline" + ClientID + "();\nSetStyles_" + eventArgument + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "woodland_belts_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "code_plot":
                        {
                            if (connection_try)
                            {
                                DataSet dataDS = new DataSet();

                                conn.Open();
                                String get_plots_info = "SELECT * FROM View_Plots_Tree WHERE code_plot = '" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                SqlDataAdapter get_plots = new SqlDataAdapter(get_plots_info, conn);
                                get_plots.Fill(dataDS, "Plots");

                                String get_agro_info = "SELECT * FROM View_For_ArcGis_Full WHERE code_plot = '" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                SqlDataAdapter get_agro = new SqlDataAdapter(get_agro_info, conn);
                                get_agro.Fill(dataDS, "Agro");

                                String get_soil_info = "SELECT * FROM View_Soil_From_Plot WHERE code_plot = '" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                SqlDataAdapter get_soil = new SqlDataAdapter(get_soil_info, conn);
                                get_soil.Fill(dataDS, "Soil");

                                conn.Close();

                                String id_sort_crop_rotation = "0";

                                if (CheckRowsCount(dataDS, "Plots"))
                                {
                                    j_script += "\n$(function () {";
                                    j_script += ("\n$(\"#OrgTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_organization"].ToString() + "');");
                                    j_script += ("\n$(\"#DepTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_department"].ToString() + "');");
                                    j_script += ("\n$(\"#UniqNumberTB\").val('" + dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "');");
                                    j_script += ("\n$(\"#NumberPlotTB\").val('" + dataDS.Tables["Plots"].Rows[0]["number_plot"].ToString() + "');");
                                    j_script += ("\n$(\"#AreaTB\").val('" + dataDS.Tables["Plots"].Rows[0]["area"].ToString() + "');");

                                    if (CheckRowsCount(dataDS, "Agro"))
                                    {
                                        j_script += ("\n$(\"#YearTB\").val('" + dataDS.Tables["Agro"].Rows[0]["year"].ToString() + "');");
                                        j_script += ("\n$(\"#NTB\").val('" + dataDS.Tables["Agro"].Rows[0]["n"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_n_group"].ToString() + "');");
                                        j_script += ("\n$(\"#P2O5TB\").val('" + dataDS.Tables["Agro"].Rows[0]["p2o5"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupP2O5TB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_p2o5_group"].ToString() + "');");
                                        j_script += ("\n$(\"#K2OTB\").val('" + dataDS.Tables["Agro"].Rows[0]["k2o"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupK2OTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_k2o_group"].ToString() + "');");
                                        j_script += ("\n$(\"#pHTB\").val('" + dataDS.Tables["Agro"].Rows[0]["ph_s"].ToString() + "');");
                                        j_script += ("\n$(\"#GrouppHTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ph_s_group"].ToString() + "');");
                                        j_script += ("\n$(\"#HumusTB\").val('" + dataDS.Tables["Agro"].Rows[0]["humus"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupHumusTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_humus_group"].ToString() + "');");
                                        j_script += ("\n$(\"#HATB\").val('" + dataDS.Tables["Agro"].Rows[0]["hydrolytic_acid"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupHATB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ha_group"].ToString() + "');");
                                        j_script += ("\n$(\"#ACTB\").val('" + dataDS.Tables["Agro"].Rows[0]["absorbance_capacity"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupACTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ac_group"].ToString() + "');");
                                        j_script += ("\n$(\"#TABTB\").val('" + dataDS.Tables["Agro"].Rows[0]["total_absorbed_bases"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupTABTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_tab_group"].ToString() + "');");
                                        j_script += ("\n$(\"#BSTB\").val('" + dataDS.Tables["Agro"].Rows[0]["base_saturation"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupBSTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_bs_group"].ToString() + "');");

                                        j_script += ("\n$(\"#MNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mn"].ToString() + "');");
                                        j_script += ("\n$(\"#CATB\").val('" + dataDS.Tables["Agro"].Rows[0]["ca"].ToString() + "');");
                                        j_script += ("\n$(\"#STB\").val('" + dataDS.Tables["Agro"].Rows[0]["s"].ToString() + "');");
                                        j_script += ("\n$(\"#BTB\").val('" + dataDS.Tables["Agro"].Rows[0]["b"].ToString() + "');");
                                        j_script += ("\n$(\"#MOTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mo"].ToString() + "');");
                                        j_script += ("\n$(\"#CUTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cu"].ToString() + "');");
                                        j_script += ("\n$(\"#ZNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["zn"].ToString() + "');");
                                        j_script += ("\n$(\"#NATB\").val('" + dataDS.Tables["Agro"].Rows[0]["na"].ToString() + "');");
                                        j_script += ("\n$(\"#COTB\").val('" + dataDS.Tables["Agro"].Rows[0]["co"].ToString() + "');");
                                        j_script += ("\n$(\"#MGTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mg"].ToString() + "');");
                                        j_script += ("\n$(\"#FETB\").val('" + dataDS.Tables["Agro"].Rows[0]["fe"].ToString() + "');");
                                        j_script += ("\n$(\"#ALTB\").val('" + dataDS.Tables["Agro"].Rows[0]["al"].ToString() + "');");

                                        j_script += ("\n$(\"#CUHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cu_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#ZNHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["zn_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#CDHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cd_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#PBHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["pb_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#NIHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["ni_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#HGHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["hg_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#ASHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["as_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#MGHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mg_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#CRHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cr_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#FHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["f_hm"].ToString() + "');");

                                        j_script += ("\n$(\"#PriorityCalcifTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_pc_group"].ToString() + "');");
                                        j_script += ("\n$(\"#pH1TB\").val('" + dataDS.Tables["Agro"].Rows[0]["ph_s"].ToString() + "');");
                                        j_script += ("\n$(\"#HA1TB\").val('" + dataDS.Tables["Agro"].Rows[0]["hydrolytic_acid"].ToString() + "');");

                                        j_script += ("\n$(\"#NumberFieldTB\").val('" + dataDS.Tables["Agro"].Rows[0]["number_field"].ToString() + "');");
                                    }

                                    if (CheckRowsCount(dataDS, "Soil"))
                                    {
                                        j_script += ("\n$(\"#Year2TB\").val('" + dataDS.Tables["Soil"].Rows[0]["year"].ToString() + "');");

                                        j_script += ("\n$(\"#Slope1TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area1"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope2TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area2"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope3TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area3"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope4TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area4"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope5TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area5"].ToString() + "');");
                                        j_script += ("\n$(\"#ExposureTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_exposure"].ToString() + "');");
                                        j_script += ("\n$(\"#ErosionTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_erosion"].ToString() + "');");
                                        j_script += ("\n$(\"#TypeSoilTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_type_soil"].ToString() + "');");
                                        j_script += ("\n$(\"#GradingTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_grading"].ToString() + "');");
                                    }

                                    //выборка и вывод данных по севообороту для редактирования
                                    //года не обновляем при переключении полей
                                    /*j_script += ("\n$(\"#Year3CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year3CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year3CB option:nth-child(3)\").attr('selected', 'selected');";

                                    j_script += ("\n$(\"#Year4CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year4CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year4CB option:nth-child(3)\").attr('selected', 'selected');";*/

                                    conn.Open();
                                    String get_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" +
                                                           dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND [year] = " + eventArgument.Split(':')[1].Split('|')[1] + ";";
                                    SqlDataAdapter get_book = new SqlDataAdapter(get_book_info, conn);
                                    get_book.Fill(dataDS, "Book");
                                    String test_eventArgument = eventArgument;

                                    String get_old_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" +
                                                           dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND [year] = " +
                                                           (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[1]) - 1).ToString() + ";";
                                    SqlDataAdapter get_old_book = new SqlDataAdapter(get_old_book_info, conn);
                                    get_old_book.Fill(dataDS, "OldBook");

                                    if (CheckRowsCount(dataDS, "Book"))
                                    {
                                        String get_book_tillage_info = "SELECT * FROM View_History_Book_Tillage WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_tillage = new SqlDataAdapter(get_book_tillage_info, conn);
                                        get_book_tillage.Fill(dataDS, "BookTillage");

                                        String get_book_plant_protection_info = "SELECT * FROM View_History_Book_Plant_Protection WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_plant_protection = new SqlDataAdapter(get_book_plant_protection_info, conn);
                                        get_book_plant_protection.Fill(dataDS, "BookPlantProtection");

                                        String get_book_pests_info = "SELECT * FROM View_History_Book_Pests WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_pests = new SqlDataAdapter(get_book_pests_info, conn);
                                        get_book_pests.Fill(dataDS, "BookPests");

                                        String get_book_diseases_info = "SELECT * FROM View_History_Book_Diseases WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_diseases = new SqlDataAdapter(get_book_diseases_info, conn);
                                        get_book_diseases.Fill(dataDS, "BookDiseases");

                                        String get_book_weeds_info = "SELECT * FROM View_History_Book_Weeds WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_weeds = new SqlDataAdapter(get_book_weeds_info, conn);
                                        get_book_weeds.Fill(dataDS, "BookWeeds");

                                        String get_book_weediness_info = "SELECT * FROM View_History_Book_Weediness WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_weediness = new SqlDataAdapter(get_book_weediness_info, conn);
                                        get_book_weediness.Fill(dataDS, "BookWeediness");
                                    }
                                    conn.Close();

                                    if (CheckRowsCount(dataDS, "OldBook"))
                                    {
                                        j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldBasicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldSowingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldDressingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#OldDoseSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#OldDoseDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                        j_script += "\nCallServer('old_total_dose:" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_basic_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["dose_basic_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_sowing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["dose_sowing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_dressing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["dose_dressing_fertilization"].ToString()) + "', 'null');";

                                        j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldOrganicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");

                                        j_script += "\nCallServer('old_org_total_dose:" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_organic_fertilizer"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["OldBook"].Rows[0]["dose_organic_fertilizer"].ToString()) + "', 'null');";
                                    }
                                    else
                                    {
                                        j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureCB option[value='0']\").prop('selected', true);");

                                        j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldBasicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldSowingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldDressingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDoseSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDoseDressingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateDressingFertTB\").val('');");

                                        j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldOrganicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateOrganicFertTB\").val('');");
                                    }

                                    if (CheckRowsCount(dataDS, "Book"))
                                    {
                                        j_script += ("\n$(\"#TypePropertyCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#TypePropertyCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_type_property"].ToString()) + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        id_sort_crop_rotation = NotNull(dataDS.Tables["Book"].Rows[0]["id_sort_crop_rotation"].ToString());
                                        j_script += ("\n$(\"#SortCRCB option[value='" + id_sort_crop_rotation + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");
                                        String id_culture = NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        //иначе сбрасывается гибрид j_script += ("\nCallServer('change_culture:" + id_culture + "|" + dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "', 'null');");
                                        /*if (Convert.ToInt32(NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString())) > 0)
                                        {
                                            conn.Open();
                                            String get_phase_pests_str = "SELECT DISTINCT id_phase, title_phase FROM View_Threshold_Pests WHERE id_culture=" + id_culture + " ORDER BY title_phase";
                                            String get_phase_diseases_str = "SELECT DISTINCT id_phase, title_phase FROM View_Threshold_Diseases WHERE id_culture=" + id_culture + " ORDER BY title_phase";
                                            SqlDataAdapter get_phase_pests = new SqlDataAdapter(get_phase_pests_str, conn);
                                            SqlDataAdapter get_phase_diseases = new SqlDataAdapter(get_phase_diseases_str, conn);
                                            get_phase_pests.Fill(dataDS, "PhasePests");
                                            get_phase_diseases.Fill(dataDS, "PhaseDiseases");

                                            j_script += "\n$(function () {";
                                            j_script += "\n$(\"#PhasePestsCB\").empty();";
                                            j_script += ("\n$(\"#PhasePestsCB\").append('<option value=\"0\"></option>');");
                                            if (CheckRowsCount(dataDS, "PhasePests"))
                                            {
                                                for (int i = 0; i < dataDS.Tables["PhasePests"].Rows.Count; i++)
                                                {
                                                    j_script += ("\n$(\"#PhasePestsCB\").append('<option value=\"" + dataDS.Tables["PhasePests"].Rows[i]["id_phase"].ToString() + "\">" +
                                                                 dataDS.Tables["PhasePests"].Rows[i]["title_phase"].ToString() + "</option>');");
                                                }
                                            }
                                            j_script += "});";

                                            j_script += "\n$(function () {";
                                            j_script += "\n$(\"#PhaseDiseasesCB\").empty();";
                                            j_script += ("\n$(\"#PhaseDiseasesCB\").append('<option value=\"0\"></option>');");
                                            if (CheckRowsCount(dataDS, "PhaseDiseases"))
                                            {
                                                for (int i = 0; i < dataDS.Tables["PhaseDiseases"].Rows.Count; i++)
                                                {
                                                    j_script += ("\n$(\"#PhaseDiseasesCB\").append('<option value=\"" + dataDS.Tables["PhaseDiseases"].Rows[i]["id_phase"].ToString() + "\">" +
                                                                 dataDS.Tables["PhaseDiseases"].Rows[i]["title_phase"].ToString() + "</option>');");
                                                }
                                            }
                                            j_script += "});";
                                            conn.Close();
                                        }*/

                                        //загрузка сортов/гибридов
                                        conn.Open();
                                        String get_cross_culture_str = "SELECT * FROM Cross_culture WHERE id_culture = " + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        SqlDataAdapter get_cross_culture = new SqlDataAdapter(get_cross_culture_str, conn);
                                        get_cross_culture.Fill(dataDS, "CrossCulture");
                                        String get_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        SqlDataAdapter get_culture_zone = new SqlDataAdapter(get_culture_zones_str, conn);
                                        get_culture_zone.Fill(dataDS, "CultureZones");
                                        try
                                        {
                                            String get_codeplot_grading_erosion_str = "SELECT Plot.code_plot,Soil.id_grading,id_erosion FROM Plot,Soil WHERE Plot.unique_id_plot='" + dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND Plot.id_plot=Soil.id_plot;";
                                            SqlDataAdapter get_codeplot_grading_erosion = new SqlDataAdapter(get_codeplot_grading_erosion_str, conn);
                                            get_codeplot_grading_erosion.Fill(dataDS, "Temp_Values");
                                            String id_grading = NotNull(dataDS.Tables["Temp_Values"].Rows[0]["id_grading"].ToString());
                                            String id_erosion = NotNull(dataDS.Tables["Temp_Values"].Rows[0]["id_erosion"].ToString());


                                            String get_coefs_str = "exec [dbo].[Get_Coefficients_N_P2O5_K2O] '" + eventArgument.Split(':')[1].Split('|')[0] + "', " + (NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString()) == "0" ? NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString()) : NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString())) + ", " + (NotNull(eventArgument.Split(':')[1].Split('|')[2]) == "0" ? "10" : NotNull(eventArgument.Split(':')[1].Split('|')[2])) + ", " + id_grading + ", " + id_erosion;
                                            SqlDataAdapter get_coefs = new SqlDataAdapter(get_coefs_str, conn);
                                            get_coefs.Fill(dataDS, "DosesCalculations");

                                            j_script += "$(\"#Loss_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_n"].ToString() + "');\n";
                                            j_script += "$(\"#Loss_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#Loss_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_k2o"].ToString() + "');\n";

                                            j_script += "$(\"#K1_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_n"].ToString() + "');\n";
                                            j_script += "$(\"#K1_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K1_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K2_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k2"].ToString() + "');\n";
                                            j_script += "$(\"#K3_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_n"].ToString() + "');\n";
                                            j_script += "$(\"#K3_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K3_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K4_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_n"].ToString() + "');\n";
                                            j_script += "$(\"#K4_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K4_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K5_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_n"].ToString() + "');\n";
                                            j_script += "$(\"#K5_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K5_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K6_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_n"].ToString() + "');\n";
                                            j_script += "$(\"#K6_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K6_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K7_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k7_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K7_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k7_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K8_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k8"].ToString() + "');\n";
                                        }
                                        catch { }

                                        conn.Close();

                                        j_script += "$(\"#Planned_productivityTB\").val($(\"#PlannedProdTB\").val());\n";
                                        j_script += "$(\"#Do_nTB\").val($(\"#OrganicTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Do_p2o5TB\").val($(\"#OrganicTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Do_k2oTB\").val($(\"#OrganicTotalDoseKTB\").val());\n";
                                        j_script += "$(\"#Dop_nTB\").val($(\"#OldOrganicTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Dop_p2o5TB\").val($(\"#OldOrganicTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Dop_k2oTB\").val($(\"#OldOrganicTotalDoseKTB\").val());\n";
                                        j_script += "$(\"#Dp_nTB\").val($(\"#OldTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Dp_p2o5TB\").val($(\"#OldTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Dp_k2oTB\").val($(\"#OldTotalDoseKTB\").val());\n";


                                        j_script += "\n$(\"#CrossCultureCB\").empty();";
                                        j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"0\"></option>');");

                                        if (CheckRowsCount(dataDS, "CrossCulture"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CrossCulture"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"" + dataDS.Tables["CrossCulture"].Rows[i]["id_cross_culture"].ToString() + "\">" +
                                                             dataDS.Tables["CrossCulture"].Rows[i]["title_cross_culture"].ToString() + "</option>');");
                                            }
                                        }

                                        j_script += "\n$(\"#CultureZoneCB\").empty();";
                                        j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"0\"></option>');");

                                        if (CheckRowsCount(dataDS, "CultureZones"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                            }
                                        }
                                        /*j_script += "\n$(\"#OldCultureZoneCB\").empty();";
                                        j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"0\"></option>');");

                                        if (dataDS.Tables["OldCultureZones"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dataDS.Tables["OldCultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"" + dataDS.Tables["OldCultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["OldCultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                            }
                                        }*/
                                        //---------------------------------

                                        j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CrossCultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_cross_culture"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#ReproductionTB\").val('" + dataDS.Tables["Book"].Rows[0]["reproduction"].ToString() + "');");
                                        j_script += ("\n$(\"#SeedingRateTB\").val('" + dataDS.Tables["Book"].Rows[0]["seeding_rate"].ToString() + "');");
                                        j_script += ("\n$(\"#PlannedProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["planned_productivity"].ToString() + "');");
                                        j_script += ("\n$(\"#SowingDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["sowing_date"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#ActualProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["actual_productivity"].ToString() + "');");
                                        j_script += ("\n$(\"#HarvestDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["harvest_date"].ToString().Split(' ')[0] + "');");

                                        j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#BasicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SowingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#DressingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#DoseSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#DoseDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                        j_script += "\nCallServer('total_dose:" + NotNull(dataDS.Tables["Book"].Rows[0]["id_basic_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["Book"].Rows[0]["dose_basic_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["Book"].Rows[0]["id_sowing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["Book"].Rows[0]["dose_sowing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["Book"].Rows[0]["id_dressing_fertilization"].ToString()) +
                                                    "|" + NotNull(dataDS.Tables["Book"].Rows[0]["dose_dressing_fertilization"].ToString()) + "', 'null');";

                                        j_script += ("\n$(\"#DoseOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                        j_script += ("\n$(\"#DateOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");
                                        j_script += "\nvar N_val, P_val, K_val;";
                                        j_script += ("\n$(\"#IdProtocolTB\").val('" + dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString() + "');");
                                        j_script += ("\n$(\"#NumberProtocolTB\").val('" + dataDS.Tables["Book"].Rows[0]["number_protocol"].ToString() + "');");
                                        j_script += ("\n$(\"#NContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["n_content"].ToString() + "');");
                                        j_script += ("\n$(\"#PContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["p_content"].ToString() + "');");
                                        j_script += ("\n$(\"#KContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["k_content"].ToString() + "');");
                                        /*j_script += "\nif($(\"#DoseOrganicFertTB\").val() != 0 && $(\"#DoseOrganicFertTB\").val() != null && $(\"#DoseOrganicFertTB\").val() != ''){";
                                        j_script += "\nif($(\"#NContentTB\").val() != 0 && $(\"#NContentTB\").val() != null && $(\"#NContentTB\").val() != '') { N_val = $(\"#DoseOrganicFertTB\").val() * $(\"#NContentTB\").val().replace(',','.') * 10; $(\"#OrganicTotalDoseNTB\").val(N_val); } ";
                                        j_script += "\nif($(\"#PContentTB\").val() != 0 && $(\"#PContentTB\").val() != null && $(\"#PContentTB\").val() != '') { P_val = $(\"#DoseOrganicFertTB\").val() * $(\"#PContentTB\").val().replace(',','.') * 10; $(\"#OrganicTotalDosePTB\").val(P_val); } ";
                                        j_script += "\nif($(\"#KContentTB\").val() != 0 && $(\"#KContentTB\").val() != null && $(\"#KContentTB\").val() != '') { L_val = $(\"#DoseOrganicFertTB\").val() * $(\"#KContentTB\").val().replace(',','.') * 10; $(\"#OrganicTotalDoseKTB\").val(K_val); } ";
                                        j_script += "\nelse { CallServer('org_total_dose:' + $(\"#OrganicFertCB\").val() + '|' + $(\"#DoseOrganicFertTB\").val() + '|0', 'null'); }";
                                        j_script += "\n};";*/

                                        if (dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString() != null && dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString() != "")
                                        {
                                            String get_protocol_str = "SELECT * FROM View_Protocols WHERE id_protocol = " + dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString();
                                            conn.Open();
                                            SqlDataAdapter get_protocol = new SqlDataAdapter(get_protocol_str, conn);
                                            get_protocol.Fill(dataDS, "Protocol");
                                            conn.Close();
                                            if (CheckRowsCount(dataDS, "Protocol"))
                                            {
                                                j_script += "\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                                j_script += ("\n$(\"#OrganicFertCB option[value='" + dataDS.Tables["Protocol"].Rows[0]["id_fertilizer"].ToString() + "']\").prop('selected', true);");

                                                j_script += ("\nCallServer('org_total_dose:" + dataDS.Tables["Protocol"].Rows[0]["id_fertilizer"].ToString() +
                                                            "|" + dataDS.Tables["Book"].Rows[0]["dose_organic_fertilizer"].ToString() +
                                                            "|" + dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString() + "', 'null');");
                                            }
                                        }
                                        else
                                        {
                                            j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                            j_script += ("\n$(\"#OrganicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                            j_script += ("\nCallServer('org_total_dose:" + NotNull(dataDS.Tables["Book"].Rows[0]["id_organic_fertilizer"].ToString()) +
                                                            "|" + NotNull(dataDS.Tables["Book"].Rows[0]["dose_organic_fertilizer"].ToString()) + "|0', 'null');");
                                        }

                                        j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator1CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_1"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_1TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_1"].ToString() + "');");
                                        j_script += ("\n$(\"#DoseAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_1"].ToString() + "');");
                                        j_script += ("\n$(\"#DateAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_1"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator2CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_2"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_2TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_2"].ToString() + "');");
                                        j_script += ("\n$(\"#DoseAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_2"].ToString() + "');");
                                        j_script += ("\n$(\"#DateAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_2"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#FactCaCO3TB\").val(GetFactCaCO3());");

                                        //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                        j_script += "\n$('#TillageT').html('');";
                                        if (CheckRowsCount(dataDS, "BookTillage"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookTillage"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#TillageT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookTillage"].Rows[i]["id_type_tillage"].ToString() + "</td><td width=\"250\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["title_type_tillage"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["depth_tillage"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["date_tillage"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-tillage\" href=\"#delete_tillage\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-tillage\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }

                                        //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                        j_script += "\n$('#PlantProtectionT').html('');";
                                        if (CheckRowsCount(dataDS, "BookPlantProtection"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookPlantProtection"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#PlantProtectionT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_type_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_type_drug"].ToString() +
                                                            "</td><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_drug"].ToString() +
                                                            "</td><td width=\"250\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_drug"].ToString() +
                                                            "</td><td width=\"50\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["dose_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["date_plant_protection"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-plant-protection\" href=\"#delete_plant_protection\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-plant-protection\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }

                                        //добавление вредителей
                                        j_script += "\n$('#PestsT').html('');";
                                        if (CheckRowsCount(dataDS, "BookPests"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookPests"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#PestsT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookPests"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["id_pest"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["title_pest"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["count_pests"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["date_pest"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-pest\" href=\"#delete_pest\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-pest\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление болезней
                                        j_script += "\n$('#DiseasesT').html('');";
                                        if (CheckRowsCount(dataDS, "BookDiseases"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookDiseases"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#DiseasesT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookDiseases"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["id_disease"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["title_disease"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["percent_disease"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["date_disease"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-disease\" href=\"#delete_disease\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-disease\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление сорняков
                                        j_script += "\n$('#WeedsT').html('');";
                                        if (CheckRowsCount(dataDS, "BookWeeds"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookWeeds"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#WeedsT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookWeeds"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["id_weed"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["title_weed"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["count_weed"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["date_weed"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-weed\" href=\"#delete_weed\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-weed\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление засоренности
                                        j_script += "\n$('#WeedinessT').html('');";
                                        if (CheckRowsCount(dataDS, "BookWeediness"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookWeediness"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#WeedinessT\").append('<tr><td width=\"150\">" + dataDS.Tables["BookWeediness"].Rows[i]["title_weediness"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["weediness"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["weediness_percent"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["date_weediness"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-weediness\" href=\"#delete_weediness\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-weediness\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        bool set_zone_10 = false;
                                        if (CheckRowsCount(dataDS, "CultureZones"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                                if (dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") set_zone_10 = true;
                                            }
                                        }
                                        if (set_zone_10) j_script += "\n$(\"#CultureZoneCB\").val(\"10\");";
                                    }
                                    else
                                    {
                                        try
                                        {
                                            conn.Open();
                                            String get_codeplot_grading_erosion_str = "SELECT Plot.code_plot,Soil.id_grading,id_erosion FROM Plot,Soil WHERE Plot.unique_id_plot='" + dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND Plot.id_plot=Soil.id_plot;";
                                            SqlDataAdapter get_codeplot_grading_erosion = new SqlDataAdapter(get_codeplot_grading_erosion_str, conn);
                                            get_codeplot_grading_erosion.Fill(dataDS, "Temp_Values");
                                            String id_grading = NotNull(dataDS.Tables["Temp_Values"].Rows[0]["id_grading"].ToString());
                                            String id_erosion = NotNull(dataDS.Tables["Temp_Values"].Rows[0]["id_erosion"].ToString());
                                            String get_coefs_str = "exec [dbo].[Get_Coefficients_N_P2O5_K2O] '" + eventArgument.Split(':')[1].Split('|')[0] + "', " + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString()) + ", " + (NotNull(eventArgument.Split(':')[1].Split('|')[2]) == "0" ? "10" : NotNull(eventArgument.Split(':')[1].Split('|')[2])) + ", " + id_grading + ", " + id_erosion;
                                            SqlDataAdapter get_coefs = new SqlDataAdapter(get_coefs_str, conn);
                                            get_coefs.Fill(dataDS, "DosesCalculations");
                                            conn.Close();
                                            j_script += "$(\"#Loss_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_n"].ToString() + "');\n";
                                            j_script += "$(\"#Loss_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#Loss_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["loss_k2o"].ToString() + "');\n";

                                            j_script += "$(\"#K1_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_n"].ToString() + "');\n";
                                            j_script += "$(\"#K1_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K1_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k1_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K2_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k2"].ToString() + "');\n";
                                            j_script += "$(\"#K3_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_n"].ToString() + "');\n";
                                            j_script += "$(\"#K3_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K3_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k3_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K4_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_n"].ToString() + "');\n";
                                            j_script += "$(\"#K4_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K4_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k4_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K5_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_n"].ToString() + "');\n";
                                            j_script += "$(\"#K5_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K5_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k5_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K6_nTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_n"].ToString() + "');\n";
                                            j_script += "$(\"#K6_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K6_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k6_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K7_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k7_p2o5"].ToString() + "');\n";
                                            j_script += "$(\"#K7_k2oTB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k7_k2o"].ToString() + "');\n";
                                            j_script += "$(\"#K8_p2o5TB\").val('" + dataDS.Tables["DosesCalculations"].Rows[0]["k8"].ToString() + "');\n";
                                        }
                                        catch { conn.Close(); }
                                        if (CheckRowsCount(dataDS, "OldBook"))
                                        {
                                            conn.Open();
                                            String get_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString());
                                            SqlDataAdapter get_culture_zone = new SqlDataAdapter(get_culture_zones_str, conn);
                                            get_culture_zone.Fill(dataDS, "CultureZones");
                                            conn.Close();
                                            bool set_zone_10 = false;
                                            if (CheckRowsCount(dataDS, "CultureZones"))
                                            {
                                                for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                                {
                                                    j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                                 dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                                    if (dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") set_zone_10 = true;
                                                }
                                            }
                                            if (set_zone_10) j_script += "\n$(\"#CultureZoneCB\").val(\"10\");";
                                        }



                                        j_script += "$(\"#Planned_productivityTB\").val($(\"#PlannedProdTB\").val());\n";
                                        j_script += "$(\"#Do_nTB\").val($(\"#OrganicTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Do_p2o5TB\").val($(\"#OrganicTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Do_k2oTB\").val($(\"#OrganicTotalDoseKTB\").val());\n";
                                        j_script += "$(\"#Dop_nTB\").val($(\"#OldOrganicTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Dop_p2o5TB\").val($(\"#OldOrganicTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Dop_k2oTB\").val($(\"#OldOrganicTotalDoseKTB\").val());\n";
                                        j_script += "$(\"#Dp_nTB\").val($(\"#OldTotalDoseNTB\").val());\n";
                                        j_script += "$(\"#Dp_p2o5TB\").val($(\"#OldTotalDosePTB\").val());\n";
                                        j_script += "$(\"#Dp_k2oTB\").val($(\"#OldTotalDoseKTB\").val());\n";



                                        j_script += ("\n$(\"#TypePropertyCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#TypePropertyCB option[value='0']\").prop('selected', true);");

                                        id_sort_crop_rotation = "0";
                                        j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SortCRCB option[value='0']\").prop('selected', true);");

                                        j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CrossCultureCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#CultureZoneCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureZoneCB option[value='0']\").prop('selected', true);");
                                        /*j_script += ("\n$(\"#OldCultureZoneCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureZoneCB option[value='0']\").prop('selected', true);");*/
                                        j_script += ("\n$(\"#ReproductionTB\").val('');");
                                        j_script += ("\n$(\"#SeedingRateTB\").val('');");
                                        j_script += ("\n$(\"#PlannedProdTB\").val('');");
                                        j_script += ("\n$(\"#SowingDateTB\").val('');");
                                        j_script += ("\n$(\"#ActualProdTB\").val('');");
                                        j_script += ("\n$(\"#HarvestDateTB\").val('');");

                                        j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#BasicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SowingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#DressingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#DateBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#DoseSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#DateSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#DoseDressingFertTB\").val('');");
                                        j_script += ("\n$(\"#DateDressingFertTB\").val('');");

                                        j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#DateOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#IdProtocolTB\").val('');");
                                        j_script += ("\n$(\"#NumberProtocolTB\").val('');");
                                        j_script += ("\n$(\"#NContentTB\").val('');");
                                        j_script += ("\n$(\"#PContentTB\").val('');");
                                        j_script += ("\n$(\"#KContentTB\").val('');");

                                        j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator1CB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_1TB\").val('');");
                                        j_script += ("\n$(\"#DoseAmeliorator1TB\").val('');");
                                        j_script += ("\n$(\"#DateAmeliorator1TB\").val('');");
                                        j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator2CB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_2TB\").val('');");
                                        j_script += ("\n$(\"#DoseAmeliorator2TB\").val('');");
                                        j_script += ("\n$(\"#DateAmeliorator2TB\").val('');");
                                        j_script += ("\n$(\"#FactCaCO3TB\").val('');");

                                        //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                        j_script += "\n$('#TillageT').html('');";
                                        //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                        j_script += "\n$('#PlantProtectionT').html('');";
                                        j_script += "\nCallServer('org_total_dose:0|0|0', 'null');";
                                    }

                                    j_script += "});";
                                    j_script += GetSortCropRotationDescription(id_sort_crop_rotation);
                                }
                            }
                            break;
                        }
                    case "code_single_plot":
                        {
                            if (connection_try)
                            {
                                DataSet dataDS = new DataSet();
                                conn.Open();
                                String get_plots_info = "SELECT * FROM View_Plots_Tree WHERE unique_id_plot = '" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                SqlDataAdapter get_plots = new SqlDataAdapter(get_plots_info, conn);
                                get_plots.Fill(dataDS, "Plots");

                                String get_plot_values_str = "SELECT * FROM Plot WHERE unique_id_plot = '" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                SqlDataAdapter get_plot_values = new SqlDataAdapter(get_plot_values_str, conn);
                                get_plot_values.Fill(dataDS, "Values");

                                String id_plot = dataDS.Tables["Values"].Rows[0]["id_plot"].ToString();
                                String code_plot = dataDS.Tables["Values"].Rows[0]["code_plot"].ToString();

                                String get_agro_info = "SELECT * FROM View_For_ArcGis_Full WHERE code_plot = '" + code_plot + "';";
                                SqlDataAdapter get_agro = new SqlDataAdapter(get_agro_info, conn);
                                get_agro.Fill(dataDS, "Agro");

                                String get_soil_info = "SELECT * FROM View_Soil_From_Plot WHERE code_plot = '" + code_plot + "';";
                                SqlDataAdapter get_soil = new SqlDataAdapter(get_soil_info, conn);
                                get_soil.Fill(dataDS, "Soil");

                                conn.Close();

                                String id_sort_crop_rotation = "0";

                                if (CheckRowsCount(dataDS, "Plots"))
                                {
                                    j_script += "\n$(function () {";
                                    j_script += ("\n$(\"#OrgTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_organization"].ToString() + "');");
                                    j_script += ("\n$(\"#DepTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_department"].ToString() + "');");
                                    j_script += ("\n$(\"#UniqNumberTB\").val('" + dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "');");
                                    j_script += ("\n$(\"#NumberPlotTB\").val('" + dataDS.Tables["Plots"].Rows[0]["number_plot"].ToString() + "');");
                                    j_script += ("\n$(\"#AreaTB\").val('" + dataDS.Tables["Plots"].Rows[0]["area"].ToString() + "');");

                                    if (CheckRowsCount(dataDS, "Agro"))
                                    {
                                        j_script += ("\n$(\"#YearTB\").val('" + dataDS.Tables["Agro"].Rows[0]["year"].ToString() + "');");
                                        j_script += ("\n$(\"#NTB\").val('" + dataDS.Tables["Agro"].Rows[0]["n"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_n_group"].ToString() + "');");
                                        j_script += ("\n$(\"#P2O5TB\").val('" + dataDS.Tables["Agro"].Rows[0]["p2o5"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupP2O5TB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_p2o5_group"].ToString() + "');");
                                        j_script += ("\n$(\"#K2OTB\").val('" + dataDS.Tables["Agro"].Rows[0]["k2o"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupK2OTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_k2o_group"].ToString() + "');");
                                        j_script += ("\n$(\"#pHTB\").val('" + dataDS.Tables["Agro"].Rows[0]["ph_s"].ToString() + "');");
                                        j_script += ("\n$(\"#GrouppHTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ph_s_group"].ToString() + "');");
                                        j_script += ("\n$(\"#HumusTB\").val('" + dataDS.Tables["Agro"].Rows[0]["humus"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupHumusTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_humus_group"].ToString() + "');");
                                        j_script += ("\n$(\"#HATB\").val('" + dataDS.Tables["Agro"].Rows[0]["hydrolytic_acid"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupHATB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ha_group"].ToString() + "');");
                                        j_script += ("\n$(\"#ACTB\").val('" + dataDS.Tables["Agro"].Rows[0]["absorbance_capacity"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupACTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_ac_group"].ToString() + "');");
                                        j_script += ("\n$(\"#TABTB\").val('" + dataDS.Tables["Agro"].Rows[0]["total_absorbed_bases"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupTABTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_tab_group"].ToString() + "');");
                                        j_script += ("\n$(\"#BSTB\").val('" + dataDS.Tables["Agro"].Rows[0]["base_saturation"].ToString() + "');");
                                        j_script += ("\n$(\"#GroupBSTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_bs_group"].ToString() + "');");

                                        j_script += ("\n$(\"#MNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mn"].ToString() + "');");
                                        j_script += ("\n$(\"#CATB\").val('" + dataDS.Tables["Agro"].Rows[0]["ca"].ToString() + "');");
                                        j_script += ("\n$(\"#STB\").val('" + dataDS.Tables["Agro"].Rows[0]["s"].ToString() + "');");
                                        j_script += ("\n$(\"#BTB\").val('" + dataDS.Tables["Agro"].Rows[0]["b"].ToString() + "');");
                                        j_script += ("\n$(\"#MOTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mo"].ToString() + "');");
                                        j_script += ("\n$(\"#CUTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cu"].ToString() + "');");
                                        j_script += ("\n$(\"#ZNTB\").val('" + dataDS.Tables["Agro"].Rows[0]["zn"].ToString() + "');");
                                        j_script += ("\n$(\"#NATB\").val('" + dataDS.Tables["Agro"].Rows[0]["na"].ToString() + "');");
                                        j_script += ("\n$(\"#COTB\").val('" + dataDS.Tables["Agro"].Rows[0]["co"].ToString() + "');");
                                        j_script += ("\n$(\"#MGTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mg"].ToString() + "');");
                                        j_script += ("\n$(\"#FETB\").val('" + dataDS.Tables["Agro"].Rows[0]["fe"].ToString() + "');");
                                        j_script += ("\n$(\"#ALTB\").val('" + dataDS.Tables["Agro"].Rows[0]["al"].ToString() + "');");

                                        j_script += ("\n$(\"#CUHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cu_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#ZNHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["zn_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#CDHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cd_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#PBHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["pb_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#NIHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["ni_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#HGHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["hg_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#ASHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["as_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#MGHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["mg_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#CRHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["cr_hm"].ToString() + "');");
                                        j_script += ("\n$(\"#FHMTB\").val('" + dataDS.Tables["Agro"].Rows[0]["f_hm"].ToString() + "');");

                                        j_script += ("\n$(\"#PriorityCalcifTB\").val('" + dataDS.Tables["Agro"].Rows[0]["title_pc_group"].ToString() + "');");
                                        j_script += ("\n$(\"#pH1TB\").val('" + dataDS.Tables["Agro"].Rows[0]["ph_s"].ToString() + "');");
                                        j_script += ("\n$(\"#HA1TB\").val('" + dataDS.Tables["Agro"].Rows[0]["hydrolytic_acid"].ToString() + "');");

                                        j_script += ("\n$(\"#NumberFieldTB\").val('" + dataDS.Tables["Agro"].Rows[0]["number_field"].ToString() + "');");
                                    }

                                    if (CheckRowsCount(dataDS, "Soil"))
                                    {
                                        j_script += ("\n$(\"#Year2TB\").val('" + dataDS.Tables["Soil"].Rows[0]["year"].ToString() + "');");

                                        j_script += ("\n$(\"#Slope1TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area1"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope2TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area2"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope3TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area3"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope4TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area4"].ToString() + "');");
                                        j_script += ("\n$(\"#Slope5TB\").val('" + dataDS.Tables["Soil"].Rows[0]["area5"].ToString() + "');");
                                        j_script += ("\n$(\"#ExposureTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_exposure"].ToString() + "');");
                                        j_script += ("\n$(\"#ErosionTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_erosion"].ToString() + "');");
                                        j_script += ("\n$(\"#TypeSoilTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_type_soil"].ToString() + "');");
                                        j_script += ("\n$(\"#GradingTB\").val('" + dataDS.Tables["Soil"].Rows[0]["title_grading"].ToString() + "');");
                                    }

                                    //выборка и вывод данных по севообороту для редактирования
                                    //года не обновляем при переключении полей
                                    /*j_script += ("\n$(\"#Year3CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year3CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year3CB option:nth-child(3)\").attr('selected', 'selected');";

                                    j_script += ("\n$(\"#Year4CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year4CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year4CB option:nth-child(3)\").attr('selected', 'selected');";*/

                                    conn.Open();
                                    String get_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" +
                                                           dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND [year] = " + DateTime.Now.Year.ToString() + ";";
                                    SqlDataAdapter get_book = new SqlDataAdapter(get_book_info, conn);
                                    get_book.Fill(dataDS, "Book");
                                    String test_eventArgument = eventArgument;

                                    String get_old_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" +
                                                           dataDS.Tables["Plots"].Rows[0]["unique_id_plot"].ToString() + "' AND [year] = " +
                                                           (DateTime.Now.Year - 1).ToString() + ";";
                                    SqlDataAdapter get_old_book = new SqlDataAdapter(get_old_book_info, conn);
                                    get_old_book.Fill(dataDS, "OldBook");

                                    if (CheckRowsCount(dataDS, "Book"))
                                    {
                                        String get_book_tillage_info = "SELECT * FROM View_History_Book_Tillage WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_tillage = new SqlDataAdapter(get_book_tillage_info, conn);
                                        get_book_tillage.Fill(dataDS, "BookTillage");

                                        String get_book_plant_protection_info = "SELECT * FROM View_History_Book_Plant_Protection WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_plant_protection = new SqlDataAdapter(get_book_plant_protection_info, conn);
                                        get_book_plant_protection.Fill(dataDS, "BookPlantProtection");

                                        String get_book_pests_info = "SELECT * FROM View_History_Book_Pests WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_pests = new SqlDataAdapter(get_book_pests_info, conn);
                                        get_book_pests.Fill(dataDS, "BookPests");

                                        String get_book_diseases_info = "SELECT * FROM View_History_Book_Diseases WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_diseases = new SqlDataAdapter(get_book_diseases_info, conn);
                                        get_book_diseases.Fill(dataDS, "BookDiseases");

                                        String get_book_weeds_info = "SELECT * FROM View_History_Book_Weeds WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_weeds = new SqlDataAdapter(get_book_weeds_info, conn);
                                        get_book_weeds.Fill(dataDS, "BookWeeds");

                                        String get_book_weediness_info = "SELECT * FROM View_History_Book_Weediness WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                        SqlDataAdapter get_book_weediness = new SqlDataAdapter(get_book_weediness_info, conn);
                                        get_book_weediness.Fill(dataDS, "BookWeediness");
                                    }
                                    conn.Close();

                                    if (CheckRowsCount(dataDS, "OldBook"))
                                    {
                                        j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldBasicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldSowingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldDressingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#OldDoseSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#OldDoseDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                        j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldOrganicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                        j_script += ("\n$(\"#OldDateOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");
                                    }
                                    else
                                    {
                                        j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureCB option[value='0']\").prop('selected', true);");

                                        j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldBasicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldSowingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldDressingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDoseSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDoseDressingFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateDressingFertTB\").val('');");

                                        j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldOrganicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#OldDateOrganicFertTB\").val('');");
                                    }

                                    if (CheckRowsCount(dataDS, "Book"))
                                    {
                                        j_script += ("\n$(\"#TypePropertyCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#TypePropertyCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_type_property"].ToString()) + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        id_sort_crop_rotation = NotNull(dataDS.Tables["Book"].Rows[0]["id_sort_crop_rotation"].ToString());
                                        j_script += ("\n$(\"#SortCRCB option[value='" + id_sort_crop_rotation + "']\").prop('selected', true);");

                                        j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");

                                        //загрузка сортов/гибридов
                                        conn.Open();
                                        String get_cross_culture_str = "SELECT * FROM Cross_culture WHERE id_culture = " + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        SqlDataAdapter get_cross_culture = new SqlDataAdapter(get_cross_culture_str, conn);
                                        get_cross_culture.Fill(dataDS, "CrossCulture");
                                        String get_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        SqlDataAdapter get_culture_zone = new SqlDataAdapter(get_culture_zones_str, conn);
                                        get_culture_zone.Fill(dataDS, "CultureZones");
                                        String get_old_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString());
                                        SqlDataAdapter get_old_culture_zone = new SqlDataAdapter(get_old_culture_zones_str, conn);
                                        get_old_culture_zone.Fill(dataDS, "OldCultureZones");
                                        conn.Close();

                                        j_script += "\n$(\"#CrossCultureCB\").empty();";
                                        j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"0\"></option>');");

                                        if (CheckRowsCount(dataDS, "CrossCulture"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CrossCulture"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"" + dataDS.Tables["CrossCulture"].Rows[i]["id_cross_culture"].ToString() + "\">" +
                                                             dataDS.Tables["CrossCulture"].Rows[i]["title_cross_culture"].ToString() + "</option>');");
                                            }
                                        }

                                        j_script += "\n$(\"#CultureZoneCB\").empty();";
                                        j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"0\"></option>');");

                                        if (CheckRowsCount(dataDS, "CultureZones"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                            }
                                        }
                                        /*j_script += "\n$(\"#OldCultureZoneCB\").empty();";
                                        j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"0\"></option>');");

                                        if (dataDS.Tables["OldCultureZones"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dataDS.Tables["OldCultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"" + dataDS.Tables["OldCultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["OldCultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                            }
                                        }*/
                                        //---------------------------------

                                        j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CrossCultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_cross_culture"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#ReproductionTB\").val('" + dataDS.Tables["Book"].Rows[0]["reproduction"].ToString() + "');");
                                        j_script += ("\n$(\"#SeedingRateTB\").val('" + dataDS.Tables["Book"].Rows[0]["seeding_rate"].ToString() + "');");
                                        j_script += ("\n$(\"#PlannedProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["planned_productivity"].ToString() + "');");
                                        j_script += ("\n$(\"#SowingDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["sowing_date"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#ActualProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["actual_productivity"].ToString() + "');");
                                        j_script += ("\n$(\"#HarvestDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["harvest_date"].ToString().Split(' ')[0] + "');");

                                        j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#BasicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SowingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#DressingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#DoseSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#DoseDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                        j_script += ("\n$(\"#DateDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                        if (NotNull(dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString()) == "0")
                                        {
                                            j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                            j_script += ("\n$(\"#OrganicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                        }
                                        else
                                        {
                                            j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                            j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                        }
                                        j_script += ("\n$(\"#DoseOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                        j_script += ("\n$(\"#DateOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#NumberProtocolTB\").val('" + dataDS.Tables["Book"].Rows[0]["number_protocol"].ToString() + "');");
                                        j_script += ("\n$(\"#NContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["n_content"].ToString() + "');");
                                        j_script += ("\n$(\"#PContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["p_content"].ToString() + "');");
                                        j_script += ("\n$(\"#KContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["k_content"].ToString() + "');");

                                        j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator1CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_1"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_1TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_1"].ToString() + "');");
                                        j_script += ("\n$(\"#DoseAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_1"].ToString() + "');");
                                        j_script += ("\n$(\"#DateAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_1"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator2CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_2"].ToString()) + "']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_2TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_2"].ToString() + "');");
                                        j_script += ("\n$(\"#DoseAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_2"].ToString() + "');");
                                        j_script += ("\n$(\"#DateAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_2"].ToString().Split(' ')[0] + "');");
                                        j_script += ("\n$(\"#FactCaCO3TB\").val(GetFactCaCO3());");

                                        //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                        j_script += "\n$('#TillageT').html('');";
                                        if (CheckRowsCount(dataDS, "BookTillage"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookTillage"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#TillageT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookTillage"].Rows[i]["id_type_tillage"].ToString() + "</td><td width=\"250\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["title_type_tillage"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["depth_tillage"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookTillage"].Rows[i]["date_tillage"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-tillage\" href=\"#delete_tillage\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-tillage\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }

                                        //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                        j_script += "\n$('#PlantProtectionT').html('');";
                                        if (CheckRowsCount(dataDS, "BookPlantProtection"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookPlantProtection"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#PlantProtectionT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_type_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_type_drug"].ToString() +
                                                            "</td><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_drug"].ToString() +
                                                            "</td><td width=\"250\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_drug"].ToString() +
                                                            "</td><td width=\"50\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["dose_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["date_plant_protection"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-plant-protection\" href=\"#delete_plant_protection\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-plant-protection\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }

                                        //добавление вредителей
                                        j_script += "\n$('#PestsT').html('');";
                                        if (CheckRowsCount(dataDS, "BookPests"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookPests"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#PestsT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookPests"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["id_pest"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["title_pest"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["count_pests"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookPests"].Rows[i]["date_pest"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-pest\" href=\"#delete_pest\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-pest\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление болезней
                                        j_script += "\n$('#DiseasesT').html('');";
                                        if (CheckRowsCount(dataDS, "BookDiseases"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookDiseases"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#DiseasesT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookDiseases"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["id_disease"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["title_disease"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["percent_disease"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookDiseases"].Rows[i]["date_disease"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-disease\" href=\"#delete_disease\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-disease\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление сорняков
                                        j_script += "\n$('#WeedsT').html('');";
                                        if (CheckRowsCount(dataDS, "BookWeeds"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["BookWeeds"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#WeedsT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookWeeds"].Rows[i]["id_phase"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["title_phase"].ToString() + "</td><td hidden=\"hidden\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["id_weed"].ToString() + "</td><td width=\"150\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["title_weed"].ToString() + "</td><td width=\"50\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["count_weed"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookWeeds"].Rows[i]["date_weed"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-weed\" href=\"#delete_weed\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-weed\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        //добавление засоренности
                                        j_script += "\n$('#WeedinessT').html('');";
                                        if (CheckRowsCount(dataDS, "BookWeediness"))
                                        {

                                            for (int i = 0; i < dataDS.Tables["BookWeediness"].Rows.Count; i++)
                                            {
                                                j_script += "\n$(\"#WeedinessT\").append('<tr><td width=\"100\">" + dataDS.Tables["BookWeediness"].Rows[i]["title_weediness"].ToString() + "</td><td width=\"75\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["weediness"].ToString() + "</td><td width=\"75\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["weediness_percent"].ToString() + "</td><td width=\"100\">" +
                                                            dataDS.Tables["BookWeediness"].Rows[i]["date_weediness"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-weediness\" href=\"#delete_weediness\">Удалить</a></td></tr>');";
                                            }
                                            j_script += "$(\".delete-weediness\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                        //-----------------------------------------------
                                        bool set_zone_10 = false;
                                        if (CheckRowsCount(dataDS, "CultureZones"))
                                        {
                                            for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                            {
                                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                             dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                                if (dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") set_zone_10 = true;
                                            }
                                        }
                                        if (set_zone_10) j_script += "\n$(\"#CultureZoneCB\").val(\"10\");";
                                    }
                                    else
                                    {
                                        j_script += ("\n$(\"#TypePropertyCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#TypePropertyCB option[value='0']\").prop('selected', true);");

                                        id_sort_crop_rotation = "0";
                                        j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SortCRCB option[value='0']\").prop('selected', true);");

                                        j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CrossCultureCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#CultureZoneCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#CultureZoneCB option[value='0']\").prop('selected', true);");
                                        /*j_script += ("\n$(\"#OldCultureZoneCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OldCultureZoneCB option[value='0']\").prop('selected', true);");*/
                                        j_script += ("\n$(\"#ReproductionTB\").val('');");
                                        j_script += ("\n$(\"#SeedingRateTB\").val('');");
                                        j_script += ("\n$(\"#PlannedProdTB\").val('');");
                                        j_script += ("\n$(\"#SowingDateTB\").val('');");
                                        j_script += ("\n$(\"#ActualProdTB\").val('');");
                                        j_script += ("\n$(\"#HarvestDateTB\").val('');");

                                        j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#BasicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#SowingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#DressingFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#DateBasicFertTB\").val('');");
                                        j_script += ("\n$(\"#DoseSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#DateSowingFertTB\").val('');");
                                        j_script += ("\n$(\"#DoseDressingFertTB\").val('');");
                                        j_script += ("\n$(\"#DateDressingFertTB\").val('');");

                                        j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#DoseOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#DateOrganicFertTB\").val('');");
                                        j_script += ("\n$(\"#NumberProtocolTB\").val('');");
                                        j_script += ("\n$(\"#NContentTB\").val('');");

                                        j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator1CB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_1TB\").val('');");
                                        j_script += ("\n$(\"#DoseAmeliorator1TB\").val('');");
                                        j_script += ("\n$(\"#DateAmeliorator1TB\").val('');");
                                        j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#Ameliorator2CB option[value='0']\").prop('selected', true);");
                                        j_script += ("\n$(\"#PercentCaCO3_2TB\").val('');");
                                        j_script += ("\n$(\"#DoseAmeliorator2TB\").val('');");
                                        j_script += ("\n$(\"#DateAmeliorator2TB\").val('');");
                                        j_script += ("\n$(\"#FactCaCO3TB\").val('');");

                                        //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                        j_script += "\n$('#TillageT').html('');";
                                        //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                        j_script += "\n$('#PlantProtectionT').html('');";
                                    }


                                    j_script += "});";
                                    j_script += GetSortCropRotationDescription(id_sort_crop_rotation);
                                    j_script += "\nCallServer('total_dose:' + $(\"#BasicFertCB\").val() + '|' + $(\"#DoseBasicFertTB\").val() + '|' + $(\"#SowingFertCB\").val()" +
                                                " + '|' + $(\"#DoseSowingFertTB\").val() + '|' + $(\"#DressingFertCB\").val() + '|' + $(\"#DoseDressingFertTB\").val(), 'null');";
                                    j_script += "\nCallServer('old_total_dose:' + $(\"#OldBasicFertCB\").val() + '|' + $(\"#OldDoseBasicFertTB\").val() + '|' + $(\"#OldSowingFertCB\").val()" +
                                                  " + '|' + $(\"#OldDoseSowingFertTB\").val() + '|' + $(\"#OldDressingFertCB\").val() + '|' + $(\"#OldDoseDressingFertTB\").val(), 'null');";
                                    j_script += "\nCallServer('org_total_dose:' + $(\"#OrganicFertCB\").val() + '|' + $(\"#DoseOrganicFertTB\").val() + '|' + $(\"#IdProtocolTB\").val(), 'null');";
                                    j_script += "\nCallServer('old_org_total_dose:' + $(\"#OldOrganicFertCB\").val() + '|' + $(\"#OldDoseOrganicFertTB\").val(), 'null');";

                                    DataSet layersDS = new DataSet();

                                    conn.Open();
                                    String get_plots_str = "SELECT [code_plot],[number_plot],[unique_id_plot],[area],[tour],[year],[number_field],[id_slope],[id_exposure],[id_farmland],[code_farmland],[title_farmland],[id_region],[id_organization],[id_department],[id_plot],[id_watering],[code_watering],[title_watering],[id_using_plot],[code_using_plot],[title_using_plot],(geometry_plot.STAsText()) AS plot_geo_json,[number_p2o5_group],[number_k2o_group],[number_ph_s_group],[number_humus_group] FROM View_Plot_Geo WHERE unique_id_plot='" + eventArgument.Split(':')[1].Split('|')[0] + "';";
                                    SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                    plots_geo_data.Fill(layersDS, "Plots");
                                    conn.Close();

                                    j_script = "\nif(vector_plots_single != null) { map.removeLayer(vector_plots_single); }";
                                    j_script += RemoveLayers();

                                    String feature_string = String.Empty,
                                           feature_name = String.Empty,
                                           properties_string = String.Empty;
                                    if (CheckRowsCount(layersDS, "Plots"))
                                    {
                                        j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                   "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                   "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                        id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();
                                        year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                        tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                        code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                        j_script += "\nvar plots_source_single = new ol.source.Vector();\nvar feature;";
                                        for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                        {
                                            feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                            feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                            for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                }
                                                if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                {
                                                    if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                    {
                                                        properties_string += ",";
                                                    }
                                                    properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                }
                                                if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                {
                                                    properties_string += "});";
                                                }
                                            }
                                            j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                            j_script += "\nplots_source_single.addFeature(feature);";
                                        }
                                        j_script += "\nvar count_plots = plots_source_single.getFeatures().length; \nif(count_plots > 0){";
                                        j_script += "\nvector_plots_single = new ol.layer.Vector({source: plots_source_single, maxResolution:100});";

                                        j_script += "\ncurrent_extent = plots_source_single.getExtent();";
                                        j_script += "\nmap.addLayer(vector_plots_single); vector_plots_single.setZIndex(1); map.getView().fit(current_extent, map.getSize());";
                                        j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                        j_script += "\nShowLegend();";
                                        //js_script += "\nvector_plots_single.on('select', function (){if(selectClick.getFeatures().getLength() > 0){var code_plot = selectClick.getFeatures()[0].getProperties().code_plot; CallServer(('code_plot:' + code_plot + '|' + $('#Year3CB').val()), 'null');}});";
                                        /*j_script += ("\nif (selectClick != null){map.on('click', function (e) {" +
                                                     "map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {" +
                                                     "if (feature.getGeometry().getType() == 'Point'){" +
                                                     "CallServer(('soil_point:' + feature.name), 'null');}" +
                                                     "else if (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon'){" +
                                                     "CallServer(('code_plot:' + feature.name + '|' + $('#Year3CB').val()), 'null');}});});}");*/
                                    }

                                    j_script += "\nvar theme = $(\"#MapThemeCB\").val();" +
                                        "\nif($(\"#OrganizationCB\").val() != 0){CallServer(theme, 'null');}" +
                                        "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + theme, 'null');}" +
                                        "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + theme, 'null');}" +
                                        "\nCallServer('legend:' + theme, 'null');";

                                }
                            }
                            break;
                        }
                    case "change_crop_rotation_year":
                        {
                            if (connection_try)
                            {
                                DataSet dataDS = new DataSet();

                                //выборка и вывод данных по севообороту для редактирования
                                conn.Open();
                                String get_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" + eventArgument.Split(':')[1].Split('|')[1] +
                                                        "' AND [year] = " + eventArgument.Split(':')[1].Split('|')[0] + ";";
                                SqlDataAdapter get_book = new SqlDataAdapter(get_book_info, conn);
                                get_book.Fill(dataDS, "Book");

                                String get_old_book_info = "SELECT * FROM View_History_Book WHERE unique_id_plot = '" +
                                                       eventArgument.Split(':')[1].Split('|')[1] + "' AND [year] = " +
                                                       (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1).ToString() + ";";
                                SqlDataAdapter get_old_book = new SqlDataAdapter(get_old_book_info, conn);
                                get_old_book.Fill(dataDS, "OldBook");

                                String get_culture_zones_str = "SELECT * FROM Loss_zones";
                                SqlDataAdapter get_culture_zone = new SqlDataAdapter(get_culture_zones_str, conn);
                                get_culture_zone.Fill(dataDS, "CultureZones");

                                String id_sort_crop_rotation = "0";

                                j_script += ("\n$(\"#DosesCT\").css('display', 'none');");
                                if (DateTime.Now.Year == (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[0]) - 1))
                                    j_script += ("\n$(\"#DosesCT\").css('display', 'inline');");

                                if (CheckRowsCount(dataDS, "Book"))
                                {
                                    String get_book_tillage_info = "SELECT * FROM View_History_Book_Tillage WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                    SqlDataAdapter get_book_tillage = new SqlDataAdapter(get_book_tillage_info, conn);
                                    get_book_tillage.Fill(dataDS, "BookTillage");

                                    String get_book_plant_protection_info = "SELECT * FROM View_History_Book_Plant_Protection WHERE id_history_book_field = " + dataDS.Tables["Book"].Rows[0]["id_history_book_field"].ToString() + ";";
                                    SqlDataAdapter get_book_plant_protection = new SqlDataAdapter(get_book_plant_protection_info, conn);
                                    get_book_plant_protection.Fill(dataDS, "BookPlantProtection");
                                }
                                conn.Close();

                                j_script += "\n$(function () {";

                                if (CheckRowsCount(dataDS, "OldBook"))
                                {
                                    j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldCultureCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");

                                    j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldBasicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldSowingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldDressingFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#OldDateBasicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#OldDoseSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#OldDateSowingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#OldDoseDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#OldDateDressingFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                    j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldOrganicFertCB option[value='" + NotNull(dataDS.Tables["OldBook"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                    j_script += ("\n$(\"#OldDateOrganicFertTB\").val('" + dataDS.Tables["OldBook"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");
                                }
                                else
                                {
                                    j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldCultureCB option[value='0']\").prop('selected', true);");

                                    j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldBasicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldSowingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldDressingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDoseSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDoseDressingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateDressingFertTB\").val('');");

                                    j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldOrganicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateOrganicFertTB\").val('');");
                                }

                                if (CheckRowsCount(dataDS, "Book"))
                                {
                                    j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    id_sort_crop_rotation = NotNull(dataDS.Tables["Book"].Rows[0]["id_sort_crop_rotation"].ToString());
                                    j_script += ("\n$(\"#SortCRCB option[value='" + id_sort_crop_rotation + "']\").prop('selected', true);");

                                    j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_culture"].ToString()) + "']\").prop('selected', true);");

                                    j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CrossCultureCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_cross_culture"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#ReproductionTB\").val('" + dataDS.Tables["Book"].Rows[0]["reproduction"].ToString() + "');");
                                    j_script += ("\n$(\"#SeedingRateTB\").val('" + dataDS.Tables["Book"].Rows[0]["seeding_rate"].ToString() + "');");
                                    j_script += ("\n$(\"#PlannedProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["planned_productivity"].ToString() + "');");
                                    j_script += ("\n$(\"#SowingDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["sowing_date"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#ActualProdTB\").val('" + dataDS.Tables["Book"].Rows[0]["actual_productivity"].ToString() + "');");
                                    j_script += ("\n$(\"#HarvestDateTB\").val('" + dataDS.Tables["Book"].Rows[0]["harvest_date"].ToString().Split(' ')[0] + "');");

                                    j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#BasicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_basic_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#SowingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_sowing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#DressingFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_dressing_fertilization"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DoseBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_basic_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#DateBasicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_basic_fertilization"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#DoseSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_sowing_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#DateSowingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_sowing_fertilization"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#DoseDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_dressing_fertilization"].ToString() + "');");
                                    j_script += ("\n$(\"#DateDressingFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_dressing_fertilization"].ToString().Split(' ')[0] + "');");

                                    if (NotNull(dataDS.Tables["Book"].Rows[0]["id_protocol"].ToString()) == "0")
                                    {
                                        j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OrganicFertCB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_organic_fertilizer"].ToString()) + "']\").prop('selected', true);");
                                    }
                                    else
                                    {
                                        j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                        j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                    }
                                    j_script += ("\n$(\"#DoseOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_organic_fertilizer"].ToString() + "');");
                                    j_script += ("\n$(\"#DateOrganicFertTB\").val('" + dataDS.Tables["Book"].Rows[0]["date_organic_fertilizer"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#NumberProtocolTB\").val('" + dataDS.Tables["Book"].Rows[0]["number_protocol"].ToString() + "');");
                                    j_script += ("\n$(\"#NContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["n_content"].ToString() + "');");
                                    j_script += ("\n$(\"#PContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["p_content"].ToString() + "');");
                                    j_script += ("\n$(\"#KContentTB\").val('" + dataDS.Tables["Book"].Rows[0]["k_content"].ToString() + "');");

                                    j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator1CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_1"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_1TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_1"].ToString() + "');");
                                    j_script += ("\n$(\"#DoseAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_1"].ToString() + "');");
                                    j_script += ("\n$(\"#DateAmeliorator1TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_1"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator2CB option[value='" + NotNull(dataDS.Tables["Book"].Rows[0]["id_ameliorator_2"].ToString()) + "']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_2TB\").val('" + dataDS.Tables["Book"].Rows[0]["percent_caco3_2"].ToString() + "');");
                                    j_script += ("\n$(\"#DoseAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["dose_ameliorator_2"].ToString() + "');");
                                    j_script += ("\n$(\"#DateAmeliorator2TB\").val('" + dataDS.Tables["Book"].Rows[0]["date_ameliorator_2"].ToString().Split(' ')[0] + "');");
                                    j_script += ("\n$(\"#FactCaCO3TB\").val(GetFactCaCO3());");

                                    //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                    j_script += "\n$('#TillageT').html('');";
                                    if (CheckRowsCount(dataDS, "BookTillage"))
                                    {
                                        for (int i = 0; i < dataDS.Tables["BookTillage"].Rows.Count; i++)
                                        {
                                            j_script += "\n$(\"#TillageT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookTillage"].Rows[i]["id_type_tillage"].ToString() + "</td><td width=\"250\">" +
                                                        dataDS.Tables["BookTillage"].Rows[i]["title_type_tillage"].ToString() + "</td><td width=\"100\">" +
                                                        dataDS.Tables["BookTillage"].Rows[i]["depth_tillage"].ToString() + "</td><td width=\"100\">" +
                                                        dataDS.Tables["BookTillage"].Rows[i]["date_tillage"].ToString().Split(' ')[0] +
                                                        "</td><td align=\"center\" width=\"100\"><a class=\"delete-tillage\" href=\"#delete_tillage\">Удалить</a></td></tr>');";
                                            j_script += "$(\".delete-tillage\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                    }

                                    //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                    j_script += "\n$('#PlantProtectionT').html('');";
                                    if (CheckRowsCount(dataDS, "BookPlantProtection"))
                                    {

                                        for (int i = 0; i < dataDS.Tables["BookPlantProtection"].Rows.Count; i++)
                                        {
                                            j_script += "\n$(\"#PlantProtectionT\").append('<tr><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_type_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_type_drug"].ToString() +
                                                            "</td><td hidden=\"hidden\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["id_drug"].ToString() +
                                                            "</td><td width=\"250\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["title_drug"].ToString() +
                                                            "</td><td width=\"50\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["dose_drug"].ToString() +
                                                            "</td><td width=\"100\">" + dataDS.Tables["BookPlantProtection"].Rows[i]["date_plant_protection"].ToString().Split(' ')[0] +
                                                            "</td><td align=\"center\" width=\"100\"><a class=\"delete-plant-protection\" href=\"#delete_plant_protection\">Удалить</a></td></tr>');";
                                            j_script += "$(\".delete-plant-protection\").button().click(function (event) {$($(this).parents().get(1)).remove();});";
                                        }
                                    }
                                }
                                else
                                {
                                    id_sort_crop_rotation = "0";
                                    j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#SortCRCB option[value='0']\").prop('selected', true);");

                                    j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CultureCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CrossCultureCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#ReproductionTB\").val('');");
                                    j_script += ("\n$(\"#SeedingRateTB\").val('');");
                                    j_script += ("\n$(\"#PlannedProdTB\").val('');");
                                    j_script += ("\n$(\"#SowingDateTB\").val('');");
                                    j_script += ("\n$(\"#ActualProdTB\").val('');");
                                    j_script += ("\n$(\"#HarvestDateTB\").val('');");

                                    j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#BasicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#SowingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#DressingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DoseBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#DateBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#DoseSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#DateSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#DoseDressingFertTB\").val('');");
                                    j_script += ("\n$(\"#DateDressingFertTB\").val('');");

                                    j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DoseOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#DateOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#NumberProtocolTB\").val('');");
                                    j_script += ("\n$(\"#NContentTB\").val('');");

                                    j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator1CB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_1TB\").val('');");
                                    j_script += ("\n$(\"#DoseAmeliorator1TB\").val('');");
                                    j_script += ("\n$(\"#DateAmeliorator1TB\").val('');");
                                    j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator2CB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_2TB\").val('');");
                                    j_script += ("\n$(\"#DoseAmeliorator2TB\").val('');");
                                    j_script += ("\n$(\"#DateAmeliorator2TB\").val('');");
                                    j_script += ("\n$(\"#FactCaCO3TB\").val('');");

                                    //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                    j_script += "\n$('#TillageT').html('');";
                                    //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                    j_script += "\n$('#PlantProtectionT').html('');";
                                }
                                j_script += "\n$(\"#CultureZoneCB\").empty();";
                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"0\"></option>');");

                                bool set_zone_10 = false;
                                if (CheckRowsCount(dataDS, "CultureZones"))
                                {
                                    for (int i = 0; i < dataDS.Tables["CultureZones"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                     dataDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                        if (dataDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") set_zone_10 = true;
                                    }
                                }
                                if (set_zone_10) j_script += "\n$(\"#CultureZoneCB\").val(\"10\");";
                                j_script += "});";
                                j_script += GetSortCropRotationDescription(id_sort_crop_rotation);
                                j_script += "\nCallServer('total_dose:' + $(\"#BasicFertCB\").val() + '|' + $(\"#DoseBasicFertTB\").val() + '|' + $(\"#SowingFertCB\").val()" +
                                                " + '|' + $(\"#DoseSowingFertTB\").val() + '|' + $(\"#DressingFertCB\").val() + '|' + $(\"#DoseDressingFertTB\").val(), 'null');";
                                j_script += "\nCallServer('old_total_dose:' + $(\"#OldBasicFertCB\").val() + '|' + $(\"#OldDoseBasicFertTB\").val() + '|' + $(\"#OldSowingFertCB\").val()" +
                                              " + '|' + $(\"#OldDoseSowingFertTB\").val() + '|' + $(\"#OldDressingFertCB\").val() + '|' + $(\"#OldDoseDressingFertTB\").val(), 'null');";
                                j_script += "\nCallServer('org_total_dose:' + $(\"#OrganicFertCB\").val() + '|' + $(\"#DoseOrganicFertTB\").val() + '|' + $(\"#IdProtocolTB\").val(), 'null');";
                                j_script += "\nCallServer('old_org_total_dose:' + $(\"#OldOrganicFertCB\").val() + '|' + $(\"#OldDoseOrganicFertTB\").val(), 'null');";
                            }
                            break;
                        }
                    case "selected_plots":
                        {//CallServer('selected_plots:' + unique_id_plots + '|' + numbers_plots + '|' + sum_area, 'null');
                            if (connection_try)
                            {
                                DataSet dataDS = new DataSet();

                                conn.Open();
                                String get_plots_info = "SELECT * FROM View_Plots_Tree WHERE unique_id_plot = '" + eventArgument.Split(':')[1].Split('|')[0].Split(',')[0] + "';";
                                SqlDataAdapter get_plots = new SqlDataAdapter(get_plots_info, conn);
                                get_plots.Fill(dataDS, "Plots");
                                conn.Close();

                                //String id_sort_crop_rotation = "0";

                                if (CheckRowsCount(dataDS, "Plots"))
                                {
                                    j_script += "\n$(function () {";
                                    j_script += ("\n$(\"#OrgTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_organization"].ToString() + "');");
                                    j_script += ("\n$(\"#DepTB\").val('" + dataDS.Tables["Plots"].Rows[0]["title_department"].ToString() + "');");
                                    j_script += ("\n$(\"#UniqNumberTB\").val('" + eventArgument.Split(':')[1].Split('|')[0] + "');");
                                    j_script += ("\n$(\"#NumberPlotTB\").val('" + eventArgument.Split(':')[1].Split('|')[1] + "');");
                                    j_script += ("\n$(\"#AreaTB\").val('" + eventArgument.Split(':')[1].Split('|')[2] + "');");

                                    j_script += ("\n$(\"#YearTB\").val('');");
                                    j_script += ("\n$(\"#NTB\").val('');");
                                    j_script += ("\n$(\"#GroupNTB\").val('');");
                                    j_script += ("\n$(\"#P2O5TB\").val('');");
                                    j_script += ("\n$(\"#GroupP2O5TB\").val('');");
                                    j_script += ("\n$(\"#K2OTB\").val('');");
                                    j_script += ("\n$(\"#GroupK2OTB\").val('');");
                                    j_script += ("\n$(\"#pHTB\").val('');");
                                    j_script += ("\n$(\"#GrouppHTB\").val('');");
                                    j_script += ("\n$(\"#HumusTB\").val('');");
                                    j_script += ("\n$(\"#GroupHumusTB\").val('');");
                                    j_script += ("\n$(\"#HATB\").val('');");
                                    j_script += ("\n$(\"#GroupHATB\").val('');");
                                    j_script += ("\n$(\"#ACTB\").val('');");
                                    j_script += ("\n$(\"#GroupACTB\").val('');");
                                    j_script += ("\n$(\"#TABTB\").val('');");
                                    j_script += ("\n$(\"#GroupTABTB\").val('');");
                                    j_script += ("\n$(\"#BSTB\").val('');");
                                    j_script += ("\n$(\"#GroupBSTB\").val('');");

                                    j_script += ("\n$(\"#MNTB\").val('');");
                                    j_script += ("\n$(\"#CATB\").val('');");
                                    j_script += ("\n$(\"#STB\").val('');");
                                    j_script += ("\n$(\"#BTB\").val('');");
                                    j_script += ("\n$(\"#MOTB\").val('');");
                                    j_script += ("\n$(\"#CUTB\").val('');");
                                    j_script += ("\n$(\"#ZNTB\").val('');");
                                    j_script += ("\n$(\"#NATB\").val('');");
                                    j_script += ("\n$(\"#COTB\").val('');");
                                    j_script += ("\n$(\"#MGTB\").val('');");
                                    j_script += ("\n$(\"#FETB\").val('');");
                                    j_script += ("\n$(\"#ALTB\").val('');");

                                    j_script += ("\n$(\"#CUHMTB\").val('');");
                                    j_script += ("\n$(\"#ZNHMTB\").val('');");
                                    j_script += ("\n$(\"#CDHMTB\").val('');");
                                    j_script += ("\n$(\"#PBHMTB\").val('');");
                                    j_script += ("\n$(\"#NIHMTB\").val('');");
                                    j_script += ("\n$(\"#HGHMTB\").val('');");
                                    j_script += ("\n$(\"#ASHMTB\").val('');");
                                    j_script += ("\n$(\"#MGHMTB\").val('');");
                                    j_script += ("\n$(\"#CRHMTB\").val('');");
                                    j_script += ("\n$(\"#FHMTB\").val('');");

                                    j_script += ("\n$(\"#PriorityCalcifTB\").val('');");
                                    j_script += ("\n$(\"#pH1TB\").val('');");
                                    j_script += ("\n$(\"#HA1TB\").val('');");

                                    j_script += ("\n$(\"#NumberFieldTB\").val('');");

                                    j_script += ("\n$(\"#Year2TB\").val('');");
                                    j_script += ("\n$(\"#Slope1TB\").val('');");
                                    j_script += ("\n$(\"#Slope2TB\").val('');");
                                    j_script += ("\n$(\"#Slope3TB\").val('');");
                                    j_script += ("\n$(\"#Slope4TB\").val('');");
                                    j_script += ("\n$(\"#Slope5TB\").val('');");
                                    j_script += ("\n$(\"#ExposureTB\").val('');");
                                    j_script += ("\n$(\"#ErosionTB\").val('');");
                                    j_script += ("\n$(\"#TypeSoilTB\").val('');");
                                    j_script += ("\n$(\"#GradingTB\").val('');");

                                    //выборка и вывод данных по севообороту для редактирования
                                    /*j_script += ("\n$(\"#Year3CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year3CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year3CB option:nth-child(3)\").attr('selected', 'selected');";

                                    j_script += ("\n$(\"#Year4CB\").empty();");
                                    for (int i = -2; i < 8; i++)
                                    {
                                        Int32 year = Convert.ToInt32(dataDS.Tables["Plots"].Rows[0]["year"].ToString()) + i;
                                        j_script += ("\n$(\"#Year4CB\").append('<option value=\"" + year.ToString() + "\">" + year.ToString() + "</option>');");
                                    }
                                    j_script += "\n$(\"#Year4CB option:nth-child(3)\").attr('selected', 'selected');";*/

                                    /*id_sort_crop_rotation = "0";
                                    j_script += ("\n$(\"#SortCRCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#SortCRCB option[value='0']\").prop('selected', true);");

                                    j_script += ("\n$(\"#CultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CultureCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#CrossCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#CrossCultureCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#ReproductionTB\").val('');");
                                    j_script += ("\n$(\"#SeedingRateTB\").val('');");
                                    j_script += ("\n$(\"#PlannedProdTB\").val('');");
                                    j_script += ("\n$(\"#SowingDateTB\").val('');");
                                    j_script += ("\n$(\"#ActualProdTB\").val('');");
                                    j_script += ("\n$(\"#HarvestDateTB\").val('');");

                                    j_script += ("\n$(\"#BasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#BasicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#SowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#SowingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#DressingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DoseBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#DateBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#DoseSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#DateSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#DoseDressingFertTB\").val('');");
                                    j_script += ("\n$(\"#DateDressingFertTB\").val('');");

                                    j_script += ("\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#DoseOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#DateOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#NumberProtocolTB\").val('');");
                                    j_script += ("\n$(\"#NContentTB\").val('');");

                                    j_script += ("\n$(\"#Ameliorator1CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator1CB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_1TB\").val('');");
                                    j_script += ("\n$(\"#DoseAmeliorator1TB\").val('');");
                                    j_script += ("\n$(\"#DateAmeliorator1TB\").val('');");
                                    j_script += ("\n$(\"#Ameliorator2CB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#Ameliorator2CB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#PercentCaCO3_2TB\").val('');");
                                    j_script += ("\n$(\"#DoseAmeliorator2TB\").val('');");
                                    j_script += ("\n$(\"#DateAmeliorator2TB\").val('');");
                                    j_script += ("\n$(\"#FactCaCO3TB\").val('');");

                                    //j_script += "\nfor (var i = 3; i < $('#TillageT tr').size() ; i++) {$('#TillageT tr').eq(i).remove();};";
                                    j_script += "\n$('#TillageT').html('');";
                                    //j_script += "\nfor (var i = 3; i < $('#PlantProtectionT tr').size() ; i++) {$('#PlantProtectionT tr').eq(i).remove();}";
                                    j_script += "\n$('#PlantProtectionT').html('');";

                                    j_script += ("\n$(\"#OldCultureCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldCultureCB option[value='0']\").prop('selected', true);");

                                    j_script += ("\n$(\"#OldBasicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldBasicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldSowingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldSowingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDressingFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldDressingFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateBasicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDoseSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateSowingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDoseDressingFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateDressingFertTB\").val('');");

                                    j_script += ("\n$(\"#OldOrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});");
                                    j_script += ("\n$(\"#OldOrganicFertCB option[value='0']\").prop('selected', true);");
                                    j_script += ("\n$(\"#OldDoseOrganicFertTB\").val('');");
                                    j_script += ("\n$(\"#OldDateOrganicFertTB\").val('');");*/

                                    j_script += "});";
                                    //j_script += GetSortCropRotationDescription(id_sort_crop_rotation);
                                }
                            }
                            break;
                        }
                    case "select_protocol_window":
                        {
                            if (connection_try)
                            {
                                String territory = eventArgument.Split(':')[1].Split('|')[0];
                                String region = eventArgument.Split(':')[1].Split('|')[1];

                                conn.Open();
                                DataSet tablesDS = new DataSet();

                                String get_regions_str = "SELECT * FROM Region WHERE id_territory = " + territory + " ORDER BY title_region";
                                SqlDataAdapter get_regions = new SqlDataAdapter(get_regions_str, conn);
                                get_regions.Fill(tablesDS, "Regions");

                                String get_farmorgs_str = "SELECT * FROM Farm_organization ORDER BY title_farm_organization";
                                SqlDataAdapter get_farmorgs = new SqlDataAdapter(get_farmorgs_str, conn);
                                get_farmorgs.Fill(tablesDS, "FarmOrgs");
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#SelectProtocolRegionCB\").empty();";
                                j_script += "\n$(\"#SelectProtocolFarmOrgCB\").empty();";
                                j_script += "\n$(\"#SelectProtocolFarmCB\").empty();";
                                j_script += "\n$(\"#SelectProtocolLagoonCB\").empty();";
                                j_script += "\n$(\"#SelectProtocolCB\").empty();";
                                j_script += ("\n$(\"#SelectProtocolRegionCB\").append('<option value=\"0\"></option>');");
                                j_script += ("\n$(\"#SelectProtocolFarmOrgCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "Regions"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["Regions"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectProtocolRegionCB\").append('<option value=\"" + tablesDS.Tables["Regions"].Rows[i]["id_region"].ToString() + "\">" +
                                                     tablesDS.Tables["Regions"].Rows[i]["title_region"].ToString() + "</option>');");
                                    }
                                }

                                if (CheckRowsCount(tablesDS, "FarmOrgs"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["FarmOrgs"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectProtocolFarmOrgCB\").append('<option value=\"" + tablesDS.Tables["FarmOrgs"].Rows[i]["id_farm_organization"].ToString() + "\">" +
                                                     tablesDS.Tables["FarmOrgs"].Rows[i]["title_farm_organization"].ToString() + "</option>');");
                                    }
                                }

                                j_script += "\n$(\"#SelectProtocolRegionCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                j_script += "\nif(" + region + "!='0') { $(\"#SelectProtocolRegionCB option[value='" + region + "']\").prop('selected', true);}"; //"CallServer('select_protocol_region:' + '" + region + "', 'null'); }";
                                j_script += "});";
                            }
                            break;
                        }
                    case "select_protocol_farm_organization":
                        {
                            if (connection_try)
                            {
                                String farm_organization = eventArgument.Split(':')[1].Split('|')[0];
                                String region = eventArgument.Split(':')[1].Split('|')[1];

                                DataSet tablesDS = new DataSet();
                                String get_farms_str = "";

                                if (farm_organization != "" && farm_organization != null && region != "" && region != null)
                                {
                                    get_farms_str = "SELECT * FROM Geographic_farm WHERE id_farm_organization=" + farm_organization + " AND id_region=" + region + " ORDER BY title_farm;";
                                }
                                else if (farm_organization != "" && farm_organization != null && (region == "" || region == null))
                                {
                                    get_farms_str = "SELECT * FROM Geographic_farm WHERE id_farm_organization=" + farm_organization + " ORDER BY title_farm;";
                                }
                                else if ((farm_organization == "" || farm_organization == null) && region != "" && region != null)
                                {
                                    get_farms_str = "SELECT * FROM Geographic_farm WHERE id_region=" + region + " ORDER BY title_farm;";
                                }

                                conn.Open();
                                SqlDataAdapter get_farms = new SqlDataAdapter(get_farms_str, conn);
                                get_farms.Fill(tablesDS, "Farms");
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#SelectProtocolFarmCB\").empty();";
                                j_script += ("\n$(\"#SelectProtocolFarmCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "Farms"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["Farms"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectProtocolFarmCB\").append('<option value=\"" + tablesDS.Tables["Farms"].Rows[i]["id_farm"].ToString() + "\">" +
                                                     tablesDS.Tables["Farms"].Rows[i]["title_farm"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }

                    case "select_protocol_farm":
                        {
                            if (connection_try)
                            {
                                String farm = eventArgument.Split(':')[1];
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_lagoons_str = "SELECT * FROM Lagoons WHERE id_farm = " + farm + " ORDER BY lagoon_number";
                                SqlDataAdapter get_lagoons = new SqlDataAdapter(get_lagoons_str, conn);
                                get_lagoons.Fill(tablesDS, "Lagoons");
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#SelectProtocolLagoonCB\").empty();";
                                j_script += ("\n$(\"#SelectProtocolLagoonCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "Lagoons"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["Lagoons"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectProtocolLagoonCB\").append('<option value=\"" + tablesDS.Tables["Lagoons"].Rows[i]["id_lagoon"].ToString() + "\">" +
                                                     tablesDS.Tables["Lagoons"].Rows[i]["lagoon_number"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }
                    case "select_protocol_lagoon":
                        {
                            if (connection_try)
                            {
                                String lagoon = eventArgument.Split(':')[1];
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_protocols_str = "SELECT * FROM Protocols WHERE id_lagoon = " + lagoon + " ORDER BY number_protocol";
                                SqlDataAdapter get_protocols = new SqlDataAdapter(get_protocols_str, conn);
                                get_protocols.Fill(tablesDS, "Protocols");
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#SelectProtocolCB\").empty();";
                                j_script += ("\n$(\"#SelectProtocolCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "Protocols"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["Protocols"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectProtocolCB\").append('<option value=\"" + tablesDS.Tables["Protocols"].Rows[i]["id_protocol"].ToString() + "\">" +
                                                     tablesDS.Tables["Protocols"].Rows[i]["number_protocol"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }
                    case "selected_protocol_significatives":
                        {
                            if (connection_try)
                            {
                                String id_protocol = eventArgument.Split(':')[1];
                                DataSet tablesDS = new DataSet();
                                /*String get_results_str = "SELECT Results_Protocol.value,Significative_Fertilizer.id_significative_fertilizer,title_significative_fertilizer"
                                    + " FROM Results_Protocol,Significative_Fertilizer WHERE id_protocol = " + id_protocol 
                                    + " AND Results_Protocol.id_significative_fertilizer=Significative_Fertilizer.id_significative_fertilizer" 
                                    + " AND (Significative_Fertilizer.id_significative_fertilizer=4"
                                        + " OR Significative_Fertilizer.id_significative_fertilizer=7"
                                        + " OR Significative_Fertilizer.id_significative_fertilizer=9) "
                                        + " ORDER BY Significative_Fertilizer.id_significative_fertilizer;";*/

                                String get_results_str = "EXEC dbo.GetResultsNPK " + id_protocol;

                                String get_protocol_str = "SELECT * FROM View_Protocols WHERE id_protocol = " + id_protocol;

                                conn.Open();
                                SqlDataAdapter get_results = new SqlDataAdapter(get_results_str, conn);
                                get_results.Fill(tablesDS, "Results");

                                SqlDataAdapter get_protocol = new SqlDataAdapter(get_protocol_str, conn);
                                get_protocol.Fill(tablesDS, "Protocol");
                                conn.Close();

                                j_script = "";

                                if (CheckRowsCount(tablesDS, "Protocol"))
                                {
                                    j_script += "\n$(\"#OrganicFertCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                    //j_script += "\n$(\"#OrganicFertCB option[value='0']\").prop('selected', true);";
                                    j_script += "\n$(\"#OrganicFertCB option[value='" + tablesDS.Tables["Protocol"].Rows[0]["id_fertilizer"].ToString() + "']\").prop('selected', true);";

                                    j_script += "\n$(\"#IdProtocolTB\").val('" + tablesDS.Tables["Protocol"].Rows[0]["id_protocol"].ToString() + "');";
                                    j_script += "\n$(\"#NumberProtocolTB\").val('" + tablesDS.Tables["Protocol"].Rows[0]["number_protocol"].ToString() + "');";

                                    if (CheckRowsCount(tablesDS, "Results"))
                                    {
                                        String n = "0";
                                        String p = "0";
                                        String k = "0";

                                        if (tablesDS.Tables["Results"].Rows.Count >= 1)
                                            n = tablesDS.Tables["Results"].Rows[0]["value"].ToString();

                                        if (tablesDS.Tables["Results"].Rows.Count >= 2)
                                            p = tablesDS.Tables["Results"].Rows[1]["value"].ToString();

                                        if (tablesDS.Tables["Results"].Rows.Count == 3)
                                            k = tablesDS.Tables["Results"].Rows[2]["value"].ToString();


                                        j_script += "\n$(\"#NContentTB\").val('" + n + "');";
                                        j_script += "\n$(\"#PContentTB\").val('" + p + "');";
                                        j_script += "\n$(\"#KContentTB\").val('" + k + "');";

                                        j_script += "\nif($(\"#DoseOrganicFertTB\").val() != 0 && $(\"#DoseOrganicFertTB\").val() != '' && $(\"#DoseOrganicFertTB\").val() != null) {";
                                        // % элемента / 100% и * на 1000кг
                                        j_script += "\nvar n_val = $(\"#DoseOrganicFertTB\").val() * 10 * " + n.Replace(',', '.') + ";  $(\"#OrganicTotalDoseNTB\").val(n_val);";
                                        j_script += "\nvar p_val = $(\"#DoseOrganicFertTB\").val() * 10 * " + p.Replace(',', '.') + ";  $(\"#OrganicTotalDosePTB\").val(p_val);";
                                        j_script += "\nvar k_val = $(\"#DoseOrganicFertTB\").val() * 10 * " + k.Replace(',', '.') + ";  $(\"#OrganicTotalDoseKTB\").val(k_val);";
                                        j_script += "\n};";

                                    }
                                }
                                j_script += "\n$('#SelectProtocolW').dialog('close');";
                            }
                            break;
                        }
                    case "get_lagoons_table":
                        {
                            if (connection_try)
                            {
                                String id_farm = eventArgument.Split(':')[1];
                                DataSet tablesDS = new DataSet();
                                String get_lagoons_str = "SELECT * FROM Lagoons WHERE id_farm=" + id_farm;

                                conn.Open();
                                SqlDataAdapter get_lagoons = new SqlDataAdapter(get_lagoons_str, conn);
                                get_lagoons.Fill(tablesDS, "Lagoons");
                                conn.Close();
                                if (tablesDS.Tables["Lagoons"].Rows.Count > 0)
                                {
                                    j_script += "\nvar content = '<table width=\"500\" border=\"1\" >' + " +
                                                                               "'<tr><td width=\"200\" align=\"center\">№ Лагуны</td><td width=\"300\" align=\"center\">Объем</td><td></td></tr><' + ";
                                    for (int i = 0; i < tablesDS.Tables["Lagoons"].Rows.Count; ++i)
                                        j_script += "'<tr><td align=\"center\">Лагуна №" + tablesDS.Tables["Lagoons"].Rows[i]["lagoon_number"].ToString() + "</td><td align=\"center\">" + tablesDS.Tables["Lagoons"].Rows[i]["lagoon_volume"].ToString() + " куб. м.</td>" +
                                            "<td><a href=\"#\" onClick=\"SP(" + tablesDS.Tables["Lagoons"].Rows[i]["id_lagoon"].ToString() + ");\">Показать протоколы</a></td></tr>' + ";
                                    j_script += "'</table>';";
                                    j_script += "\ndocument.getElementById('PopupW').innerHTML += content;";
                                }
                            }
                            break;
                        }
                    case "get_protocols_table":
                        {
                            if (connection_try)
                            {
                                String id_lagoon = eventArgument.Split(':')[1];
                                DataSet tablesDS = new DataSet();
                                String get_lagoons_str = "SELECT * FROM Protocols WHERE id_lagoon=" + id_lagoon;

                                conn.Open();
                                SqlDataAdapter get_lagoons = new SqlDataAdapter(get_lagoons_str, conn);
                                get_lagoons.Fill(tablesDS, "Protocols");
                                conn.Close();
                                j_script += "\nbackup_content_lagoon=document.getElementById('PopupW').innerHTML;";
                                if (tablesDS.Tables["Protocols"].Rows.Count > 0)
                                {
                                    j_script += "\nvar content = '<a href=\"#\" onClick=\"BackupLagoonContent();\">Назад</a>'";
                                    j_script += "\ncontent += '<table width=\"500\" border=\"1\" >' + " +
                                                "'<tr><td width=\"200\" align=\"center\">Протокол</td><td width=\"300\" align=\"center\"></td><td align=\"center\">Дата</td><td></td><td></td</tr><' + ";
                                    for (int i = 0; i < tablesDS.Tables["Protocols"].Rows.Count; i++)
                                        j_script += "'<tr><td align=\"center\">Протокол " + tablesDS.Tables["Protocols"].Rows[i]["number_protocol"].ToString() + "</td><td align=\"center\">" + tablesDS.Tables["Protocols"].Rows[i]["date_input"].ToString() + "</td>" +
                                            "<td><a href=\"#\" onClick=\"ShowFertilizerProtocolReportById(" + tablesDS.Tables["Protocols"].Rows[i]["id_protocol"].ToString() + ");\"><div class=\"img-button img-button-show-protocol\"></div></a></td>" +
                                            "<td><a href=\"#\" onClick=\"ShowReportOrganicFertilizerSurplus(" + tablesDS.Tables["Protocols"].Rows[i]["id_protocol"].ToString() + ");\">Вывоз и остаток</div></a></td></tr>' + ";
                                    j_script += "'</table>';";
                                    j_script += "\ndocument.getElementById('PopupW').innerHTML = ''; \ndocument.getElementById('PopupW').innerHTML = content;";
                                }
                            }
                            break;
                        }
                    case "change_type_drugs":
                        {
                            if (connection_try)
                            {
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_drugs_str = "SELECT id_drug, title_drug FROM Drugs WHERE id_type_drug = " + eventArgument.Split(':')[1] + " ORDER BY title_drug";
                                SqlDataAdapter get_drugs = new SqlDataAdapter(get_drugs_str, conn);
                                get_drugs.Fill(tablesDS, "Drugs");
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#DrugCB\").empty();";
                                j_script += ("\n$(\"#DrugCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "Drugs"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["Drugs"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#DrugCB\").append('<option value=\"" + tablesDS.Tables["Drugs"].Rows[i]["id_drug"].ToString() + "\">" +
                                                     tablesDS.Tables["Drugs"].Rows[i]["title_drug"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }
                    case "change_culture":
                        {
                            if (connection_try)
                            {
                                String id_old_culture;
                                String id_culture = eventArgument.Split(':')[1].Split('|')[0];
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_cross_culture_str = "SELECT * FROM Cross_culture WHERE id_culture = " + id_culture;
                                SqlDataAdapter get_cross_culture = new SqlDataAdapter(get_cross_culture_str, conn);
                                get_cross_culture.Fill(tablesDS, "CrossCulture");
                                String get_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + id_culture;
                                SqlDataAdapter get_culture_zone = new SqlDataAdapter(get_culture_zones_str, conn);
                                get_culture_zone.Fill(tablesDS, "CultureZones");
                                /* String get_old_culture_str = "SELECT Agronomic.id_old_culture,Culture.title_culture FROM Plot,Agronomic,Culture WHERE Plot.unique_id_plot='" + eventArgument.Split(':')[1].Split('|')[1] + "' AND Plot.id_plot=Agronomic.id_plot AND Agronomic.id_culture=" + eventArgument.Split(':')[1].Split('|')[0] + " AND Culture.id_culture=Agronomic.id_old_culture";
                                 SqlDataAdapter get_old_culture = new SqlDataAdapter(get_old_culture_str, conn);
                                 get_old_culture.Fill(tablesDS, "OldCulture");*/

                                if (Convert.ToInt32(id_culture) > 0)
                                {
                                    String get_pests_str = "SELECT DISTINCT id_pest, title_pest FROM View_Threshold_Pests WHERE id_culture=" + id_culture + " ORDER BY title_pest";
                                    SqlDataAdapter get_pests = new SqlDataAdapter(get_pests_str, conn);
                                    get_pests.Fill(tablesDS, "Pests");
                                    String get_diseases_str = "SELECT DISTINCT id_disease, title_disease FROM View_Threshold_Diseases WHERE id_culture=" + id_culture + " ORDER BY title_disease";
                                    SqlDataAdapter get_diseases = new SqlDataAdapter(get_diseases_str, conn);
                                    get_diseases.Fill(tablesDS, "Diseases");
                                    String get_weeds_str = "SELECT DISTINCT id_weed, title_weed FROM View_Threshold_Weeds WHERE id_culture=" + id_culture + " ORDER BY title_weed";
                                    SqlDataAdapter get_weeds = new SqlDataAdapter(get_weeds_str, conn);
                                    get_weeds.Fill(tablesDS, "Weeds");

                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#PestsCB\").empty();";
                                    j_script += ("\n$(\"#PestsCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "Pests"))
                                    {
                                        //PestsCB.DataSource = tablesDS.Tables["Pests"];
                                        //PestsCB.DataBind();
                                        for (int i = 0; i < tablesDS.Tables["Pests"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#PestsCB\").append('<option value=\"" + tablesDS.Tables["Pests"].Rows[i]["id_pest"].ToString() + "\">" +
                                                         tablesDS.Tables["Pests"].Rows[i]["title_pest"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";
                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#DiseasesCB\").empty();";
                                    j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "Diseases"))
                                    {
                                        /*DiseasesCB.DataSource = tablesDS.Tables["Diseases"];
                                        DiseasesCB.DataBind();*/
                                        for (int i = 0; i < tablesDS.Tables["Diseases"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"" + tablesDS.Tables["Diseases"].Rows[i]["id_disease"].ToString() + "\">" +
                                                         tablesDS.Tables["Diseases"].Rows[i]["title_disease"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";

                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#WeedsCB\").empty();";
                                    j_script += ("\n$(\"#WeedsCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "Weeds"))
                                    {
                                        /*DiseasesCB.DataSource = tablesDS.Tables["Diseases"];
                                        DiseasesCB.DataBind();*/
                                        for (int i = 0; i < tablesDS.Tables["Weeds"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#WeedsCB\").append('<option value=\"" + tablesDS.Tables["Weeds"].Rows[i]["id_weed"].ToString() + "\">" +
                                                         tablesDS.Tables["Weeds"].Rows[i]["title_weed"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";
                                    String get_phase_pests_str = "SELECT DISTINCT id_phase, title_phase FROM View_Threshold_Pests WHERE id_culture=" + id_culture + " ORDER BY title_phase";
                                    String get_phase_diseases_str = "SELECT DISTINCT id_phase, title_phase FROM View_Threshold_Diseases WHERE id_culture=" + id_culture + " ORDER BY title_phase";
                                    String get_phase_weeds_str = "SELECT DISTINCT id_phase, title_phase FROM View_Threshold_Weeds WHERE id_culture=" + id_culture + " ORDER BY title_phase";
                                    SqlDataAdapter get_phase_pests = new SqlDataAdapter(get_phase_pests_str, conn);
                                    SqlDataAdapter get_phase_diseases = new SqlDataAdapter(get_phase_diseases_str, conn);
                                    SqlDataAdapter get_phase_weeds = new SqlDataAdapter(get_phase_weeds_str, conn);
                                    get_phase_pests.Fill(tablesDS, "PhasePests");
                                    get_phase_diseases.Fill(tablesDS, "PhaseDiseases");
                                    get_phase_weeds.Fill(tablesDS, "PhaseWeeds");

                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#PhasePestsCB\").empty();";
                                    j_script += ("\n$(\"#PhasePestsCB\").append('<option value=\"0\"></option>');");
                                    //j_script += "\n$(\"#PestsCB\").empty();";
                                    //j_script += ("\n$(\"#PestsCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "PhasePests"))
                                    {
                                        /*PhasePestsCB.DataSource = tablesDS.Tables["PhasePests"];
                                        PhasePestsCB.DataBind();*/
                                        for (int i = 0; i < tablesDS.Tables["PhasePests"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#PhasePestsCB\").append('<option value=\"" + tablesDS.Tables["PhasePests"].Rows[i]["id_phase"].ToString() + "\">" +
                                                         tablesDS.Tables["PhasePests"].Rows[i]["title_phase"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";

                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#PhaseDiseasesCB\").empty();";
                                    j_script += ("\n$(\"#PhaseDiseasesCB\").append('<option value=\"0\"></option>');");
                                    //j_script += "\n$(\"#DiseasesCB\").empty();";
                                    //j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "PhaseDiseases"))
                                    {
                                        /*PhaseDiseasesCB.DataSource = tablesDS.Tables["PhaseDiseases"];
                                        PhaseDiseasesCB.DataBind();*/
                                        for (int i = 0; i < tablesDS.Tables["PhaseDiseases"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#PhaseDiseasesCB\").append('<option value=\"" + tablesDS.Tables["PhaseDiseases"].Rows[i]["id_phase"].ToString() + "\">" +
                                                         tablesDS.Tables["PhaseDiseases"].Rows[i]["title_phase"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";

                                    j_script += "\n$(function () {";
                                    j_script += "\n$(\"#PhaseWeedsCB\").empty();";
                                    j_script += ("\n$(\"#PhaseWeedsCB\").append('<option value=\"0\"></option>');");
                                    //j_script += "\n$(\"#DiseasesCB\").empty();";
                                    //j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"0\"></option>');");
                                    if (CheckRowsCount(tablesDS, "PhaseWeeds"))
                                    {
                                        /*PhaseDiseasesCB.DataSource = tablesDS.Tables["PhaseDiseases"];
                                        PhaseDiseasesCB.DataBind();*/
                                        for (int i = 0; i < tablesDS.Tables["PhaseWeeds"].Rows.Count; i++)
                                        {
                                            j_script += ("\n$(\"#PhaseWeedsCB\").append('<option value=\"" + tablesDS.Tables["PhaseWeeds"].Rows[i]["id_phase"].ToString() + "\">" +
                                                         tablesDS.Tables["PhaseWeeds"].Rows[i]["title_phase"].ToString() + "</option>');");
                                        }
                                    }
                                    j_script += "});";
                                }
                                conn.Close();

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#CrossCultureCB\").empty();";
                                j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"0\"></option>');");

                                if (CheckRowsCount(tablesDS, "CrossCulture"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["CrossCulture"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#CrossCultureCB\").append('<option value=\"" + tablesDS.Tables["CrossCulture"].Rows[i]["id_cross_culture"].ToString() + "\">" +
                                                     tablesDS.Tables["CrossCulture"].Rows[i]["title_cross_culture"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#CultureZoneCB\").empty();";
                                j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"0\"></option>');");

                                bool set_zone_10 = false;
                                if (CheckRowsCount(tablesDS, "CultureZones"))
                                {
                                    for (int i = 0; i < tablesDS.Tables["CultureZones"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#CultureZoneCB\").append('<option value=\"" + tablesDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                     tablesDS.Tables["CultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                        if (tablesDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") set_zone_10 = true;
                                    }
                                }
                                if (set_zone_10) j_script += "\n$(\"#CultureZoneCB\").val(\"10\");";
                                j_script += "});";

                                /* j_script += "\n$(function () {";
                                 j_script += "\n$(\"#OldCultureCB\").empty();";
                                 j_script += ("\n$(\"#OldCultureCB\").append('<option value=\"0\"></option>');");

                                 if (tablesDS.Tables["OldCulture"].Rows.Count > 0)
                                 {
                                     j_script += ("\n$(\"#OldCultureCB\").append('<option value=\"" + tablesDS.Tables["OldCulture"].Rows[0]["id_old_culture"].ToString() + "\">" +
                                                  tablesDS.Tables["OldCulture"].Rows[0]["title_culture"].ToString() + "</option>');");
                                 }
                                 j_script += "});";*/
                            }
                            break;
                        }
                    /*case "change_old_culture":
                        {
                            if (connection_try)
                            {
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_old_culture_zones_str = "SELECT * FROM Loss,Loss_zones WHERE Loss.id_loss_zone=Loss_zones.id_loss_zone AND Loss.id_culture = " + eventArgument.Split(':')[1];
                                SqlDataAdapter get_old_culture_zone = new SqlDataAdapter(get_old_culture_zones_str, conn);
                                get_old_culture_zone.Fill(tablesDS, "OldCultureZones");
                                conn.Close();
                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#OldCultureZoneCB\").empty();";
                                j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"0\"></option>');");

                                if (tablesDS.Tables["OldCultureZones"].Rows.Count > 0)
                                {
                                    for (int i = 0; i < tablesDS.Tables["OldCultureZones"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#OldCultureZoneCB\").append('<option value=\"" + tablesDS.Tables["OldCultureZones"].Rows[i]["id_loss_zone"].ToString() + "\">" +
                                                     tablesDS.Tables["OldCultureZones"].Rows[i]["title_loss_zone"].ToString() + "</option>');");
                                        if (tablesDS.Tables["CultureZones"].Rows[i]["id_loss_zone"].ToString() == "10") { j_script += "\n$(\"#CultureZoneCB\").val(\"10\");"; }
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }*/
                    case "doses_calculations_culture":
                        {
                            if (connection_try)
                            {
                                String unique_number_plot = eventArgument.Split(':')[1].Split('|')[0];
                                String id_culture = eventArgument.Split(':')[1].Split('|')[1];
                                String id_loss_zone = eventArgument.Split(':')[1].Split('|')[2];
                                String planned_productivity = NotNull(eventArgument.Split(':')[1].Split('|')[3]).Replace(',', '.');
                                String dose_organic_fertilizer_n = NotNull(eventArgument.Split(':')[1].Split('|')[4]).Replace(',', '.');
                                String dose_organic_fertilizer_p2o5 = NotNull(eventArgument.Split(':')[1].Split('|')[5]).Replace(',', '.');
                                String dose_organic_fertilizer_k2o = NotNull(eventArgument.Split(':')[1].Split('|')[6]).Replace(',', '.');
                                String old_dose_organic_fertilizer_n = NotNull(eventArgument.Split(':')[1].Split('|')[7]).Replace(',', '.');
                                String old_dose_organic_fertilizer_p2o5 = NotNull(eventArgument.Split(':')[1].Split('|')[8]).Replace(',', '.');
                                String old_dose_organic_fertilizer_k2o = NotNull(eventArgument.Split(':')[1].Split('|')[9]).Replace(',', '.');
                                String old_dose_mineral_fertilizer_n = NotNull(eventArgument.Split(':')[1].Split('|')[10]).Replace(',', '.');
                                String old_dose_mineral_fertilizer_p2o5 = NotNull(eventArgument.Split(':')[1].Split('|')[11]).Replace(',', '.');
                                String old_dose_mineral_fertilizer_k2o = NotNull(eventArgument.Split(':')[1].Split('|')[12]).Replace(',', '.');
                                String year = eventArgument.Split(':')[1].Split('|')[13];
                                String area = eventArgument.Split(':')[1].Split('|')[14].Replace(',', '.');
                                String code_plot = "", id_grading = "", id_erosion = "";
                                if (year != (DateTime.Now.Year + 1).ToString()) break;
                                //j_script += "$(\"#DosesCalculationsW\").dialog(\"open\");\n";

                                conn.Open();
                                DataSet tableDS = new DataSet();
                                String get_codeplot_grading_erosion_str = "SELECT Plot.code_plot,Soil.id_grading,id_erosion FROM Plot,Soil WHERE Plot.unique_id_plot='" + unique_number_plot + "' AND Plot.id_plot=Soil.id_plot;";
                                SqlDataAdapter get_codeplot_grading_erosion = new SqlDataAdapter(get_codeplot_grading_erosion_str, conn);
                                get_codeplot_grading_erosion.Fill(tableDS, "Temp_Values");
                                code_plot = tableDS.Tables["Temp_Values"].Rows[0]["code_plot"].ToString();
                                id_grading = NotNull(tableDS.Tables["Temp_Values"].Rows[0]["id_grading"].ToString());
                                id_erosion = NotNull(tableDS.Tables["Temp_Values"].Rows[0]["id_erosion"].ToString());

                                String get_doses_calculations_str = "exec [dbo].[Calсulate_N_P2O5_K2O] '" + code_plot + "', " + id_culture + ", " + id_loss_zone + ", " + planned_productivity + ", " + id_grading + ", " + id_erosion + ", " + dose_organic_fertilizer_n + ", " + dose_organic_fertilizer_p2o5 + ", " + dose_organic_fertilizer_k2o + ", " + old_dose_organic_fertilizer_n + ", " + old_dose_organic_fertilizer_p2o5 + ", " + old_dose_organic_fertilizer_k2o + ", " + old_dose_mineral_fertilizer_n + ", " + old_dose_mineral_fertilizer_p2o5 + ", " + old_dose_mineral_fertilizer_k2o;
                                SqlDataAdapter get_doses_calculations = new SqlDataAdapter(get_doses_calculations_str, conn);
                                get_doses_calculations.Fill(tableDS, "DosesCalculations");
                                conn.Close();

                                j_script += "$(\"#Loss_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_n"].ToString() + "');\n";
                                j_script += "$(\"#Loss_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#Loss_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_k2o"].ToString() + "');\n";
                                j_script += "$(\"#Planned_productivityTB\").val('" + planned_productivity + "');\n";
                                j_script += "$(\"#Do_nTB\").val('" + dose_organic_fertilizer_n + "');\n";
                                j_script += "$(\"#Do_p2o5TB\").val('" + dose_organic_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#Do_k2oTB\").val('" + dose_organic_fertilizer_k2o + "');\n";
                                j_script += "$(\"#Dop_nTB\").val('" + old_dose_organic_fertilizer_n + "');\n";
                                j_script += "$(\"#Dop_p2o5TB\").val('" + old_dose_organic_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#Dop_k2oTB\").val('" + old_dose_organic_fertilizer_k2o + "');\n";
                                j_script += "$(\"#Dp_nTB\").val('" + old_dose_mineral_fertilizer_n + "');\n";
                                j_script += "$(\"#Dp_p2o5TB\").val('" + old_dose_mineral_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#Dp_k2oTB\").val('" + old_dose_mineral_fertilizer_k2o + "');\n";

                                j_script += "$(\"#K1_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_n"].ToString() + "');\n";
                                j_script += "$(\"#K1_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K1_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K2_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k2"].ToString() + "');\n";
                                j_script += "$(\"#K3_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_n"].ToString() + "');\n";
                                j_script += "$(\"#K3_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K3_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K4_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_n"].ToString() + "');\n";
                                j_script += "$(\"#K4_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K4_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K5_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_n"].ToString() + "');\n";
                                j_script += "$(\"#K5_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K5_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K6_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k6_n"].ToString() + "');\n";
                                j_script += "$(\"#K6_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k6_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K6_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k6_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K7_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k7_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K7_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k7_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K8_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["k8"].ToString() + "');\n";

                                /*j_script += "$(\"#A_org_fact_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_organic_n"].ToString() + "');\n";
                                j_script += "$(\"#A_org_fact_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_organic_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#A_org_fact_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_organic_k2o"].ToString() + "');\n";
                                j_script += "$(\"#A_min_fact_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["mineral_n"].ToString() + "');\n";
                                j_script += "$(\"#A_min_fact_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["mineral_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#A_min_fact_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["mineral_k2o"].ToString() + "');\n";
                                j_script += "$(\"#A_org_plan_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["organic_n"].ToString() + "');\n";
                                j_script += "$(\"#A_org_plan_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["organic_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#A_org_plan_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["organic_k2o"].ToString() + "');\n";
                                j_script += "$(\"#A_min_plan_nTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_mineral_n"].ToString() + "');\n";
                                j_script += "$(\"#A_min_plan_p2o5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_mineral_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#A_min_plan_k2oTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["old_mineral_k2o"].ToString() + "');\n";*/

                                if (Convert.ToDouble(tableDS.Tables["DosesCalculations"].Rows[0]["d_n"].ToString()) >= 0)
                                    j_script += "$(\"#D_NTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["d_n"].ToString() + "');\n";
                                else j_script += "$(\"#D_NTB\").val('0');\n";
                                if (Convert.ToDouble(tableDS.Tables["DosesCalculations"].Rows[0]["d_p2o5"].ToString()) >= 0)
                                    j_script += "$(\"#D_P2O5TB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["d_p2o5"].ToString() + "');\n";
                                else j_script += "$(\"#D_P2O5TB\").val('0');\n";
                                if (Convert.ToDouble(tableDS.Tables["DosesCalculations"].Rows[0]["d_k2o"].ToString()) >= 0)
                                    j_script += "$(\"#D_K2OTB\").val('" + tableDS.Tables["DosesCalculations"].Rows[0]["d_k2o"].ToString() + "');\n";
                                else j_script += "$(\"#D_K2OTB\").val('0');\n";
                                j_script += "$(\"#D_NTBc\").val($(\"#D_NTB\").val())\n";
                                j_script += "$(\"#D_P2O5TBc\").val($(\"#D_P2O5TB\").val())\n";
                                j_script += "$(\"#D_K2OTBc\").val($(\"#D_K2OTB\").val())\n";

                                j_script += "$(\"#DNL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["d_n"].ToString() + "');\n";
                                j_script += "$(\"#LossNL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_n"].ToString() + "');\n";
                                j_script += "$(\"#ProductivityNL\").attr('title', '" + planned_productivity + "');\n";
                                j_script += "$(\"#K1NL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_n"].ToString() + "');\n";
                                j_script += "$(\"#K2NL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k2"].ToString() + "');\n";
                                j_script += "$(\"#K3NL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_n"].ToString() + "');\n";
                                j_script += "$(\"#DoNL\").attr('title', '" + dose_organic_fertilizer_n + "');\n";
                                j_script += "$(\"#K4NL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_n"].ToString() + "');\n";
                                j_script += "$(\"#DopNL\").attr('title', '" + old_dose_organic_fertilizer_n + "');\n";
                                j_script += "$(\"#K5NL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_n"].ToString() + "');\n";

                                j_script += "$(\"#DP2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["d_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#LossP2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#ProductivityP2O5L\").attr('title', '" + planned_productivity + "');\n";
                                j_script += "$(\"#K1P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K3P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K7P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k7_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#K8P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k8"].ToString() + "');\n";
                                j_script += "$(\"#DoP2O5L\").attr('title', '" + dose_organic_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#K4P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#DopP2O5L\").attr('title', '" + old_dose_organic_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#K5P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_p2o5"].ToString() + "');\n";
                                j_script += "$(\"#DpP2O5L\").attr('title', '" + old_dose_mineral_fertilizer_p2o5 + "');\n";
                                j_script += "$(\"#K6P2O5L\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k6_p2o5"].ToString() + "');\n";

                                j_script += "$(\"#DK2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["d_k2o"].ToString() + "');\n";
                                j_script += "$(\"#LossK2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["loss_k2o"].ToString() + "');\n";
                                j_script += "$(\"#ProductivityK2OL\").attr('title', '" + planned_productivity + "');\n";
                                j_script += "$(\"#K1K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k1_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K3K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k3_k2o"].ToString() + "');\n";
                                j_script += "$(\"#K7K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k7_k2o"].ToString() + "');\n";
                                j_script += "$(\"#DoK2OL\").attr('title', '" + dose_organic_fertilizer_k2o + "');\n";
                                j_script += "$(\"#K4K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k4_k2o"].ToString() + "');\n";
                                j_script += "$(\"#DopK2OL\").attr('title', '" + old_dose_organic_fertilizer_k2o + "');\n";
                                j_script += "$(\"#K5K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k5_k2o"].ToString() + "');\n";
                                j_script += "$(\"#DpK2OL\").attr('title', '" + old_dose_mineral_fertilizer_k2o + "');\n";
                                j_script += "$(\"#K6K2OL\").attr('title', '" + tableDS.Tables["DosesCalculations"].Rows[0]["k6_k2o"].ToString() + "');\n";


                            }
                            break;
                        }
                    case "savedata":
                        {
                            try
                            {
                                String[] crop_rotation_data = new String[11];
                                for (int i = 0; i < 11; i++)
                                {
                                    crop_rotation_data[i] = String.Empty;
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 5)
                                {
                                    crop_rotation_data[0] = eventArgument.Split(':')[1].Split('&')[0];
                                    crop_rotation_data[1] = eventArgument.Split(':')[1].Split('&')[1];
                                    crop_rotation_data[2] = eventArgument.Split(':')[1].Split('&')[2];
                                    crop_rotation_data[3] = eventArgument.Split(':')[1].Split('&')[3];
                                    crop_rotation_data[4] = eventArgument.Split(':')[1].Split('&')[4];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 6)
                                {
                                    crop_rotation_data[5] = eventArgument.Split(':')[1].Split('&')[5];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 7)
                                {
                                    crop_rotation_data[6] = eventArgument.Split(':')[1].Split('&')[6];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 8)
                                {
                                    crop_rotation_data[7] = eventArgument.Split(':')[1].Split('&')[7];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 9)
                                {
                                    crop_rotation_data[8] = eventArgument.Split(':')[1].Split('&')[8];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length >= 10)
                                {
                                    crop_rotation_data[9] = eventArgument.Split(':')[1].Split('&')[9];
                                }
                                if (eventArgument.Split(':')[1].Split('&').Length == 11)
                                {
                                    crop_rotation_data[10] = eventArgument.Split(':')[1].Split('&')[10];
                                }

                                if (connection_try)
                                {
                                    conn.Open();
                                    for (int count_plots = 0; count_plots < crop_rotation_data[0].Split('|')[0].Split(',').Length; count_plots++)
                                    {
                                        if (eventArgument.Split(':')[1].Split('&').Length >= 5)
                                        {
                                            //добавление данных по севообороту
                                            SqlCommand set_crop_rotation = new SqlCommand("Add_Edit_History_Book_Fields", conn);
                                            set_crop_rotation.CommandType = CommandType.StoredProcedure;
                                            set_crop_rotation.Parameters.AddWithValue("@unique_id_plot", crop_rotation_data[0].Split('|')[0].Split(',')[count_plots]);
                                            set_crop_rotation.Parameters.AddWithValue("@year", Convert.ToInt32(crop_rotation_data[0].Split('|')[1]));
                                            set_crop_rotation.Parameters.AddWithValue("@id_sort_crop_rotation", Convert.ToInt32(NotNull(crop_rotation_data[0].Split('|')[2])));
                                            set_crop_rotation.Parameters.Add("@id_history_book_field", SqlDbType.Int);
                                            set_crop_rotation.Parameters["@id_history_book_field"].Direction = ParameterDirection.Output;
                                            set_crop_rotation.ExecuteNonQuery();

                                            //задание вида собственности
                                            if (NotNull(crop_rotation_data[0].Split('|')[3]) != "0")
                                            {
                                                SqlCommand set_type_property = new SqlCommand("Add_Edit_Plot_type_property", conn);
                                                set_type_property.CommandType = CommandType.StoredProcedure;
                                                set_type_property.Parameters.AddWithValue("@unique_id_plot", crop_rotation_data[0].Split('|')[0].Split(',')[count_plots]);
                                                set_type_property.Parameters.AddWithValue("@id_type_property", Convert.ToInt32(NotNull(crop_rotation_data[0].Split('|')[3])));
                                                set_type_property.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                SqlCommand delete_type_property = new SqlCommand("Delete_Plot_type_property", conn);
                                                delete_type_property.CommandType = CommandType.StoredProcedure;
                                                delete_type_property.Parameters.AddWithValue("@unique_id_plot", crop_rotation_data[0].Split('|')[0].Split(',')[count_plots]);
                                                delete_type_property.ExecuteNonQuery();
                                            }

                                            Int32 id_history_book_field = Convert.ToInt32(set_crop_rotation.Parameters["@id_history_book_field"].Value);

                                            SqlCommand get_old_crop_rotation = new SqlCommand("Get_Id_History_Book_Fields", conn);
                                            get_old_crop_rotation.CommandType = CommandType.StoredProcedure;
                                            get_old_crop_rotation.Parameters.AddWithValue("@unique_id_plot", crop_rotation_data[0].Split('|')[0].Split(',')[count_plots]);
                                            get_old_crop_rotation.Parameters.AddWithValue("@year", (Convert.ToInt32(crop_rotation_data[0].Split('|')[1]) - 1));
                                            get_old_crop_rotation.Parameters.Add("@id_history_book_field", SqlDbType.Int);
                                            get_old_crop_rotation.Parameters["@id_history_book_field"].Direction = ParameterDirection.Output;
                                            get_old_crop_rotation.ExecuteNonQuery();

                                            Int32 id_old_history_book_field = Convert.ToInt32(get_old_crop_rotation.Parameters["@id_history_book_field"].Value);

                                            if (id_old_history_book_field == 0)
                                            {
                                                set_crop_rotation = new SqlCommand("Add_Edit_History_Book_Fields", conn);
                                                set_crop_rotation.CommandType = CommandType.StoredProcedure;
                                                set_crop_rotation.Parameters.AddWithValue("@unique_id_plot", crop_rotation_data[0].Split('|')[0].Split(',')[count_plots]);
                                                set_crop_rotation.Parameters.AddWithValue("@year", (Convert.ToInt32(crop_rotation_data[0].Split('|')[1]) - 1));
                                                set_crop_rotation.Parameters.AddWithValue("@id_sort_crop_rotation", 0);
                                                set_crop_rotation.Parameters.Add("@id_history_book_field", SqlDbType.Int);
                                                set_crop_rotation.Parameters["@id_history_book_field"].Direction = ParameterDirection.Output;
                                                set_crop_rotation.ExecuteNonQuery();

                                                id_old_history_book_field = Convert.ToInt32(set_crop_rotation.Parameters["@id_history_book_field"].Value);
                                            }

                                            //добавление данных по культурам
                                            DateTime sowing_date;
                                            Int64 sowing_date_long = 0;
                                            if (crop_rotation_data[1].Split('|')[6] != String.Empty)
                                            {
                                                sowing_date = DateTime.ParseExact(crop_rotation_data[1].Split('|')[6], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                sowing_date_long = sowing_date.Ticks;
                                            }

                                            DateTime harvest_date;
                                            Int64 harvest_date_long = 0;
                                            if (crop_rotation_data[1].Split('|')[8] != String.Empty)
                                            {
                                                harvest_date = DateTime.ParseExact(crop_rotation_data[1].Split('|')[8], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                harvest_date_long = harvest_date.Ticks;
                                            }

                                            //добавление данных по культурам
                                            SqlCommand set_culture = new SqlCommand("Add_Edit_History_book_cultures", conn);
                                            set_culture.CommandType = CommandType.StoredProcedure;
                                            set_culture.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                            set_culture.Parameters.AddWithValue("@id_culture", Convert.ToInt32(NotNull(crop_rotation_data[1].Split('|')[0])));
                                            set_culture.Parameters.AddWithValue("@id_loss_zone", Convert.ToInt32(NotNull(crop_rotation_data[1].Split('|')[9])));
                                            set_culture.Parameters.AddWithValue("@id_cross_culture", Convert.ToInt32(NotNull(crop_rotation_data[1].Split('|')[2])));
                                            set_culture.Parameters.AddWithValue("@reproduction", NotNull(crop_rotation_data[1].Split('|')[3]));
                                            set_culture.Parameters.AddWithValue("@seding_rate", Convert.ToDouble(NotNull(crop_rotation_data[1].Split('|')[4])));
                                            set_culture.Parameters.AddWithValue("@planned_productivity", Convert.ToDouble(NotNull(crop_rotation_data[1].Split('|')[5])));
                                            set_culture.Parameters.AddWithValue("@actual_productivity", Convert.ToDouble(NotNull(crop_rotation_data[1].Split('|')[7])));
                                            set_culture.Parameters.AddWithValue("@sowing_date_long", sowing_date_long);
                                            set_culture.Parameters.AddWithValue("@harvest_date_long", harvest_date_long);
                                            set_culture.ExecuteNonQuery();

                                            SqlCommand update_old_culture = new SqlCommand("Update_Old_Culture_In_History_book", conn);
                                            update_old_culture.CommandType = CommandType.StoredProcedure;
                                            update_old_culture.Parameters.AddWithValue("@id_history_book_field", id_old_history_book_field);
                                            update_old_culture.Parameters.AddWithValue("@id_culture", Convert.ToInt32(NotNull(crop_rotation_data[1].Split('|')[1])));
                                            //update_old_culture.Parameters.AddWithValue("@id_loss_zone", Convert.ToInt32(NotNull(crop_rotation_data[1].Split('|')[10])));
                                            update_old_culture.ExecuteNonQuery();

                                            //добавление данных по минеральным удобрениям
                                            DateTime date_basic_fertilization;
                                            Int64 date_basic_fertilization_long = 0;
                                            if (crop_rotation_data[2].Split('|')[2] != String.Empty)
                                            {
                                                date_basic_fertilization = DateTime.ParseExact(crop_rotation_data[2].Split('|')[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_basic_fertilization_long = date_basic_fertilization.Ticks;
                                            }
                                            DateTime date_sowing_fertilizatio;
                                            Int64 date_sowing_fertilizatio_long = 0;
                                            if (crop_rotation_data[2].Split('|')[5] != String.Empty)
                                            {
                                                date_sowing_fertilizatio = DateTime.ParseExact(crop_rotation_data[2].Split('|')[5], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_sowing_fertilizatio_long = date_sowing_fertilizatio.Ticks;
                                            }
                                            DateTime date_dressing_fertilization;
                                            Int64 date_dressing_fertilization_long = 0;
                                            if (crop_rotation_data[2].Split('|')[8] != String.Empty)
                                            {
                                                date_dressing_fertilization = DateTime.ParseExact(crop_rotation_data[2].Split('|')[8], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_dressing_fertilization_long = date_dressing_fertilization.Ticks;
                                            }

                                            SqlCommand set_mineral_fertilizer = new SqlCommand("Add_Edit_History_book_mineral", conn);
                                            set_mineral_fertilizer.CommandType = CommandType.StoredProcedure;
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_basic_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[0])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_basic_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[1])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_basic_fertilization_long", date_basic_fertilization_long);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_sowing_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[3])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_sowing_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[4])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_sowing_fertilization_long", date_sowing_fertilizatio_long);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_dressing_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[6])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_dressing_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[7])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_dressing_fertilization_long", date_dressing_fertilization_long);
                                            set_mineral_fertilizer.ExecuteNonQuery();

                                            date_basic_fertilization_long = 0;
                                            if (crop_rotation_data[2].Split('|')[11] != String.Empty)
                                            {
                                                date_basic_fertilization = DateTime.ParseExact(crop_rotation_data[2].Split('|')[11], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_basic_fertilization_long = date_basic_fertilization.Ticks;
                                            }

                                            date_sowing_fertilizatio_long = 0;
                                            if (crop_rotation_data[2].Split('|')[14] != String.Empty)
                                            {
                                                date_sowing_fertilizatio = DateTime.ParseExact(crop_rotation_data[2].Split('|')[14], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_sowing_fertilizatio_long = date_sowing_fertilizatio.Ticks;
                                            }

                                            date_dressing_fertilization_long = 0;
                                            if (crop_rotation_data[2].Split('|')[17] != String.Empty)
                                            {
                                                date_dressing_fertilization = DateTime.ParseExact(crop_rotation_data[2].Split('|')[17], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_dressing_fertilization_long = date_dressing_fertilization.Ticks;
                                            }

                                            set_mineral_fertilizer = new SqlCommand("Add_Edit_History_book_mineral", conn);
                                            set_mineral_fertilizer.CommandType = CommandType.StoredProcedure;
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_history_book_field", id_old_history_book_field);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_basic_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[9])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_basic_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[10])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_basic_fertilization_long", date_basic_fertilization_long);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_sowing_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[12])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_sowing_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[13])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_sowing_fertilization_long", date_sowing_fertilizatio_long);
                                            set_mineral_fertilizer.Parameters.AddWithValue("@id_dressing_fertilization", Convert.ToInt32(NotNull(crop_rotation_data[2].Split('|')[15])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@dose_dressing_fertilization", Convert.ToDouble(NotNull(crop_rotation_data[2].Split('|')[16])));
                                            set_mineral_fertilizer.Parameters.AddWithValue("@date_dressing_fertilization_long", date_dressing_fertilization_long);
                                            set_mineral_fertilizer.ExecuteNonQuery();

                                            //добавление данных по органическим удобрениям
                                            DateTime date_organic_fertilizer;
                                            Int64 date_organic_fertilizer_long = 0;
                                            if (crop_rotation_data[3].Split('|')[2] != String.Empty)
                                            {
                                                date_organic_fertilizer = DateTime.ParseExact(crop_rotation_data[3].Split('|')[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_organic_fertilizer_long = date_organic_fertilizer.Ticks;
                                            }

                                            SqlCommand set_organic_fertilizer = new SqlCommand("Add_Edit_History_book_organic", conn);
                                            set_organic_fertilizer.CommandType = CommandType.StoredProcedure;
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_organic_fertilizer", Convert.ToInt32(NotNull(crop_rotation_data[3].Split('|')[0])));
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_protocol", Convert.ToInt32(NotNull(crop_rotation_data[3].Split('|')[6])));
                                            set_organic_fertilizer.Parameters.AddWithValue("@dose_organic_fertilizer", Convert.ToDouble(NotNull(crop_rotation_data[3].Split('|')[1])));
                                            set_organic_fertilizer.Parameters.AddWithValue("@date_organic_fertilizer_long", date_organic_fertilizer_long);
                                            set_organic_fertilizer.ExecuteNonQuery();

                                            date_organic_fertilizer_long = 0;
                                            if (crop_rotation_data[3].Split('|')[5] != String.Empty)
                                            {
                                                date_organic_fertilizer = DateTime.ParseExact(crop_rotation_data[3].Split('|')[5], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_organic_fertilizer_long = date_organic_fertilizer.Ticks;
                                            }

                                            set_organic_fertilizer = new SqlCommand("Add_Edit_History_book_organic", conn);
                                            set_organic_fertilizer.CommandType = CommandType.StoredProcedure;
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_history_book_field", id_old_history_book_field);
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_organic_fertilizer", Convert.ToInt32(NotNull(crop_rotation_data[3].Split('|')[3])));
                                            set_organic_fertilizer.Parameters.AddWithValue("@id_protocol", "0");
                                            set_organic_fertilizer.Parameters.AddWithValue("@dose_organic_fertilizer", Convert.ToDouble(NotNull(crop_rotation_data[3].Split('|')[4])));
                                            set_organic_fertilizer.Parameters.AddWithValue("@date_organic_fertilizer_long", date_organic_fertilizer_long);
                                            set_organic_fertilizer.ExecuteNonQuery();

                                            //добавление мелиорантов
                                            DateTime date_ameliorator_1;
                                            Int64 date_ameliorator_1_long = 0;
                                            if (crop_rotation_data[4].Split('|')[3] != String.Empty)
                                            {
                                                date_ameliorator_1 = DateTime.ParseExact(crop_rotation_data[4].Split('|')[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_ameliorator_1_long = date_ameliorator_1.Ticks;
                                            }

                                            DateTime date_ameliorator_2;
                                            Int64 date_ameliorator_2_long = 0;
                                            if (crop_rotation_data[4].Split('|')[7] != String.Empty)
                                            {
                                                date_ameliorator_2 = DateTime.ParseExact(crop_rotation_data[4].Split('|')[7], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date_ameliorator_2_long = date_ameliorator_2.Ticks;
                                            }

                                            SqlCommand set_ameliorators = new SqlCommand("Add_Edit_History_book_ameliorators", conn);
                                            set_ameliorators.CommandType = CommandType.StoredProcedure;
                                            set_ameliorators.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                            set_ameliorators.Parameters.AddWithValue("@id_ameliorator_1", Convert.ToInt32(NotNull(crop_rotation_data[4].Split('|')[0])));
                                            set_ameliorators.Parameters.AddWithValue("@percent_caco3_1", Convert.ToDouble(NotNull(crop_rotation_data[4].Split('|')[1])));
                                            set_ameliorators.Parameters.AddWithValue("@dose_ameliorator_1", Convert.ToDouble(NotNull(crop_rotation_data[4].Split('|')[2])));
                                            set_ameliorators.Parameters.AddWithValue("@date_ameliorator_1_long", date_ameliorator_1_long);
                                            set_ameliorators.Parameters.AddWithValue("@id_ameliorator_2", Convert.ToInt32(NotNull(crop_rotation_data[4].Split('|')[4])));
                                            set_ameliorators.Parameters.AddWithValue("@percent_caco3_2", Convert.ToDouble(NotNull(crop_rotation_data[4].Split('|')[5])));
                                            set_ameliorators.Parameters.AddWithValue("@dose_ameliorator_2", Convert.ToDouble(NotNull(crop_rotation_data[4].Split('|')[6])));
                                            set_ameliorators.Parameters.AddWithValue("@date_ameliorator_2_long", date_ameliorator_2_long);
                                            set_ameliorators.ExecuteNonQuery();

                                            //добавление обработки почвы
                                            if (eventArgument.Split(':')[1].Split('&').Length >= 6)
                                            {
                                                SqlCommand delete_tilliage = new SqlCommand("Delete_History_book_tillage", conn);
                                                delete_tilliage.CommandType = CommandType.StoredProcedure;
                                                delete_tilliage.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_tilliage.ExecuteNonQuery();

                                                if (crop_rotation_data[5] != String.Empty)
                                                {
                                                    SqlCommand add_tillage;
                                                    DateTime date_tillage;
                                                    Int64 date_tillage_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[5].Split('|').Length; i++)
                                                    {
                                                        date_tillage_long = 0;
                                                        if (crop_rotation_data[5].Split('|')[i].Split(';')[2] != String.Empty)
                                                        {
                                                            date_tillage = DateTime.ParseExact(crop_rotation_data[5].Split('|')[i].Split(';')[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_tillage_long = date_tillage.Ticks;
                                                        }

                                                        add_tillage = new SqlCommand("Add_History_book_tillage", conn);
                                                        add_tillage.CommandType = CommandType.StoredProcedure;
                                                        add_tillage.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_tillage.Parameters.AddWithValue("@id_type_tillage", Convert.ToInt32(NotNull(crop_rotation_data[5].Split('|')[i].Split(';')[0])));
                                                        add_tillage.Parameters.AddWithValue("@depth_tillage", Convert.ToDouble(NotNull(crop_rotation_data[5].Split('|')[i].Split(';')[1])));
                                                        add_tillage.Parameters.AddWithValue("@date_tillage_long", date_tillage_long);
                                                        add_tillage.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //добавление защиты растений
                                            if (eventArgument.Split(':')[1].Split('&').Length >= 7)
                                            {
                                                SqlCommand delete_plant_protection = new SqlCommand("Delete_History_book_protection", conn);
                                                delete_plant_protection.CommandType = CommandType.StoredProcedure;
                                                delete_plant_protection.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_plant_protection.ExecuteNonQuery();

                                                if (crop_rotation_data[6] != String.Empty)
                                                {
                                                    SqlCommand add_plant_protection;
                                                    DateTime date_plant_protection;
                                                    Int64 date_plant_protection_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[6].Split('|').Length; i++)
                                                    {
                                                        date_plant_protection_long = 0;
                                                        if (crop_rotation_data[6].Split('|')[i].Split(';')[3] != String.Empty)
                                                        {
                                                            date_plant_protection = DateTime.ParseExact(crop_rotation_data[6].Split('|')[i].Split(';')[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_plant_protection_long = date_plant_protection.Ticks;
                                                        }

                                                        add_plant_protection = new SqlCommand("Add_History_book_protection", conn);
                                                        add_plant_protection.CommandType = CommandType.StoredProcedure;
                                                        add_plant_protection.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_plant_protection.Parameters.AddWithValue("@id_drug", Convert.ToInt32(NotNull(crop_rotation_data[6].Split('|')[i].Split(';')[1])));
                                                        add_plant_protection.Parameters.AddWithValue("@dose_drug", Convert.ToDouble(NotNull(crop_rotation_data[6].Split('|')[i].Split(';')[2])));
                                                        add_plant_protection.Parameters.AddWithValue("@date_plant_protection_long", date_plant_protection_long);
                                                        add_plant_protection.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //добавление вредителей
                                            if (eventArgument.Split(':')[1].Split('&').Length >= 8)
                                            {
                                                SqlCommand delete_pests = new SqlCommand("Delete_History_book_pests", conn);
                                                delete_pests.CommandType = CommandType.StoredProcedure;
                                                delete_pests.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_pests.ExecuteNonQuery();

                                                if (crop_rotation_data[7] != String.Empty)
                                                {
                                                    SqlCommand add_pest;
                                                    DateTime date_pest;
                                                    Int64 date_pest_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[7].Split('|').Length; i++)
                                                    {
                                                        date_pest_long = 0;
                                                        if (crop_rotation_data[7].Split('|')[i].Split(';')[3] != String.Empty)
                                                        {
                                                            date_pest = DateTime.ParseExact(crop_rotation_data[7].Split('|')[i].Split(';')[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_pest_long = date_pest.Ticks;
                                                        }

                                                        add_pest = new SqlCommand("Add_History_book_pest", conn);
                                                        add_pest.CommandType = CommandType.StoredProcedure;
                                                        add_pest.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_pest.Parameters.AddWithValue("@id_phase", Convert.ToInt32(NotNull(crop_rotation_data[7].Split('|')[i].Split(';')[0])));
                                                        add_pest.Parameters.AddWithValue("@id_pest", Convert.ToInt32(NotNull(crop_rotation_data[7].Split('|')[i].Split(';')[1])));
                                                        add_pest.Parameters.AddWithValue("@count_pests", Convert.ToDouble(NotNull(crop_rotation_data[7].Split('|')[i].Split(';')[2])));
                                                        add_pest.Parameters.AddWithValue("@date_pest_long", date_pest_long);
                                                        add_pest.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //добавление болезней
                                            if (eventArgument.Split(':')[1].Split('&').Length >= 9)
                                            {
                                                SqlCommand delete_diseases = new SqlCommand("Delete_History_book_diseases", conn);
                                                delete_diseases.CommandType = CommandType.StoredProcedure;
                                                delete_diseases.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_diseases.ExecuteNonQuery();

                                                if (crop_rotation_data[8] != String.Empty)
                                                {
                                                    SqlCommand add_disease;
                                                    DateTime date_disease;
                                                    Int64 date_disease_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[8].Split('|').Length; i++)
                                                    {
                                                        date_disease_long = 0;
                                                        if (crop_rotation_data[8].Split('|')[i].Split(';')[3] != String.Empty)
                                                        {
                                                            date_disease = DateTime.ParseExact(crop_rotation_data[8].Split('|')[i].Split(';')[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_disease_long = date_disease.Ticks;
                                                        }

                                                        add_disease = new SqlCommand("Add_History_book_disease", conn);
                                                        add_disease.CommandType = CommandType.StoredProcedure;
                                                        add_disease.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_disease.Parameters.AddWithValue("@id_phase", Convert.ToInt32(NotNull(crop_rotation_data[8].Split('|')[i].Split(';')[0])));
                                                        add_disease.Parameters.AddWithValue("@id_disease", Convert.ToInt32(NotNull(crop_rotation_data[8].Split('|')[i].Split(';')[1])));
                                                        add_disease.Parameters.AddWithValue("@percent_disease", Convert.ToDouble(NotNull(crop_rotation_data[8].Split('|')[i].Split(';')[2])));
                                                        add_disease.Parameters.AddWithValue("@date_disease_long", date_disease_long);
                                                        add_disease.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //добавление сорняков
                                            if (eventArgument.Split(':')[1].Split('&').Length >= 10)
                                            {
                                                SqlCommand delete_weeds = new SqlCommand("Delete_History_book_weed", conn);
                                                delete_weeds.CommandType = CommandType.StoredProcedure;
                                                delete_weeds.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_weeds.ExecuteNonQuery();

                                                if (crop_rotation_data[9] != String.Empty)
                                                {
                                                    SqlCommand add_weed;
                                                    DateTime date_weed;
                                                    Int64 date_weed_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[9].Split('|').Length; i++)
                                                    {
                                                        date_weed_long = 0;
                                                        if (crop_rotation_data[9].Split('|')[i].Split(';')[3] != String.Empty)
                                                        {
                                                            date_weed = DateTime.ParseExact(crop_rotation_data[9].Split('|')[i].Split(';')[3], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_weed_long = date_weed.Ticks;
                                                        }

                                                        add_weed = new SqlCommand("Add_History_book_weed", conn);
                                                        add_weed.CommandType = CommandType.StoredProcedure;
                                                        add_weed.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_weed.Parameters.AddWithValue("@id_phase", Convert.ToInt32(NotNull(crop_rotation_data[9].Split('|')[i].Split(';')[0])));
                                                        add_weed.Parameters.AddWithValue("@id_weed", Convert.ToInt32(NotNull(crop_rotation_data[9].Split('|')[i].Split(';')[1])));
                                                        add_weed.Parameters.AddWithValue("@count_weed", Convert.ToDouble(NotNull(crop_rotation_data[9].Split('|')[i].Split(';')[2])));
                                                        add_weed.Parameters.AddWithValue("@date_weed_long", date_weed_long);
                                                        add_weed.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //добавление засоренности
                                            if (eventArgument.Split(':')[1].Split('&').Length == 11)
                                            {
                                                SqlCommand delete_weediness = new SqlCommand("Delete_History_book_weediness", conn);
                                                delete_weediness.CommandType = CommandType.StoredProcedure;
                                                delete_weediness.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                delete_weediness.ExecuteNonQuery();

                                                if (crop_rotation_data[10] != String.Empty)
                                                {
                                                    SqlCommand add_weediness;
                                                    DateTime date_weediness;
                                                    Int64 date_weediness_long = 0;
                                                    for (int i = 0; i < crop_rotation_data[10].Split('|').Length; i++)
                                                    {
                                                        date_weediness_long = 0;
                                                        if (crop_rotation_data[10].Split('|')[i].Split(';')[2] != String.Empty)
                                                        {
                                                            date_weediness = DateTime.ParseExact(crop_rotation_data[10].Split('|')[i].Split(';')[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            date_weediness_long = date_weediness.Ticks;
                                                        }

                                                        add_weediness = new SqlCommand("Add_History_book_weediness", conn);
                                                        add_weediness.CommandType = CommandType.StoredProcedure;
                                                        add_weediness.Parameters.AddWithValue("@id_history_book_field", id_history_book_field);
                                                        add_weediness.Parameters.AddWithValue("@weediness", Convert.ToInt32(NotNull(crop_rotation_data[10].Split('|')[i].Split(';')[0])));
                                                        add_weediness.Parameters.AddWithValue("@weediness_percent", Convert.ToDouble(NotNull(crop_rotation_data[10].Split('|')[i].Split(';')[1])));
                                                        add_weediness.Parameters.AddWithValue("@date_weediness_long", date_weediness_long);
                                                        add_weediness.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    conn.Close();
                                    j_script = "alert('Данные успешно сохранены!')";
                                }
                            }
                            catch (Exception e)
                            {
                                j_script = "alert('" + e.Message + "')";
                            }
                            break;
                        }
                    case "login":
                        {
                            String login = eventArgument.Split(':')[1].Split('|')[0];
                            String md5Password = GetMd5Hash(eventArgument.Split(':')[1].Split('|')[1]);
                            Int32 type_user = 0;
                            if (connection_try)
                            {
                                String id_user = String.Empty, surname = String.Empty, name = String.Empty, patronymic = String.Empty;
                                conn.Open();
                                type_user = 0;
                                SqlCommand get_user_data = new SqlCommand("Get_User_Data", conn);
                                get_user_data.CommandType = CommandType.StoredProcedure;
                                get_user_data.Parameters.AddWithValue("@login", login);
                                get_user_data.Parameters.AddWithValue("@password", md5Password);
                                get_user_data.Parameters.Add("@id_user", SqlDbType.Int);
                                get_user_data.Parameters["@id_user"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@surname", SqlDbType.VarChar, 30);
                                get_user_data.Parameters["@surname"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@name", SqlDbType.VarChar, 20);
                                get_user_data.Parameters["@name"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@patronymic", SqlDbType.VarChar, 30);
                                get_user_data.Parameters["@patronymic"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@read_role", SqlDbType.Bit);
                                get_user_data.Parameters["@read_role"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@edit_role", SqlDbType.Bit);
                                get_user_data.Parameters["@edit_role"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@add_role", SqlDbType.Bit);
                                get_user_data.Parameters["@add_role"].Direction = ParameterDirection.Output;
                                get_user_data.Parameters.Add("@delete_role", SqlDbType.Bit);
                                get_user_data.Parameters["@delete_role"].Direction = ParameterDirection.Output;
                                get_user_data.ExecuteNonQuery();

                                if (get_user_data.Parameters["@id_user"].Value.ToString() == "0")
                                {
                                    type_user = 1;
                                    SqlCommand get_outside_user = new SqlCommand("Get_Outside_User", conn);
                                    get_outside_user.CommandType = CommandType.StoredProcedure;
                                    get_outside_user.Parameters.AddWithValue("@login", login);
                                    get_outside_user.Parameters.AddWithValue("@password", md5Password);
                                    get_outside_user.Parameters.Add("@id_outside_user", SqlDbType.Int);
                                    get_outside_user.Parameters["@id_outside_user"].Direction = ParameterDirection.Output;
                                    get_outside_user.Parameters.Add("@surname", SqlDbType.VarChar, 30);
                                    get_outside_user.Parameters["@surname"].Direction = ParameterDirection.Output;
                                    get_outside_user.Parameters.Add("@name", SqlDbType.VarChar, 20);
                                    get_outside_user.Parameters["@name"].Direction = ParameterDirection.Output;
                                    get_outside_user.Parameters.Add("@patronymic", SqlDbType.VarChar, 30);
                                    get_outside_user.Parameters["@patronymic"].Direction = ParameterDirection.Output;
                                    get_outside_user.ExecuteNonQuery();
                                    if (get_outside_user.Parameters["@id_outside_user"].Value.ToString() != "0")
                                    {
                                        id_user = get_outside_user.Parameters["@id_outside_user"].Value.ToString();
                                        surname = get_outside_user.Parameters["@surname"].Value.ToString();
                                        name = get_outside_user.Parameters["@name"].Value.ToString();
                                        patronymic = get_outside_user.Parameters["@patronymic"].Value.ToString();
                                    }
                                }
                                else
                                {
                                    id_user = get_user_data.Parameters["@id_user"].Value.ToString();
                                    surname = get_user_data.Parameters["@surname"].Value.ToString();
                                    name = get_user_data.Parameters["@name"].Value.ToString();
                                    patronymic = get_user_data.Parameters["@patronymic"].Value.ToString();
                                }
                                conn.Close();

                                if (id_user != String.Empty)
                                {
                                    j_script += ("\ndocument.cookie = 'Agrochim31_Map_User=id_user=" + id_user + "&surname=" +
                                                 surname + "&name=" + name + "&patronymic=" + patronymic + "&type_user=" + type_user.ToString() + "';");
                                    j_script += "\n$(\"#LoginW\").dialog('close');";

                                    //загрузка областей
                                    j_script += GetTerritoryJS(id_user, type_user);
                                    //загрузка районов
                                    //j_script += GetRegionsJS(id_user, type_user);
                                }
                                else
                                {
                                    j_script += "\nalert('Пользователь с таким логином и паролем не найден!');";
                                }
                            }
                            break;
                        }
                    case "select_territory":
                        {
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                DataSet stylesDS = new DataSet();
                                String id_territory = eventArgument.Split(':')[1].Split('|')[0];
                                String id_user = eventArgument.Split(':')[1].Split('|')[1];
                                Int32 type_user = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]);

                                j_script = "$(function (){";
                                j_script += GetRegionsJS(id_territory, id_user, type_user);
                                j_script += "\n$(\"#OrganizationCB\").empty();";
                                j_script += "\n$(\"#OrganizationCB\").append('<option value=\"0\"></option>');";
                                j_script += "});";

                                j_script += "\nif(vector_regions != null) { map.removeLayer(vector_regions); }";
                                j_script += "\nif(vector_organizations != null) { map.removeLayer(vector_organizations); }";
                                j_script += "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                j_script += RemoveLayers();

                                Boolean select_territory = false;

                                if (type_user == 0)
                                {
                                    select_territory = true;
                                }
                                else
                                {
                                    conn.Open();
                                    String select_access_territory_str = "SELECT TOP 1 * FROM Access_Territory WHERE id_territory = " + id_territory + " AND id_outside_user = " + id_user;
                                    SqlDataAdapter select_access_territory = new SqlDataAdapter(select_access_territory_str, conn);
                                    select_access_territory.Fill(layersDS, "Access_Territory");
                                    conn.Close();

                                    if (CheckRowsCount(layersDS, "Access_Territory"))
                                    {
                                        select_territory = true;
                                    }
                                    else
                                    {
                                        select_territory = false;
                                    }
                                }

                                if (select_territory)
                                {
                                    conn.Open();
                                    String get_regions_str = "exec [dbo].[GetRegionsGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                    SqlDataAdapter get_regions = new SqlDataAdapter(get_regions_str, conn);
                                    get_regions.Fill(layersDS, "Regions");
                                    conn.Close();

                                    String regions_feature_string = String.Empty, regions_feature_name = String.Empty, regions_properties_string = String.Empty;
                                    if (CheckRowsCount(layersDS, "Regions"))
                                    {
                                        j_script += "\nregions_source = new ol.source.Vector();\nvar regions_feature;";
                                        for (int i = 0; i < layersDS.Tables["Regions"].Rows.Count; i++)
                                        {
                                            regions_feature_string = "\nregions_feature = format.readFeature('" + layersDS.Tables["Regions"].Rows[i]["region_geo_json"] + "');";
                                            regions_feature_name = "\nregions_feature.name = '" + layersDS.Tables["Regions"].Rows[i]["code_region"] + "';";
                                            for (int j = 0; j < layersDS.Tables["Regions"].Columns.Count; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    regions_properties_string = "\nregions_feature.setProperties({'layer':'regions','id_feature':'" + i.ToString() + "'";
                                                }
                                                if (layersDS.Tables["Regions"].Columns[j].ColumnName.ToString() != "region_geo_json")
                                                {
                                                    if (regions_properties_string[regions_properties_string.Length - 1] != '{' && regions_properties_string[regions_properties_string.Length - 1] != ',')
                                                    {
                                                        regions_properties_string += ",";
                                                    }
                                                    regions_properties_string += ("'" + layersDS.Tables["Regions"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Regions"].Rows[i][j].ToString() + "'");
                                                }
                                                if (j == (layersDS.Tables["Regions"].Columns.Count - 1))
                                                {
                                                    regions_properties_string += "});";
                                                }
                                            }
                                            j_script += (regions_feature_string + regions_feature_name + regions_properties_string + "\nregions_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                            j_script += "\nregions_source.addFeature(regions_feature);";
                                        }
                                        j_script += "\nvar count_regions = regions_source.getFeatures().length; \nif(count_regions > 0){";
                                        j_script += "\nvector_regions = new ol.layer.Vector({source: regions_source, minResolution:100, maxResolution:600});";
                                        j_script += "\ncurrent_extent = regions_source.getExtent();";
                                        j_script += "\nmap.addLayer(vector_regions); map.getView().fit(current_extent, map.getSize());";
                                        j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";

                                        //задание стиля для районов
                                        j_script += "\nvar regions_colors = {};";
                                        j_script += "\nSetColors(regions_colors, " + layersDS.Tables["Regions"].Rows.Count + ", 0.2);";
                                        j_script += ("\nfunction SetStyles_Regions" + ClientID + "(){");
                                        j_script += "\nvar regions_styleCache = {};";
                                        j_script += "\nvar regions_style = function (feature, resolution){";
                                        j_script += ("\nvar text = feature.name;");
                                        j_script += "\nif (!regions_styleCache[text]){";
                                        String fill_str, stroke_str, font_str;
                                        j_script += "regions_styleCache[text] = [new ol.style.Style({";
                                        fill_str = "\nfill: new ol.style.Fill({";
                                        fill_str += "\ncolor: regions_colors[feature.getProperties().id_feature]})";
                                        stroke_str = "\nstroke: new ol.style.Stroke({";
                                        stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                                        stroke_str += "\nwidth: 1})";
                                        font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                        font_str += "\ntext: feature.getProperties().title_region,";
                                        font_str += "\nfill: new ol.style.Fill({";
                                        font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                        font_str += "\nstroke: new ol.style.Stroke({";
                                        font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                        font_str += "\nwidth: 2})})";

                                        j_script += fill_str;
                                        j_script += ("," + stroke_str);
                                        j_script += ("," + font_str);
                                        j_script += "})];";

                                        j_script += "\n}\nreturn regions_styleCache[text];};";
                                        j_script += "\nvector_regions.setStyle(regions_style);}";
                                        j_script += "\nSetStyles_Regions" + ClientID + "();";
                                    }
                                }

                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "regions_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            j_script += "\nvar theme = $(\"#MapThemeCB\").val();" +
                                        "\nif($(\"#OrganizationCB\").val() != 0){CallServer(theme, 'null');}" +
                                        "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + theme, 'null');}" +
                                        "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + theme, 'null');}" +
                                        "\nCallServer('legend:' + theme, 'null');";
                            j_script += "\nShowHideFarm();";

                            j_script += "\nCallServer('get_tours_years_by_filter:id_territory=" + eventArgument.Split(':')[1].Split('|')[0] + "', 'null');";
                            break;
                        }
                    case "select_region":
                        {
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                DataSet stylesDS = new DataSet();
                                String id_region = eventArgument.Split(':')[1].Split('|')[0];
                                String id_user = eventArgument.Split(':')[1].Split('|')[1];
                                Int32 type_user = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]);

                                j_script = "$(function (){";
                                j_script += GetOrganizationsJS(id_region, id_user, type_user);
                                j_script += "});";

                                j_script += "\nif(vector_organizations != null) { map.removeLayer(vector_organizations); }";
                                j_script += "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                j_script += RemoveLayers();

                                Boolean select_region = false;

                                if (type_user == 0)
                                {
                                    select_region = true;
                                }
                                else
                                {
                                    conn.Open();
                                    String select_access_region_str = "SELECT TOP 1 * FROM Access_Region WHERE id_region = " + id_region + " AND id_outside_user = " + id_user;
                                    SqlDataAdapter select_access_region = new SqlDataAdapter(select_access_region_str, conn);
                                    select_access_region.Fill(layersDS, "Access_Region");
                                    conn.Close();

                                    if (CheckRowsCount(layersDS, "Access_Region"))
                                    {
                                        select_region = true;
                                    }
                                    else
                                    {
                                        select_region = false;
                                    }
                                }

                                if (select_region)
                                {
                                    conn.Open();
                                    String get_organization_str = "exec [dbo].[GetOrganizationsGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                    SqlDataAdapter get_organization = new SqlDataAdapter(get_organization_str, conn);
                                    get_organization.Fill(layersDS, "Organizations");
                                    conn.Close();

                                    String organizations_feature_string = String.Empty, organizations_feature_name = String.Empty, organizations_properties_string = String.Empty;
                                    if (CheckRowsCount(layersDS, "Organizations"))
                                    {
                                        j_script += "\norganizations_source = new ol.source.Vector();\nvar organizations_feature;";
                                        for (int i = 0; i < layersDS.Tables["Organizations"].Rows.Count; i++)
                                        {
                                            organizations_feature_string = "\norganizations_feature = format.readFeature('" + layersDS.Tables["Organizations"].Rows[i]["organization_geo_json"] + "');";
                                            organizations_feature_name = "\norganizations_feature.name = '" + layersDS.Tables["Organizations"].Rows[i]["code_organization"] + "';";
                                            for (int j = 0; j < layersDS.Tables["Organizations"].Columns.Count; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    organizations_properties_string = "\norganizations_feature.setProperties({'layer':'organizations','id_feature':'" + i.ToString() + "'";
                                                }
                                                if (layersDS.Tables["Organizations"].Columns[j].ColumnName.ToString() != "organization_geo_json")
                                                {
                                                    if (organizations_properties_string[organizations_properties_string.Length - 1] != '{' && organizations_properties_string[organizations_properties_string.Length - 1] != ',')
                                                    {
                                                        organizations_properties_string += ",";
                                                    }
                                                    organizations_properties_string += ("'" + layersDS.Tables["Organizations"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Organizations"].Rows[i][j].ToString() + "'");
                                                }
                                                if (j == (layersDS.Tables["Organizations"].Columns.Count - 1))
                                                {
                                                    organizations_properties_string += "});";
                                                }
                                            }
                                            j_script += (organizations_feature_string + organizations_feature_name + organizations_properties_string + "\norganizations_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                            j_script += "\norganizations_source.addFeature(organizations_feature);";
                                        }
                                        j_script += "\nvar count_organizations = organizations_source.getFeatures().length; \nif(count_organizations > 0){";
                                        j_script += "\nvector_organizations = new ol.layer.Vector({source: organizations_source, minResolution:50, maxResolution:300});";
                                        j_script += "\ncurrent_extent = organizations_source.getExtent();";
                                        j_script += "\nmap.addLayer(vector_organizations); map.getView().fit(current_extent, map.getSize());";
                                        j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";

                                        //задание стиля для организаций
                                        j_script += "\nvar organization_colors = {};";
                                        j_script += "\nSetColors(organization_colors, " + layersDS.Tables["Organizations"].Rows.Count + ", 0.2);";
                                        j_script += ("\nfunction " + "SetStyles_Organizations" + ClientID + "(){");
                                        j_script += "\nvar organization_styleCache = {};";
                                        j_script += "\nvar organization_style = function (feature, resolution){";
                                        j_script += ("\nvar text = feature.name;");
                                        j_script += "\nif (!organization_styleCache[text]){";
                                        j_script += ("\nswitch (feature.name){");
                                        String fill_str, stroke_str, font_str;
                                        for (int i = 0; i < layersDS.Tables["Organizations"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + layersDS.Tables["Organizations"].Rows[i]["code_organization"].ToString() + "':{organization_styleCache[text] = [new ol.style.Style({");
                                            fill_str = "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: organization_colors[" + i + "]})");
                                            stroke_str = "\nstroke: new ol.style.Stroke({";
                                            stroke_str += "\ncolor: 'rgba(255,125,125,0.5)',";
                                            stroke_str += "\nwidth: 1})";
                                            font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                            font_str += "\ntext: feature.getProperties().title_organization,";
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                            font_str += "\nwidth: 2})})";

                                            j_script += fill_str;
                                            j_script += ("," + stroke_str);
                                            j_script += ("," + font_str);
                                            j_script += "})];\nbreak;}";
                                        }
                                        j_script += "}\n}\nreturn organization_styleCache[text];};";
                                        j_script += "\nvector_organizations.setStyle(organization_style);}";
                                        j_script += ("\nSetStyles_Organizations" + ClientID + "();");
                                    }
                                }
                            }
                            j_script += "\nvar theme = $(\"#MapThemeCB\").val();" +
                                        "\nif($(\"#OrganizationCB\").val() != 0){CallServer(theme, 'null');}" +
                                        "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + theme, 'null');}" +
                                        "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + theme, 'null');}" +
                                        "\nCallServer('legend:' + theme, 'null');";
                            j_script += "\nShowHideFarm();";

                            j_script += "\nCallServer('get_tours_years_by_filter:id_region=" + eventArgument.Split(':')[1].Split('|')[0] + "', 'null');";
                            break;
                        }
                    case "select_organization":
                        {
                            String id_org = eventArgument.Split(':')[1].Split('|')[0];
                            String id_user = eventArgument.Split(':')[1].Split('|')[1];
                            Int32 type_user = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]);
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                j_script += RemoveLayers();

                                Boolean select_plots = false;

                                if (type_user == 0)
                                {
                                    select_plots = true;
                                }
                                else
                                {
                                    conn.Open();
                                    String select_access_org_str = "SELECT TOP 1 * FROM Access_Organization WHERE id_organization = " + id_org + " AND id_outside_user = " + id_user;
                                    SqlDataAdapter select_access_org = new SqlDataAdapter(select_access_org_str, conn);
                                    select_access_org.Fill(layersDS, "Access_Organization");
                                    conn.Close();
                                    if (CheckRowsCount(layersDS, "Access_Organization"))
                                    {
                                        select_plots = true;
                                    }
                                    else
                                    {
                                        select_plots = false;

                                    }
                                }

                                if (select_plots)
                                {
                                    conn.Open();
                                    String get_plots_str = "exec [dbo].[GetLastPlotsGeoJSON] " + id_org;
                                    SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                    plots_geo_data.Fill(layersDS, "Plots");
                                    conn.Close();

                                    String feature_string = String.Empty,
                                           feature_name = String.Empty,
                                           properties_string = String.Empty;
                                    if (CheckRowsCount(layersDS, "Plots"))
                                    {
                                        j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                   "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                   "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                        id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();
                                        year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                        tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                        code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                        j_script += "\nplots_source = new ol.source.Vector();\nvar feature;";
                                        for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                        {
                                            feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                            feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                            for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                }
                                                if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                {
                                                    if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                    {
                                                        properties_string += ",";
                                                    }
                                                    properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                }
                                                if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                {
                                                    properties_string += "});";
                                                }
                                            }
                                            j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                            j_script += "\nplots_source.addFeature(feature);";
                                        }
                                        j_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                        j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:100});";

                                        j_script += "\ncurrent_extent = plots_source.getExtent();";
                                        j_script += "\nmap.addLayer(vector_plots); vector_plots.setZIndex(1); map.getView().fit(current_extent, map.getSize());";
                                        j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                        j_script += "\nShowLegend();";

                                        //js_script += "\nvector_plots.on('select', function (){if(selectClick.getFeatures().getLength() > 0){var code_plot = selectClick.getFeatures()[0].getProperties().code_plot; CallServer(('code_plot:' + code_plot + '|' + $('#Year3CB').val()), 'null');}});";
                                        /*j_script += ("\nif (selectClick != null){map.on('click', function (e) {" +
                                                     "map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {" +
                                                     "if (feature.getGeometry().getType() == 'Point'){" +
                                                     "CallServer(('soil_point:' + feature.name), 'null');}" +
                                                     "else if (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon'){" +
                                                     "CallServer(('code_plot:' + feature.name + '|' + $('#Year3CB').val()), 'null');}});});}");*/
                                    }
                                }
                            }
                            j_script += "\nvar theme = $(\"#MapThemeCB\").val();" +
                                        "\nif($(\"#OrganizationCB\").val() != 0){CallServer(theme, 'null');}" +
                                        "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + theme, 'null');}" +
                                        "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + theme, 'null');}" +
                                        "\nCallServer('legend:' + theme, 'null');";
                            j_script += "\nCallServer('get_tours_years_by_filter:id_organization=" + id_org + "', 'null');";
                            j_script += "\n$(\"#TypePlotCB option:selected\").each(function() { $(this).prop('selected', false); });";
                            j_script += "\n$(\"#TypePlotCB option[value='0']\").prop('selected', true);";

                            //j_script += "\nmap.updateSize();";
                            break;
                        }
                    case "change_sort_crop_rotation":
                        {
                            j_script += GetSortCropRotationDescription(eventArgument.Split(':')[1].ToString());
                            break;
                        }
                    case "soil_point":
                        {
                            if (connection_try)
                            {
                                conn.Open();
                                DataSet tablesDS = new DataSet();
                                String get_soil_sample_str = "exec [dbo].[GetSoilPointInfoById] " + eventArgument.Split(':')[1];
                                SqlDataAdapter get_soil_sample = new SqlDataAdapter(get_soil_sample_str, conn);
                                get_soil_sample.Fill(tablesDS, "SoilSample");
                                conn.Close();

                                j_script = "$(function (){";
                                //j_script += "\nfor (var i = 0; i < $(\"#SoilSampleT tr\").size() ; i++) {$(\"#SoilSampleT tr\").eq(i).remove();}";
                                j_script += "\n$(\"#SoilSampleT\").html('');";
                                if (CheckRowsCount(tablesDS, "SoilSample"))
                                {
                                    j_script += "\n$(\"#SoilSampleYearTB\").val('" + tablesDS.Tables["SoilSample"].Rows[0]["year"].ToString() + "');";
                                    for (int i = 0; i < tablesDS.Tables["SoilSample"].Rows.Count; i++)
                                    {
                                        j_script += "\n$(\"#SoilSampleT\").append('<tr><td hidden=\"hidden\">" + tablesDS.Tables["SoilSample"].Rows[i]["id_soil_sample"].ToString() +
                                                    "</td><td hidden=\"hidden\">" + tablesDS.Tables["SoilSample"].Rows[i]["id_geographic_soil_point"].ToString() +
                                                    "</td><td width=\"70\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["name_point"].ToString() +
                                                    "</td><td width=\"75\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["soil_horizon"].ToString() +
                                                    "</td><td width=\"75\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["depth"].ToString() +
                                                    "</td><td width=\"55\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["humus_soil_sample"].ToString() +
                                                    "</td><td width=\"45\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["p2o5_soil_sample"].ToString() +
                                                    "</td><td width=\"40\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["k2o_soil_sample"].ToString() +
                                                    "</td><td width=\"50\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["ph_w_soil_sample"].ToString() +
                                                    "</td><td width=\"50\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["ph_s_soil_sample"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["hydrolytic_acid_soil_sample"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["absorbed_base_soil_sample"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["ca_soil_sample"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["mg_soil_sample"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["na_soil_sample"].ToString() +
                                                    "</td><td width=\"75\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["coarse_sand"].ToString() +
                                                    "</td><td width=\"70\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["fine_sand"].ToString() +
                                                    "</td><td width=\"70\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["coarse_dust"].ToString() +
                                                    "</td><td width=\"80\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["medium_dust"].ToString() +
                                                    "</td><td width=\"90\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["fine_dust"].ToString() +
                                                    "</td><td width=\"50\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["silt"].ToString() +
                                                    "</td><td width=\"80\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["content_physical_clay"].ToString() +
                                                    "</td><td width=\"105\" align=\"center\">" + tablesDS.Tables["SoilSample"].Rows[i]["soil_sample_grading"].ToString() +
                                                    "</td></tr>');";
                                    }
                                }
                                j_script += "});";
                            }
                            break;
                        }
                    case "total_dose":
                        {
                            if (connection_try)
                            {
                                DataSet tablesDS = new DataSet();
                                conn.Open();
                                String get_basic_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[0];
                                SqlDataAdapter get_basic_fert = new SqlDataAdapter(get_basic_fert_str, conn);
                                get_basic_fert.Fill(tablesDS, "BasicFert");

                                String get_sowing_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[2];
                                SqlDataAdapter get_sowing_fert = new SqlDataAdapter(get_sowing_fert_str, conn);
                                get_sowing_fert.Fill(tablesDS, "SowingFert");

                                String get_dressing_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[4];
                                SqlDataAdapter get_dressing_fert = new SqlDataAdapter(get_dressing_fert_str, conn);
                                get_dressing_fert.Fill(tablesDS, "DressingFert");
                                conn.Close();

                                Double N = 0.0, P = 0.0, K = 0.0;

                                if (CheckRowsCount(tablesDS, "BasicFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["BasicFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["BasicFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["BasicFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                }
                                if (CheckRowsCount(tablesDS, "SowingFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["SowingFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["SowingFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["SowingFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                }
                                if (CheckRowsCount(tablesDS, "DressingFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["DressingFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["DressingFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["DressingFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                }

                                j_script = "\n$(function () {";
                                j_script += "\n$(\"#TotalDoseNTB\").val('" + N.ToString() + "');";
                                j_script += "\n$(\"#TotalDosePTB\").val('" + P.ToString() + "');";
                                j_script += "\n$(\"#TotalDoseKTB\").val('" + K.ToString() + "');";
                                j_script += "\n});";
                            }
                            break;
                        }
                    case "old_total_dose":
                        {
                            if (connection_try)
                            {
                                DataSet tablesDS = new DataSet();
                                conn.Open();
                                String get_basic_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[0];
                                SqlDataAdapter get_basic_fert = new SqlDataAdapter(get_basic_fert_str, conn);
                                get_basic_fert.Fill(tablesDS, "OldBasicFert");

                                String get_sowing_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[2];
                                SqlDataAdapter get_sowing_fert = new SqlDataAdapter(get_sowing_fert_str, conn);
                                get_sowing_fert.Fill(tablesDS, "OldSowingFert");

                                String get_dressing_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[4];
                                SqlDataAdapter get_dressing_fert = new SqlDataAdapter(get_dressing_fert_str, conn);
                                get_dressing_fert.Fill(tablesDS, "OldDressingFert");
                                conn.Close();

                                Double N = 0.0, P = 0.0, K = 0.0;

                                if (CheckRowsCount(tablesDS, "OldBasicFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldBasicFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldBasicFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldBasicFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) / 100), 2);
                                }
                                if (CheckRowsCount(tablesDS, "OldSowingFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldSowingFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldSowingFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldSowingFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[3])) / 100), 2);
                                }
                                if (CheckRowsCount(tablesDS, "OldDressingFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldDressingFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldDressingFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldDressingFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[5])) / 100), 2);
                                }

                                j_script = "\n$(function () {";
                                j_script += "\n$(\"#OldTotalDoseNTB\").val('" + N.ToString() + "');";
                                j_script += "\n$(\"#OldTotalDosePTB\").val('" + P.ToString() + "');";
                                j_script += "\n$(\"#OldTotalDoseKTB\").val('" + K.ToString() + "');";
                                j_script += "\n});";
                            }
                            break;
                        }
                    case "org_total_dose":
                        {
                            if (connection_try)
                            {
                                DataSet tablesDS = new DataSet();
                                String get_basic_fert_str;
                                if (NotNull(eventArgument.Split(':')[1].Split('|')[2]) == "0" || eventArgument.Split(':')[1].Split('|')[2] == "undefined")
                                {
                                    get_basic_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[0];
                                }
                                else
                                {
                                    get_basic_fert_str = "SELECT n, p, k FROM View_Protocols_NPK WHERE id_protocol = " + eventArgument.Split(':')[1].Split('|')[2];
                                }

                                conn.Open();
                                SqlDataAdapter get_basic_fert = new SqlDataAdapter(get_basic_fert_str, conn);
                                get_basic_fert.Fill(tablesDS, "OrganicFert");
                                conn.Close();

                                Double N = 0.0, P = 0.0, K = 0.0;

                                if (CheckRowsCount(tablesDS, "OrganicFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OrganicFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OrganicFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OrganicFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                }

                                j_script = "\n$(function () {";
                                j_script += "\n$(\"#OrganicTotalDoseNTB\").val('" + N.ToString() + "');";
                                j_script += "\n$(\"#OrganicTotalDosePTB\").val('" + P.ToString() + "');";
                                j_script += "\n$(\"#OrganicTotalDoseKTB\").val('" + K.ToString() + "');";
                                j_script += "\n});";
                            }
                            break;
                        }
                    case "old_org_total_dose":
                        {
                            if (connection_try)
                            {
                                DataSet tablesDS = new DataSet();
                                conn.Open();
                                String get_basic_fert_str = "SELECT * FROM Fertilizer WHERE id_fertilizer = " + eventArgument.Split(':')[1].Split('|')[0];
                                SqlDataAdapter get_basic_fert = new SqlDataAdapter(get_basic_fert_str, conn);
                                get_basic_fert.Fill(tablesDS, "OldOrganicFert");
                                conn.Close();

                                Double N = 0.0, P = 0.0, K = 0.0;

                                if (CheckRowsCount(tablesDS, "OldOrganicFert"))
                                {
                                    N += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldOrganicFert"].Rows[0]["n"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                    P += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldOrganicFert"].Rows[0]["p"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                    K += Round((Convert.ToDouble(NotNull(tablesDS.Tables["OldOrganicFert"].Rows[0]["k"].ToString())) * Convert.ToDouble(NotNull(eventArgument.Split(':')[1].Split('|')[1])) * 10), 2);
                                }

                                j_script = "\n$(function () {";
                                j_script += "\n$(\"#OldOrganicTotalDoseNTB\").val('" + N.ToString() + "');";
                                j_script += "\n$(\"#OldOrganicTotalDosePTB\").val('" + P.ToString() + "');";
                                j_script += "\n$(\"#OldOrganicTotalDoseKTB\").val('" + K.ToString() + "');";
                                j_script += "\n});";
                            }
                            break;
                        }
                    case "region_theme":
                        {
                            if (connection_try)
                            {
                                if (eventArgument.Split(':')[1] == "p2o5" || eventArgument.Split(':')[1] == "k2o" || eventArgument.Split(':')[1] == "ph_s" || eventArgument.Split(':')[1] == "humus")
                                {
                                    conn.Open();
                                    String get_styles_str = "exec [dbo].[SelectGroupsStyleBySignificative] '" + eventArgument.Split(':')[1] + "'";
                                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                                    DataSet stylesDS = new DataSet();
                                    get_styles.Fill(stylesDS, "GroupsStyles");
                                    conn.Close();

                                    if (CheckRowsCount(stylesDS, "GroupsStyles"))
                                    {
                                        j_script = ("\nfunction SetStyles_" + eventArgument.Split(':')[1] + ClientID + "_Region(){");
                                        j_script += "\nvar regions_styleCache = {};";
                                        j_script += "\nvar regions_style = function (feature, resolution){";
                                        j_script += ("\nvar regions_text = feature.getProperties().code_region;");
                                        j_script += "\nif (!regions_styleCache[regions_text]){";
                                        j_script += ("\nswitch (feature.getProperties().number_" + eventArgument.Split(':')[1] + "_group){");
                                        String fill_str, stroke_str, font_str;
                                        for (int i = 0; i < stylesDS.Tables["GroupsStyles"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + (i + 1).ToString() + "':{regions_styleCache[regions_text] = [new ol.style.Style({");
                                            fill_str = String.Empty;
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                            {
                                                fill_str += "\nfill: new ol.style.Fill({";
                                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                            }
                                            stroke_str = String.Empty;
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                            {
                                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                stroke_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_width"].ToString());
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                                {
                                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                stroke_str += "\n})";
                                            }
                                            font_str = String.Empty;
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {
                                                font_str += "\ntext: new ol.style.Text({font: '";
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                                {
                                                    font_str += "bold ";
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                                {
                                                    font_str += "italic ";
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                                {
                                                    font_str += "underline ";
                                                }
                                                font_str += (stylesDS.Tables["GroupsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_name"].ToString() + "',");
                                                font_str += "\ntext: feature.getProperties().title_region,";
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetX: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetY: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                                }
                                                font_str += "\nfill: new ol.style.Fill({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                font_str += "\nstroke: new ol.style.Stroke({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                font_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_width"].ToString());
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                {
                                                    font_str += ",\nlineDash: [0.5, 4]})";
                                                }
                                                font_str += "\n})})";
                                            }

                                            if (fill_str != String.Empty)
                                            {
                                                j_script += fill_str;
                                            }
                                            if (stroke_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += stroke_str;
                                            }
                                            if (font_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += font_str;
                                            }
                                            j_script += "})];\nbreak;}";
                                        }
                                        j_script += "}\n}\nreturn regions_styleCache[regions_text];};";
                                        j_script += "\nif(vector_regions != null){vector_regions.setStyle(regions_style);}}";
                                        j_script += ("\nSetStyles_" + eventArgument.Split(':')[1] + ClientID + "_Region();");

                                        /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "regions_style_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                        SW.Write(j_script);
                                        SW.Close();*/
                                    }
                                }
                                //if (eventArgument.Split(':')[1] == "null")

                                else
                                {
                                    j_script = "\nif(vector_regions.getSource() != null){";
                                    j_script += "\nvar regions_colors = {};";
                                    j_script += "\nSetColors(regions_colors, vector_regions.getSource().getFeatures().length, 0.2);";
                                    j_script += ("\nfunction SetStyles_Regions(){");
                                    j_script += "\nvar regions_styleCache = {};";
                                    j_script += "\nvar regions_style = function (feature, resolution){";
                                    j_script += ("\nvar regions_text = feature.name;");
                                    j_script += "\nif (!regions_styleCache[regions_text]){";
                                    String fill_str, stroke_str, font_str;
                                    j_script += "regions_styleCache[regions_text] = [new ol.style.Style({";
                                    fill_str = "\nfill: new ol.style.Fill({";
                                    fill_str += "\ncolor: regions_colors[feature.getProperties().id_feature]})";
                                    stroke_str = "\nstroke: new ol.style.Stroke({";
                                    stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                                    stroke_str += "\nwidth: 1})";
                                    font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                    font_str += "\ntext: feature.getProperties().title_region,";
                                    font_str += "\nfill: new ol.style.Fill({";
                                    font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                    font_str += "\nstroke: new ol.style.Stroke({";
                                    font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                    font_str += "\nwidth: 2})})";

                                    j_script += fill_str;
                                    j_script += ("," + stroke_str);
                                    j_script += ("," + font_str);
                                    j_script += "})];";

                                    j_script += "\n}\nreturn regions_styleCache[regions_text];};";
                                    j_script += "\nvector_regions.setStyle(regions_style);}";
                                    j_script += "\nSetStyles_Regions();";
                                    j_script += "\n}";
                                }
                            }
                            break;
                        }
                    case "territory_theme":
                        {
                            if (connection_try)
                            {
                                if (eventArgument.Split(':')[1] == "p2o5" || eventArgument.Split(':')[1] == "k2o" || eventArgument.Split(':')[1] == "ph_s" || eventArgument.Split(':')[1] == "humus")
                                {
                                    conn.Open();
                                    String get_styles_str = "exec [dbo].[SelectGroupsStyleBySignificative] '" + eventArgument.Split(':')[1] + "'";
                                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                                    DataSet stylesDS = new DataSet();
                                    get_styles.Fill(stylesDS, "GroupsStyles");
                                    conn.Close();

                                    if (CheckRowsCount(stylesDS, "GroupsStyles"))
                                    {
                                        j_script = ("\nfunction SetStyles_" + eventArgument.Split(':')[1] + ClientID + "_Territory(){");
                                        j_script += "\nvar territory_styleCache = {};";
                                        j_script += "\nvar territory_style = function (feature, resolution){";
                                        j_script += ("\nvar territory_text = feature.getProperties().code_territory;");
                                        j_script += "\nif (!territory_styleCache[territory_text]){";
                                        j_script += ("\nswitch (feature.getProperties().number_" + eventArgument.Split(':')[1] + "_group){");
                                        String fill_str, stroke_str, font_str;
                                        for (int i = 0; i < stylesDS.Tables["GroupsStyles"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + (i + 1).ToString() + "':{territory_styleCache[territory_text] = [new ol.style.Style({");
                                            fill_str = String.Empty;
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                            {
                                                fill_str += "\nfill: new ol.style.Fill({";
                                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                            }
                                            stroke_str = String.Empty;
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                            {
                                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                stroke_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_width"].ToString());
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                                {
                                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                stroke_str += "\n})";
                                            }
                                            font_str = String.Empty;
                                            /*if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {
                                                font_str += "\ntext: new ol.style.Text({font: '";
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                                {
                                                    font_str += "bold ";
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                                {
                                                    font_str += "italic ";
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                                {
                                                    font_str += "underline ";
                                                }
                                                font_str += (stylesDS.Tables["GroupsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_name"].ToString() + "',");
                                                font_str += "\ntext: feature.getProperties().title_territory,";
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetX: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                                }
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetY: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                                }
                                                font_str += "\nfill: new ol.style.Fill({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                font_str += "\nstroke: new ol.style.Stroke({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                font_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_width"].ToString());
                                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                {
                                                    font_str += ",\nlineDash: [0.5, 4]})";
                                                }
                                                font_str += "\n})})";
                                            }*/

                                            if (fill_str != String.Empty)
                                            {
                                                j_script += fill_str;
                                            }
                                            if (stroke_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += stroke_str;
                                            }
                                            if (font_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += font_str;
                                            }
                                            j_script += "})];\nbreak;}";
                                        }
                                        j_script += "}\n}\nreturn territory_styleCache[territory_text];};";
                                        j_script += "\nif(vector_territory != null){vector_territory.setStyle(territory_style);}}";
                                        j_script += ("\nSetStyles_" + eventArgument.Split(':')[1] + ClientID + "_Territory();");

                                        /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "territory_style_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                        SW.Write(j_script);
                                        SW.Close();*/
                                    }
                                }
                                //if (eventArgument.Split(':')[1] == "null")

                                else
                                {
                                    j_script = "\nif(vector_territory.getSource() != null){";
                                    j_script += "\nvar territory_colors = {};";
                                    j_script += "\nSetColors(territory_colors, vector_territory.getSource().getFeatures().length, 0.2);";
                                    //j_script += "\nSetColors(territory_colors, territory_source.getFeatures().length, 0.2);";
                                    j_script += ("\nfunction SetStyles_Territory(){");
                                    j_script += "\nvar territory_styleCache = {};";
                                    j_script += "\nvar territory_style = function (feature, resolution){";
                                    j_script += ("\nvar territory_text = feature.name;");
                                    j_script += "\nif (!territory_styleCache[territory_text]){";
                                    String fill_str, stroke_str;// font_str;
                                    j_script += "territory_styleCache[territory_text] = [new ol.style.Style({";
                                    fill_str = "\nfill: new ol.style.Fill({";
                                    fill_str += "\ncolor: territory_colors[feature.getProperties().id_feature]})";
                                    stroke_str = "\nstroke: new ol.style.Stroke({";
                                    stroke_str += "\ncolor: 'rgba(0,255,255,0.5)',";
                                    stroke_str += "\nwidth: 1})";
                                    /*font_str = "\ntext: new ol.style.Text({font: 'Arial 14px',";
                                    font_str += "\ntext: feature.getProperties().title_territory,";
                                    font_str += "\nfill: new ol.style.Fill({";
                                    font_str += "\ncolor: 'rgba(0,0,0,1)'}),";
                                    font_str += "\nstroke: new ol.style.Stroke({";
                                    font_str += "\ncolor: 'rgba(255,255,255,1)',";
                                    font_str += "\nwidth: 2})})";*/

                                    j_script += fill_str;
                                    j_script += ("," + stroke_str);
                                    //j_script += ("," + font_str);
                                    j_script += "})];";

                                    j_script += "\n}\nreturn territory_styleCache[territory_text];};";
                                    j_script += "\nvector_territory.setStyle(territory_style);}";
                                    j_script += "\nSetStyles_Territory();";
                                    j_script += "\n}";
                                }
                            }
                            break;
                        }
                    case "legend":
                        {
                            j_script = GetLegendJS(eventArgument.Split(':')[1], id_organization, tour, year);
                            break;
                        }
                    case "gettrackers":
                        {
                            //eventArgument.Split(':')[1]
                            if (connection_try)
                            {
                                String id_org = eventArgument.Split(':')[1].Split('|')[0];
                                if (eventArgument.Split(':')[1].Split('|')[2] == "0") { id_org = "0"; }

                                DataSet stylesDS = new DataSet();
                                conn.Open();
                                String get_styles_str = "exec [dbo].[GetGPSTrackersStyle] '" + id_org + "'";
                                SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                                get_styles.Fill(stylesDS, "GPSTrackersStyle");
                                conn.Close();
                                //GPSTrackersT
                                j_script = "\nvar trackers_table =  document.getElementById('GPSTrackersT');";
                                j_script += "\ntrackers_table.innerHTML = '';";
                                j_script += "\ntrackers_table.innerHTML = '<tr class=\"ui-widget-header\">";
                                j_script += "<td width=\"30\"></td>";
                                j_script += "<td hidden=\"hidden\">ID трекера</td>";
                                j_script += "<td width=\"150\">Автомобиль</td>";
                                j_script += "<td width=\"100\">Гос. номер</td>";
                                j_script += "<td width=\"150\">IMEI трекера</td>";
                                j_script += "<td width=\"10\">Вкл.</td>";
                                j_script += "<td width=\"100\">Расстояние</td>";
                                j_script += "</tr>";
                                String last_point_layers = String.Empty;
                                String preload_track = String.Empty;
                                for (int i = 0; i < stylesDS.Tables["GPSTrackersStyle"].Rows.Count; i++)
                                {
                                    String row = "<tr>";
                                    Int32 opacity = 0;
                                    opacity = Convert.ToInt32(Convert.ToDouble(NotNull(stylesDS.Tables["GPSTrackersStyle"].Rows[i]["opacity"].ToString())) * 255);

                                    row += ("<td width=\"30\"><hr style=\"height: 4px; border: none; background-color:");
                                    row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(stylesDS.Tables["GPSTrackersStyle"].Rows[i]["red"].ToString())),
                                        Convert.ToInt32(NotNull(stylesDS.Tables["GPSTrackersStyle"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(stylesDS.Tables["GPSTrackersStyle"].Rows[i]["blue"].ToString())))));
                                    row += ";\"></td><td hidden=\"hidden\">";
                                    row += stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString();
                                    row += "</td><td>";
                                    row += stylesDS.Tables["GPSTrackersStyle"].Rows[i]["car_model"].ToString();
                                    row += "</td><td>";
                                    row += stylesDS.Tables["GPSTrackersStyle"].Rows[i]["license_plate"].ToString();
                                    row += "</td><td>";
                                    row += stylesDS.Tables["GPSTrackersStyle"].Rows[i]["imei"].ToString();
                                    row += "</td><td>";
                                    row += "<input type=\"checkbox\" id=\"Tracker" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() +
                                            "CB\" class=\"tracker\" value =\"" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() +
                                            "\" onclick=\"ShowHideTracks(this.id, this.checked, 0);\">";
                                    row += "</td><td>";
                                    row += "<input size=\"9\" type=\"text\" id=\"Distance" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "\" readonly=\"true\">";
                                    row += "</td></tr>";
                                    j_script += row;
                                    last_point_layers += "\ntrack_last_point_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.source.Vector();";
                                    last_point_layers += "\nvector_track_last_point[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.layer.Vector({ source: track_last_point_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] });";
                                    last_point_layers += "\nvar lp_feature = format.readFeature('POINT (0 0)');";
                                    last_point_layers += "track_last_point_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "].addFeature(lp_feature);";
                                    last_point_layers += "\nmap.addLayer(vector_track_last_point[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "]);";

                                    preload_track += "\ntrack_point_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.source.Vector();";
                                    preload_track += "\ntrack_point_ln_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.source.Vector();";
                                    preload_track += "\nvector_track_points[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.layer.Vector({ source: track_point_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] });";
                                    preload_track += "\nvector_track_lines[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "] = new ol.layer.Vector({ source: track_point_ln_source[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "], maxResolution: 10 });";
                                    preload_track += "\nmap.addLayer(vector_track_points[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "]);";
                                    preload_track += "\nmap.addLayer(vector_track_lines[" + stylesDS.Tables["GPSTrackersStyle"].Rows[i]["id_gps_tracker"].ToString() + "]);";
                                }
                                j_script += "';";
                                j_script += "\nSetTracks(); SetUpdateTimer();";
                                j_script += last_point_layers;
                                j_script += preload_track;
                                j_script += "\n$(\"#GPSTrackersW\").dialog('open');";
                            }
                            break;
                        }
                    case "SHtracks":
                        {
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                conn.Open();
                                String[] args = eventArgument.Split(':')[1].Split('|');

                                String date_from_str = args[args.Length - 1].Split(';')[0];
                                String date_to_str = args[args.Length - 1].Split(';')[1];
                                Boolean is_end = Convert.ToBoolean(args[args.Length - 1].Split(';')[2]);
                                Int32 timeout_value = 300 + Convert.ToInt32(args[args.Length - 1].Split(';')[3]);
                                Int64 date_from_ticks, date_to_ticks;
                                int parts = 10;
                                if (date_from_str.Contains('.') && date_to_str.Contains('.'))
                                {
                                    DateTime date_from, date_to;
                                    date_from = DateTime.ParseExact(date_from_str + " 00:00:00", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    date_to = DateTime.ParseExact(date_to_str + " 23:59:59", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    date_from_ticks = date_from_str != String.Empty ? date_from.Ticks : 0;
                                    date_to_ticks = date_to_str != String.Empty ? date_to.Ticks : 0;
                                    if (date_to.Date == DateTime.Today.Date) j_script += "\nrealtime_tracking = true;";

                                    if (date_from_ticks > date_to_ticks)
                                    {
                                        date_from = DateTime.ParseExact(date_to_str + " 00:00:00", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                        date_to = DateTime.ParseExact(date_from_str + " 23:59:59", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                        date_from_ticks = date_to_str != String.Empty ? date_from.Ticks : 0;
                                        date_to_ticks = date_from_str != String.Empty ? date_to.Ticks : 0;
                                    }
                                }
                                else
                                {
                                    date_from_ticks = Convert.ToInt64(date_from_str);
                                    date_to_ticks = Convert.ToInt64(date_to_str);
                                }

                                Int32 id_tracker = Convert.ToInt32(args[0].Split(';')[0]);
                                Boolean is_checked = Convert.ToBoolean(args[0].Split(';')[1]);

                                String track_points_querry_string = "exec [dbo].[GetTrackPointsGeoJSON] " + id_tracker + ", " + date_from_ticks.ToString() + ", " + date_to_ticks.ToString();
                                SqlDataAdapter track_points_geo_data = new SqlDataAdapter(track_points_querry_string, conn);
                                track_points_geo_data.Fill(layersDS, "TrackPoints");

                                conn.Close();
                                int count = layersDS.Tables["TrackPoints"].Rows.Count;
                                if (count > 0)
                                {
                                    String str_dt = layersDS.Tables["TrackPoints"].Rows[count - 1]["datetime_point"].ToString();
                                    Int64 last_ticks = DateTime.ParseExact(str_dt, "dd.MM.yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks;
                                    j_script += "\nSetTrackersActive(0);";
                                    j_script += "\ntrack_current_date_rt[" + id_tracker.ToString() + "] = " + last_ticks.ToString() + ";";
                                    if (date_from_str.Contains('.') && date_to_str.Contains('.'))
                                    {
                                        if (count < parts) parts = 1;
                                        const int max_portion = 3000;
                                        int portion = count / parts;
                                        if (portion > max_portion) portion = max_portion;
                                        int threads = 1 + count / portion;
                                        int start = 0;
                                        for (int i = 0; i < threads; ++i)
                                        {
                                            j_script += "\nCallServer('showhidetracks:" + args[0].Split(';')[0] + ";" + args[0].Split(';')[1] + "|" + date_from_str + ";" + date_to_str + ";" + start.ToString() + ";" + (start + portion).ToString() + ";" + is_end.ToString() + ";" + timeout_value.ToString() + "');";
                                            start += portion;
                                        }
                                    }
                                    else
                                    {
                                        j_script += "\nCallServer('showhidetracks:" + args[0].Split(';')[0] + ";" + args[0].Split(';')[1] + "|" + date_from_str + ";" + date_to_str + ";" + 0.ToString() + ";" + count.ToString() + ";true;0');";
                                    }
                                }
                                else
                                {
                                    if (is_end)
                                    { j_script += "\nsetTimeout(function() { SetTrackersActive(1); }, " + (1000).ToString() + ");"; }
                                }
                            }
                            break;
                        }
                    case "callback_test":
                        {
                            break;
                        }
                    case "showhidetracks":
                        {
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                DataSet stylesDS = new DataSet();

                                conn.Open();
                                String[] args = eventArgument.Split(':')[1].Split('|');

                                String date_from_str = args[args.Length - 1].Split(';')[0];
                                String date_to_str = args[args.Length - 1].Split(';')[1];
                                Int32 start = Convert.ToInt32(args[args.Length - 1].Split(';')[2]);
                                Int32 end = Convert.ToInt32(args[args.Length - 1].Split(';')[3]);
                                Boolean is_end = Convert.ToBoolean(args[args.Length - 1].Split(';')[4]);
                                Int32 timeout_value = 300 + Convert.ToInt32(args[args.Length - 1].Split(';')[5]);
                                Int64 date_from_ticks, date_to_ticks;
                                Int32 min_interval = 1;
                                if (date_from_str.Contains('.') && date_to_str.Contains('.'))
                                {
                                    if (date_from_str == "NaN") date_from_str = String.Empty;
                                    if (date_to_str == "NaN") date_to_str = String.Empty;

                                    date_from_ticks = date_from_str != String.Empty ? DateTime.ParseExact(date_from_str + " 00:00:00", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks : 0;
                                    date_to_ticks = date_to_str != String.Empty ? DateTime.ParseExact(date_to_str + " 23:59:59", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks : 0;

                                    if (date_from_ticks > date_to_ticks)
                                    {
                                        date_from_ticks = date_to_str != String.Empty ? DateTime.ParseExact(date_to_str + " 00:00:00", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks : 0;
                                        date_to_ticks = date_from_str != String.Empty ? DateTime.ParseExact(date_from_str + " 23:59:59", "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks : 0;
                                    }
                                }
                                else
                                {
                                    if (date_from_str == "NaN" || date_from_str == String.Empty) date_from_str = "0";
                                    if (date_to_str == "NaN" || date_to_str == String.Empty) date_to_str = "0";

                                    date_from_ticks = Convert.ToInt64(date_from_str);
                                    date_to_ticks = Convert.ToInt64(date_to_str);
                                    min_interval = 0;
                                }
                                Int32 id_tracker = Convert.ToInt32(args[0].Split(';')[0]);
                                Boolean is_checked = Convert.ToBoolean(args[0].Split(';')[1]);

                                String track_points_querry_string = "exec [dbo].[GetTrackPointsGeoJSON] " + id_tracker + ", " + date_from_ticks.ToString() + ", " + date_to_ticks.ToString();
                                SqlDataAdapter track_points_geo_data = new SqlDataAdapter(track_points_querry_string, conn);
                                track_points_geo_data.Fill(layersDS, "TrackPoints");

                                String get_track_styles_str = "SELECT * FROM ViewGPSTrackersStyle";
                                SqlDataAdapter get_track_styles = new SqlDataAdapter(get_track_styles_str, conn);
                                get_track_styles.Fill(stylesDS, "TrackPointsStyles");

                                conn.Close();

                                if (end > layersDS.Tables["TrackPoints"].Rows.Count) end = layersDS.Tables["TrackPoints"].Rows.Count;
                                String str_dt = layersDS.Tables["TrackPoints"].Rows[end - 1]["datetime_point"].ToString();
                                Int64 last_ticks = DateTime.ParseExact(str_dt, "dd.MM.yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Ticks;
                                j_script += "\nif(track_current_date_rt[" + id_tracker.ToString() + "] < " + last_ticks.ToString() + ")";
                                j_script += "\n{ track_current_date_rt[" + id_tracker.ToString() + "] = " + last_ticks.ToString() + "; } ";

                                if (is_checked)
                                {
                                    if (CheckRowsCount(layersDS, "TrackPoints"))
                                    {
                                        String track_point_feature_string = String.Empty;
                                        String track_point_feature_name = String.Empty;
                                        String track_point_properties_string = String.Empty;

                                        //const Int32 max_interval = 3600;

                                        j_script += "\nvar track_point_feature_prev;";
                                        j_script += "\nvar track_point_feature;";
                                        Int32 i = start;
                                        /*if (start != 0) j_script += "\nvar D = Number($('#Distance" + layersDS.Tables["TrackPoints"].Rows[0]["id_gps_tracker"] + "').val().split(' ')[0]) * 1000;";
                                        else { j_script += "\nvar D = 0;"; ++start; }*/
                                        j_script += "\n$('#Distance" + layersDS.Tables["TrackPoints"].Rows[0]["id_gps_tracker"] + "').val('');";
                                        if (start == 0) ++start;
                                        j_script += "\ntrack_point_source[" + id_tracker.ToString() + "] = vector_track_points[" + id_tracker.ToString() + "].getSource();";
                                        j_script += "\ntrack_point_ln_source[" + id_tracker.ToString() + "] = vector_track_lines[" + id_tracker.ToString() + "].getSource();";
                                        j_script += "\nvar coords = []; \nvar feature_ln;";
                                        int k = start - 1;
                                        j_script += "\ntrack_point_feature_prev = format.readFeature('" + layersDS.Tables["TrackPoints"].Rows[k]["track_point_geo_json"] + "');";
                                        j_script += "\ntrack_point_feature_prev.name = '" + layersDS.Tables["TrackPoints"].Rows[k]["id_geographic_track_point"] + "';";
                                        j_script += "\ntrack_point_feature_prev.getGeometry().transform('EPSG:4326', 'EPSG:3857');";
                                        for (i = start; i < end - 1; ++i)
                                        {
                                            DateTime dt_prev = DateTime.ParseExact(layersDS.Tables["TrackPoints"].Rows[k]["datetime_point"].ToString(), "dd.MM.yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                            DateTime dt_curr = DateTime.ParseExact(layersDS.Tables["TrackPoints"].Rows[i]["datetime_point"].ToString(), "dd.MM.yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                            Double interval = (dt_curr - dt_prev).TotalSeconds;
                                            if (interval < min_interval) continue;
                                            track_point_feature_string = "\ntrack_point_feature = format.readFeature('" + layersDS.Tables["TrackPoints"].Rows[i]["track_point_geo_json"] + "');";
                                            track_point_feature_name = "\ntrack_point_feature.name = '" + layersDS.Tables["TrackPoints"].Rows[i]["id_geographic_track_point"] + "';";
                                            for (int j = 0; j < layersDS.Tables["TrackPoints"].Columns.Count; ++j)
                                            {
                                                if (j == 0)
                                                {
                                                    track_point_properties_string = "\ntrack_point_feature.setProperties({'layer':'track_points','id_feature':'" + i.ToString() + "'";
                                                }
                                                if (layersDS.Tables["TrackPoints"].Columns[j].ColumnName.ToString() != "track_point_geo_json")
                                                {
                                                    if (track_point_properties_string[track_point_properties_string.Length - 1] != '{' && track_point_properties_string[track_point_properties_string.Length - 1] != ',')
                                                    {
                                                        track_point_properties_string += ",";
                                                    }
                                                    track_point_properties_string += ("'" + layersDS.Tables["TrackPoints"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["TrackPoints"].Rows[i][j].ToString() + "'");
                                                }
                                                if (j == (layersDS.Tables["TrackPoints"].Columns.Count - 1))
                                                {
                                                    track_point_properties_string += "});";
                                                }
                                            }
                                            j_script += (track_point_feature_string + track_point_feature_name + track_point_properties_string + "\ntrack_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');");
                                            j_script += "\ntrack_point_feature.setId(" + i.ToString() + ");";
                                            j_script += "\ntrack_point_source[" + id_tracker.ToString() + "].addFeature(track_point_feature);";
                                            double lat1 = double.Parse(layersDS.Tables["TrackPoints"].Rows[k]["latitude"].ToString());
                                            double lon1 = double.Parse(layersDS.Tables["TrackPoints"].Rows[k]["longitude"].ToString());
                                            double lat2 = double.Parse(layersDS.Tables["TrackPoints"].Rows[i]["latitude"].ToString());
                                            double lon2 = double.Parse(layersDS.Tables["TrackPoints"].Rows[i]["longitude"].ToString());
                                            double alt1 = double.Parse(layersDS.Tables["TrackPoints"].Rows[k]["altitude"].ToString());
                                            double alt2 = double.Parse(layersDS.Tables["TrackPoints"].Rows[i]["altitude"].ToString());
                                            double tmp_D = CountDistance(lat1, lon1, alt1, lat2, lon2, alt2);
                                            /*j_script += "\nvar lat1 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[k]["latitude"].ToString().Replace(',', '.') + ");";
                                            j_script += "\nvar lon1 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[k]["longitude"].ToString().Replace(',', '.') + ");";
                                            j_script += "\nvar lat2 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[i]["latitude"].ToString().Replace(',', '.') + ");";
                                            j_script += "\nvar lon2 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[i]["longitude"].ToString().Replace(',', '.') + ");";*/
                                            //j_script += "\nvar alt1 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[k]["altitude"].ToString().Replace(',', '.') + ");";
                                            //j_script += "\nvar alt2 = parseFloat(" + layersDS.Tables["TrackPoints"].Rows[i]["altitude"].ToString().Replace(',', '.') + ");";
                                            j_script += "\nvar tmp_D = Number('" + tmp_D.ToString().Replace(',', '.') + "');";
                                            //j_script += "\nvar tmp_D = Number(CountDistance(lat1, lon1, lat2, lon2));";
                                            j_script += "\nif(true){ D[" + id_tracker.ToString() + "] += tmp_D;";

                                            j_script += "\nif(tmp_D<100 && tmp_D!=0) { var point_prev = track_point_feature_prev.getGeometry().getCoordinates();";
                                            j_script += "\nvar point_curr = track_point_feature.getGeometry().getCoordinates();";
                                            j_script += "\ncoords = [point_prev, point_curr];";
                                            j_script += "\nfeature_ln = new ol.Feature({ geometry: new ol.geom.LineString(coords) });";
                                            j_script += "\nfeature_ln.name = 'track_line_" + (i - 1).ToString() + "';";
                                            j_script += "\nvar layer_name = 'vector_track_lines[" + id_tracker.ToString() + "]';";
                                            j_script += "\nfeature_ln.setProperties({ 'layer': layer_name, 'id_feature': " + (i - 1).ToString() + ", 'id_gps_tracker': track_point_feature.getProperties().id_gps_tracker });";
                                            j_script += "\ntrack_point_ln_source[" + id_tracker.ToString() + "].addFeature(feature_ln);}}";
                                            k = i;
                                            j_script += "\ntrack_point_feature_prev = track_point_feature;";
                                        }
                                        //bool flag = i >= layersDS.Tables["TrackPoints"].Rows.Count;
                                        j_script += "\nvar count_track_point = track_point_source[" + id_tracker.ToString() + "].getFeatures().length;";
                                        // j_script += "\nvar vector_track_points_lines = new ol.layer.Vector({source: new ol.source.Vector({features: [new ol.Feature({geometry: new ol.geom.LineString(tmp_points) })] }) });";
                                        //j_script += "\nvector_track_points_lines = new ol.layer.Vector({source: new ol.source.Vector({ maxResolution:500, features: [new ol.Feature({geometry: new ol.geom.LineString(track_points_ol)})]})});";
                                        // j_script += "\nvector_track_points[" + id_tracker.ToString() + "] = new ol.layer.Vector({source: track_point_source, maxResolution: 100 });";
                                        // j_script += "\nmap.addLayer(vector_track_points[" + id_tracker.ToString() + "]);  }; "
                                        j_script += "\nvector_track_lines[" + id_tracker.ToString() + "].setZIndex(3);";
                                        //j_script += "\nCallServer('setlinesstyle:" + id_tracker.ToString() + "', 'null');";
                                        j_script += "\nvector_track_points[" + id_tracker.ToString() + "].setZIndex(4);";
                                        //центрирование на треке
                                        /*j_script += "\nvar track_extent = track_point_source[" + id_tracker.ToString() + "].getExtent();";
                                        j_script += "\nmap.getView().fit(track_extent, map.getSize());";*/

                                        if (CheckRowsCount(stylesDS, "TrackPointsStyles"))
                                        {
                                            j_script += ("\nfunction " + "SetStyles_track_points" + ClientID + "(){");
                                            j_script += "\nvar track_points_styleCache = {};";
                                            j_script += "\nvar track_points_style = function (feature, resolution){";
                                            j_script += "\nvar track_point_text = feature.name;";
                                            j_script += "\nif (!track_points_styleCache[track_point_text]){";
                                            j_script += ("\nswitch (feature.getProperties().id_gps_tracker){");
                                            String image_points_str, font_points_str;
                                            for (i = 0; i < stylesDS.Tables["TrackPointsStyles"].Rows.Count; i++)
                                            {
                                                j_script += ("\ncase '" + stylesDS.Tables["TrackPointsStyles"].Rows[i]["id_gps_tracker"].ToString() + "':{");
                                                if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                                {
                                                    j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().imei;\nif (resolution > " +
                                                                stylesDS.Tables["TrackPointsStyles"].Rows[i]["maxresolution"].ToString() +
                                                                ") { t = ''; }\nreturn t;};");
                                                }
                                                image_points_str = String.Empty;
                                                image_points_str += "\ntrack_points_styleCache[track_point_text] = [new ol.style.Style({image: new ol.style.Circle({fill: new ol.style.Fill({";
                                                image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackPointsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                image_points_str += "\nstroke: new ol.style.Stroke({";
                                                image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                image_points_str += ("\nwidth: " + stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_width"].ToString());
                                                if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                                {
                                                    image_points_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                image_points_str += ("}),\nradius: 3})");
                                                font_points_str = String.Empty;
                                                if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                                {

                                                    font_points_str += "\ntext: new ol.style.Text({font: '";
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                                    {
                                                        font_points_str += "bold ";
                                                    }
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                                    {
                                                        font_points_str += "italic ";
                                                    }
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                                    {
                                                        font_points_str += "underline ";
                                                    }
                                                    font_points_str += (stylesDS.Tables["TrackPointsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_name"].ToString() + "',");
                                                    font_points_str += "\ntext: getText(feature, resolution),";
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                    {
                                                        font_points_str += ("\noffsetX: " + stylesDS.Tables["TrackPointsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                                    }
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                    {
                                                        font_points_str += ("\noffsetY: " + stylesDS.Tables["TrackPointsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                                    }
                                                    font_points_str += "\nfill: new ol.style.Fill({";
                                                    font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                    font_points_str += "\nstroke: new ol.style.Stroke({";
                                                    font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                                  stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                    font_points_str += ("\nwidth: " + stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_width"].ToString());
                                                    if (stylesDS.Tables["TrackPointsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                    {
                                                        font_points_str += ",\nlineDash: [0.5, 4]";
                                                    }
                                                    font_points_str += "\n})})";
                                                }

                                                if (image_points_str != String.Empty)
                                                {
                                                    j_script += image_points_str;
                                                    if (font_points_str != String.Empty)
                                                    {
                                                        if (image_points_str != String.Empty)
                                                        {
                                                            j_script += ",";
                                                        }
                                                        j_script += font_points_str;
                                                    }
                                                    j_script += "})];\nbreak;}";
                                                }
                                                else
                                                {
                                                    j_script += "\ntrack_points_styleCache[track_point_text] = null;\nbreak;}";
                                                }

                                            }
                                            j_script += "}\n}\nreturn track_points_styleCache[track_point_text];};";
                                            j_script += "\nvector_track_points[" + id_tracker.ToString() + "].setStyle(track_points_style);}";
                                            //j_script += "\nD["   + id_tracker.ToString() + "] = D["  + id_tracker.ToString() + "].toFixed(3);";
                                            j_script += "\n$('#Distance" + layersDS.Tables["TrackPoints"].Rows[0]["id_gps_tracker"] + "').val(D[" + id_tracker.ToString() + "].toFixed(3) + ' км');";
                                            j_script += ("\nSetStyles_track_points" + ClientID + "();");
                                            j_script += "CallServer('setlinesstyle:" + id_tracker.ToString() + "', 'null');";
                                            //j_script += "\nDrawTrackLines(vector_track_points[" + id_tracker.ToString() + "], " + id_tracker.ToString() + ", " + start.ToString() + ", " + end.ToString() + ");";

                                        }
                                        if (end == layersDS.Tables["TrackPoints"].Rows.Count && is_end)
                                        {
                                            j_script += "\nsetTimeout(function() { SetTrackersActive(1); }, " + timeout_value.ToString() + ");";
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "setlinesstyle":
                        {
                            if (connection_try)
                            {
                                DataSet stylesDS = new DataSet();

                                conn.Open();


                                Int32 id_tracker = Convert.ToInt32(eventArgument.Split(':')[1].Split(';')[0]);
                                String get_track_styles_str = "SELECT * FROM ViewGPSTrackersStyle";
                                SqlDataAdapter get_track_styles = new SqlDataAdapter(get_track_styles_str, conn);
                                get_track_styles.Fill(stylesDS, "TrackLinesStyles");

                                conn.Close();
                                if (CheckRowsCount(stylesDS, "TrackLinesStyles"))
                                {
                                    j_script += ("\nfunction SetStyles_track_lines" + ClientID + "(){");
                                    j_script += "\nvar track_lines_styleCache = {};";
                                    j_script += "\nvar track_lines_style = function (feature, resolution){";
                                    j_script += "\nvar geometry = feature.getGeometry();";
                                    j_script += "\nvar track_line_text = feature.name;";
                                    j_script += "\nif (!track_lines_styleCache[track_line_text]){";
                                    j_script += ("\nswitch (feature.getProperties().id_gps_tracker){");
                                    String image_lines_str, font_lines_str, arrow_lines_str;
                                    for (int i = 0; i < stylesDS.Tables["TrackLinesStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["id_gps_tracker"].ToString() + "':{");
                                        if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().imei;\nif (resolution > " +
                                                        stylesDS.Tables["TrackLinesStyles"].Rows[i]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        image_lines_str = String.Empty;
                                        image_lines_str += "\ntrack_lines_styleCache[track_line_text] = [new ol.style.Style({";
                                        image_lines_str += "fill: new ol.style.Fill({";
                                        image_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["red"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["green"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                        image_lines_str += "\nstroke: new ol.style.Stroke({";
                                        image_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        image_lines_str += ("\nwidth: " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_width"].ToString());
                                        if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                        {
                                            image_lines_str += ",\nlineDash: [4, 4]";
                                        }
                                        image_lines_str += (" })");
                                        font_lines_str = String.Empty;
                                        if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {

                                            font_lines_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_lines_str += "bold ";
                                            }
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_lines_str += "italic ";
                                            }
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_lines_str += "underline ";
                                            }
                                            font_lines_str += (stylesDS.Tables["TrackLinesStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_lines_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_lines_str += ("\noffsetX: " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_lines_str += ("\noffsetY: " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_lines_str += "\nfill: new ol.style.Fill({";
                                            font_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_lines_str += "\nstroke: new ol.style.Stroke({";
                                            font_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_lines_str += ("\nwidth: " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_lines_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            font_lines_str += "\n})})";
                                        }
                                        arrow_lines_str = String.Empty;
                                        arrow_lines_str += ("\nvar canvas = (document.createElement('canvas')); var vectorContext = ol.render.toContext((canvas.getContext('2d')),{size: [10, 10], pixelRatio: 1});");
                                        //arrow_lines_str += "\nvectorContext.setFillStrokeStyle(";
                                        arrow_lines_str += "\nvar render_style = new ol.style.Style({";
                                        arrow_lines_str += "\nfill: new ol.style.Fill({";
                                        arrow_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["red"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["green"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                        arrow_lines_str += "\nstroke: new ol.style.Stroke({";
                                        arrow_lines_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        arrow_lines_str += ("\nwidth: " + stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_width"].ToString());
                                        if (stylesDS.Tables["TrackLinesStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                        {
                                            arrow_lines_str += ",\nlineDash: [0.5, 4]";
                                        }

                                        arrow_lines_str += "\n}) });";
                                        arrow_lines_str += "\nvectorContext.setStyle(render_style);";
                                        arrow_lines_str += "\nvectorContext.drawGeometry(new ol.geom.MultiLineString([[[0,0],[5,10]],[[5,10],[10,0]]]));";
                                        arrow_lines_str += "\ngeometry.forEachSegment(function(start, end) {";
                                        arrow_lines_str += "\nvar dx = end[0] - start[0];";
                                        arrow_lines_str += "\nvar dy = end[1] - start[1];";
                                        arrow_lines_str += "\nvar rotation = Math.PI/2 + Math.atan2(dy, dx);";
                                        arrow_lines_str += "\ntrack_lines_styleCache[track_line_text].push(new ol.style.Style({ ";
                                        arrow_lines_str += "\ngeometry: new ol.geom.Point(end),";
                                        arrow_lines_str += "\nimage: new ol.style.Icon({";
                                        arrow_lines_str += "\nimg: canvas, imgSize: [canvas.width, canvas.height],";
                                        arrow_lines_str += "\nanchor: [0.5, 1],";
                                        arrow_lines_str += "\nrotateWithView: false,";
                                        arrow_lines_str += "\nrotation: -rotation";
                                        arrow_lines_str += "\n}) }) ); });";

                                        if (image_lines_str != String.Empty)
                                        {
                                            j_script += image_lines_str;
                                            if (font_lines_str != String.Empty)
                                            {
                                                if (image_lines_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += font_lines_str;
                                            }
                                            j_script += "})];";
                                            j_script += arrow_lines_str;
                                            j_script += "\nbreak;}";
                                        }
                                        else
                                        {
                                            j_script += "\ntrack_lines_styleCache[track_line_text] = null;\nbreak;}";
                                        }

                                    }

                                    j_script += "}\n}\nreturn track_lines_styleCache[track_line_text];};";
                                    j_script += "\nvector_track_lines[" + id_tracker.ToString() + "].setStyle(track_lines_style);}";
                                    j_script += ("\nSetStyles_track_lines" + ClientID + "();");

                                    //запись лог файлов
                                    /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "track_lines_style_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                    SW.Write(j_script);
                                    SW.Close();*/
                                }
                            }
                            break;
                        }
                    case "drawlastpoint":
                        {
                            if (connection_try)
                            {
                                DataSet layersDS = new DataSet();
                                DataSet stylesDS = new DataSet();

                                conn.Open();
                                Int32 id_tracker = Convert.ToInt32(eventArgument.Split(':')[1]);


                                String track_points_querry_string = "exec [dbo].[GetLastTrackPointGeoJSON] " + id_tracker.ToString();
                                SqlDataAdapter track_points_geo_data = new SqlDataAdapter(track_points_querry_string, conn);
                                track_points_geo_data.Fill(layersDS, "LastTrackPoint");

                                String get_track_styles_str = "SELECT * FROM ViewCarsStyle";
                                SqlDataAdapter get_track_styles = new SqlDataAdapter(get_track_styles_str, conn);
                                get_track_styles.Fill(stylesDS, "LastTrackPointStyle");

                                conn.Close();
                                if (CheckRowsCount(layersDS, "LastTrackPoint"))
                                {
                                    String track_last_point_feature_string = String.Empty;
                                    String track_last_point_feature_name = String.Empty;
                                    String track_last_point_properties_string = String.Empty;

                                    j_script += "\nvar track_last_point_feature;";
                                    /*if (start != 0) j_script += "\nvar D = Number($('#Distance" + layersDS.Tables["TrackPoints"].Rows[0]["id_gps_tracker"] + "').val().split(' ')[0]) * 1000;";
                                    else { j_script += "\nvar D = 0;"; ++start; }*/
                                    track_last_point_feature_string = "\ntrack_last_point_feature = format.readFeature('" + layersDS.Tables["LastTrackPoint"].Rows[0]["track_point_geo_json"] + "');";
                                    track_last_point_feature_name = "\ntrack_last_point_feature.name = '" + layersDS.Tables["LastTrackPoint"].Rows[0]["id_car"] + "';";
                                    for (int j = 0; j < layersDS.Tables["LastTrackPoint"].Columns.Count; ++j)
                                    {
                                        if (j == 0)
                                        {
                                            track_last_point_properties_string = "\ntrack_last_point_feature.setProperties({'layer':'track_last_point','id_feature':'0'";
                                        }
                                        if (layersDS.Tables["LastTrackPoint"].Columns[j].ColumnName.ToString() != "track_point_geo_json")
                                        {
                                            if (track_last_point_properties_string[track_last_point_properties_string.Length - 1] != '{' && track_last_point_properties_string[track_last_point_properties_string.Length - 1] != ',')
                                            {
                                                track_last_point_properties_string += ",";
                                            }
                                            track_last_point_properties_string += ("'" + layersDS.Tables["LastTrackPoint"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["LastTrackPoint"].Rows[0][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["LastTrackPoint"].Columns.Count - 1))
                                        {
                                            track_last_point_properties_string += "});";
                                        }
                                    }
                                    j_script += (track_last_point_feature_string + track_last_point_feature_name + track_last_point_properties_string + "\ntrack_last_point_feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');");
                                    //j_script += "\ntrack_last_point_feature.setId(0);";
                                    j_script += "\ntrack_last_point_source[" + id_tracker.ToString() + "].clear();";
                                    j_script += "\ntrack_last_point_source[" + id_tracker.ToString() + "].addFeature(track_last_point_feature);";
                                    //j_script += "\nmap.addLayer(vector_track_last_point[" + id_tracker.ToString() + "]);";
                                    j_script += "\nvector_track_last_point[" + id_tracker.ToString() + "].setZIndex(3);";

                                    if (CheckRowsCount(stylesDS, "LastTrackPointStyle"))
                                    {
                                        j_script += ("\nfunction SetStyles_last_track_point" + ClientID + "(){");
                                        j_script += "\nvar last_track_point_styleCache = {};  ";
                                        j_script += "\nvar last_track_point_style = function (feature, resolution){ ";
                                        j_script += "\nvar last_track_point_text = feature.name; ";
                                        j_script += "\nif (!last_track_point_styleCache[last_track_point_text]){ ";
                                        j_script += ("\nswitch (feature.getProperties().id_car){ ");
                                        String image_points_str, font_points_str;
                                        for (int i = 0; i < stylesDS.Tables["LastTrackPointStyle"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["id_car"].ToString() + "':{");
                                            if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {
                                                j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().car_model;\nif (resolution > " +
                                                            stylesDS.Tables["LastTrackPointStyle"].Rows[i]["maxresolution"].ToString() +
                                                            ") { t = ''; }\nreturn t;};");
                                            }
                                            image_points_str = String.Empty;
                                            image_points_str += "\nlast_track_point_styleCache[last_track_point_text] = [new ol.style.Style({image: new ol.style.Circle({fill: new ol.style.Fill({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            image_points_str += "\nstroke: new ol.style.Stroke({";
                                            image_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            image_points_str += ("\nwidth: " + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                image_points_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            image_points_str += ("}),\nradius: 5})");
                                            font_points_str = String.Empty;
                                            if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {

                                                font_points_str += "\ntext: new ol.style.Text({font: '";
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["bold_font"].ToString() == "1")
                                                {
                                                    font_points_str += "bold ";
                                                }
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["italic_font"].ToString() == "1")
                                                {
                                                    font_points_str += "italic ";
                                                }
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["underline_font"].ToString() == "1")
                                                {
                                                    font_points_str += "underline ";
                                                }
                                                font_points_str += (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_name"].ToString() + "',");
                                                font_points_str += "\ntext: getText(feature, resolution),";
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                {
                                                    font_points_str += ("\noffsetX: " + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["offset_x"].ToString() + ",");
                                                }
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                {
                                                    font_points_str += ("\noffsetY: " + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["offset_y"].ToString() + ",");
                                                }
                                                font_points_str += "\nfill: new ol.style.Fill({";
                                                font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_red"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_green"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                font_points_str += "\nstroke: new ol.style.Stroke({";
                                                font_points_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                font_points_str += ("\nwidth: " + stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_width"].ToString());
                                                if (stylesDS.Tables["LastTrackPointStyle"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                {
                                                    font_points_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                font_points_str += "\n})})";
                                            }

                                            if (image_points_str != String.Empty)
                                            {
                                                j_script += image_points_str;
                                                if (font_points_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                    j_script += font_points_str;
                                                }
                                                j_script += "})];\nbreak;}";
                                            }
                                            else
                                            {
                                                j_script += "\nlast_track_point_styleCache[last_track_point_text] = null;\nbreak;}";
                                            }

                                        }
                                        j_script += "}\n}\nreturn last_track_point_styleCache[last_track_point_text];};";
                                        j_script += "\nvector_track_last_point[" + id_tracker.ToString() + "].setStyle(last_track_point_style);}";
                                        j_script += "\nSetStyles_last_track_point" + ClientID + "();";

                                        /*//запись лога
                                        StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "track_last_point_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                        SW.Write(j_script);
                                        SW.Close();*/
                                    }
                                }
                            }
                            break;
                        }
                    case "showselecttour":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_tours_str = String.Empty;

                            switch (type_report_object)
                            {
                                //отделение
                                case "0":
                                    {
                                        //get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_department = " + id_report_object;
                                        break;
                                    }
                                //организация
                                case "1":
                                    {
                                        get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_organization = " + id_report_object + " ORDER BY tour";
                                        break;
                                    }
                                //район
                                case "2":
                                    {
                                        get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_region = " + id_report_object + " ORDER BY tour";
                                        break;
                                    }
                                //область
                                case "3":
                                    {
                                        break;
                                    }
                                //страна
                                case "4":
                                    {
                                        break;
                                    }
                            }

                            if (get_tours_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_tours = new SqlDataAdapter(get_tours_str, conn);
                                DataSet toursDS = new DataSet();
                                get_tours.Fill(toursDS, "ReportTours");
                                conn.Close();

                                if (CheckRowsCount(toursDS, "ReportTours"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectTourCB\").empty();";
                                    j_script += ("\n$(\"#SelectTourCB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < toursDS.Tables["ReportTours"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectTourCB\").append('<option value=\"" + toursDS.Tables["ReportTours"].Rows[i]["tour"].ToString() + "\">" +
                                                     toursDS.Tables["ReportTours"].Rows[i]["tour"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "showselecttourerosionchange":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_tours_str = String.Empty;

                            switch (type_report_object)
                            {
                                //отделение
                                case "0":
                                    {
                                        //get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_department = " + id_report_object;
                                        break;
                                    }
                                //организация
                                case "1":
                                    {
                                        get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_organization = " + id_report_object + " ORDER BY tour";
                                        break;
                                    }
                                //район
                                case "2":
                                    {
                                        break;
                                    }
                                //область
                                case "3":
                                    {
                                        break;
                                    }
                                //страна
                                case "4":
                                    {
                                        break;
                                    }
                            }

                            if (get_tours_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_tours = new SqlDataAdapter(get_tours_str, conn);
                                DataSet toursDS = new DataSet();
                                get_tours.Fill(toursDS, "ReportErosionChangeTours");
                                conn.Close();

                                if (CheckRowsCount(toursDS, "ReportErosionChangeTours"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectTourErosionChangeCB\").empty();";
                                    j_script += ("\n$(\"#SelectTourErosionChangeCB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < toursDS.Tables["ReportErosionChangeTours"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectTourErosionChangeCB\").append('<option value=\"" + toursDS.Tables["ReportErosionChangeTours"].Rows[i]["tour"].ToString() + "\">" +
                                                     toursDS.Tables["ReportErosionChangeTours"].Rows[i]["tour"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "showselect_u_w_f_tour":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_tours_str = String.Empty;

                            switch (type_report_object)
                            {
                                //отделение
                                case "0":
                                    {
                                        //get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_department = " + id_report_object;
                                        break;
                                    }
                                //организация
                                case "1":
                                    {
                                        //get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_organization = " + id_report_object + " ORDER BY tour";
                                        break;
                                    }
                                //район
                                case "2":
                                    {
                                        get_tours_str = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_region = " + id_report_object + " ORDER BY tour";
                                        break;
                                    }
                                //область
                                case "3":
                                    {
                                        break;
                                    }
                                //страна
                                case "4":
                                    {
                                        break;
                                    }
                            }

                            if (get_tours_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_tours = new SqlDataAdapter(get_tours_str, conn);
                                DataSet toursDS = new DataSet();
                                get_tours.Fill(toursDS, "ReportTours");
                                conn.Close();

                                if (CheckRowsCount(toursDS, "ReportTours"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectTour1CB\").empty();";
                                    j_script += ("\n$(\"#SelectTour1CB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < toursDS.Tables["ReportTours"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectTour1CB\").append('<option value=\"" + toursDS.Tables["ReportTours"].Rows[i]["tour"].ToString() + "\">" +
                                                     toursDS.Tables["ReportTours"].Rows[i]["tour"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "showselectyear":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_years_str = String.Empty;

                            switch (type_report_object)
                            {
                                //культуры по организации
                                case "1":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Organizarion WHERE id_organization = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                                case "2":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Region WHERE id_region = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                            }

                            if (get_years_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_years = new SqlDataAdapter(get_years_str, conn);
                                DataSet yearDS = new DataSet();
                                get_years.Fill(yearDS, "ReportYears");
                                conn.Close();

                                if (CheckRowsCount(yearDS, "ReportYears"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectYearCB\").empty();";
                                    j_script += ("\n$(\"#SelectYearCB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < yearDS.Tables["ReportYears"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectYearCB\").append('<option value=\"" + yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "\">" +
                                                     yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }

                    case "showselectyearculture":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_years_str = String.Empty;

                            switch (type_report_object)
                            {
                                //урожайность по организации
                                case "1":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Organizarion WHERE id_organization = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                                case "2":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Region WHERE id_region = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                            }

                            if (get_years_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_years = new SqlDataAdapter(get_years_str, conn);
                                DataSet yearDS = new DataSet();
                                get_years.Fill(yearDS, "ReportYears");
                                conn.Close();

                                if (CheckRowsCount(yearDS, "ReportYears"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectYear1CB\").empty();";
                                    j_script += ("\n$(\"#SelectYear1CB\").append('<option value=\"0\"></option>');");

                                    j_script += "\n$(\"#SelectCultureCB\").empty();";
                                    j_script += ("\n$(\"#SelectCultureCB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < yearDS.Tables["ReportYears"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectYear1CB\").append('<option value=\"" + yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "\">" +
                                                     yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "select_culture_for_productivity":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String year = eventArgument.Split(':')[1].Split('|')[2];
                            String get_cultures_str = String.Empty;

                            switch (type_report_object)
                            {
                                //урожайность по организации
                                case "1":
                                    {
                                        get_cultures_str = "exec [dbo].[GetHBCulturesByYearForOrganization] " + id_report_object + ", " + year;
                                        break;
                                    }
                                case "2":
                                    {
                                        get_cultures_str = "exec [dbo].[GetHBCulturesByYearForRegion] " + id_report_object + ", " + year;
                                        break;
                                    }
                            }

                            if (get_cultures_str != String.Empty && id_report_object != null && year != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_cultures = new SqlDataAdapter(get_cultures_str, conn);
                                DataSet cultureDS = new DataSet();
                                get_cultures.Fill(cultureDS, "ReportCultures");
                                conn.Close();

                                if (CheckRowsCount(cultureDS, "ReportCultures"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectCultureCB\").empty();";
                                    j_script += ("\n$(\"#SelectCultureCB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < cultureDS.Tables["ReportCultures"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectCultureCB\").append('<option value=\"" + cultureDS.Tables["ReportCultures"].Rows[i]["id_culture"].ToString() + "\">" +
                                                     cultureDS.Tables["ReportCultures"].Rows[i]["title_culture"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "getaccessreports":
                        {
                            String id_user = eventArgument.Split(':')[1].Split('|')[0];
                            String type_user = eventArgument.Split(':')[1].Split('|')[1];
                            String id_organization = eventArgument.Split(':')[1].Split('|')[2];
                            String id_region = eventArgument.Split(':')[1].Split('|')[3];
                            String id_territory = eventArgument.Split(':')[1].Split('|')[4];

                            if (type_user == "1")
                            {
                                String get_access_outside_user_str = "SELECT * FROM View_Access_Outside_Users WHERE id_outside_user=" + id_user;
                                conn.Open();
                                SqlDataAdapter get_access_outside_user = new SqlDataAdapter(get_access_outside_user_str, conn);
                                DataSet accessDS = new DataSet();
                                get_access_outside_user.Fill(accessDS, "AccessOutsideUser");
                                conn.Close();
                                j_script = "\n$(function () {";
                                DataRow access_row;
                                //добавить если нужно доступ к отчёту по отделению
                                if (accessDS.Tables["AccessOutsideUser"].Select("id_organization=" + id_organization).Length > 0)
                                {
                                    access_row = accessDS.Tables["AccessOutsideUser"].Select("id_organization=" + id_organization)[0];
                                    if (access_row["code_access_organization"].ToString() == "3")
                                    {
                                        j_script += "$('#OrganizationReportB').button('enable');";
                                        j_script += "$('#CulturesReportB').button('enable');";
                                        j_script += "$('#CulturesProductivityReportB').button('enable');";
                                    }
                                    else
                                    {
                                        j_script += "$('#OrganizationReportB').button('disable');";
                                        j_script += "$('#CulturesReportB').button('disable');";
                                        j_script += "$('#CulturesProductivityReportB').button('disable');";
                                    }
                                }
                                else
                                {
                                    j_script += "$('#OrganizationReportB').button('disable');";
                                }

                                if (accessDS.Tables["AccessOutsideUser"].Select("id_region=" + id_region).Length > 0)
                                {
                                    access_row = accessDS.Tables["AccessOutsideUser"].Select("id_region=" + id_region)[0];
                                    if (access_row["code_access_region"].ToString() == "3")
                                    {
                                        j_script += "$('#RegionReportB').button('enable');";
                                        j_script += "$('#CulturesRegionReportB').button('enable');";
                                        j_script += "$('#CulturesProductivityRegionReportB').button('enable');";
                                    }
                                    else
                                    {
                                        j_script += "$('#RegionReportB').button('disable');";
                                        j_script += "$('#CulturesRegionReportB').button('disable');";
                                        j_script += "$('#CulturesProductivityRegionReportB').button('disable');";
                                    }
                                }
                                else
                                {
                                    j_script += "$('#RegionReportB').button('disable');";
                                }
                                //добавить если нужно доступ к отчёту по области
                                j_script += "});";
                            }

                            break;
                        }
                    case "cultures":
                        {
                            DataSet culturesDS = new DataSet();
                            conn.Open();

                            Int32 type_object = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]);

                            String get_styles_str = "SELECT * FROM ViewCultureStyle";
                            SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                            get_styles.Fill(culturesDS, "CulturesStyles");

                            String get_plots_cultures_str = "", legend_str = "";

                            //j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                            j_script += RemoveLayers();

                            switch (type_object)
                            {
                                case 1:
                                    {
                                        get_plots_cultures_str = "exec [dbo].[GetPlotsWithHBCultures] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1];
                                        //j_script += "\nCallServer('select_organization:' + " + eventArgument.Split(':')[1].Split('|')[0] + ", 'null');";
                                        break;
                                    }
                                case 2:
                                    {
                                        DataSet layersDS = new DataSet();
                                        get_plots_cultures_str = "exec [dbo].[GetPlotsWithHBCulturesRegion] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1];
                                        String get_plots_str = "exec [dbo].[GetLastPlotsRegionGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                        SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                        plots_geo_data.Fill(layersDS, "Plots");

                                        String feature_string = String.Empty,
                                               feature_name = String.Empty,
                                               properties_string = String.Empty;
                                        if (CheckRowsCount(layersDS, "Plots"))
                                        {
                                            /*j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                       "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                       "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                            id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();*/
                                            id_organization = "0";
                                            year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                            tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                            code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                            j_script += "\n$(\"#OrganizationCB option[value='0']\").prop('selected', true);";
                                            j_script += "\n$(\"#OrganizationCB option:nth-child(1)\").attr('selected', 'selected');";
                                            j_script += "\nmap.removeLayer(vector_plots);";
                                            j_script += "\nvar plots_source = new ol.source.Vector();\nvar feature;";
                                            for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                            {
                                                feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                                feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                                for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                                {
                                                    if (j == 0)
                                                    {
                                                        properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                    }
                                                    if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                    {
                                                        if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                        {
                                                            properties_string += ",";
                                                        }
                                                        properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                    }
                                                    if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                    {
                                                        properties_string += "});";
                                                    }
                                                }
                                                j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                                j_script += "\nplots_source.addFeature(feature);";
                                            }
                                            j_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                            j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:250});";

                                            j_script += "\ncurrent_extent = plots_source.getExtent();";
                                            j_script += "\nmap.addLayer(vector_plots);\nmap.getView().fit(current_extent, map.getSize());";
                                            j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                        }

                                        break;
                                    }
                            }

                            SqlDataAdapter get_plots_cultures = new SqlDataAdapter(get_plots_cultures_str, conn);
                            get_plots_cultures.Fill(culturesDS, "PlotsCultures");
                            conn.Close();

                            if (CheckRowsCount(culturesDS, "PlotsCultures"))
                            {
                                j_script += "\nvar plots_cultures = {};";
                                String id_culture;
                                for (int i = 0; i < culturesDS.Tables["PlotsCultures"].Rows.Count; i++)
                                {
                                    id_culture = culturesDS.Tables["PlotsCultures"].Rows[i]["id_culture"].ToString();
                                    if (id_culture == String.Empty || id_culture == null) { id_culture = "0"; }
                                    j_script += "\nplots_cultures[\"" + culturesDS.Tables["PlotsCultures"].Rows[i]["code_plot"].ToString() + "\"] = " + id_culture + ";";
                                    //j_script += "\nalert(plots_cultures[\"" + culturesDS.Tables["PlotsCultures"].Rows[i]["code_plot"].ToString() + "\"]);";
                                }

                                if (CheckRowsCount(culturesDS, "CulturesStyles"))
                                {
                                    j_script += ("\nfunction " + "SetStyles_Culture" + ClientID + "(){");
                                    j_script += "\nvar plots_styleCache = {};";
                                    j_script += "\nvar plots_style = function (feature, resolution){";
                                    j_script += "\nvar id_culture = plots_cultures[feature.getProperties().code_plot];";
                                    j_script += "\nif (id_culture == \"\" || id_culture == null) { id_culture = 0; }";
                                    j_script += ("\nvar plot_text = feature.getProperties().id_plot;");
                                    if (culturesDS.Tables["CulturesStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    culturesDS.Tables["CulturesStyles"].Rows[0]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    j_script += "\nif (!plots_styleCache[plot_text]){";
                                    j_script += ("\nswitch (id_culture){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < culturesDS.Tables["CulturesStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase " + culturesDS.Tables["CulturesStyles"].Rows[i]["id_culture"].ToString() + ":{plots_styleCache[plot_text] = [new ol.style.Style({");
                                        fill_str = String.Empty;
                                        if (culturesDS.Tables["CulturesStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + culturesDS.Tables["CulturesStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (culturesDS.Tables["CulturesStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_width"].ToString());
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (culturesDS.Tables["CulturesStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (culturesDS.Tables["CulturesStyles"].Rows[i]["size_font"].ToString() + "px " + culturesDS.Tables["CulturesStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + culturesDS.Tables["CulturesStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + culturesDS.Tables["CulturesStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + culturesDS.Tables["CulturesStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (culturesDS.Tables["CulturesStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]})";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "\ndefault: {plots_styleCache[plot_text] = [new ol.style.Style({";
                                    j_script += "\nfill: new ol.style.Fill({color: 'rgba(0,0,0,1)'})";
                                    j_script += "})];\nbreak;}";
                                    j_script += "}\n}\nreturn plots_styleCache[plot_text];};";
                                    j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                                    j_script += ("\nSetStyles_Culture" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Culture" + ClientID + "();});");
                                }
                                switch (type_object)
                                {
                                    case 1:
                                        {
                                            legend_str = GetLegendJS("culture", eventArgument.Split(':')[1].Split('|')[0], tour, eventArgument.Split(':')[1].Split('|')[1]);
                                            break;
                                        }
                                    case 2:
                                        {
                                            legend_str = GetLegendJS("culture_region", eventArgument.Split(':')[1].Split('|')[0], tour, eventArgument.Split(':')[1].Split('|')[1]);
                                            break;
                                        }
                                }

                                j_script += legend_str;

                                j_script += "\n$(\"#MapThemeCB\").val(\"report\");";
                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "cultures_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/

                            break;
                        }
                    case "productivity":
                        {
                            DataSet productivityDS = new DataSet();
                            conn.Open();
                            Int32 type_object = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[3]);
                            String get_styles_str = "SELECT * FROM ViewCultureProductivityStyle WHERE id_culture = " + eventArgument.Split(':')[1].Split('|')[2];
                            SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                            get_styles.Fill(productivityDS, "ProductivityStyles");

                            String get_plots_productivity_str = "", legend_str = "";

                            //j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                            j_script = RemoveLayers();

                            switch (type_object)
                            {
                                case 1:
                                    {
                                        get_plots_productivity_str = "exec [dbo].[GetPlotsWithHBProductivity] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1] + ", " + eventArgument.Split(':')[1].Split('|')[2];
                                        //j_script += "\nCallServer('select_organization:' + " + eventArgument.Split(':')[1].Split('|')[0] + ", 'null');";
                                        break;
                                    }
                                case 2:
                                    {
                                        DataSet layersDS = new DataSet();
                                        get_plots_productivity_str = "exec [dbo].[GetPlotsWithHBProductivityRegion] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1] + ", " + eventArgument.Split(':')[1].Split('|')[2];
                                        String get_plots_str = "exec [dbo].[GetLastPlotsRegionGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                        SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                        plots_geo_data.Fill(layersDS, "Plots");

                                        String feature_string = String.Empty,
                                               feature_name = String.Empty,
                                               properties_string = String.Empty;
                                        if (CheckRowsCount(layersDS, "Plots"))
                                        {
                                            /*j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                       "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                       "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                            id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();*/
                                            id_organization = "0";
                                            year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                            tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                            code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                            j_script += "\n$(\"#OrganizationCB option[value='0']\").prop('selected', true);";
                                            j_script += "\n$(\"#OrganizationCB option:nth-child(1)\").attr('selected', 'selected');";
                                            j_script += "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                            j_script += "\nplots_source = new ol.source.Vector();\nvar feature;";
                                            for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                            {
                                                feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                                feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                                for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                                {
                                                    if (j == 0)
                                                    {
                                                        properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                    }
                                                    if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                    {
                                                        if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                        {
                                                            properties_string += ",";
                                                        }
                                                        properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                    }
                                                    if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                    {
                                                        properties_string += "});";
                                                    }
                                                }
                                                j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                                j_script += "\nplots_source.addFeature(feature);";
                                            }
                                            j_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                            j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:250});";

                                            j_script += "\ncurrent_extent = plots_source.getExtent();";
                                            j_script += "\nmap.addLayer(vector_plots);\nmap.getView().fit(current_extent, map.getSize());";
                                            j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                        }

                                        break;
                                    }
                            }

                            SqlDataAdapter get_plots_productivity = new SqlDataAdapter(get_plots_productivity_str, conn);
                            get_plots_productivity.Fill(productivityDS, "PlotsProductivity");
                            conn.Close();

                            if (CheckRowsCount(productivityDS, "PlotsProductivity"))
                            {
                                j_script += "\nvar plots_productivity = {};";
                                String productivity;
                                for (int i = 0; i < productivityDS.Tables["PlotsProductivity"].Rows.Count; i++)
                                {
                                    productivity = productivityDS.Tables["PlotsProductivity"].Rows[i]["actual_productivity"].ToString();
                                    if (productivity == String.Empty || productivity == null) { productivity = "0"; }
                                    j_script += "\nplots_productivity[\"" + productivityDS.Tables["PlotsProductivity"].Rows[i]["code_plot"].ToString() + "\"] = " + productivity.Replace(',', '.') + ";";
                                    //j_script += "\nalert(plots_cultures[\"" + culturesDS.Tables["PlotsCultures"].Rows[i]["code_plot"].ToString() + "\"]);";
                                }
                                if (CheckRowsCount(productivityDS, "ProductivityStyles"))
                                {
                                    j_script += ("\nfunction SetStyles_Productivity" + ClientID + "(){");
                                    j_script += "\nvar plots_styleCache = {};";
                                    j_script += "\nvar plots_style = function (feature, resolution){";
                                    j_script += "\nvar productivity = plots_productivity[feature.getProperties().code_plot];";
                                    //j_script += "\nalert(productivity);"; //undefined
                                    j_script += "\nif (productivity == null || productivity == 'undefined') { productivity = 0; }";
                                    j_script += ("\nvar plot_text = feature.getProperties().id_plot;");
                                    if (productivityDS.Tables["ProductivityStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    productivityDS.Tables["ProductivityStyles"].Rows[0]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    j_script += "\nif (!plots_styleCache[plot_text]){";
                                    //svitch заменён на if из-за диапазона значений
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < productivityDS.Tables["ProductivityStyles"].Rows.Count; i++)
                                    {
                                        if (i > 0) { j_script += "\nelse "; }
                                        j_script += ("\nif (productivity > " + productivityDS.Tables["ProductivityStyles"].Rows[i]["productivity_from"].ToString().Replace(',', '.') + " && " +
                                                           "productivity <= " + productivityDS.Tables["ProductivityStyles"].Rows[i]["productivity_to"].ToString().Replace(',', '.') + " )");
                                        j_script += "{plots_styleCache[plot_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (productivityDS.Tables["ProductivityStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + productivityDS.Tables["ProductivityStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (productivityDS.Tables["ProductivityStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_width"].ToString());
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (productivityDS.Tables["ProductivityStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (productivityDS.Tables["ProductivityStyles"].Rows[i]["size_font"].ToString() + "px " + productivityDS.Tables["ProductivityStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + productivityDS.Tables["ProductivityStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + productivityDS.Tables["ProductivityStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + productivityDS.Tables["ProductivityStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (productivityDS.Tables["ProductivityStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]})";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];}";
                                    }
                                    /*j_script += "\nelse {plots_styleCache[plot_text] = [new ol.style.Style({";
                                    j_script += "\nfill: new ol.style.Fill({color: 'rgba(0,0,0,0)'})";
                                    j_script += "})];}";*/
                                    j_script += "\n}\nreturn plots_styleCache[plot_text];};";
                                    j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                                    j_script += ("\nSetStyles_Productivity" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Productivity" + ClientID + "();});");
                                }
                                switch (type_object)
                                {
                                    case 1:
                                        {
                                            legend_str = GetLegendJS("productivity", eventArgument.Split(':')[1].Split('|')[0] + "|" + eventArgument.Split(':')[1].Split('|')[2], tour, eventArgument.Split(':')[1].Split('|')[1]);
                                            break;
                                        }
                                    case 2:
                                        {
                                            legend_str = GetLegendJS("productivity_region", eventArgument.Split(':')[1].Split('|')[0] + "|" + eventArgument.Split(':')[1].Split('|')[2], tour, eventArgument.Split(':')[1].Split('|')[1]);
                                            break;
                                        }
                                }

                                j_script += legend_str;

                                j_script += "\n$(\"#MapThemeCB\").val(\"report\");";
                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "productivity_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/
                            break;
                        }
                    case "showfarm":
                        {
                            DataSet layersDS = new DataSet();
                            String id_territory = eventArgument.Split(':')[1].Split('|')[0];
                            String id_region = eventArgument.Split(':')[1].Split('|')[1];

                            if (connection_try)
                            {
                                conn.Open();
                                querry_string = "SELECT * FROM View_Farm_GeoJSON";
                                if (id_region != "0" && id_region != "null" && id_region != null)
                                {
                                    querry_string += (" WHERE id_region = " + id_region);
                                }
                                else if (id_territory != "0" && id_territory != "null" && id_territory != null)
                                {
                                    querry_string += (" WHERE id_territory = " + id_territory);
                                }

                                SqlDataAdapter get_farm_geo_data = new SqlDataAdapter(querry_string, conn);
                                get_farm_geo_data.Fill(layersDS, "Farms");
                                conn.Close();

                                String farms_feature_string = String.Empty, farms_feature_name = String.Empty, farms_properties_string = String.Empty;
                                if (CheckRowsCount(layersDS, "Farms"))
                                {
                                    j_script = "\nfarms_source = new ol.source.Vector();\nvar farm_feature;";
                                    for (int i = 0; i < layersDS.Tables["Farms"].Rows.Count; i++)
                                    {
                                        if (layersDS.Tables["Farms"].Rows[i]["farm_geo_json"].ToString() == "POINT EMPTY") continue;
                                        farms_feature_string = "\nfarm_feature = format.readFeature('" + layersDS.Tables["Farms"].Rows[i]["farm_geo_json"] + "');";
                                        farms_feature_name = "\nfarm_feature.name = '" + layersDS.Tables["Farms"].Rows[i]["number_farm"] + "';";
                                        for (int j = 0; j < layersDS.Tables["Farms"].Columns.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                farms_properties_string = "\nfarm_feature.setProperties({'layer':'farms','id_feature':'" + i.ToString() + "'";
                                            }
                                            if (layersDS.Tables["Farms"].Columns[j].ColumnName.ToString() != "farm_geo_json")
                                            {
                                                if (farms_properties_string[farms_properties_string.Length - 1] != '{' && farms_properties_string[farms_properties_string.Length - 1] != ',')
                                                {
                                                    farms_properties_string += ",";
                                                }
                                                farms_properties_string += ("'" + layersDS.Tables["Farms"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Farms"].Rows[i][j].ToString() + "'");
                                            }
                                            if (j == (layersDS.Tables["Farms"].Columns.Count - 1))
                                            {
                                                farms_properties_string += "});";
                                            }
                                        }
                                        j_script += (farms_feature_string + farms_feature_name + farms_properties_string + "\nfarm_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                        j_script += "\nfarms_source.addFeature(farm_feature);";
                                    }
                                    j_script += "\nvar count_farms = farms_source.getFeatures().length; \nif(count_farms > 0){";
                                    j_script += "\nvector_farms = new ol.layer.Vector({source: farms_source});";
                                    j_script += "\nmap.addLayer(vector_farms);}";

                                    //задание стилей для комплексов
                                    DataSet stylesDS = new DataSet();
                                    conn.Open();
                                    String get_styles_str = "SELECT * FROM ViewTypeFarmStyle";
                                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                                    get_styles.Fill(stylesDS, "TypeFarmStyles");
                                    conn.Close();

                                    if (CheckRowsCount(stylesDS, "TypeFarmStyles"))
                                    {
                                        j_script += ("\nfunction " + "SetStyles_TypeFarm" + ClientID + "(){");
                                        j_script += "\nvar farms_styleCache = {};";
                                        j_script += "\nvar farm_style = function (feature, resolution){";
                                        j_script += ("\nvar farm_text = feature.getProperties().id_farm;");
                                        if (stylesDS.Tables["TypeFarmStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().title_farm;\nif (resolution > " +
                                                        stylesDS.Tables["TypeFarmStyles"].Rows[0]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nif (!farms_styleCache[farm_text]){";
                                        j_script += ("\nswitch (feature.getProperties().id_type_farm){");
                                        String fill_str, stroke_str, font_str;
                                        for (int i = 0; i < stylesDS.Tables["TypeFarmStyles"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + stylesDS.Tables["TypeFarmStyles"].Rows[i]["id_type_farm"].ToString() + "':{farms_styleCache[farm_text] = [new ol.style.Style({");
                                            fill_str = String.Empty;
                                            if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                            {
                                                fill_str += "\nfill: new ol.style.Fill({";
                                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypeFarmStyles"].Rows[i]["red"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["green"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                            }
                                            stroke_str = String.Empty;
                                            if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                            {
                                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                stroke_str += ("\nwidth: " + stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_width"].ToString());
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                                {
                                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                stroke_str += "\n})";
                                            }
                                            font_str = String.Empty;
                                            if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {
                                                font_str += "\ntext: new ol.style.Text({font: '";
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["bold_font"].ToString() == "1")
                                                {
                                                    font_str += "bold ";
                                                }
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["italic_font"].ToString() == "1")
                                                {
                                                    font_str += "italic ";
                                                }
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["underline_font"].ToString() == "1")
                                                {
                                                    font_str += "underline ";
                                                }
                                                font_str += (stylesDS.Tables["TypeFarmStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_name"].ToString() + "',");
                                                font_str += "\ntext: getText(feature, resolution),";
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetX: " + stylesDS.Tables["TypeFarmStyles"].Rows[i]["offset_x"].ToString() + ",");
                                                }
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetY: " + stylesDS.Tables["TypeFarmStyles"].Rows[i]["offset_y"].ToString() + ",");
                                                }
                                                font_str += "\nfill: new ol.style.Fill({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                font_str += "\nstroke: new ol.style.Stroke({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                font_str += ("\nwidth: " + stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_width"].ToString());
                                                if (stylesDS.Tables["TypeFarmStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                {
                                                    font_str += ",\nlineDash: [0.5, 4]})";
                                                }
                                                font_str += "\n})})";
                                            }

                                            if (fill_str != String.Empty)
                                            {
                                                j_script += fill_str;
                                            }
                                            if (stroke_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += stroke_str;
                                            }
                                            if (font_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += font_str;
                                            }
                                            j_script += "})];\nbreak;}";
                                        }
                                        j_script += "}\n}\nreturn farms_styleCache[farm_text];};";
                                        j_script += "\nvector_farms.setStyle(farm_style);}";
                                        j_script += ("\nSetStyles_TypeFarm" + ClientID + "();");
                                        j_script += ("\nmap.on('moveend', function () {\nSetStyles_TypeFarm" + ClientID + "();});");
                                    }
                                    //отображение лагун
                                    j_script += "\nCallServer('showlagoons:" + id_territory + "|" + id_region + "', 'null');";
                                }
                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "farms_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/

                            break;
                        }
                    case "showdig":
                        {
                            break;
                        }
                    case "showlagoons":
                        {
                            DataSet layersDS = new DataSet();
                            String id_territory = eventArgument.Split(':')[1].Split('|')[0];
                            String id_region = eventArgument.Split(':')[1].Split('|')[1];

                            if (connection_try)
                            {
                                conn.Open();
                                querry_string = "SELECT * FROM View_Lagoons_GeoJSON";
                                if (id_region != "0" && id_region != "null" && id_region != null)
                                {
                                    querry_string += (" WHERE id_region = " + id_region);
                                }
                                else if (id_territory != "0" && id_territory != "null" && id_territory != null)
                                {
                                    querry_string += (" WHERE id_territory = " + id_territory);
                                }

                                SqlDataAdapter get_lagoons_geo_data = new SqlDataAdapter(querry_string, conn);
                                get_lagoons_geo_data.Fill(layersDS, "Lagoons");
                                conn.Close();

                                String lagoons_feature_string = String.Empty, lagoons_feature_name = String.Empty, lagoons_properties_string = String.Empty;
                                if (CheckRowsCount(layersDS, "Lagoons"))
                                {
                                    j_script = "\nlagoons_source = new ol.source.Vector();\nvar lagoon_feature;";
                                    for (int i = 0; i < layersDS.Tables["Lagoons"].Rows.Count; i++)
                                    {
                                        lagoons_feature_string = "\nlagoon_feature = format.readFeature('" + layersDS.Tables["Lagoons"].Rows[i]["lagoon_geo_json"] + "');";
                                        lagoons_feature_name = "\nlagoon_feature.name = '" + layersDS.Tables["Lagoons"].Rows[i]["lagoon_number"] + "';";
                                        for (int j = 0; j < layersDS.Tables["Lagoons"].Columns.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                lagoons_properties_string = "\nlagoon_feature.setProperties({'layer':'lagoons','id_feature':'" + i.ToString() + "'";
                                            }
                                            if (layersDS.Tables["Lagoons"].Columns[j].ColumnName.ToString() != "lagoon_geo_json")
                                            {
                                                if (lagoons_properties_string[lagoons_properties_string.Length - 1] != '{' && lagoons_properties_string[lagoons_properties_string.Length - 1] != ',')
                                                {
                                                    lagoons_properties_string += ",";
                                                }
                                                lagoons_properties_string += ("'" + layersDS.Tables["Lagoons"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Lagoons"].Rows[i][j].ToString() + "'");
                                            }
                                            if (j == (layersDS.Tables["Lagoons"].Columns.Count - 1))
                                            {
                                                lagoons_properties_string += "});";
                                            }
                                        }
                                        j_script += (lagoons_feature_string + lagoons_feature_name + lagoons_properties_string + "\nlagoon_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                        j_script += "\nlagoons_source.addFeature(lagoon_feature);";
                                    }
                                    j_script += "\nvar count_lagoons = lagoons_source.getFeatures().length; \nif(count_lagoons > 0){";
                                    j_script += "\nvector_lagoons = new ol.layer.Vector({source: lagoons_source, maxResolution:15});";
                                    j_script += "\nmap.addLayer(vector_lagoons);}";

                                    //задание стилей для лагун
                                    DataSet stylesDS = new DataSet();
                                    conn.Open();
                                    String get_styles_str = "SELECT * FROM ViewLagoonsStyle";
                                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                                    get_styles.Fill(stylesDS, "LagoonsStyles");
                                    conn.Close();

                                    if (CheckRowsCount(stylesDS, "LagoonsStyles"))
                                    {
                                        j_script += ("\nfunction SetStyles_Lagoons" + ClientID + "(){");
                                        j_script += "\nvar lagoons_styleCache = {};";
                                        j_script += "\nvar lagoon_style = function (feature, resolution){";
                                        j_script += ("\nvar lagoon_text = feature.getProperties().id_lagoon;");
                                        if (stylesDS.Tables["LagoonsStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                        {
                                            j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().lagoon_number;\nif (resolution > " +
                                                        stylesDS.Tables["LagoonsStyles"].Rows[0]["maxresolution"].ToString() +
                                                        ") { t = ''; }\nreturn t;};");
                                        }
                                        j_script += "\nif (!lagoons_styleCache[lagoon_text]){";
                                        j_script += ("\nswitch (feature.getProperties().id_type_lagoon){");
                                        String fill_str, stroke_str, font_str;
                                        for (int i = 0; i < stylesDS.Tables["LagoonsStyles"].Rows.Count; i++)
                                        {
                                            j_script += ("\ncase '" + stylesDS.Tables["LagoonsStyles"].Rows[i]["id_type_lagoon"].ToString() + "':{lagoons_styleCache[lagoon_text] = [new ol.style.Style({");
                                            fill_str = String.Empty;
                                            if (stylesDS.Tables["LagoonsStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                            {
                                                fill_str += "\nfill: new ol.style.Fill({";
                                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LagoonsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                            }
                                            stroke_str = String.Empty;
                                            if (stylesDS.Tables["LagoonsStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                            {
                                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                stroke_str += ("\nwidth: " + stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_width"].ToString());
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                                {
                                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                                }
                                                stroke_str += "\n})";
                                            }
                                            font_str = String.Empty;
                                            if (stylesDS.Tables["LagoonsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                            {
                                                font_str += "\ntext: new ol.style.Text({font: '";
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                                {
                                                    font_str += "bold ";
                                                }
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                                {
                                                    font_str += "italic ";
                                                }
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                                {
                                                    font_str += "underline ";
                                                }
                                                font_str += (stylesDS.Tables["LagoonsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["LagoonsStyles"].Rows[i]["font_name"].ToString() + "',");
                                                font_str += "\ntext: getText(feature, resolution),";
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetX: " + stylesDS.Tables["LagoonsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                                }
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                                {
                                                    font_str += ("\noffsetY: " + stylesDS.Tables["LagoonsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                                }
                                                font_str += "\nfill: new ol.style.Fill({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LagoonsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                                font_str += "\nstroke: new ol.style.Stroke({";
                                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                              stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                                font_str += ("\nwidth: " + stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_width"].ToString());
                                                if (stylesDS.Tables["LagoonsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                                {
                                                    font_str += ",\nlineDash: [0.5, 4]})";
                                                }
                                                font_str += "\n})})";
                                            }

                                            if (fill_str != String.Empty)
                                            {
                                                j_script += fill_str;
                                            }
                                            if (stroke_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += stroke_str;
                                            }
                                            if (font_str != String.Empty)
                                            {
                                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                                {
                                                    j_script += ",";
                                                }
                                                j_script += font_str;
                                            }
                                            j_script += "})];\nbreak;}";
                                        }
                                        j_script += "}\n}\nreturn lagoons_styleCache[lagoon_text];};";
                                        j_script += "\nvector_lagoons.setStyle(lagoon_style);}";
                                        j_script += ("\nSetStyles_Lagoons" + ClientID + "();");
                                        j_script += ("\nmap.on('moveend', function () {\nSetStyles_Lagoons" + ClientID + "();});");
                                    }
                                }
                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "lagoons_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/

                            break;
                        }
                    case "erosion_change":
                        {
                            DataSet stylesDS = new DataSet();
                            conn.Open();

                            DataSet layersDS = new DataSet();
                            String get_erosion_change_str = "exec [dbo].[GetErosionIntersectionByOrganization] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1];
                            String get_erosion_change_style_str = "SELECT * FROM ViewErosionSoilChangeStyle";
                            SqlDataAdapter erosion_change_geo_data = new SqlDataAdapter(get_erosion_change_str, conn);
                            erosion_change_geo_data.Fill(layersDS, "ErosionChange");
                            SqlDataAdapter get_erosion_change_style = new SqlDataAdapter(get_erosion_change_style_str, conn);
                            get_erosion_change_style.Fill(stylesDS, "ErosionChangeStyle");
                            conn.Close();

                            String feature_string = String.Empty,
                                   feature_name = String.Empty,
                                   properties_string = String.Empty;
                            if (CheckRowsCount(layersDS, "ErosionChange"))
                            {

                                j_script += "\nvar erosion_change_source = new ol.source.Vector();\nvar erosion_change_feature;";
                                for (int i = 0; i < layersDS.Tables["ErosionChange"].Rows.Count; i++)
                                {
                                    feature_string = "\nerosion_change_feature = format.readFeature('" + layersDS.Tables["ErosionChange"].Rows[i]["erosion_intersection"] + "');";
                                    feature_name = "\nerosion_change_feature.name = '" + i.ToString() + "-" + layersDS.Tables["ErosionChange"].Rows[i]["code_erosion_change"] + "';";
                                    for (int j = 0; j < layersDS.Tables["ErosionChange"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            properties_string = "\nerosion_change_feature.setProperties({'layer':'erosion_intersection','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["ErosionChange"].Columns[j].ColumnName.ToString() != "erosion_intersection")
                                        {
                                            if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                            {
                                                properties_string += ",";
                                            }
                                            properties_string += ("'" + layersDS.Tables["ErosionChange"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["ErosionChange"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["ErosionChange"].Columns.Count - 1))
                                        {
                                            properties_string += "});";
                                        }
                                    }
                                    j_script += (feature_string + feature_name + properties_string + "\nerosion_change_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nerosion_change_source.addFeature(erosion_change_feature);";
                                }
                                j_script += "\nvar count_ec = erosion_change_source.getFeatures().length; \nif(count_ec > 0){";
                                j_script += "\nvector_erosion_intersection = new ol.layer.Vector({source: erosion_change_source, maxResolution:250});";
                                j_script += "\nmap.addLayer(vector_erosion_intersection); vector_erosion_intersection.setZIndex(2);}";

                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "erosion_change_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/

                            if (CheckRowsCount(stylesDS, "ErosionChangeStyle"))
                            {
                                j_script += ("\nfunction SetStyles_erosion_change_" + ClientID + "(){");
                                j_script += "\nvar erosion_change_styleCache = {};";
                                j_script += "\nvar erosing_change_style = function (feature, resolution){";
                                j_script += ("\nvar erosing_change_text = feature.getProperties().id_erosion_change;");
                                if (stylesDS.Tables["ErosionChangeStyle"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                {
                                    j_script += ("\nvar getText = function (feature, resolution) {\nvar t = feature.getProperties().code_erosion_change;\nif (resolution > " +
                                                stylesDS.Tables["ErosionChangeStyle"].Rows[0]["maxresolution"].ToString() +
                                                ") { t = ''; }\nreturn t;};");
                                }
                                j_script += "\nif (!erosion_change_styleCache[erosing_change_text]){";
                                j_script += ("\nswitch (erosing_change_text){");
                                String fill_str, stroke_str, font_str;
                                for (int i = 0; i < stylesDS.Tables["ErosionChangeStyle"].Rows.Count; i++)
                                {
                                    j_script += ("\ncase '" + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["id_erosion_change"].ToString() + "':{erosion_change_styleCache[erosing_change_text] = [new ol.style.Style({");
                                    fill_str = String.Empty;
                                    if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["id_color"].ToString() != String.Empty)
                                    {
                                        fill_str += "\nfill: new ol.style.Fill({";
                                        fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["red"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["green"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                    }
                                    stroke_str = String.Empty;
                                    if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                    {
                                        stroke_str += "\nstroke: new ol.style.Stroke({";
                                        stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        stroke_str += ("\nwidth: " + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_width"].ToString());
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                        {
                                            stroke_str += ",\nlineDash: [0.5, 4]";
                                        }
                                        stroke_str += "\n})";
                                    }
                                    font_str = String.Empty;
                                    if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                    {
                                        font_str += "\ntext: new ol.style.Text({font: '";
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["bold_font"].ToString() == "1")
                                        {
                                            font_str += "bold ";
                                        }
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["italic_font"].ToString() == "1")
                                        {
                                            font_str += "italic ";
                                        }
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["underline_font"].ToString() == "1")
                                        {
                                            font_str += "underline ";
                                        }
                                        font_str += (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_name"].ToString() + "',");
                                        font_str += "\ntext: getText(feature, resolution),";
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["offset_x"].ToString() != String.Empty)
                                        {
                                            font_str += ("\noffsetX: " + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["offset_x"].ToString() + ",");
                                        }
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["offset_y"].ToString() != String.Empty)
                                        {
                                            font_str += ("\noffsetY: " + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["offset_y"].ToString() + ",");
                                        }
                                        font_str += "\nfill: new ol.style.Fill({";
                                        font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                        font_str += "\nstroke: new ol.style.Stroke({";
                                        font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                      stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                        font_str += ("\nwidth: " + stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_width"].ToString());
                                        if (stylesDS.Tables["ErosionChangeStyle"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                        {
                                            font_str += ",\nlineDash: [0.5, 4]})";
                                        }
                                        font_str += "\n})})";
                                    }

                                    if (fill_str != String.Empty)
                                    {
                                        j_script += fill_str;
                                    }
                                    if (stroke_str != String.Empty)
                                    {
                                        if (fill_str != String.Empty)
                                        {
                                            j_script += ",";
                                        }
                                        j_script += stroke_str;
                                    }
                                    if (font_str != String.Empty)
                                    {
                                        if (fill_str != String.Empty || stroke_str != String.Empty)
                                        {
                                            j_script += ",";
                                        }
                                        j_script += font_str;
                                    }
                                    j_script += "})];\nbreak;}";
                                }
                                /*j_script += "\ndefault: {plots_styleCache[plot_text] = [new ol.style.Style({";
                                j_script += "\nfill: new ol.style.Fill({color: 'rgba(0,0,0,1)'})";
                                j_script += "})];\nbreak;}";*/
                                j_script += "}\n}\nreturn erosion_change_styleCache[erosing_change_text];};";
                                j_script += "\nvector_erosion_intersection.setStyle(erosing_change_style);}";
                                j_script += ("\nSetStyles_erosion_change_" + ClientID + "();");
                                // j_script += ("\nmap.on('moveend', function () {\nSetStyles_erosion_change_" + ClientID + "();});");
                            }
                            break;
                        }
                    case "get_region_plots":
                        {
                            j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                            j_script += RemoveLayers();

                            if (eventArgument.Split(':')[1].Split('|')[1] != "null")
                            {
                                DataSet layersDS = new DataSet();

                                conn.Open();
                                String region_plots_querry_string = "exec [dbo].[GetLastPlotsRegionGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                SqlDataAdapter region_plots_querry = new SqlDataAdapter(region_plots_querry_string, conn);
                                region_plots_querry.Fill(layersDS, "Plots");
                                conn.Close();

                                Int32 count_parts = 0;
                                if (CheckRowsCount(layersDS, "Plots"))
                                {
                                    count_parts = Convert.ToInt32(Round(layersDS.Tables["Plots"].Rows.Count / 50, 0));
                                    j_script += "\nplots_source = new ol.source.Vector();";
                                    j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:300});";
                                    j_script += "\nmap.addLayer(vector_plots);";
                                    for (int i = 1; i <= count_parts + 1; i++)
                                    {
                                        j_script += "\nCallServer('get_region_plots_part:" + eventArgument.Split(':')[1].Split('|')[0] + "|" + eventArgument.Split(':')[1].Split('|')[1] +
                                                                                        "|" + i.ToString() + "|" + (count_parts + 1).ToString() + "', 'null');";
                                    }
                                }
                            }
                            break;
                        }
                    case "get_region_plots_part":
                        {
                            DataSet layersDS = new DataSet();
                            DataSet stylesDS = new DataSet();

                            conn.Open();

                            String region_plots_querry_string = "exec [dbo].[GetLastPlotsRegionGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                            SqlDataAdapter region_plots_querry = new SqlDataAdapter(region_plots_querry_string, conn);
                            region_plots_querry.Fill(layersDS, "Plots");

                            String get_styles_str = "exec [dbo].[SelectGroupsStyleBySignificative] '" + eventArgument.Split(':')[1].Split('|')[1] + "'";
                            SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                            get_styles.Fill(stylesDS, "GroupsStyles");

                            conn.Close();

                            if (CheckRowsCount(layersDS, "Plots"))
                            {
                                //добавление карты уклонов
                                String plot_feature_string = String.Empty;
                                String plot_feature_name = String.Empty;
                                String plot_properties_string = String.Empty;
                                j_script = "\nplots_source = vector_plots.getSource();\nvar plot_feature;";
                                //eventArgument.Split(':')[1]
                                Int32 from_i = 0, to_i = 0;
                                from_i = (Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]) - 1) * 50;
                                to_i = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]) * 50;
                                if (to_i > layersDS.Tables["Plots"].Rows.Count) { to_i = layersDS.Tables["Plots"].Rows.Count; };
                                for (int i = from_i; i < to_i; i++)
                                {
                                    plot_feature_string = "\nplot_feature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"].ToString() + "');";
                                    plot_feature_name = "\nplot_feature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"].ToString() + "';";
                                    for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                    {
                                        if (j == 0)
                                        {
                                            plot_properties_string = "\nplot_feature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                        }
                                        if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                        {
                                            if (plot_properties_string[plot_properties_string.Length - 1] != '{' && plot_properties_string[plot_properties_string.Length - 1] != ',')
                                            {
                                                plot_properties_string += ",";
                                            }
                                            plot_properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                        }
                                        if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                        {
                                            plot_properties_string += "});";
                                        }
                                    }
                                    j_script += (plot_feature_string + plot_feature_name + plot_properties_string + "\nplot_feature.getGeometry().transform('EPSG:4326', current_projection);");
                                    j_script += "\nplots_source.addFeature(plot_feature);";
                                }

                                if (CheckRowsCount(stylesDS, "GroupsStyles"))
                                {
                                    j_script += ("\nfunction " + "SetStyles_" + eventArgument.Split(':')[1].Split('|')[1] + ClientID + "(){");
                                    j_script += "\nvar plots_styleCache = {};";
                                    j_script += "\nvar plots_style = function (feature, resolution){";
                                    j_script += ("\nvar plot_text = feature.getProperties().number_plot;");
                                    if (stylesDS.Tables["GroupsStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                                  "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                                  "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    stylesDS.Tables["GroupsStyles"].Rows[0]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    j_script += "\nif (!plots_styleCache[plot_text]){";
                                    j_script += ("\nswitch (feature.getProperties().number_" + eventArgument.Split(':')[1].Split('|')[1] + "_group){");
                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < stylesDS.Tables["GroupsStyles"].Rows.Count; i++)
                                    {
                                        j_script += ("\ncase '" + (i + 1).ToString() + "':{plots_styleCache[plot_text] = [new ol.style.Style({");
                                        fill_str = String.Empty;
                                        if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_width"].ToString());
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (stylesDS.Tables["GroupsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]})";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    j_script += "}\n}\nreturn plots_styleCache[plot_text];};";
                                    j_script += "\nvector_plots.setStyle(plots_style);}";
                                    j_script += ("\nSetStyles_" + eventArgument.Split(':')[1].Split('|')[1] + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_" + eventArgument.Split(':')[1].Split('|')[1] + ClientID + "();});");
                                }
                                //запись лог файлов
                                /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "slope_log_" + current_part.ToString() + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                                SW.Write(j_script);
                                SW.Close();*/
                            }
                            break;
                        }
                    case "showselectyear_organic_fert":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_years_str = String.Empty;

                            switch (type_report_object)
                            {
                                //культуры по организации
                                case "1":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Organizarion WHERE id_organization = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                                case "2":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Region WHERE id_region = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                            }

                            if (get_years_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_years = new SqlDataAdapter(get_years_str, conn);
                                DataSet yearDS = new DataSet();
                                get_years.Fill(yearDS, "ReportYears");
                                conn.Close();

                                if (CheckRowsCount(yearDS, "ReportYears"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectYear2CB\").empty();";
                                    j_script += ("\n$(\"#SelectYear2CB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < yearDS.Tables["ReportYears"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectYear2CB\").append('<option value=\"" + yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "\">" +
                                                     yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "change_phase_pest":
                        {
                            /*String id_phase = eventArgument.Split(':')[1].Split('|')[0];
                            String id_culture = eventArgument.Split(':')[1].Split('|')[1];
                            conn.Open();
                            DataSet tablesDS = new DataSet();
                            if (Convert.ToInt32(id_culture) > 0 && Convert.ToInt32(id_phase) > 0)
                            {
                                String get_pests_str = "SELECT DISTINCT id_pest, title_pest FROM View_Threshold_Pests WHERE id_culture=" + id_culture + " AND id_phase=" +
                                                       id_phase + " ORDER BY title_pest";
                                SqlDataAdapter get_pests = new SqlDataAdapter(get_pests_str, conn);
                                get_pests.Fill(tablesDS, "Pests");

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#PestsCB\").empty();";
                                j_script += ("\n$(\"#PestsCB\").append('<option value=\"0\"></option>');");
                                if (CheckRowsCount(tablesDS, "Pests"))
                                {
                                    //PestsCB.DataSource = tablesDS.Tables["Pests"];
                                    //PestsCB.DataBind();
                                    for (int i = 0; i < tablesDS.Tables["Pests"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#PestsCB\").append('<option value=\"" + tablesDS.Tables["Pests"].Rows[i]["id_pest"].ToString() + "\">" +
                                                     tablesDS.Tables["Pests"].Rows[i]["title_pest"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            conn.Close();*/
                            break;
                        }
                    case "change_phase_disease":
                        {
                            /*String id_phase = eventArgument.Split(':')[1].Split('|')[0];
                            String id_culture = eventArgument.Split(':')[1].Split('|')[1];
                            conn.Open();
                            DataSet tablesDS = new DataSet();
                            if (Convert.ToInt32(id_culture) > 0 && Convert.ToInt32(id_phase) > 0)
                            {
                                String get_pests_str = "SELECT DISTINCT id_disease, title_disease FROM View_Threshold_Diseases WHERE id_culture=" + id_culture + " AND id_phase=" +
                                                       id_phase + " ORDER BY title_disease";
                                SqlDataAdapter get_pests = new SqlDataAdapter(get_pests_str, conn);
                                get_pests.Fill(tablesDS, "Diseases");

                                j_script += "\n$(function () {";
                                j_script += "\n$(\"#DiseasesCB\").empty();";
                                j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"0\"></option>');");
                                if (CheckRowsCount(tablesDS, "Diseases"))
                                {
                                    //DiseasesCB.DataSource = tablesDS.Tables["Diseases"];
                                    //DiseasesCB.DataBind();
                                    for (int i = 0; i < tablesDS.Tables["Diseases"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#DiseasesCB\").append('<option value=\"" + tablesDS.Tables["Diseases"].Rows[i]["id_disease"].ToString() + "\">" +
                                                     tablesDS.Tables["Diseases"].Rows[i]["title_disease"].ToString() + "</option>');");
                                    }
                                }
                                j_script += "});";
                            }
                            conn.Close();*/
                            break;
                        }
                    case "change_weediness":
                        {
                            String weediness = eventArgument.Split(':')[1].Split('|')[0];
                            String weediness_percent = eventArgument.Split(':')[1].Split('|')[1];

                            conn.Open();
                            DataSet tablesDS = new DataSet();
                            if (weediness == "undefined") { weediness = ""; }
                            if (weediness_percent == "undefined") { weediness_percent = ""; }

                            SqlDataAdapter get_weediness = new SqlDataAdapter("exec GetWeediness " + NotNull(weediness) + ", " + NotNull(weediness_percent), conn);
                            get_weediness.Fill(tablesDS, "Weediness");

                            if (CheckRowsCount(tablesDS, "Weediness"))
                            {
                                j_script += "\n$(function () {";
                                j_script += ("\n$(\"#WeedinessTB\").val('" + tablesDS.Tables["Weediness"].Rows[0]["title_weediness"].ToString() + "');");
                                j_script += "});";
                            }

                            conn.Close();

                            break;
                        }
                    case "showselectyear_pests_diseases_weediness":
                        {
                            String type_report_object = eventArgument.Split(':')[1].Split('|')[0];
                            String id_report_object = eventArgument.Split(':')[1].Split('|')[1];
                            String get_years_str = String.Empty;

                            switch (type_report_object)
                            {
                                //культуры по организации
                                case "1":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Organizarion WHERE id_organization = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                                case "2":
                                    {
                                        get_years_str = "SELECT year FROM View_HB_Years_By_Id_Region WHERE id_region = " + id_report_object + " ORDER BY [year]";
                                        break;
                                    }
                            }

                            if (get_years_str != String.Empty && id_report_object != null)
                            {
                                conn.Open();
                                SqlDataAdapter get_years = new SqlDataAdapter(get_years_str, conn);
                                DataSet yearDS = new DataSet();
                                get_years.Fill(yearDS, "ReportYears");
                                conn.Close();

                                if (CheckRowsCount(yearDS, "ReportYears"))
                                {
                                    j_script = "\n$(function () {";
                                    j_script += "\n$(\"#SelectYear3CB\").empty();";
                                    j_script += ("\n$(\"#SelectYear3CB\").append('<option value=\"0\"></option>');");

                                    for (int i = 0; i < yearDS.Tables["ReportYears"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SelectYear3CB\").append('<option value=\"" + yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "\">" +
                                                     yearDS.Tables["ReportYears"].Rows[i]["year"].ToString() + "</option>');");
                                    }
                                    j_script += "});";
                                }
                            }
                            break;
                        }
                    case "weediness":
                        {
                            DataSet weedinessDS = new DataSet();
                            conn.Open();
                            Int32 type_object = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[2]);
                            String get_styles_str = "SELECT * FROM ViewWeedinessStyle";
                            SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                            get_styles.Fill(weedinessDS, "WeedinessStyles");

                            String get_plots_weediness_str = "";

                            //j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                            j_script = RemoveLayers();

                            switch (type_object)
                            {
                                case 1:
                                    {
                                        get_plots_weediness_str = "exec [dbo].[GetPlotsWithHBWeediness] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1];
                                        //j_script += "\nCallServer('select_organization:' + " + eventArgument.Split(':')[1].Split('|')[0] + ", 'null');";
                                        break;
                                    }
                                case 2:
                                    {
                                        DataSet layersDS = new DataSet();
                                        get_plots_weediness_str = "exec [dbo].[GetPlotsWithHBWeedinessRegion] " + eventArgument.Split(':')[1].Split('|')[0] + ", " + eventArgument.Split(':')[1].Split('|')[1];
                                        String get_plots_str = "exec [dbo].[GetLastPlotsRegionGeoJSON] " + eventArgument.Split(':')[1].Split('|')[0];
                                        SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                        plots_geo_data.Fill(layersDS, "Plots");

                                        String feature_string = String.Empty,
                                               feature_name = String.Empty,
                                               properties_string = String.Empty;
                                        if (CheckRowsCount(layersDS, "Plots"))
                                        {
                                            /*j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                       "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                       "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                            id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();*/
                                            id_organization = "0";
                                            year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                            tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                            code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                            j_script += "\n$(\"#OrganizationCB option[value='0']\").prop('selected', true);";
                                            j_script += "\n$(\"#OrganizationCB option:nth-child(1)\").attr('selected', 'selected');";
                                            j_script += "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                            j_script += "\nplots_source = new ol.source.Vector();\nvar feature;";
                                            for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                            {
                                                feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                                feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                                for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                                {
                                                    if (j == 0)
                                                    {
                                                        properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                    }
                                                    if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                    {
                                                        if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                        {
                                                            properties_string += ",";
                                                        }
                                                        properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                    }
                                                    if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                    {
                                                        properties_string += "});";
                                                    }
                                                }
                                                j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                                j_script += "\nplots_source.addFeature(feature);";
                                            }
                                            j_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                            j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:250});";

                                            j_script += "\ncurrent_extent = plots_source.getExtent();";
                                            j_script += "\nmap.addLayer(vector_plots);\nmap.getView().fit(current_extent, map.getSize());";
                                            j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                        }

                                        break;
                                    }
                            }

                            SqlDataAdapter get_plots_weediness = new SqlDataAdapter(get_plots_weediness_str, conn);
                            get_plots_weediness.Fill(weedinessDS, "PlotsWeediness");
                            conn.Close();

                            if (CheckRowsCount(weedinessDS, "PlotsWeediness"))
                            {
                                j_script += "\nvar plots_weediness = {};";
                                String weediness;
                                for (int i = 0; i < weedinessDS.Tables["PlotsWeediness"].Rows.Count; i++)
                                {
                                    weediness = weedinessDS.Tables["PlotsWeediness"].Rows[i]["code_weediness"].ToString();
                                    if (weediness == String.Empty || weediness == null) { weediness = "0"; }
                                    j_script += "\nplots_weediness[\"" + weedinessDS.Tables["PlotsWeediness"].Rows[i]["code_plot"].ToString() + "\"] = " + weediness.Replace(',', '.') + ";";
                                    //j_script += "\nalert(plots_cultures[\"" + culturesDS.Tables["PlotsCultures"].Rows[i]["code_plot"].ToString() + "\"]);";
                                }
                                if (CheckRowsCount(weedinessDS, "WeedinessStyles"))
                                {
                                    j_script += ("\nfunction SetStyles_Weediness" + ClientID + "(){");
                                    j_script += "\nvar plots_styleCache = {};";
                                    j_script += "\nvar plots_style = function (feature, resolution){";
                                    j_script += "\nvar weediness = plots_weediness[feature.getProperties().code_plot];";
                                    //j_script += "\nalert(productivity);"; //undefined
                                    j_script += "\nif (weediness == null || weediness == 'undefined') { weediness = 0; }";
                                    j_script += ("\nvar plot_text = feature.getProperties().id_plot;");
                                    if (weedinessDS.Tables["WeedinessStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                                    {
                                        j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                                    weedinessDS.Tables["WeedinessStyles"].Rows[0]["maxresolution"].ToString() +
                                                    ") { t = ''; }\nreturn t;};");
                                    }
                                    j_script += "\nif (!plots_styleCache[plot_text]){";
                                    j_script += ("\nswitch (weediness){");

                                    String fill_str, stroke_str, font_str;
                                    for (int i = 0; i < weedinessDS.Tables["WeedinessStyles"].Rows.Count; i++)
                                    {

                                        j_script += "\ncase '" + weedinessDS.Tables["WeedinessStyles"].Rows[i]["code_weediness"].ToString() + "': {plots_styleCache[plot_text] = [new ol.style.Style({";
                                        fill_str = String.Empty;
                                        if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                                        {
                                            fill_str += "\nfill: new ol.style.Fill({";
                                            fill_str += ("\ncolor: 'rgba(" + weedinessDS.Tables["WeedinessStyles"].Rows[i]["red"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["green"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["blue"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                                        }
                                        stroke_str = String.Empty;
                                        if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                                        {
                                            stroke_str += "\nstroke: new ol.style.Stroke({";
                                            stroke_str += ("\ncolor: 'rgba(" + weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            stroke_str += ("\nwidth: " + weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_width"].ToString());
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                            {
                                                stroke_str += ",\nlineDash: [0.5, 4]";
                                            }
                                            stroke_str += "\n})";
                                        }
                                        font_str = String.Empty;
                                        if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                                        {
                                            font_str += "\ntext: new ol.style.Text({font: '";
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["bold_font"].ToString() == "1")
                                            {
                                                font_str += "bold ";
                                            }
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["italic_font"].ToString() == "1")
                                            {
                                                font_str += "italic ";
                                            }
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["underline_font"].ToString() == "1")
                                            {
                                                font_str += "underline ";
                                            }
                                            font_str += (weedinessDS.Tables["WeedinessStyles"].Rows[i]["size_font"].ToString() + "px " + weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_name"].ToString() + "',");
                                            font_str += "\ntext: getText(feature, resolution),";
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetX: " + weedinessDS.Tables["WeedinessStyles"].Rows[i]["offset_x"].ToString() + ",");
                                            }
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                            {
                                                font_str += ("\noffsetY: " + weedinessDS.Tables["WeedinessStyles"].Rows[i]["offset_y"].ToString() + ",");
                                            }
                                            font_str += "\nfill: new ol.style.Fill({";
                                            font_str += ("\ncolor: 'rgba(" + weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                            font_str += "\nstroke: new ol.style.Stroke({";
                                            font_str += ("\ncolor: 'rgba(" + weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                                          weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                            font_str += ("\nwidth: " + weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_width"].ToString());
                                            if (weedinessDS.Tables["WeedinessStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                            {
                                                font_str += ",\nlineDash: [0.5, 4]})";
                                            }
                                            font_str += "\n})})";
                                        }

                                        if (fill_str != String.Empty)
                                        {
                                            j_script += fill_str;
                                        }
                                        if (stroke_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += stroke_str;
                                        }
                                        if (font_str != String.Empty)
                                        {
                                            if (fill_str != String.Empty || stroke_str != String.Empty)
                                            {
                                                j_script += ",";
                                            }
                                            j_script += font_str;
                                        }
                                        j_script += "})];\nbreak;}";
                                    }
                                    /*j_script += "\nelse {plots_styleCache[plot_text] = [new ol.style.Style({";
                                    j_script += "\nfill: new ol.style.Fill({color: 'rgba(0,0,0,0)'})";
                                    j_script += "})];}";*/
                                    j_script += "}\n}\nreturn plots_styleCache[plot_text];};";
                                    j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                                    j_script += ("\nSetStyles_Weediness" + ClientID + "();");
                                    j_script += ("\nmap.on('moveend', function () {\nSetStyles_Weediness" + ClientID + "();});");
                                }

                                j_script += GetLegendJS("weediness", eventArgument.Split(':')[1].Split('|')[0] + "|" + eventArgument.Split(':')[1].Split('|')[2], tour, eventArgument.Split(':')[1].Split('|')[1]);

                                j_script += "\n$(\"#MapThemeCB\").val(\"report\");";
                            }

                            //запись лога в файл
                            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "weediness_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                            SW.Write(j_script);
                            SW.Close();*/
                            break;
                        }
                    case "get_plots_by_filter":
                        {
                            String filter = eventArgument.Split(':')[1].Split('|')[0];
                            String id_org = eventArgument.Split(':')[1].Split('|')[1];
                            String id_user = eventArgument.Split(':')[1].Split('|')[2];
                            Int32 type_user = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[3]);

                            if (filter.IndexOf("tour") != -1 || filter.IndexOf("year") != -1)
                            {
                                if (connection_try)
                                {
                                    DataSet layersDS = new DataSet();

                                    j_script = "\nif(vector_plots != null) { map.removeLayer(vector_plots); }";
                                    j_script += RemoveLayers();

                                    Boolean select_plots = false;

                                    if (type_user == 0)
                                    {
                                        select_plots = true;
                                    }
                                    else
                                    {
                                        conn.Open();
                                        String select_access_org_str = "SELECT TOP 1 * FROM Access_Organization WHERE id_organization = " + id_org + " AND id_outside_user = " + id_user;
                                        SqlDataAdapter select_access_org = new SqlDataAdapter(select_access_org_str, conn);
                                        select_access_org.Fill(layersDS, "Access_Organization");
                                        conn.Close();
                                        if (CheckRowsCount(layersDS, "Access_Organization"))
                                        {
                                            select_plots = true;
                                        }
                                        else
                                        {
                                            select_plots = false;

                                        }
                                    }

                                    if (select_plots)
                                    {
                                        conn.Open();
                                        String get_plots_str = "exec [dbo].[GetPlotGeoJSONDynamic] '" + filter + "';";
                                        SqlDataAdapter plots_geo_data = new SqlDataAdapter(get_plots_str, conn);
                                        plots_geo_data.Fill(layersDS, "Plots");
                                        conn.Close();

                                        String feature_string = String.Empty,
                                               feature_name = String.Empty,
                                               properties_string = String.Empty;
                                        if (CheckRowsCount(layersDS, "Plots"))
                                        {
                                            j_script += "\ndocument.cookie = 'Agrochim31_For_Map=id_organization=" + layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString() +
                                                       "&year=" + layersDS.Tables["Plots"].Rows[0]["year"].ToString() + "&tour=" + layersDS.Tables["Plots"].Rows[0]["tour"].ToString() +
                                                       "&code_plot=" + layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString() + "';";

                                            id_organization = layersDS.Tables["Plots"].Rows[0]["id_organization"].ToString();
                                            year = layersDS.Tables["Plots"].Rows[0]["year"].ToString();
                                            tour = layersDS.Tables["Plots"].Rows[0]["tour"].ToString();
                                            code_plot = layersDS.Tables["Plots"].Rows[0]["code_plot"].ToString();

                                            j_script += "\nplots_source = new ol.source.Vector();\nvar feature;";
                                            for (int i = 0; i < layersDS.Tables["Plots"].Rows.Count; i++)
                                            {
                                                feature_string = "\nfeature = format.readFeature('" + layersDS.Tables["Plots"].Rows[i]["plot_geo_json"] + "');";
                                                feature_name = "\nfeature.name = '" + layersDS.Tables["Plots"].Rows[i]["code_plot"] + "';";
                                                for (int j = 0; j < layersDS.Tables["Plots"].Columns.Count; j++)
                                                {
                                                    if (j == 0)
                                                    {
                                                        properties_string = "\nfeature.setProperties({'layer':'plots','id_feature':'" + i.ToString() + "'";
                                                    }
                                                    if (layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() != "plot_geo_json")
                                                    {
                                                        if (properties_string[properties_string.Length - 1] != '{' && properties_string[properties_string.Length - 1] != ',')
                                                        {
                                                            properties_string += ",";
                                                        }
                                                        properties_string += ("'" + layersDS.Tables["Plots"].Columns[j].ColumnName.ToString() + "':'" + layersDS.Tables["Plots"].Rows[i][j].ToString() + "'");
                                                    }
                                                    if (j == (layersDS.Tables["Plots"].Columns.Count - 1))
                                                    {
                                                        properties_string += "});";
                                                    }
                                                }
                                                j_script += (feature_string + feature_name + properties_string + "\nfeature.getGeometry().transform('EPSG:4326', current_projection);");
                                                j_script += "\nplots_source.addFeature(feature);";
                                            }
                                            j_script += "\nvar count_plots = plots_source.getFeatures().length; \nif(count_plots > 0){";
                                            j_script += "\nvector_plots = new ol.layer.Vector({source: plots_source, maxResolution:100});";

                                            j_script += "\ncurrent_extent = plots_source.getExtent();";
                                            j_script += "\nmap.addLayer(vector_plots); vector_plots.setZIndex(1); map.getView().fit(current_extent, map.getSize());";
                                            j_script += "\nmap.removeControl(zoom_to_extent); zoom_to_extent = new ol.control.ZoomToExtent({extent: current_extent}); map.addControl(zoom_to_extent);};";
                                            j_script += "\nShowLegend();";

                                            //js_script += "\nvector_plots.on('select', function (){if(selectClick.getFeatures().getLength() > 0){var code_plot = selectClick.getFeatures()[0].getProperties().code_plot; CallServer(('code_plot:' + code_plot + '|' + $('#Year3CB').val()), 'null');}});";
                                            /*j_script += ("\nif (selectClick != null){map.on('click', function (e) {" +
                                                         "map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {" +
                                                         "if (feature.getGeometry().getType() == 'Point'){" +
                                                         "CallServer(('soil_point:' + feature.name), 'null');}" +
                                                         "else if (feature.getGeometry().getType() == 'Polygon' || feature.getGeometry().getType() == 'MultiPolygon'){" +
                                                         "CallServer(('code_plot:' + feature.name + '|' + $('#Year3CB').val()), 'null');}});});}");*/
                                        }
                                    }
                                }
                                j_script += "\nvar theme = $(\"#MapThemeCB\").val();" +
                                            "\nif($(\"#OrganizationCB\").val() != 0){CallServer(theme, 'null');}" +
                                            "\nif($(\"#TerritoryCB\").val() != 0){CallServer('region_theme:' + theme, 'null');}" +
                                            "\nif(vector_territory.getSource() != null){CallServer('territory_theme:' + theme, 'null');}" +
                                            "\nCallServer('legend:' + theme, 'null');";

                                //j_script += "\nmap.updateSize();";
                            }
                            else
                            {
                                j_script = "var values = get_cookie('Agrochim31_Map_User').split('&')[0].split('=')[1] +'|'+ get_cookie('Agrochim31_Map_User').split('&')[4].split('=')[1]; " +
                                           "\nCallServer('select_organization:' + $(\"#OrganizationCB\").val() +'|'+ values , 'null');";
                            }
                            break;
                        }
                    case "get_tours_years_by_filter":
                        {
                            if (connection_try)
                            {
                                DataSet ToursYearsDS = new DataSet();
                                String filter = "NULL";

                                if (eventArgument.Split(':')[1] != "" && eventArgument.Split(':')[1] != null)
                                {
                                    filter = eventArgument.Split(':')[1];
                                }

                                String get_tours_str = "exec [dbo].[GetToursDynamic] '" + filter + "'";
                                String get_years_str = "exec [dbo].[GetYearsDynamic] '" + filter + "'";

                                SqlDataAdapter get_tours = new SqlDataAdapter(get_tours_str, conn);
                                SqlDataAdapter get_years = new SqlDataAdapter(get_years_str, conn);

                                get_tours.Fill(ToursYearsDS, "CurrentTours");
                                get_years.Fill(ToursYearsDS, "CurrentYears");

                                j_script = "\n$(\"#SurveyTourCB\").empty();";
                                j_script += "\n$(\"#SurveyTourCB\").append('<option value=\"NULL\"></option>');";

                                if (CheckRowsCount(ToursYearsDS, "CurrentTours"))
                                {
                                    String last_tour = "";
                                    String children_count = (ToursYearsDS.Tables["CurrentTours"].Rows.Count + 1).ToString();
                                    for (int i = 0; i < ToursYearsDS.Tables["CurrentTours"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SurveyTourCB\").append('<option value=\"" + ToursYearsDS.Tables["CurrentTours"].Rows[i]["tour"].ToString() + "\">" +
                                                    ToursYearsDS.Tables["CurrentTours"].Rows[i]["tour"].ToString() + "</option>');");
                                        last_tour = ToursYearsDS.Tables["CurrentTours"].Rows[i]["tour"].ToString();
                                    }
                                    j_script += "\n$(\"#SurveyTourCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                    j_script += "\n$(\"#SurveyTourCB option[value='" + last_tour + "']\").prop('selected', true);";
                                    //j_script += "\n$(\"#SurveyTourCB option:nth-child(" + children_count + ")\").attr('selected', 'selected');";
                                }

                                j_script += "\n$(\"#SurveyYearCB\").empty();";
                                j_script += "\n$(\"#SurveyYearCB\").append('<option value=\"NULL\"></option>');";

                                if (CheckRowsCount(ToursYearsDS, "CurrentYears"))
                                {
                                    String last_year = "";
                                    String children_count = (ToursYearsDS.Tables["CurrentYears"].Rows.Count + 1).ToString();
                                    for (int i = 0; i < ToursYearsDS.Tables["CurrentYears"].Rows.Count; i++)
                                    {
                                        j_script += ("\n$(\"#SurveyYearCB\").append('<option value=\"" + ToursYearsDS.Tables["CurrentYears"].Rows[i]["year"].ToString() + "\">" +
                                                    ToursYearsDS.Tables["CurrentYears"].Rows[i]["year"].ToString() + "</option>');");
                                        last_year = ToursYearsDS.Tables["CurrentYears"].Rows[i]["year"].ToString();
                                    }
                                    j_script += "\n$(\"#SurveyYearCB option:selected\").each(function(){$(this).prop('selected', false);});";
                                    //j_script += "\n$(\"#SurveyYearCB option[value='" + last_year + "']\").prop('selected', true);";
                                    //j_script += "\n$(\"#SurveyYearCB option:nth-child(" + children_count + ")\").attr('selected', 'selected');";
                                }
                            }
                            break;
                        }
                    case "show_organic_fertilizer":
                        {
                            String id_protocol = eventArgument.Split(':')[1];
                            DataSet surplusDS = new DataSet();

                            conn.Open();
                            String get_plots_organic_fertilizer_str = "EXEC GetOrganicFertilizerSurplus " + id_protocol;
                            SqlDataAdapter get_plots_organic_fertilizer = new SqlDataAdapter(get_plots_organic_fertilizer_str, conn);
                            get_plots_organic_fertilizer.Fill(surplusDS, "PlotsOrganicFertilizer");
                            conn.Close();

                            if (CheckRowsCount(surplusDS, "PlotsOrganicFertilizer"))
                            {
                                j_script = "\nvar plots_organic_fertilizer = {};";
                                String dose;
                                for (int i = 0; i < surplusDS.Tables["PlotsOrganicFertilizer"].Rows.Count; i++)
                                {
                                    dose = surplusDS.Tables["PlotsOrganicFertilizer"].Rows[i]["dose_organic_fertilizer"].ToString();
                                    if (dose == String.Empty || dose == null) { dose = "0"; }
                                    j_script += "\nplots_organic_fertilizer[\"" + surplusDS.Tables["PlotsOrganicFertilizer"].Rows[i]["code_plot"].ToString() + "\"] = " + dose + ";";
                                    //j_script += "\nalert(plots_cultures[\"" + culturesDS.Tables["PlotsCultures"].Rows[i]["code_plot"].ToString() + "\"]);";
                                }

                                j_script += ("\nfunction " + "SetStyles_OrganicFertilizer" + ClientID + "(){");
                                j_script += "\nvar plots_styleCache = {};";
                                j_script += "\nvar plots_style = function (feature, resolution){";
                                j_script += "\nvar dose = plots_organic_fertilizer[feature.getProperties().code_plot];";
                                j_script += "\nif (dose == \"\" || dose == null) { dose = 0; }";
                                j_script += ("\nvar plot_text = feature.getProperties().id_plot;");
                                j_script += ("\nvar getText = function (feature, resolution)" +
                                            "{\nvar t = plots_organic_fertilizer[feature.getProperties().code_plot];" +
                                            "\nif (resolution > 100) { t = ''; }\nreturn t;};");
                                j_script += "\nif (!plots_styleCache[plot_text]){";
                                j_script += ("\nif(dose > 0){");
                                String fill_str, stroke_str, font_str;
                                j_script += "plots_styleCache[plot_text] = [new ol.style.Style({";
                                fill_str = "\nfill: new ol.style.Fill({\ncolor: 'rgba(135,108,153,1)'\n})";
                                stroke_str = "\nstroke: new ol.style.Stroke({\ncolor: 'rgba(139,0,255)',\nwidth: 2\n})";
                                font_str = ("\ntext: new ol.style.Text({font: '12px Arial',\ntext: getText(feature, resolution)," +
                                           "\nfill: new ol.style.Fill({\ncolor: 'rgba(255,0,0,1)'\n})," +
                                           "\nstroke: new ol.style.Stroke({\ncolor: 'rgba(255,255,255,1)',\nwidth: 2\n})})");
                                j_script += (fill_str + "," + stroke_str + "," + font_str);
                                j_script += "r})];}";
                                j_script += "\n}\nreturn plots_styleCache[plot_text];};";
                                j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                                j_script += ("\nSetStyles_OrganicFertilizer" + ClientID + "();");
                                j_script += ("\nmap.on('moveend', function () {\nSetStyles_OrganicFertilizer" + ClientID + "();});");
                            }


                            j_script += GetLegendJS("plots_organic_fertilizer", id_protocol, tour, year);
                            j_script += "\n$(\"#MapThemeCB\").val(\"report\");";
                            break;
                        }

                    case "show_plantations":
                        {
                            String filter = eventArgument.Split(':')[1];
                            DataSet plantationsDS = new DataSet();

                            conn.Open();
                            String get_plantations_str = "EXEC GetPlantations '" + filter + "';";
                            SqlDataAdapter get_plantations = new SqlDataAdapter(get_plantations_str, conn);
                            get_plantations.Fill(plantationsDS, "Plantations");
                            conn.Close();

                            if (CheckRowsCount(plantationsDS, "Plantations"))
                            {
                                j_script = "\n$(function (){\nvar table_plants = '<table id=\"PlantationsT\">'";
                                j_script += "\n+ '<tr bgcolor=\"#b7bfc9\"><td></td><td>Наименование</td><td>Общая площадь, га</td><td>Площадь многолетних насаждений, га</td><td>Год обследования</td><td></td></tr>'";
                                Int32 id_territory = 0, id_region = 0;
                                String title_territory = String.Empty, title_region = String.Empty;
                                Double all_area_territory = 0;
                                Double plant_area_territory = 0;
                                Double all_area_region = 0;
                                Double plant_area_region = 0;

                                for (int i = 0; i < plantationsDS.Tables["Plantations"].Rows.Count; i++)
                                {
                                    if (id_territory != Convert.ToInt32(plantationsDS.Tables["Plantations"].Rows[i]["id_territory"]))
                                    {
                                        id_territory = Convert.ToInt32(plantationsDS.Tables["Plantations"].Rows[i]["id_territory"]);
                                        title_territory = plantationsDS.Tables["Plantations"].Rows[i]["title_territory"].ToString();

                                        j_script = j_script.Replace("@tsa", all_area_territory.ToString());
                                        j_script = j_script.Replace("@tspa", plant_area_territory.ToString());

                                        j_script += "\n+ '<tr bgcolor=\"#b7bfc9\"><td>Область</td><td>" + title_territory + "</td><td>@tsa</td><td>@tspa</td><td></td><td></td></tr>'";

                                        all_area_territory = 0;
                                        plant_area_territory = 0;
                                    }

                                    if (id_region != Convert.ToInt32(plantationsDS.Tables["Plantations"].Rows[i]["id_region"]))
                                    {
                                        id_region = Convert.ToInt32(plantationsDS.Tables["Plantations"].Rows[i]["id_region"]);
                                        title_region = plantationsDS.Tables["Plantations"].Rows[i]["title_region"].ToString();

                                        j_script = j_script.Replace("@rsa", all_area_region.ToString());
                                        j_script = j_script.Replace("@rspa", plant_area_region.ToString());

                                        j_script += "\n+ '<tr bgcolor=\"#dbdbdb\"><td>Район</td><td>" + title_region + "</td><td>@rsa</td><td>@rspa</td><td></td><td></td></tr>'";

                                        all_area_region = 0;
                                        plant_area_region = 0;
                                    }

                                    all_area_territory += Convert.ToDouble(plantationsDS.Tables["Plantations"].Rows[i]["all_area_org"]);
                                    plant_area_territory += Convert.ToDouble(plantationsDS.Tables["Plantations"].Rows[i]["plant_area_org"]);

                                    all_area_region += Convert.ToDouble(plantationsDS.Tables["Plantations"].Rows[i]["all_area_org"]);
                                    plant_area_region += Convert.ToDouble(plantationsDS.Tables["Plantations"].Rows[i]["plant_area_org"]);

                                    j_script += "\n+ '<tr><td></td><td>" + plantationsDS.Tables["Plantations"].Rows[i]["title_organization"].ToString() +
                                                "</td><td>" + plantationsDS.Tables["Plantations"].Rows[i]["all_area_org"].ToString() +
                                                "</td><td>" + plantationsDS.Tables["Plantations"].Rows[i]["plant_area_org"].ToString() +
                                                "</td><td>" + plantationsDS.Tables["Plantations"].Rows[i]["last_year"].ToString() +
                                                "</td><td><a href=\"#\" onclick=\"ShowPlantation(" + plantationsDS.Tables["Plantations"].Rows[i]["id_territory"].ToString() + ", " +
                                                plantationsDS.Tables["Plantations"].Rows[i]["id_region"].ToString() + ", " +
                                                plantationsDS.Tables["Plantations"].Rows[i]["id_organization"].ToString() + ", " +
                                                plantationsDS.Tables["Plantations"].Rows[i]["last_year"].ToString() +
                                                ")\">Показать</a></td></tr>'";

                                    if (i == (plantationsDS.Tables["Plantations"].Rows.Count - 1))
                                    {
                                        j_script = j_script.Replace("@tsa", all_area_territory.ToString());
                                        j_script = j_script.Replace("@tspa", plant_area_territory.ToString());

                                        j_script = j_script.Replace("@rsa", all_area_region.ToString());
                                        j_script = j_script.Replace("@rspa", plant_area_region.ToString());
                                    }
                                }

                                j_script += "\n+ '</table>';";

                                j_script += "\n$('#PlantationsW').get(0).innerHTML = table_plants;\n});";
                            }

                            break;
                        }
                    case "load_region_organization":
                        {
                            String id_territory = eventArgument.Split(':')[1].Split('|')[0];
                            String id_region = eventArgument.Split(':')[1].Split('|')[1];
                            String id_org = eventArgument.Split(':')[1].Split('|')[2];
                            String id_user = eventArgument.Split(':')[1].Split('|')[3];
                            Int32 type_user = Convert.ToInt32(eventArgument.Split(':')[1].Split('|')[4]);

                            j_script = "$(function (){";
                            j_script += "$(\"#TerritoryCB option:selected\").each(function(){$(this).prop('selected', false); });";
                            j_script += ("$(\"#TerritoryCB option[value='" + id_territory + "']\").prop('selected', true);");
                            j_script += GetRegionsJS(id_territory, id_user, type_user);
                            j_script += "$(\"#RegionCB option:selected\").each(function(){$(this).prop('selected', false); });";
                            j_script += ("$(\"#RegionCB option[value='" + id_region + "']\").prop('selected', true);");
                            j_script += GetOrganizationsJS(id_region, id_user, type_user);
                            j_script += "$(\"#OrganizationCB option:selected\").each(function(){$(this).prop('selected', false); });";
                            j_script += ("$(\"#OrganizationCB option[value='" + id_org + "']\").prop('selected', true);");
                            j_script += "});";
                            j_script += "\nCallServer('get_tours_years_by_filter:id_organization=" + id_org + "', 'null');";
                            break;
                        }
                }
                if (eventArgument == "p2o5" || eventArgument == "k2o" || eventArgument == "ph_s" || eventArgument == "humus")
                {
                    conn.Open();
                    String get_styles_str = "exec [dbo].[SelectGroupsStyleBySignificative] '" + eventArgument + "'";
                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                    DataSet stylesDS = new DataSet();
                    get_styles.Fill(stylesDS, "GroupsStyles");
                    conn.Close();

                    j_script = RemoveLayers();
                    if (CheckRowsCount(stylesDS, "GroupsStyles"))
                    {
                        j_script += ("\nfunction SetStyles_" + eventArgument + ClientID + "(){");
                        j_script += "\nvar plots_styleCache = {};";
                        j_script += "\nvar plots_style = function (feature, resolution){";
                        j_script += ("\nvar plot_text = feature.getProperties().number_plot;");
                        if (stylesDS.Tables["GroupsStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                        {
                            /*j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                        stylesDS.Tables["GroupsStyles"].Rows[0]["maxresolution"].ToString() +
                                        ") { t = ''; }\nreturn t;};");*/
                            j_script += ("\nvar getText = function (feature, resolution) {var t;" +
                                          "\nif (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '' && feature.getProperties().id_type_label_text == 1)" +
                                          "\n{ t = feature.getProperties().unique_id_plot; }" +
                                          "\nelse if (feature.getProperties().number_plot != null && feature.getProperties().number_plot != '' && feature.getProperties().id_type_label_text == 2)" +
                                          "\n{ t = feature.getProperties().number_plot; }" +
                                          "\nelse if (feature.getProperties().area != null && feature.getProperties().area != '' && feature.getProperties().id_type_label_text == 3)" +
                                          "\n{ t = feature.getProperties().area; }" +
                                          "\nelse if (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '')" +
                                          "\n{ t = feature.getProperties().unique_id_plot; }" +
                                          "\nelse { t = feature.getProperties().number_plot; };" +
                                          "\nif (resolution > " + stylesDS.Tables["GroupsStyles"].Rows[0]["maxresolution"].ToString() + ") { t = ''; } \nreturn t;};");
                        }
                        j_script += "\nif (!plots_styleCache[plot_text]){";
                        j_script += ("\nswitch (feature.getProperties().number_" + eventArgument + "_group){");
                        String fill_str, stroke_str, font_str;
                        for (int i = 0; i < stylesDS.Tables["GroupsStyles"].Rows.Count; i++)
                        {
                            j_script += ("\ncase '" + (i + 1).ToString() + "':{plots_styleCache[plot_text] = [new ol.style.Style({");
                            fill_str = String.Empty;
                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["red"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["green"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_width"].ToString());
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["GroupsStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["GroupsStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["GroupsStyles"].Rows[i]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["GroupsStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                j_script += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    j_script += ",";
                                }
                                j_script += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    j_script += ",";
                                }
                                j_script += font_str;
                            }
                            j_script += "})];\nbreak;}";
                        }
                        j_script += "}\n}\nreturn plots_styleCache[plot_text];};";
                        j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                        j_script += ("\nSetStyles_" + eventArgument + ClientID + "();");
                        j_script += ("\nmap.on('moveend', function () {\nSetStyles_" + eventArgument + ClientID + "();});");
                    }
                }
                else if (eventArgument == "farmland" || eventArgument == "watering" || eventArgument == "using_plot")
                {
                    String Arg = String.Empty;

                    if (eventArgument == "farmland") { Arg = "TypeFarmland"; }
                    else if (eventArgument == "watering") { Arg = "Watering"; }
                    else if (eventArgument == "using_plot") { Arg = "UsingPlot"; }

                    DataSet stylesDS = new DataSet();
                    conn.Open();
                    String get_styles_str = "SELECT * FROM View" + Arg + "Style;";
                    SqlDataAdapter get_styles = new SqlDataAdapter(get_styles_str, conn);
                    get_styles.Fill(stylesDS, "ArgStyles");
                    conn.Close();

                    j_script = RemoveLayers();
                    if (CheckRowsCount(stylesDS, "ArgStyles"))
                    {
                        j_script += ("\nfunction SetStyles_" + eventArgument + ClientID + "(){");
                        j_script += "\nvar plots_styleCache = {};";
                        j_script += "\nvar plots_style = function (feature, resolution){";
                        j_script += ("\nvar plot_text = feature.getProperties().id_plot;");
                        if (stylesDS.Tables["ArgStyles"].Rows[0]["id_text_style"].ToString() != String.Empty)
                        {
                            /*j_script += ("\nvar getText = function (feature, resolution) {\nvar t; \nif(feature.getProperties().unique_id_plot != null &&" +
                                                      "feature.getProperties().unique_id_plot != '')\n{t = feature.getProperties().unique_id_plot;}" +
                                                      "else {t = feature.getProperties().number_plot;};\nif (resolution > " +
                                        stylesDS.Tables["ArgStyles"].Rows[0]["maxresolution"].ToString() +
                                        ") { t = ''; }\nreturn t;};");*/
                            j_script += ("\nvar getText = function (feature, resolution) {var t;" +
                                          "\nif (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '' && feature.getProperties().id_type_label_text == 1)" +
                                          "\n{ t = feature.getProperties().unique_id_plot; }" +
                                          "\nelse if (feature.getProperties().number_plot != null && feature.getProperties().number_plot != '' && feature.getProperties().id_type_label_text == 2)" +
                                          "\n{ t = feature.getProperties().number_plot; }" +
                                          "\nelse if (feature.getProperties().area != null && feature.getProperties().area != '' && feature.getProperties().id_type_label_text == 3)" +
                                          "\n{ t = feature.getProperties().area; }" +
                                          "\nelse if (feature.getProperties().unique_id_plot != null && feature.getProperties().unique_id_plot != '')" +
                                          "\n{ t = feature.getProperties().unique_id_plot; }" +
                                          "\nelse { t = feature.getProperties().number_plot; };" +
                                          "\nif (resolution > " + stylesDS.Tables["ArgStyles"].Rows[0]["maxresolution"].ToString() + ") { t = ''; } \nreturn t;};");
                        }
                        j_script += "\nif (!plots_styleCache[plot_text]){";
                        j_script += ("\nswitch (feature.getProperties().id_" + eventArgument + "){");
                        String fill_str, stroke_str, font_str;
                        for (int i = 0; i < stylesDS.Tables["ArgStyles"].Rows.Count; i++)
                        {
                            j_script += ("\ncase '" + stylesDS.Tables["ArgStyles"].Rows[i]["id_" + eventArgument].ToString() + "':{plots_styleCache[plot_text] = [new ol.style.Style({");
                            fill_str = String.Empty;
                            if (stylesDS.Tables["ArgStyles"].Rows[i]["id_color"].ToString() != String.Empty)
                            {
                                fill_str += "\nfill: new ol.style.Fill({";
                                fill_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ArgStyles"].Rows[i]["red"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["green"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["blue"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["opacity"].ToString().Replace(',', '.') + ")'\n})");
                            }
                            stroke_str = String.Empty;
                            if (stylesDS.Tables["ArgStyles"].Rows[i]["id_stroke_style"].ToString() != String.Empty)
                            {
                                stroke_str += "\nstroke: new ol.style.Stroke({";
                                stroke_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ArgStyles"].Rows[i]["stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                stroke_str += ("\nwidth: " + stylesDS.Tables["ArgStyles"].Rows[i]["stroke_width"].ToString());
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["stroke_dash_type"].ToString() == "1")
                                {
                                    stroke_str += ",\nlineDash: [0.5, 4]";
                                }
                                stroke_str += "\n})";
                            }
                            font_str = String.Empty;
                            if (stylesDS.Tables["ArgStyles"].Rows[i]["id_text_style"].ToString() != String.Empty)
                            {
                                font_str += "\ntext: new ol.style.Text({font: '";
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["bold_font"].ToString() == "1")
                                {
                                    font_str += "bold ";
                                }
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["italic_font"].ToString() == "1")
                                {
                                    font_str += "italic ";
                                }
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["underline_font"].ToString() == "1")
                                {
                                    font_str += "underline ";
                                }
                                font_str += (stylesDS.Tables["ArgStyles"].Rows[i]["size_font"].ToString() + "px " + stylesDS.Tables["ArgStyles"].Rows[i]["font_name"].ToString() + "',");
                                font_str += "\ntext: getText(feature, resolution),";
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["offset_x"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetX: " + stylesDS.Tables["ArgStyles"].Rows[i]["offset_x"].ToString() + ",");
                                }
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["offset_y"].ToString() != String.Empty)
                                {
                                    font_str += ("\noffsetY: " + stylesDS.Tables["ArgStyles"].Rows[i]["offset_y"].ToString() + ",");
                                }
                                font_str += "\nfill: new ol.style.Fill({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ArgStyles"].Rows[i]["font_red"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_green"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_blue"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_opacity"].ToString().Replace(',', '.') + ")'\n}),");
                                font_str += "\nstroke: new ol.style.Stroke({";
                                font_str += ("\ncolor: 'rgba(" + stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_red"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_green"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_blue"].ToString() + ", " +
                                                              stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_opacity"].ToString().Replace(',', '.') + ")',");
                                font_str += ("\nwidth: " + stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_width"].ToString());
                                if (stylesDS.Tables["ArgStyles"].Rows[i]["font_stroke_dash_type"].ToString() == "1")
                                {
                                    font_str += ",\nlineDash: [0.5, 4]})";
                                }
                                font_str += "\n})})";
                            }

                            if (fill_str != String.Empty)
                            {
                                j_script += fill_str;
                            }
                            if (stroke_str != String.Empty)
                            {
                                if (fill_str != String.Empty)
                                {
                                    j_script += ",";
                                }
                                j_script += stroke_str;
                            }
                            if (font_str != String.Empty)
                            {
                                if (fill_str != String.Empty || stroke_str != String.Empty)
                                {
                                    j_script += ",";
                                }
                                j_script += font_str;
                            }
                            j_script += "})];\nbreak;}";
                        }
                        /*j_script += "\ndefault: {plots_styleCache[plot_text] = [new ol.style.Style({";
                        j_script += "\nfill: new ol.style.Fill({color: 'rgba(0,0,0,1)'})";
                        j_script += "})];\nbreak;}";*/
                        j_script += "}\n}\nreturn plots_styleCache[plot_text];};";
                        j_script += "\nvector_plots.setStyle(plots_style); vector_plots_single.setStyle(plots_style);}";
                        j_script += ("\nSetStyles_" + eventArgument + ClientID + "();");
                        j_script += ("\nmap.on('moveend', function () {\nSetStyles_" + eventArgument + ClientID + "();});");
                    }

                    //запись лога в файл
                    /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "crimea_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
                    SW.Write(j_script);
                    SW.Close();*/
                }
            }

            return j_script;
        }

        public String RemoveLayers()
        {
            String rez = String.Empty;

            rez += "\nif(vector_soil != null) { map.removeLayer(vector_soil); }";
            rez += "\nif(vector_soil_points != null) { map.removeLayer(vector_soil_points); }";
            rez += "\nif(vector_erosion != null) { map.removeLayer(vector_erosion); }";
            rez += "\nif(vector_erosion_intersection != null) { map.removeLayer(vector_erosion_intersection); }";
            rez += "\nif(vector_slope != null) { map.removeLayer(vector_slope); }";
            rez += "\nif(vector_exposure != null) { map.removeLayer(vector_exposure); }";
            rez += "\nif(vector_typing != null) { map.removeLayer(vector_typing); }";

            rez += "\nif(vector_project_plots != null) { map.removeLayer(vector_project_plots); }";
            rez += "\nif(vector_grassing != null) { map.removeLayer(vector_grassing); }";
            rez += "\nif(vector_water_objects != null) { map.removeLayer(vector_water_objects); }";
            rez += "\nif(vector_woodland_belts != null) { map.removeLayer(vector_woodland_belts); }";
            return rez;
        }

        #endregion

        public void SetLegend(String theme)
        {
            if (connection_try)
            {
                DataSet legendDS = new DataSet();
                String get_legend_str = String.Empty;

                if (theme == "null")
                {
                    get_legend_str = "SELECT * FROM View_Default_Legend";
                }
                else if(theme == "soil")
                {
                    //get_legend_str = "SELECT * FROM View_Soil_Legend";
                    //get_legend_str = "exec [dbo].[Get_Soil_Legend] " + id_organization + ", " + year;
                    get_legend_str = "exec [dbo].[Get_Soil_Legend] " + id_organization + ", " + tour;
                }
                else if(theme == "erosion_soil")
                {
                    get_legend_str = "SELECT * FROM View_Erosion_Legend";
                }
                else if(theme == "slope")
                {
                    get_legend_str = "SELECT * FROM View_Slope_Legend";
                }
                else if (theme == "typing")
                {
                    get_legend_str = "SELECT * FROM View_Typing_Legend";
                }
                else
                {
                    get_legend_str = "exec [dbo].[SelectLegendBySignificative] '" + theme + "';";
                }
                conn.Open();
                legendDS.Clear();
                SqlDataAdapter get_legend = new SqlDataAdapter(get_legend_str, conn);
                get_legend.Fill(legendDS, "Legend");
                conn.Close();

                LegendTable.Rows.Clear();
                for (int i = 0; i < legendDS.Tables["Legend"].Rows.Count; i++)
                {
                    LegendTable.Rows.Add(new TableRow());
                    LegendTable.Rows[i].Cells.Add(new TableCell());
                    LegendTable.Rows[i].Cells.Add(new TableCell());
                    if (theme == "typing" && legendDS.Tables["Legend"].Rows[i]["description"].ToString() != String.Empty)
                    {
                        LegendTable.Rows[i].ToolTip = legendDS.Tables["Legend"].Rows[i]["description"].ToString();
                    }
                    LegendTable.Rows[i].Cells[0].Width = 30;
                    if (theme == "erosion_soil")
                    {
                        LegendTable.Rows[i].Cells[0].Text = legendDS.Tables["Legend"].Rows[i]["number_erosion_soil"].ToString();
                    }
                    LegendTable.Rows[i].Cells[0].BackColor = Color.FromArgb(Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["opacity"]), Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["red"]), 
                        Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["green"]), Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["blue"]));
                    LegendTable.Rows[i].Cells[0].ForeColor = Color.FromArgb(Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["font_opacity"]), Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["font_red"]),
                        Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["font_green"]), Convert.ToInt32(legendDS.Tables["Legend"].Rows[i]["font_blue"]));
                    LegendTable.Rows[i].Cells[1].Text = legendDS.Tables["Legend"].Rows[i]["text"].ToString();
                }
            }
        }

        public String GetLegendJS(String theme, String id_object, String tour, String year)
        {
            String legend = String.Empty;

            if (connection_try)
            {
                DataSet legendDS = new DataSet();
                String get_legend_str = String.Empty;
                String legend_marker = String.Empty;
                String title = String.Empty;
                //String legend_title = "\n$('#Legend').attr('title', 'Легенда');";
                String legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда');";

                if (theme == "null")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда');";
                    get_legend_str = "SELECT * FROM View_Default_Legend";
                }
                else if (theme == "soil")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Типы почвы');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Типы почвы');";
                    //get_legend_str = "SELECT * FROM View_Soil_Legend";
                    //get_legend_str = "exec [dbo].[Get_Soil_Legend] " + id_organization + ", " + year;
                    get_legend_str = "exec [dbo].[Get_Soil_Legend] " + id_object + ", " + tour;
                }
                else if (theme == "erosion_soil")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Эрозия почвы');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Эрозия почвы');";
                    get_legend_str = "SELECT * FROM View_Erosion_Legend";
                }
                else if (theme == "slope")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Крутизна склонов');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Крутизна склонов');";
                    get_legend_str = "SELECT * FROM View_Slope_Legend";
                }
                else if (theme == "exposure")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Экспозиция');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Экспозиция');";
                    get_legend_str = "SELECT * FROM View_Exposure_Legend";
                }
                else if (theme == "typing")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Типизация почвы');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Типизация почвы');";
                    get_legend_str = "SELECT * FROM View_Typing_Legend";
                }
                else if (theme == "project")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Проект АЛСЗ');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Проект АЛСЗ');";
                    get_legend_str = "SELECT * FROM View_Project_Legend";
                }
                else if (theme == "culture")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Культуры');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Культуры');";
                    get_legend_str = "[dbo].[GetLegendCulturesWithArea] " + id_object + ", " + year;
                }
                else if (theme == "productivity")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Урожайность');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Урожайность');";
                    get_legend_str = "[dbo].[GetLegendProductivity] " + id_object.Split('|')[0] + ", " + year + ", " + id_object.Split('|')[1];
                }
                else if (theme == "culture_region")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Культуры');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Культуры');";
                    get_legend_str = "[dbo].[GetLegendCulturesWithAreaRegion] " + id_object + ", " + year;
                }
                else if (theme == "productivity_region")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Урожайность');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Урожайность');";
                    get_legend_str = "[dbo].[GetLegendProductivityRegion] " + id_object.Split('|')[0] + ", " + year + ", " + id_object.Split('|')[1];
                }
                else if (theme == "farmland")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Типы угодий');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Типы угодий');";
                    get_legend_str = "SELECT * FROM ViewTypeFarmlandLegend";
                }
                else if (theme == "watering")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Орошаемость');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Орошаемость');";
                    get_legend_str = "SELECT * FROM ViewWateringLegend";
                }
                else if (theme == "using_plot")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Использование земель');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Использование земель');";
                    get_legend_str = "SELECT * FROM ViewUsingPlotLegend";
                }
                else if (theme == "weediness")
                {
                    //legend_title = "\n$('#Legend').attr('title', 'Легенда: Засоренность');";
                    legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Засоренность');";
                    get_legend_str = "SELECT * FROM ViewWeedinessLegend";
                }
                else
                {
                    switch (theme)
                    {
                        case "ph_s":
                            {
                                //legend_title = "\n$('#Legend').attr('title', 'Легенда: Содержание степени кислотности');";
                                legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: pH');";
                                break;
                            }
                        case "p2o5":
                            {
                                //legend_title = "\n$('#Legend').attr('title', 'Легенда: Содержание подвижного фосфора');";
                                legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Содержание P2O5');";
                                break;
                            }
                        case "k2o":
                            {
                                //legend_title = "\n$('#Legend').attr('title', 'Легенда: Содержание подвижного калия');";
                                legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Содержание K2O');";
                                break;
                            }
                        case "humus":
                            {
                                //legend_title = "\n$('#Legend').attr('title', 'Легенда: Содержание органического вещества');";
                                legend_title = "\n$('#Legend').parent().find('.ui-dialog-title').text('Легенда: Содержание орг. вещества');";
                                break;
                            }
                    }
                    get_legend_str = "exec [dbo].[SelectLegendBySignificative] '" + theme + "';";
                }

                String get_farm_legend_str = "SELECT * FROM View_Type_farm_Legend;";
                conn.Open();
                legendDS.Clear();
                SqlDataAdapter get_legend = new SqlDataAdapter(get_legend_str, conn);
                get_legend.Fill(legendDS, "Legend");
                SqlDataAdapter get_farm_legend = new SqlDataAdapter(get_farm_legend_str, conn);
                get_farm_legend.Fill(legendDS, "FarmLegend");
                String erosion_change_str = "SELECT * FROM ViewErosionSoilChangeLegend";
                SqlDataAdapter erosion_change = new SqlDataAdapter(erosion_change_str, conn);
                erosion_change.Fill(legendDS, "ErosionChange");
                conn.Close();

                legend = legend_title;
                legend += "\nvar legend_table =  document.getElementById('LegendTable');";
                legend += "\nlegend_table.innerHTML = '';";
                legend += "\nlegend_table.innerHTML = '";
                for (int i = 0; i < legendDS.Tables["Legend"].Rows.Count; i++)
                {
                    if (theme == "erosion_soil")
                    {
                        legend_marker = legendDS.Tables["Legend"].Rows[i]["number_erosion_soil"].ToString();
                    }
                    else
                    {
                        legend_marker = String.Empty;
                    }
                    if (theme == "typing")
                    {
                        if (legendDS.Tables["Legend"].Rows[i]["description"].ToString() != String.Empty)
                        {
                            title = " title=\"" + legendDS.Tables["Legend"].Rows[i]["description"].ToString() + "\"" + " class=\"tooltip\"";
                        }
                        else
                        {
                            title = String.Empty;
                        }
                    }
                    else
                    {
                        title = String.Empty;
                    }

                    if (theme == "project")
                    {
                        if (legendDS.Tables["Legend"].Rows[i]["legend"].ToString() != "4")
                        {
                            String row = "<tr" + title + ">";
                            Int32 opacity = 0, font_opacity = 0;
                            opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["opacity"].ToString())) * 255);
                            font_opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["font_opacity"].ToString())) * 255);

                            row += ("<td style=\"color:");

                            row += (ColorTranslator.ToHtml(Color.FromArgb(font_opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_red"].ToString())),
                                Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_blue"].ToString())))));
                            row += ";background-color:";
                            row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["red"].ToString())),
                                Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["blue"].ToString())))));
                            row += ";\" width=\"30\" align=\"center\">" + legend_marker + "</td><td>";
                            row += legendDS.Tables["Legend"].Rows[i]["text"].ToString();
                            row += "</td></tr>";
                            legend += row;
                        }
                        else
                        {
                            String row = "<tr" + title + ">";
                            Int32 opacity = 0, font_opacity = 0;
                            opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["opacity"].ToString())) * 255);
                            font_opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["font_opacity"].ToString())) * 255);

                            row += ("<td width=\"30\"><hr style=\"height: 4px; border: none; color:");

                            row += (ColorTranslator.ToHtml(Color.FromArgb(font_opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_red"].ToString())),
                                Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_blue"].ToString())))));
                            row += ";background-color:";
                            row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["red"].ToString())),
                                Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["blue"].ToString())))));
                            row += ";\"></td><td>";
                            row += legendDS.Tables["Legend"].Rows[i]["text"].ToString();
                            row += "</td></tr>";
                            legend += row;
                        }
                    }
                    else
                    {
                        //<td style="color:#FF0000;background-color:#FFC896;width:30px;"></td><td>Пашня</td>
                        String row = "<tr" + title + ">";
                        Int32 opacity = 0, font_opacity = 0;
                        opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["opacity"].ToString())) * 255);
                        font_opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["Legend"].Rows[i]["font_opacity"].ToString())) * 255);

                        row += ("<td style=\"color:");

                        row += (ColorTranslator.ToHtml(Color.FromArgb(font_opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_red"].ToString())),
                            Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["font_blue"].ToString())))));
                        row += ";background-color:";
                        row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["red"].ToString())),
                            Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["Legend"].Rows[i]["blue"].ToString())))));
                        row += ";\" width=\"30\" align=\"center\">" + legend_marker + "</td><td>";
                        row += legendDS.Tables["Legend"].Rows[i]["text"].ToString();
                        row += "</td></tr>";
                        legend += row;
                    }
                }

                if (theme == "plots_organic_fertilizer")
                {

                    String row = ("<tr><td style=\"color:");
                    row += ColorTranslator.ToHtml(Color.FromArgb(255, 135, 108, 153));
                    row += ";background-color:";
                    row += ColorTranslator.ToHtml(Color.FromArgb(255, 135, 108, 153));
                    row += ";\" width=\"30\" align=\"center\"></td><td>";
                    row += "Участок с орг. удобрением";
                    row += "</td></tr>";

                    row += ("<tr><td style=\"color:");
                    row += ColorTranslator.ToHtml(Color.FromArgb(255, 255, 0, 0));
                    row += ";background-color:";
                    row += ColorTranslator.ToHtml(Color.FromArgb(0, 255, 255, 255));
                    row += ";\" width=\"30\" align=\"center\">10</td><td>";
                    row += "Доза орг. удобрения, т/га";
                    row += "</td></tr>";

                    legend += row;
                }

                legend += "';";

                legend += "\nif($('#FarmCB').prop('checked')){";
                legend += "\nlegend_table.innerHTML += '";
                for (int i = 0; i < legendDS.Tables["FarmLegend"].Rows.Count; i++)
                {
                    String row = "<tr>";
                    Int32 opacity = 0, font_opacity = 0;
                    opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["opacity"].ToString())) * 255);
                    font_opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["font_opacity"].ToString())) * 255);

                    row += ("<td style=\"color:");

                    row += (ColorTranslator.ToHtml(Color.FromArgb(font_opacity, Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["font_red"].ToString())),
                        Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["font_green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["font_blue"].ToString())))));
                    row += ";background-color:";
                    row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["red"].ToString())),
                        Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["FarmLegend"].Rows[i]["blue"].ToString())))));
                    row += ";\" width=\"30\" align=\"center\"></td><td>";
                    row += legendDS.Tables["FarmLegend"].Rows[i]["text"].ToString();
                    row += "</td></tr>";
                    legend += row;
                }
                legend += "';}";

                if (theme == "erosion_soil")
                {
                    legend += "\nlegend_table.innerHTML += '";
                    for (int i = 0; i < legendDS.Tables["ErosionChange"].Rows.Count; i++)
                    {
                        String row = "<tr>";
                        Int32 opacity = 0, font_opacity = 0;
                        opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["opacity"].ToString())) * 255);
                        font_opacity = Convert.ToInt32(Convert.ToDouble(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["font_opacity"].ToString())) * 255);

                        row += ("<td style=\"color:");

                        row += (ColorTranslator.ToHtml(Color.FromArgb(font_opacity, Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["font_red"].ToString())),
                            Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["font_green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["font_blue"].ToString())))));
                        row += ";background-color:";
                        row += (ColorTranslator.ToHtml(Color.FromArgb(opacity, Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["red"].ToString())),
                            Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["green"].ToString())), Convert.ToInt32(NotNull(legendDS.Tables["ErosionChange"].Rows[i]["blue"].ToString())))));
                        row += ";\" width=\"30\" align=\"center\"></td><td>";
                        row += legendDS.Tables["ErosionChange"].Rows[i]["text"].ToString();
                        row += "</td></tr>";
                        legend += row;
                    }
                    legend += "';";
                }

                legend += "\n$(\".tooltip\").tooltip({track: true});";
            }

            //запись лога в файл
            /*StreamWriter SW = new StreamWriter(new FileStream(Server.MapPath("~/") + "legend_log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            SW.Write(legend);
            SW.Close();*/

            return legend;
        }

        public String GetTerritoryJS(String id_user, Int32 type_user)
        {
            String js_code = String.Empty;
            String get_territory;
            if (connection_try)
            {
                conn.Open();
                DataSet tablesDS = new DataSet();
                if (type_user == 0)
                {
                    get_territory = "SELECT * FROM Territory ORDER BY title_territory;";
                }
                else
                {
                    get_territory = "SELECT * FROM View_Territory_From_Outside_User WHERE id_outside_user=" + id_user + " ORDER BY title_territory;";
                }

                SqlDataAdapter get_regions_da = new SqlDataAdapter(get_territory, conn);
                get_regions_da.Fill(tablesDS, "SelectTerritory");
                conn.Close();
                js_code = "\n$(\"#TerritoryCB\").empty();";
                js_code += "\n$(\"#TerritoryCB\").append('<option value=\"0\"></option>');";
                if (CheckRowsCount(tablesDS, "SelectTerritory"))
                {
                    for (int i = 0; i < tablesDS.Tables["SelectTerritory"].Rows.Count; i++)
                    {
                        js_code += ("\n$(\"#TerritoryCB\").append('<option value=\"" + tablesDS.Tables["SelectTerritory"].Rows[i]["id_territory"].ToString() + "\">" +
                                    tablesDS.Tables["SelectTerritory"].Rows[i]["title_territory"].ToString() + "</option>');");
                    }
                    js_code += "\n$(\"#TerritoryCB option:nth-child(1)\").attr('selected', 'selected');";
                }
            }
            return js_code;
        }

        public String GetRegionsJS(String id_territory, String id_user, Int32 type_user)
        {
            String js_code = String.Empty;
            String get_regions;
            if (connection_try)
            {
                conn.Open();
                DataSet tablesDS = new DataSet();
                if (type_user == 0)
                {
                    get_regions = "SELECT * FROM Region WHERE id_territory = " + id_territory + " ORDER BY title_region;";
                }
                else
                {
                    //get_regions = "SELECT * FROM View_Region_From_Outside_User WHERE id_territory = " + id_territory + " AND id_outside_user=" + id_user + " ORDER BY title_region;";
                    get_regions = "exec [dbo].[SelectRegionsByAccess] " + id_territory + ", " + id_user;
                }

                SqlDataAdapter get_regions_data = new SqlDataAdapter(get_regions, conn);
                get_regions_data.Fill(tablesDS, "SelectRegions");
                conn.Close();
                js_code = "\n$(\"#RegionCB\").empty();";
                js_code += "\n$(\"#RegionCB\").append('<option value=\"0\"></option>');";
                if (CheckRowsCount(tablesDS, "SelectRegions"))
                {
                    for (int i = 0; i < tablesDS.Tables["SelectRegions"].Rows.Count; i++)
                    {
                        js_code += ("\n$(\"#RegionCB\").append('<option value=\"" + tablesDS.Tables["SelectRegions"].Rows[i]["id_region"].ToString() + "\">" +
                                    tablesDS.Tables["SelectRegions"].Rows[i]["title_region"].ToString() + "</option>');");
                    }
                    js_code += "\n$(\"#RegionCB option:nth-child(1)\").attr('selected', 'selected');";
                }
            }
            return js_code;
        }

        public String GetOrganizationsJS(String id_region, String id_user, Int32 type_user)
        {
            String js_code = String.Empty;
            String get_organizations;
            if (connection_try)
            {
                conn.Open();
                DataSet tablesDS = new DataSet();
                if (type_user == 0)
                {
                    //get_organizations = "SELECT * FROM Organization WHERE id_region=" + id_region + " ORDER BY title_organization;";
                    get_organizations = "exec [dbo].[GetOrganizationsByLastTourGeoJSON] " + id_region;
                }
                else
                {
                    //get_organizations = "SELECT * FROM View_Organization_From_Outside_User WHERE id_region=" + id_region + " AND id_outside_user=" + id_user + " ORDER BY title_organization;";
                    get_organizations = "exec [dbo].[SelectOrganizationsByAccess] " + id_region + ", " + id_user;
                }

                SqlDataAdapter get_organizations_da = new SqlDataAdapter(get_organizations, conn);
                get_organizations_da.Fill(tablesDS, "SelectOrganizations");
                conn.Close();

                js_code = "\n$(\"#OrganizationCB\").empty();";
                js_code += "\n$(\"#OrganizationCB\").append('<option value=\"0\"></option>');";

                if (CheckRowsCount(tablesDS, "SelectOrganizations"))
                {
                    for (int i = 0; i < tablesDS.Tables["SelectOrganizations"].Rows.Count; i++)
                    {
                        js_code += ("\n$(\"#OrganizationCB\").append('<option value=\"" + tablesDS.Tables["SelectOrganizations"].Rows[i]["id_organization"].ToString() + "\">" +
                                    tablesDS.Tables["SelectOrganizations"].Rows[i]["title_organization"].ToString() + "</option>');");
                    }
                    js_code += "\n$(\"#OrganizationCB option:selected\").each(function(){$(this).prop('selected', false);});";
                    js_code += "\n$(\"#OrganizationCB option[value='0']\").prop('selected', true);";
                    js_code += "\n$(\"#OrganizationCB option:nth-child(1)\").attr('selected', 'selected');";
                }
            }
            return js_code;
        }

        public String GetSortCropRotationDescription(String id_sort_crop_rotation)
        {
            String js_code = String.Empty;
            if (connection_try)
            {
                conn.Open();
                DataSet tablesDS = new DataSet();
                String get_sort_crop_rotation_str = "SELECT TOP 1 * FROM Sort_crop_rotation WHERE id_sort_crop_rotation = " + id_sort_crop_rotation;
                SqlDataAdapter get_sort_crop_rotation = new SqlDataAdapter(get_sort_crop_rotation_str, conn);
                get_sort_crop_rotation.Fill(tablesDS, "CurrentSortCropRotation");
                conn.Close();

                js_code += "\n$(function (){";
                if (CheckRowsCount(tablesDS, "CurrentSortCropRotation"))
                {
                    js_code += ("\n$(\"#SpravCropRotationB\").attr('title','" + tablesDS.Tables["CurrentSortCropRotation"].Rows[0]["description"].ToString() + "');");
                }
                else
                {
                    js_code += ("\n$(\"#SpravCropRotationB\").attr('title','');");
                }

                js_code += "});";
            }
            return js_code;
        }

        public Boolean CheckRowsCount(DataSet dataSet, String tableName)
        {
            Boolean result = false;
            for (int tc = 0; tc < dataSet.Tables.Count; tc++)
            {
                if (String.Compare(dataSet.Tables[tc].TableName, tableName) == 0)
                {
                    if (dataSet.Tables[tableName].Rows.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}