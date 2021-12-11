using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace agro_proba
{
    public partial class index : System.Web.UI.Page
    {

        public System.Data.DataView indexDV;
        public DataSet indexDS;
        public SqlDataAdapter adapterRegion, adapterOrganization, adapterDepartment, adapterSoil, adapterCulture, adapterCropRotation, adapterFarmland, adapterErosion, adapterGrading, adapterReportTours,
                               adapterSignificative, adapterGroups, adapterUser, adapterRole, adapterJobTitle, adapterMission, adapterPhones, adapterOldTitle, adapterTour, adapterYear, adapterPlot, adapterEPlot, 
                               adapterSlope,
                               adapterSlopeFromPlot, adapterExposure, adapterMethod, adapterAnalysPhS, adapterAnalysPK, adapterAnalysHA, adapterWorker, adapterChief, adapterMatcher, adapterOrganizationData, 
                               adapterRegionsTourForReport, adapterRegionsTour,
                               adapterAnalysToPlotPlots, adapterAnalysToPlotSamples, adapterRegionsYearForReport, adapterRegionsYear, adapterAnalysSampleData, adapterAnalysHM, adapterTrackers, adapterCars,
                               adapterTerritory, adapterStatisticsAreaRegion;
        public SqlConnection conn;
        public String connStr, selectCommRegion, selectCommOrganization, selectCommDepartment, current_id_region, current_id_organization, current_id_department, selectCommSoil, selectCommCulture, current_password,
                       selectCommCropRotation, selectCommFarmland, selectCommErosion, selectCommGrading, selectCommSignificative, selectCommGroups, selectCommUser, selectCommRole, selectCommJobTitle, selectCommDivisions,
                       selectCommMission, selectCommPhones, current_id_significative, current_id_role,
                       selectCommOldTitle, selectCommTour, selectCommYear, current_code_region, current_code_organization, current_code_department, current_selected_region, current_selected_organization,
                       current_selected_department, selectCommPlot,
                       selectCommEPlot, selectCommSlope, selectCommSlopeFromPlot, selectCommExposure, selectCommMethod, selectReportTours, selectCommAnalysPhS, selectCommAnalysPK, selectCommAnalysHA,
                       selectCommAnalysTours, selectCommAnalysYears, selectCommAnalysHMTours, selectCommAnalysHMYears, selectCommAnalysHMElement,
                       selectCommAnalysRegion, selectCommAnalysOrganization, selectCommAnalysDepartment, selectCommPlanUsers, selectCommPlansWorker, selectCommPlansMissions, selectCommPlans,
                       selectCommSurveyList, selectCommChief, selectCommMatcher, selectCommCheckProbes, selectCommCheckMaps,
                       selectCommCheckPoints, selectCommCheckAct, selectCommPlanCloser, selectCommOrganizationData, selectCommRegionsTourForReport, selectCommAnalysToPlotPlots, selectCommAnalysToPlotSamples,
                       selectCommRegionsYearForReport, selectReportYears, selectCommAnalysSampleData, selectCommAnalysHM, selectCommTrackers, selectCommCars, selectCommTerritory, selectCommStatisticsAreaRegion;
        public HttpCookie cookie, cookie_phone, cookie_old_title, cookie_eplot, cookie_report_tours, cookie_login_user;
        public id_value_action[] phone_iva, old_title_iva;
        public id_action_values[] eplot_iav, new_eplot_iav;
        public Window win_2_3, win_2_1, win_1_1, win_1_2, win_1_3, win_1_4, win_1_5, win_2_4, win_2_5, win_3_1, win_2_2, win_3_2, win_3_3, win_4_1, win_4_2, win_ph, win_ph_control, win_p_k, win_p_k_control, win_ha, win_ha_control, win_the_plan, win_plan, win_plan_region, win_plan_worker,
                      win_plan_mission, win_plan_null, win_driver, win_7_1, win_stickers, win_form_1, win_form_2, win_7_3, win_upload;
        public login_data user_reg_data;
        public Boolean connection_try = false;
        public Int32 login_form_show = 1;

        public static String epsg4326 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
        public static String google_marcator = "PROJCS[\"Google Mercator\",GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9102\"]],AXIS[\"north\",NORTH],AXIS[\"east\",EAST]],PROJECTION[\"Mercator_1SP\"],PARAMETER[\"semi_major\",6378137],PARAMETER[\"semi_minor\",6378137],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",0],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",0],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"East\",EAST],AXIS[\"North\",NORTH]]";

        //структура для временного хранения переменных
        public struct id_value_action
        {
            public Int32 id;
            public Int32 action;
            public String value;
        }

        public struct name_value
        {
            public String name;
            public String value;
        }

        public struct id_action_values
        {
            public Int32 action;
            public Int32 id_elementary_plot;
            public name_value[] nv;
        }

        public struct login_data
        {
            public Int32 id_user;
            public String login;
            public String surname;
            public String name;
            public String patronymic;
            public String authorization;
            public Boolean read;
            public Boolean edit;
            public Boolean add;
            public Boolean delete;
        }

        //функция получения набора данных из куки для работы с таблицей
        public void CookieSplit(id_value_action[] iva, HttpCookie cookie_iva)
        {
            String[] cookie_values = cookie_iva.Value.Split('&');
            for (int i = 0; i < cookie_values.Length; i++)
            {
                iva[i].id = Convert.ToInt32(cookie_values[i].Split('=')[0]);
                iva[i].action = Convert.ToInt32(cookie_values[i].Split('=')[1].Split('|')[0]);
                iva[i].value = cookie_values[i].Split('=')[1].Split('|')[1];
            }
        }
        //функция получения набора данных из куки для работы с таблицей
        public String[] CookieEPlotSplit(HttpCookie cookie_eplots)
        {
            String[] delete_eplots;

            if (cookie_eplots.Value == null || cookie_eplots.Value == String.Empty || cookie_eplots.Value == "")
            {
                delete_eplots = null;
            }
            else
            {
                delete_eplots = cookie_eplots.Value.Split(',');
            }
            return delete_eplots;
        }

        //класс установки флага редактирования
        private class FlagEditing
        {
            public String table_editing;
            public Int32 id_editing;
            public SqlConnection DBConnecction;
            // { get; set; }

            /*public FlagEditing() //конструктор по умоланию
            {
                this.DBConnecction = null;
                this.table_editing = String.Empty;
                this.id_editing = 0;
            }*/

            public FlagEditing(SqlConnection connection, String table, Int32 id) //конструктор с параметрами
            {
                this.DBConnecction = connection;
                this.table_editing = table;
                this.id_editing = id;
            }

            public void SetFlag()
            {
                DBConnecction.Open();
                SqlCommand Command = new SqlCommand("SetFlagEditing", this.DBConnecction);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@table", this.table_editing);
                Command.Parameters.AddWithValue("@id", Convert.ToInt32(this.id_editing));
                Command.ExecuteNonQuery();
                DBConnecction.Close();
            }

            public Boolean GetFlag()
            {
                DBConnecction.Open();
                SqlCommand Command = new SqlCommand("GetFlagEditing", this.DBConnecction);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@table", this.table_editing);
                Command.Parameters.AddWithValue("@id", Convert.ToInt32(this.id_editing));
                Command.Parameters.Add("@flag", SqlDbType.Bit);
                //Command.Parameters["@table"].Direction = ParameterDirection.Input;
                //Command.Parameters["@id"].Direction = ParameterDirection.Input;
                Command.Parameters["@flag"].Direction = ParameterDirection.Output;
                Command.ExecuteNonQuery();
                DBConnecction.Close();
                return Convert.ToBoolean(Command.Parameters["@flag"].Value);
            }

            public void DeleteFlag()
            {
                DBConnecction.Open();
                SqlCommand Command = new SqlCommand("DeleteFlagEditing", this.DBConnecction);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@table", this.table_editing);
                Command.Parameters.AddWithValue("@id", Convert.ToInt32(this.id_editing));
                Command.ExecuteNonQuery();
                DBConnecction.Close();
            }
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
                            if(z.Name == "value")
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

        public static double Round(double value, int digits)
        {
            double scale = Math.Pow(10.0, digits);
            double round = Math.Floor(Math.Abs(value) * scale + 0.5);
            return (Math.Sign(value) * round / scale);
        }

        public String AdaptationValue(String value, String name_sign)
        {
            String number_of_digits = "0";
            Double min_value = 0.0;
            Double max_value = 9999.0;
            String type = "1";
            String temp = value;
            if (temp != null && temp != "null" && temp != "")
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterSignificative = new SqlDataAdapter(selectCommSignificative, conn);
                    adapterSignificative.Fill(indexDS, "Significative");
                    conn.Close();

                    DataTable dt_sign = indexDS.Tables["Significative"];
                    if (dt_sign.Select("name_significative = '" + name_sign + "'").Length > 0)
                    {
                        number_of_digits = dt_sign.Select("name_significative = '" + name_sign + "'")[0]["number_of_digits"].ToString();
                        min_value = Convert.ToDouble(dt_sign.Select("name_significative = '" + name_sign + "'")[0]["min_value"].ToString());
                        max_value = Convert.ToDouble(dt_sign.Select("name_significative = '" + name_sign + "'")[0]["max_value"].ToString());
                    }
                    if (Convert.ToInt32(number_of_digits) > 0) { type = "2"; }


                    temp = ReplaceValue(value, type);
                    temp = Round(Convert.ToDouble(temp), Convert.ToInt32(number_of_digits)).ToString();
                    if (Convert.ToDouble(temp) < min_value)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибочное значение",
                            Message = "Значение не может быть меньше минимума!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                        temp = min_value.ToString();
                    }
                    if (Convert.ToDouble(temp) > max_value)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибочное значение",
                            Message = "Значение не может быть больше максимума!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                        temp = max_value.ToString();
                    }
                }
            }
            return temp;
        }

        public Int32 GetCountWorkingDays(DateTime date_from, DateTime date_to)
        {
            TimeSpan count_days = date_to - date_from;
            Int32 count_working_days = 0;
            for (DateTime day = date_from; day <= date_to; day = day.AddDays(1))
            {
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    count_working_days += 1;
                }
            }
            return count_working_days;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += Page_LoadComplete;

            /*String scriptText = "var template = '<div style=\"height:100%; margin:0px; padding:0px; width:100%; background-color:#{0};\">{1}</div>';" +
                                "var rerender = function(value) { var arr = value.split('|'); if(arr[1] == '' || arr[1] == null) { return arr[0] = arr[0].toFixed(arr[2]).replace('.',','); }" +
                                "else { return Ext.String.format(template, arr[1], arr[0] = arr[0].toFixed(arr[2]).replace('.',',')); }};";*/
            String scriptText = "var template = '<div style=\"height:100%; margin:0px; padding:0px; width:100%; background-color:#{0};\">{1}</div>';" +
                                "var rerender = function(value) { var arr = value.split('|'); if(arr[1] == '' || arr[1] == null) { return arr[0].replace('.',','); }" +
                                "else { return Ext.String.format(template, arr[1], arr[0].replace('.',',')); }};";
            if (!ClientScript.IsClientScriptBlockRegistered("RerenderScript"))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "RerenderScript", scriptText, true);
            }

            //HttpContext.Current.Server.ScriptTimeout = 300000;
            indexDS = new DataSet();
            //connStr = "Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N";
            connStr = SetConnectionString();
            conn = new SqlConnection(connStr);
            connection_try = TryConnection(connStr);

            //проверяем куки
            if (Request.Browser.Cookies)
            {
                cookie = Request.Cookies["Agrochim31"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("Agrochim31");
                    cookie["current_id_region"] = "1";
                    cookie["current_id_organization"] = "1";
                    cookie["current_id_department"] = "0";
                    cookie["current_code_region"] = "0";
                    cookie["current_code_organization"] = "0";
                    cookie["current_code_department"] = "0";
                    cookie["current_selected_region"] = "0";
                    cookie["current_selected_organization"] = "0";
                    cookie["current_selected_department"] = "0";
                    cookie["current_id_significative"] = "0";
                    cookie["current_id_role"] = "0";
                    cookie["current_password"] = "0";
                    cookie["login_form_show"] = "1";
                    cookie.Expires = DateTime.Now.AddMonths(24);
                    Response.Cookies.Add(cookie);
                    SetMinRegion();
                    //SetMinOrganization(current_id_region);
                    //SetMinDepartment(current_id_organization);
                }
                else
                {
                    current_id_region = cookie["current_id_region"];
                    current_id_organization = cookie["current_id_organization"];
                    current_id_department = cookie["current_id_department"];
                    current_code_region = cookie["current_code_region"];
                    current_code_organization = cookie["current_code_organization"];
                    current_code_department = cookie["current_code_department"];
                    current_selected_region = cookie["current_selected_region"];
                    current_selected_organization = cookie["current_selected_organization"];
                    current_selected_department = cookie["current_selected_department"];
                    current_id_significative = cookie["current_id_significative"];
                    current_id_role = cookie["current_id_role"];
                    current_password = cookie["current_password"];
                    login_form_show = Convert.ToInt32(cookie["login_form_show"]);
                }
                cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                if (cookie_login_user == null)
                {
                    cookie_login_user = new HttpCookie("Agrochim31_Authorization");
                    cookie_login_user["id_user"] = "0";
                    cookie_login_user["login"] = "0";
                    cookie_login_user["surname"] = "0";
                    cookie_login_user["name"] = "0";
                    cookie_login_user["patronymic"] = "0";
                    cookie_login_user["authorization"] = "0";
                    cookie_login_user["role"] = "0|0|0|0";
                    cookie_login_user.Expires = DateTime.Now.AddHours(24);
                    Response.Cookies.Add(cookie_login_user);
                }
                else
                {
                    String date_author = DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString() + cookie_login_user["id_user"].ToString();
                    if (String.Compare(cookie_login_user["authorization"].ToString(), GetMd5Hash(date_author)) != 0 || !connection_try || String.Compare(cookie_login_user["id_user"].ToString(), "0") == 0)
                    {
                        cookie_login_user["login"] = "0";
                        cookie_login_user["surname"] = "0";
                        cookie_login_user["name"] = "0";
                        cookie_login_user["patronymic"] = "0";
                        cookie_login_user["authorization"] = "0";
                        cookie_login_user["role"] = "0|0|0|0";
                        UserPassword.Text = "";
                        UserNameL.Text = "...";
                        UserNameL.Hidden = true;
                        ExitB.Hidden = true;
                        /*RegionS.RemoveAll();
                        OrganizationS.RemoveAll();
                        DepartmentS.RemoveAll();*/
                        if (login_form_show == 1)
                        {
                            LoginW.Show();
                            if (UsernameTF.Text == String.Empty)
                            {
                                if (User.Identity.Name.ToString().Split('\\').Length > 1)
                                {
                                    UsernameTF.Text = User.Identity.Name.ToString().Split('\\')[1].ToString();
                                }
                                else
                                {
                                    UsernameTF.Text = User.Identity.Name.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        UsernameTF.Text = cookie_login_user["login"].ToString();
                        LoginW.Close();
                        conn.Open();
                        SqlCommand get_user_data_by_id = new SqlCommand("Get_User_Data_By_Id", conn);
                        get_user_data_by_id.CommandType = CommandType.StoredProcedure;
                        get_user_data_by_id.Parameters.AddWithValue("@id_user", Convert.ToInt32(cookie_login_user["id_user"].ToString()));
                        get_user_data_by_id.Parameters.Add("@surname", SqlDbType.VarChar, 30);
                        get_user_data_by_id.Parameters["@surname"].Direction = ParameterDirection.Output;
                        get_user_data_by_id.Parameters.Add("@name", SqlDbType.VarChar, 20);
                        get_user_data_by_id.Parameters["@name"].Direction = ParameterDirection.Output;
                        get_user_data_by_id.Parameters.Add("@patronymic", SqlDbType.VarChar, 30);
                        get_user_data_by_id.Parameters["@patronymic"].Direction = ParameterDirection.Output;
                        get_user_data_by_id.ExecuteNonQuery();
                        conn.Close();
                        cookie_login_user["surname"] = get_user_data_by_id.Parameters["@surname"].Value.ToString();
                        cookie_login_user["name"] = get_user_data_by_id.Parameters["@name"].Value.ToString();
                        cookie_login_user["patronymic"] = get_user_data_by_id.Parameters["@patronymic"].Value.ToString();

                        UserNameL.Text = cookie_login_user["surname"] + " " + cookie_login_user["name"] + " " + cookie_login_user["patronymic"] + "   ";
                        UserNameL.Hidden = false;
                        ExitB.Hidden = false;
                    }
                    Response.Cookies.Set(cookie_login_user);
                    HttpCookie cookie_map_user = Request.Cookies["Agrochim31_Map_User"];
                    if (cookie_map_user == null)
                    {
                        cookie_map_user = new HttpCookie("Agrochim31_Map_User");
                        Response.Cookies.Add(cookie_map_user);
                    }
                    cookie_map_user["id_user"] = cookie_login_user["id_user"].ToString();
                    cookie_map_user["surname"] = cookie_login_user["surname"].ToString();
                    cookie_map_user["name"] = cookie_login_user["name"].ToString();
                    cookie_map_user["patronymic"] = cookie_login_user["patronymic"].ToString();
                    cookie_map_user["type_user"] = "0";
                    Response.Cookies.Set(cookie_map_user);
                }
                user_reg_data = CookieLoginSplit(cookie_login_user);
                SetSecurity(user_reg_data);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (connection_try)
            {
                selectCommRegion = "SELECT * FROM Region WHERE id_territory = 31";
                selectCommOrganization = "SELECT * FROM ViewOrganization";
                selectCommDepartment = "SELECT * FROM ViewDepartment";
                selectCommTour = "SELECT DISTINCT tour FROM View_Tour_And_Year";
                selectCommYear = "SELECT DISTINCT year FROM View_Tour_And_Year";
                selectCommPlot = "SELECT * FROM ViewPlot";
                selectCommEPlot = "SELECT * FROM ViewEPlot";
                selectCommPhones = "SELECT * FROM Phones";
                selectCommOldTitle = "SELECT * FROM Old_title_organization";
                selectCommFarmland = "SELECT * FROM Type_farmland";
                selectCommCulture = "SELECT * FROM Culture";
                selectCommCropRotation = "SELECT * FROM Type_crop_rotation";
                selectCommSoil = "SELECT * FROM Type_soil";
                selectCommGrading = "SELECT * FROM Type_grading";
                selectCommErosion = "SELECT * FROM Type_erosion";
                selectCommSlope = "SELECT * FROM Slope";
                selectCommSlopeFromPlot = "SELECT * FROM View_Slope_From_Plot";
                selectCommExposure = "SELECT * FROM Exposure";
                selectCommSignificative = "SELECT * FROM Significative";
                selectCommGroups = "SELECT * FROM View_Groups_With_Method";
                selectCommRole = "SELECT * FROM Roles";
                selectCommUser = "SELECT * FROM View_Users";
                selectCommJobTitle = "SELECT * FROM Job_title";
                selectCommDivisions = "SELECT * FROM Divisions";
                selectCommMission = "SELECT * FROM Missions";
                selectCommPlot = "SELECT * FROM ViewPlot";
                selectCommMethod = "SELECT * FROM Method";
                selectCommAnalysPhS = "SELECT * FROM View_Analys_Ph";
                selectCommAnalysPK = "SELECT * FROM View_Analys_P_K";
                selectCommAnalysHA = "SELECT * FROM View_Analys_HA";
                selectCommAnalysTours = "SELECT DISTINCT tour FROM Analys_sample";
                selectCommAnalysYears = "SELECT DISTINCT year FROM Analys_sample";
                selectCommAnalysRegion = "SELECT id_region, title_region FROM Region";
                selectCommAnalysOrganization = "SELECT id_organization, title_organization FROM Organization";
                selectCommAnalysDepartment = "SELECT id_department, title_department FROM Department";
                selectCommPlanUsers = "SELECT id_user, CAST(surname + ' ' + name + ' ' + patronymic AS varchar(82)) AS user_editor FROM Users ORDER BY user_editor";
                selectCommPlansWorker = "SELECT * FROM View_Workers ORDER BY title_worker";
                selectCommPlansMissions = "SELECT * FROM Missions";
                selectCommPlans = "SELECT * FROM View_Plans";
                selectCommSurveyList = "SELECT * FROM View_Survey_list";
                selectCommChief = "SELECT * FROM View_Chief ORDER BY title_chief";
                selectCommMatcher = "SELECT * FROM View_Matcher ORDER BY title_matcher";
                selectCommCheckProbes = "SELECT * FROM View_Check_Probes ORDER BY title_check_probes";
                selectCommCheckMaps = "SELECT * FROM View_Check_Maps ORDER BY title_check_maps";
                selectCommCheckPoints = "SELECT * FROM View_Check_Points ORDER BY title_check_points";
                selectCommCheckAct = "SELECT * FROM View_Check_Act ORDER BY title_check_act";
                selectCommPlanCloser = "SELECT * FROM View_Plan_Closer ORDER BY title_plan_closer";
                selectCommOrganizationData = "SELECT * FROM View_Organization_Data";
                selectCommRegionsTourForReport = "SELECT * FROM View_RegionsTourForReport";
                selectCommAnalysToPlotPlots = "SELECT * FROM Plot";
                selectCommAnalysToPlotSamples = "SELECT * FROM Analys_sample";
                selectCommRegionsYearForReport = "SELECT * FROM View_RegionsYearForReport";
                selectCommAnalysSampleData = "SELECT * FROM View_Analys_Sample";
                selectCommAnalysHM = "SELECT * FROM View_Analys_HM";
                selectCommAnalysHMTours = "SELECT DISTINCT tour FROM Analys_hm";
                selectCommAnalysHMYears = "SELECT DISTINCT year FROM Analys_hm";
                selectCommAnalysHMElement = "SELECT * FROM View_HM_Significative";
                selectCommTrackers = "SELECT * FROM GPS_Trackers";
                selectCommCars = "SELECT * FROM View_Cars";
                selectCommTerritory = "SELECT * FROM Territory";
                selectCommStatisticsAreaRegion = "SELECT * FROM View_StatisticsAreaRegion ORDER BY title_region";

                // вывод таблиц справочников
                // общие справочники
                // типы почв
                BindTypeSoil();
                // культуры
                BindCulture();
                // типы севооборота 
                BindTypeCropRotation();
                // типы сельхозугодий
                BindTypeFarmland();
                // степени эродированности
                BindTypeErosion();
                // механический состав
                BindTypeGrading();
                //уклоны
                BindSlope();
                //экспозиция
                BindExposure();
                //методы определения
                BindMethod();

                // дополнительные справочники
                //должности
                BindJobTitle();
                //задания
                BindMission();
                //трекеры
                BindTrackers();
                //автомобили
                //BindCars();
                //области
                BindTerritory();

                // показатели и группы BindSignificative() и BindGroups() ^                
                // пользователи
                //BindUser();
                // роли
                //BindRole();

                //создаём и открываем соединение
                if (!IsPostBack)
                {
                    FillRegion();
                    //FillOrganization(current_id_region);
                    //FillDepartment(current_id_organization);
                    RegionGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_region));
                    //OrganizationGP.GetSelectionModel().Select(0);
                    //DepartmentGP.GetSelectionModel().Select(0);
                    BindSignificative();
                    BindRole();
                }
                
                conn.Open();
                SqlCommand get_count_sig = new SqlCommand("Get_Significative_Count", conn);
                get_count_sig.CommandType = CommandType.StoredProcedure;
                get_count_sig.Parameters.Add("@count_significative", SqlDbType.Int);
                get_count_sig.Parameters["@count_significative"].Direction = ParameterDirection.Output;
                get_count_sig.ExecuteNonQuery();
                Int32 count_sig = Convert.ToInt32(get_count_sig.Parameters["@count_significative"].Value);
                SqlCommand get_name_digits_sig;
                String name_significative, number_of_digits;
                for (int i = 1; i <= count_sig; i++)
                {
                    get_name_digits_sig = new SqlCommand("Get_Significative_Name_And_Digits", conn);
                    get_name_digits_sig.CommandType = CommandType.StoredProcedure;
                    get_name_digits_sig.Parameters.AddWithValue("@id_significative", i);
                    get_name_digits_sig.Parameters.Add("@name_significative", SqlDbType.VarChar, 20);
                    get_name_digits_sig.Parameters.Add("@number_of_digits", SqlDbType.Int);
                    get_name_digits_sig.Parameters["@name_significative"].Direction = ParameterDirection.Output;
                    get_name_digits_sig.Parameters["@number_of_digits"].Direction = ParameterDirection.Output;
                    get_name_digits_sig.ExecuteNonQuery();
                    name_significative = get_name_digits_sig.Parameters["@name_significative"].Value.ToString();
                    number_of_digits = get_name_digits_sig.Parameters["@number_of_digits"].Value.ToString();
                    //конвертер значений
                    try
                    { //обход ошибки, если поле не найдено
                        if (name_significative != "ph_s" && name_significative != "p2o5" && name_significative != "k2o" && name_significative != "hydrolytic_acid")
                        {
                            PlotsM.Fields.Get(name_significative).Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value = value.toFixed(" + number_of_digits + ").replace('.',',');}";
                        }
                        /*else
                        {
                            PlotsM.Fields.Get(name_significative).Convert.Handler = "if(value == '0' || value == null){return value = ''} else {var split_arr = value.split('|'); return value = (split_arr[0].toFixed(" + number_of_digits + ").replace('.',',') + '|' + split_arr[1]);}";
                        }*/
                        EPlotsM.Fields.Get(name_significative).Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value = value.toFixed(" + number_of_digits + ").replace('.',',');}";
                        //EditEPlotM.Fields.Get(name_significative).Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value = value.toFixed(" + number_of_digits + ").replace('.',',');}";
                    }
                    catch (NullReferenceException exc) { }
                }
                
                try
                { //обход ошибки, если поле не найдено
                    //PlotsM.Fields.Get("area").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value = value.toFixed(2).replace('.',',');}";
                    PlotsM.Fields.Get("area").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value = value.toFixed(2);}";
                }
                catch (NullReferenceException exc) { }
                //закрываем соединение
                conn.Close();
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            AcceptLoginB.Listeners.Click.Handler = "App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());";
            UsernameTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
            UserPassword.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
            // открытие окна ввода кода доступа
            ConnectionB.Listeners.Click.Handler = "App.direct.ClearAccessCodeW();";
            // принятие или отмена кода доступа            
            AcceptAccessCodeB.Listeners.Click.Handler = "App.direct.CodeValue(#{AccessCodeTF}.getValue());";
            CancelAccessCodeB.Listeners.Click.Handler = "#{AccessCodeW}.close(); App.direct.LoginFormShow(1);";
            // окно настроек подключения
            ConnectionW.Listeners.Show.Handler = "App.direct.GetAllValue();";// получение настроек подключения
            AcceptConnectionB.Listeners.Click.Handler = "App.direct.SetAllValue(#{ServerdbTF}.getValue(), #{DbTF}.getValue(), #{LoginTF}.getValue(), #{PasswordTF}.getValue(), #{EditAccessCodeTF}.getValue());";
            CancelConnectionB.Listeners.Click.Handler = "#{ConnectionW}.close(); App.direct.LoginFormShow(1);";

            ExitB.Listeners.Click.Handler = "App.direct.ExitUser();";

            /*RegionGP.GetSelectionModel().Select(0);
            OrganizationGP.GetSelectionModel().Select(0);
            DepartmentGP.GetSelectionModel().Select(0);
            RegionGP.Listeners.ViewReady.Handler = "#{RegionGP}.getSelectionModel().selectRow(0)";
            OrganizationGP.Listeners.ViewReady.Handler = "#{OrganizationGP}.getSelectionModel().selectRow(0)";
            DepartmentGP.Listeners.ViewReady.Handler = "#{DepartmentGP}.getSelectionModel().selectRow(0)";*/
            RegionGP.Listeners.ViewReady.Handler = "App.direct.SelectRegion();";
            /*OrganizationGP.Listeners.ViewReady.Handler = "App.direct.SelectOrganization();";
            DepartmentGP.Listeners.ViewReady.Handler = "App.direct.SelectDepartment();";*/
            //выбор id из таблиц
            //RegionGP.Listeners.CellClick.Handler = "App.direct.SelectedRegion(record.data.id_region, record.data.code_region);";
            RegionGP.Listeners.Select.Handler = "App.direct.SelectedRegion(record.data.id_region, record.data.code_region, this.getStore().indexOf(record));";
            //OrganizationGP.Listeners.CellClick.Handler = "App.direct.SelectedOrganization(record.data.id_organization, record.data.code_organization);";
            OrganizationGP.Listeners.Select.Handler = "App.direct.SelectedOrganization(record.data.id_organization, record.data.code_organization, this.getStore().indexOf(record));";
            //DepartmentGP.Listeners.CellClick.Handler = "App.direct.SelectedDepartment(record.data.id_department, record.data.code_department);";
            DepartmentGP.Listeners.Select.Handler = "App.direct.SelectedDepartment(record.data.id_department, record.data.code_department, this.getStore().indexOf(record));";
            
            AddRegionB.Listeners.Click.Handler = "App.direct.ShowAddRegionW();";
            EditRegionB.Listeners.Click.Handler = "var record = #{RegionGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditRegionW(record.data.id_region,record.data.code_region,record.data.title_region,record.data.okato_region);";
            RegionGP.Listeners.CellDblClick.Handler = "App.direct.ShowEditRegionW(record.data.id_region,record.data.code_region,record.data.title_region,record.data.okato_region);";
            AddOrganizationB.Listeners.Click.Handler = "App.direct.ShowAddOrganizationW();";
            EditOrganizationB.Listeners.Click.Handler = "var record = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditOrganizationW(record.data.id_organization, record.data.code_region, record.data.code_organization, record.data.title_organization, record.data.full_title_organization, record.data.leader, record.data.basis_document, record.data.chief_agronomist, record.data.legal_address, record.data.mailing_address, record.data.email_organization, record.data.inn_organization, record.data.okato_organization, record.data.oktmo_organization, record.data.kpp_organization, record.data.ogrn_organization, record.data.okved_organization, record.data.okpo_organization, record.data.pay_account, record.data.full_bank_name, record.data.bik, record.data.bank_correspond_account);";
            //OrganizationGP.Listeners.CellDblClick.Handler = "App.direct.ShowEditOrganizationW(record.data.id_organization, record.data.code_region, record.data.code_organization, record.data.title_organization, record.data.inn_organization, record.data.okato_organization, record.data.leader, record.data.chief_agronomist);";
            OrganizationGP.Listeners.CellDblClick.Handler = "App.direct.ShowEditdbW();";
            AddDepartmentB.Listeners.Click.Handler = "App.direct.ShowAddDepartmentW();";
            EditDepartmentB.Listeners.Click.Handler = "var record = #{DepartmentGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditDepartmentW(record.data.id_department, record.data.code_organization, record.data.code_department, record.data.title_department);";
            DepartmentGP.Listeners.CellDblClick.Handler = "App.direct.ShowEditDepartmentW(record.data.id_department, record.data.code_organization, record.data.code_department, record.data.title_department);";
            // удаление отделения
            DeleteDepartmentB.Listeners.Click.Handler = "var record = #{DepartmentGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalDepartment(record.data.id_department)";

            //удаление организации
            DeleteOrganizationB.Listeners.Click.Handler = "var record = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.DeleteOrganizationMessage(record.data.id_organization, #{DepartmentGP}.getRowsValues());";
            ConfirmDeleteOrganizationB.Listeners.Click.Handler = "App.direct.DeleteOrganization(#{IdDeleteOrganizationTF}.getValue(), #{ConfirmTF}.getValue());";
            CancelDeleteOrganizationB.Listeners.Click.Handler = "App.direct.CancelDeleteOrganization();";
            // ввод данных по участкам
            EditdbB.Listeners.Click.Handler = "App.direct.ShowEditdbW();";
            // редактирование данных по участкам и образцам

            //AddPlotB.Listeners.Click.Handler = "var count_plots = (#{PlotsGP}.getStore().getCount() + 1); var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowAddPlotW(count_plots, record.data.year, record.data.tour, record.data.date_input, record.data.title_farmland, record.data.title_culture, record.data.title_crop_rotation, record.data.title_type_soil, record.data.title_grading, record.data.title_erosion, record.data.code_farmland, record.data.code_culture, record.data.code_crop_rotation, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion, record.data.number_crop_rotation, record.data.number_field);";
            //AddPlotB.Listeners.Click.Handler = "var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowAddPlotW(record.data.year, record.data.tour, record.data.f_input, record.data.title_farmland, record.data.title_culture, record.data.title_crop_rotation, record.data.title_type_soil, record.data.title_grading, record.data.title_erosion, record.data.code_farmland, record.data.code_culture, record.data.code_crop_rotation, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion, record.data.number_crop_rotation, record.data.number_field);";
            AddPlotB.Listeners.Click.Handler = "#{EditEPlotS}.removeAll(); if(#{PlotsGP}.getStore().getCount() > 0 ) {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0];"
                                             + " var record_year = #{YearsGP}.getView().getSelectionModel().getSelection()[0];"
                                             + " var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0];"
                                             + " App.direct.ShowAddPlotW(record_year.data.year, record_tour.data.tour, record.data.title_farmland,"
                                             + " record.data.title_culture, record.data.title_old_culture, record.data.title_crop_rotation, record.data.title_type_soil, record.data.title_grading, record.data.title_erosion,"
                                             + " record.data.code_farmland, record.data.code_culture, record.data.code_old_culture, record.data.code_crop_rotation, record.data.code_type_soil, record.data.code_grading,"
                                             + " record.data.code_erosion, record.data.number_crop_rotation, record.data.number_field, record.data.date_survey);} else if(#{ToursGP}.getStore().getCount() > 0 && #{YearsGP}.getStore().getCount() > 0)"
                                             + " {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0];"
                                             + " var record_year = #{YearsGP}.getView().getSelectionModel().getSelection()[0];"
                                             + " App.direct.ShowAddPlotWAllNull(record_year.data.year, record_tour.data.tour);}";
            EditPlotB.Listeners.Click.Handler = "#{EditEPlotS}.removeAll(); var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditPlotW(record.data.id_plot, record.data.year, record.data.tour, record.data.date_input, record.data.date_last_edit, record.data.title_farmland, record.data.title_culture, record.data.title_old_culture, record.data.title_crop_rotation, record.data.title_type_soil, record.data.title_grading, record.data.title_erosion, record.data.title_method, record.data.code_farmland, record.data.code_culture, record.data.code_old_culture, record.data.code_crop_rotation, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion, record.data.number_crop_rotation, record.data.number_field, record.data.n, record.data.no3, record.data.no2, record.data.humus, record.data.absorbance_capacity, record.data.total_absorbed_bases, record.data.base_saturation, record.data.p2o5, record.data.k2o, record.data.ph_s, record.data.ph_w, record.data.hydrolytic_acid, record.data.mn, record.data.s, record.data.cu, record.data.zn, record.data.co, record.data.al, record.data.ca, record.data.mo, record.data.b, record.data.mg, record.data.na, record.data.cu_hm, record.data.zn_hm, record.data.cd_hm, record.data.pb_hm, record.data.ni_hm, record.data.hg_hm, record.data.mg_hm, record.data.cr_hm, record.data.fe_hm, record.data.f_hm, record.data.as_hm, record.data.cs137, record.data.sr90, record.data.code_slope, record.data.title_slope, record.data.code_exposure, record.data.title_exposure, record.data.number_plot, record.data.area, record.data.date_survey, record.data.dry_residue);";
            PlotsGP.Listeners.CellDblClick.Handler = "var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditPlotW(record.data.id_plot, record.data.year, record.data.tour, record.data.date_input, record.data.date_last_edit, record.data.title_farmland, record.data.title_culture, record.data.title_old_culture, record.data.title_crop_rotation, record.data.title_type_soil, record.data.title_grading, record.data.title_erosion, record.data.title_method, record.data.code_farmland, record.data.code_culture, record.data.code_old_culture, record.data.code_crop_rotation, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion, record.data.number_crop_rotation, record.data.number_field, record.data.n, record.data.no3, record.data.no2, record.data.humus, record.data.absorbance_capacity, record.data.total_absorbed_bases, record.data.base_saturation, record.data.p2o5, record.data.k2o, record.data.ph_s, record.data.ph_w, record.data.hydrolytic_acid, record.data.mn, record.data.s, record.data.cu, record.data.zn, record.data.co, record.data.al, record.data.ca, record.data.mo, record.data.b, record.data.mg, record.data.na, record.data.cu_hm, record.data.zn_hm, record.data.cd_hm, record.data.pb_hm, record.data.ni_hm, record.data.hg_hm, record.data.mg_hm, record.data.cr_hm, record.data.fe_hm, record.data.f_hm, record.data.as_hm, record.data.cs137, record.data.sr90, record.data.code_slope, record.data.title_slope, record.data.code_exposure, record.data.title_exposure, record.data.number_plot, record.data.area, record.data.date_survey, record.data.dry_residue);";
            //проба с цветом
            //Ph_s.Renderer.Handler = "value = App.direct.ColorPhS(record.data.id_plot, 'ph_s', record.data.ph_s, record.data.id_method);";
            //Ph_s.Renderer.Fn = "function(value) { return '<div style=height:100%;margin:0px;padding:0px;width:100%;background-color:#FFFF00;>' + value + '</div>'; };";
            //var template = '<div style="height:20px; margin:1px; padding:0px; width:100%; background-color:{0};">{1}</div>'; String.format(template, color, value);
            //Ph_s.Renderer.Fn = "function(value) { return '<div style=height:100%;margin:0px;padding:0px;width:100%;background-color:#' + value.split(':')[1] + ';>' + value.split(':')[0] + '</div>'; };";
            //Ph_s.Renderer.Fn = "function(value) { var template = '<div style=height:100%;margin:0px;padding:0px;width:100%;background-color:#{0};>{1}</div>'; return String.format(template, value.split(':')[1], value.split(':')[0]); };";
            Ph_s.Renderer.Fn = "rerender";
            P2O5.Renderer.Fn = "rerender";
            K2O.Renderer.Fn = "rerender";
            Hydrolytic_acid.Renderer.Fn = "rerender";

            // удаление участка
            DeletePlotB.Listeners.Click.Handler = "var record = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalPlot(record.data.id_plot)";

            // координаты
            CoordinatesB.Listeners.Click.Handler = "#{CoordinatesW}.show();";
            // справочники
            GuideSoilMI.Listeners.Click.Handler = "App.direct.ShowSoilW();";
            GuideСultureMI.Listeners.Click.Handler = "App.direct.ShowCultureW();";
            GuideCropRotationMI.Listeners.Click.Handler = "App.direct.ShowCropRotationW();";
            GuideFarmlandMI.Listeners.Click.Handler = "App.direct.ShowFarmlandW();";
            GuideErosionMI.Listeners.Click.Handler = "App.direct.ShowErosionW();";
            GuideGradingMI.Listeners.Click.Handler = "App.direct.ShowGradingW();";
            GuideSlopeMI.Listeners.Click.Handler = "App.direct.ShowSlopeW();";
            GuideExposureMI.Listeners.Click.Handler = "App.direct.ShowExposureW();";
            GuideSignificativeMI.Listeners.Click.Handler = "App.direct.ShowSignificativeW();";
            GuideUserMI.Listeners.Click.Handler = "App.direct.ShowUserW();";
            GuideJobTitleMI.Listeners.Click.Handler = "App.direct.ShowJobTitleW();";
            GuideMissionMI.Listeners.Click.Handler = "App.direct.ShowMissionW();";
            GuideTrackersMI.Listeners.Click.Handler = "App.direct.ShowTrackersW();";
            GuideCarsMI.Listeners.Click.Handler = "App.direct.ShowCarsW();";
            
            // импорт данных
            //уклон и экспозиция
            /*ImportSlopeExposureMI.Listeners.Click.Handler = "#{ImportW}.show(this);";
            ImportSlopesB.Listeners.Click.Handler = "#{ImportW}.show(this);";
            //точки
            ImportPointsMI.Listeners.Click.Handler = "#{ImportW}.show(this);";*/
            //уклон и экспозиция
            ImportSlopeExposureMI.Listeners.Click.Handler = "App.direct.ShowImpornW();";
            ImportSlopesB.Listeners.Click.Handler = "App.direct.ShowImpornW();";
            //точки
            ImportPointsMI.Listeners.Click.Handler = "App.direct.ShowImpornW();";

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //анализы
            //степень кислотности
            AnalysPhMI.Listeners.Click.Handler = "App.direct.ShowAnalysPhSW();";
            //AnalysPhSTourCB.Listeners.Change.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPhSTour(record.data.tour);";
            //AnalysPhSYearCB.Listeners.Change.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPhSYear(record.data.year, #{AnalysPhSTourCB}.getValue());";
            AnalysPhSTourCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPhSTour(record.data.tour);";
            AnalysPhSYearCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPhSYear(record.data.year, #{AnalysPhSTourCB}.getValue());";
            AnalysPhSNumberSampleTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPhS(#{AnalysPhSTourCB}.getValue(), #{AnalysPhSYearCB}.getValue(), #{AnalysPhSIdSampleTF}.getValue(), #{AnalysPhSNumberSampleTF}.getValue(), #{AnalysPhSValuePhSTF}.getValue(), #{AnalysPhSControlTF}.getValue());};";
            AnalysPhSValuePhSTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPhS(#{AnalysPhSTourCB}.getValue(), #{AnalysPhSYearCB}.getValue(), #{AnalysPhSIdSampleTF}.getValue(), #{AnalysPhSNumberSampleTF}.getValue(), #{AnalysPhSValuePhSTF}.getValue(), #{AnalysPhSControlTF}.getValue());};";
            AnalysPhSControlTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPhS(#{AnalysPhSTourCB}.getValue(), #{AnalysPhSYearCB}.getValue(), #{AnalysPhSIdSampleTF}.getValue(), #{AnalysPhSNumberSampleTF}.getValue(), #{AnalysPhSValuePhSTF}.getValue(), #{AnalysPhSControlTF}.getValue());};";
            //DeleteAnalysPhSColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            DeleteAnalysPhSColumn.Listeners.Command.Handler = "App.direct.DeleteAnalysPhS(record.data.id_sample, record.data.id_ph)";
            //AnalysPhSS.Listeners.Remove.Handler = "App.direct.DeleteAnalysPhS(record.data.id_sample, record.data.id_ph);";
            AnalysPhSAddFewB.Listeners.Click.Handler = "App.direct.AddFewAnalysPhS(#{AnalysPhSTourCB}.getValue(), #{AnalysPhSYearCB}.getValue(), #{AnalysPhSFewTF}.getValue());";
            AnalysPhSGP.Listeners.Select.Handler = "App.direct.SelectAnalysPhS(record.data.id_sample, record.data.number_sample, record.data.ph_s_value, record.data.control_value);";

            //фосфор, калий
            AnalysPKMI.Listeners.Click.Handler = "App.direct.ShowAnalysPKW();";
            AnalysPKTourCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPKTour(record.data.tour);";
            AnalysPKYearCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysPKYear(record.data.year, #{AnalysPKTourCB}.getValue());";
            AnalysPKNumberSampleTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKIdSampleTF}.getValue(), #{AnalysPKNumberSampleTF}.getValue(), #{AnalysPKValueP2O5TF}.getValue(), #{AnalysPKValueK2OTF}.getValue(), #{AnalysPKControlP2O5TF}.getValue(), #{AnalysPKControlK2OTF}.getValue());};";
            AnalysPKValueP2O5TF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKIdSampleTF}.getValue(), #{AnalysPKNumberSampleTF}.getValue(), #{AnalysPKValueP2O5TF}.getValue(), #{AnalysPKValueK2OTF}.getValue(), #{AnalysPKControlP2O5TF}.getValue(), #{AnalysPKControlK2OTF}.getValue());};";
            AnalysPKValueK2OTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKIdSampleTF}.getValue(), #{AnalysPKNumberSampleTF}.getValue(), #{AnalysPKValueP2O5TF}.getValue(), #{AnalysPKValueK2OTF}.getValue(), #{AnalysPKControlP2O5TF}.getValue(), #{AnalysPKControlK2OTF}.getValue());};";
            AnalysPKControlP2O5TF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKIdSampleTF}.getValue(), #{AnalysPKNumberSampleTF}.getValue(), #{AnalysPKValueP2O5TF}.getValue(), #{AnalysPKValueK2OTF}.getValue(), #{AnalysPKControlP2O5TF}.getValue(), #{AnalysPKControlK2OTF}.getValue());};";
            AnalysPKControlK2OTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKIdSampleTF}.getValue(), #{AnalysPKNumberSampleTF}.getValue(), #{AnalysPKValueP2O5TF}.getValue(), #{AnalysPKValueK2OTF}.getValue(), #{AnalysPKControlP2O5TF}.getValue(), #{AnalysPKControlK2OTF}.getValue());};";
            DeleteAnalysPKColumn.Listeners.Command.Handler = "App.direct.DeleteAnalysPK(record.data.id_sample, record.data.id_p_k);";
            //AnalysPKAddFewB.Listeners.Click.Handler = "App.direct.AddFewAnalysPK(#{AnalysPKTourCB}.getValue(), #{AnalysPKYearCB}.getValue(), #{AnalysPKFewTF}.getValue());";
            AnalysPKGP.Listeners.Select.Handler = "App.direct.SelectAnalysPK(record.data.id_sample, record.data.number_sample, record.data.p2o5_value, record.data.p2o5_control_value, record.data.k2o_value, record.data.k2o_control_value);";
            
            //гидролитическая кислотность
            AnalysHAMI.Listeners.Click.Handler = "App.direct.ShowAnalysHAW();";
            //AnalysHATourCB.Listeners.Change.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHATour(record.data.tour);";
            //AnalysHAYearCB.Listeners.Change.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHAYear(record.data.year, #{AnalysHaTourCB}.getValue());";
            AnalysHATourCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHATour(record.data.tour);";
            AnalysHAYearCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHAYear(record.data.year, #{AnalysHATourCB}.getValue());";
            AnalysHANumberSampleTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAIdSampleTF}.getValue(), #{AnalysHANumberSampleTF}.getValue(), #{AnalysHAValueHATF}.getValue(), #{AnalysHAControlTF}.getValue());};";
            AnalysHAValueHATF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAIdSampleTF}.getValue(), #{AnalysHANumberSampleTF}.getValue(), #{AnalysHAValueHATF}.getValue(), #{AnalysHAControlTF}.getValue());};";
            AnalysHAControlTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAIdSampleTF}.getValue(), #{AnalysHANumberSampleTF}.getValue(), #{AnalysHAValueHATF}.getValue(), #{AnalysHAControlTF}.getValue());};";
            DeleteAnalysHAColumn.Listeners.Command.Handler = "App.direct.DeleteAnalysHA(record.data.id_sample, record.data.id_ha)";
            //AnalysHAS.Listeners.Remove.Handler = "App.direct.DeleteAnalysHA(record.data.id_sample, record.data.id_ph);";
            AnalysHAAddFewB.Listeners.Click.Handler = "App.direct.AddFewAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAFewTF}.getValue());";
            AnalysHAGP.Listeners.Select.Handler = "App.direct.SelectAnalysHA(record.data.id_sample, record.data.number_sample, record.data.ha_value, record.data.control_value);";
            AnalysHAValuePhSTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAIdSampleTF}.getValue(), #{AnalysHANumberSampleTF}.getValue(), #{AnalysHAValueHATF}.getValue(), #{AnalysHAControlTF}.getValue());};";
            AnalysHAControlPhSTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHA(#{AnalysHATourCB}.getValue(), #{AnalysHAYearCB}.getValue(), #{AnalysHAIdSampleTF}.getValue(), #{AnalysHANumberSampleTF}.getValue(), #{AnalysHAValueHATF}.getValue(), #{AnalysHAControlTF}.getValue());};";
            AnalysHAValuePhSTF.Listeners.KeyUp.Handler = "App.direct.GetHAValuebyPhSValue(#{AnalysHAValuePhSTF}.getValue());";
            AnalysHAControlPhSTF.Listeners.KeyUp.Handler = "App.direct.GetHAControlbyPhSControl(#{AnalysHAControlPhSTF}.getValue());";

            //тяжелые металлы
            AnalysHMMI.Listeners.Click.Handler = "App.direct.ShowAnalysHMW();";
            AnalysHMTourCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMTour(record.data.tour);";
            AnalysHMYearCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMYear(record.data.year, #{AnalysHMTourCB}.getValue());";
            AnalysHMElementCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMSignificative(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), record.data.id_significative);";
            AnalysHMDeleteIC.Listeners.Command.Handler = "App.direct.DeleteAnalysHM(record.data.id_hm)";
            AnalysHMNumberPlotTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMValueTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMControlTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMGP.Listeners.Select.Handler = "App.direct.SelectAnalysHM(record.data.id_hm, record.data.number_plot, record.data.value, record.data.control_value);";
            AnalysHMAddFewB.Listeners.Click.Handler = "App.direct.AddFewAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMFewTF}.getValue());";

            //микроэлементы
            AnalysMicroMI.Listeners.Click.Handler = "App.direct.ShowAnalysMicroW();";
            AnalysHMTourCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMTour(record.data.tour);";
            AnalysHMYearCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMYear(record.data.year, #{AnalysHMTourCB}.getValue());";
            AnalysHMElementCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillAnalysHMSignificative(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), record.data.id_significative);";
            AnalysHMDeleteIC.Listeners.Command.Handler = "App.direct.DeleteAnalysHM(record.data.id_hm)";
            AnalysHMNumberPlotTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMValueTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMControlTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.AddEditAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMIdHMTF}.getValue(), #{AnalysHMNumberPlotTF}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMValueTF}.getValue(), #{AnalysHMControlTF}.getValue());};";
            AnalysHMGP.Listeners.Select.Handler = "App.direct.SelectAnalysHM(record.data.id_hm, record.data.number_plot, record.data.value, record.data.control_value);";
            AnalysHMAddFewB.Listeners.Click.Handler = "App.direct.AddFewAnalysHM(#{AnalysHMTourCB}.getValue(), #{AnalysHMYearCB}.getValue(), #{AnalysHMElementCB}.getValue(), #{AnalysHMFewTF}.getValue());";
            
            //планы-задания
            ResetPlansB.Listeners.Click.Handler = "App.direct.ResetPlans();";
            DeletePlansB.Listeners.Click.Handler = "if(#{PlansGP}.getStore().getCount() > 0){ var record = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; App.direct.DeletePlan(record.data.id_plan); };";
            ReportPlansB.Listeners.Click.Handler = "if(#{PlansGP}.getStore().getCount() > 0){ var record = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ReportPlan(record.data.id_plan); };";
            PlansWorkerCB.Listeners.Select.Handler = "App.direct.ChangePlansFilter()";
            PlansRegionCB.Listeners.Select.Handler = "App.direct.ChangePlansFilter()";
            PlansMissionCB.Listeners.Select.Handler = "App.direct.ChangePlansFilter()";
            PlansFromDF.Listeners.Select.Handler = "App.direct.ChangePlansFilter()";
            PlansToDF.Listeners.Select.Handler = "App.direct.ChangePlansFilter()";
            PlansGP.Listeners.Select.Handler = "var record = this.getView().getSelectionModel().getSelection()[0]; App.direct.SelectedPlans(record.data.id_plan);";
            EditPlansDateFromDF.Listeners.Select.Handler = "App.direct.ChangeDatePlan(#{EditPlansDateFromDF}.value, #{EditPlansDateToDF}.value);";
            EditPlansDateToDF.Listeners.Select.Handler = "App.direct.ChangeDatePlan(#{EditPlansDateFromDF}.value, #{EditPlansDateToDF}.value);";
            EditPlansCountDaysNF.Listeners.Change.Handler = "App.direct.ChangeDayPlan(#{EditPlansDateFromDF}.value, #{EditPlansDateToDF}.value, #{EditPlansCountDaysNF}.value);";
            PlansGP.Listeners.BeforeEdit.Handler = "var record = this.getView().getSelectionModel().getSelection()[0]; App.direct.BindPlansData(record.data.date_from, record.data.date_to, record.data.date_check_probes, record.data.date_check_maps, record.data.date_check_points, record.data.date_check_act);";
            AddPlansB.Listeners.Click.Handler = "var store = #{PlansGP}.store; store.insert(0, {id_plan: '0', count_days: '1'}); #{PlansGP}.editingPlugin.startEdit(0, 0);";
            CopyPlansB.Listeners.Click.Handler = "var store = #{PlansGP}.store; var record = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; store.insert(0, {id_plan: (record.data.id_plan * (-1)), title_worker: record.data.title_worker, title_region: record.data.title_region, title_mission: record.data.title_mission, date_from: record.data.date_from, " +
                                              "date_to: record.data.date_to, count_days: record.data.count_days, count_working_days: record.data.count_working_days, title_chief: record.data.title_chief, title_matcher: record.data.title_matcher, is_driver: record.data.is_driver, title_check_probes: record.data.title_check_probes, title_check_maps: record.data.title_check_maps, " +
                                              "title_check_points: record.data.title_check_points, title_check_act: record.data.title_check_act, date_check_probes: record.data.date_check_probes, date_check_maps: record.data.date_check_maps, date_check_points: record.data.date_check_points, " +
                                              "date_check_act: record.data.date_check_act, plan_result: record.data.plan_result, title_plan_closer: record.data.title_plan_closer }); #{PlansGP}.editingPlugin.startEdit(0, 0);";
            PlansS.Listeners.Update.Handler = "App.direct.AcceptAddEditPlan(record.data.id_plan, record.data.title_worker, record.data.title_region, record.data.title_mission, record.data.date_from, " +
                                              "record.data.date_to, record.data.count_days, record.data.count_working_days, record.data.title_chief, record.data.title_matcher, record.data.is_driver, record.data.title_check_probes, record.data.title_check_maps, " +
                                              "record.data.title_check_points, record.data.title_check_act, record.data.date_check_probes, record.data.date_check_maps, record.data.date_check_points, " +
                                              "record.data.date_check_act, record.data.plan_result, record.data.title_plan_closer);";
            RowEditingPlans.Listeners.CancelEdit.Handler = "App.direct.FillPlans();";

            AddSurveyListB.Listeners.Click.Handler = "if(#{PlansGP}.getStore().getCount() > 0 && #{SurveyListGP}.getStore().getCount() < 7){ var store = #{SurveyListGP}.store; var record_plan = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; store.insert(0, {id_survey_list: '0', id_plan: record_plan.data.id_plan}); #{SurveyListGP}.editingPlugin.startEdit(0, 0); }" +
                                                     "else if(#{SurveyListGP}.getStore().getCount() >= 7){ App.direct.SurveyListMaxCount(); };";
            RowEditingSurveyList.Listeners.CancelEdit.Handler = "if(#{PlansGP}.getStore().getCount() > 0){ var record_plan = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; App.direct.FillSurveyList(record_plan.data.id_plan); };";
            SurveyListGP.Listeners.BeforeEdit.Handler = "var record = this.getView().getSelectionModel().getSelection()[0]; var record_plan = #{PlansGP}.getView().getSelectionModel().getSelection()[0]; App.direct.BindSurveyListData(record_plan.data.id_region);";
            SurveyListS.Listeners.Update.Handler = "App.direct.AcceptAddEditSurveyList(record.data.id_survey_list, record.data.id_plan, record.data.title_organization, record.data.planned_area, record.data.planned_probes, " + 
                                                   "record.data.planned_cuts, record.data.planned_floor_pits, record.data.planned_excavation, record.data.actual_area, record.data.actual_probes, record.data.actual_cuts, "+
                                                   "record.data.actual_floor_pits, record.data.actual_excavation);";
            SurveyListDeleteIC.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            SurveyListS.Listeners.Remove.Handler = "App.direct.DeleteSurveyList(record.data.id_survey_list);";
            PrintStickersB.Listeners.Click.Handler = "var record = #{SurveyListGP}.getView().getSelectionModel().getSelection()[0]; App.direct.PrintStickers(record.data.id_organization, record.data.planned_probes);";
            PrintForm1B.Listeners.Click.Handler = "var record_survey = #{SurveyListGP}.getView().getSelectionModel().getSelection()[0]; var record_plan = #{PlansGP}.getView().getSelectionModel().getSelection()[0];" +
                                                 "App.direct.PrintForm(record_survey.data.id_organization, record_survey.data.planned_probes, record_plan.data.date_from, 1);";
            PrintForm2B.Listeners.Click.Handler = "var record_survey = #{SurveyListGP}.getView().getSelectionModel().getSelection()[0]; var record_plan = #{PlansGP}.getView().getSelectionModel().getSelection()[0];" +
                                                 "App.direct.PrintForm(record_survey.data.id_organization, record_survey.data.planned_area, record_plan.data.date_from, 2);";
            SurveyListTitleOrg.SummaryRenderer.Handler = "return 'Итого по плану'";
            SurveyListPlannedArea.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListPlannedProbes.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListPlannedCuts.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListPlannedFloorPips.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListPlannedExcavation.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListActualArea.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListActualProbes.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListActualCuts.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListActualFloorPips.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";
            SurveyListActualExcavation.SummaryRenderer.Handler = "return ((value === 0) ? '' : value);";

            //Синхронизация данных
            AnalysToPlotB.Listeners.Click.Handler = "App.direct.ShowAnalysToPlotW()";
            AnalysToPlotRegionCB.Listeners.Select.Handler = "App.direct.FillAnalysToPlotOrganization(this.getValue());";
            AnalysToPlotOrganizationCB.Listeners.Select.Handler = "App.direct.FillAnalysToPlotDepartment(this.getValue());";
            AnalysToPlotDepartmentCB.Listeners.Select.Handler = "App.direct.FillAnalysToPlotTour(this.getValue());";
            AnalysToPlotTourCB.Listeners.Select.Handler = "App.direct.FillAnalysToPlotYear(this.getValue(), #{AnalysToPlotDepartmentCB}.getValue());";
            AnalysToPlotYearCB.Listeners.Select.Handler = "App.direct.FillAnalysToPlotData(this.getValue(), #{AnalysToPlotDepartmentCB}.getValue());";
            AnalysToPlotSetNumberPlotB.Listeners.Click.Handler = "var plot_data = #{AnalysToPlotPlotsGP}.getView().getSelectionModel().getSelection()[0]; " +
                                                                 "var analyses_data = #{AnalysToPlotAnalysesGP}.getRowsValues({selectedOnly:true}); " +
                                                                 "App.direct.AnalysToPlotSetNumberPlot(plot_data.data.number_plot, analyses_data);";
            AnalysToPlotAcceptB.Listeners.Click.Handler = "App.direct.AnalysToPlotUpdatePlot(#{AnalysToPlotDepartmentCB}.getValue(), #{AnalysToPlotYearCB}.getValue());";
            AnalysToPlotCancelB.Listeners.Click.Handler = "App.direct.AnalysToPlotCancel();";

            // отчет по анализам
            //ReportAnalysRegionCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.FillReportAnalysOrganization(record.data.id_region);";
            ReportAnalysRegionCB.Listeners.Select.Handler = "App.direct.FillReportAnalysOrganization(this.getValue());";
            ReportAnalysOrganizationCB.Listeners.Select.Handler = "App.direct.FillReportAnalysDepartment(this.getValue());";
            ReportAnalysDepartmentCB.Listeners.Select.Handler = "App.direct.FillReportAnalysTour(this.getValue());";
            ReportAnalysTourCB.Listeners.Select.Handler = "App.direct.FillReportAnalysYear(this.getValue(), #{ReportAnalysDepartmentCB}.getValue());";
            ResetReportAnalysB.Listeners.Click.Handler = "App.direct.ResetReportAnalys();";
            CancelReportAnalysB.Listeners.Click.Handler = "App.direct.CancelReportAnalys();";
            AcceptReportAnalysB.Listeners.Click.Handler = "App.direct.AcceptReportAnalys();";
            AcceptReportAnalysControlB.Listeners.Click.Handler = "App.direct.AcceptReportAnalysControl();";

            // отчет по планам
            ResetReportPlanB.Listeners.Click.Handler = "App.direct.ResetReportPlan();";
            CancelReportPlanB.Listeners.Click.Handler = "App.direct.CancelReportPlan();";
            AcceptReportPlanB.Listeners.Click.Handler = "App.direct.AcceptReportPlan();";
            AcceptReportDriverB.Listeners.Click.Handler = "App.direct.AcceptReportDriver();";

            // отчёт в Москву
            GetRegionsTourForReportB.Listeners.Click.Handler = "App.direct.GetRegionsTourForReport(0, #{RegionsTourForReportGP}.getRowsValues());";
            GetRegionsTourForReportHMB.Listeners.Click.Handler = "App.direct.GetRegionsTourForReport(1, #{RegionsTourForReportGP}.getRowsValues());";
            CancelRegionsTourForReportB.Listeners.Click.Handler = "App.direct.CancelRegionsTourForReportW();";
            CellEditingRegionsTourForReport.Listeners.BeforeEdit.Handler = "var record = #{RegionsTourForReportGP}.getView().getSelectionModel().getSelection()[0]; App.direct.BindRegionsTour(record.data.id_region);";

            GetRegionsYearForReportB.Listeners.Click.Handler = "App.direct.GetRegionsYearForReport(0, #{RegionsYearForReportGP}.getRowsValues());";
            CancelRegionsYearForReportB.Listeners.Click.Handler = "App.direct.CancelRegionsYearForReportW();";
            CellEditingRegionsYearForReport.Listeners.BeforeEdit.Handler = "var record = #{RegionsYearForReportGP}.getView().getSelectionModel().getSelection()[0]; App.direct.BindRegionsYear(record.data.id_region);";

            // общие справочники
            // добавить, удалить, изменить тип почв
            AddSoilB.Listeners.Click.Handler = "var store = #{SoilGP}.store; store.insert(0, {id_type_soil: '0', code_type_soil: '0', title_type_soil : 'New'}); #{SoilGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteSoilColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            SoilS.Listeners.Update.Handler = "App.direct.AddEditSoil(record.data.id_type_soil, record.data.code_type_soil, record.data.title_type_soil);";
            SoilS.Listeners.Remove.Handler = "App.direct.DeleteSoil(record.data.id_type_soil);";
            // добавить, удалить, изменить тип культур
            AddCultureB.Listeners.Click.Handler = "var store = #{CultureGP}.store; store.insert(0, {id_culture: '0', code_culture: '0', title_culture : 'New'}); #{CultureGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteCultureColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            CultureS.Listeners.Update.Handler = "App.direct.AddEditCulture(record.data.id_culture, record.data.code_culture, record.data.title_culture);";
            CultureS.Listeners.Remove.Handler = "App.direct.DeleteCulture(record.data.id_culture);";
            // добавить, удалить, изменить тип севооборота
            AddCropRotationB.Listeners.Click.Handler = "var store = #{CropRotationGP}.store; store.insert(0, {id_crop_rotation: '0', code_crop_rotation: '0', title_crop_rotation : 'New'}); #{CropRotationGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteCropRotationColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            CropRotationS.Listeners.Update.Handler = "App.direct.AddEditCropRotation(record.data.id_crop_rotation, record.data.code_crop_rotation, record.data.title_crop_rotation);";
            CropRotationS.Listeners.Remove.Handler = "App.direct.DeleteCropRotation(record.data.id_crop_rotation);";
            // добавить, удалить, изменить тип с/х угодий
            AddFarmlandB.Listeners.Click.Handler = "var store = #{FarmlandGP}.store; store.insert(0, {id_farmland: '0', code_farmland: '0', title_farmland : 'New'}); #{FarmlandGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteFarmlandColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            FarmlandS.Listeners.Update.Handler = "App.direct.AddEditFarmland(record.data.id_farmland, record.data.code_farmland, record.data.title_farmland);";
            FarmlandS.Listeners.Remove.Handler = "App.direct.DeleteFarmland(record.data.id_farmland);";
            // добавить, удалить, изменить степень эродир-ти
            AddErosionB.Listeners.Click.Handler = "var store = #{ErosionGP}.store; store.insert(0, {id_erosion: '0', code_erosion: '0', title_erosion : 'New'}); #{ErosionGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteErosionColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            ErosionS.Listeners.Update.Handler = "App.direct.AddEditErosion(record.data.id_erosion, record.data.code_erosion, record.data.title_erosion);";
            ErosionS.Listeners.Remove.Handler = "App.direct.DeleteErosion(record.data.id_erosion);";
            // добавить, удалить, изменить мех. состав
            AddGradingB.Listeners.Click.Handler = "var store = #{GradingGP}.store; store.insert(0, {id_grading: '0', code_grading: '0', title_grading : 'New'}); #{GradingGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteGradingColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            GradingS.Listeners.Update.Handler = "App.direct.AddEditGrading(record.data.id_grading, record.data.code_grading, record.data.title_grading);";
            GradingS.Listeners.Remove.Handler = "App.direct.DeleteGrading(record.data.id_grading);";
            // добавить, удалить, изменить уклон
            AddSlopeB.Listeners.Click.Handler = "var store = #{SlopeGP}.store; store.insert(0, {id_slope: '0', code_slope: '0', title_slope : 'New'}); #{SlopeGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteSlopeColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            SlopeS.Listeners.Update.Handler = "App.direct.AddEditSlope(record.data.id_slope, record.data.code_slope, record.data.title_slope);";
            SlopeS.Listeners.Remove.Handler = "App.direct.DeleteSlope(record.data.id_slope);";
            // добавить, удалить, изменить экспозицию
            AddExposureB.Listeners.Click.Handler = "var store = #{ExposureGP}.store; store.insert(0, {id_exposure: '0', code_exposure: '0', title_exposure : 'New'}); #{ExposureGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteExposureColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            ExposureS.Listeners.Update.Handler = "App.direct.AddEditExposure(record.data.id_exposure, record.data.code_exposure, record.data.title_exposure);";
            ExposureS.Listeners.Remove.Handler = "App.direct.DeleteExposure(record.data.id_exposure);";

            // дополнительные справочники
            // добавление, удаление, изменение показателей
            SignificativeGP.Listeners.ViewReady.Handler = "App.direct.SelectSignificative();";
            SignificativeGP.Listeners.Select.Handler = "App.direct.SelectedSignificative(record.data.id_significative);";
            //SignificativeGP.Listeners.CellClick.Handler = "App.direct.BindGroups(record.data.id_significative);";
            AddSignificativeB.Listeners.Click.Handler = "var store = #{SignificativeGP}.store; store.insert(0, {id_significative : '0', title_significative : 'New', unit_significative : '*', name_significative : 'name', min_value : '0', max_value : '0', number_of_digits : '0'}); #{SignificativeGP}.editingPlugin.startEdit(0, 0);";
            DeleteSignificativeColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            SignificativeS.Listeners.Update.Handler = "App.direct.AddEditSignificative(record.data.id_significative, record.data.title_significative, record.data.unit_significative, record.data.name_significative, record.data.min_value, record.data.max_value, record.data.number_of_digits);";
            SignificativeS.Listeners.Remove.Handler = "App.direct.DeleteSignificative(record.data.id_significative);";
            // добавление, удаление, изменение групп
            AddGroupsB.Listeners.Click.Handler = "var store = #{GroupsGP}.store; store.insert(0, {id_group: '0', number_group : '0', title_group : 'New', title_method : 'method', from_group : '0', to_group : '0', coefficient : '0'}); #{GroupsGP}.editingPlugin.startEdit(0, 0);";
            UpdateGroupsBySignificativeB.Listeners.Click.Handler = "var record = #{SignificativeGP}.getView().getSelectionModel().getSelection()[0]; App.direct.UpdateGroupsBySignificative(record.data.id_significative);";
            DeleteGroupColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            GroupsS.Listeners.Update.Handler = "App.direct.AddEditGroup(record.data.id_group, record.data.number_group, record.data.title_group, record.data.title_method, record.data.from_group, record.data.to_group, record.data.coefficient, record.data.color);";
            GroupsS.Listeners.Remove.Handler = "App.direct.DeleteGroup(record.data.id_group);";
            //MethodGroupCB.Listeners.Enable.Handler = "App.direct.LoadMethod();";

            // добавление, удаление, изменение ролей
            RoleGP.Listeners.ViewReady.Handler = "App.direct.SelectRole();";
            RoleGP.Listeners.Select.Handler = "App.direct.SelectedRole(record.data.id_role);";
            AddRoleB.Listeners.Click.Handler = "var store = #{RoleGP}.store; store.insert(0, {id_role : '0'}); #{RoleGP}.editingPlugin.startEdit(0, 0);";
            DeleteRoleColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            RoleS.Listeners.Update.Handler = "App.direct.AddEditRole(record.data.id_role, record.data.title_role, record.data.read_role, record.data.edit_role, record.data.add_role, record.data.delete_role);";
            RoleS.Listeners.Remove.Handler = "App.direct.DeleteRole(record.data.id_role);";
            // добавление, удаление, изменение пользователей
            AddUserB.Listeners.Click.Handler = "var store = #{UserGP}.store; store.insert(0, {id_user : '0'}); #{UserGP}.editingPlugin.startEdit(0, 0);";
            DeleteUserColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            UserS.Listeners.Update.Handler = "App.direct.AddEditUser(record.data.id_user, record.data.surname, record.data.name, record.data.patronymic, record.data.title_division, record.data.title_job_title, record.data.login, record.data.password);";
            UserS.Listeners.Remove.Handler = "App.direct.DeleteUser(record.data.id_user);";
            UserGP.Listeners.Select.Handler = "var record = #{UserGP}.getView().getSelectionModel().getSelection()[0]; if(record.password = '') { App.direct.GetCurrentPassword('0'); } else { App.direct.GetCurrentPassword(record.password); }";
            //PasswordUserTF.Listeners.Show.Handler = "App.direct.ClearPass();";
            UserGP.Listeners.BeforeEdit.Handler = "App.direct.BindJobTitleUser(); App.direct.BindDivisionsUser();";
            /*RoleGP.Listeners.CellClick.Handler = "App.direct.BindUsers(record.data.id_role);";
            AddRoleB.Listeners.Click.Handler = "var store = #{RoleGP}.store; store.insert(0, {title_role : 'New'}); #{RoleGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            AddUserB.Listeners.Click.Handler = "var store = #{UserGP}.store; store.insert(0, {name : 'New'}); #{UserGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";*/

            //добавление, удаление, изменение должностей
            AddJobTitleB.Listeners.Click.Handler = "var store = #{JobTitleGP}.store; store.insert(0, {id_job_title: '0', title_job_title : 'New'}); #{JobTitleGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteJobTitleColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            JobTitleS.Listeners.Update.Handler = "App.direct.AddEditJobTitle(record.data.id_job_title, record.data.title_job_title);";
            JobTitleS.Listeners.Remove.Handler = "App.direct.DeleteJobTitle(record.data.id_job_title);";

            //добавление, удаление, изменение заданий
            AddMissionB.Listeners.Click.Handler = "var store = #{MissionGP}.store; store.insert(0, {id_mission: '0', title_mission : 'New'}); #{MissionGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            DeleteMissionColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            MissionS.Listeners.Update.Handler = "App.direct.AddEditMission(record.data.id_mission, record.data.title_mission);";
            MissionS.Listeners.Remove.Handler = "App.direct.DeleteMission(record.data.id_mission);";

            //добавление, удаление, изменение трекеров
            AddTrackerB.Listeners.Click.Handler = "var store = #{TrackersGP}.store; store.insert(0, {id_gps_tracker: '0', imei : '000000000000000'}); #{TrackersGP}.editingPlugin.startEdit(0, 0);";
            DeleteTrackerColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            TrackersS.Listeners.Update.Handler = "App.direct.AddEditTracker(record.data.id_gps_tracker, record.data.imei);";
            TrackersS.Listeners.Remove.Handler = "App.direct.DeleteTracker(record.data.id_gps_tracker);";

            //добавление, удаление, изменение автомобилей
            //AddEditCar(String id_car, String car_model, String license_plate, String imei, String id_region, String title_organization)
            AddCarB.Listeners.Click.Handler = "var store = #{CarsGP}.store; store.insert(0, {id_car: '0', car_model: 'New Car', license_plate: '', imei : '', id_region: 0, title_organization: ''});" +
                                              "#{CarsGP}.editingPlugin.startEdit(0, 0);";
            DeleteCarColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            CarsS.Listeners.Update.Handler = "App.direct.AddEditCar(record.data.id_car, record.data.car_model, record.data.license_plate, record.data.imei, #{CarsRegionCB}.getValue(), record.data.title_organization);";
            CarsS.Listeners.Remove.Handler = "App.direct.DeleteCar(record.data.id_car);";
            //CarsTerritoryCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.GetRegionsForTerritory(record.data.id_territory);";
            CarsTerritoryCB.Listeners.Select.Handler = "App.direct.GetRegionsForTerritory(#{CarsTerritoryCB}.getValue());";
            CarsGP.Listeners.BeforeEdit.Handler = "App.direct.GetCarOrganizationsForRegion(#{CarsRegionCB}.getValue()); App.direct.BindCarTrackers();";

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // выполнение хранимых процедур (добавление карт, точек)
            ExecuteTeritoryMI.Listeners.Click.Handler = "App.direct.SetTerritoryGeometry();";
            ExecuteRegionsMI.Listeners.Click.Handler = "App.direct.SetRegionsGeometry();";
            ExecuteOrganizationsMI.Listeners.Click.Handler = "App.direct.SetOrganizationsGeometry();";
            ExecuteUpdatePointsMI.Listeners.Click.Handler = "App.direct.UpdateIdPlotForPoints();";
            ExecuteAddSoilPointsMI.Listeners.Click.Handler = "App.direct.SetPointsGeometry();";
            ExecutePlotsMI.Listeners.Click.Handler = "App.direct.SetPlotsGeometry();";
            ExecuteSoilMI.Listeners.Click.Handler = "App.direct.SetSoilGeometry();";
            ExecuteSlopeMI.Listeners.Click.Handler = "App.direct.SetSlopeGeometry();";
            ExecuteExposureMI.Listeners.Click.Handler = "App.direct.SetExposureGeometry();";
            ExecuteErosionMI.Listeners.Click.Handler = "App.direct.SetErosionGeometry();";
            ExecuteAddSoilSamplesMI.Listeners.Click.Handler = "App.direct.Import_Soil_Samples();";
            ExecuteTypingMI.Listeners.Click.Handler = "App.direct.SetTypingGeometry();";
            ExecuteALSZMI.Listeners.Click.Handler = "App.direct.SetALSZGeometry();";
            ExecuteFarmsMI.Listeners.Click.Handler = "App.direct.SetFarmsGeometry();";
            ExecuteLagoonsMI.Listeners.Click.Handler = "App.direct.SetLagoonsGeometry();";
            // настройки
            SettingsB.Listeners.Click.Handler = "#{SettingsW}.show(this);";
            // информация
            HelpB.Listeners.Click.Handler = "#{InformationW}.show(this);";
            //копирование / перемещение участков
            CopyMovePlotsB.Listeners.Click.Handler = "App.direct.CopyMoveWindow();";
            //ввод/вывод планов-заданий
            PlansB.Listeners.Click.Handler = "App.direct.PlansWindows();";
            // добавление цикла
            AddTourB.Listeners.Click.Handler = "var store = #{ToursGP}.store; store.insert(0, {tour : ''}); #{ToursGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            // удаление цикла
            //DeleteTourColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";
            // добаление года обследования
            AddYearB.Listeners.Click.Handler = "var store = #{YearsGP}.store; store.insert(0, {year : ''}); #{YearsGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            // удаление года обследования
            //DeleteYearColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";

            // добавление телефона организации
            AddPhoneB.Listeners.Click.Handler = "var store = #{PhonesGP}.store; var count_phones = ((#{PhonesGP}.getStore().getCount()*(-1)) - 1); store.insert(0, {id_phone : count_phones, phone : '+7'}); #{PhonesGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            // удаление телефона организации
            DeletePhoneColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";

            // добавление старого названия организации
            AddOldOrganizationB.Listeners.Click.Handler = "var store = #{OldOrganizationGP}.store; var count_old_titles = ((#{OldOrganizationGP}.getStore().getCount()*(-1)) - 1); store.insert(0, {id_old_title_organization : count_old_titles, old_title_organization : 'New'}); #{OldOrganizationGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            // удаление старого названия организации
            DeleteOldOrganizationColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";

            //добавление нового образца (элементарного участка)
            AddEPlotB.Listeners.Click.Handler = "var store = #{EditEPlotGP}.store; var count_eplots = ((#{EditEPlotGP}.getStore().getCount()*(-1)) - 1); store.insert(0, {id_elementary_plot : count_eplots,"
                                              + " number_elementary_plot : '', p2o5: '', k2o: '', ph_s: '', ph_w: '', hydrolytic_acid: '', code_type_soil: '', code_grading: '', code_erosion: '',"
                                              + " code_slope: '', code_exposure: '', n: '', no3: '', no2: '', humus: '', absorbance_capacity: '', total_absorbed_bases: '', base_saturation: '',"
                                              + " id_priority_calcification: '', s: '', ca: '', mn: '', mo: '', b: '', cu: '', mg: '', zn: '', na: '', co: '', al: '', fe: '', cu_hm: '',"
                                              + "zn_hm: '', cd_hm: '', pb_hm: '', ni_hm: '', hg_hm: '', mg_hm: '', cr_hm: '', fe_hm: '', f_hm: '', as_hm: '', cs137: '', sr90: ''});"
                                              + " #{EditEPlotGP}.editingPlugin.startEditByPosition({row: 0, column: 1});";
                                              //+ " #{EditEPlotGP}.editingPlugin.startEdit(store.getAt(0), #{EditEPlotGP}.columns[1]);";
            /*AddEPlotB.Listeners.Activate.Handler = "var store = #{EditEPlotGP}.store; var count_eplots = ((#{EditEPlotGP}.getStore().getCount()*(-1)) - 1); store.insert(0, {id_elementary_plot : count_eplots,"
                                              + " number_elementary_plot : '0', p2o5: '0', k2o: '0', ph_s: '0', ph_w: '0', hydrolytic_acid: '0', code_type_soil: '0', code_grading: '0', code_erosion: '0',"
                                              + " code_slope: '0', code_exposure: '0', n: '0', no3: '0', no2: '0', humus: '0', absorbance_capacity: '0', total_absorbed_bases: '0', base_saturation: '0',"
                                              + " id_priority_calcification: '0', s: '0', ca: '0', mn: '0', mo: '0', b: '0', cu: '0', mg: '0', zn: '0', na: '0', co: '0', al: '0', fe: '0', cu_hm: '0',"
                                              + "zn_hm: '0', cd_hm: '0', pb_hm: '0', ni_hm: '0', hg_hm: '0', mg_hm: '0', cr_hm: '0', fe_hm: '0', f_hm: '0', as_hm: '0', cs137: '0', sr90: '0'});"
                                              + " #{EditEPlotGP}.editingPlugin.startEditByPosition({row: 0, column: 1});";*/
            AddEPlotB.Listeners.Activate.Handler = "var store = #{EditEPlotGP}.store; var count_eplots = ((#{EditEPlotGP}.getStore().getCount()*(-1)) - 1); store.insert(0, {id_elementary_plot : count_eplots,"
                                              + " number_elementary_plot : '', p2o5: '', k2o: '', ph_s: '', ph_w: '', hydrolytic_acid: '', code_type_soil: '', code_grading: '', code_erosion: '',"
                                              + " code_slope: '', code_exposure: '', n: '', no3: '', no2: '', humus: '', absorbance_capacity: '', total_absorbed_bases: '', base_saturation: '',"
                                              + " id_priority_calcification: '', s: '', ca: '', mn: '', mo: '', b: '', cu: '', mg: '', zn: '', na: '', co: '', al: '', fe: '', cu_hm: '',"
                                              + "zn_hm: '', cd_hm: '', pb_hm: '', ni_hm: '', hg_hm: '', mg_hm: '', cr_hm: '', fe_hm: '', f_hm: '', as_hm: '', cs137: '', sr90: ''});"
                                              + " #{EditEPlotGP}.editingPlugin.startEditByPosition({row: 0, column: 1});";
            //AddEPlotB.Listeners.Focus.Handler = "App.direct.AddEPlotBFocus();";
            AddEPlotB.Listeners.Blur.Handler = "App.direct.AddEPlotBBlur();";
            //AddEPlotB.Listeners.MouseOver.Handler = "App.direct.AddEPlotBRedBorder();";
            //AddEPlotB.Listeners.MouseOut.Handler = "App.direct.AddEPlotBBlur();";
            //EditEPlotS.Listeners.Add.Handler = "App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";
            //EditEPlotS.Listeners.Remove.Handler = "App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";
            //EditEPlotS.Listeners.Update.Handler = "App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";
            EditEPlotGP.Listeners.Edit.Handler = "App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";
            //EditEPlotS.Listeners.Remove.Handler = "App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";

            // удаление образца
            DeleteEPlotColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex); App.direct.GetAvgForPlot(#{EditEPlotGP}.getRowsValues(), #{SignificativeGP}.getRowsValues());";

            // добавление строки в таблицу координат
            AddCoordinatesB.Listeners.Click.Handler = "var store = #{CoordinatesGP}.store; store.insert(0, {id_coordinates : 'New'}); #{CoordinatesGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            // удаление строки в таблице координат
            DeleteDotColumn.Listeners.Command.Handler = "this.up('gridpanel').store.removeAt(recordIndex);";

            PhonesS.Listeners.Update.Handler = "App.direct.AddEditDeletePhone(record.data.id_phone, record.data.phone, 1);";
            PhonesS.Listeners.Remove.Handler = "App.direct.AddEditDeletePhone(record.data.id_phone, record.data.phone, 0);";

            OldOrganizationS.Listeners.Update.Handler = "App.direct.AddEditDeleteOldTitle(record.data.id_old_title_organization, record.data.old_title_organization, 1);";
            OldOrganizationS.Listeners.Remove.Handler = "App.direct.AddEditDeleteOldTitle(record.data.id_old_title_organization, record.data.old_title_organization, 0);";

            /*EditEPlotS.Listeners.Update.Handler = "App.direct.AddEditDeleteEplot(record.data.id_elementary_plot, record.data.number_elementary_plot, record.data.p2o5, record.data.k2o,"
                                                + " record.data.ph_s, record.data.ph_w, record.data.hydrolytic_acid, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion,"
                                                + " record.data.code_slope, record.data.code_exposure, record.data.n, record.data.no3, record.data.no2, record.data.humus, record.data.total_absorbed_bases,"
                                                + " record.data.s, record.data.ca, record.data.mn, record.data.mo, record.data.b, record.data.cu, record.data.mg,"
                                                + " record.data.zn, record.data.na, record.data.co, record.data.al, record.data.fe, record.data.cu_hm, record.data.zn_hm, record.data.cd_hm, record.data.pb_hm,"
                                                + " record.data.ni_hm, record.data.hg_hm, record.data.mg_hm, record.data.cr_hm, record.data.fe_hm, record.data.f_hm, record.data.as_hm, record.data.cs137, record.data.sr90, 1);";*/
            /*EditEPlotS.Listeners.Remove.Handler = "App.direct.AddEditDeleteEplot(record.data.id_elementary_plot, record.data.number_elementary_plot, record.data.p2o5, record.data.k2o,"
                                                + " record.data.ph_s, record.data.ph_w, record.data.hydrolytic_acid, record.data.code_type_soil, record.data.code_grading, record.data.code_erosion,"
                                                + " record.data.code_slope, record.data.code_exposure, record.data.n, record.data.no3, record.data.no2, record.data.humus, record.data.total_absorbed_bases,"
                                                + " record.data.s, record.data.ca, record.data.mn, record.data.mo, record.data.b, record.data.cu, record.data.mg,"
                                                + " record.data.zn, record.data.na, record.data.co, record.data.al, record.data.fe, record.data.cu_hm, record.data.zn_hm, record.data.cd_hm, record.data.pb_hm,"
                                                + " record.data.ni_hm, record.data.hg_hm, record.data.mg_hm, record.data.cr_hm, record.data.fe_hm, record.data.f_hm, record.data.as_hm, record.data.cs137, record.data.sr90, 0);";*/
            EditEPlotS.Listeners.Remove.Handler = "App.direct.DeleteEplot(record.data.id_elementary_plot);";
            // действия кнопок окна настроек
            AcceptSettingsB.Listeners.Click.Handler = "#{SettingsW}.close(); App.direct.AcceptSettings()";
            CancelSettingsB.Listeners.Click.Handler = "#{SettingsW}.close(); App.direct.CancelSettings()";
            
            // подтверждение добавления или редактирования записи
            AcceptAddRegionB.Listeners.Click.Handler = "App.direct.AddRegion(#{CodeRegionTF}.getValue(), #{TitleRegionTF}.getValue(), #{OKATORegionTF}.getValue()); #{AddEditRegionW}.close();";
            AcceptEditRegionB.Listeners.Click.Handler = "App.direct.EditRegion(#{IdRegionTF}.getValue(), #{CodeRegionTF}.getValue(), #{TitleRegionTF}.getValue(), #{OKATORegionTF}.getValue()); #{AddEditRegionW}.close();";
            AcceptAddOrganizationB.Listeners.Click.Handler = "App.direct.AddOrganization(#{TitleOrganizationTF}.getValue(), #{FullTitleOrganizationTF}.getValue(), #{LeaderTF}.getValue(), #{BasisDocumentTF}.getValue(), #{ChiefAgronomistTF}.getValue(), #{LegalAddressTF}.getValue(), #{MailingAddressTF}.getValue(), #{EMailOrganizationTF}.getValue(), #{OKATOOrganizationTF}.getValue(), #{OKTMOOrganizationTF}.getValue(), #{INNOrganizationTF}.getValue(), #{KPPOrganizationTF}.getValue(), #{OGRNOrganizationTF}.getValue(), #{OKVEDOrganizationTF}.getValue(), #{OKPOOrganizationTF}.getValue(), #{PayAccountTF}.getValue(), #{FullBankNameTF}.getValue(), #{BIKTF}.getValue(), #{BankCorrespondingAccountTF}.getValue()); #{AddEditOrganizationW}.close();"; 
            //EditOrganization(String id_organization, String title_org, String OKATO_org, String INN_org, String leader_org, String chief_agronomist)
            AcceptEditOrganizationB.Listeners.Click.Handler = "App.direct.EditOrganization(#{IdOrganizationTF}.getValue(), #{TitleOrganizationTF}.getValue(), #{FullTitleOrganizationTF}.getValue(), #{LeaderTF}.getValue(), #{BasisDocumentTF}.getValue(), #{ChiefAgronomistTF}.getValue(), #{LegalAddressTF}.getValue(), #{MailingAddressTF}.getValue(), #{EMailOrganizationTF}.getValue(), #{OKATOOrganizationTF}.getValue(), #{OKTMOOrganizationTF}.getValue(), #{INNOrganizationTF}.getValue(), #{KPPOrganizationTF}.getValue(), #{OGRNOrganizationTF}.getValue(), #{OKVEDOrganizationTF}.getValue(), #{OKPOOrganizationTF}.getValue(), #{PayAccountTF}.getValue(), #{FullBankNameTF}.getValue(), #{BIKTF}.getValue(), #{BankCorrespondingAccountTF}.getValue()); #{AddEditOrganizationW}.close();";
            AcceptAddDepartmentB.Listeners.Click.Handler = "App.direct.AddDepartment(#{TitleDepartmentTF}.getValue());";
            AcceptEditDepartmentB.Listeners.Click.Handler = "App.direct.EditDepartment(#{IdDepartmentTF}.getValue(), #{TitleDepartmentTF}.getValue()); #{AddEditDepartmentW}.close();";
            // кнопки на отмену редактирования 
            CancelAddEditRegionB.Listeners.Click.Handler = "#{AddEditRegionW}.close();";
            AddEditRegionW.Listeners.Close.Handler = "App.direct.CloseAddEditRegion(#{IdRegionTF}.getValue());";
            CancelAddEditOrganizationB.Listeners.Click.Handler = "#{AddEditOrganizationW}.close();";
            AddEditOrganizationW.Listeners.Close.Handler = "App.direct.CloseAddEditOrganization(#{IdOrganizationTF}.getValue());";
            CancelAddEditDepartmentB.Listeners.Click.Handler = "#{AddEditDepartmentW}.close();";
            AddEditDepartmentW.Listeners.Close.Handler = "App.direct.CloseAddEditDepartment(#{IdDepartmentTF}.getValue());";

            // подтверждение добавления или редактирования участка
            AcceptAddPlotB.Listeners.Click.Handler = "App.direct.AddPlot(#{NumberPlotTF}.getValue(), #{EditAreaTF}.getValue(), #{YearTF}.getValue(), #{TourTF}.getValue(), #{EditSlopeTF}.getValue(), #{EditExposureTF}.getValue(), #{EditSoilTF}.getValue(), #{EditGradingTF}.getValue(), #{EditErosionTF}.getValue(), #{EditFarmlandTF}.getValue(), #{EditCultureTF}.getValue(), #{EditOldCultureTF}.getValue(), #{EditTypeCropRotationTF}.getValue(), #{EditNumberCropRotationTF}.getValue(), #{EditFieldTF}.getValue(), #{NTF}.getValue(), #{NO3TF}.getValue(), #{NO2TF}.getValue(), #{P2O5TF}.getValue(), #{K2OTF}.getValue(), #{PhSTF}.getValue(), #{PhWTF}.getValue(), #{STF}.getValue(), #{HumusTF}.getValue(), #{HydrAcidTF}.getValue(), #{TotalAbsorbedBaseTF}.getValue(), #{CaTF}.getValue(), #{MnTF}.getValue(), #{MoTF}.getValue(), #{BTF}.getValue(), #{CuTF}.getValue(), #{MgTF}.getValue(), #{ZnTF}.getValue(), #{NaTF}.getValue(), #{CoTF}.getValue(), #{AlTF}.getValue(), #{FeTF}.getValue(), #{CuhmTF}.getValue(), #{ZnhmTF}.getValue(), #{CdhmTF}.getValue(), #{PbhmTF}.getValue(), #{NihmTF}.getValue(), #{HghmTF}.getValue(), #{MghmTF}.getValue(), #{CrhmTF}.getValue(), #{FehmTF}.getValue(), #{FhmTF}.getValue(), #{AshmTF}.getValue(), #{Cs137TF}.getValue(), #{Sr90TF}.getValue(), #{EditEPlotGP}.getRowsValues(), #{DateSurvey}.getValue(), #{DryResidueTF}.getValue());";
            //id_user ДОБАВИТЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            AcceptEditPlotB.Listeners.Click.Handler = "App.direct.EditPlot(#{IdPlotTF}.getValue(), #{NumberPlotTF}.getValue(), #{EditAreaTF}.getValue(), #{YearTF}.getValue(), #{TourTF}.getValue(), #{EditSlopeTF}.getValue(), #{EditExposureTF}.getValue(), #{EditSoilTF}.getValue(), #{EditGradingTF}.getValue(), #{EditErosionTF}.getValue(), #{EditFarmlandTF}.getValue(), #{EditCultureTF}.getValue(), #{EditOldCultureTF}.getValue(), #{EditTypeCropRotationTF}.getValue(), #{EditNumberCropRotationTF}.getValue(), #{EditFieldTF}.getValue(), #{NTF}.getValue(), #{NO3TF}.getValue(), #{NO2TF}.getValue(), #{P2O5TF}.getValue(), #{K2OTF}.getValue(), #{PhSTF}.getValue(), #{PhWTF}.getValue(), #{STF}.getValue(), #{HumusTF}.getValue(), #{HydrAcidTF}.getValue(), #{TotalAbsorbedBaseTF}.getValue(), #{CaTF}.getValue(), #{MnTF}.getValue(), #{MoTF}.getValue(), #{BTF}.getValue(), #{CuTF}.getValue(), #{MgTF}.getValue(), #{ZnTF}.getValue(), #{NaTF}.getValue(), #{CoTF}.getValue(), #{AlTF}.getValue(), #{FeTF}.getValue(), #{CuhmTF}.getValue(), #{ZnhmTF}.getValue(), #{CdhmTF}.getValue(), #{PbhmTF}.getValue(), #{NihmTF}.getValue(), #{HghmTF}.getValue(), #{MghmTF}.getValue(), #{CrhmTF}.getValue(), #{FehmTF}.getValue(), #{FhmTF}.getValue(), #{AshmTF}.getValue(), #{Cs137TF}.getValue(), #{Sr90TF}.getValue(), #{EditEPlotGP}.getRowsValues(), #{DateSurvey}.getValue(), #{DryResidueTF}.getValue());";
            //                                                             String id_plot,         String number_plot,         String area,              String year,          String tour,          String code_slope,         String code_exposure,         String code_type_soil,    String code_grading,         String code_erosion,         String code_farmland,         String code_culture,         String code_old_culture,        String code_crop_rotation,            String number_crop_rotation,            String number_field,       String n,          String no3,          String no2,          String p2o5,          String k2o,          String ph_s,         String ph_w,         String s,          String humus,          String hydrolytic_acid,   String tab,                        String ca,          String mn,          String mo,          String b,          String cu,          String mg,          String zn,          String na,          String co,          String al,          String fe,          String cu_hm,         String zn_hm,         String cd_hm,         String pb_hm,         String ni_hm,         String hg_hm,         String mg_hm,         String cr_hm,         String fe_hm,         String f_hm,         String as_hm,         String cs137,          String sr90
            // отмена редактирования участка
            CancelEditPlotB.Listeners.Click.Handler = "#{EditPlotW}.close(); App.direct.CancelSave();";
            EditPlotW.Listeners.Close.Handler = "App.direct.CloseEditPlot(#{IdPlotTF}.getValue());";

            // сохранение редактирования координат
            SaveCoordinatesB.Listeners.Click.Handler = "#{CoordinatesW}.close(); App.direct.SaveChanges();";
            // отмена сохранения редактирования координат
            CancelCoordinatesB.Listeners.Click.Handler = "#{CoordinatesW}.close(); App.direct.CancelSave();";

            // справочники
            // добавление, удаление элементов
            SignificativeGP.Listeners.CellClick.Handler = "App.direct.BindGroups(record.data.id_significative);";
            AddSignificativeB.Listeners.Click.Handler = "var store = #{SignificativeGP}.store; store.insert(0, {name : 'New'}); #{SignificativeGP}.editingPlugin.startEditByPosition({row: 0, column: 0});";
            //DeleteSignificativeB.Listeners.Click.Handler = "var grid = #{SignificativeGP}, sm = grid.getSelectionModel(); grid.editingPlugin.cancelEdit(); grid.store.remove(sm.getSelection()); if (grid.store.getCount() > 0) { sm.select(0); };";
            //AddGroupB.Listeners.Click.Handler = "var store = #{GroupsGP}.store; store.insert(0, {title : 'New'}); #{GroupsGP}.editingPlugin.startEdit(store.getAt(0), #{GroupsGP}.columns[0]);";
            //DeleteGroupB.Listeners.Click.Handler = "var grid = #{GroupsGP}, sm = grid.getSelectionModel(); grid.editingPlugin.cancelEdit(); grid.store.remove(sm.getSelection()); if (grid.store.getCount() > 0) { sm.select(0); };";

            EditdbDepartmentCB.Listeners.Select.Handler = "var record = this.findRecord(this.valueField, this.getValue()); App.direct.EditdbDepartmentCBValueChange(record.data.id_department, record.data.code_department);";

            ToursGP.Listeners.CellClick.Handler = "App.direct.FillYear(record.data.tour); App.direct.StatisticsOrganization(record.data.tour);";
            ToursGP.Listeners.Edit.Handler = "App.direct.FillYear(record.data.tour); App.direct.StatisticsOrganization(record.data.tour);";
            ToursGP.Listeners.Select.Handler = "App.direct.FillYear(record.data.tour); App.direct.StatisticsOrganization(record.data.tour);";
            YearsGP.Listeners.CellClick.Handler = "App.direct.FillPlot(record.data.year);";
            YearsGP.Listeners.Edit.Handler = "App.direct.FillPlot(record.data.year);";
            YearsGP.Listeners.Select.Handler = "App.direct.FillPlot(record.data.year);";

            ToursTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.FocusOnAddYear(record.data.tour);};";
            YearsTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.FocusOnAddPlot(record.data.year);};";

            StatisticsRegionB.Listeners.Click.Handler = "App.direct.ShowStatisticsAreaRegionW();";
            
            try
            { //обход ошибки, если поле не найдено
                PlotsM.Fields.Get("id_farmlang").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_farmlang").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_farmlang").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_old_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_old_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_old_culture").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_crop_rotation").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_crop_rotation").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_crop_rotation").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("number_crop_rotation").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("number_field").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_erosion").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_erosion").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_erosion").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("slope").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("code_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_priority_calcification").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("number_pc_group").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_pc_group").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("id_method").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                PlotsM.Fields.Get("title_method").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";

                EPlotsM.Fields.Get("id_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("code_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_type_soil").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("id_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("code_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_grading").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("id_erosion").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("code_erosion").Convert.Handler = "if(value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_erosion").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("slope").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("id_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("code_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_exposure").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("id_priority_calcification").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("number_pc_group").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_pc_group").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("id_method").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
                EPlotsM.Fields.Get("title_method").Convert.Handler = "if(value == '0' || value == null){return value = ''} else {return value;}";
            }
            catch (NullReferenceException exc) { }
            MicroElemCB.Listeners.Change.Handler = "App.direct.MicroElemShowHidden()";
            HeavyMetalCB.Listeners.Change.Handler = "App.direct.HeavyMetalShowHidden()";
            RadiologyCB.Listeners.Change.Handler = "App.direct.RadiologyShowHidden()";

            PlotsGP.Listeners.Select.Handler = "App.direct.FillEPlotAndSlope(record.data.id_plot)";

            EditFarmlandTF.Listeners.Change.Handler = "App.direct.ShowTypeFarmland();";
            EditFarmlandTF.Listeners.Blur.Handler = "App.direct.BlurTypeFarmland();";
            EditCultureTF.Listeners.Change.Handler = "App.direct.ShowCulture();";
            EditCultureTF.Listeners.Blur.Handler = "App.direct.BlurCulture();";
            EditOldCultureTF.Listeners.Change.Handler = "App.direct.ShowOldCulture();";
            EditOldCultureTF.Listeners.Blur.Handler = "App.direct.BlurOldCulture();";
            EditTypeCropRotationTF.Listeners.Change.Handler = "App.direct.ShowTypeCropRotation();";
            EditTypeCropRotationTF.Listeners.Blur.Handler = "App.direct.BlurTypeCropRotation();";
            EditSoilTF.Listeners.Change.Handler = "App.direct.ShowTypeSoil();";
            EditSoilTF.Listeners.Blur.Handler = "App.direct.BlurTypeSoil();";
            EditGradingTF.Listeners.Change.Handler = "App.direct.ShowTypeGrading();";
            EditGradingTF.Listeners.Blur.Handler = "App.direct.BlurTypeGrading();";
            EditErosionTF.Listeners.Change.Handler = "App.direct.ShowTypeErosion();";
            EditErosionTF.Listeners.Blur.Handler = "App.direct.BlurTypeErosion();";
            EditSlopeTF.Listeners.Change.Handler = "App.direct.ShowSlope();";
            EditSlopeTF.Listeners.Blur.Handler = "App.direct.BlurSlope();";
            EditExposureTF.Listeners.Change.Handler = "App.direct.ShowExposure();";
            EditExposureTF.Listeners.Blur.Handler = "App.direct.BlurExposure();";
            PhSTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.ShowMethod(#{EditEPlotGP}.getStore().getCount());};";
            TotalAbsorbedBaseTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.ShowACBS_TAB();};";
            //HydrAcidTF.Listeners.KeyUp.Handler = "App.direct.ShowACBS_HA();";
            HydrAcidTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.ShowACBS_HA(#{EditEPlotGP}.getStore().getCount());};";
            //HydrAcidTF.Listeners.Change.Handler = "App.direct.MaskChange();";

            EditNumberCropRotationTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditNumberCropRotation();};";
            EditFieldTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditField();};";
            NumberPlotTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditNumberPlot();};";
            EditAreaTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditArea();};";
            NTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditN();};";
            NO3TF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditNO3();};";
            NO2TF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditNO2();};";
            HumusTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditHumus();};";
            P2O5TF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditP2O5(#{EditEPlotGP}.getStore().getCount());};";
            K2OTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditK2O(#{EditEPlotGP}.getStore().getCount());};";
            PhWTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditPhW();};";
            STF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditS();};";
            MnTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditMn();};";
            FeTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditFe();};";
            CuTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCu();};";
            ZnTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditZn();};";
            CoTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCo();};";
            AlTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditAl();};";
            CaTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCa();};";
            MoTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditMo();};";
            BTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditB();};";
            MgTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditMg();};";
            NaTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){#{EditExposureTF}.focus();App.direct.EditNa();};";
            CuhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCutm();};";
            ZnhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditZntm();};";
            CdhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCdtm();};";
            PbhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditPbtm();};";
            NihmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditNitm();};";
            HghmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditHgtm();};";
            MghmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditMgtm();};";
            CrhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCrtm();};";
            FehmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditFetm();};";
            FhmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditFtm();};";
            AshmTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditAstm();};";
            Cs137TF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditCs137();};";
            Sr90TF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditSr90();};";
            DryResidueTF.Listeners.KeyPress.Handler = "if(e.keyCode==9 || e.keyCode==13){App.direct.EditDryResidue();};";

            //нажатия клавиш при редактировании элементарных участков
            //HydrolyticAcidEPlotTF.Listeners.Blur.Handler = "App.direct.EditHAEPlot(#{EditEPlotGP}.getSelectionModel().getSelections().RowIndex, this.value);";
            HydrolyticAcidEPlotTF.Listeners.Blur.Handler = "App.direct.EditHAEPlot(this.value);";
            PhSEPlotTF.Listeners.Blur.Handler = "App.direct.EditPhSEPlot(this.value);";
            P2O5EPlotTF.Listeners.Blur.Handler = "App.direct.EditP2O5EPlot(this.value);";
            K2OEPlotTF.Listeners.Blur.Handler = "App.direct.EditK2OEPlot(this.value);";

            //NumberEplotTF.Listeners.KeyUp.Handler = "if(e.keyCode==13){#{EditEPlotGP}.editingPlugin.completeEdit(); App.direct.AddEPlotBFocus();};";
            NumberEplotTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.AddEPlotBFocus(); this.value = '';};";
            HydrolyticAcidEPlotTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.AddEPlotBFocus(); this.value = '';};";
            PhSEPlotTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.AddEPlotBFocus(); this.value = '';};";
            P2O5EPlotTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.AddEPlotBFocus(); this.value = '';};";
            K2OEPlotTF.Listeners.KeyDown.Handler = "if(e.keyCode==13){App.direct.AddEPlotBFocus(); this.value = '';};";

            //ReportsM1Item1.Listeners.Click.Handler = "window.open('RegionReport.aspx','_blank')";
            //PaspVedB.Listeners.Click.Handler = "if(#{PlotsGP}.getStore().getCount() > 0) {var record_year = #{YearsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.PasportReport(record_year.data.year); window.open('ReportPasport.aspx','_blank');}";
            PaspVedB.Listeners.Click.Handler = "if(#{PlotsGP}.getStore().getCount() > 0) {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0]; App.direct.PasportReport(record_tour.data.tour);}";
            OcherIzvB.Listeners.Click.Handler = "if(#{PlotsGP}.getStore().getCount() > 0) {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0]; App.direct.PriorityCalcificationReport(record_tour.data.tour);}";
            GroupByTFB.Listeners.Click.Handler = "if(#{PlotsGP}.getStore().getCount() > 0) {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0]; App.direct.GroupsByTFReport(record_tour.data.tour);}";
            GroupByTFHMB.Listeners.Click.Handler = "if(#{PlotsGP}.getStore().getCount() > 0) {var record_tour = #{ToursGP}.getView().getSelectionModel().getSelection()[0]; App.direct.GroupsByTFHMReport(record_tour.data.tour);}";
            //отчёты
            ReportsM1Item1.Listeners.Click.Handler = "App.direct.SelectReportTours(0, '1_1');";
            ReportsM1Item2.Listeners.Click.Handler = "App.direct.SelectReportTours(0, '1_2');";
            ReportsM1Item3.Listeners.Click.Handler = "App.direct.SelectReportTours(0, '1_3');";
            ReportsM1Item4.Listeners.Click.Handler = "App.direct.SelectReportTours(0, '1_4');";
            ReportsM1Item5.Listeners.Click.Handler = "App.direct.SelectReportTours(0, '1_5');";
            ReportsM2Item1.Listeners.Click.Handler = "App.direct.SelectReportTours(1, '2_1');";
            ReportsM2Item2.Listeners.Click.Handler = "App.direct.SelectReportTours(1, '2_2');";
            ReportsM2Item3.Listeners.Click.Handler = "App.direct.SelectReportTours(1, '2_3');";
            ReportsM2Item4.Listeners.Click.Handler = "App.direct.SelectReportTours(1, '2_4');";
            ReportsM2Item5.Listeners.Click.Handler = "App.direct.SelectReportTours(1, '2_5');";
            //отчёт по району
            ReportsM3Item1.Listeners.Click.Handler = "App.direct.SelectReportTours(2, '3_1');";
            ReportsM3Item2.Listeners.Click.Handler = "App.direct.SelectReportTours(2, '3_2');";
            ReportsM3Item3.Listeners.Click.Handler = "App.direct.SelectReportTours(2, '3_3');";
            ReportToursSB.Listeners.Change.Handler = "App.direct.SelectTours(this.value);";
            //отчёт по области
            ReportsM4Item1.Listeners.Click.Handler = "App.direct.SelectReportTours(3, '4_1');";
            ReportsM4Item2.Listeners.Click.Handler = "App.direct.SelectReportTours(3, '4_2');";
            /*ReportsM1Item5.Listeners.Click.Handler = "window.open('ReportSignificativeBySlopeDepartment.aspx', '_blank');";
            ReportsM2Item3.Listeners.Click.Handler = "window.open('ReportSignificativeBySlopeOrganization.aspx', '_blank');";*/
            ReportToursAcceptB.Listeners.Click.Handler = "App.direct.AcceptReportToursW();";
            ReportToursCancelB.Listeners.Click.Handler = "App.direct.CloseReportToursW();";
            //отчет по анализам
            //ReportsM5Item1.Listeners.Click.Handler = "var recordreg = #{RegionGP}.getView().getSelectionModel().getSelection()[0]; var recordorg = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; var recorddep = #{DepartmentGP}.getView().getSelectionModel().getSelection()[0]; "
            //                                       + "App.direct.ShowReportAnalysW(recordreg.data.id_region, recordorg.data.id_organization, recordreg.data.title_region, recordorg.data.title_organization, recorddep.data.id_department, recorddep.data.title_department);";
            ReportsM5Item1.Listeners.Click.Handler = "App.direct.ShowReportAnalysW();";
            // отчет по планам-заданиям
            ReportsM6Item1.Listeners.Click.Handler = "App.direct.ShowReportPlansW();";

            // отчёт в москву
            ReportsM7Item1.Listeners.Click.Handler = "App.direct.ShowRegionsTourForReportW(0);";
            ReportsM7Item2.Listeners.Click.Handler = "App.direct.ShowRegionsTourForReportW(1);";
            ReportsM7Item3.Listeners.Click.Handler = "App.direct.ShowRegionsYearForReportW(0);";

            CopyPlotsB.Listeners.Click.Handler = "App.direct.CopyPlots(#{CopyMovePlotsTF}.getValue(), #{CodeDepartmentFromTF}.getValue(), #{CodeDepartmentToTF}.getValue(), #{TourFromTF}.getValue(), #{TourToTF}.getValue(), #{YearFromTF}.getValue(), #{YearToTF}.getValue())";
            MovePlotsB.Listeners.Click.Handler = "App.direct.MovePlots(#{CopyMovePlotsTF}.getValue(), #{CodeDepartmentFromTF}.getValue(), #{CodeDepartmentToTF}.getValue(), #{TourFromTF}.getValue(), #{TourToTF}.getValue(), #{YearFromTF}.getValue(), #{YearToTF}.getValue())";
            CancelCopyMovePlotsB.Listeners.Click.Handler = "App.direct.CloseCopyMoveW()";

            MapB.Listeners.Click.Handler = "var record_org = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; var record_about_org = #{AboutOrgGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowMap(record_org.data.id_organization, record_about_org.data.year, record_about_org.data.tour);";
            ShowMapB.Listeners.Click.Handler = "var record_year = #{YearsGP}.getView().getSelectionModel().getSelection()[0]; var record_plot = #{PlotsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowPlotOnMap(record_year.data.year, record_year.data.tour, record_plot.data.code_plot);";
            AboutOrgGP.Listeners.Select.Handler = "App.direct.SelectedRowInAboutOrg(record.data.id_organization, record.data.year, record.data.tour);";

            //установка масок ввода данных
            SetMask();

            //окна для отчётов           
            win_1_1 = new Window()
            {
                ID = "PasportReportW",
                Title = "Паспортная ведомость",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPasport.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_1_2 = new Window()
            {
                ID = "PriorityCalcificationDepartmentW",
                Title = "Нуждаемость в известковании (по отделениям)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPriorityCalcificationDepartment.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_1_3 = new Window()
            {
                ID = "SignificativeBySoilDepartmentW",
                Title = "Группировка почв по типу сельхозугодий (Отделение)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilDepartment.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_1_4 = new Window()
            {
                ID = "SignificativeBySlopeDepartmentW",
                Title = "Группировка групп обеспеченности элементами по уклонам (Хозяйство и отделение)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySlopeDepartment.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_1_5 = new Window()
            {
                ID = "SignificativeBySoilDepartmentAdditionW",
                Title = "Группировка почв по типам сельхозугодий (с дополнительными группами)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilDepartmentAddition.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_2_1 = new Window()
            {
                ID = "SignificativeBySoilOrganizationW",
                Title = "Группировка почв по типу сельхозугодий (Хозяйство)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilOrganization.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_2_2 = new Window()
            {
                ID = "HeavyMetalBySoilOrganizationW",
                Title = "Группировка почв по типу сельхозугодий (ТМ) (Хозяйство)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportHeavyMetalBySoilOrganization.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_2_3 = new Window()
            {
                ID = "SignificativeBySlopeOrganizationW",
                Title = "Группировка групп обеспеченности элементами по уклонам (Хозяйство)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySlopeOrganization.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_2_4 = new Window()
            {
                ID = "PriorityCalcificationW",
                Title = "Нуждаемость в известковании",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPriorityCalcificationOrganization.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_2_5 = new Window()
            {
                ID = "SignificativeBySoilOrganizationAdditionW",
                Title = "Группировка почв по типам сельхозугодий (с дополнительными группами)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilOrganizationAddition.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_3_1 = new Window()
            {
                ID = "SignificativeBySoilRegionW",
                Title = "Группировка почв по типу сельхозугодий (Район)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilRegion.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };
            
            win_3_2 = new Window()
            {
                ID = "HeavyMetalBySoilRegionW",
                Title = "Группировка почв по типу сельхозугодий (ТМ) (Район)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportHeavyMetalBySoilRegion.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_3_3 = new Window()
            {
                ID = "SignificativeBySoilRegionAdditionW",
                Title = "Группировка почв по типам сельхозугодий (с дополнительными группами)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilRegionAddition.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_4_1 = new Window()
            {
                ID = "SignificativeBySoilTerritoryW",
                Title = "Группировка почв по типу сельхозугодий (Область)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilTerritory.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_4_2 = new Window()
            {
                ID = "HeavyMetalBySoilTerritoryW",
                Title = "Группировка почв по типу сельхозугодий (ТМ) (Район)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportHeavyMetalBySoilTerritory.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_ph = new Window()
            {
                ID = "Report_analys_pH",
                Title = "Отчёт по степени кислотности",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_pH.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_ph_control = new Window()
            {
                ID = "Report_analys_control_pH",
                Title = "Контрольный отчёт по степени кислотности",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_control_pH.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_p_k = new Window()
            {
                ID = "Report_analys_p_k",
                Title = "Отчёт по фосфору, калию",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_p_k.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_p_k_control = new Window()
            {
                ID = "Report_analys_control_p_k",
                Title = "Контрольный отчёт по фосфору, калию",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_control_p_k.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_ha = new Window()
            {
                ID = "Report_analys_HA",
                Title = "Отчёт по гидролитической кислотности",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_HA.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_ha_control = new Window()
            {
                ID = "Report_analys_control_HA",
                Title = "Контрольный отчёт по гидролитической кислотности",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_analys_control_HA.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_the_plan = new Window()
            {
                ID = "Report_On_The_Plan",
                Title = "Отчёт по плану",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "Report_On_The_Plan.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_plan = new Window()
            {
                ID = "ReportPlan",
                Title = "Отчёт по планам",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPlan.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_plan_region = new Window()
            {
                ID = "ReportPlanRegion",
                Title = "Отчёт по планам",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPlanRegion.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_plan_worker = new Window()
            {
                ID = "ReportPlanWorker",
                Title = "Отчёт по планам",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPlanWorker.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_plan_mission = new Window()
            {
                ID = "ReportPlanMission",
                Title = "Отчёт по планам",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPlanMission.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_plan_null = new Window()
            {
                ID = "ReportPlanNull",
                Title = "Отчёт по планам",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportPlanNull.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_driver = new Window()
            {
                ID = "ReportDriver",
                Title = "Отчёт по водителям",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportDriver.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_7_1 = new Window()
            {
                ID = "SignificativeBySoilMoscowW",
                Title = "Группировка почв по типу сельхозугодий (в Москву)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilMoscow.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_stickers = new Window()
            {
                ID = "ReportStickers",
                Title = "Этикетки",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportStickers.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_form_1 = new Window()
            {
                ID = "ReportForm1",
                Title = "Форма 1",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportForm1.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_form_2 = new Window()
            {
                ID = "ReportForm2",
                Title = "Форма 2",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportForm2.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_7_3 = new Window()
            {
                ID = "SignificativeBySoilMoscowYearW",
                Title = "Группировка почв по типу сельхозугодий (в Москву по годам)",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportSignificativeBySoilMoscowYear.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win_upload = new Window()
            {
                ID = "UploadW",
                Title = "Загрузка файла",
                Width = Unit.Pixel(400),
                Height = Unit.Pixel(150),
                Modal = false,
                AutoRender = true,
                Collapsible = false,
                Maximizable = false,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "FileUpload.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(win_1_1);
            this.Form.Controls.Add(win_1_2);
            this.Form.Controls.Add(win_1_3);
            this.Form.Controls.Add(win_1_4);
            this.Form.Controls.Add(win_1_5);
            this.Form.Controls.Add(win_2_1);
            this.Form.Controls.Add(win_2_2);
            this.Form.Controls.Add(win_2_3);
            this.Form.Controls.Add(win_2_4);
            this.Form.Controls.Add(win_2_5);
            this.Form.Controls.Add(win_3_1);
            this.Form.Controls.Add(win_3_2);
            this.Form.Controls.Add(win_3_3);
            this.Form.Controls.Add(win_4_1);
            this.Form.Controls.Add(win_4_2);
            this.Form.Controls.Add(win_ph);
            this.Form.Controls.Add(win_ph_control);
            this.Form.Controls.Add(win_p_k);
            this.Form.Controls.Add(win_p_k_control);
            this.Form.Controls.Add(win_ha);
            this.Form.Controls.Add(win_ha_control);
            this.Form.Controls.Add(win_the_plan);
            this.Form.Controls.Add(win_plan);
            this.Form.Controls.Add(win_plan_region);
            this.Form.Controls.Add(win_plan_worker);
            this.Form.Controls.Add(win_plan_mission);
            this.Form.Controls.Add(win_plan_null);
            this.Form.Controls.Add(win_driver);
            this.Form.Controls.Add(win_7_1);
            this.Form.Controls.Add(win_stickers);
            this.Form.Controls.Add(win_form_1);
            this.Form.Controls.Add(win_form_2);
            this.Form.Controls.Add(win_7_3);
            this.Form.Controls.Add(win_upload);

            if (!Request.Browser.Cookies)
            {
                X.Msg.Confirm("Внимание!!!", "В вашем браузере отключены или не поддерживаются cookie.<br />Включите cookie или используйте другой браузер!", new MessageBoxButtonsConfig
                {
                    Cancel = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.RedirectToGoogle();",
                        Text = "Закрыть"
                    }
                }).Show();
            }
        }

        //ограничения на ввод данных и автоматическое написание запятой
        [DirectMethod]
        public String ReplaceValue(String value, String type_str)
        { // 0 - текст, 1 - целые числа, 2 - дробные числа
            Int32 type = Convert.ToInt32(type_str);
            String result = String.Empty;
            if (type >= 1)
            {
                String no_point = value.Replace('.', ',').Trim();
                String[] temp = no_point.Split(',');
                if (temp.Length > 1)
                {
                    if (temp[0] != null && temp[0] != "null" && temp[0] != String.Empty) { result = temp[0] + ","; }
                    else { result = "0,"; }
                    for (int i = 1; i < temp.Length; i++)
                    {
                        if (temp[i] != null && temp[i] != "null" && temp[i] != String.Empty) { result += temp[i]; }
                        else { result += 0; }
                    }
                }
                else if (temp[0] != null && temp[0] != "null" && temp[0] != String.Empty)
                {
                    result = value + ",0";
                }
                else
                {
                    result = "0,0";
                }
            }
            else
            {
                result = value;
            }
            /*if (result.Length >= 2 && result[0] == ',')
            {
                result = "0" + result;
            }
            if (result.Length == 2)
            {
                if (result[1] == ',')
                {
                    result = result[0].ToString();
                }
            }
            try
            {
                if (Convert.ToDouble(result) == 0)
                {
                    result = String.Empty;
                }
            }
            catch (Exception e) { result = String.Empty; }
            if (result != String.Empty)
            {
                if (type == 1)
                {
                    result = Convert.ToInt32(Convert.ToDouble(result)).ToString();
                }
                if (type == 2)
                {
                    result = Convert.ToDouble(result).ToString();
                }
            }*/
            if (type == 1)
            {
                result = Convert.ToInt32(Convert.ToDouble(result)).ToString();
            }
            return result;
        }

        [DirectMethod]
        public void LoadFailed()
        {
            X.Msg.Notify("Ошибка", "Произошла ошибка при загрузке в SWFUpload").Show();
        }

        [DirectMethod]
        public void FileUpload(object sender, FileUploadEventArgs e)
        {
            //~ для корневого каталога сайта
            Random random = new Random();
            String new_file_name = DateTime.Now.ToString(@"yyyy-MM-dd-HH-mm-ss-") + random.Next(1, 999) + Path.GetExtension(e.FileName);
            String y = System.DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            String m = System.DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
            String strPath = HttpContext.Current.Server.MapPath("/Uploads/" + y + "/" + m + "/" + new_file_name);
            String pathMoth = HttpContext.Current.Server.MapPath("/Uploads/" + y + "/" + m);
            String webPath = "/Uploads/" + y + "/" + m + "/" + new_file_name;
            if (!Directory.Exists(pathMoth))
            {
                Directory.CreateDirectory(pathMoth);
            }
            HttpPostedFile file = e.PostedFile;
            file.SaveAs(strPath);
            X.Msg.Notify("Загружен файл", "Имя файла: " + e.FileName + "\n Сохранён как: " + new_file_name).Show();
            ImportData(strPath);
        }

        public void ImportData(String file_name)
        {
            if (connection_try)
            {
                FileStream readF = new FileStream(file_name, System.IO.FileMode.Open);
                StreamReader streamReader = new StreamReader(readF, System.Text.Encoding.Default);
                String ext_file = Path.GetExtension(file_name), stored_proc = String.Empty;
                ListBox readLB = new ListBox();
                Int32 sv_id_column = 0, gridcode_column = 1, sa_column = 2, group_column = 1;
                Boolean is_slope = true;
                SqlCommand import_proc;
                while (!streamReader.EndOfStream)
                {
                    readLB.Items.Add(streamReader.ReadLine());
                }
                streamReader.Close();
                readF.Close();
                //connStr = "Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N";
                connStr = SetConnectionString();
                conn = new SqlConnection(connStr);
                conn.Open();
                String headers = "";
                if (ext_file == ".slope")
                {
                    stored_proc = "Import_Slope";
                    for (int i = 0; i < readLB.Items[0].ToString().Split(';').Length; i++)
                    {
                        if (readLB.Items[0].ToString().Split(';')[i] == "SV_ID") { sv_id_column = i; }
                        if (readLB.Items[0].ToString().Split(';')[i] == "GRIDCODE") { gridcode_column = i; }
                        if (readLB.Items[0].ToString().Split(';')[i] == "SA") { sa_column = i; }
                        headers = headers + " " + readLB.Items[0].ToString().Split(';')[i];
                    }
                    is_slope = true;
                    X.Msg.Notify("Заголовки", headers).Show();
                    for (int i = 1; i < readLB.Items.Count; i++)
                    {
                        import_proc = new SqlCommand(stored_proc, conn);
                        import_proc.CommandType = CommandType.StoredProcedure;
                        import_proc.Parameters.AddWithValue("@sv_id", readLB.Items[i].ToString().Split(';')[sv_id_column].ToString());
                        if (is_slope)
                        {
                            import_proc.Parameters.AddWithValue("@gridcode", readLB.Items[i].ToString().Split(';')[gridcode_column].ToString());
                            import_proc.Parameters.AddWithValue("@sa", readLB.Items[i].ToString().Split(';')[sa_column].ToString());
                        }
                        else
                        {
                            import_proc.Parameters.AddWithValue("@group", readLB.Items[i].ToString().Split(';')[group_column].ToString());
                        }
                        import_proc.ExecuteNonQuery();
                    }
                }

                else if (ext_file == ".exposure")
                {
                    stored_proc = "Import_Exposure";
                    for (int i = 0; i < readLB.Items[0].ToString().Split(';').Length; i++)
                    {
                        if (readLB.Items[0].ToString().Split(';')[i] == "SV_ID") { sv_id_column = i; }
                        if (readLB.Items[0].ToString().Split(';')[i] == "GROUP") { group_column = i; }
                    }
                    is_slope = false;
                    X.Msg.Notify("Заголовки", headers).Show();
                    for (int i = 1; i < readLB.Items.Count; i++)
                    {
                        import_proc = new SqlCommand(stored_proc, conn);
                        import_proc.CommandType = CommandType.StoredProcedure;
                        import_proc.Parameters.AddWithValue("@sv_id", readLB.Items[i].ToString().Split(';')[sv_id_column].ToString());
                        if (is_slope)
                        {
                            import_proc.Parameters.AddWithValue("@gridcode", readLB.Items[i].ToString().Split(';')[gridcode_column].ToString());
                            import_proc.Parameters.AddWithValue("@sa", readLB.Items[i].ToString().Split(';')[sa_column].ToString());
                        }
                        else
                        {
                            import_proc.Parameters.AddWithValue("@group", readLB.Items[i].ToString().Split(';')[group_column].ToString());
                        }
                        import_proc.ExecuteNonQuery();
                    }
                }

                else if (ext_file == ".gpx")
                {
                    stored_proc = "Import_Points";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file_name);
                    foreach (XmlElement element in doc.GetElementsByTagName("wpt"))
                    {
                        DateTime date_point = DateTime.Now;
                        XmlNodeList date_time = element.GetElementsByTagName("time");
                        if (date_time != null)
                        {
                            date_point = DateTime.ParseExact(date_time[0].InnerText, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            date_time = element.GetElementsByTagName("cmt");
                            if (date_time != null)
                            {
                                date_point = DateTime.ParseExact(date_time[0].InnerText, "d-MMM-yy H:m:s", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                date_time = element.GetElementsByTagName("desc");
                                if (date_time != null)
                                {
                                    date_point = DateTime.ParseExact(date_time[0].InnerText, "d-MMM-yy H:m:s", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }

                        Int64 date_point_ticks = date_point.Ticks;
                        XmlNodeList name = element.GetElementsByTagName("name");
                        XmlNodeList altitude = element.GetElementsByTagName("ele");
                        import_proc = new SqlCommand(stored_proc, conn);
                        import_proc.CommandType = CommandType.StoredProcedure;
                        import_proc.Parameters.AddWithValue("@altitude_point", Convert.ToDouble(altitude[0].InnerText.Replace('.', ',')));
                        import_proc.Parameters.AddWithValue("@name_point", name[0].InnerText);
                        import_proc.Parameters.AddWithValue("@longitude", Convert.ToDouble(element.Attributes["lon"].Value.Replace('.', ',')));
                        import_proc.Parameters.AddWithValue("@latitude", Convert.ToDouble(element.Attributes["lat"].Value.Replace('.', ',')));
                        import_proc.Parameters.AddWithValue("@date_time_ticks", date_point_ticks);
                        import_proc.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }

        [DirectMethod]
        public void PasportReport(String tour)
        {
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                if (cookie_report_tours == null)
                {
                    cookie_report_tours = new HttpCookie("Agrochim31_ReportTours");
                    cookie_report_tours["id"] = current_id_department;
                    cookie_report_tours["tour"] = tour;
                    cookie_report_tours.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie_report_tours);
                }
                else
                {
                    cookie_report_tours["id"] = current_id_department;
                    cookie_report_tours["tour"] = tour;
                    Response.Cookies.Set(cookie_report_tours);
                }
                win_1_1.Reload();
                win_1_1.Show();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        //изменение данных в текстовых полях при вводе
        [DirectMethod]
        public void ShowTypeFarmland()
        {
            //String temp = ReplaceValue(EditFarmlandTF.Text, 1.ToString());
            /*String temp = EditFarmlandTF.Text;
            if (EditFarmlandTF.Text != temp) { EditFarmlandTF.Text = temp; }
            else */
            if (EditFarmlandTF.Text != null && EditFarmlandTF.Text != String.Empty && EditFarmlandTF.Text != "null")
            {
                DataTable dt_type_farmland = indexDS.Tables["Type_farmland"];
                if (EditFarmlandTF.Text != String.Empty && dt_type_farmland.Select("code_farmland='" + EditFarmlandTF.Text + "'").Length > 0)
                {
                    FarmlandTF.Text = NullToEmpty(dt_type_farmland.Select("code_farmland='" + EditFarmlandTF.Text + "'")[0]["title_farmland"].ToString());
                }
                else
                {
                    FarmlandTF.Text = String.Empty;
                    /*EditFarmlandTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Типа с/х угодий с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                FarmlandTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurTypeFarmland()
        {
            if (EditFarmlandTF.Text != null && EditFarmlandTF.Text != String.Empty && EditFarmlandTF.Text != "null")
            {
                DataTable dt_type_farmland = indexDS.Tables["Type_farmland"];
                if (EditFarmlandTF.Text != String.Empty && dt_type_farmland.Select("code_farmland='" + EditFarmlandTF.Text + "'").Length <= 0)
                {
                    /*X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Типа с/х угодий с кодом " + EditFarmlandTF.Text + " не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                    X.Msg.Confirm("Предупреждение!!!", "Типа с/х угодий с кодом " + EditFarmlandTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditFarmlandTF}.focus(); #{EditFarmlandTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                    /*EditFarmlandTF.Text = String.Empty;
                    EditFarmlandTF.Focus();
                    EditFarmlandTF.SelectText();*/
                }
            }
        }

        [DirectMethod]
        public void ShowCulture()
        {
            //String temp = ReplaceValue(EditCultureTF.Text, 1.ToString());
            /*String temp = EditCultureTF.Text;
            if (EditCultureTF.Text != temp) { EditCultureTF.Text = temp; }
            else */
            if (EditCultureTF.Text != null && EditCultureTF.Text != String.Empty && EditCultureTF.Text != "null")
            {
                DataTable dt_culture = indexDS.Tables["Culture"];
                if (EditCultureTF.Text != String.Empty && dt_culture.Select("code_culture='" + EditCultureTF.Text + "'").Length > 0)
                {
                    CultureTF.Text = NullToEmpty(dt_culture.Select("code_culture='" + EditCultureTF.Text + "'")[0]["title_culture"].ToString());
                }
                else
                {
                    CultureTF.Text = String.Empty;
                    /*EditCultureTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Культуры с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                CultureTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurCulture()
        {
            if (EditCultureTF.Text != null && EditCultureTF.Text != String.Empty && EditCultureTF.Text != "null")
            {
                DataTable dt_culture = indexDS.Tables["Culture"];
                if (EditCultureTF.Text != String.Empty && dt_culture.Select("code_culture='" + EditCultureTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Культуры с кодом " + EditCultureTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditCultureTF}.focus(); #{EditCultureTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowOldCulture()
        {
            //String temp = ReplaceValue(EditOldCultureTF.Text, 1.ToString());
            /*String temp = EditOldCultureTF.Text;
            if (EditOldCultureTF.Text != temp) { EditOldCultureTF.Text = temp; }
            else */
            if (EditOldCultureTF.Text != null && EditOldCultureTF.Text != String.Empty && EditOldCultureTF.Text != "null")
            {
                DataTable dt_old_culture = indexDS.Tables["Culture"];
                if (EditOldCultureTF.Text != String.Empty && dt_old_culture.Select("code_culture='" + EditOldCultureTF.Text + "'").Length > 0)
                {
                    OldCultureTF.Text = NullToEmpty(dt_old_culture.Select("code_culture='" + EditOldCultureTF.Text + "'")[0]["title_culture"].ToString());
                }
                else
                {
                    OldCultureTF.Text = String.Empty;
                    /*EditOldCultureTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Предшествующей культуры с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                OldCultureTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurOldCulture()
        {
            if (EditOldCultureTF.Text != null && EditOldCultureTF.Text != String.Empty && EditOldCultureTF.Text != "null")
            {
                DataTable dt_old_culture = indexDS.Tables["Culture"];
                if (EditOldCultureTF.Text != String.Empty && dt_old_culture.Select("code_culture='" + EditOldCultureTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Предшествующей культуры с кодом " + EditOldCultureTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditOldCultureTF}.focus(); #{EditOldCultureTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowTypeCropRotation()
        {
            //String temp = ReplaceValue(EditTypeCropRotationTF.Text, 1.ToString());
            /*String temp = EditTypeCropRotationTF.Text;
            if (EditTypeCropRotationTF.Text != temp) { EditTypeCropRotationTF.Text = temp; }
            else */
            if (EditTypeCropRotationTF.Text != null && EditTypeCropRotationTF.Text != String.Empty && EditTypeCropRotationTF.Text != "null")
            {
                DataTable dt_type_crop_rotation = indexDS.Tables["Type_crop_rotation"];
                if (EditTypeCropRotationTF.Text != String.Empty && dt_type_crop_rotation.Select("code_crop_rotation='" + EditTypeCropRotationTF.Text + "'").Length > 0)
                {
                    CropRotationTF.Text = NullToEmpty(dt_type_crop_rotation.Select("code_crop_rotation='" + EditTypeCropRotationTF.Text + "'")[0]["title_crop_rotation"].ToString());
                }
                else
                {
                    CropRotationTF.Text = String.Empty;
                    /*EditTypeCropRotationTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Типа севооборота с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                CropRotationTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurTypeCropRotation()
        {
            if (EditTypeCropRotationTF.Text != null && EditTypeCropRotationTF.Text != String.Empty && EditTypeCropRotationTF.Text != "null")
            {
                DataTable dt_type_crop_rotation = indexDS.Tables["Type_crop_rotation"];
                if (EditTypeCropRotationTF.Text != String.Empty && dt_type_crop_rotation.Select("code_crop_rotation='" + EditTypeCropRotationTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Типа севооборота с кодом " + EditTypeCropRotationTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditTypeCropRotationTF}.focus(); #{EditTypeCropRotationTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowTypeSoil()
        {
            //String temp = ReplaceValue(EditSoilTF.Text, 1.ToString());
            /*String temp = EditSoilTF.Text;
            if (EditSoilTF.Text != temp) { EditSoilTF.Text = temp; }
            else */
            if (EditSoilTF.Text != null && EditSoilTF.Text != String.Empty && EditSoilTF.Text != "null")
            {
                DataTable dt_type_soil = indexDS.Tables["Type_soil"];
                if (EditSoilTF.Text != String.Empty && dt_type_soil.Select("code_type_soil='" + EditSoilTF.Text + "'").Length > 0)
                {
                    SoilTF.Text = NullToEmpty(dt_type_soil.Select("code_type_soil='" + EditSoilTF.Text + "'")[0]["title_type_soil"].ToString());
                }
                else
                {
                    SoilTF.Text = String.Empty;
                    /*EditSoilTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Типа почвы с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                SoilTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurTypeSoil()
        {
            if (EditSoilTF.Text != null && EditSoilTF.Text != String.Empty && EditSoilTF.Text != "null")
            {
                DataTable dt_type_soil = indexDS.Tables["Type_soil"];
                if (EditSoilTF.Text != String.Empty && dt_type_soil.Select("code_type_soil='" + EditSoilTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Типа почвы с кодом " + EditSoilTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditSoilTF}.focus(); #{EditSoilTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowTypeGrading()
        {
            //String temp = ReplaceValue(EditGradingTF.Text, 1.ToString());
            /*String temp = EditGradingTF.Text;
            if (EditGradingTF.Text != temp) { EditGradingTF.Text = temp; }
            else */
            if (EditGradingTF.Text != null && EditGradingTF.Text != String.Empty && EditGradingTF.Text != "null")
            {
                DataTable dt_type_grading = indexDS.Tables["Type_grading"];
                if (EditGradingTF.Text != String.Empty && dt_type_grading.Select("code_grading='" + EditGradingTF.Text + "'").Length > 0)
                {
                    GradingTF.Text = NullToEmpty(dt_type_grading.Select("code_grading='" + EditGradingTF.Text + "'")[0]["title_grading"].ToString());
                }
                else
                {
                    GradingTF.Text = String.Empty;
                    /*EditGradingTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Мех. состава с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                GradingTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurTypeGrading()
        {
            if (EditGradingTF.Text != null && EditGradingTF.Text != String.Empty && EditGradingTF.Text != "null")
            {
                DataTable dt_type_grading = indexDS.Tables["Type_grading"];
                if (EditGradingTF.Text != String.Empty && dt_type_grading.Select("code_grading='" + EditGradingTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Мех. состава с кодом " + EditGradingTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditGradingTF}.focus(); #{EditGradingTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowTypeErosion()
        {
            //String temp = ReplaceValue(EditErosionTF.Text, 1.ToString());
            /*String temp = EditErosionTF.Text;
            if (EditErosionTF.Text != temp) { EditErosionTF.Text = temp; }
            else */
            if (EditErosionTF.Text != null && EditErosionTF.Text != String.Empty && EditErosionTF.Text != "null")
            {
                DataTable dt_type_erosion = indexDS.Tables["Type_erosion"];
                if (EditErosionTF.Text != String.Empty && dt_type_erosion.Select("code_erosion='" + EditErosionTF.Text + "'").Length > 0)
                {
                    ErosionTF.Text = NullToEmpty(dt_type_erosion.Select("code_erosion='" + EditErosionTF.Text + "'")[0]["title_erosion"].ToString());
                }
                else
                {
                    ErosionTF.Text = String.Empty;
                    /*EditErosionTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Степени эродированности с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                ErosionTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurTypeErosion()
        {
            if (EditErosionTF.Text != null && EditErosionTF.Text != String.Empty && EditErosionTF.Text != "null")
            {
                DataTable dt_type_erosion = indexDS.Tables["Type_erosion"];
                if (EditErosionTF.Text != String.Empty && dt_type_erosion.Select("code_erosion='" + EditErosionTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Степени эродированности с кодом " + EditErosionTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditErosionTF}.focus(); #{EditErosionTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowSlope()
        {
            //String temp = ReplaceValue(EditSlopeTF.Text, 1.ToString());
            /*String temp = EditSlopeTF.Text;
            if (EditSlopeTF.Text != temp) { EditSlopeTF.Text = temp; }
            else */
            if (EditSlopeTF.Text != null && EditSlopeTF.Text != String.Empty && EditSlopeTF.Text != "null")
            {
                DataTable dt_slope = indexDS.Tables["Slope"];
                
                if (EditSlopeTF.Text != String.Empty && dt_slope.Select("code_slope='" + EditSlopeTF.Text + "'").Length > 0)
                {
                    SlopeTF.Text = NullToEmpty(dt_slope.Select("code_slope='" + EditSlopeTF.Text + "'")[0]["title_slope"].ToString());
                }
                else
                {
                    SlopeTF.Text = String.Empty;
                    /*EditSlopeTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Уклона с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else 
            {
                SlopeTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurSlope()
        {
            if (EditSlopeTF.Text != null && EditSlopeTF.Text != String.Empty && EditSlopeTF.Text != "null")
            {
                DataTable dt_slope = indexDS.Tables["Slope"];

                if (EditSlopeTF.Text != String.Empty && dt_slope.Select("code_slope='" + EditSlopeTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Уклона с кодом " + EditSlopeTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditSlopeTF}.focus(); #{EditSlopeTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowExposure()
        {
            //String temp = ReplaceValue(EditExposureTF.Text, 1.ToString());
            /*String temp = EditExposureTF.Text;
            if (EditExposureTF.Text != temp) { EditExposureTF.Text = temp; }
            else */
            if (EditExposureTF.Text != null && EditExposureTF.Text != String.Empty && EditExposureTF.Text != "null")
            {
                DataTable dt_exposure = indexDS.Tables["Exposure"];
                if (EditExposureTF.Text != String.Empty && dt_exposure.Select("code_exposure='" + EditExposureTF.Text + "'").Length > 0)
                {
                    ExposureTF.Text = NullToEmpty(dt_exposure.Select("code_exposure='" + EditExposureTF.Text + "'")[0]["title_exposure"].ToString());
                }
                else
                {
                    ExposureTF.Text = String.Empty;
                    /*EditExposureTF.Text = String.Empty;
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = "Экспозиции с таким  кодом не существует. Значение будет удалено!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
            }
            else
            {
                ExposureTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void BlurExposure()
        {
            if (EditExposureTF.Text != null && EditExposureTF.Text != String.Empty && EditExposureTF.Text != "null")
            {
                DataTable dt_exposure = indexDS.Tables["Exposure"];
                if (EditExposureTF.Text != String.Empty && dt_exposure.Select("code_exposure='" + EditExposureTF.Text + "'").Length <= 0)
                {
                    X.Msg.Confirm("Предупреждение!!!", "Экспозиции с кодом " + EditExposureTF.Text + " не существует. Исправьте значение!", new MessageBoxButtonsConfig
                    {
                        Ok = new MessageBoxButtonConfig
                        {
                            Handler = "#{EditExposureTF}.focus(); #{EditExposureTF}.selectText();",
                            Text = "ОК"
                        }
                    }).Show();
                }
            }
        }

        [DirectMethod]
        public void ShowMethod(String count_eplot)
        {
            String temp = AdaptationValue(PhSTF.Text, "ph_s");
            if (PhSTF.Text != temp) { PhSTF.Text = temp; }
            else if (PhSTF.Text != null && PhSTF.Text != String.Empty && PhSTF.Text != "null")
            {
                DataTable dt_method = indexDS.Tables["Method"];
                if (PhSTF.Text != String.Empty && dt_method.Select("condition='0' AND from_pH<'" + PhSTF.Text + "' AND to_pH>='" + PhSTF.Text + "'").Length > 0)
                {
                    MethodTF.Text = dt_method.Select("condition='0' AND from_pH<'" + PhSTF.Text + "' AND to_pH>='" + PhSTF.Text + "'")[0]["title_method"].ToString();
                }
                else { MethodTF.Text = String.Empty; }
            }
            else
            {
                MethodTF.Text = String.Empty;
            }
            AddEditEplotFromPlotByPhS(count_eplot);
        }

        [DirectMethod]
        public void ShowACBS_TAB()
        {
            String tab = TotalAbsorbedBaseTF.Text, hydr_acid = AdaptationValue(HydrAcidTF.Text, "hydrolytic_acid");
            String temp_str = AdaptationValue(tab, "total_absorbed_bases");
            if (tab != temp_str) { TotalAbsorbedBaseTF.Text = temp_str; }
            if (temp_str == null || temp_str == String.Empty || temp_str == "null") { temp_str = "0"; }
            if (hydr_acid == null || hydr_acid == String.Empty || hydr_acid == "null") { hydr_acid = "0"; }
            Double temp = Convert.ToDouble(temp_str) + Convert.ToDouble(hydr_acid);
            if (temp_str != "0" || hydr_acid != "0")
            {
                CapacityTF.Text = AdaptationValue(temp.ToString(), "absorbance_capacity");
            }
            if (tab != "0" && hydr_acid != "0")
            {
                BaseSaturationTF.Text = AdaptationValue(((Convert.ToDouble(temp_str) / Convert.ToDouble(temp) * 100.0)).ToString(), "base_saturation");
            }
        }

        [DirectMethod]
        public void ShowACBS_HA(String count_eplot)
        {
            String tab = AdaptationValue(TotalAbsorbedBaseTF.Text, "total_absorbed_bases"), hydr_acid = HydrAcidTF.Text;
            String temp_str = AdaptationValue(hydr_acid, "hydrolytic_acid");
            if (hydr_acid != temp_str) { HydrAcidTF.Text = temp_str; }
            if (tab == null || tab == String.Empty || tab == "null") { tab = "0"; }
            if (temp_str == null || temp_str == String.Empty || temp_str == "null") { temp_str = "0"; }
            Double temp = Convert.ToDouble(tab) + Convert.ToDouble(temp_str);
            if (tab != "0" || temp_str != "0")
            {
                CapacityTF.Text = AdaptationValue(temp.ToString(), "absorbance_capacity");
            }
            if (tab != "0" && hydr_acid != "0")
            {
                BaseSaturationTF.Text = AdaptationValue(((Convert.ToDouble(tab) / Convert.ToDouble(temp) * 100.0)).ToString(), "base_saturation");
            }
            AddEditEplotFromPlotByHA(count_eplot);
        }

        [DirectMethod]
        public void EditNumberCropRotation()
        {
            String temp = ReplaceValue(EditNumberCropRotationTF.Text, 1.ToString());
            if (EditNumberCropRotationTF.Text != temp) { EditNumberCropRotationTF.Text = temp; }
        }

        [DirectMethod]
        public void EditField()
        {
            String temp = ReplaceValue(EditFieldTF.Text, 0.ToString());
            if (EditFieldTF.Text != temp) { EditFieldTF.Text = temp; }
        }

        [DirectMethod]
        public void EditNumberPlot()
        {
            String temp = ReplaceValue(NumberPlotTF.Text, 0.ToString());
            if (NumberPlotTF.Text != temp) { NumberPlotTF.Text = temp; }
        }

        [DirectMethod]
        public void EditArea()
        {
            String temp = ReplaceValue(EditAreaTF.Text, 2.ToString());
            if (EditAreaTF.Text != temp) { EditAreaTF.Text = temp; }
            AddEPlotBFocus();
        }

        [DirectMethod]
        public void EditN()
        {
            String temp = AdaptationValue(NTF.Text, "n");
            if (NTF.Text != temp) { NTF.Text = temp; }
        }

        [DirectMethod]
        public void EditNO3()
        {
            String temp = AdaptationValue(NO3TF.Text, "no3");
            if (NO3TF.Text != temp) { NO3TF.Text = temp; }
        }

        [DirectMethod]
        public void EditNO2()
        {
            String temp = AdaptationValue(NO2TF.Text, "no2");
            if (NO2TF.Text != temp) { NO2TF.Text = temp; }
        }

        [DirectMethod]
        public void EditHumus()
        {
            String temp = AdaptationValue(HumusTF.Text, "humus");
            if (HumusTF.Text != temp) { HumusTF.Text = temp; }
        }

        [DirectMethod]
        public void EditP2O5(String count_eplot)
        {
            String temp = AdaptationValue(P2O5TF.Text, "p2o5");
            if (P2O5TF.Text != temp) { P2O5TF.Text = temp; }
            AddEditEplotFromPlotByP2O5(count_eplot);
        }

        [DirectMethod]
        public void EditK2O(String count_eplot)
        {
            String temp = AdaptationValue(K2OTF.Text, "k2o");
            if (K2OTF.Text != temp) { K2OTF.Text = temp; }
            AddEditEplotFromPlotByK2O(count_eplot);
        }

        [DirectMethod]
        public void EditPhW()
        {
            String temp = AdaptationValue(PhWTF.Text, "ph_w");
            if (PhWTF.Text != temp) { PhWTF.Text = temp; }
        }

        [DirectMethod]
        public void EditS()
        {
            String temp = AdaptationValue(STF.Text, "s");
            if (STF.Text != temp) { STF.Text = temp; }
        }

        [DirectMethod]
        public void EditMn()
        {
            String temp = AdaptationValue(MnTF.Text, "mn");
            if (MnTF.Text != temp) { MnTF.Text = temp; }
        }

        [DirectMethod]
        public void EditFe()
        {
            String temp = AdaptationValue(FeTF.Text, "fe");
            if (FeTF.Text != temp) { FeTF.Text = temp; }
        }

        [DirectMethod]
        public void EditCu()
        {
            String temp = AdaptationValue(CuTF.Text, "cu");
            if (CuTF.Text != temp) { CuTF.Text = temp; }
        }

        [DirectMethod]
        public void EditZn()
        {
            String temp = AdaptationValue(ZnTF.Text, "zn");
            if (ZnTF.Text != temp) { ZnTF.Text = temp; }
        }

        [DirectMethod]
        public void EditCo()
        {
            String temp = AdaptationValue(CoTF.Text, "co");
            if (CoTF.Text != temp) { CoTF.Text = temp; }
        }

        [DirectMethod]
        public void EditAl()
        {
            String temp = AdaptationValue(AlTF.Text, "al");
            if (AlTF.Text != temp) { AlTF.Text = temp; }
        }

        [DirectMethod]
        public void EditCa()
        {
            String temp = AdaptationValue(CaTF.Text, "ca");
            if (CaTF.Text != temp) { CaTF.Text = temp; }
        }

        [DirectMethod]
        public void EditMo()
        {
            String temp = AdaptationValue(MoTF.Text, "mo");
            if (MoTF.Text != temp) { MoTF.Text = temp; }
        }

        [DirectMethod]
        public void EditB()
        {
            String temp = AdaptationValue(BTF.Text, "b");
            if (BTF.Text != temp) { BTF.Text = temp; }
        }

        [DirectMethod]
        public void EditMg()
        {
            String temp = AdaptationValue(MgTF.Text, "mg");
            if (MgTF.Text != temp) { MgTF.Text = temp; }
        }

        [DirectMethod]
        public void EditNa()
        {
            String temp = AdaptationValue(NaTF.Text, "na");
            if (NaTF.Text != temp) { NaTF.Text = temp; }
        }

        [DirectMethod]
        public void EditCutm()
        {
            String temp = AdaptationValue(CuhmTF.Text, "cu_hm");
            if (CuhmTF.Text != temp) { CuhmTF.Text = temp; }
        }
            
        [DirectMethod]
        public void EditZntm()
        {
            String temp = AdaptationValue(ZnhmTF.Text, "zn_hm");
            if (ZnhmTF.Text != temp) { ZnhmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditCdtm()
        {
            String temp = AdaptationValue(CdhmTF.Text, "cd_hm");
            if (CdhmTF.Text != temp) { CdhmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditPbtm()
        {
            String temp = AdaptationValue(PbhmTF.Text, "pb_hm");
            if (PbhmTF.Text != temp) { PbhmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditNitm()
        {
            String temp = AdaptationValue(NihmTF.Text, "ni_hm");
            if (NihmTF.Text != temp) { NihmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditHgtm()
        {
            String temp = AdaptationValue(HghmTF.Text, "hg_hm");
            if (HghmTF.Text != temp) { HghmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditMgtm()
        {
            String temp = AdaptationValue(MghmTF.Text, "mg_hm");
            if (MghmTF.Text != temp) { MghmTF.Text = temp; }
        }
        
        [DirectMethod]
        public void EditCrtm()
        {
            String temp = AdaptationValue(CrhmTF.Text, "cr_hm");
            if (CrhmTF.Text != temp) { CrhmTF.Text = temp; }
        }

        [DirectMethod]
        public void EditFetm()
        {
            String temp = AdaptationValue(FehmTF.Text, "fe_hm");
            if (FehmTF.Text != temp) { FehmTF.Text = temp; }
        }
        
        [DirectMethod]
        public void EditFtm()
        {
            String temp = AdaptationValue(FhmTF.Text, "f_hm");
            if (FhmTF.Text != temp) { FhmTF.Text = temp; }
        }
        
        [DirectMethod]
        public void EditAstm()
        {
            String temp = AdaptationValue(AshmTF.Text, "as_hm");
            if (AshmTF.Text != temp) { AshmTF.Text = temp; }
        }
        [DirectMethod]
        public void EditCs137()
        {
            String temp = AdaptationValue(Cs137TF.Text, "cs137");
            if (Cs137TF.Text != temp) { Cs137TF.Text = temp; }
        }
        [DirectMethod]
        public void EditSr90()
        {
            String temp = AdaptationValue(Sr90TF.Text, "sr90");
            if (Sr90TF.Text != temp) { Sr90TF.Text = temp; }
        }

        public void EditDryResidue()
        {
            String temp = AdaptationValue(DryResidueTF.Text, "dry_residue");
            if (DryResidueTF.Text != temp) { DryResidueTF.Text = temp; }
        }
        //------------------------------------------------------------------------------------------

        [DirectMethod]
        public void MicroElemShowHidden()
        {
            Micro.Hidden = MicroElemCB.Checked;
        }

        [DirectMethod]
        public void HeavyMetalShowHidden()
        {
            Heavy_metal.Hidden = HeavyMetalCB.Checked;
        }

        [DirectMethod]
        public void RadiologyShowHidden()
        {
            Radiology.Hidden = RadiologyCB.Checked;
        }

        [DirectMethod]
        public void SelectRegion()
        {
            if (connection_try)
            {
                //RegionGP.GetSelectionModel().Select(0);
                RegionGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_region));
            }
        }

        /*[DirectMethod]
        public void SelectOrganization()
        {
            OrganizationGP.GetSelectionModel().Select(0);
        }
        
        [DirectMethod]
        public void SelectDepartment()
        {
            DepartmentGP.GetSelectionModel().Select(0);
        }*/

        //выбор минимальных отделения, организации и района
        public void SetMinRegion()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand get_min_reg = new SqlCommand("Get_Min_Region", conn);
                get_min_reg.CommandType = CommandType.StoredProcedure;
                get_min_reg.Parameters.Add("@id_region", SqlDbType.Int);
                get_min_reg.Parameters.Add("@code_region", SqlDbType.Int);
                get_min_reg.Parameters["@id_region"].Direction = ParameterDirection.Output;
                get_min_reg.Parameters["@code_region"].Direction = ParameterDirection.Output;
                get_min_reg.ExecuteNonQuery();
                if (get_min_reg.Parameters["@id_region"].Value != null)
                {
                    if (Request.Browser.Cookies)
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        cookie["current_id_region"] = get_min_reg.Parameters["@id_region"].Value.ToString();
                        cookie["current_code_region"] = get_min_reg.Parameters["@code_region"].Value.ToString();
                        cookie["current_selected_region"] = 0.ToString();
                        Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = "Необходимо включить поддержку cookies!!!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    current_id_region = get_min_reg.Parameters["@id_region"].Value.ToString();
                    current_code_region = get_min_reg.Parameters["@code_region"].Value.ToString();
                    current_selected_region = 0.ToString();
                }
                else
                {
                    if (Request.Browser.Cookies)
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        cookie["current_id_region"] = 0.ToString();
                        cookie["current_code_region"] = 0.ToString();
                        cookie["current_selected_region"] = 0.ToString();
                        Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = "Необходимо включить поддержку cookies!!!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    current_id_region = 0.ToString();
                    current_code_region = 0.ToString();
                    current_selected_region = 0.ToString();
                }
                conn.Close();
            }
        }

        //функция временного хранения телефонов
        [DirectMethod]
        public void AddEditDeletePhone(String id_phone, String phone, Int32 action)
        {
            if (Request.Browser.Cookies)
            {
                cookie_phone = Request.Cookies["Agrochim31_Phones"];
                if (cookie_phone == null)
                {
                    cookie_phone = new HttpCookie("Agrochim31_Phones");
                    cookie_phone.Expires = DateTime.Now.AddHours(1);
                    cookie_phone[id_phone] = action + @"|" + phone;
                    Response.Cookies.Add(cookie_phone);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                cookie_phone[id_phone] = action + @"|" + phone;
                Response.Cookies.Set(cookie_phone);
            }
            //cookie_phone = Request.Cookies["Agrochim31_Phones"];
            //IndexSB.SetStatus(new StatusBarStatusConfig { Text = cookie_phone.Value.ToString(), IconCls = "icon-accept", Clear2 = true });
        }

        //функция временного хранения старых названий
        [DirectMethod]
        public void AddEditDeleteOldTitle(String id_old_title, String old_title, String action)
        {
            if (Request.Browser.Cookies)
            {
                cookie_old_title = Request.Cookies["Agrochim31_Old_Title"];
                if (cookie_old_title == null)
                {
                    cookie_old_title = new HttpCookie("Agrochim31_Old_Title");
                    cookie_old_title.Expires = DateTime.Now.AddHours(1);
                    cookie_old_title[id_old_title] = action + @"|" + old_title;
                    Response.Cookies.Add(cookie_old_title);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                cookie_old_title[id_old_title] = action + @"|" + old_title;
                Response.Cookies.Set(cookie_old_title);
            }
        }
        
        //функция временного хранения элементарных участков (образцов)
        /*[DirectMethod]
        public void AddEditDeleteEplot(String id_elementary_plot, String number_elementary_plot, String p2o5, String k2o, String ph_s, String ph_w, String hydrolytic_acid, String code_type_soil,
                                       String code_grading, String code_erosion, String code_slope, String code_exposure, String n, String no3, String no2, String humus, String total_absorbed_bases,
                                       String s, String ca, String mn, String mo, String b, String cu, String mg, String zn, String na, String co, String al, String fe, String cu_hm, String zn_hm,
                                       String cd_hm, String pb_hm, String ni_hm, String hg_hm, String mg_hm, String cr_hm, String fe_hm, String f_hm, String as_hm, String cs137, String sr90, String action)
        {
            if (Request.Browser.Cookies)
            {
                cookie_eplot = Request.Cookies["Agrochim31_Eplot"];
                if (cookie_eplot == null)
                {
                    cookie_eplot = new HttpCookie("Agrochim31_Eplot");
                    cookie_eplot.Expires = DateTime.Now.AddHours(1);
                    cookie_eplot[id_elementary_plot] = action + @"*number_elementary_plot|" + number_elementary_plot + @"#p2o5|" + p2o5 + @"#k2o|" + k2o + @"#ph_s|" + ph_s + @"#ph_w|" + ph_w
                                                     + @"#hydrolytic_acid|" + hydrolytic_acid + @"#code_type_soil|" + code_type_soil + @"#code_grading|" + code_grading + @"#code_erosion|" + code_erosion
                                                     + @"#code_slope|" + code_slope + @"#code_exposure|" + code_exposure + @"#n|" + n + @"#no3|" + no3 + @"#no2|" + no2 + @"#humus|" + humus
                                                     + @"#total_absorbed_bases|" + total_absorbed_bases + @"#s|" + s + @"#ca|" + ca + @"#mn|" + mn + @"#mo|" + mo + @"#b|" + b + @"#cu|" + cu + @"#mg|" + mg
                                                     + @"#zn|" + zn + @"#na|" + na + @"#co|" + co + @"#al|" + al + @"#fe|" + fe + @"#cu_hm|" + cu_hm + @"#zn_hm|" + zn_hm + @"#cd_hm|" + cd_hm
                                                     + @"#pb_hm|" + pb_hm + @"#ni_hm|" + ni_hm + @"#hg_hm|" + hg_hm + @"#mg_hm|" + mg_hm + @"#cr_hm|" + cr_hm + @"#fe_hm|" + fe_hm + @"#f_hm|" + f_hm
                                                     + @"#as_hm|" + as_hm + @"#cs137|" + cs137 + @"#sr90|" + sr90;
                    Response.Cookies.Add(cookie_eplot);
                }
                cookie_eplot[id_elementary_plot] = action + @"*number_elementary_plot|" + number_elementary_plot + @"#p2o5|" + p2o5 + @"#k2o|" + k2o + @"#ph_s|" + ph_s + @"#ph_w|" + ph_w
                                                     + @"#hydrolytic_acid|" + hydrolytic_acid + @"#code_type_soil|" + code_type_soil + @"#code_grading|" + code_grading + @"#code_erosion|" + code_erosion
                                                     + @"#code_slope|" + code_slope + @"#code_exposure|" + code_exposure + @"#n|" + n + @"#no3|" + no3 + @"#no2|" + no2 + @"#humus|" + humus
                                                     + @"#total_absorbed_bases|" + total_absorbed_bases + @"#s|" + s + @"#ca|" + ca + @"#mn|" + mn + @"#mo|" + mo + @"#b|" + b + @"#cu|" + cu + @"#mg|" + mg
                                                     + @"#zn|" + zn + @"#na|" + na + @"#co|" + co + @"#al|" + al + @"#fe|" + fe + @"#cu_hm|" + cu_hm + @"#zn_hm|" + zn_hm + @"#cd_hm|" + cd_hm
                                                     + @"#pb_hm|" + pb_hm + @"#ni_hm|" + ni_hm + @"#hg_hm|" + hg_hm + @"#mg_hm|" + mg_hm + @"#cr_hm|" + cr_hm + @"#fe_hm|" + fe_hm + @"#f_hm|" + f_hm
                                                     + @"#as_hm|" + as_hm + @"#cs137|" + cs137 + @"#sr90|" + sr90;
                Response.Cookies.Set(cookie_eplot);
            }
        }*/

        [DirectMethod]
        public void DeleteEplot(String id_elementary_plot)
        {
            if (Request.Browser.Cookies)
            {
                cookie_eplot = Request.Cookies["Agrochim31_Eplot"];
                if (Convert.ToInt32(NotNull(id_elementary_plot)) > 0)
                {
                    if (cookie_eplot == null)
                    {
                        cookie_eplot = new HttpCookie("Agrochim31_Eplot");
                        cookie_eplot.Expires = DateTime.Now.AddHours(1);
                        cookie_eplot.Value = id_elementary_plot;
                        Response.Cookies.Add(cookie_eplot);
                    }
                    else
                    {

                        cookie_eplot.Expires = DateTime.Now.AddHours(1);
                        cookie_eplot.Value = cookie_eplot.Value + "," + id_elementary_plot;
                        Response.Cookies.Set(cookie_eplot);
                    }
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        // вывод районов
        public void FillRegion()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRegion = new SqlDataAdapter(selectCommRegion, conn);
                adapterRegion.Fill(indexDS, "Region");
                indexDV = new System.Data.DataView(indexDS.Tables["Region"]);
                RegionS.DataSource = indexDV;
                RegionS.DataBind();
                conn.Close();
                /*RegionGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_region));
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Район в списке",
                    Message = current_selected_region,
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
                //FillOrganization(current_id_region);
            }
        }

        //получение id региона по выделенной записи
        [DirectMethod]
        public void SelectedRegion(String id_region, String code_region, String record_id)
        {
            if (connection_try)
            {
                String old_region = String.Empty;
                if (Request.Browser.Cookies)
                {
                    cookie = Request.Cookies["Agrochim31"];
                    old_region = cookie["current_id_region"].ToString();
                    cookie["current_id_region"] = id_region;
                    cookie["current_code_region"] = code_region;
                    cookie["current_selected_region"] = record_id;
                    Response.Cookies.Set(cookie);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                current_id_region = id_region;
                current_code_region = code_region;
                current_selected_region = record_id;
                FillOrganization(current_id_region);
                if (String.Compare(old_region, id_region) != 0)
                {
                    OrganizationGP.GetSelectionModel().Select(0);
                }
            }
        }
        
        // вывод всех хозяйств выделенного района
        public void FillOrganization(String id_region)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter(selectCommOrganization + " WHERE id_region=" + id_region, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                indexDV = new System.Data.DataView(indexDS.Tables["Organization"]);
                OrganizationS.DataSource = indexDV;
                OrganizationS.DataBind();
                OrganizationS.Sort("title_organization", Ext.Net.SortDirection.Default);
                //OrganizationGP.GetSelectionModel().Select(0);
                OrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_organization));
                //FillDepartment(current_id_organization);
                conn.Close();
            }
        }

        //получение id хозяйства по выделенной записи
        [DirectMethod]
        public void SelectedOrganization(String id_organization, String code_organization, String record_id)
        {
            try
            {
                if (connection_try)
                {
                    String old_organization = String.Empty;
                    if (Request.Browser.Cookies)
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        if (cookie.ToString().Contains("current_id_organization"))
                        {
                            old_organization = cookie["current_id_organization"].ToString();
                        }
                        cookie["current_id_organization"] = id_organization;
                        cookie["current_code_organization"] = code_organization;
                        cookie["current_selected_organization"] = record_id;
                        cookie["current_id_department"] = 0.ToString();
                        cookie["current_code_department"] = 0.ToString();
                        cookie["current_selected_department"] = 0.ToString();
                        Response.Cookies.Set(cookie);
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = "Необходимо включить поддержку cookies!!!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    current_id_organization = id_organization;
                    current_code_organization = code_organization;
                    current_selected_organization = record_id;
                    FillDepartment(current_id_organization);
                    GetOrganizationData(current_id_organization);
                    if (String.Compare(old_organization, id_organization) != 0)
                    {
                        DepartmentGP.GetSelectionModel().Select(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        // вывод всех отделений выделенного хозяйства
        public void FillDepartment(String id_organization)
        {
            if (connection_try)
            {
                conn.Open();
                adapterDepartment = new SqlDataAdapter(selectCommDepartment + " WHERE id_organization=" + id_organization, conn);
                adapterDepartment.Fill(indexDS, "Department");
                indexDV = new System.Data.DataView(indexDS.Tables["Department"]);
                DepartmentS.DataSource = indexDV;
                DepartmentS.DataBind();
                //DepartmentS.Sort("code_department");
                DepartmentGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_department));
                conn.Close();
            }
        }

        //вывод данных по хозяйству
        public void GetOrganizationData(String id_organization)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganizationData = new SqlDataAdapter(selectCommOrganizationData + " WHERE id_organization=" + id_organization, conn);
                adapterOrganizationData.Fill(indexDS, "OrganizationData");
                indexDV = new System.Data.DataView(indexDS.Tables["OrganizationData"]);
                AboutOrgS.DataSource = indexDV;
                AboutOrgS.DataBind();
                conn.Close();
                List<Dictionary<String, String>> records_dataorg = JSON.Deserialize<List<Dictionary<String, String>>>(AboutOrgGP.GetStore().JsonData);
                if (records_dataorg.Count > 0)
                {
                    AboutOrgGP.GetSelectionModel().Select(records_dataorg.Count - 1);
                }
                else
                {
                    AboutOrgS.RemoveAll();
                }
            }
        }

        //получение id отделения по выделенной записи
        [DirectMethod]
        public void SelectedDepartment(String id_department, String code_department, String record_id)
        {
            if (connection_try)
            {
                if (Request.Browser.Cookies)
                {
                    cookie = Request.Cookies["Agrochim31"];
                    cookie["current_id_department"] = id_department;
                    cookie["current_code_department"] = code_department;
                    cookie["current_selected_department"] = record_id;
                    Response.Cookies.Set(cookie);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                current_id_department = id_department;
                current_code_department = code_department;
                current_selected_department = record_id;
            }
        }

        // вывод окна добавления районов
        [DirectMethod]
        public void ShowAddRegionW()
        {
            AddEditRegionW.Title = "Новый район";
            AddEditRegionW.Icon = Icon.PageAdd;
            AcceptAddRegionB.Hidden = false;
            AcceptEditRegionB.Hidden = true;
            IdRegionTF.Text = String.Empty;
            CodeRegionTF.Text = String.Empty;
            TitleRegionTF.Text = String.Empty;
            OKATORegionTF.Text = String.Empty;
            AddEditRegionW.Show();
            CodeRegionTF.Focus();
            CodeRegionTF.SelectText();
        }
        
        // добавление района
        [DirectMethod]
        public void AddRegion(String code_region, String title_region, String OKATO_region)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_region = new SqlCommand("Add_Region", conn);
                add_region.CommandType = CommandType.StoredProcedure;
                add_region.Parameters.AddWithValue("@code_region", Convert.ToInt32(code_region));
                add_region.Parameters.AddWithValue("@title_region", title_region);
                add_region.Parameters.AddWithValue("@OKATO_region", OKATO_region);
                add_region.ExecuteNonQuery();
                conn.Close();
                FillRegion();
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись добавлена!", IconCls = "icon-accept", Clear2 = true });
            }
        }
        
        // вывод окна редактирования районов
        [DirectMethod]
        public void ShowEditRegionW(String id_region, String code_region, String title_region, String OKATO_region)
        {
            if (!user_reg_data.edit) { return; }
            if (connection_try)
            {
                FlagEditing regionFE = new FlagEditing(conn, "Region", Convert.ToInt32(id_region));
                if (regionFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Район с кодом " + code_region + " редактируется другим пользователем!").Show();
                    return;
                }
                regionFE.SetFlag();

                AddEditRegionW.Title = "Редактировать район";
                AddEditRegionW.Icon = Icon.PageEdit;
                AcceptAddRegionB.Hidden = true;
                AcceptEditRegionB.Hidden = false;
                IdRegionTF.Text = NullToEmpty(id_region);
                CodeRegionTF.Text = NullToEmpty(code_region);
                TitleRegionTF.Text = NullToEmpty(title_region);
                OKATORegionTF.Text = NullToEmpty(OKATO_region);
                AddEditRegionW.Show();
                TitleRegionTF.Focus();
                TitleRegionTF.SelectText();
            }
        }

        // редактирование района
        [DirectMethod]
        public void EditRegion(String id_region, String code_region, String title_region, String OKATO_region)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand edit_region = new SqlCommand("Edit_Region", conn);
                edit_region.CommandType = CommandType.StoredProcedure;
                edit_region.Parameters.AddWithValue("@id_region", Convert.ToInt32(id_region));
                edit_region.Parameters.AddWithValue("@code_region", Convert.ToInt32(code_region));
                edit_region.Parameters.AddWithValue("@title_region", title_region);
                edit_region.Parameters.AddWithValue("@OKATO_region", OKATO_region);
                edit_region.ExecuteNonQuery();
                conn.Close();
                FillRegion();
                /*FlagEditing regionFE = new FlagEditing(conn, "Region", Convert.ToInt32(id_region));
                regionFE.DeleteFlag();*/
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
            }
        }

        // отмена редактирования(и добавления) района
        [DirectMethod]
        public void CloseAddEditRegion(String id_region)
        {
            if (id_region != String.Empty && connection_try)
            {
                FlagEditing regionFE = new FlagEditing(conn, "Region", Convert.ToInt32(id_region));
                regionFE.DeleteFlag();
            }
        }

        //заполняем телефоны
        public void FillPhones(String id_org)
        {
            if (connection_try)
            {
                conn.Open();
                adapterPhones = new SqlDataAdapter(selectCommPhones + " WHERE id_organization=" + id_org.ToString(), conn);
                adapterPhones.Fill(indexDS, "Phones");
                indexDV = new System.Data.DataView(indexDS.Tables["Phones"]);
                PhonesS.DataSource = indexDV;
                PhonesS.DataBind();
                conn.Close();
            }
        }
        //заполняем старые названия
        public void FillOldTitle(String id_org)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOldTitle = new SqlDataAdapter(selectCommOldTitle + " WHERE id_organization=" + id_org.ToString(), conn);
                adapterOldTitle.Fill(indexDS, "Old_title_organization");
                indexDV = new System.Data.DataView(indexDS.Tables["Old_title_organization"]);
                OldOrganizationS.DataSource = indexDV;
                OldOrganizationS.DataBind();
                conn.Close();
            }
        }

        // вывод окна добавления хозяйств
        [DirectMethod]
        public void ShowAddOrganizationW()
        {
            AddEditOrganizationW.Title = "Новая организация (хозяйство)";
            AddEditOrganizationW.Icon = Icon.PageAdd;
            AcceptAddOrganizationB.Hidden = false;
            AcceptEditOrganizationB.Hidden = true;
            CodeRegionOrganizationTF.Text = String.Empty;
            CodeOrganizationTF.Text = String.Empty;
            TitleOrganizationTF.Text = String.Empty;
            FullTitleOrganizationTF.Text = String.Empty;
            LeaderTF.Text = String.Empty;
            BasisDocumentTF.Text = String.Empty;
            ChiefAgronomistTF.Text = String.Empty;
            LegalAddressTF.Text = String.Empty;
            MailingAddressTF.Text = String.Empty;
            EMailOrganizationTF.Text = String.Empty;
            OKTMOOrganizationTF.Text = String.Empty;
            OKATOOrganizationTF.Text = String.Empty;
            INNOrganizationTF.Text = String.Empty;
            KPPOrganizationTF.Text = String.Empty;
            OGRNOrganizationTF.Text = String.Empty;
            OKVEDOrganizationTF.Text = String.Empty;
            OKPOOrganizationTF.Text = String.Empty;
            PayAccountTF.Text = String.Empty;
            FullBankNameTF.Text = String.Empty;
            BIKTF.Text = String.Empty;
            BankCorrespondingAccountTF.Text = String.Empty;

            IdOrganizationTF.Text = String.Empty;


            CodeRegionOrganizationTF.Hidden = true;
            CodeOrganizationTF.Hidden = true;
            FillPhones("0");
            FillOldTitle("0");
            AddEditOrganizationW.Show();
            TitleOrganizationTF.Focus();
            TitleOrganizationTF.SelectText();
        }

        //подтверждение добавления хозяйства
        [DirectMethod]
        public void AddOrganization(String title_organization, String full_title_org, String leader_org, String basis_document, String chief_agronomist, String legal_address, String mailing_address, String email_address, String OKATO_org, String OKTMO_org, String INN_org, String KPP_org, String OGRN_org, String OKVED_org, String OKPO_org, String pay_account_org, String full_bank_name, String bik, String bank_correspond_account)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter(selectCommOrganization + " WHERE id_region=" + current_id_region, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                System.Data.DataView dv_organization = new System.Data.DataView(indexDS.Tables["Organization"]);
                conn.Close();
                dv_organization.Sort = "code_organization";
                String code_organization = String.Empty;
                for (Int32 i = 1; i <= 999; i++)
                {
                    if (i < 10) { code_organization = current_code_region + "00" + i.ToString(); }
                    else if (i > 9 && i < 100) { code_organization = current_code_region + "0" + i.ToString(); }
                    else if (i > 99 && i < 1000) { code_organization = current_code_region + i.ToString(); }
                    if (Convert.ToInt32(dv_organization.Find(code_organization).ToString()) == -1) { break; }
                }
                //условие на проверку названия!!!
                if (title_organization != String.Empty)
                {
                    conn.Open();
                    SqlCommand add_organization = new SqlCommand("Add_Organization", conn);
                    add_organization.CommandType = CommandType.StoredProcedure;
                    add_organization.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                    add_organization.Parameters.AddWithValue("@code_organization", Convert.ToInt32(code_organization));
                    add_organization.Parameters.AddWithValue("@title_organization", NotNullText(title_organization));
                    add_organization.Parameters.AddWithValue("@full_title_organization", NotNullText(full_title_org));
                    add_organization.Parameters.AddWithValue("@leader", NotNullText(leader_org));
                    add_organization.Parameters.AddWithValue("@basis_document", NotNullText(basis_document));
                    add_organization.Parameters.AddWithValue("@chief_agronomist", NotNullText(chief_agronomist));
                    add_organization.Parameters.AddWithValue("@legal_address", NotNullText(legal_address));
                    add_organization.Parameters.AddWithValue("@mailing_address", NotNullText(mailing_address));
                    add_organization.Parameters.AddWithValue("@email_organization", NotNullText(email_address));
                    add_organization.Parameters.AddWithValue("@inn_organization", NotNullText(INN_org));
                    add_organization.Parameters.AddWithValue("@okato_organization", NotNullText(OKATO_org));
                    add_organization.Parameters.AddWithValue("@oktmo_organization", NotNullText(OKTMO_org));
                    add_organization.Parameters.AddWithValue("@kpp_organization", NotNullText(KPP_org));
                    add_organization.Parameters.AddWithValue("@ogrn_organization", NotNullText(OGRN_org));
                    add_organization.Parameters.AddWithValue("@okved_organization", NotNullText(OKVED_org));
                    add_organization.Parameters.AddWithValue("@okpo_organization", NotNullText(OKPO_org));
                    add_organization.Parameters.AddWithValue("@pay_account", NotNullText(pay_account_org));
                    add_organization.Parameters.AddWithValue("@full_bank_name", NotNullText(full_bank_name));
                    add_organization.Parameters.AddWithValue("@bik", NotNullText(bik));
                    add_organization.Parameters.AddWithValue("@bank_correspond_account", NotNullText(bank_correspond_account));
                    add_organization.ExecuteNonQuery();
                    conn.Close();

                    if (Request.Browser.Cookies)
                    {
                        //Добавление телефонов
                        cookie_phone = Request.Cookies["Agrochim31_Phones"];
                        if (cookie_phone != null)
                        {
                            phone_iva = new id_value_action[cookie_phone.Value.Split('=').Length];
                            CookieSplit(phone_iva, cookie_phone);
                            if (phone_iva.Length > 0)
                            {
                                conn.Open();
                                SqlCommand add_phone;
                                for (int i = 0; i < phone_iva.Length; i++)
                                {
                                    if (phone_iva[i].action == 1)
                                    {
                                        add_phone = new SqlCommand("Add_Phone", conn);
                                        add_phone.CommandType = CommandType.StoredProcedure;
                                        add_phone.Parameters.AddWithValue("@code_organization", Convert.ToInt32(code_organization));
                                        add_phone.Parameters.AddWithValue("@phone", phone_iva[i].value);
                                        add_phone.ExecuteNonQuery();
                                    }
                                }
                                conn.Close();
                                Response.Cookies["Agrochim31_Phones"].Expires = DateTime.Now.AddHours(-1);
                            }
                        }
                        //Добавление старых названий
                        cookie_old_title = Request.Cookies["Agrochim31_Old_Title"];
                        if (cookie_old_title != null)
                        {
                            old_title_iva = new id_value_action[cookie_old_title.Value.Split('=').Length];
                            CookieSplit(old_title_iva, cookie_old_title);
                            if (old_title_iva.Length > 0)
                            {
                                conn.Open();
                                SqlCommand add_old_title;
                                for (int i = 0; i < old_title_iva.Length; i++)
                                {
                                    if (old_title_iva[i].action == 1)
                                    {
                                        add_old_title = new SqlCommand("Add_Old_Title", conn);
                                        add_old_title.CommandType = CommandType.StoredProcedure;
                                        add_old_title.Parameters.AddWithValue("@code_organization", Convert.ToInt32(code_organization));
                                        add_old_title.Parameters.AddWithValue("@old_title_organization", old_title_iva[i].value);
                                        add_old_title.ExecuteNonQuery();
                                    }
                                }
                                conn.Close();
                                Response.Cookies["Agrochim31_Old_Title"].Expires = DateTime.Now.AddHours(-1);
                            }
                        }
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = "Необходимо включить поддержку cookies!!!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                }
                if (indexDS.Tables["Organization"] != null) { indexDS.Tables["Organization"].Clear(); }
                FillOrganization(current_id_region);
                //10062016 Добавление отделения №1
                DataSet CurrentOrganizationDS = new DataSet();
                String select_organization_string = "SELECT * FROM Organization WHERE code_organization=" + code_organization;
                SqlDataAdapter select_organization_data = new SqlDataAdapter(select_organization_string, conn);
                select_organization_data.Fill(CurrentOrganizationDS, "CurrentOrganization");
                if (CurrentOrganizationDS.Tables["CurrentOrganization"].Rows.Count > 0)
                {
                    current_id_organization = CurrentOrganizationDS.Tables["CurrentOrganization"].Rows[0]["id_organization"].ToString();
                    current_code_organization = code_organization;
                    AddDepartment("Отделение №1");
                }
                //-------------------------
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись добавлена!", IconCls = "icon-accept", Clear2 = true });
            }
        }

        // вывод окна редактирования хозяйств
        [DirectMethod]
        public void ShowEditOrganizationW(String id_organization, String code_region, String code_org, String title_org, String full_title_org, String leader_org, String basis_document, String chief_agronomist, String legal_address, String mailing_address, String email_address, String INN_org, String OKATO_org, String OKTMO_org, String KPP_org, String OGRN_org, String OKVED_org, String OKPO_org, String pay_account_org, String full_bank_name, String bik, String bank_correspond_account)
        {
            if (!user_reg_data.edit) { return; }
            if (connection_try)
            {
                FlagEditing organizationFE = new FlagEditing(conn, "Organization", Convert.ToInt32(id_organization));
                if (organizationFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Организация с кодом " + code_org + " редактируется другим пользователем!").Show();
                    return;
                }
                organizationFE.SetFlag();
                AddEditOrganizationW.Title = "Редактировать организацию (хозяйство)";
                AddEditOrganizationW.Icon = Icon.PageEdit;
                AcceptAddOrganizationB.Hidden = true;
                AcceptEditOrganizationB.Hidden = false;
                IdOrganizationTF.Text = NullToEmpty(id_organization);
                CodeRegionOrganizationTF.Text = NullToEmpty(code_region);
                CodeOrganizationTF.Text = NullToEmpty(code_org);
                TitleOrganizationTF.Text = NullToEmpty(title_org);
                FullTitleOrganizationTF.Text = NullToEmpty(full_title_org);
                LeaderTF.Text = NullToEmpty(leader_org);
                BasisDocumentTF.Text = NullToEmpty(basis_document);
                ChiefAgronomistTF.Text = NullToEmpty(chief_agronomist);
                LegalAddressTF.Text = NullToEmpty(legal_address);
                MailingAddressTF.Text = NullToEmpty(mailing_address);
                EMailOrganizationTF.Text = NullToEmpty(email_address);
                OKTMOOrganizationTF.Text = NullToEmpty(OKTMO_org);
                OKATOOrganizationTF.Text = NullToEmpty(OKATO_org);
                INNOrganizationTF.Text = NullToEmpty(INN_org);
                KPPOrganizationTF.Text = NullToEmpty(KPP_org);
                OGRNOrganizationTF.Text = NullToEmpty(OGRN_org);
                OKVEDOrganizationTF.Text = NullToEmpty(OKVED_org);
                OKPOOrganizationTF.Text = NullToEmpty(OKPO_org);
                PayAccountTF.Text = NullToEmpty(pay_account_org);
                FullBankNameTF.Text = NullToEmpty(full_bank_name);
                BIKTF.Text = NullToEmpty(bik);
                BankCorrespondingAccountTF.Text = NullToEmpty(bank_correspond_account);

                CodeRegionOrganizationTF.Hidden = false;
                CodeOrganizationTF.Hidden = false;
                FillPhones(id_organization);
                FillOldTitle(id_organization);
                AddEditOrganizationW.Show();
                TitleOrganizationTF.Focus();
                TitleOrganizationTF.SelectText();
            }
        }

        [DirectMethod]
        public void EditOrganization(String id_organization, String title_org, String full_title_org, String leader_org, String basis_document, String chief_agronomist, String legal_address, String mailing_address, String email_address, String OKATO_org, String OKTMO_org, String INN_org, String KPP_org, String OGRN_org, String OKVED_org, String OKPO_org, String pay_account_org, String full_bank_name, String bik, String bank_correspond_account)
        {
            if (title_org != String.Empty && connection_try)
            {
                conn.Open();
                SqlCommand edit_organization = new SqlCommand("Edit_Organization", conn);
                edit_organization.CommandType = CommandType.StoredProcedure;
                edit_organization.Parameters.AddWithValue("@id_organization", Convert.ToInt32(id_organization));
                edit_organization.Parameters.AddWithValue("@title_organization", NotNullText(title_org));
                edit_organization.Parameters.AddWithValue("@full_title_organization", NotNullText(full_title_org));
                edit_organization.Parameters.AddWithValue("@leader", NotNullText(leader_org));
                edit_organization.Parameters.AddWithValue("@basis_document", NotNullText(basis_document));
                edit_organization.Parameters.AddWithValue("@chief_agronomist", NotNullText(chief_agronomist));
                edit_organization.Parameters.AddWithValue("@legal_address", NotNullText(legal_address));
                edit_organization.Parameters.AddWithValue("@mailing_address", NotNullText(mailing_address));
                edit_organization.Parameters.AddWithValue("@email_organization", NotNullText(email_address));
                edit_organization.Parameters.AddWithValue("@inn_organization", NotNullText(INN_org));
                edit_organization.Parameters.AddWithValue("@okato_organization", NotNullText(OKATO_org));
                edit_organization.Parameters.AddWithValue("@oktmo_organization", NotNullText(OKTMO_org));
                edit_organization.Parameters.AddWithValue("@kpp_organization", NotNullText(KPP_org));
                edit_organization.Parameters.AddWithValue("@ogrn_organization", NotNullText(OGRN_org));
                edit_organization.Parameters.AddWithValue("@okved_organization", NotNullText(OKVED_org));
                edit_organization.Parameters.AddWithValue("@okpo_organization", NotNullText(OKPO_org));
                edit_organization.Parameters.AddWithValue("@pay_account", NotNullText(pay_account_org));
                edit_organization.Parameters.AddWithValue("@full_bank_name", NotNullText(full_bank_name));
                edit_organization.Parameters.AddWithValue("@bik", NotNullText(bik));
                edit_organization.Parameters.AddWithValue("@bank_correspond_account", NotNullText(bank_correspond_account));

                edit_organization.ExecuteNonQuery();
                conn.Close();
                if (Request.Browser.Cookies)
                {
                    //Добавление телефонов
                    cookie_phone = Request.Cookies["Agrochim31_Phones"];
                    if (cookie_phone != null)
                    {
                        phone_iva = new id_value_action[cookie_phone.Value.Split('&').Length];
                        CookieSplit(phone_iva, cookie_phone);
                        if (phone_iva.Length > 0)
                        {
                            conn.Open();
                            SqlCommand add_edit_phone, delete_phone;
                            for (int i = 0; i < phone_iva.Length; i++)
                            {
                                if (phone_iva[i].action == 1)
                                {
                                    add_edit_phone = new SqlCommand("Add_Edit_Phone", conn);
                                    add_edit_phone.CommandType = CommandType.StoredProcedure;
                                    add_edit_phone.Parameters.AddWithValue("@id_phone", phone_iva[i].id);
                                    add_edit_phone.Parameters.AddWithValue("@id_organization", id_organization);
                                    add_edit_phone.Parameters.AddWithValue("@phone", phone_iva[i].value);
                                    add_edit_phone.ExecuteNonQuery();
                                }
                                if (phone_iva[i].action == 0 && phone_iva[i].id > 0)
                                {
                                    delete_phone = new SqlCommand("Delete_Phone", conn);
                                    delete_phone.CommandType = CommandType.StoredProcedure;
                                    delete_phone.Parameters.AddWithValue("@id_phone", phone_iva[i].id);
                                    delete_phone.Parameters.AddWithValue("@id_organization", id_organization);
                                    delete_phone.ExecuteNonQuery();
                                }
                            }
                            conn.Close();
                            Response.Cookies["Agrochim31_Phones"].Expires = DateTime.Now.AddHours(-1);
                        }
                    }
                    //Добавление старых названий
                    cookie_old_title = Request.Cookies["Agrochim31_Old_Title"];
                    if (cookie_old_title != null)
                    {
                        old_title_iva = new id_value_action[cookie_old_title.Value.Split('&').Length];
                        CookieSplit(old_title_iva, cookie_old_title);
                        if (old_title_iva.Length > 0)
                        {
                            conn.Open();
                            SqlCommand add_edit_old_title, delete_old_title;
                            for (int i = 0; i < old_title_iva.Length; i++)
                            {
                                if (old_title_iva[i].action == 1)
                                {
                                    add_edit_old_title = new SqlCommand("Add_Edit_Old_Title", conn);
                                    add_edit_old_title.CommandType = CommandType.StoredProcedure;
                                    add_edit_old_title.Parameters.AddWithValue("@id_old_title_organization", old_title_iva[i].id);
                                    add_edit_old_title.Parameters.AddWithValue("@id_organization", id_organization);
                                    add_edit_old_title.Parameters.AddWithValue("@old_title_organization", old_title_iva[i].value);
                                    add_edit_old_title.ExecuteNonQuery();
                                }
                                if (old_title_iva[i].action == 0 && old_title_iva[i].id > 0)
                                {
                                    delete_old_title = new SqlCommand("Delete_Old_Title", conn);
                                    delete_old_title.CommandType = CommandType.StoredProcedure;
                                    delete_old_title.Parameters.AddWithValue("@id_old_title_organization", old_title_iva[i].id);
                                    delete_old_title.Parameters.AddWithValue("@id_organization", id_organization);
                                    delete_old_title.ExecuteNonQuery();
                                }
                            }
                            conn.Close();
                            Response.Cookies["Agrochim31_Old_Title"].Expires = DateTime.Now.AddHours(-1);
                        }
                    }
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                /*FlagEditing organizationFE = new FlagEditing(conn, "Organization", Convert.ToInt32(id_organization));
                organizationFE.DeleteFlag();*/
                if (indexDS.Tables["Organization"] != null) { indexDS.Tables["Organization"].Clear(); }
                FillOrganization(current_id_region);
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
            }
        }

        // отмена редактирования(и добавления) хозяйства
        [DirectMethod]
        public void CloseAddEditOrganization(String id_organization)
        {
            if (id_organization != String.Empty && connection_try)
            {
                FlagEditing organizationFE = new FlagEditing(conn, "Organization", Convert.ToInt32(id_organization));
                organizationFE.DeleteFlag();
            }
            //чистим куки при отмене
            if (Request.Browser.Cookies)
            {
                if (Request.Cookies["Agrochim31_Phones"] != null)
                {
                    Response.Cookies["Agrochim31_Phones"].Expires = DateTime.Now.AddHours(-1);
                }
                if (Request.Cookies["Agrochim31_Old_Title"] != null)
                {
                    Response.Cookies["Agrochim31_Old_Title"].Expires = DateTime.Now.AddHours(-1);
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        // вывод окна добавления отделений
        [DirectMethod]
        public void ShowAddDepartmentW()
        {
            AddEditDepartmentW.Title = "Новое отделение";
            AddEditDepartmentW.Icon = Icon.PageAdd;
            AcceptAddDepartmentB.Hidden = false;
            AcceptEditDepartmentB.Hidden = true;
            CodeOrganizationDepartmentTF.Hidden = true;
            CodeDepartmentTF.Hidden = true;
            CodeOrganizationDepartmentTF.Text = String.Empty;
            CodeDepartmentTF.Text = String.Empty;
            TitleDepartmentTF.Text = String.Empty;
            IdDepartmentTF.Text = String.Empty;
            AddEditDepartmentW.Show();
            TitleDepartmentTF.Focus();
            TitleDepartmentTF.SelectText();
        }
        
        //добавление отделения
        [DirectMethod]
        public void AddDepartment(String title_department)
        {
            if (connection_try)
            {
                conn.Open();
                adapterDepartment = new SqlDataAdapter(selectCommDepartment + " WHERE id_organization=" + current_id_organization, conn);
                adapterDepartment.Fill(indexDS, "Department");
                System.Data.DataView dv_department = new System.Data.DataView(indexDS.Tables["Department"]);
                dv_department.Sort = "code_department";
                String code_department = String.Empty;
                for (Int32 i = 1; i <= 999; i++)
                {
                    if (i < 10) { code_department = current_code_organization + "0" + i.ToString(); }
                    else if (i > 9 && i < 100) { code_department = current_code_organization + i.ToString(); }
                    if (Convert.ToInt32(dv_department.Find(code_department).ToString()) == -1) { break; }
                }
                //условие на проверку названия!!!
                if (title_department != String.Empty)
                {
                    SqlCommand add_department = new SqlCommand("Add_Department", conn);
                    add_department.CommandType = CommandType.StoredProcedure;
                    add_department.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                    add_department.Parameters.AddWithValue("@code_department", Convert.ToInt32(code_department));
                    add_department.Parameters.AddWithValue("@title_department", title_department);
                    add_department.ExecuteNonQuery();
                }
                if (indexDS.Tables["Department"] != null) { indexDS.Tables["Department"].Clear(); }
                conn.Close();
                FillDepartment(current_id_organization);
                AddEditDepartmentW.Close();
            }
        }

        // вывод окна редактирования отделений
        [DirectMethod]
        public void ShowEditDepartmentW(String id_department, String code_organization_department, String code_department, String title_department)
        {
            if (!user_reg_data.edit) { return; }
            if (connection_try)
            {
                FlagEditing departmentFE = new FlagEditing(conn, "Department", Convert.ToInt32(id_department));
                if (departmentFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Отделение с кодом " + code_department + " редактируется другим пользователем!").Show();
                    return;
                }
                departmentFE.SetFlag();
                AddEditDepartmentW.Title = "Редактировать отделение";
                AddEditDepartmentW.Icon = Icon.PageEdit;
                AcceptAddDepartmentB.Hidden = true;
                AcceptEditDepartmentB.Hidden = false;
                CodeOrganizationDepartmentTF.Hidden = false;
                CodeDepartmentTF.Hidden = false;
                CodeOrganizationDepartmentTF.Text = NullToEmpty(code_organization_department);
                CodeDepartmentTF.Text = NullToEmpty(code_department);
                TitleDepartmentTF.Text = NullToEmpty(title_department);
                IdDepartmentTF.Text = NullToEmpty(id_department);
                AddEditDepartmentW.Show();
                TitleDepartmentTF.Focus();
                TitleDepartmentTF.SelectText();
            }
        }

        //редактирование отделения
        [DirectMethod]
        public void EditDepartment(String id_department, String title_department)
        {
            if (connection_try)
            {
                //условие на проверку названия!!!
                if (title_department != String.Empty && connection_try)
                {
                    conn.Open();
                    SqlCommand edit_department = new SqlCommand("Edit_Department", conn);
                    edit_department.CommandType = CommandType.StoredProcedure;
                    edit_department.Parameters.AddWithValue("@id_department", Convert.ToInt32(id_department));
                    edit_department.Parameters.AddWithValue("@title_department", title_department);
                    edit_department.ExecuteNonQuery();
                    conn.Close();
                }
                /*FlagEditing departmentFE = new FlagEditing(conn, "Department", Convert.ToInt32(id_department));
                departmentFE.DeleteFlag();*/
                if (indexDS.Tables["Department"] != null) { indexDS.Tables["Department"].Clear(); }
                FillDepartment(current_id_organization);
            }
        }

        // отмена редактирования(и добавления) отделения
        [DirectMethod]
        public void CloseAddEditDepartment(String id_department)
        {
            if (id_department != String.Empty && connection_try)
            {
                FlagEditing departmentFE = new FlagEditing(conn, "Department", Convert.ToInt32(id_department));
                departmentFE.DeleteFlag();
            }
        }

        // окно удаления отделения
        [DirectMethod]
        public void WindowRemovalDepartment(String id_department)
        {
            if (connection_try)
            {
                FlagEditing departmentFE = new FlagEditing(conn, "Department", Convert.ToInt32(id_department));
                if (departmentFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Даннное отделение редактируется другим пользователем!").Show();
                    return;
                }

                X.Msg.Confirm("Удаление записи", "Удалить выбранное отделение?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.RemoveYes(" + id_department + ");",
                        Text = "Да"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.RemoveNo();",
                        Text = "Нет"
                    }
                }).Show();
            }
        }

        [DirectMethod]
        public void RemoveYes(String id_department)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_department = new SqlCommand("Delete_Department", conn);
                delete_department.CommandType = CommandType.StoredProcedure;
                delete_department.Parameters.AddWithValue("@id_department", Convert.ToInt32(id_department));
                delete_department.ExecuteNonQuery();
                conn.Close();
                FillDepartment(current_id_organization);
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Отделение удалено!", IconCls = "icon-accept", Clear2 = true });
            }
        }

        [DirectMethod]
        public void RemoveNo()
        {
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Удаление отменено!", IconCls = "icon-cancel", Clear2 = true });
        }

        [DirectMethod]
        public void FillTour()
        {
            if (connection_try)
            {
                conn.Open();
                adapterTour = new SqlDataAdapter(selectCommTour + " WHERE id_department = " + current_id_department, conn);
                adapterTour.Fill(indexDS, "Tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Tours"]);
                ToursS.DataSource = indexDV;
                ToursS.DataBind();
                List<Dictionary<String, String>> records_tours = JSON.Deserialize<List<Dictionary<String, String>>>(ToursGP.GetStore().JsonData);
                if (records_tours.Count > 0)
                {
                    ToursGP.GetSelectionModel().Select(records_tours.Count - 1);
                }
                else
                {
                    YearsS.RemoveAll();
                    PlotsS.RemoveAll();
                    EPlotsS.RemoveAll();
                    EditEPlotS.RemoveAll();
                }
                conn.Close();
            }
        }
        /*
        [DirectMethod]
        public void SetCurrentTour(String tour)
        {
            if (Request.Browser.Cookies && tour != null && tour != "null" && tour != String.Empty)
            {
                HttpCookie current_tour_cookie = Request.Cookies["Agrochim31"];
                if (current_tour_cookie == null)
                {
                    current_tour_cookie = new HttpCookie("Agrochim31");
                    Response.Cookies.Add(current_tour_cookie);
                }
                current_tour_cookie["current_tour"] = tour;
                Response.Cookies.Set(current_tour_cookie);
            }
        }

        [DirectMethod]
        public void SetCurrentYear(String year)
        {
            if (Request.Browser.Cookies && year != null && year != "null" && year != String.Empty)
            {
                HttpCookie current_year_cookie = Request.Cookies["Agrochim31"];
                if (current_year_cookie == null)
                {
                    current_year_cookie = new HttpCookie("Agrochim31");
                    Response.Cookies.Add(current_year_cookie);
                }
                current_year_cookie["current_year"] = year;
                Response.Cookies.Set(current_year_cookie);
            }
        }
        */
        [DirectMethod]
        public void FillYear(String tour)
        {
            if (tour != String.Empty && tour != "" && tour != null)
            {
                adapterYear = new SqlDataAdapter(selectCommYear + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                if (connection_try)
                {
                    conn.Open();
                    adapterYear.Fill(indexDS, "Years");
                    indexDV = new System.Data.DataView(indexDS.Tables["Years"]);
                    YearsS.DataSource = indexDV;
                    YearsS.DataBind();
                    //PlotsS.Reload();
                    //EPlotsS.Reload();
                    conn.Close();
                }
                List<Dictionary<String, String>> records_years = JSON.Deserialize<List<Dictionary<String, String>>>(YearsGP.GetStore().JsonData);
                if (records_years.Count > 0)
                {
                    YearsGP.GetSelectionModel().Select(records_years.Count - 1);
                    /*X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Предупреждение!!!",
                        Message = records_years.Count.ToString(),
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });*/
                }
                else
                {
                    YearsS.RemoveAll();
                    PlotsS.RemoveAll();
                    EPlotsS.RemoveAll();
                    EditEPlotS.RemoveAll();
                }
            }
            else
            {
                YearsS.RemoveAll();
                PlotsS.RemoveAll();
                EPlotsS.RemoveAll();
                EditEPlotS.RemoveAll();
            }
        }

        /*public String GetMaxYear()
        {
            String max_year;
            SqlCommand get_max_year = new SqlCommand("Get_Max_Year", conn);
            get_max_year.CommandType = CommandType.StoredProcedure;
            get_max_year.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
            get_max_year.Parameters.Add("@max_year", SqlDbType.Int);
            get_max_year.Parameters["@max_year"].Direction = ParameterDirection.Output;
            get_max_year.ExecuteNonQuery();
            if (get_max_year.Parameters["@max_year"].Value.ToString() != "" && get_max_year.Parameters["@max_year"].Value != null)
            {
                max_year = get_max_year.Parameters["@max_year"].Value.ToString();
            }
            else { max_year = "0"; }
            return max_year;
        }*/

        [DirectMethod]
        public void FillPlot(String year)
        {
            if (year != String.Empty && connection_try)
            {
                conn.Open();
                adapterPlot = new SqlDataAdapter(selectCommPlot + " WHERE id_department=" + current_id_department + " AND year=" + year + "ORDER BY date_input", conn);
                adapterPlot.Fill(indexDS, "Plot");
                indexDV = new System.Data.DataView(indexDS.Tables["Plot"]);
                PlotsS.DataSource = indexDV;
                PlotsS.DataBind();
                conn.Close();
                List<Dictionary<String, String>> records_plot = JSON.Deserialize<List<Dictionary<String, String>>>(PlotsGP.GetStore().JsonData);
                if (records_plot.Count > 0)
                {
                    PlotsGP.GetSelectionModel().Select(records_plot.Count - 1);
                }
                else
                {
                    EPlotsS.RemoveAll();
                }
                /*if (indexDS.Tables["Plot"].Rows.Count > 0)
                {
                    PlotsGP.GetSelectionModel().Select(indexDS.Tables["Plot"].Rows.Count - 1);
                }*/
            }
        }

        [DirectMethod]
        public void FillEPlot(String id_plot)
        {
            if (connection_try)
            {
                conn.Open();
                adapterEPlot = new SqlDataAdapter(selectCommEPlot + " WHERE id_plot=" + id_plot, conn);
                adapterEPlot.Fill(indexDS, "Elementary_plot");
                
                indexDV = new System.Data.DataView(indexDS.Tables["Elementary_plot"]);
                EPlotsS.DataSource = indexDV;
                EPlotsS.DataBind();
                EPlotsS.Sort("number_elementary_plot", Ext.Net.SortDirection.Default);
                //EPlotsGP.GetSelectionModel().Select(0);
                EditEPlotS.DataSource = indexDV;
                EditEPlotS.DataBind();
                EditEPlotS.Sort("number_elementary_plot", Ext.Net.SortDirection.Default);
                //EditEPlotGP.GetSelectionModel().Select(0);
                conn.Close();
            }
        }

        [DirectMethod]
        public void FillSlopeFromPlot(String id_plot)
        {
            if (connection_try)
            {
                conn.Open();
                adapterSlopeFromPlot = new SqlDataAdapter(selectCommSlopeFromPlot + " WHERE id_plot=" + id_plot, conn);
                adapterSlopeFromPlot.Fill(indexDS, "Slope_From_Plot");
                indexDV = new System.Data.DataView(indexDS.Tables["Slope_From_Plot"]);
                SlopesS.DataSource = indexDV;
                SlopesS.DataBind();
                SlopesGP.GetSelectionModel().Select(0);
                conn.Close();
            }
        }

        [DirectMethod]
        public void FillEPlotAndSlope(String id_plot)
        {
            FillEPlot(id_plot);
            FillSlopeFromPlot(id_plot);
        }

        [DirectMethod]
        public void StatisticsOrganization(String tour)
        {
            if (tour != String.Empty && connection_try && tour != "" && tour != null)
            {
                conn.Open();
                SqlCommand statistics_org = new SqlCommand("Statistics_Organization", conn);
                statistics_org.CommandType = CommandType.StoredProcedure;
                statistics_org.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                statistics_org.Parameters.AddWithValue("@tour", Convert.ToInt32(tour));
                statistics_org.Parameters.Add("@count_plots", SqlDbType.Int);
                statistics_org.Parameters.Add("@count_eplot", SqlDbType.Int);
                statistics_org.Parameters.Add("@area_plots", SqlDbType.Float);
                statistics_org.Parameters["@count_plots"].Direction = ParameterDirection.Output;
                statistics_org.Parameters["@count_eplot"].Direction = ParameterDirection.Output;
                statistics_org.Parameters["@area_plots"].Direction = ParameterDirection.Output;
                statistics_org.ExecuteNonQuery();
                CountPlotTF.Text = statistics_org.Parameters["@count_plots"].Value.ToString();
                CountEPlotTF.Text = statistics_org.Parameters["@count_eplot"].Value.ToString();
                AreaFieldTF.Text = statistics_org.Parameters["@area_plots"].Value.ToString();
                conn.Close();
            }
            else
            {
                CountPlotTF.Text = String.Empty;
                CountEPlotTF.Text = String.Empty;
                AreaFieldTF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void ShowEditdbW()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterDepartmentCB = new SqlDataAdapter(selectCommDepartment + " WHERE id_organization=" + current_id_organization, conn);
                adapterDepartmentCB.Fill(indexDS, "DepartmentCB");
                indexDV = new System.Data.DataView(indexDS.Tables["DepartmentCB"]);
                conn.Close();
                EditdbDepartmentS.DataSource = indexDV;
                EditdbDepartmentS.DataBind();
                EditdbW.Show();
                FillTour();
                FillYear("");
                //FillPlot(GetMaxYear());
                conn.Open();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                EditdbRegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                EditdbOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                EditdbDepartmentCB.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                //EditdbDepartmentCB.Select(0);
                conn.Close();
                /*Int32 count_tour = ToursGP.Items.Count() - 1;
                if (count_tour >= 0) { ToursGP.GetSelectionModel().Select(count_tour); }
                Int32 count_year = YearsGP.Items.Count() - 1;
                if (count_year >= 0) { YearsGP.GetSelectionModel().Select(count_year); }*/
            }
        }

        [DirectMethod]
        public void EditdbDepartmentCBValueChange(String id_department, String code_department)
        {
            if (Request.Browser.Cookies)
            {
                cookie = Request.Cookies["Agrochim31"];
                cookie["current_id_department"] = id_department;
                current_id_department = id_department;
                cookie["current_code_department"] = code_department;
                current_code_department = code_department;
                Response.Cookies.Set(cookie);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            current_id_department = id_department;
            current_code_department = code_department;
            FillTour();
            FillYear("");
            //FillPlot(GetMaxYear());
        }

        /*
        // вывод показателей
        public void FillSignificative()
        {
            if (connection_try)
            {
                conn.Open();
                adapterSignificative = new SqlDataAdapter(selectCommSignificative, conn);
                adapterSignificative.Fill(indexDS, "Significative");
                indexDV = new System.Data.DataView(indexDS.Tables["Significative"]);
                SignificativeS.DataSource = indexDV;
                SignificativeS.DataBind();
                conn.Close();
            }
        }
         */
        /*
        //получение групп выделенного показателя
        [DirectMethod]
        public void FillGroups(String id_significative)
        {
            if (connection_try && id_significative != null && id_significative != "" && id_significative != String.Empty)
            {
                conn.Open();
                adapterGroups = new SqlDataAdapter(selectCommGroups + " WHERE id_significative=" + id_significative, conn);
                adapterGroups.Fill(indexDS, "Groups");
                indexDV = new System.Data.DataView(indexDS.Tables["Groups"]);
                GroupsS.DataSource = indexDV;
                GroupsS.DataBind();
                conn.Close();
            }
        }*/

        //получение пользователей выделенной роли
        /*[DirectMethod]
        public void FillUsers(String id_role)
        {
            if (connection_try && id_role != null && id_role != "" && id_role != String.Empty)
            {
                conn.Open();
                adapterUser = new SqlDataAdapter(selectCommUser + " WHERE id_role=" + id_role, conn);
                adapterUser.Fill(indexDS, "Users");
                indexDV = new System.Data.DataView(indexDS.Tables["Users"]);
                UserS.DataSource = indexDV;
                UserS.DataBind();
                conn.Close();
            }
        }*/

        // сохранение и отмена сохранения настроек
        [DirectMethod]
        public void AcceptSettings()
        {
            IndexSB.ShowBusy("Сохранение...");
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Настройки сохранены!", IconCls = "icon-accept", Clear2 = true });
        }
        [DirectMethod]
        public void CancelSettings()
        {
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Настройки не сохранены!", IconCls = "icon-cancel", Clear2 = true });
        }

        // добавление хозяйства и отделения
        [DirectMethod]
        public void SaveAdd()
        {
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись добавлена!", IconCls = "icon-accept", Clear2 = true });
        }
        
        // редактирование хозяйства и отделения
        [DirectMethod]
        public void SaveEdit()
        {
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
        }

        // добавление и редактирование участка
        [DirectMethod]
        public void SavePlot()
        {
            EditdbSB.SetStatus(new StatusBarStatusConfig { Text = "Данные по участку сохранены!", IconCls = "icon-accept", Clear2 = true });
        }
        
        // удаление участка
        [DirectMethod]
        public void WindowRemovalPlot(String id_plot)
        {
            if (connection_try)
            {
                FlagEditing plotFE = new FlagEditing(conn, "Plot", Convert.ToInt32(id_plot));
                if (plotFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Данный участок редактируется другим пользователем!").Show();
                    return;
                }

                X.Msg.Confirm("Удаление участка", "Удалить выбранный участок?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.RemovePlotYes(" + id_plot + ")",
                        Text = "Да"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.RemovePlotNo()",
                        Text = "Нет"
                    }
                }).Show();
            }
        }
        [DirectMethod]
        public void RemovePlotYes(String id_plot)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_plot = new SqlCommand("Delete_Plot", conn);
                delete_plot.CommandType = CommandType.StoredProcedure;
                delete_plot.Parameters.AddWithValue("@id_plot", Convert.ToInt32(id_plot));
                delete_plot.ExecuteNonQuery();
                conn.Close();
                PlotsS.RemoveAll();
                FillTour();
                FillYear("");
                PlotsS.Reload();
                EditdbSB.SetStatus(new StatusBarStatusConfig { Text = "Участок удален!", IconCls = "icon-accept", Clear2 = true });
            }
        }
        [DirectMethod]
        public void RemovePlotNo()
        {
            EditdbSB.SetStatus(new StatusBarStatusConfig { Text = "Удаление участка отменено!", IconCls = "icon-cancel", Clear2 = true });
        }

        /*
        [DirectMethod]
        public void RemoveYes(String id_department)
        {
            conn.Open();
            SqlCommand delete_department = new SqlCommand("Delete_Department", conn);
            delete_department.CommandType = CommandType.StoredProcedure;
            delete_department.Parameters.AddWithValue("@id_department", Convert.ToInt32(id_department));
            delete_department.ExecuteNonQuery();
            FillDepartment(current_id_organization);
            conn.Close();
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Отделение удалено!", IconCls = "icon-accept", Clear2 = true });
        }
 
         */
        // отмена сохранения
        [DirectMethod]
        public void CancelSave()
        {
            EditdbSB.SetStatus(new StatusBarStatusConfig { Text = "Изменения не сохранены!", IconCls = "icon-cancel", Clear2 = true });
        }
        
        // сохранение изменений       
        [DirectMethod]
        public void SaveChanges()
        {
            EditdbSB.SetStatus(new StatusBarStatusConfig { Text = "Изменения сохранены!", IconCls = "icon-accept", Clear2 = true });
        }

        // очистка формы ввода кода доступа
        [DirectMethod]
        public void ClearAccessCodeW()
        {
            LoginFormShow("0");
            AccessCodeTF.Text = String.Empty;
            AccessCodeL.Hidden = true;
            AccessCodeW.Show();
        }

        // проверка кода доступа к БД
        [DirectMethod]
        public void CodeValue(String str)
        {
            //код доступа по умолчанию "md5hash"
            //f9d08276bc85d30d578e8883f3c7e843
            //Создаём переменную и загружаем Xml документ
            XmlDocument connect_to_server = new XmlDocument();
            connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "connection.xml"));
            //XmlNodeList connection = connect_to_server.GetElementsByTagName("item");
            String strHash = GetValueFromXml(connect_to_server, "item", "accesscode", "value");
            if (VerifyMd5Hash(str, strHash))
            {
                AccessCodeW.Close();
                EditAccessCodeTF.Text = "";
                ConnectionW.Show();
            }
            else 
            {
                AccessCodeL.Hidden = false;
            }
        }
        
        // подключение
        [DirectMethod]
        public void GetAllValue()
        {
            //Создаём переменную Xml
            XmlDocument connect_to_server = new XmlDocument();
            //Загружаем параметры с файла
            connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "connection.xml"));
            //Выбираем отдельные элементы и выводим их
            ServerdbTF.Text = GetValueFromXml(connect_to_server, "item", "server", "value");
            DbTF.Text = GetValueFromXml(connect_to_server, "item", "database", "value");
            LoginTF.Text = GetValueFromXml(connect_to_server, "item", "login", "value");
            PasswordTF.Text = GetValueFromXml(connect_to_server, "item", "password", "value");
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
                                          + "; Password=" + GetValueFromXml(connect_to_server, "item", "password", "value");
            }
            catch (Exception exc)
            {
                X.Msg.Notify("Ошибка", exc.Message).Show();
            }
            return connString;
        }

        //запись настроек подключения
        [DirectMethod]
        public void SetAllValue(String serverDB, String nameDB, String login, String pass, String accesscode)
        {
            //Создаём переменную и загружаем Xml документ
            XmlDocument connect_to_server = new XmlDocument();
            XmlNodeList connection;
            if (System.IO.File.Exists(Server.MapPath("~/") + "connection.xml"))
            {
                connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "connection.xml"));
                connection = connect_to_server.GetElementsByTagName("item");
                //Задаём значения элементов по очереди
                foreach (XmlNode x in connection)
                {
                    if (x.Attributes["id"].Value == "server")
                    {
                        x.Attributes["value"].Value = serverDB;
                    }
                    else if (x.Attributes["id"].Value == "database")
                    {
                        x.Attributes["value"].Value = nameDB;
                    }
                    else if (x.Attributes["id"].Value == "login")
                    {
                        x.Attributes["value"].Value = login;
                    }
                    else if (x.Attributes["id"].Value == "password")
                    {
                        x.Attributes["value"].Value = pass;
                    }
                    else if (x.Attributes["id"].Value == "accesscode")
                    {
                        if (accesscode != String.Empty && accesscode.Length >= 4)
                        {
                            x.Attributes["value"].Value = GetMd5Hash(accesscode);
                        }
                    }
                }
                //Сохраняем xml документ в файл
                connect_to_server.Save(Server.MapPath("~/") + "connection.xml");
            }

            //Редактируем строку подключения к БД в Web.config
            connect_to_server = new XmlDocument();
            if (System.IO.File.Exists(Server.MapPath("~/") + "Web.config"))
            {
                connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "Web.config"));
                connection = connect_to_server.GetElementsByTagName("connectionStrings");
                //String cs = "Data Source=MAINSERVER\SQLEXPRESS;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa;Password=Tr1nItr0N;MultipleActiveResultSets=True";
                String con_str = "Data Source=" + serverDB + ";Initial Catalog=" + nameDB + ";Persist Security Info=True;User ID=" + login + ";Password=" + pass + ";MultipleActiveResultSets=True";
                if (connection.Count > 0)
                {
                    XmlNodeList child_nodes = connection[0].ChildNodes;
                    if (child_nodes.Count > 0)
                    {

                        if (child_nodes[0].Attributes["name"].Value == "Agrochim31ConnectionString")
                        {
                            child_nodes[0].Attributes["connectionString"].Value = con_str;
                        }
                    }
                }
                connect_to_server.Save(Server.MapPath("~/") + "Web.config");
            }

            LoginFormShow("1");
            /*connStr = SetConnectionString();
            conn = new SqlConnection(connStr);*/
            ConnectionW.Close();
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

        [DirectMethod]
        public void ShowAddPlotWAllNull(String year, String tour)
        {//String count_plot,
            IdPlotTF.Text = String.Empty;
            IdPlotTF.Hidden = true;
            YearTF.Text = NullToEmpty(year);
            TourTF.Text = NullToEmpty(tour);
            DepartmentCodeTF.Text = current_code_department;
            DateInputTF.Text = String.Empty;
            DateInputTF.Hidden = true;
            DateEditTF.Text = String.Empty;
            DateEditTF.Hidden = true;

            FarmlandTF.Text = String.Empty;
            CultureTF.Text = String.Empty;
            OldCultureTF.Text = String.Empty;
            CropRotationTF.Text = String.Empty;
            SoilTF.Text = String.Empty;
            GradingTF.Text = String.Empty;
            ErosionTF.Text = String.Empty;
            SlopeTF.Text = String.Empty;
            ExposureTF.Text = String.Empty;

            EditFarmlandTF.Text = String.Empty;
            EditCultureTF.Text = String.Empty;
            EditOldCultureTF.Text = String.Empty;
            EditTypeCropRotationTF.Text = String.Empty;
            EditSoilTF.Text = String.Empty;
            EditGradingTF.Text = String.Empty;
            EditErosionTF.Text = String.Empty;
            EditNumberCropRotationTF.Text = String.Empty;
            EditFieldTF.Text = String.Empty;
            //NumberPlotTF.Text = String.Empty;
            if (GetLastPlot(current_id_department, year) != (-1))
            {
                NumberPlotTF.Text = (GetLastPlot(current_id_department, year) + 1).ToString();
            }
            else
            {
                NumberPlotTF.Text = String.Empty;
            }
            EditAreaTF.Text = String.Empty;

            NTF.Text = String.Empty;
            NO3TF.Text = String.Empty;
            HumusTF.Text = String.Empty;
            CapacityTF.Text = String.Empty;
            TotalAbsorbedBaseTF.Text = String.Empty;
            BaseSaturationTF.Text = String.Empty;
            P2O5TF.Text = String.Empty;
            K2OTF.Text = String.Empty;
            PhSTF.Text = String.Empty;
            PhWTF.Text = String.Empty;
            HydrAcidTF.Text = String.Empty;

            MnTF.Text = String.Empty;
            STF.Text = String.Empty;
            CuTF.Text = String.Empty;
            ZnTF.Text = String.Empty;
            CoTF.Text = String.Empty;
            AlTF.Text = String.Empty;
            CaTF.Text = String.Empty;
            MoTF.Text = String.Empty;
            BTF.Text = String.Empty;
            MgTF.Text = String.Empty;
            NaTF.Text = String.Empty;

            CuhmTF.Text = String.Empty;
            ZnhmTF.Text = String.Empty;
            CdhmTF.Text = String.Empty;
            PbhmTF.Text = String.Empty;
            NihmTF.Text = String.Empty;
            HghmTF.Text = String.Empty;
            MghmTF.Text = String.Empty;
            CrhmTF.Text = String.Empty;
            FehmTF.Text = String.Empty;
            FhmTF.Text = String.Empty;
            AshmTF.Text = String.Empty;

            Cs137TF.Text = String.Empty;
            Sr90TF.Text = String.Empty;
            EditSlopeTF.Text = String.Empty;
            EditExposureTF.Text = String.Empty;
            DryResidueTF.Text = String.Empty;

            DateSurvey.Value = null;
            DateSurvey.Text = String.Empty;

            EditEPlotS.RemoveAll();
            EditPlotW.Show();

            //устанавливаем фокус на первую ячейку
            EditFarmlandTF.Focus();
            EditFarmlandTF.SelectText();

            AcceptAddPlotB.Hidden = false;
            AcceptEditPlotB.Hidden = true;
        }

        [DirectMethod]
        public void ShowAddPlotW(String year, String tour, String title_farmland, String title_culture, String title_old_culture, String title_crop_rotation, String title_type_soil, String title_grading, String title_erosion, String code_farmland, String code_culture, String code_old_culture, String code_crop_rotation, String code_type_soil, String code_grading, String code_erosion, String number_crop_rotation, String number_field, String date_survey_str)
        {
            IdPlotTF.Text = String.Empty;
            IdPlotTF.Hidden = true;
            YearTF.Text = NullToEmpty(year);
            TourTF.Text = NullToEmpty(tour);
            DepartmentCodeTF.Text = NullToEmpty(current_code_department);
            DateInputTF.Text = String.Empty;
            DateInputTF.Hidden = true;
            DateEditTF.Text = String.Empty;
            DateEditTF.Hidden = true;

            FarmlandTF.Text = NullToEmpty(title_farmland);
            CultureTF.Text = NullToEmpty(title_culture);
            OldCultureTF.Text = NullToEmpty(title_old_culture);
            CropRotationTF.Text = NullToEmpty(title_crop_rotation);
            SoilTF.Text = NullToEmpty(title_type_soil);
            GradingTF.Text = NullToEmpty(title_grading);
            ErosionTF.Text = NullToEmpty(title_erosion);
            SlopeTF.Text = String.Empty;
            ExposureTF.Text = String.Empty;

            EditFarmlandTF.Text = NullToEmpty(code_farmland);
            EditCultureTF.Text = NullToEmpty(code_culture);
            EditOldCultureTF.Text = NullToEmpty(code_old_culture);
            EditTypeCropRotationTF.Text = NullToEmpty(code_crop_rotation);
            EditSoilTF.Text = NullToEmpty(code_type_soil);
            EditGradingTF.Text = NullToEmpty(code_grading);
            EditErosionTF.Text = NullToEmpty(code_erosion);
            EditNumberCropRotationTF.Text = NullToEmpty(number_crop_rotation);
            EditFieldTF.Text = NullToEmpty(number_field);
            //NumberPlotTF.Text = String.Empty;
            if (GetLastPlot(current_id_department, year) != (-1))
            {
                NumberPlotTF.Text = (GetLastPlot(current_id_department, year) + 1).ToString();
            }
            else
            {
                NumberPlotTF.Text = String.Empty;
            }
            EditAreaTF.Text = String.Empty;

            NTF.Text = String.Empty;
            NO3TF.Text = String.Empty;
            NO2TF.Text = String.Empty;
            HumusTF.Text = String.Empty;
            CapacityTF.Text = String.Empty;
            TotalAbsorbedBaseTF.Text = String.Empty;
            BaseSaturationTF.Text = String.Empty;
            P2O5TF.Text = String.Empty;
            K2OTF.Text = String.Empty;
            PhSTF.Text = String.Empty;
            PhWTF.Text = String.Empty;
            HydrAcidTF.Text = String.Empty;

            MnTF.Text = String.Empty;
            STF.Text = String.Empty;
            CuTF.Text = String.Empty;
            ZnTF.Text = String.Empty;
            CoTF.Text = String.Empty;
            AlTF.Text = String.Empty;
            CaTF.Text = String.Empty;
            MoTF.Text = String.Empty;
            BTF.Text = String.Empty;
            MgTF.Text = String.Empty;
            NaTF.Text = String.Empty;

            CuhmTF.Text = String.Empty;
            ZnhmTF.Text = String.Empty;
            CdhmTF.Text = String.Empty;
            PbhmTF.Text = String.Empty;
            NihmTF.Text = String.Empty;
            HghmTF.Text = String.Empty;
            MghmTF.Text = String.Empty;
            CrhmTF.Text = String.Empty;
            FehmTF.Text = String.Empty;
            FhmTF.Text = String.Empty;
            AshmTF.Text = String.Empty;

            Cs137TF.Text = String.Empty;
            Sr90TF.Text = String.Empty;
            EditSlopeTF.Text = String.Empty;
            EditExposureTF.Text = String.Empty;
            DryResidueTF.Text = String.Empty;

            if (NullToEmpty(date_survey_str) != String.Empty)
            {
                String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                if (date_survey_str.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                DateTime date_survey = DateTime.ParseExact(date_survey_str, format_date_time, System.Globalization.CultureInfo.InvariantCulture);
                DateSurvey.Value = date_survey;
            }
            else
            {
                DateSurvey.Value = null;
                DateSurvey.Text = String.Empty;
            }

            EditEPlotS.RemoveAll();
            EditPlotW.Show();

            //устанавливаем фокус на номер участка
            EditAreaTF.Focus();
            EditAreaTF.SelectText();

            AcceptAddPlotB.Hidden = false;
            AcceptEditPlotB.Hidden = true;
        }

        [DirectMethod]
        public void ShowEditPlotW(String id_plot, String year, String tour, String date_input, String date_last_edit, String title_farmland, String title_culture, String title_old_culture, String title_crop_rotation, String title_type_soil, String title_grading, String title_erosion, String title_method, String code_farmland, String code_culture, String code_old_culture, String code_crop_rotation, String code_type_soil, String code_grading, String code_erosion, String number_crop_rotation, String number_field, String n, String no3, String no2, String humus, String capacity, String tab, String base_s, String p2o5, String k2o, String ph_s, String ph_w, String ha, String mn, String s, String cu, String zn, String co, String al, String ca, String mo, String b, String mg, String na, String cu_tm, String zn_tm, String cd_tm, String pb_tm, String ni_tm, String hg_tm, String mg_tm, String cr_tm, String fe_tm, String f_tm, String as_tm, String cs137, String sr90, String code_slope, String title_slope, String code_exposure, String title_exposure, String number_plot, String area, String date_survey_str, String dry_residue)
        {
            if (!user_reg_data.edit) { return; }

            if (connection_try)
            {
                FlagEditing plotFE = new FlagEditing(conn, "Plot", Convert.ToInt32(id_plot));
                if (plotFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Участок редактируется другим пользователем!").Show();
                    return;
                }
                plotFE.SetFlag();
                conn.Open();
                adapterEPlot = new SqlDataAdapter(selectCommEPlot + " WHERE id_plot=" + id_plot, conn);
                adapterEPlot.Fill(indexDS, "Elementary_plot");
                indexDV = new System.Data.DataView(indexDS.Tables["Elementary_plot"]);
                EditEPlotS.DataSource = indexDV;
                EditEPlotS.DataBind();
                conn.Close();
            }

            //обработка раскрашиваемых элементов
            ph_s = ph_s.Split('|')[0].Replace('.', ',');
            p2o5 = p2o5.Split('|')[0].Replace('.', ',');
            k2o = k2o.Split('|')[0].Replace('.', ',');
            ha = ha.Split('|')[0].Replace('.', ',');
            //----------------------------------

            IdPlotTF.Text = NullToEmpty(id_plot);
            IdPlotTF.Hidden = true;
            YearTF.Text = NullToEmpty(year);
            TourTF.Text = NullToEmpty(tour);
            DepartmentCodeTF.Text = NullToEmpty(current_code_department);
            MethodTF.Text = NullToEmpty(title_method);
            DateInputTF.Hidden = false;
            DateEditTF.Hidden = false;
            DateInputTF.Text = NullToEmpty(date_input);
            DateEditTF.Text = NullToEmpty(date_last_edit);

            FarmlandTF.Text = NullToEmpty(title_farmland);
            CultureTF.Text = NullToEmpty(title_culture);
            OldCultureTF.Text = NullToEmpty(title_old_culture);
            CropRotationTF.Text = NullToEmpty(title_crop_rotation);
            SoilTF.Text = NullToEmpty(title_type_soil);
            GradingTF.Text = NullToEmpty(title_grading);
            ErosionTF.Text = NullToEmpty(title_erosion);
            SlopeTF.Text = NullToEmpty(title_slope);
            ExposureTF.Text = NullToEmpty(title_exposure);

            EditFarmlandTF.Text = NullToEmpty(code_farmland);
            EditCultureTF.Text = NullToEmpty(code_culture);
            EditOldCultureTF.Text = NullToEmpty(code_old_culture);
            EditTypeCropRotationTF.Text = NullToEmpty(code_crop_rotation);
            EditSoilTF.Text = NullToEmpty(code_type_soil);
            EditGradingTF.Text = NullToEmpty(code_grading);
            EditErosionTF.Text = NullToEmpty(code_erosion);
            EditNumberCropRotationTF.Text = NullToEmpty(number_crop_rotation);
            EditFieldTF.Text = NullToEmpty(number_field);
            NumberPlotTF.Text = NullToEmpty(number_plot);
            EditAreaTF.Text = NullToEmpty(area.Replace('.', ','));

            NTF.Text = NullToEmpty(n);
            NO3TF.Text = NullToEmpty(no3);
            NO2TF.Text = NullToEmpty(no2);
            HumusTF.Text = NullToEmpty(humus);
            CapacityTF.Text = NullToEmpty(capacity);
            TotalAbsorbedBaseTF.Text = NullToEmpty(tab);
            BaseSaturationTF.Text = NullToEmpty(base_s);
            P2O5TF.Text = NullToEmpty(p2o5);
            K2OTF.Text = NullToEmpty(k2o);
            PhSTF.Text = NullToEmpty(ph_s);
            PhWTF.Text = NullToEmpty(ph_w);
            HydrAcidTF.Text = NullToEmpty(ha);

            MnTF.Text = NullToEmpty(mn);
            STF.Text = NullToEmpty(s);
            CuTF.Text = NullToEmpty(cu);
            ZnTF.Text = NullToEmpty(zn);
            CoTF.Text = NullToEmpty(co);
            AlTF.Text = NullToEmpty(al);
            CaTF.Text = NullToEmpty(ca);
            MoTF.Text = NullToEmpty(mo);
            BTF.Text = NullToEmpty(b);
            MgTF.Text = NullToEmpty(mg);
            NaTF.Text = NullToEmpty(na);

            CuhmTF.Text = NullToEmpty(cu_tm);
            ZnhmTF.Text = NullToEmpty(zn_tm);
            CdhmTF.Text = NullToEmpty(cd_tm);
            PbhmTF.Text = NullToEmpty(pb_tm);
            NihmTF.Text = NullToEmpty(ni_tm);
            HghmTF.Text = NullToEmpty(hg_tm);
            MghmTF.Text = NullToEmpty(mg_tm);
            CrhmTF.Text = NullToEmpty(cr_tm);
            FehmTF.Text = NullToEmpty(fe_tm);
            FhmTF.Text = NullToEmpty(f_tm);
            AshmTF.Text = NullToEmpty(as_tm);

            Cs137TF.Text = NullToEmpty(cs137);
            Sr90TF.Text = NullToEmpty(sr90);
            EditSlopeTF.Text = NullToEmpty(code_slope);
            EditExposureTF.Text = NullToEmpty(code_exposure);
            IdPlotTF.Text = NullToEmpty(id_plot);
            DepartmentCodeTF.Text = NullToEmpty(current_code_department);
            DryResidueTF.Text = NullToEmpty(dry_residue);

            if (NullToEmpty(date_survey_str) != String.Empty)
            {
                String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                if (date_survey_str.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                DateTime date_survey = DateTime.ParseExact(date_survey_str, format_date_time, System.Globalization.CultureInfo.InvariantCulture);
                DateSurvey.Value = date_survey;
            }
            else
            {
                DateSurvey.Value = null;
                DateSurvey.Text = String.Empty;
            }

            EditPlotW.Show();
            //устанавливаем фокус на площадь
            NTF.Focus();
            NTF.SelectText();

            AcceptAddPlotB.Hidden = true;
            AcceptEditPlotB.Hidden = false;
        }
        //проверка значений на null
        public String NotNull(String value)
        {
            if (value != null && String.Compare(value, "null") != 0 && value != String.Empty)
            {
                value = value.Replace('.', ',');
            }
            return ((value == String.Empty || value == null || String.Compare(value, "null") == 0) ? "0" : value);
        }

        public String NotNullText(String value)
        {
            return ((value == String.Empty || value == null || String.Compare(value, "null") == 0) ? "0" : value);
        }

        [DirectMethod]
        public void AddPlot(String number_plot, String area, String year, String tour, String code_slope, String code_exposure, String code_type_soil, String code_grading, String code_erosion, String code_farmland, String code_culture, String code_old_culture, String code_crop_rotation, String number_crop_rotation, String number_field, String n, String no3, String no2, String p2o5, String k2o, String ph_s, String ph_w, String s, String humus, String hydrolytic_acid, String tab, String ca, String mn, String mo, String b, String cu, String mg, String zn, String na, String co, String al, String fe, String cu_hm, String zn_hm, String cd_hm, String pb_hm, String ni_hm, String hg_hm, String mg_hm, String cr_hm, String fe_hm, String f_hm, String as_hm, String cs137, String sr90, String data_eplots, String date_survey_str, String dry_residue)
        // public void AddPlot(String number_plot,String area,String year, String tour, String id_user, String slope, String code_exposure, String code_type_soil, String code_grading, String code_erosion, String code_farmland, String code_culture, String code_crop_rotation, String number_crop_rotation, String number_field, String n, String no3, String no2, String p2o5, String k2o, String ph_s, String ph_w, String s, String humus, String hydrolytic_acid, String capacity, String tab, String base_saturation, String ca, String mn, String mo, String b, String cu, String mg, String zn, String na, String co, String al, String fe, String cu_hm, String zn_hm, String cd_hm, String pb_hm, String ni_hm, String hg_hm, String mg_hm, String cr_hm, String fe_hm, String f_hm, String as_hm, String cs137, String sr90)
        {
            if (connection_try)
            {
                String result_plot = "0", result_eplot = "0";
                /*adapterPlot = new SqlDataAdapter(selectCommPlot + " WHERE id_department=" + current_id_department,conn);
                adapterPlot.Fill(indexDS, "Plot");
                System.Data.DataView dv_plot = new System.Data.DataView(indexDS.Tables["Plot"]);*/
                /*if (Request.Browser.Cookies)
                {
                    cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                    id_user = cookie_login_user["id_user"].ToString();
                    //CookieLoginSplit(user_reg_data, cookie_login_user);
                }*/
                if ((area != String.Empty) && (year != String.Empty) && (tour != String.Empty) && (number_field != String.Empty)) //проверка ввода обязательных полей!
                {
                    Int64 date_survey_long = 0;
                    if (date_survey_str != String.Empty && date_survey_str != null && date_survey_str != "null")
                    {
                        date_survey_str = date_survey_str.Replace("\"", "");
                        String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                        if (date_survey_str.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                        date_survey_long = DateTime.ParseExact(date_survey_str, format_date_time, System.Globalization.CultureInfo.InvariantCulture).Ticks;
                    }
                    if (number_plot == String.Empty) { number_plot = "0"; }
                    conn.Open();
                    SqlCommand add_plot = new SqlCommand("Add_Plot", conn);
                    add_plot.CommandType = CommandType.StoredProcedure;
                    add_plot.CommandTimeout = 300;
                    add_plot.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                    add_plot.Parameters.AddWithValue("@code_department", current_code_department);
                    add_plot.Parameters.AddWithValue("@number_plot", number_plot);
                    add_plot.Parameters.AddWithValue("@area", float.Parse(NotNull(area)));
                    add_plot.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                    add_plot.Parameters.AddWithValue("@tour", Convert.ToInt32(tour));
                    add_plot.Parameters.AddWithValue("@date_survey_long", date_survey_long);
                    add_plot.Parameters.AddWithValue("@id_user", user_reg_data.id_user);// id_user передавать

                    add_plot.Parameters.AddWithValue("@code_slope", Convert.ToInt32(NotNull(code_slope)));
                    add_plot.Parameters.AddWithValue("@code_exposure", Convert.ToInt32(NotNull(code_exposure)));

                    add_plot.Parameters.AddWithValue("@code_type_soil", Convert.ToInt32(NotNull(code_type_soil)));
                    add_plot.Parameters.AddWithValue("@code_grading", Convert.ToInt32(NotNull(code_grading)));
                    add_plot.Parameters.AddWithValue("@code_erosion", Convert.ToInt32(NotNull(code_erosion)));

                    add_plot.Parameters.AddWithValue("@code_farmland", Convert.ToInt32(NotNull(code_farmland)));
                    add_plot.Parameters.AddWithValue("@code_culture", Convert.ToInt32(NotNull(code_culture)));
                    add_plot.Parameters.AddWithValue("@code_old_culture", Convert.ToInt32(NotNull(code_old_culture)));
                    add_plot.Parameters.AddWithValue("@code_crop_rotation", Convert.ToInt32(NotNull(code_crop_rotation)));
                    add_plot.Parameters.AddWithValue("@number_crop_rotation", Convert.ToInt32(NotNull(number_crop_rotation)));
                    add_plot.Parameters.AddWithValue("@number_field", number_field);

                    add_plot.Parameters.AddWithValue("@n", float.Parse(NotNull(n)));
                    add_plot.Parameters.AddWithValue("@no3", float.Parse(NotNull(no3)));
                    add_plot.Parameters.AddWithValue("@no2", float.Parse(NotNull(no2)));
                    add_plot.Parameters.AddWithValue("@p2o5", float.Parse(NotNull(p2o5)));
                    add_plot.Parameters.AddWithValue("@k2o", float.Parse(NotNull(k2o)));
                    add_plot.Parameters.AddWithValue("@ph_s", float.Parse(NotNull(ph_s)));
                    add_plot.Parameters.AddWithValue("@ph_w", float.Parse(NotNull(ph_w)));
                    add_plot.Parameters.AddWithValue("@s", float.Parse(NotNull(s)));
                    add_plot.Parameters.AddWithValue("@humus", float.Parse(NotNull(humus)));
                    add_plot.Parameters.AddWithValue("@hydrolytic_acid", float.Parse(NotNull(hydrolytic_acid)));
                    add_plot.Parameters.AddWithValue("@total_absorbed_bases", float.Parse(NotNull(tab)));
                    add_plot.Parameters.AddWithValue("@dry_residue", float.Parse(NotNull(dry_residue)));

                    add_plot.Parameters.AddWithValue("@ca", float.Parse(NotNull(ca)));
                    add_plot.Parameters.AddWithValue("@mn", float.Parse(NotNull(mn)));
                    add_plot.Parameters.AddWithValue("@mo", float.Parse(NotNull(mo)));
                    add_plot.Parameters.AddWithValue("@b", float.Parse(NotNull(b)));
                    add_plot.Parameters.AddWithValue("@cu", float.Parse(NotNull(cu)));
                    add_plot.Parameters.AddWithValue("@mg", float.Parse(NotNull(mg)));
                    add_plot.Parameters.AddWithValue("@zn", float.Parse(NotNull(zn)));
                    add_plot.Parameters.AddWithValue("@na", float.Parse(NotNull(na)));
                    add_plot.Parameters.AddWithValue("@co", float.Parse(NotNull(co)));
                    add_plot.Parameters.AddWithValue("@al", float.Parse(NotNull(al)));
                    add_plot.Parameters.AddWithValue("@fe", float.Parse(NotNull(fe)));

                    add_plot.Parameters.AddWithValue("@cu_hm", float.Parse(NotNull(cu_hm)));
                    add_plot.Parameters.AddWithValue("@zn_hm", float.Parse(NotNull(zn_hm)));
                    add_plot.Parameters.AddWithValue("@cd_hm", float.Parse(NotNull(cd_hm)));
                    add_plot.Parameters.AddWithValue("@pb_hm", float.Parse(NotNull(pb_hm)));
                    add_plot.Parameters.AddWithValue("@ni_hm", float.Parse(NotNull(ni_hm)));
                    add_plot.Parameters.AddWithValue("@hg_hm", float.Parse(NotNull(hg_hm)));
                    add_plot.Parameters.AddWithValue("@mg_hm", float.Parse(NotNull(mg_hm)));
                    add_plot.Parameters.AddWithValue("@cr_hm", float.Parse(NotNull(cr_hm)));
                    add_plot.Parameters.AddWithValue("@fe_hm", float.Parse(NotNull(fe_hm)));
                    add_plot.Parameters.AddWithValue("@f_hm", float.Parse(NotNull(f_hm)));
                    add_plot.Parameters.AddWithValue("@as_hm", float.Parse(NotNull(as_hm)));

                    add_plot.Parameters.AddWithValue("@cs137", float.Parse(NotNull(cs137)));
                    add_plot.Parameters.AddWithValue("@sr90", float.Parse(NotNull(sr90)));

                    add_plot.Parameters.Add("@id_plot", SqlDbType.Int);
                    add_plot.Parameters.Add("@code_plot", SqlDbType.VarChar, 30);
                    add_plot.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                    add_plot.Parameters["@id_plot"].Direction = ParameterDirection.Output;
                    add_plot.Parameters["@code_plot"].Direction = ParameterDirection.Output;
                    add_plot.Parameters["@result"].Direction = ParameterDirection.Output;

                    add_plot.ExecuteNonQuery();

                    Int32 id_plot = Convert.ToInt32(add_plot.Parameters["@id_plot"].Value);
                    String code_plot = add_plot.Parameters["@code_plot"].Value.ToString();
                    result_plot = add_plot.Parameters["@result"].Value.ToString();

                    if (result_plot == "0")
                    {
                        List<Dictionary<String, String>> records_eplots = JSON.Deserialize<List<Dictionary<String, String>>>(data_eplots);
                        if (records_eplots.Count > 0)
                        {
                            //выборка новых элементарных участков
                            Int32[] new_eplots = new Int32[records_eplots.Count];
                            for (int c = 0; c < records_eplots.Count; c++)
                            {
                                new_eplots[c] = Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"]));
                            }
                            //сортировка и добавление новых элементарных участков
                            Array.Sort(new_eplots);
                            for (int i = (new_eplots.Length - 1); i >= 0; i--)
                            {
                                for (int c = 0; c < records_eplots.Count; c++)
                                {
                                    if (Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])) == new_eplots[i])
                                    {
                                        if (records_eplots[c]["ph_s"] != String.Empty || records_eplots[c]["p2o5"] != String.Empty ||
                                           records_eplots[c]["k2o"] != String.Empty || records_eplots[c]["hydrolytic_acid"] != String.Empty)
                                        {
                                            /*ICollection<String> keys = records_eplots[c].Keys;
                                            String[] keys_string = new String[keys.Count];
                                            keys.CopyTo(keys_string, 0);
                                            keys_string[j] records_eplots[c][keys_string[j]]*/
                                            SqlCommand add_eplot = new SqlCommand("Add_Eplot", conn);
                                            add_eplot.CommandType = CommandType.StoredProcedure;
                                            add_eplot.CommandTimeout = 300;
                                            add_eplot.Parameters.AddWithValue("@id_plot", Convert.ToInt32(id_plot));
                                            add_eplot.Parameters.AddWithValue("@code_plot", code_plot);
                                            //основные данные
                                            add_eplot.Parameters.AddWithValue("@number_elementary_plot", NotNull(records_eplots[c]["number_elementary_plot"]));
                                            add_eplot.Parameters.AddWithValue("@code_type_soil", float.Parse(NotNull(records_eplots[c]["code_type_soil"])));
                                            add_eplot.Parameters.AddWithValue("@code_grading", float.Parse(NotNull(records_eplots[c]["code_grading"])));
                                            add_eplot.Parameters.AddWithValue("@code_erosion", float.Parse(NotNull(records_eplots[c]["code_erosion"])));
                                            add_eplot.Parameters.AddWithValue("@code_slope", float.Parse(NotNull(records_eplots[c]["code_slope"])));
                                            add_eplot.Parameters.AddWithValue("@code_exposure", float.Parse(NotNull(records_eplots[c]["code_exposure"])));
                                            //макроэлементы
                                            add_eplot.Parameters.AddWithValue("@n", float.Parse(NotNull(records_eplots[c]["n"])));
                                            add_eplot.Parameters.AddWithValue("@no3", float.Parse(NotNull(records_eplots[c]["no3"])));
                                            add_eplot.Parameters.AddWithValue("@no2", float.Parse(NotNull(records_eplots[c]["no2"])));
                                            add_eplot.Parameters.AddWithValue("@p2o5", float.Parse(NotNull(records_eplots[c]["p2o5"])));
                                            add_eplot.Parameters.AddWithValue("@k2o", float.Parse(NotNull(records_eplots[c]["k2o"])));
                                            add_eplot.Parameters.AddWithValue("@ph_s", float.Parse(NotNull(records_eplots[c]["ph_s"])));
                                            add_eplot.Parameters.AddWithValue("@ph_w", float.Parse(NotNull(records_eplots[c]["ph_w"])));
                                            add_eplot.Parameters.AddWithValue("@s", float.Parse(NotNull(records_eplots[c]["s"])));
                                            add_eplot.Parameters.AddWithValue("@humus", float.Parse(NotNull(records_eplots[c]["humus"])));
                                            add_eplot.Parameters.AddWithValue("@hydrolytic_acid", float.Parse(NotNull(records_eplots[c]["hydrolytic_acid"])));
                                            add_eplot.Parameters.AddWithValue("@total_absorbed_bases", float.Parse(NotNull(records_eplots[c]["total_absorbed_bases"])));
                                            //микроэлементы
                                            add_eplot.Parameters.AddWithValue("@ca", float.Parse(NotNull(records_eplots[c]["ca"])));
                                            add_eplot.Parameters.AddWithValue("@mn", float.Parse(NotNull(records_eplots[c]["mn"])));
                                            add_eplot.Parameters.AddWithValue("@mo", float.Parse(NotNull(records_eplots[c]["mo"])));
                                            add_eplot.Parameters.AddWithValue("@b", float.Parse(NotNull(records_eplots[c]["b"])));
                                            add_eplot.Parameters.AddWithValue("@cu", float.Parse(NotNull(records_eplots[c]["cu"])));
                                            add_eplot.Parameters.AddWithValue("@mg", float.Parse(NotNull(records_eplots[c]["mg"])));
                                            add_eplot.Parameters.AddWithValue("@zn", float.Parse(NotNull(records_eplots[c]["zn"])));
                                            add_eplot.Parameters.AddWithValue("@na", float.Parse(NotNull(records_eplots[c]["na"])));
                                            add_eplot.Parameters.AddWithValue("@co", float.Parse(NotNull(records_eplots[c]["co"])));
                                            add_eplot.Parameters.AddWithValue("@al", float.Parse(NotNull(records_eplots[c]["al"])));
                                            add_eplot.Parameters.AddWithValue("@fe", float.Parse(NotNull(records_eplots[c]["fe"])));
                                            //тяжёлые металлы
                                            add_eplot.Parameters.AddWithValue("@cu_hm", float.Parse(NotNull(records_eplots[c]["cu_hm"])));
                                            add_eplot.Parameters.AddWithValue("@zn_hm", float.Parse(NotNull(records_eplots[c]["zn_hm"])));
                                            add_eplot.Parameters.AddWithValue("@cd_hm", float.Parse(NotNull(records_eplots[c]["cd_hm"])));
                                            add_eplot.Parameters.AddWithValue("@pb_hm", float.Parse(NotNull(records_eplots[c]["pb_hm"])));
                                            add_eplot.Parameters.AddWithValue("@ni_hm", float.Parse(NotNull(records_eplots[c]["ni_hm"])));
                                            add_eplot.Parameters.AddWithValue("@hg_hm", float.Parse(NotNull(records_eplots[c]["hg_hm"])));
                                            add_eplot.Parameters.AddWithValue("@mg_hm", float.Parse(NotNull(records_eplots[c]["mg_hm"])));
                                            add_eplot.Parameters.AddWithValue("@cr_hm", float.Parse(NotNull(records_eplots[c]["cr_hm"])));
                                            add_eplot.Parameters.AddWithValue("@fe_hm", float.Parse(NotNull(records_eplots[c]["fe_hm"])));
                                            add_eplot.Parameters.AddWithValue("@f_hm", float.Parse(NotNull(records_eplots[c]["f_hm"])));
                                            add_eplot.Parameters.AddWithValue("@as_hm", float.Parse(NotNull(records_eplots[c]["as_hm"])));
                                            //радиоэлементы
                                            add_eplot.Parameters.AddWithValue("@cs137", float.Parse(NotNull(records_eplots[c]["cs137"])));
                                            add_eplot.Parameters.AddWithValue("@sr90", float.Parse(NotNull(records_eplots[c]["sr90"])));
                                            //результат
                                            add_eplot.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                                            add_eplot.Parameters["@result"].Direction = ParameterDirection.Output;
                                            add_eplot.ExecuteNonQuery();
                                            result_eplot = add_eplot.Parameters["@result"].Value.ToString();
                                            if (result_eplot != "0")
                                            {
                                                X.Msg.Show(new MessageBoxConfig
                                                {
                                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                                    Title = "Результат добавления элементарных участков",
                                                    Message = "Некоторые элементарные участки не добавлены! Обратитесь к администратору.\n" + result_eplot,
                                                    Buttons = MessageBox.Button.OK,
                                                    Icon = MessageBox.Icon.INFO
                                                });
                                            }
                                        }
                                    }
                                    //17092013 добавить координаты
                                }
                            }
                        }
                    }
                    else
                    {
                        //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Результат добавления участка",
                            Message = "Участок не добавлен! Обратитесь к администратору.\n" + result_plot,
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    //Обновляем участок по элементарным участкам
                    SqlCommand update_plot = new SqlCommand("Update_Plot_From_EPlot", conn);
                    update_plot.CommandType = CommandType.StoredProcedure;
                    update_plot.Parameters.AddWithValue("@code_plot", code_plot);
                    update_plot.ExecuteNonQuery();
                    conn.Close();
                    //Добавить проверку на результат
                    //FillTour();
                    //FillYear("");
                    FillPlot(year);
                    //FillPlot(GetMaxYear());
                    /*Int32 count_tour = ToursGP.Items.Count() - 1;
                    if (count_tour > 0) { ToursGP.GetSelectionModel().Select(count_tour); }
                    Int32 count_year = YearsGP.Items.Count() - 1;
                    if (count_year > 0) { YearsGP.GetSelectionModel().Select(count_year); }*/
                    EditPlotW.Close();
                    AddPlotB.Focus();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Внимание!!!",
                        Message = "Заполнены не все обязательные поля!\nЗаполните поля и повторите действие.",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
            }
        }

        [DirectMethod]
        public void EditPlot(String id_plot, String number_plot, String area, String year, String tour, String code_slope, String code_exposure, String code_type_soil, String code_grading, String code_erosion, String code_farmland, String code_culture, String code_old_culture, String code_crop_rotation, String number_crop_rotation, String number_field, String n, String no3, String no2, String p2o5, String k2o, String ph_s, String ph_w, String s, String humus, String hydrolytic_acid, String tab, String ca, String mn, String mo, String b, String cu, String mg, String zn, String na, String co, String al, String fe, String cu_hm, String zn_hm, String cd_hm, String pb_hm, String ni_hm, String hg_hm, String mg_hm, String cr_hm, String fe_hm, String f_hm, String as_hm, String cs137, String sr90, String data_eplots, String date_survey_str, String dry_residue)
        {
            if (connection_try)
            {
                String result_plot = "0", result_eplot = "0";
                /*adapterPlot = new SqlDataAdapter(selectCommPlot + " WHERE id_department=" + current_id_department, conn);
                adapterPlot.Fill(indexDS, "Plot");
                System.Data.DataView dv_plot = new System.Data.DataView(indexDS.Tables["Plot"]);*/
                /*if (Request.Browser.Cookies)
                {
                    cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                    id_user = cookie_login_user["id_user"].ToString();
                    //CookieLoginSplit(user_reg_data, cookie_login_user);
                }*/
                if ((id_plot != String.Empty) && (area != String.Empty) && (year != String.Empty) && (tour != String.Empty) && (number_field != String.Empty)) //проверка ввода обязательных полей!
                {
                    Int64 date_survey_long = 0;
                    if (date_survey_str != String.Empty && date_survey_str != null && date_survey_str != "null")
                    {
                        date_survey_str = date_survey_str.Replace("\"", "");
                        String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                        if (date_survey_str.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                        date_survey_long = DateTime.ParseExact(date_survey_str, format_date_time, System.Globalization.CultureInfo.InvariantCulture).Ticks;
                    }
                    if (number_plot == String.Empty) { number_plot = "0"; }
                    conn.Open();
                    SqlCommand edit_plot = new SqlCommand("Edit_Plot", conn);
                    edit_plot.CommandType = CommandType.StoredProcedure;
                    edit_plot.CommandTimeout = 300;
                    edit_plot.Parameters.AddWithValue("@id_plot", Convert.ToInt32(id_plot));
                    edit_plot.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                    edit_plot.Parameters.AddWithValue("@code_department", current_code_department);
                    edit_plot.Parameters.AddWithValue("@number_plot", number_plot);
                    edit_plot.Parameters.AddWithValue("@area", float.Parse(NotNull(area)));
                    edit_plot.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                    edit_plot.Parameters.AddWithValue("@tour", Convert.ToInt32(tour));
                    edit_plot.Parameters.AddWithValue("@date_survey_long", date_survey_long);
                    edit_plot.Parameters.AddWithValue("@id_user", user_reg_data.id_user);// id_user передавать

                    edit_plot.Parameters.AddWithValue("@code_slope", Convert.ToInt32(NotNull(code_slope)));
                    edit_plot.Parameters.AddWithValue("@code_exposure", Convert.ToInt32(NotNull(code_exposure)));

                    edit_plot.Parameters.AddWithValue("@code_type_soil", Convert.ToInt32(NotNull(code_type_soil)));
                    edit_plot.Parameters.AddWithValue("@code_grading", Convert.ToInt32(NotNull(code_grading)));
                    edit_plot.Parameters.AddWithValue("@code_erosion", Convert.ToInt32(NotNull(code_erosion)));

                    edit_plot.Parameters.AddWithValue("@code_farmland", Convert.ToInt32(NotNull(code_farmland)));
                    edit_plot.Parameters.AddWithValue("@code_culture", Convert.ToInt32(NotNull(code_culture)));
                    edit_plot.Parameters.AddWithValue("@code_old_culture", Convert.ToInt32(NotNull(code_old_culture)));
                    edit_plot.Parameters.AddWithValue("@code_crop_rotation", Convert.ToInt32(NotNull(code_crop_rotation)));
                    edit_plot.Parameters.AddWithValue("@number_crop_rotation", Convert.ToInt32(NotNull(number_crop_rotation)));
                    edit_plot.Parameters.AddWithValue("@number_field", number_field);

                    edit_plot.Parameters.AddWithValue("@n", float.Parse(NotNull(n)));
                    edit_plot.Parameters.AddWithValue("@no3", float.Parse(NotNull(no3)));
                    edit_plot.Parameters.AddWithValue("@no2", float.Parse(NotNull(no2)));
                    edit_plot.Parameters.AddWithValue("@p2o5", float.Parse(NotNull(p2o5)));
                    edit_plot.Parameters.AddWithValue("@k2o", float.Parse(NotNull(k2o)));
                    edit_plot.Parameters.AddWithValue("@ph_s", float.Parse(NotNull(ph_s)));
                    edit_plot.Parameters.AddWithValue("@ph_w", float.Parse(NotNull(ph_w)));
                    edit_plot.Parameters.AddWithValue("@s", float.Parse(NotNull(s)));
                    edit_plot.Parameters.AddWithValue("@humus", float.Parse(NotNull(humus)));
                    edit_plot.Parameters.AddWithValue("@hydrolytic_acid", float.Parse(NotNull(hydrolytic_acid)));
                    edit_plot.Parameters.AddWithValue("@total_absorbed_bases", float.Parse(NotNull(tab)));
                    edit_plot.Parameters.AddWithValue("@dry_residue", float.Parse(NotNull(dry_residue)));

                    edit_plot.Parameters.AddWithValue("@ca", float.Parse(NotNull(ca)));
                    edit_plot.Parameters.AddWithValue("@mn", float.Parse(NotNull(mn)));
                    edit_plot.Parameters.AddWithValue("@mo", float.Parse(NotNull(mo)));
                    edit_plot.Parameters.AddWithValue("@b", float.Parse(NotNull(b)));
                    edit_plot.Parameters.AddWithValue("@cu", float.Parse(NotNull(cu)));
                    edit_plot.Parameters.AddWithValue("@mg", float.Parse(NotNull(mg)));
                    edit_plot.Parameters.AddWithValue("@zn", float.Parse(NotNull(zn)));
                    edit_plot.Parameters.AddWithValue("@na", float.Parse(NotNull(na)));
                    edit_plot.Parameters.AddWithValue("@co", float.Parse(NotNull(co)));
                    edit_plot.Parameters.AddWithValue("@al", float.Parse(NotNull(al)));
                    edit_plot.Parameters.AddWithValue("@fe", float.Parse(NotNull(fe)));

                    edit_plot.Parameters.AddWithValue("@cu_hm", float.Parse(NotNull(cu_hm)));
                    edit_plot.Parameters.AddWithValue("@zn_hm", float.Parse(NotNull(zn_hm)));
                    edit_plot.Parameters.AddWithValue("@cd_hm", float.Parse(NotNull(cd_hm)));
                    edit_plot.Parameters.AddWithValue("@pb_hm", float.Parse(NotNull(pb_hm)));
                    edit_plot.Parameters.AddWithValue("@ni_hm", float.Parse(NotNull(ni_hm)));
                    edit_plot.Parameters.AddWithValue("@hg_hm", float.Parse(NotNull(hg_hm)));
                    edit_plot.Parameters.AddWithValue("@mg_hm", float.Parse(NotNull(mg_hm)));
                    edit_plot.Parameters.AddWithValue("@cr_hm", float.Parse(NotNull(cr_hm)));
                    edit_plot.Parameters.AddWithValue("@fe_hm", float.Parse(NotNull(fe_hm)));
                    edit_plot.Parameters.AddWithValue("@f_hm", float.Parse(NotNull(f_hm)));
                    edit_plot.Parameters.AddWithValue("@as_hm", float.Parse(NotNull(as_hm)));

                    edit_plot.Parameters.AddWithValue("@cs137", float.Parse(NotNull(cs137)));
                    edit_plot.Parameters.AddWithValue("@sr90", float.Parse(NotNull(sr90)));

                    edit_plot.Parameters.Add("@code_plot", SqlDbType.VarChar, 30);
                    edit_plot.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                    edit_plot.Parameters["@code_plot"].Direction = ParameterDirection.Output;
                    edit_plot.Parameters["@result"].Direction = ParameterDirection.Output;
                    edit_plot.ExecuteNonQuery();

                    String code_plot = edit_plot.Parameters["@code_plot"].Value.ToString();
                    result_plot = edit_plot.Parameters["@result"].Value.ToString();

                    if (result_plot == "0")
                    {
                        //удаление элементарных участков
                        if (Request.Browser.Cookies)
                        {
                            cookie_eplot = Request.Cookies["Agrochim31_Eplot"];
                            if (cookie_eplot != null)
                            {
                                String[] del_eplots = CookieEPlotSplit(cookie_eplot);
                                Response.Cookies["Agrochim31_Eplot"].Expires = DateTime.Now.AddHours(-1);
                                for (int i = 0; i < del_eplots.Length; i++)
                                {
                                    SqlCommand delete_eplot = new SqlCommand("Delete_Elementary_Plot", conn);
                                    delete_eplot.CommandType = CommandType.StoredProcedure;
                                    delete_eplot.CommandTimeout = 300;
                                    delete_eplot.Parameters.AddWithValue("@id_elementary_plot", Convert.ToInt32(NotNull(del_eplots[i])));
                                    delete_eplot.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Title = "Ошибка cookies",
                                Message = "Необходимо включить поддержку cookies!!!",
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.INFO
                            });
                        }
                        //получение списка всех элементартных участков
                        List<Dictionary<String, String>> records_eplots = JSON.Deserialize<List<Dictionary<String, String>>>(data_eplots);
                        if (records_eplots.Count > 0)
                        {
                            //изменение существующих элементартных участков
                            for (int c = 0; c < records_eplots.Count; c++)
                            {
                                if (Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])) > 0)
                                {
                                    if (records_eplots[c]["ph_s"] != String.Empty || records_eplots[c]["p2o5"] != String.Empty ||
                                           records_eplots[c]["k2o"] != String.Empty || records_eplots[c]["hydrolytic_acid"] != String.Empty)
                                    {
                                        /*ICollection<String> keys = records_eplots[c].Keys;
                                        String[] keys_string = new String[keys.Count];
                                        keys.CopyTo(keys_string, 0);
                                        keys_string[j] records_eplots[c][keys_string[j]]*/
                                        SqlCommand add_edit_eplot = new SqlCommand("Add_Edit_EPlot", conn);
                                        add_edit_eplot.CommandType = CommandType.StoredProcedure;
                                        add_edit_eplot.CommandTimeout = 300;
                                        add_edit_eplot.Parameters.AddWithValue("@id_plot", Convert.ToInt32(id_plot));
                                        add_edit_eplot.Parameters.AddWithValue("@code_plot", code_plot);
                                        //основные данные
                                        add_edit_eplot.Parameters.AddWithValue("@id_elementary_plot", Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])));
                                        add_edit_eplot.Parameters.AddWithValue("@number_elementary_plot", NotNull(records_eplots[c]["number_elementary_plot"]));
                                        add_edit_eplot.Parameters.AddWithValue("@code_type_soil", float.Parse(NotNull(records_eplots[c]["code_type_soil"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_grading", float.Parse(NotNull(records_eplots[c]["code_grading"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_erosion", float.Parse(NotNull(records_eplots[c]["code_erosion"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_slope", float.Parse(NotNull(records_eplots[c]["code_slope"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_exposure", float.Parse(NotNull(records_eplots[c]["code_exposure"])));
                                        //макроэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@n", float.Parse(NotNull(records_eplots[c]["n"])));
                                        add_edit_eplot.Parameters.AddWithValue("@no3", float.Parse(NotNull(records_eplots[c]["no3"])));
                                        add_edit_eplot.Parameters.AddWithValue("@no2", float.Parse(NotNull(records_eplots[c]["no2"])));
                                        add_edit_eplot.Parameters.AddWithValue("@p2o5", float.Parse(NotNull(records_eplots[c]["p2o5"])));
                                        add_edit_eplot.Parameters.AddWithValue("@k2o", float.Parse(NotNull(records_eplots[c]["k2o"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ph_s", float.Parse(NotNull(records_eplots[c]["ph_s"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ph_w", float.Parse(NotNull(records_eplots[c]["ph_w"])));
                                        add_edit_eplot.Parameters.AddWithValue("@s", float.Parse(NotNull(records_eplots[c]["s"])));
                                        add_edit_eplot.Parameters.AddWithValue("@humus", float.Parse(NotNull(records_eplots[c]["humus"])));
                                        add_edit_eplot.Parameters.AddWithValue("@hydrolytic_acid", float.Parse(NotNull(records_eplots[c]["hydrolytic_acid"])));
                                        add_edit_eplot.Parameters.AddWithValue("@total_absorbed_bases", float.Parse(NotNull(records_eplots[c]["total_absorbed_bases"])));
                                        //микроэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@ca", float.Parse(NotNull(records_eplots[c]["ca"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mn", float.Parse(NotNull(records_eplots[c]["mn"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mo", float.Parse(NotNull(records_eplots[c]["mo"])));
                                        add_edit_eplot.Parameters.AddWithValue("@b", float.Parse(NotNull(records_eplots[c]["b"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cu", float.Parse(NotNull(records_eplots[c]["cu"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mg", float.Parse(NotNull(records_eplots[c]["mg"])));
                                        add_edit_eplot.Parameters.AddWithValue("@zn", float.Parse(NotNull(records_eplots[c]["zn"])));
                                        add_edit_eplot.Parameters.AddWithValue("@na", float.Parse(NotNull(records_eplots[c]["na"])));
                                        add_edit_eplot.Parameters.AddWithValue("@co", float.Parse(NotNull(records_eplots[c]["co"])));
                                        add_edit_eplot.Parameters.AddWithValue("@al", float.Parse(NotNull(records_eplots[c]["al"])));
                                        add_edit_eplot.Parameters.AddWithValue("@fe", float.Parse(NotNull(records_eplots[c]["fe"])));
                                        //тяжёлые металлы
                                        add_edit_eplot.Parameters.AddWithValue("@cu_hm", float.Parse(NotNull(records_eplots[c]["cu_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@zn_hm", float.Parse(NotNull(records_eplots[c]["zn_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cd_hm", float.Parse(NotNull(records_eplots[c]["cd_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@pb_hm", float.Parse(NotNull(records_eplots[c]["pb_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ni_hm", float.Parse(NotNull(records_eplots[c]["ni_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@hg_hm", float.Parse(NotNull(records_eplots[c]["hg_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mg_hm", float.Parse(NotNull(records_eplots[c]["mg_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cr_hm", float.Parse(NotNull(records_eplots[c]["cr_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@fe_hm", float.Parse(NotNull(records_eplots[c]["fe_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@f_hm", float.Parse(NotNull(records_eplots[c]["f_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@as_hm", float.Parse(NotNull(records_eplots[c]["as_hm"])));
                                        //радиоэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@cs137", float.Parse(NotNull(records_eplots[c]["cs137"])));
                                        add_edit_eplot.Parameters.AddWithValue("@sr90", float.Parse(NotNull(records_eplots[c]["sr90"])));
                                        //результат
                                        add_edit_eplot.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                                        add_edit_eplot.Parameters["@result"].Direction = ParameterDirection.Output;
                                        add_edit_eplot.ExecuteNonQuery();
                                        result_eplot = add_edit_eplot.Parameters["@result"].Value.ToString();
                                        if (result_eplot != "0")
                                        {
                                            X.Msg.Show(new MessageBoxConfig
                                            {
                                                //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                                Title = "Результат изменения элементарных участков",
                                                Message = "Некоторые элементарные участки не изменены! Обратитесь к администратору.\n" + result_eplot,
                                                Buttons = MessageBox.Button.OK,
                                                Icon = MessageBox.Icon.INFO
                                            });
                                        }
                                    }
                                }
                            }
                            //добаление новых элементарных участков
                            //подсчёт кол-ва новых элементарных участков
                            Int32 count_new_eplots = 0;
                            for (int c = 0; c < records_eplots.Count; c++)
                            {
                                if (Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"]))  < 0)
                                {
                                    count_new_eplots += 1;
                                }
                            }
                            //выборка новых элементарных участков
                            Int32[] new_eplots = new Int32[count_new_eplots];
                            for (int c = 0; c < records_eplots.Count; c++)
                            {
                                if (Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])) < 0)
                                {
                                    new_eplots[c] = Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"]));
                                }
                            }
                            //сортировка и добавление новых элементарных участков
                            Array.Sort(new_eplots);
                            for (int i = (new_eplots.Length - 1); i >= 0; i--)
                            {
                                for (int c = 0; c < records_eplots.Count; c++)
                                {
                                    if (Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])) == new_eplots[i])
                                    {
                                        SqlCommand add_edit_eplot = new SqlCommand("Add_Edit_EPlot", conn);
                                        add_edit_eplot.CommandType = CommandType.StoredProcedure;
                                        add_edit_eplot.Parameters.AddWithValue("@id_plot", Convert.ToInt32(id_plot));
                                        add_edit_eplot.Parameters.AddWithValue("@code_plot", code_plot);
                                        //основные данные
                                        add_edit_eplot.Parameters.AddWithValue("@id_elementary_plot", Convert.ToInt32(NotNull(records_eplots[c]["id_elementary_plot"])));
                                        add_edit_eplot.Parameters.AddWithValue("@number_elementary_plot", NotNull(records_eplots[c]["number_elementary_plot"]));
                                        add_edit_eplot.Parameters.AddWithValue("@code_type_soil", float.Parse(NotNull(records_eplots[c]["code_type_soil"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_grading", float.Parse(NotNull(records_eplots[c]["code_grading"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_erosion", float.Parse(NotNull(records_eplots[c]["code_erosion"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_slope", float.Parse(NotNull(records_eplots[c]["code_slope"])));
                                        add_edit_eplot.Parameters.AddWithValue("@code_exposure", float.Parse(NotNull(records_eplots[c]["code_exposure"])));
                                        //макроэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@n", float.Parse(NotNull(records_eplots[c]["n"])));
                                        add_edit_eplot.Parameters.AddWithValue("@no3", float.Parse(NotNull(records_eplots[c]["no3"])));
                                        add_edit_eplot.Parameters.AddWithValue("@no2", float.Parse(NotNull(records_eplots[c]["no2"])));
                                        add_edit_eplot.Parameters.AddWithValue("@p2o5", float.Parse(NotNull(records_eplots[c]["p2o5"])));
                                        add_edit_eplot.Parameters.AddWithValue("@k2o", float.Parse(NotNull(records_eplots[c]["k2o"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ph_s", float.Parse(NotNull(records_eplots[c]["ph_s"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ph_w", float.Parse(NotNull(records_eplots[c]["ph_w"])));
                                        add_edit_eplot.Parameters.AddWithValue("@s", float.Parse(NotNull(records_eplots[c]["s"])));
                                        add_edit_eplot.Parameters.AddWithValue("@humus", float.Parse(NotNull(records_eplots[c]["humus"])));
                                        add_edit_eplot.Parameters.AddWithValue("@hydrolytic_acid", float.Parse(NotNull(records_eplots[c]["hydrolytic_acid"])));
                                        add_edit_eplot.Parameters.AddWithValue("@total_absorbed_bases", float.Parse(NotNull(records_eplots[c]["total_absorbed_bases"])));
                                        //микроэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@ca", float.Parse(NotNull(records_eplots[c]["ca"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mn", float.Parse(NotNull(records_eplots[c]["mn"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mo", float.Parse(NotNull(records_eplots[c]["mo"])));
                                        add_edit_eplot.Parameters.AddWithValue("@b", float.Parse(NotNull(records_eplots[c]["b"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cu", float.Parse(NotNull(records_eplots[c]["cu"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mg", float.Parse(NotNull(records_eplots[c]["mg"])));
                                        add_edit_eplot.Parameters.AddWithValue("@zn", float.Parse(NotNull(records_eplots[c]["zn"])));
                                        add_edit_eplot.Parameters.AddWithValue("@na", float.Parse(NotNull(records_eplots[c]["na"])));
                                        add_edit_eplot.Parameters.AddWithValue("@co", float.Parse(NotNull(records_eplots[c]["co"])));
                                        add_edit_eplot.Parameters.AddWithValue("@al", float.Parse(NotNull(records_eplots[c]["al"])));
                                        add_edit_eplot.Parameters.AddWithValue("@fe", float.Parse(NotNull(records_eplots[c]["fe"])));
                                        //тяжёлые металлы
                                        add_edit_eplot.Parameters.AddWithValue("@cu_hm", float.Parse(NotNull(records_eplots[c]["cu_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@zn_hm", float.Parse(NotNull(records_eplots[c]["zn_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cd_hm", float.Parse(NotNull(records_eplots[c]["cd_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@pb_hm", float.Parse(NotNull(records_eplots[c]["pb_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@ni_hm", float.Parse(NotNull(records_eplots[c]["ni_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@hg_hm", float.Parse(NotNull(records_eplots[c]["hg_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@mg_hm", float.Parse(NotNull(records_eplots[c]["mg_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@cr_hm", float.Parse(NotNull(records_eplots[c]["cr_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@fe_hm", float.Parse(NotNull(records_eplots[c]["fe_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@f_hm", float.Parse(NotNull(records_eplots[c]["f_hm"])));
                                        add_edit_eplot.Parameters.AddWithValue("@as_hm", float.Parse(NotNull(records_eplots[c]["as_hm"])));
                                        //радиоэлементы
                                        add_edit_eplot.Parameters.AddWithValue("@cs137", float.Parse(NotNull(records_eplots[c]["cs137"])));
                                        add_edit_eplot.Parameters.AddWithValue("@sr90", float.Parse(NotNull(records_eplots[c]["sr90"])));
                                        //результат
                                        add_edit_eplot.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                                        add_edit_eplot.Parameters["@result"].Direction = ParameterDirection.Output;
                                        add_edit_eplot.ExecuteNonQuery();
                                        result_eplot = add_edit_eplot.Parameters["@result"].Value.ToString();
                                        if (result_eplot != "0")
                                        {
                                            X.Msg.Show(new MessageBoxConfig
                                            {
                                                //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                                Title = "Результат добавления элементарных участков",
                                                Message = "Некоторые элементарные участки не добавлены! Обратитесь к администратору.\n" + result_eplot,
                                                Buttons = MessageBox.Button.OK,
                                                Icon = MessageBox.Icon.INFO
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Результат редактирования участка",
                            Message = ("Участок не изменён! Обратитесь к администратору\n" + result_plot).ToString(),
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    //Обновляем участок по элементарным участкам
                    SqlCommand update_plot = new SqlCommand("Update_Plot_From_EPlot", conn);
                    update_plot.CommandType = CommandType.StoredProcedure;
                    update_plot.Parameters.AddWithValue("@code_plot", code_plot);
                    update_plot.ExecuteNonQuery();
                    conn.Close();
                    //Добавить проверку на результат
                    //FillTour();
                    //FillYear("");
                    FillPlot(year);
                    //FillPlot(GetMaxYear());
                    /*Int32 count_tour = ToursGP.Items.Count() - 1;
                    if (count_tour > 0) { ToursGP.GetSelectionModel().Select(count_tour); }
                    Int32 count_year = YearsGP.Items.Count() - 1;
                    if (count_year > 0) { YearsGP.GetSelectionModel().Select(count_year); }*/
                    EditPlotW.Close();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Внимание!!!",
                        Message = "Заполнены не все обязательные поля!\nЗаполните поля и повторите действие.",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
            }
        }
        
        [DirectMethod]
        public void SelectReportTours(String odject, String sender)
        {
            //odject =  0 - отделение, 1 - организания, 2 - район, 3 - область
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                if (cookie_report_tours == null)
                {
                    cookie_report_tours = new HttpCookie("Agrochim31_ReportTours");
                    cookie_report_tours["object"] = odject;
                    cookie_report_tours["sender"] = sender;
                    switch (odject)
                    {
                        case "0": cookie_report_tours["id"] = current_id_department; break;
                        case "1": cookie_report_tours["id"] = current_id_organization; break;
                        case "2": cookie_report_tours["id"] = current_id_region; break;
                        case "3": cookie_report_tours["id"] = "0"; break;
                    }
                    cookie_report_tours["tour"] = "0";
                    cookie_report_tours.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie_report_tours);
                }
                else
                {
                    cookie_report_tours["object"] = odject;
                    cookie_report_tours["sender"] = sender;
                    switch (odject)
                    {
                        case "0": cookie_report_tours["id"] = current_id_department; break;
                        case "1": cookie_report_tours["id"] = current_id_organization; break;
                        case "2": cookie_report_tours["id"] = current_id_region; break;
                        case "3": cookie_report_tours["id"] = "0"; break;
                    }
                    cookie_report_tours["tour"] = "0";
                    Response.Cookies.Set(cookie_report_tours);
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            if (connection_try)
            {
                conn.Open();
                switch (odject)
                {
                    case "0": selectReportTours = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_department = " + current_id_department; break;
                    case "1": selectReportTours = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_organization = " + current_id_organization; break;
                    case "2": selectReportTours = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_region = " + current_id_region; break;
                    case "3": selectReportTours = "SELECT DISTINCT tour FROM View_Plots_Tree"; break;
                }
                ReportToursS.RemoveAll();
                adapterReportTours = new SqlDataAdapter(selectReportTours, conn);
                adapterReportTours.Fill(indexDS, "Report_Tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Report_Tours"]);
                ReportToursS.DataSource = indexDV;
                ReportToursS.DataBind();
                ReportToursS.Sort("tour", Ext.Net.SortDirection.ASC);
                conn.Close();
            }
            ReportToursW.Hidden = false;
            ReportToursSB.Text = String.Empty;
            ReportToursAcceptB.Enabled = false;
        }

        [DirectMethod]
        public void SelectTours(String value)
        {
            if (value != String.Empty && value != null)
            {
                ReportToursAcceptB.Hidden = false;
            }
            else
            {
                ReportToursAcceptB.Hidden = true;
            }
        }

        [DirectMethod]
        public void AcceptReportToursW()
        {
            String sender = String.Empty;
            //odject = 0 - организания, 1 - отделение, 2 - район, 3 - область
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                cookie_report_tours["tour"] = ReportToursSB.Text;
                Response.Cookies.Set(cookie_report_tours);
                sender = cookie_report_tours["sender"];
                ReportToursSB.Text = String.Empty;
                ReportToursAcceptB.Hidden = true;
                ReportToursW.Hidden = true;
                switch (sender)
                {
                    case "1_1": win_1_1.Reload(); win_1_1.Show(); break;
                    case "1_2": win_1_2.Reload(); win_1_2.Show(); break;
                    case "1_3": win_1_3.Reload(); win_1_3.Show(); break;
                    case "1_4": win_1_4.Reload(); win_1_4.Show(); break;
                    case "1_5": win_1_5.Reload(); win_1_5.Show(); break;
                    case "2_3": win_2_3.Reload(); win_2_3.Show(); break;
                    case "2_2": win_2_2.Reload(); win_2_2.Show(); break;
                    case "2_1": win_2_1.Reload(); win_2_1.Show(); break;
                    case "2_4": win_2_4.Reload(); win_2_4.Show(); break;
                    case "2_5": win_2_5.Reload(); win_2_5.Show(); break;
                    case "3_1": win_3_1.Reload(); win_3_1.Show(); break;
                    case "3_2": win_3_2.Reload(); win_3_2.Show(); break;
                    case "3_3": win_3_3.Reload(); win_3_3.Show(); break;
                    case "4_1": win_4_1.Reload(); win_4_1.Show(); break;
                    case "4_2": win_4_2.Reload(); win_4_2.Show(); break;
                }
            }
            else
            {
                ReportToursSB.Text = String.Empty;
                ReportToursAcceptB.Hidden = true;
                ReportToursW.Hidden = true;
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void CloseReportToursW()
        {
            ReportToursSB.Text = String.Empty;
            ReportToursAcceptB.Hidden = true;
            ReportToursW.Hidden = true;
        }
        
        [DirectMethod]
        public void BindTypeSoil()
        {
            if (connection_try)
            {
                conn.Open();
                adapterSoil = new SqlDataAdapter(selectCommSoil, conn);
                adapterSoil.Fill(indexDS, "Type_soil");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_soil"]);
                SoilS.DataSource = indexDV;
                SoilS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindCulture()
        {
            if (connection_try)
            {
                conn.Open();
                adapterCulture = new SqlDataAdapter(selectCommCulture, conn);
                adapterCulture.Fill(indexDS, "Culture");
                indexDV = new System.Data.DataView(indexDS.Tables["Culture"]);
                CultureS.DataSource = indexDV;
                CultureS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTypeCropRotation()
        {
            if (connection_try)
            {
                conn.Open();
                adapterCropRotation = new SqlDataAdapter(selectCommCropRotation, conn);
                adapterCropRotation.Fill(indexDS, "Type_crop_rotation");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_crop_rotation"]);
                CropRotationS.DataSource = indexDV;
                CropRotationS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTypeFarmland()
        {
            if (connection_try)
            {
                conn.Open();
                adapterFarmland = new SqlDataAdapter(selectCommFarmland, conn);
                adapterFarmland.Fill(indexDS, "Type_farmland");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_farmland"]);
                FarmlandS.DataSource = indexDV;
                FarmlandS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTypeErosion()
        {
            if (connection_try)
            {
                conn.Open();
                adapterErosion = new SqlDataAdapter(selectCommErosion, conn);
                adapterErosion.Fill(indexDS, "Type_erosion");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_erosion"]);
                ErosionS.DataSource = indexDV;
                ErosionS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTypeGrading()
        {
            if (connection_try)
            {
                conn.Open();
                adapterGrading = new SqlDataAdapter(selectCommGrading, conn);
                adapterGrading.Fill(indexDS, "Type_grading");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_grading"]);
                GradingS.DataSource = indexDV;
                GradingS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindSlope()
        {
            if (connection_try)
            {
                conn.Open();
                adapterSlope = new SqlDataAdapter(selectCommSlope, conn);
                adapterSlope.Fill(indexDS, "Slope");
                indexDV = new System.Data.DataView(indexDS.Tables["Slope"]);
                SlopeS.DataSource = indexDV;
                SlopeS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindExposure()
        {
            if (connection_try)
            {
                conn.Open();
                adapterExposure = new SqlDataAdapter(selectCommExposure, conn);
                adapterExposure.Fill(indexDS, "Exposure");
                indexDV = new System.Data.DataView(indexDS.Tables["Exposure"]);
                ExposureS.DataSource = indexDV;
                ExposureS.DataBind();
                conn.Close();
            }
        }

        // вывод показателей
        public void BindSignificative()
        {
            if (connection_try)
            {
                conn.Open();
                adapterSignificative = new SqlDataAdapter(selectCommSignificative, conn);
                adapterSignificative.Fill(indexDS, "Significative");
                indexDV = new System.Data.DataView(indexDS.Tables["Significative"]);
                SignificativeS.DataSource = indexDV;
                SignificativeS.DataBind();

                /*adapterGroups = new SqlDataAdapter(selectCommGroups, conn);
                adapterGroups.Fill(indexDS, "Groups");
                indexDV = new System.Data.DataView(indexDS.Tables["Groups"]);
                GroupsS.DataSource = indexDV;
                GroupsS.DataBind();*/
                conn.Close();
            }
        }

        //получение групп выделенного показателя
        [DirectMethod]
        public void BindGroups(String id_significative)
        {
            if (connection_try && id_significative != null && id_significative != "" && id_significative != String.Empty)
            {
                conn.Open();
                /*if (indexDS.Tables["Method"] != null) { indexDS.Tables["Method"].Clear(); }
                adapterMethod = new SqlDataAdapter(selectCommMethod, conn);
                adapterMethod.Fill(indexDS, "Method");
                indexDV = new System.Data.DataView(indexDS.Tables["Method"]);
                MethodGroupS.DataSource = indexDV;
                MethodGroupS.DataBind();*/

                adapterGroups = new SqlDataAdapter(selectCommGroups + " WHERE id_significative=" + id_significative, conn);
                adapterGroups.Fill(indexDS, "Groups");
                indexDV = new System.Data.DataView(indexDS.Tables["Groups"]);
                GroupsS.DataSource = indexDV;
                GroupsS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindUsers(String id_role)
        {
            if (connection_try && id_role != null && id_role != "" && id_role != String.Empty)
            {
                conn.Open();
                adapterUser = new SqlDataAdapter(selectCommUser + " WHERE id_role=" + id_role, conn);
                adapterUser.Fill(indexDS, "Users");
                indexDV = new System.Data.DataView(indexDS.Tables["Users"]);
                UserS.DataSource = indexDV;
                UserS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindRole()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRole = new SqlDataAdapter(selectCommRole, conn);
                adapterRole.Fill(indexDS, "Roles");
                indexDV = new System.Data.DataView(indexDS.Tables["Roles"]);
                RoleS.DataSource = indexDV;
                RoleS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindJobTitle()
        {
            if (connection_try)
            {
                conn.Open();
                adapterJobTitle = new SqlDataAdapter(selectCommJobTitle, conn);
                adapterJobTitle.Fill(indexDS, "JobTitle");
                indexDV = new System.Data.DataView(indexDS.Tables["JobTitle"]);
                JobTitleS.DataSource = indexDV;
                JobTitleS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindMission()
        {
            if (connection_try)
            {
                conn.Open();
                adapterMission = new SqlDataAdapter(selectCommMission, conn);
                adapterMission.Fill(indexDS, "Missions");
                indexDV = new System.Data.DataView(indexDS.Tables["Missions"]);
                MissionS.DataSource = indexDV;
                MissionS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTrackers()
        {
            if (connection_try)
            {
                conn.Open();
                adapterTrackers = new SqlDataAdapter(selectCommTrackers, conn);
                adapterTrackers.Fill(indexDS, "Trackers");
                indexDV = new System.Data.DataView(indexDS.Tables["Trackers"]);
                TrackersS.DataSource = indexDV;
                TrackersS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindCarTrackers()
        {
            if (connection_try)
            {
                conn.Open();
                adapterTrackers = new SqlDataAdapter(selectCommTrackers, conn);
                adapterTrackers.Fill(indexDS, "CarTrackers");
                indexDV = new System.Data.DataView(indexDS.Tables["CarTrackers"]);
                CarsIMEITrackerS.DataSource = indexDV;
                CarsIMEITrackerS.DataBind();
                conn.Close();
            }
        }   

        [DirectMethod]
        public void BindCars()
        {
            if (connection_try)
            {
                conn.Open();
                adapterCars = new SqlDataAdapter(selectCommCars, conn);
                adapterCars.Fill(indexDS, "Cars");
                indexDV = new System.Data.DataView(indexDS.Tables["Cars"]);
                CarsS.DataSource = indexDV;
                CarsS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindTerritory()
        {
            if (connection_try)
            {
                conn.Open();
                adapterTerritory = new SqlDataAdapter(selectCommTerritory, conn);
                adapterTerritory.Fill(indexDS, "Territory");
                indexDV = new System.Data.DataView(indexDS.Tables["Territory"]);
                CarsTerritoryS.DataSource = indexDV;
                CarsTerritoryS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void AddEditSoil(String id_type_soil, String code_type_soil, String title_type_soil)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_soil = new SqlCommand("Add_Edit_Type_Soil", conn);
                add_edit_soil.CommandType = CommandType.StoredProcedure;
                add_edit_soil.Parameters.AddWithValue("@id_type_soil", Convert.ToInt32(id_type_soil));
                add_edit_soil.Parameters.AddWithValue("@code_type_soil", Convert.ToInt32(code_type_soil));
                add_edit_soil.Parameters.AddWithValue("@title_type_soil", title_type_soil);
                add_edit_soil.ExecuteNonQuery();
                conn.Close();
                BindTypeSoil();
                SoilGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteSoil(String id_type_soil)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_soil = new SqlCommand("Delete_Type_Soil", conn);
                delete_soil.CommandType = CommandType.StoredProcedure;
                delete_soil.Parameters.AddWithValue("@id_type_soil", Convert.ToInt32(id_type_soil));
                delete_soil.ExecuteNonQuery();
                conn.Close();
                SoilGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditCulture(String id_culture, String code_culture, String title_culture)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_culture = new SqlCommand("Add_Edit_Culture", conn);
                add_edit_culture.CommandType = CommandType.StoredProcedure;
                add_edit_culture.Parameters.AddWithValue("@id_culture", Convert.ToInt32(id_culture));
                add_edit_culture.Parameters.AddWithValue("@code_culture", Convert.ToInt32(code_culture));
                add_edit_culture.Parameters.AddWithValue("@title_culture", title_culture);
                add_edit_culture.ExecuteNonQuery();
                conn.Close();
                BindCulture();
                CultureGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteCulture(String id_culture)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_culture = new SqlCommand("Delete_Culture", conn);
                delete_culture.CommandType = CommandType.StoredProcedure;
                delete_culture.Parameters.AddWithValue("@id_culture", Convert.ToInt32(id_culture));
                delete_culture.ExecuteNonQuery();
                conn.Close();
                CultureGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditCropRotation(String id_crop_rotation, String code_crop_rotation, String title_crop_rotation)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_crop_rotation = new SqlCommand("Add_Edit_Crop_Rotation", conn);
                add_edit_crop_rotation.CommandType = CommandType.StoredProcedure;
                add_edit_crop_rotation.Parameters.AddWithValue("@id_crop_rotation", Convert.ToInt32(id_crop_rotation));
                add_edit_crop_rotation.Parameters.AddWithValue("@code_crop_rotation", Convert.ToInt32(code_crop_rotation));
                add_edit_crop_rotation.Parameters.AddWithValue("@title_crop_rotation", title_crop_rotation);
                add_edit_crop_rotation.ExecuteNonQuery();
                conn.Close();
                BindTypeCropRotation();
                CropRotationGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteCropRotation(String id_crop_rotation)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_crop_rotation = new SqlCommand("Delete_Crop_Rotation", conn);
                delete_crop_rotation.CommandType = CommandType.StoredProcedure;
                delete_crop_rotation.Parameters.AddWithValue("@id_crop_rotation", Convert.ToInt32(id_crop_rotation));
                delete_crop_rotation.ExecuteNonQuery();
                conn.Close();
                CropRotationGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditFarmland(String id_farmland, String code_farmland, String title_farmland)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_farmland = new SqlCommand("Add_Edit_Type_Farmaland", conn);
                add_edit_farmland.CommandType = CommandType.StoredProcedure;
                add_edit_farmland.Parameters.AddWithValue("@id_farmland", Convert.ToInt32(id_farmland));
                add_edit_farmland.Parameters.AddWithValue("@code_farmland", Convert.ToInt32(code_farmland));
                add_edit_farmland.Parameters.AddWithValue("@title_farmland", title_farmland);
                add_edit_farmland.ExecuteNonQuery();
                conn.Close();
                BindTypeFarmland();
                FarmlandGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteFarmland(String id_farmland)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_farmland = new SqlCommand("Delete_Type_Farmland", conn);
                delete_farmland.CommandType = CommandType.StoredProcedure;
                delete_farmland.Parameters.AddWithValue("@id_farmland", Convert.ToInt32(id_farmland));
                delete_farmland.ExecuteNonQuery();
                conn.Close();
                FarmlandGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditErosion(String id_erosion, String code_erosion, String title_erosion)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_erosion = new SqlCommand("Add_Edit_Type_Erosion", conn);
                add_edit_erosion.CommandType = CommandType.StoredProcedure;
                add_edit_erosion.Parameters.AddWithValue("@id_erosion", Convert.ToInt32(id_erosion));
                add_edit_erosion.Parameters.AddWithValue("@code_erosion", Convert.ToInt32(code_erosion));
                add_edit_erosion.Parameters.AddWithValue("@title_erosion", title_erosion);
                add_edit_erosion.ExecuteNonQuery();
                conn.Close();
                BindTypeErosion();
                ErosionGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteErosion(String id_erosion)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_erosion = new SqlCommand("Delete_Type_Erosion", conn);
                delete_erosion.CommandType = CommandType.StoredProcedure;
                delete_erosion.Parameters.AddWithValue("@id_erosion", Convert.ToInt32(id_erosion));
                delete_erosion.ExecuteNonQuery();
                conn.Close();
                ErosionGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditGrading(String id_grading, String code_grading, String title_grading)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_grading = new SqlCommand("Add_Edit_Type_Grading", conn);
                add_edit_grading.CommandType = CommandType.StoredProcedure;
                add_edit_grading.Parameters.AddWithValue("@id_grading", Convert.ToInt32(id_grading));
                add_edit_grading.Parameters.AddWithValue("@code_grading", Convert.ToInt32(code_grading));
                add_edit_grading.Parameters.AddWithValue("@title_grading", title_grading);
                add_edit_grading.ExecuteNonQuery();
                conn.Close();
                BindTypeGrading();
                GradingGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteGrading(String id_grading)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_grading = new SqlCommand("Delete_Type_Grading", conn);
                delete_grading.CommandType = CommandType.StoredProcedure;
                delete_grading.Parameters.AddWithValue("@id_grading", Convert.ToInt32(id_grading));
                delete_grading.ExecuteNonQuery();
                conn.Close();
                GradingGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditExposure(String id_exposure, String code_exposure, String title_exposure)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_exposure = new SqlCommand("Add_Edit_Exposure", conn);
                add_edit_exposure.CommandType = CommandType.StoredProcedure;
                add_edit_exposure.Parameters.AddWithValue("@id_exposure", Convert.ToInt32(id_exposure));
                add_edit_exposure.Parameters.AddWithValue("@code_exposure", Convert.ToInt32(code_exposure));
                add_edit_exposure.Parameters.AddWithValue("@title_exposure", title_exposure);
                add_edit_exposure.ExecuteNonQuery();
                conn.Close();
                BindExposure();
                ExposureGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteExposure(String id_exposure)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_exposure = new SqlCommand("Delete_Exposure", conn);
                delete_exposure.CommandType = CommandType.StoredProcedure;
                delete_exposure.Parameters.AddWithValue("@id_exposure", Convert.ToInt32(id_exposure));
                delete_exposure.ExecuteNonQuery();
                conn.Close();
                ExposureGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditSlope(String id_slope, String code_slope, String title_slope)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_slope = new SqlCommand("Add_Edit_Slope", conn);
                add_edit_slope.CommandType = CommandType.StoredProcedure;
                add_edit_slope.Parameters.AddWithValue("@id_slope", Convert.ToInt32(id_slope));
                add_edit_slope.Parameters.AddWithValue("@code_slope", Convert.ToInt32(code_slope));
                add_edit_slope.Parameters.AddWithValue("@title_slope", title_slope);
                add_edit_slope.ExecuteNonQuery();
                conn.Close();
                BindSlope();
                SlopeGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteSlope(String id_slope)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_slope = new SqlCommand("Delete_Slope", conn);
                delete_slope.CommandType = CommandType.StoredProcedure;
                delete_slope.Parameters.AddWithValue("@id_slope", Convert.ToInt32(id_slope));
                delete_slope.ExecuteNonQuery();
                conn.Close();
                SlopeGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void SelectSignificative()
        {
            if (connection_try)
            {
                SignificativeGP.GetSelectionModel().Select(0);
            }
        }

        [DirectMethod]
        public void SelectRole()
        {
            if (connection_try)
            {
                RoleGP.GetSelectionModel().Select(0);
            }
        }

        [DirectMethod]
        public void SelectedSignificative(String id_significative)
        {
            if (Request.Browser.Cookies)
            {
                if (id_significative == "" || id_significative == null || id_significative == String.Empty) { id_significative = "0"; }
                cookie = Request.Cookies["Agrochim31"];
                cookie["current_id_significative"] = id_significative;
                Response.Cookies.Set(cookie);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            current_id_significative = id_significative;
            BindGroups(current_id_significative);
        }

        [DirectMethod]
        public void SelectedRole(String id_role)
        {
            if (Request.Browser.Cookies)
            {
                if (id_role == "" || id_role == null || id_role == String.Empty) { id_role = "0"; }
                cookie = Request.Cookies["Agrochim31"];
                cookie["current_id_role"] = id_role;
                Response.Cookies.Set(cookie);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            current_id_role = id_role;
            BindUsers(current_id_role);
        }

        [DirectMethod]
        public void AddEditSignificative(String id_significative, String title_significative, String unit_significative, String name_significative, String min_value, String max_value, String number_of_digits)
        {
            if (connection_try)
            {
                if (id_significative == "" || id_significative == null || id_significative == String.Empty) { id_significative = "0"; }
                if ((title_significative != "" && title_significative != null && title_significative != String.Empty) &&
                    (unit_significative != "" && unit_significative != null && unit_significative != String.Empty) &&
                    (name_significative != "" && name_significative != null && name_significative != String.Empty) &&
                    (min_value != "" && min_value != null && min_value != String.Empty) &&
                    (max_value != "" && max_value != null && max_value != String.Empty) &&
                    (number_of_digits != "" && number_of_digits != null && number_of_digits != String.Empty))
                {
                    min_value = min_value.Replace('.', ',');
                    max_value = max_value.Replace('.', ',');
                    conn.Open();
                    SqlCommand add_edit_significative = new SqlCommand("Add_Edit_Significative", conn);
                    add_edit_significative.CommandType = CommandType.StoredProcedure;
                    add_edit_significative.Parameters.AddWithValue("@id_significative", Convert.ToInt32(NotNull(id_significative)));
                    add_edit_significative.Parameters.AddWithValue("@title_significative", NotNull(title_significative));
                    add_edit_significative.Parameters.AddWithValue("@unit_significative", NotNull(unit_significative));
                    add_edit_significative.Parameters.AddWithValue("@name_significative", NotNull(name_significative));
                    add_edit_significative.Parameters.AddWithValue("@min_value", Convert.ToDouble(NotNull(min_value)));
                    add_edit_significative.Parameters.AddWithValue("@max_value", Convert.ToDouble(NotNull(max_value)));
                    add_edit_significative.Parameters.AddWithValue("@number_of_digits", Convert.ToInt32(NotNull(number_of_digits)));
                    add_edit_significative.ExecuteNonQuery();
                    conn.Close();
                    BindSignificative();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Оповещение",
                        Message = "Не все обязательные поля заполнены! Запись будет удалена.",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                    SignificativeGP.GetStore().Remove(0);
                }
                //SignificativeGP.GetStore().Reload(); 
            }
        }

        [DirectMethod]
        public void DeleteSignificative(String id_significative)
        {
            if (connection_try && id_significative != null && id_significative != "" && id_significative != String.Empty)
            {
                conn.Open();
                SqlCommand delete_significative = new SqlCommand("Delete_Significative", conn);
                delete_significative.CommandType = CommandType.StoredProcedure;
                delete_significative.Parameters.AddWithValue("@id_significative", Convert.ToInt32(id_significative));
                delete_significative.ExecuteNonQuery();
                conn.Close();
                //SignificativeGP.GetStore().Reload();
                //BindSignificative();
            }
        }

        //добавление группы
        [DirectMethod]
        public void AddEditGroup(String id_group, String number_group, String title_group, String title_method, String from_group, String to_group, String coefficient, String color)
        {
            if (connection_try && current_id_significative != "0" && current_id_significative != String.Empty)
            {
                conn.Open();
                adapterGroups = new SqlDataAdapter(selectCommGroups + " WHERE id_significative=" + current_id_significative, conn);
                adapterGroups.Fill(indexDS, "Groups");
                System.Data.DataView dv_group = new System.Data.DataView(indexDS.Tables["Groups"]);
                dv_group.Sort = "number_group";
                conn.Close();
                //условие на проверку вводимых значений!!!
                if ((number_group != String.Empty) && (title_group != String.Empty) && (title_method != String.Empty) && (from_group != String.Empty) && (to_group != String.Empty) && (coefficient != String.Empty))
                {
                    from_group = ReplaceValue(from_group, "2");
                    to_group = ReplaceValue(to_group, "2");
                    coefficient = ReplaceValue(coefficient, "2");
                    conn.Open();
                    SqlCommand add_edit_group = new SqlCommand("Add_Edit_Group", conn);
                    add_edit_group.CommandType = CommandType.StoredProcedure;
                    add_edit_group.Parameters.AddWithValue("@id_group", Convert.ToInt32(NotNull(id_group)));
                    add_edit_group.Parameters.AddWithValue("@id_significative", Convert.ToInt32(NotNull(current_id_significative)));
                    add_edit_group.Parameters.AddWithValue("@number_group", Convert.ToInt32(NotNull(number_group)));
                    add_edit_group.Parameters.AddWithValue("@title_group", NotNull(title_group));
                    add_edit_group.Parameters.AddWithValue("@title_method", NotNull(title_method));
                    add_edit_group.Parameters.AddWithValue("@from_group", float.Parse(NotNull(from_group)));
                    add_edit_group.Parameters.AddWithValue("@to_group", float.Parse(NotNull(to_group)));
                    add_edit_group.Parameters.AddWithValue("@coefficient", float.Parse(NotNull(coefficient)));
                    add_edit_group.Parameters.AddWithValue("@color", color);
                    add_edit_group.ExecuteNonQuery();
                    conn.Close();
                }
                if (indexDS.Tables["Groups"] != null) { indexDS.Tables["Groups"].Clear(); }
                BindGroups(current_id_significative);
            }
        }

        [DirectMethod]
        public void DeleteGroup(String id_group)
        {
            if (connection_try && id_group != null && id_group != "" && id_group != String.Empty)
            {
                conn.Open();
                SqlCommand delete_group = new SqlCommand("Delete_Group", conn);
                delete_group.CommandType = CommandType.StoredProcedure;
                delete_group.Parameters.AddWithValue("@id_group", Convert.ToInt32(id_group));
                delete_group.ExecuteNonQuery();
                conn.Close();
            }
        }

        //добавление, редактирование ролей
        [DirectMethod]
        public void AddEditRole(String id_role, String title_role, String read_role, String edit_role, String add_role, String delete_role)
        {
            if (connection_try)
            {
                if (id_role == "" || id_role == null || id_role ==String.Empty) { id_role="0"; }
                if (title_role != "" && title_role != null && title_role != String.Empty)
                {
                    conn.Open();
                    if (String.Compare("true", read_role) == 0) { read_role = "1"; }
                    else { read_role = "0"; }
                    if (String.Compare("true", edit_role) == 0) { edit_role = "1"; }
                    else { edit_role = "0"; }
                    if (String.Compare("true", add_role) == 0) { add_role = "1"; }
                    else { add_role = "0"; }
                    if (String.Compare("true", delete_role) == 0) { delete_role = "1"; }
                    else { delete_role = "0"; }
                    SqlCommand add_edit_role = new SqlCommand("Add_Edit_Role", conn);
                    add_edit_role.CommandType = CommandType.StoredProcedure;
                    add_edit_role.Parameters.AddWithValue("@id_role", Convert.ToInt32(NotNull(id_role)));
                    add_edit_role.Parameters.AddWithValue("@title_role", NotNull(title_role));
                    add_edit_role.Parameters.AddWithValue("@read_role", Convert.ToBoolean(Convert.ToInt32(read_role)));
                    add_edit_role.Parameters.AddWithValue("@edit_role", Convert.ToBoolean(Convert.ToInt32(edit_role)));
                    add_edit_role.Parameters.AddWithValue("@add_role", Convert.ToBoolean(Convert.ToInt32(add_role)));
                    add_edit_role.Parameters.AddWithValue("@delete_role", Convert.ToBoolean(Convert.ToInt32(delete_role)));
                    add_edit_role.ExecuteNonQuery();
                    conn.Close();
                    BindRole();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Оповещение",
                        Message = "Не все обязательные поля заполнены! Запись будет удалена.",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                    RoleGP.GetStore().Remove(0);
                }
            }
        }

        //удаление роли
        [DirectMethod]
        public void DeleteRole(String id_role)
        {
            if (connection_try && id_role != null && id_role != "" && id_role != String.Empty)
            {
                conn.Open();
                SqlCommand delete_role = new SqlCommand("Delete_Role", conn);
                delete_role.CommandType = CommandType.StoredProcedure;
                delete_role.Parameters.AddWithValue("@id_role", Convert.ToInt32(id_role));
                delete_role.ExecuteNonQuery();
                conn.Close();
                //RoleGP.GetStore().Reload();
            }
        }

        //добавление, редактирование пользователей
        [DirectMethod]
        public void AddEditUser(String id_user, String surname, String name, String patronymic, String title_division, String title_job_title, String login, String password)
        {
            if (connection_try)
            {
                if (id_user == "" || id_user == String.Empty || id_user == null) { id_user = "0"; }
                if (current_id_role != "" && current_id_role != String.Empty && current_id_role != null)
                {
                    if ((surname != "" && surname != String.Empty && surname != null) &&
                        (name != "" && name != String.Empty && name != null) &&
                        (patronymic != "" && patronymic != String.Empty && patronymic != null) &&
                        (login != "" && login != String.Empty && login != null) &&
                        (password != "" && password != String.Empty && password != null))
                    {
                        //title_job_title = NotNull(title_job_title);
                        conn.Open();
                        SqlCommand add_edit_user = new SqlCommand("Add_Edit_User", conn);
                        add_edit_user.CommandType = CommandType.StoredProcedure;
                        add_edit_user.Parameters.AddWithValue("@id_user", Convert.ToInt32(NotNull(id_user)));
                        add_edit_user.Parameters.AddWithValue("@id_role", Convert.ToInt32(NotNull(current_id_role)));
                        add_edit_user.Parameters.AddWithValue("@surname", NotNullText(surname));
                        add_edit_user.Parameters.AddWithValue("@name", NotNullText(name));
                        add_edit_user.Parameters.AddWithValue("@patronymic", NotNullText(patronymic));
                        add_edit_user.Parameters.AddWithValue("@title_division", NotNullText(title_division));
                        add_edit_user.Parameters.AddWithValue("@title_job_title", NotNullText(title_job_title));
                        add_edit_user.Parameters.AddWithValue("@login", NotNullText(login));
                        if (Request.Browser.Cookies)
                        {
                            if (String.Compare(current_password, password) == 0)
                            {
                                add_edit_user.Parameters.AddWithValue("@password", current_password);
                            }
                            else
                            {
                                add_edit_user.Parameters.AddWithValue("@password", GetMd5Hash(password));
                            }
                        }
                        else
                        {
                            add_edit_user.Parameters.AddWithValue("@password", GetMd5Hash(password));
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Title = "Ошибка cookies",
                                Message = "Необходимо включить поддержку cookies!!!",
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.INFO
                            });
                        }
                        add_edit_user.ExecuteNonQuery();
                        conn.Close();
                        BindUsers(current_id_role);
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Оповещение",
                            Message = "Не все обязательные поля заполнены! Запись будет удалена.",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                        UserGP.GetStore().Remove(0);
                    }
                }
            }
        }

        // удаление пользователя
        [DirectMethod]
        public void DeleteUser(String id_user)
        {
            if (connection_try && id_user != null && id_user != "" && id_user != String.Empty)
            {
                conn.Open();
                SqlCommand delete_user = new SqlCommand("Delete_User", conn);
                delete_user.CommandType = CommandType.StoredProcedure;
                delete_user.Parameters.AddWithValue("@id_user", Convert.ToInt32(id_user));
                delete_user.ExecuteNonQuery();
                conn.Close();
               // UserGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditJobTitle(String id_job_title, String title_job_title)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_job_title = new SqlCommand("Add_Edit_Job_Title", conn);
                add_job_title.CommandType = CommandType.StoredProcedure;
                add_job_title.Parameters.AddWithValue("@id_job_title", Convert.ToInt32(NotNull(id_job_title)));
                add_job_title.Parameters.AddWithValue("@title_job_title", NotNull(title_job_title));
                add_job_title.ExecuteNonQuery();
                conn.Close();
                BindJobTitle();
                //JobTitleGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteJobTitle(String id_job_title)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_job_title = new SqlCommand("Delete_Job_Title", conn);
                delete_job_title.CommandType = CommandType.StoredProcedure;
                delete_job_title.Parameters.AddWithValue("@id_job_title", Convert.ToInt32(id_job_title));
                delete_job_title.ExecuteNonQuery();
                conn.Close();
                //JobTitleGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditMission(String id_mission, String title_mission)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_mission = new SqlCommand("Add_Edit_Mission", conn);
                add_edit_mission.CommandType = CommandType.StoredProcedure;
                add_edit_mission.Parameters.AddWithValue("@id_mission", Convert.ToInt32(NotNull(id_mission)));
                add_edit_mission.Parameters.AddWithValue("@title_mission", title_mission);
                add_edit_mission.ExecuteNonQuery();
                conn.Close();
                BindMission();
                //MissionGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteMission(String id_mission)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_mission = new SqlCommand("Delete_Mission", conn);
                delete_mission.CommandType = CommandType.StoredProcedure;
                delete_mission.Parameters.AddWithValue("@id_mission", Convert.ToInt32(id_mission));
                delete_mission.ExecuteNonQuery();
                conn.Close();
                //MissionGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditTracker(String id_gps_tracker, String imei)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_tracker = new SqlCommand("Add_Edit_GPS_Tracker", conn);
                add_edit_tracker.CommandType = CommandType.StoredProcedure;
                add_edit_tracker.Parameters.AddWithValue("@id_gps_tracker", Convert.ToInt32(NotNull(id_gps_tracker)));
                add_edit_tracker.Parameters.AddWithValue("@imei", imei);
                add_edit_tracker.ExecuteNonQuery();
                conn.Close();
                BindTrackers();
                //TrackersGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteTracker(String id_gps_tracker)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_tracker = new SqlCommand("Delete_GPS_Tracker", conn);
                delete_tracker.CommandType = CommandType.StoredProcedure;
                delete_tracker.Parameters.AddWithValue("@id_gps_tracker", Convert.ToInt32(id_gps_tracker));
                delete_tracker.ExecuteNonQuery();
                conn.Close();
                TrackersGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void AddEditCar(String id_car, String car_model, String license_plate, String imei, String id_region, String title_organization)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand add_edit_car = new SqlCommand("Add_Edit_Car", conn);
                add_edit_car.CommandType = CommandType.StoredProcedure;
                add_edit_car.Parameters.AddWithValue("@id_car", Convert.ToInt32(NotNull(id_car)));
                add_edit_car.Parameters.AddWithValue("@car_model", NotNull(car_model));
                add_edit_car.Parameters.AddWithValue("@license_plate", NotNull(license_plate));
                add_edit_car.Parameters.AddWithValue("@imei", NotNull(imei));
                add_edit_car.Parameters.AddWithValue("@id_region", Convert.ToInt32(NotNull(id_region)));
                add_edit_car.Parameters.AddWithValue("@title_organization", NotNull(title_organization));
                add_edit_car.ExecuteNonQuery();
                conn.Close();
                BindCars();
                //CarsGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void DeleteCar(String id_car)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_car = new SqlCommand("Delete_car", conn);
                delete_car.CommandType = CommandType.StoredProcedure;
                delete_car.Parameters.AddWithValue("@id_car", Convert.ToInt32(id_car));
                delete_car.ExecuteNonQuery();
                conn.Close();
                //CarsGP.GetStore().Reload();
            }
        }

        [DirectMethod]
        public void GetRegionsForTerritory(String id_territory)
        {
            if (connection_try)
            {
                String sql_querry = "SELECT * FROM Region WHERE id_territory=" + NotNull(id_territory);
                conn.Open();
                SqlDataAdapter adapterRedionsForTerritory = new SqlDataAdapter(sql_querry, conn);
                adapterRedionsForTerritory.Fill(indexDS, "RedionsForTerritory");
                indexDV = new System.Data.DataView(indexDS.Tables["RedionsForTerritory"]);
                CarsRegionS.DataSource = indexDV;
                CarsRegionS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void GetCarOrganizationsForRegion(String id_region)
        {
            if (connection_try)
            {
                String sql_querry = "SELECT * FROM Organization WHERE id_region=" + NotNull(id_region);
                conn.Open();
                SqlDataAdapter adapterOrganizationsForRegion = new SqlDataAdapter(sql_querry, conn);
                adapterOrganizationsForRegion.Fill(indexDS, "OrganizationsForRegion");
                indexDV = new System.Data.DataView(indexDS.Tables["OrganizationsForRegion"]);
                CarsOrganizationS.DataSource = indexDV;
                CarsOrganizationS.DataBind();
                conn.Close();
            }
        }

        //функция получения набора данных из куки для определения пошльзователя и его прав
        public login_data CookieLoginSplit(HttpCookie cookie_login)
        {
            login_data reg_user = new login_data();
            reg_user.id_user = Convert.ToInt32(cookie_login["id_user"]);
            reg_user.login = cookie_login["login"];
            reg_user.surname = cookie_login["surname"];
            reg_user.name = cookie_login["name"];
            reg_user.patronymic = cookie_login["patronymic"];
            reg_user.authorization = cookie_login["authorization"];
            reg_user.read = Convert.ToBoolean(Convert.ToInt32(cookie_login_user["role"].Split('|')[0]));
            reg_user.edit = Convert.ToBoolean(Convert.ToInt32(cookie_login_user["role"].Split('|')[1]));
            reg_user.add = Convert.ToBoolean(Convert.ToInt32(cookie_login_user["role"].Split('|')[2]));
            reg_user.delete = Convert.ToBoolean(Convert.ToInt32(cookie_login_user["role"].Split('|')[3]));
            return reg_user;
        }
        //функция получения набора данных из куки для определения пошльзователя и его прав
        public void SetCookieLogin(login_data reg_user, HttpCookie cookie_login)
        {
            cookie_login["id_user"] = reg_user.id_user.ToString();
            cookie_login["login"] = reg_user.login;
            cookie_login["surname"] = reg_user.surname;
            cookie_login["name"] = reg_user.name;
            cookie_login["patronymic"] = reg_user.patronymic;
            cookie_login["authorization"] = reg_user.authorization.ToString();
            cookie_login["role"] = Convert.ToInt32(reg_user.read).ToString() + "|" + Convert.ToInt32(reg_user.edit).ToString() + "|" 
                                 + Convert.ToInt32(reg_user.add).ToString() + "|" + Convert.ToInt32(reg_user.delete).ToString();
        }
        //проверка авотризации пользователя
        [DirectMethod]
        public void GetAuthorization(String login, String password)
        {
            connection_try = TryConnection(SetConnectionString());
            if (connection_try)
            {
                conn.Open();
                String md5Password = GetMd5Hash(password);
                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Пароль md5",
                    Message = md5Password,
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
                try
                {
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
                    if (get_user_data.Parameters["@id_user"].Value.ToString() != "0" &&
                        get_user_data.Parameters["@id_user"].Value.ToString() != null &&
                        get_user_data.Parameters["@id_user"].Value.ToString() != "null")
                    {
                        user_reg_data.id_user = Convert.ToInt32(get_user_data.Parameters["@id_user"].Value);
                        user_reg_data.login = login;
                        user_reg_data.surname = get_user_data.Parameters["@surname"].Value.ToString();
                        user_reg_data.name = get_user_data.Parameters["@name"].Value.ToString();
                        user_reg_data.patronymic = get_user_data.Parameters["@patronymic"].Value.ToString();
                        user_reg_data.read = Convert.ToBoolean(get_user_data.Parameters["@read_role"].Value);
                        user_reg_data.edit = Convert.ToBoolean(get_user_data.Parameters["@edit_role"].Value);
                        user_reg_data.add = Convert.ToBoolean(get_user_data.Parameters["@add_role"].Value);
                        user_reg_data.delete = Convert.ToBoolean(get_user_data.Parameters["@delete_role"].Value);
                        user_reg_data.authorization = GetMd5Hash(DateTime.Today.Day.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString() + user_reg_data.id_user.ToString());
                        if (Request.Browser.Cookies)
                        {
                            cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                            if (cookie_login_user == null)
                            {
                                cookie_login_user = new HttpCookie("Agrochim31_Authorization");
                                cookie_login_user.Expires = DateTime.Now.AddHours(24);
                                Response.Cookies.Add(cookie_login_user);
                            }
                            SetCookieLogin(user_reg_data, cookie_login_user);
                            Response.Cookies.Set(cookie_login_user);
                        }
                        else
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Title = "Ошибка cookies",
                                Message = "Необходимо включить поддержку cookies!!!",
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.INFO
                            });
                        }
                        LoginW.Close();
                        UserNameL.Text = user_reg_data.surname + " " + user_reg_data.name + " " + user_reg_data.patronymic + " ";
                        UserNameL.Hidden = false;
                        ExitB.Hidden = false;
                        //обновление страницы!
                        Response.Redirect(Request.RawUrl);
                        /*FillRegion();
                        BindSignificative();
                        RegionGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_region));*/
                    }
                    else
                    {
                        user_reg_data.id_user = 0;
                        user_reg_data.login = "0";
                        user_reg_data.surname = "0";
                        user_reg_data.name = "0";
                        user_reg_data.patronymic = "0";
                        user_reg_data.read = false;
                        user_reg_data.edit = false;
                        user_reg_data.add = false;
                        user_reg_data.delete = false;
                        user_reg_data.authorization = "0";
                        if (Request.Browser.Cookies)
                        {
                            cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                            if (cookie_login_user == null)
                            {
                                cookie_login_user = new HttpCookie("Agrochim31_Authorization");
                                cookie_login_user.Expires = DateTime.Now.AddHours(24);
                                Response.Cookies.Add(cookie_login_user);
                            }
                            SetCookieLogin(user_reg_data, cookie_login_user);
                            Response.Cookies.Set(cookie_login_user);
                        }
                        else
                        {
                            X.Msg.Show(new MessageBoxConfig
                            {
                                Title = "Ошибка cookies",
                                Message = "Необходимо включить поддержку cookies!!!",
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.INFO
                            });
                        }
                        UserNameL.Text = "...";
                        UserNameL.Hidden = true;
                        ExitB.Hidden = true;
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка входа",
                            Message = "Пользователя с таким логином или паролем не существует!",
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                    SetSecurity(user_reg_data);
                }
                catch (Exception exc)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка выполнения процедуры",
                        Message = exc.Message + "<br />" + login + "<br />" + md5Password,
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                    connection_try = false;
                }
                conn.Close();
            }
        }
        //выход пользователя из системы
        [DirectMethod]
        public void ExitUser()
        {
            UserNameL.Text = "...";
            UserNameL.Hidden = true;
            ExitB.Hidden = true;
            UserPassword.Text = "";
            if (Request.Browser.Cookies)
            {
                cookie_login_user = Request.Cookies["Agrochim31_Authorization"];
                if (cookie_login_user == null)
                {
                    cookie_login_user = new HttpCookie("Agrochim31_Authorization");
                    cookie_login_user["id_user"] = "0";
                    cookie_login_user["login"] = "0";
                    cookie_login_user["surname"] = "0";
                    cookie_login_user["name"] = "0";
                    cookie_login_user["patronymic"] = "0";
                    cookie_login_user["authorization"] = "0";
                    cookie_login_user["role"] = "0|0|0|0";
                    cookie_login_user.Expires = DateTime.Now.AddHours(24);
                    Response.Cookies.Add(cookie_login_user);
                }
                else
                {
                    cookie_login_user["authorization"] = "0";
                    cookie_login_user["role"] = "0|0|0|0";
                    Response.Cookies.Set(cookie_login_user);
                }
                user_reg_data = CookieLoginSplit(cookie_login_user);
                SetSecurity(user_reg_data);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            /*RegionS.RemoveAll();
            OrganizationS.RemoveAll();
            DepartmentS.RemoveAll();*/
            LoginW.Show();
        }
        //вывод или скрытие окна входа
        [DirectMethod]
        public void LoginFormShow(String true_false)
        {
            if (Request.Browser.Cookies)
            {
                cookie = Request.Cookies["Agrochim31"];
                cookie["login_form_show"] = true_false;
                Response.Cookies.Set(cookie);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            if (Convert.ToInt32(true_false) == 0)
            {
                LoginW.Hide();
            }
            else
            {
                LoginW.Show();
            }
        }
        //проверка соединения
        public Boolean TryConnection(String connection_stirng)
        {
            SqlConnection connection = new SqlConnection(connection_stirng);
            Boolean rez = false;
            try
            {
                connection.Open();
                rez = true;
            }
            catch (Exception exc)
            {
                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка соединения с БД",
                    Message = exc.Message + "<br />" + connection.ConnectionString + "<br />Настройте параметры подключения к базе данных!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
                X.Msg.Notify("Ошибка соединения с БД", exc.Message + "\n" + connection.ConnectionString + "\nНастройте параметры подключения к базе данных!").Show();
                rez = false;
            }
            finally
            {
                connection.Dispose();
            }
            return rez;
        }

        //меняет null в String.Empty
        [DirectMethod]
        public String NullToEmpty(String value)
        {
            if (value == "null" || value == null) { value = String.Empty; }
            return value;
        }

        [DirectMethod]
        public void RedirectToGoogle()
        {
            Response.Redirect("http://lmgtfy.com/?q=%D0%9A%D0%B0%D0%BA+%D0%B2%D0%BA%D0%BB%D1%8E%D1%87%D0%B8%D1%82%D1%8C+cookies+%D0%B2+%D0%B1%D1%80%D0%B0%D1%83%D0%B7%D0%B5%D1%80%D0%B5%3F");
        }

        //применение прав пользователя
        public void SetSecurity(login_data urd)
        {
            if (urd.read)
            {
                RegionP.Hidden = false;
                OrganizationP.Hidden = false;
                DepartmentP.Hidden = false;
            }
            else
            {
                RegionP.Hidden = true;
                OrganizationP.Hidden = true;
                DepartmentP.Hidden = true;
            }
            if (urd.edit)
            {
                ImportDataB.Hidden = false;
                //SettingsB.Hidden = false;
                EditRegionB.Hidden = false;
                EditOrganizationB.Hidden = false;
                EditDepartmentB.Hidden = false;
                EditPlotB.Hidden = false;
                RowEditing1.Visible = true;
                RowEditing2.Visible = true;
                RowEditing3.Visible = true;
                RowEditing4.Visible = true;
                RowEditing5.Visible = true;
                RowEditing6.Visible = true;
                RowEditing7.Visible = true;
                RowEditing8.Visible = true;
                RowEditing9.Visible = true;
                RowEditing10.Visible = true;
                RowEditing11.Visible = true;
                RowEditingUser.Visible = true;
                RowEditingPlans.Visible = true;
                RowEditingSurveyList.Visible = true;
                CopyMovePlotsB.Hidden = false;
                AnalysPhSNumberSampleTF.Hidden = false;
                AnalysPhSValuePhSTF.Hidden = false;
                AnalysPhSControlTF.Hidden = false;
                DeleteAnalysPhSColumn.Hidden = false;
                AnalysPKPanel2.Hidden = false;
                DeleteAnalysPKColumn.Hidden = false;
                AnalysHANumberSampleTF.Hidden = false;
                AnalysHAValuePhSTF.Hidden = false;
                AnalysHAValueHATF.Hidden = false;
                AnalysHAControlPhSTF.Hidden = false;
                AnalysHAControlTF.Hidden = false;
                DeleteAnalysHAColumn.Hidden = false;
                RowEditing13.Visible = true;
                RowEditing14.Visible = true;
                UpdateGroupsBySignificativeB.Hidden = false;
                AnalysToPlotB.Hidden = false;
            }
            else
            {
                ImportDataB.Hidden = true;
                //SettingsB.Hidden = true;
                EditRegionB.Hidden = true;
                EditOrganizationB.Hidden = true;
                EditDepartmentB.Hidden = true;
                EditPlotB.Hidden = true;
                RowEditing1.Visible = false;
                RowEditing2.Visible = false;
                RowEditing3.Visible = false;
                RowEditing4.Visible = false;
                RowEditing5.Visible = false;
                RowEditing6.Visible = false;
                RowEditing7.Visible = false;
                RowEditing8.Visible = false;
                RowEditing9.Visible = false;
                RowEditing10.Visible = false;
                RowEditing11.Visible = false;
                RowEditingUser.Visible = false;
                RowEditingPlans.Visible = false;
                RowEditingSurveyList.Visible = false;
                CopyMovePlotsB.Hidden = true;
                AnalysPhSNumberSampleTF.Hidden = true;
                AnalysPhSValuePhSTF.Hidden = true;
                AnalysPhSControlTF.Hidden = true;
                DeleteAnalysPhSColumn.Hidden = true;
                AnalysPKPanel2.Hidden = true;
                DeleteAnalysPKColumn.Hidden = true;
                AnalysHANumberSampleTF.Hidden = true;
                AnalysHAValuePhSTF.Hidden = true;
                AnalysHAValueHATF.Hidden = true;
                AnalysHAControlPhSTF.Hidden = true;
                AnalysHAControlTF.Hidden = true;
                DeleteAnalysHAColumn.Hidden = true;
                RowEditing13.Visible = false;
                RowEditing14.Visible = false;
                UpdateGroupsBySignificativeB.Hidden = true;
                AnalysToPlotB.Hidden = true;
            }
            if (urd.add)
            {
                AddRegionB.Hidden = false;
                AddOrganizationB.Hidden = false;
                AddDepartmentB.Hidden = false;
                AddPhoneB.Hidden = false;
                AddDepartmentB.Hidden = false;
                //AcceptAddRegionB.Hidden = false;
                AddPhoneB.Hidden = false;
                AddOldOrganizationB.Hidden = false;
                //AcceptAddOrganizationB.Hidden = false;
                //AcceptAddDepartmentB.Hidden = false;
                AddPlotB.Hidden = false;
                //AcceptAddPlotB.Hidden = false;
                AddCoordinatesB.Hidden = false;
                AddSoilB.Hidden = false;
                AddCultureB.Hidden = false;
                AddCropRotationB.Hidden = false;
                AddFarmlandB.Hidden = false;
                AddErosionB.Hidden = false;
                AddGradingB.Hidden = false;
                AddSlopeB.Hidden = false;
                AddExposureB.Hidden = false;
                AddSignificativeB.Hidden = false;
                AddGroupsB.Hidden = false;
                AddRoleB.Hidden = false;
                AddUserB.Hidden = false;
                AddPlansB.Hidden = false;
                AddSurveyListB.Hidden = false;
                /*AnalysPhSNumberSampleTF.Hidden = false;
                AnalysPhSValuePhSTF.Hidden = false;
                AnalysPhSControlTF.Hidden = false;
                AnalysPhSTB.Hidden = false;
                AnalysPKPanel2.Hidden = false;*/
                AddJobTitleB.Hidden = false;
                AddMissionB.Hidden = false;
            }
            else
            {
                AddRegionB.Hidden = true;
                AddOrganizationB.Hidden = true;
                AddDepartmentB.Hidden = true;
                AddPhoneB.Hidden = true;
                AddDepartmentB.Hidden = true;
                //AcceptAddRegionB.Hidden = true;
                AddPhoneB.Hidden = true;
                AddOldOrganizationB.Hidden = true;
                //AcceptAddOrganizationB.Hidden = true;
                //AcceptAddDepartmentB.Hidden = true;
                AddPlotB.Hidden = true;
                //AcceptAddPlotB.Hidden = true;
                AddCoordinatesB.Hidden = true;
                AddSoilB.Hidden = true;
                AddCultureB.Hidden = true;
                AddCropRotationB.Hidden = true;
                AddFarmlandB.Hidden = true;
                AddErosionB.Hidden = true;
                AddGradingB.Hidden = true;
                AddSlopeB.Hidden = true;
                AddExposureB.Hidden = true;
                AddSignificativeB.Hidden = true;
                AddGroupsB.Hidden = true;
                AddRoleB.Hidden = true;
                AddUserB.Hidden = true;
                AddPlansB.Hidden = true;
                AddSurveyListB.Hidden = true;
                /*AnalysPhSNumberSampleTF.Hidden = true;
                AnalysPhSValuePhSTF.Hidden = true;
                AnalysPhSControlTF.Hidden = true;
                AnalysPhSTB.Hidden = true;
                AnalysPKPanel2.Hidden = true;*/
                AddJobTitleB.Hidden = true;
                AddMissionB.Hidden = true;
            }
            if (urd.delete)
            {
                DeleteDepartmentB.Hidden = false;
                DeletePlotB.Hidden = false;
                DeletePhoneColumn.Hidden = false;
                DeleteOldOrganizationColumn.Hidden = false;
                DeleteEPlotColumn.Hidden = false;
                DeleteDotColumn.Hidden = false;
                DeleteSoilColumn.Hidden = false;
                DeleteCultureColumn.Hidden = false;
                DeleteCropRotationColumn.Hidden = false;
                DeleteFarmlandColumn.Hidden = false;
                DeleteErosionColumn.Hidden = false;
                DeleteGradingColumn.Hidden = false;
                DeleteSlopeColumn.Hidden = false;
                DeleteExposureColumn.Hidden = false;
                DeleteSignificativeColumn.Hidden = false;
                DeleteGroupColumn.Hidden = false;
                DeleteRoleColumn.Hidden = false;
                DeleteUserColumn.Hidden = false;
                DeleteOrganizationB.Hidden = false;
                DeletePlansB.Hidden = false;
                SurveyListDeleteIC.Hidden = false;
                DeleteJobTitleColumn.Hidden = false;
                DeleteMissionColumn.Hidden = false;
            }
            else
            {
                DeleteDepartmentB.Hidden = true;
                DeletePlotB.Hidden = true;
                DeletePhoneColumn.Hidden = true;
                DeleteOldOrganizationColumn.Hidden = true;
                DeleteEPlotColumn.Hidden = true;
                DeleteDotColumn.Hidden = true;
                DeleteSoilColumn.Hidden = true;
                DeleteCultureColumn.Hidden = true;
                DeleteCropRotationColumn.Hidden = true;
                DeleteFarmlandColumn.Hidden = true;
                DeleteErosionColumn.Hidden = true;
                DeleteGradingColumn.Hidden = true;
                DeleteSlopeColumn.Hidden = true;
                DeleteExposureColumn.Hidden = true;
                DeleteSignificativeColumn.Hidden = true;
                DeleteGroupColumn.Hidden = true;
                DeleteRoleColumn.Hidden = true;
                DeleteUserColumn.Hidden = true;
                DeleteOrganizationB.Hidden = true;
                DeletePlansB.Hidden = true;
                SurveyListDeleteIC.Hidden = true;
                DeleteJobTitleColumn.Hidden = true;
                DeleteMissionColumn.Hidden = true;
            }

            //Проверка разрешений
            /*String msg = urd.read.ToString() + urd.edit.ToString() + urd.add.ToString() + urd.delete.ToString();
            X.Msg.Show(new MessageBoxConfig
            {
                Title = "Предупреждение!!!",
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.INFO
            });*/
            
        }

        public String ReturnMaskRe(Int32 type)
        {
            String mask = String.Empty;
            if (type == 1) { mask = @"/[0-9]/"; }
            else if (type == 2) { mask = @"/[0-9.,]/"; }
            return mask;
        }

        public void SetMask()
        {
            EditFarmlandTF.MaskRe = ReturnMaskRe(1);
            EditCultureTF.MaskRe = ReturnMaskRe(1);
            EditOldCultureTF.MaskRe = ReturnMaskRe(1);
            EditTypeCropRotationTF.MaskRe = ReturnMaskRe(1);
            EditSoilTF.MaskRe = ReturnMaskRe(1);
            EditGradingTF.MaskRe = ReturnMaskRe(1);
            EditErosionTF.MaskRe = ReturnMaskRe(1);
            EditNumberCropRotationTF.MaskRe = ReturnMaskRe(1);
            //текстовые, маска не нужна
            //EditFieldTF.MaskRe = ReturnMaskRe(1);
            //NumberPlotTF.MaskRe = ReturnMaskRe(1);
            EditAreaTF.MaskRe = ReturnMaskRe(2);

            NTF.MaskRe = ReturnMaskRe(1);
            NO3TF.MaskRe = ReturnMaskRe(1);
            NO2TF.MaskRe = ReturnMaskRe(1);
            HumusTF.MaskRe = ReturnMaskRe(2);
            CapacityTF.MaskRe = ReturnMaskRe(2);
            TotalAbsorbedBaseTF.MaskRe = ReturnMaskRe(2);
            P2O5TF.MaskRe = ReturnMaskRe(1);
            K2OTF.MaskRe = ReturnMaskRe(1);
            PhSTF.MaskRe = ReturnMaskRe(2);
            PhWTF.MaskRe = ReturnMaskRe(2);
            HydrAcidTF.MaskRe = ReturnMaskRe(2);

            STF.MaskRe = ReturnMaskRe(2);
            MnTF.MaskRe = ReturnMaskRe(2);
            FeTF.MaskRe = ReturnMaskRe(2);
            CuTF.MaskRe = ReturnMaskRe(2);
            ZnTF.MaskRe = ReturnMaskRe(2);
            CoTF.MaskRe = ReturnMaskRe(2);
            AlTF.MaskRe = ReturnMaskRe(2);
            CaTF.MaskRe = ReturnMaskRe(2);
            MoTF.MaskRe = ReturnMaskRe(2);
            BTF.MaskRe = ReturnMaskRe(2);
            MgTF.MaskRe = ReturnMaskRe(2);
            NaTF.MaskRe = ReturnMaskRe(2);

            CuhmTF.MaskRe = ReturnMaskRe(2);
            ZnhmTF.MaskRe = ReturnMaskRe(2);
            CdhmTF.MaskRe = ReturnMaskRe(2);
            PbhmTF.MaskRe = ReturnMaskRe(2);
            NihmTF.MaskRe = ReturnMaskRe(2);
            HghmTF.MaskRe = ReturnMaskRe(2);
            MghmTF.MaskRe = ReturnMaskRe(2);
            CrhmTF.MaskRe = ReturnMaskRe(2);
            FehmTF.MaskRe = ReturnMaskRe(2);
            FhmTF.MaskRe = ReturnMaskRe(2);
            AshmTF.MaskRe = ReturnMaskRe(2);

            Cs137TF.MaskRe = ReturnMaskRe(2);
            Sr90TF.MaskRe = ReturnMaskRe(2);
            EditSlopeTF.MaskRe = ReturnMaskRe(1);
            EditExposureTF.MaskRe = ReturnMaskRe(1);

            //для элементарных участков
            HydrolyticAcidEPlotTF.MaskRe = HydrAcidTF.MaskRe;
            PhSEPlotTF.MaskRe = PhSTF.MaskRe;
            P2O5EPlotTF.MaskRe = P2O5TF.MaskRe;
            K2OEPlotTF.MaskRe = K2OTF.MaskRe;
        }

        public Int32 GetLastPlot(String id_dep, String year)
        {
            Int32 number_last_plot = 0;
            String numbers = "0123456789";
            String no_char = String.Empty;
            if (connection_try)
            {
                conn.Open();
                SqlCommand get_lasp_plot = new SqlCommand("Get_Last_Number_Plot_From_Department", conn);
                get_lasp_plot.CommandType = CommandType.StoredProcedure;
                get_lasp_plot.Parameters.AddWithValue("@id_department", id_dep);
                get_lasp_plot.Parameters.AddWithValue("@year", year);
                get_lasp_plot.Parameters.Add("@number_plot", SqlDbType.VarChar, 10);
                get_lasp_plot.Parameters["@number_plot"].Direction = ParameterDirection.Output;
                get_lasp_plot.ExecuteNonQuery();
                for (int i = 0; i < get_lasp_plot.Parameters["@number_plot"].Value.ToString().Length; i++ )
                {
                    if (numbers.Split(get_lasp_plot.Parameters["@number_plot"].Value.ToString()[i]).Length == 2)
                    {
                        no_char += get_lasp_plot.Parameters["@number_plot"].Value.ToString()[i];
                    }
                    if (numbers.Split(get_lasp_plot.Parameters["@number_plot"].Value.ToString()[i]).Length <= 1)
                    {
                        number_last_plot = (-1);
                        break;
                    }
                }
                if (number_last_plot != (-1) && no_char != String.Empty)
                {
                    number_last_plot = Convert.ToInt32(no_char);
                }
                conn.Close();
            }
            return number_last_plot;
        }

        [DirectMethod]
        public void MaskChange()
        {
            String temp = HydrAcidTF.Text;
            foreach (Char znak in temp)
            {
                if (znak == ',' || znak == '.')
                {
                    HydrAcidTF.MaskRe = ReturnMaskRe(1);
                }
                else
                {
                    HydrAcidTF.MaskRe = ReturnMaskRe(2);
                }
            }
        }

        [DirectMethod]
        public void EditHAEPlot(String value)
        {
            RowSelectionModel sm = this.EditEPlotGP.GetSelectionModel() as RowSelectionModel;
            EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("hydrolytic_acid", AdaptationValue(value, "hydrolytic_acid"));
        }

        [DirectMethod]
        public void EditPhSEPlot(String value)
        {
            String rep_value = AdaptationValue(value, "ph_s");
            RowSelectionModel sm = this.EditEPlotGP.GetSelectionModel() as RowSelectionModel;
            EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("ph_s", rep_value);
            if (rep_value != null && rep_value != String.Empty && String.Compare(rep_value,"null") != 0)
            {
                DataTable dt_method_eplot = indexDS.Tables["Method"];
                if (dt_method_eplot.Select("condition='0' AND from_pH<'" + rep_value + "' AND to_pH>='" + rep_value + "'").Length > 0)
                {
                    EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("id_method", dt_method_eplot.Select("condition='0' AND from_pH<'" + rep_value + "' AND to_pH>='" + rep_value + "'")[0]["id_method"].ToString());
                    EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("title_method", dt_method_eplot.Select("condition='0' AND from_pH<'" + rep_value + "' AND to_pH>='" + rep_value + "'")[0]["title_method"].ToString());
                }
                else
                {
                    EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("id_method", "");
                    EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("title_method", "");
                }
            }
            else
            {
                EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("id_method", "");
                EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("title_method", "");
            }
        }

        [DirectMethod]
        public void EditP2O5EPlot(String value)
        {
            RowSelectionModel sm = this.EditEPlotGP.GetSelectionModel() as RowSelectionModel;
            EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("p2o5", AdaptationValue(value, "p2o5"));
        }

        [DirectMethod]
        public void EditK2OEPlot(String value)
        {
            RowSelectionModel sm = this.EditEPlotGP.GetSelectionModel() as RowSelectionModel;
            EditEPlotGP.GetStore().GetAt(sm.SelectedRow.RowIndex).Set("k2o", AdaptationValue(value, "k2o"));
        }

        [DirectMethod]
        public void AddEPlotBFocus()
        {
            AddEPlotB.Cls = "x-btn-focusClass";
            AddEPlotB.ReRender();
            AddEPlotB.Focus();
        }

        [DirectMethod]
        public void AddEPlotBRedBorder()
        {
            AddEPlotB.Cls = "x-btn-focusClass";
            AddEPlotB.ReRender();
        }

        [DirectMethod]
        public void AddEPlotBBlur()
        {
            AddEPlotB.Cls = "x-btn-default";
            AddEPlotB.ReRender();
        }

        public void AddEditEplotFromPlotByHA(String count_eplot)
        {
            String hydr_acid = ReplaceValue(HydrAcidTF.Text, 2.ToString());
            if (Convert.ToInt32(count_eplot) == 0 && hydr_acid != null && hydr_acid != String.Empty && hydr_acid != "null")
            {
                EditEPlotGP.GetStore().Add(new List<object>
                                              {
                                                  new {id_elementary_plot = "", number_elementary_plot = "", p2o5 = "", k2o= "", ph_s= "", ph_w= "", hydrolytic_acid= "", code_type_soil= "", code_grading= "", code_erosion= "",
                                                       code_slope= "", code_exposure= "", n= "", no3= "", no2= "", humus= "", absorbance_capacity= "", total_absorbed_bases= "", base_saturation= "",
                                                       id_priority_calcification= "", s= "", ca= "", mn= "", mo= "", b= "", cu= "", mg= "", zn= "", na= "", co= "", al= "", fe= "", cu_hm= "",
                                                       zn_hm= "", cd_hm= "", pb_hm= "", ni_hm= "", hg_hm= "", mg_hm= "", cr_hm= "", fe_hm= "", f_hm= "", as_hm= "", cs137= "", sr90= ""
                                                  }
                                              });
                EditEPlotGP.GetStore().GetAt(0).Set("id_elementary_plot", "-1");
                EditEPlotGP.GetStore().GetAt(0).Set("hydrolytic_acid", ReplaceValue(hydr_acid, "2"));
            }
            else if (Convert.ToInt32(count_eplot) == 1)
            {
                EditEPlotGP.GetStore().GetAt(0).Set("hydrolytic_acid", ReplaceValue(hydr_acid, "2"));
            }
        }

        public void AddEditEplotFromPlotByPhS(String count_eplot)
        {
            String temp = AdaptationValue(PhSTF.Text, "ph_s");
            if (Convert.ToInt32(count_eplot) == 0 && temp != null && temp != String.Empty && temp != "null")
            {
                EditEPlotGP.GetStore().Add(new List<object>
                                              {
                                                  new {id_elementary_plot = "", number_elementary_plot = "", p2o5 = "", k2o= "", ph_s= "", ph_w= "", hydrolytic_acid= "", code_type_soil= "", code_grading= "", code_erosion= "",
                                                       code_slope= "", code_exposure= "", n= "", no3= "", no2= "", humus= "", absorbance_capacity= "", total_absorbed_bases= "", base_saturation= "",
                                                       id_priority_calcification= "", s= "", ca= "", mn= "", mo= "", b= "", cu= "", mg= "", zn= "", na= "", co= "", al= "", fe= "", cu_hm= "",
                                                       zn_hm= "", cd_hm= "", pb_hm= "", ni_hm= "", hg_hm= "", mg_hm= "", cr_hm= "", fe_hm= "", f_hm= "", as_hm= "", cs137= "", sr90= ""
                                                  }
                                              });
                EditEPlotGP.GetStore().GetAt(0).Set("id_elementary_plot", "-1");
                EditEPlotGP.GetStore().GetAt(0).Set("ph_s", temp);
            }
            else if (Convert.ToInt32(count_eplot) == 1)
            {
                EditEPlotGP.GetStore().GetAt(0).Set("ph_s", temp);
            }
            if (Convert.ToInt32(count_eplot) == 1)
            {
                if (temp != null && temp != String.Empty && temp != "null")
                {
                    DataTable dt_method = indexDS.Tables["Method"];
                    if (dt_method.Select("condition='0' AND from_pH<'" + temp + "' AND to_pH>='" + temp + "'").Length > 0)
                    {
                        EditEPlotGP.GetStore().GetAt(0).Set("id_method", dt_method.Select("condition='0' AND from_pH<'" + temp + "' AND to_pH>='" + temp + "'")[0]["id_method"].ToString());
                        EditEPlotGP.GetStore().GetAt(0).Set("title_method", dt_method.Select("condition='0' AND from_pH<'" + temp + "' AND to_pH>='" + temp + "'")[0]["title_method"].ToString());
                    }
                    else
                    {
                        EditEPlotGP.GetStore().GetAt(0).Set("id_method", "");
                        EditEPlotGP.GetStore().GetAt(0).Set("title_method", "");
                    }
                }
                else
                {
                    EditEPlotGP.GetStore().GetAt(0).Set("id_method", "");
                    EditEPlotGP.GetStore().GetAt(0).Set("title_method", "");
                }
            }
        }

        public void AddEditEplotFromPlotByP2O5(String count_eplot)
        {
            String temp = ReplaceValue(P2O5TF.Text, 1.ToString());
            if (Convert.ToInt32(count_eplot) == 0 && temp != null && temp != String.Empty && temp != "null")
            {
                EditEPlotGP.GetStore().Add(new List<object>
                                              {
                                                  new {id_elementary_plot = "", number_elementary_plot = "", p2o5 = "", k2o= "", ph_s= "", ph_w= "", hydrolytic_acid= "", code_type_soil= "", code_grading= "", code_erosion= "",
                                                       code_slope= "", code_exposure= "", n= "", no3= "", no2= "", humus= "", absorbance_capacity= "", total_absorbed_bases= "", base_saturation= "",
                                                       id_priority_calcification= "", s= "", ca= "", mn= "", mo= "", b= "", cu= "", mg= "", zn= "", na= "", co= "", al= "", fe= "", cu_hm= "",
                                                       zn_hm= "", cd_hm= "", pb_hm= "", ni_hm= "", hg_hm= "", mg_hm= "", cr_hm= "", fe_hm= "", f_hm= "", as_hm= "", cs137= "", sr90= ""
                                                  }
                                              });
                EditEPlotGP.GetStore().GetAt(0).Set("id_elementary_plot", "-1");
                EditEPlotGP.GetStore().GetAt(0).Set("p2o5", temp);
            }
            else if (Convert.ToInt32(count_eplot) == 1)
            {
                EditEPlotGP.GetStore().GetAt(0).Set("p2o5", temp);
            }
        }

        public void AddEditEplotFromPlotByK2O(String count_eplot)
        {
            String temp = ReplaceValue(K2OTF.Text, 1.ToString());
            if (Convert.ToInt32(count_eplot) == 0 && temp != null && temp != String.Empty && temp != "null")
            {
                EditEPlotGP.GetStore().Add(new List<object>
                                              {
                                                  new {id_elementary_plot = "", number_elementary_plot = "", p2o5 = "", k2o= "", ph_s= "", ph_w= "", hydrolytic_acid= "", code_type_soil= "", code_grading= "", code_erosion= "",
                                                       code_slope= "", code_exposure= "", n= "", no3= "", no2= "", humus= "", absorbance_capacity= "", total_absorbed_bases= "", base_saturation= "",
                                                       id_priority_calcification= "", s= "", ca= "", mn= "", mo= "", b= "", cu= "", mg= "", zn= "", na= "", co= "", al= "", fe= "", cu_hm= "",
                                                       zn_hm= "", cd_hm= "", pb_hm= "", ni_hm= "", hg_hm= "", mg_hm= "", cr_hm= "", fe_hm= "", f_hm= "", as_hm= "", cs137= "", sr90= ""
                                                  }
                                              });
                EditEPlotGP.GetStore().GetAt(0).Set("id_elementary_plot", "-1");
                EditEPlotGP.GetStore().GetAt(0).Set("k2o", temp);
            }
            else if (Convert.ToInt32(count_eplot) == 1)
            {
                EditEPlotGP.GetStore().GetAt(0).Set("k2o", temp);
            }
        }

        [DirectMethod]
        public void GroupsByTFReport(String tour)
        {
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                if (cookie_report_tours == null)
                {
                    cookie_report_tours = new HttpCookie("Agrochim31_ReportTours");
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    cookie_report_tours.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie_report_tours);
                }
                else
                {
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    Response.Cookies.Set(cookie_report_tours);
                }
                win_2_1.Reload();
                win_2_1.Show();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void GroupsByTFHMReport(String tour)
        {
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                if (cookie_report_tours == null)
                {
                    cookie_report_tours = new HttpCookie("Agrochim31_ReportTours");
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    cookie_report_tours.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie_report_tours);
                }
                else
                {
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    Response.Cookies.Set(cookie_report_tours);
                }
                win_2_2.Reload();
                win_2_2.Show();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void PriorityCalcificationReport(String tour)
        {
            if (Request.Browser.Cookies)
            {
                cookie_report_tours = Request.Cookies["Agrochim31_ReportTours"];
                if (cookie_report_tours == null)
                {
                    cookie_report_tours = new HttpCookie("Agrochim31_ReportTours");
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    cookie_report_tours.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie_report_tours);
                }
                else
                {
                    cookie_report_tours["id"] = current_id_organization;
                    cookie_report_tours["tour"] = tour;
                    Response.Cookies.Set(cookie_report_tours);
                }
                win_2_4.Reload();
                win_2_4.Show();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        /*[DirectMethod]
        public void ClearPass()
        {
            PasswordUserTF.Text = "";
        }*/

        [DirectMethod]
        public void GetCurrentPassword(String password)
        {
            if (Request.Browser.Cookies)
            {
                if (password == "" || password == null || password == String.Empty) { password = "0"; }
                cookie = Request.Cookies["Agrochim31"];
                cookie["current_password"] = password;
                Response.Cookies.Set(cookie);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
            current_password = password;
        }

        [DirectMethod]
        public void BindMethod()
        {
            if (connection_try)
            {
                conn.Open();
                adapterMethod = new SqlDataAdapter(selectCommMethod, conn);
                adapterMethod.Fill(indexDS, "Method");
                //indexDV = new System.Data.DataView(indexDS.Tables["Method"]);
                MethodGroupS.DataSource = new System.Data.DataView(indexDS.Tables["Method"]);
                //MethodGroupS.DataBind();
                conn.Close();
            }
        }

        /*[DirectMethod]
        public void LoadMethod()
        {
            if (connection_try)
            {
                conn.Open();
                adapterMethod = new SqlDataAdapter(selectCommMethod, conn);
                adapterMethod.Fill(indexDS, "Method");
                indexDV = new System.Data.DataView(indexDS.Tables["Method"]);
                MethodGroupS.DataSource = indexDV;
                MethodGroupS.DataBind();
                conn.Close();
            }
        }*/
        
        [DirectMethod]
        public void ShowUserW()
        {
            BindRole();
            SelectRole();
            UserW.Show();
        }

        [DirectMethod]
        public void BindJobTitleUser()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterJobTitleUser = new SqlDataAdapter(selectCommJobTitle, conn);
                adapterJobTitleUser.Fill(indexDS, "JobTitleUser");
                indexDV = new System.Data.DataView(indexDS.Tables["JobTitleUser"]);
                JobTitleUserS.DataSource = indexDV;
                JobTitleUserS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void BindDivisionsUser()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterDivisionsUser = new SqlDataAdapter(selectCommDivisions, conn);
                adapterDivisionsUser.Fill(indexDS, "DivisionsUser");
                indexDV = new System.Data.DataView(indexDS.Tables["DivisionsUser"]);
                DivisionUserS.DataSource = indexDV;
                DivisionUserS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void ShowSignificativeW()
        {
            BindSignificative();
            SelectSignificative();
            SignificativeW.Show();
        }
        
        [DirectMethod]
        public void ShowJobTitleW()
        {
            BindJobTitle();
            JobTitleW.Show();
        }

        [DirectMethod]
        public void ShowMissionW()
        {
            BindMission();
            MissionW.Show();
        }

        [DirectMethod]
        public void ShowTrackersW()
        {
            BindTrackers();
            TrackersW.Show();
        }

        [DirectMethod]
        public void ShowCarsW()
        {
            BindTerritory();
            BindCars();
            CarsW.Show();
        }
        
        [DirectMethod]
        public void ShowSoilW()
        {
            BindTypeSoil();
            SoilW.Show();
        }

        [DirectMethod]
        public void ShowCultureW()
        {
            BindCulture();
            CultureW.Show();
        }

        
        [DirectMethod]
        public void ShowCropRotationW()
        {
            BindTypeCropRotation();
            CropRotationW.Show();
        }

        [DirectMethod]
        public void ShowFarmlandW()
        {
            BindTypeFarmland();
            FarmlandW.Show();
        }

        [DirectMethod]
        public void ShowErosionW()
        {
            BindTypeErosion();
            ErosionW.Show();
        }

        [DirectMethod]
        public void ShowGradingW()
        {
            BindTypeGrading();
            GradingW.Show();
        }

        [DirectMethod]
        public void ShowSlopeW()
        {
            BindSlope();
            SlopeW.Show();
        }

        [DirectMethod]
        public void ShowExposureW()
        {
            BindExposure();
            ExposureW.Show();
        }

        //получение и усреднение данных по элементарным участкам
        [DirectMethod]
        public void GetAvgForPlot(String data_eplots, String data_sign)
        {
            //создаём массивы данных по эл. уч. и элементам
            List<Dictionary<String, String>> records = JSON.Deserialize<List<Dictionary<String, String>>>(data_eplots);
            List<Dictionary<String, String>> records_sign = JSON.Deserialize<List<Dictionary<String, String>>>(data_sign);
            if (records_sign.Count > 0 && records.Count > 0)
            {
                Double ha_d = 0, ph_s_d = 0, p2o5_d = 0, k2o_d = 0;
                Int32 count_value_ha = 0, count_value_ph_s = 0, count_value_p2o5 = 0, count_value_k2o = 0;
                Int32 nod_ha = 0, nod_ph_s = 0, nod_p2o5 = 0, nod_k2o = 0, method_for_avg = 0;

                for (int i = 0; i < records_sign.Count; i++)
                {
                    if (String.Compare(records_sign[i]["name_significative"], "hydrolytic_acid") == 0)
                    {
                        nod_ha = Convert.ToInt32(records_sign[i]["number_of_digits"].ToString());
                    }
                    if (String.Compare(records_sign[i]["name_significative"], "ph_s") == 0)
                    {
                        nod_ph_s = Convert.ToInt32(records_sign[i]["number_of_digits"].ToString());
                    }
                    if (String.Compare(records_sign[i]["name_significative"], "p2o5") == 0)
                    {
                        nod_p2o5 = Convert.ToInt32(records_sign[i]["number_of_digits"].ToString());
                    }
                    if (String.Compare(records_sign[i]["name_significative"], "k2o") == 0)
                    {
                        nod_k2o = Convert.ToInt32(records_sign[i]["number_of_digits"].ToString());
                    }
                }

                String[][] values = new String[4][];
                values[0] = new String[records.Count];
                values[1] = new String[records.Count];
                values[2] = new String[records.Count];
                values[3] = new String[records.Count];

                if (records.Count > 1)
                {
                    for (int i = 0; i < records.Count; i++)
                    {
                        if (records[i]["ph_s"] != String.Empty && records[i]["ph_s"] != null && records[i]["ph_s"] != "null")
                        {
                            count_value_ph_s += 1;
                            ph_s_d += Convert.ToDouble(NotNull(ReplaceValue(records[i]["ph_s"], 2.ToString())));
                            values[0][i] = records[i]["ph_s"].ToString();
                        }
                        if (records[i]["hydrolytic_acid"] != String.Empty && records[i]["hydrolytic_acid"] != null
                            && records[i]["hydrolytic_acid"] != "" && records[i]["hydrolytic_acid"] != "null")
                        {
                            count_value_ha += 1;
                            ha_d += Convert.ToDouble(NotNull(ReplaceValue(records[i]["hydrolytic_acid"].ToString(), 2.ToString())));
                            values[1][i] = records[i]["hydrolytic_acid"].ToString();
                        }
                    }
                    if (count_value_ph_s != 0) { ph_s_d = Round(ph_s_d / count_value_ph_s, nod_ph_s); } else { ph_s_d = 0; }
                    if (count_value_ha != 0) { ha_d = Round(ha_d / count_value_ha, nod_ha); } else { ha_d = 0; }


                    if (connection_try)
                    {
                        String method_str = String.Empty;
                        DataTable dt_method_plot = indexDS.Tables["Method"];
                        if (dt_method_plot.Select("condition='0' AND from_pH<'" + ph_s_d.ToString() + "' AND to_pH>='" + ph_s_d.ToString() + "'").Length > 0)
                        {
                            method_str = dt_method_plot.Select("condition='0' AND from_pH<'" + ph_s_d.ToString() + "' AND to_pH>='" + ph_s_d.ToString() + "'")[0]["id_method"].ToString();
                        }

                        if (method_str != String.Empty && String.Compare(method_str, "null") != 0 && method_str != null)
                        {
                            method_for_avg = Convert.ToInt32(method_str);
                        }
                        else
                        {
                            method_for_avg = 1;
                        }

                        conn.Open();
                        SqlCommand get_value_from_method;

                        for (int i = 0; i < records.Count; i++)
                        {
                            get_value_from_method = new SqlCommand("Get_Value_From_Method", conn);
                            if (records[i]["ph_s"] != String.Empty && records[i]["ph_s"] != null && records[i]["ph_s"] != "null")
                            {
                                if (records[i]["p2o5"] != String.Empty && records[i]["p2o5"] != null && records[i]["p2o5"] != "null")
                                {
                                    count_value_p2o5 += 1;

                                    get_value_from_method = new SqlCommand("Get_Value_From_Method", conn);
                                    get_value_from_method.CommandType = CommandType.StoredProcedure;
                                    get_value_from_method.Parameters.AddWithValue("@ph_s", Convert.ToDouble(NotNull(ReplaceValue(records[i]["ph_s"].ToString(), 2.ToString()))));
                                    get_value_from_method.Parameters.AddWithValue("@name_significative", "p2o5");
                                    get_value_from_method.Parameters.AddWithValue("@value_old", Convert.ToDouble(NotNull(ReplaceValue(records[i]["p2o5"].ToString(), 2.ToString()))));
                                    get_value_from_method.Parameters.AddWithValue("@id_method_avg", method_for_avg);
                                    get_value_from_method.Parameters.Add("@value_new", SqlDbType.Float);
                                    get_value_from_method.Parameters["@value_new"].Direction = ParameterDirection.Output;
                                    get_value_from_method.ExecuteNonQuery();

                                    p2o5_d += Convert.ToDouble(NotNull(ReplaceValue(get_value_from_method.Parameters["@value_new"].Value.ToString(), 2.ToString())));
                                    values[2][i] = get_value_from_method.Parameters["@value_new"].Value.ToString();
                                }
                                if (records[i]["k2o"] != String.Empty && records[i]["k2o"] != null && records[i]["k2o"] != "null")
                                {
                                    count_value_k2o += 1;

                                    get_value_from_method = new SqlCommand("Get_Value_From_Method", conn);
                                    get_value_from_method.CommandType = CommandType.StoredProcedure;
                                    get_value_from_method.Parameters.AddWithValue("@ph_s", Convert.ToDouble(NotNull(ReplaceValue(records[i]["ph_s"].ToString(), 2.ToString()))));
                                    get_value_from_method.Parameters.AddWithValue("@name_significative", "k2o");
                                    get_value_from_method.Parameters.AddWithValue("@value_old", Convert.ToDouble(NotNull(ReplaceValue(records[i]["k2o"].ToString(), 2.ToString()))));
                                    get_value_from_method.Parameters.AddWithValue("@id_method_avg", method_for_avg);
                                    get_value_from_method.Parameters.Add("@value_new", SqlDbType.Float);
                                    get_value_from_method.Parameters["@value_new"].Direction = ParameterDirection.Output;
                                    get_value_from_method.ExecuteNonQuery();

                                    k2o_d += Convert.ToDouble(NotNull(ReplaceValue(get_value_from_method.Parameters["@value_new"].Value.ToString(), 2.ToString())));
                                    values[3][i] = get_value_from_method.Parameters["@value_new"].Value.ToString();
                                }
                            }
                        }
                        if (count_value_p2o5 != 0) { p2o5_d = Round(p2o5_d / count_value_p2o5, nod_p2o5); } else { p2o5_d = 0; }
                        if (count_value_k2o != 0) { k2o_d = Round(k2o_d / count_value_k2o, nod_k2o); } else { k2o_d = 0; }
                        conn.Close();
                    }
                }
                else
                {
                    if (records[0]["ph_s"] != String.Empty && records[0]["ph_s"] != null && records[0]["ph_s"] != "null")
                    {
                        ph_s_d = Convert.ToDouble(NotNull(ReplaceValue(records[0]["ph_s"].ToString(), 2.ToString())));
                    }
                    if (records[0]["hydrolytic_acid"] != String.Empty && records[0]["hydrolytic_acid"] != null && records[0]["hydrolytic_acid"] != "null")
                    {
                        ha_d = Convert.ToDouble(NotNull(ReplaceValue(records[0]["hydrolytic_acid"].ToString(), 2.ToString())));
                    }
                    if (records[0]["p2o5"] != String.Empty && records[0]["p2o5"] != null && records[0]["p2o5"] != "null")
                    {
                        p2o5_d = Convert.ToDouble(NotNull(ReplaceValue(records[0]["p2o5"], 2.ToString())));
                    }
                    if (records[0]["k2o"] != String.Empty && records[0]["k2o"] != null && records[0]["k2o"] != "null")
                    {
                        k2o_d = Convert.ToDouble(NotNull(ReplaceValue(records[0]["k2o"].ToString(), 2.ToString())));
                    }
                }
                if (ha_d > 0) { HydrAcidTF.Text = ha_d.ToString(); }
                if (ph_s_d > 0) { PhSTF.Text = ph_s_d.ToString(); }
                if (p2o5_d > 0) { P2O5TF.Text = p2o5_d.ToString(); }
                if (k2o_d > 0) { K2OTF.Text = k2o_d.ToString(); }

                String v1 = String.Empty, v2 = String.Empty, v3 = String.Empty, v4 = String.Empty;
                for (int i = 0; i < records.Count; i++)
                {
                    v1 += ("  " + values[0][i]);
                    v2 += ("  " + values[1][i]);
                    v3 += ("  " + values[2][i]);
                    v4 += ("  " + values[3][i]);
                }

                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Предупреждение!!!",
                    Message = data_eplots + "<br />" + v1 + "<br />" + v2 + "<br />" + v3 + "<br />" + v4,
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
            }
            else
            {
                HydrAcidTF.Text = String.Empty;
                PhSTF.Text = String.Empty;
                P2O5TF.Text = String.Empty;
                K2OTF.Text = String.Empty;
            }
            ShowMethod(records.Count.ToString());
        }

        [DirectMethod]
        public void CloseEditPlot(String id_plot)
        {
            if (id_plot != String.Empty && connection_try)
            {
                FlagEditing plotFE = new FlagEditing(conn, "Plot", Convert.ToInt32(id_plot));
                plotFE.DeleteFlag();
            }
        }

        [DirectMethod]
        public void FocusOnAddYear(String new_tour)
        {
            if (new_tour != null && new_tour != "null" && new_tour != String.Empty && new_tour != "")
            {
                AddYearB.Focus();
            }
        }

        [DirectMethod]
        public void FocusOnAddPlot(String new_year)
        {
            if (new_year != null && new_year != "null" && new_year != String.Empty && new_year != "")
            {
                AddPlotB.Focus();
            }
        }

        [DirectMethod]
        public void CopyMoveWindow()
        {
            CodeDepartmentFromTF.Text = String.Empty;
            TourFromTF.Text = String.Empty;
            YearFromTF.Text = String.Empty;
            CopyMovePlotsTF.Text = String.Empty;
            CodeDepartmentToTF.Text = String.Empty;
            TourToTF.Text = String.Empty;
            YearToTF.Text = String.Empty;
            CopyMovePlotW.Show();
            CodeDepartmentFromTF.Focus();
            CodeDepartmentFromTF.SelectText();
        }

        [DirectMethod]
        public void CloseCopyMoveW()
        {
            CopyMovePlotW.Close();
        }

        [DirectMethod]
        public void CopyPlots(String plots, String code_dep_from, String code_dep_to, String tour_from, String tour_to, String year_from, String year_to)
        {
            if (code_dep_from != String.Empty && code_dep_to != String.Empty &&
               tour_from != String.Empty && tour_to != String.Empty && year_from != String.Empty && year_to != String.Empty && connection_try)
            {
                conn.Open();
                if (plots == String.Empty || plots == "null" || plots == null)
                {
                    SqlCommand get_all_plots = new SqlCommand("Get_All_Plots", conn);
                    get_all_plots.CommandType = CommandType.StoredProcedure;
                    get_all_plots.CommandTimeout = 60;
                    get_all_plots.Parameters.AddWithValue("@code_department", Convert.ToInt32(code_dep_from));
                    get_all_plots.Parameters.AddWithValue("@tour", Convert.ToInt32(tour_from));
                    get_all_plots.Parameters.AddWithValue("@year", Convert.ToInt32(year_from));
                    get_all_plots.Parameters.Add("@plots", SqlDbType.VarChar, 2000);
                    get_all_plots.Parameters["@plots"].Direction = ParameterDirection.Output;
                    get_all_plots.ExecuteNonQuery();
                    plots = get_all_plots.Parameters["@plots"].Value.ToString();
                }

                String[] plots_array = plots.Replace('.', ',').Split(',');

                for (int i = 0; i < plots_array.Length; i++)
                {
                    for (int j = 0; j < plots_array.Length; j++)
                    {
                        if (String.Compare(plots_array[j], plots_array[i]) == 0 && i != j)
                        {
                            plots_array[j] = String.Empty;
                        }
                    }
                    if (plots_array[i] != String.Empty && plots_array[i]!="NULL")
                    {
                        SqlCommand copy_plots = new SqlCommand("Copy_Plot_From_To", conn);
                        copy_plots.CommandType = CommandType.StoredProcedure;
                        copy_plots.CommandTimeout = 60;
                        copy_plots.Parameters.AddWithValue("@code_dep", Convert.ToInt32(code_dep_from));
                        copy_plots.Parameters.AddWithValue("@nu", plots_array[i]);
                        copy_plots.Parameters.AddWithValue("@tour", Convert.ToInt32(tour_from));
                        copy_plots.Parameters.AddWithValue("@year", Convert.ToInt32(year_from));
                        copy_plots.Parameters.AddWithValue("@code_dep_to", Convert.ToInt32(code_dep_to));
                        copy_plots.Parameters.AddWithValue("@tour_to", Convert.ToInt32(tour_to));
                        copy_plots.Parameters.AddWithValue("@year_to", Convert.ToInt32(year_to));
                        copy_plots.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                        copy_plots.ExecuteNonQuery();
                    }
                }
                conn.Close();
                CopyMovePlotW.Close();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Предупреждение!!!",
                    Message ="Не все поля заполнены!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }
        [DirectMethod]
        public void MovePlots(String plots, String code_dep_from, String code_dep_to, String tour_from, String tour_to, String year_from, String year_to)
        {
            if (code_dep_from != String.Empty && code_dep_to != String.Empty &&
               tour_from != String.Empty && tour_to != String.Empty && year_from != String.Empty && year_to != String.Empty && connection_try)
            {
                conn.Open();
                if (plots == String.Empty || plots == "null" || plots == null)
                {
                    SqlCommand get_all_plots = new SqlCommand("Get_All_Plots", conn);
                    get_all_plots.CommandType = CommandType.StoredProcedure;
                    get_all_plots.CommandTimeout = 60;
                    get_all_plots.Parameters.AddWithValue("@code_department", Convert.ToInt32(code_dep_from));
                    get_all_plots.Parameters.AddWithValue("@tour", Convert.ToInt32(tour_from));
                    get_all_plots.Parameters.AddWithValue("@year", Convert.ToInt32(year_from));
                    get_all_plots.Parameters.Add("@plots", SqlDbType.VarChar, 2000);
                    get_all_plots.Parameters["@plots"].Direction = ParameterDirection.Output;
                    get_all_plots.ExecuteNonQuery();
                    plots = get_all_plots.Parameters["@plots"].Value.ToString();
                }

                String[] plots_array = plots.Replace('.', ',').Split(',');

                for (int i = 0; i < plots_array.Length; i++)
                {
                    for (int j = 0; j < plots_array.Length; j++)
                    {
                        if (String.Compare(plots_array[j], plots_array[i]) == 0 && i != j)
                        {
                            plots_array[j] = String.Empty;
                        }
                    }
                    if (plots_array[i] != String.Empty)
                    {
                        SqlCommand move_plots = new SqlCommand("Move_Plot_From_To", conn);
                        move_plots.CommandType = CommandType.StoredProcedure;
                        move_plots.CommandTimeout = 120;
                        move_plots.Parameters.AddWithValue("@code_dep", Convert.ToInt32(code_dep_from));
                        move_plots.Parameters.AddWithValue("@nu", plots_array[i]);
                        move_plots.Parameters.AddWithValue("@tour", Convert.ToInt32(tour_from));
                        move_plots.Parameters.AddWithValue("@year", Convert.ToInt32(year_from));
                        move_plots.Parameters.AddWithValue("@code_dep_to", Convert.ToInt32(code_dep_to));
                        move_plots.Parameters.AddWithValue("@tour_to", Convert.ToInt32(tour_to));
                        move_plots.Parameters.AddWithValue("@year_to", Convert.ToInt32(year_to));
                        move_plots.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                        move_plots.ExecuteNonQuery();
                    }
                }
                conn.Close();
                CopyMovePlotW.Close();
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Предупреждение!!!",
                    Message = "Не все поля заполнены!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        //Планы-задания
        [DirectMethod]
        public void BindPlansData(String str_date_from, String str_date_to, String str_date_check_probes, String str_date_check_maps, String str_date_check_points, String str_date_check_act)
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterPlansWorker = new SqlDataAdapter(selectCommPlansWorker, conn);
                adapterPlansWorker.Fill(indexDS, "Plans_Workers");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Workers"]);
                EditPlansWorkerS.DataSource = indexDV;
                EditPlansWorkerS.DataBind();
                SqlDataAdapter adapterPlansRegion = new SqlDataAdapter(selectCommRegion, conn);
                adapterPlansRegion.Fill(indexDS, "Plans_Regions");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Regions"]);
                EditPlansRegionS.DataSource = indexDV;
                EditPlansRegionS.DataBind();
                SqlDataAdapter adapterPlansMissions = new SqlDataAdapter(selectCommPlansMissions, conn);
                adapterPlansMissions.Fill(indexDS, "Plans_Missions");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Missions"]);
                EditPlansMissionS.DataSource = indexDV;
                EditPlansMissionS.DataBind();
                SqlDataAdapter adapterPlansChief = new SqlDataAdapter(selectCommChief, conn);
                adapterPlansChief.Fill(indexDS, "Plans_Chief");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Chief"]);
                EditPlansChiefS.DataSource = indexDV;
                EditPlansChiefS.DataBind();
                SqlDataAdapter adapterPlansMatcher = new SqlDataAdapter(selectCommMatcher, conn);
                adapterPlansMatcher.Fill(indexDS, "Plans_Matcher");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Matcher"]);
                EditPlansMatcherS.DataSource = indexDV;
                EditPlansMatcherS.DataBind();
                SqlDataAdapter adapterPlansCheckProbes = new SqlDataAdapter(selectCommCheckProbes, conn);
                adapterPlansCheckProbes.Fill(indexDS, "Plans_Check_Probes");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Check_Probes"]);
                EditPlansCheckProbesS.DataSource = indexDV;
                EditPlansCheckProbesS.DataBind();
                SqlDataAdapter adapterPlansCheckMaps = new SqlDataAdapter(selectCommCheckMaps, conn);
                adapterPlansCheckMaps.Fill(indexDS, "Plans_Check_Maps");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Check_Maps"]);
                EditPlansCheckMapsS.DataSource = indexDV;
                EditPlansCheckMapsS.DataBind();
                SqlDataAdapter adapterPlansCheckPoints = new SqlDataAdapter(selectCommCheckPoints, conn);
                adapterPlansCheckPoints.Fill(indexDS, "Plans_Check_Points");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Check_Points"]);
                EditPlansCheckPointsS.DataSource = indexDV;
                EditPlansCheckPointsS.DataBind();
                SqlDataAdapter adapterPlansCheckAct = new SqlDataAdapter(selectCommCheckAct, conn);
                adapterPlansCheckAct.Fill(indexDS, "Plans_Check_Act");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Check_Act"]);
                EditPlansCheckActS.DataSource = indexDV;
                EditPlansCheckActS.DataBind();
                SqlDataAdapter adapterPlansPlanCloser = new SqlDataAdapter(selectCommPlanCloser, conn);
                adapterPlansPlanCloser.Fill(indexDS, "Plans_Plan_Closer");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Plan_Closer"]);
                EditPlanCloserS.DataSource = indexDV;
                EditPlanCloserS.DataBind();
                conn.Close();
                //DateTime date_from = DateTime.ParseExact(str_date_from, "yyyy-MM-ddTHH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                if (str_date_from != String.Empty && str_date_from != null && str_date_from != "null")
                {
                    DateTime date_from = DateTime.ParseExact(str_date_from, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditPlansDateFromDF.SelectedValue = date_from;
                }
                else
                {
                    EditPlansDateFromDF.SelectedValue = DateTime.Today;
                }
                if (str_date_to != String.Empty && str_date_to != null && str_date_to != "null")
                {
                    DateTime date_to = DateTime.ParseExact(str_date_to, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditPlansDateToDF.SelectedValue = date_to;
                }
                else
                {
                    EditPlansDateToDF.SelectedValue = DateTime.Today;
                }

                if (str_date_check_probes != String.Empty && str_date_check_probes != null && str_date_check_probes != "null")
                {
                    DateTime date_check_probes = DateTime.ParseExact(str_date_check_probes, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditDateCheckProbesDF.SelectedValue = date_check_probes;
                }
                else
                {
                    EditDateCheckProbesDF.Text = String.Empty;
                }
                if (str_date_check_maps != String.Empty && str_date_check_maps != null && str_date_check_maps != "null")
                {
                    DateTime date_check_maps = DateTime.ParseExact(str_date_check_maps, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditDateCheckMapsDF.SelectedValue = date_check_maps;
                }
                else
                {
                    EditDateCheckMapsDF.Text = String.Empty;
                }
                if (str_date_check_points != String.Empty && str_date_check_points != null && str_date_check_points != "null")
                {
                    DateTime date_check_points = DateTime.ParseExact(str_date_check_points, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditDateCheckPointsDF.SelectedValue = date_check_points;
                }
                else
                {
                    EditDateCheckPointsDF.Text = String.Empty;
                }
                if (str_date_check_act != String.Empty && str_date_check_act != null && str_date_check_act != "null")
                {
                    DateTime date_check_act = DateTime.ParseExact(str_date_check_act, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    EditDateCheckActDF.SelectedValue = date_check_act;
                }
                else
                {
                    EditDateCheckActDF.Text = String.Empty;
                }
            }
        }

        [DirectMethod]
        public void FillPlans()
        {
            if (connection_try)
            {
                conn.Open();
                String filter_plan = String.Empty;
                if (PlansWorkerCB.Value != null)
                {
                    if (PlansWorkerCB.Value.ToString() != "0" && PlansWorkerCB.Value.ToString() != String.Empty && PlansWorkerCB.Value.ToString() != null)
                    {
                        filter_plan += (" AND id_worker = " + PlansWorkerCB.Value.ToString());
                    }
                }
                if (PlansRegionCB.Value != null)
                {
                    if (PlansRegionCB.Value.ToString() != "0" && PlansRegionCB.Value.ToString() != String.Empty && PlansRegionCB.Value.ToString() != null)
                    {
                        filter_plan += (" AND id_region = " + PlansRegionCB.Value.ToString());
                    }
                }
                if (PlansMissionCB.Value != null)
                {
                    if (PlansMissionCB.Value.ToString() != "0" && PlansMissionCB.Value.ToString() != String.Empty && PlansMissionCB.Value.ToString() != null)
                    {
                        filter_plan += (" AND id_mission = " + PlansMissionCB.Value.ToString());
                    }
                }
                String plan_date_form = PlansFromDF.SelectedDate.Ticks.ToString();
                String plan_date_to = PlansToDF.SelectedDate.Ticks.ToString();
                SqlDataAdapter adapterPlans = new SqlDataAdapter(selectCommPlans + " WHERE date_from >= dbo.NetFxUtcTicksToDateTime(" + plan_date_form + ") AND date_to <= dbo.NetFxUtcTicksToDateTime(" + plan_date_to + ")" + filter_plan, conn);
                adapterPlans.Fill(indexDS, "Plans");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans"]);
                PlansS.DataSource = indexDV;
                PlansS.DataBind();
                conn.Close();
                
                List<Dictionary<String, String>> records_plan = JSON.Deserialize<List<Dictionary<String, String>>>(PlansGP.GetStore().JsonData);
                if (records_plan.Count > 0)
                {
                    PlansGP.GetSelectionModel().Select(records_plan.Count - 1);
                }
                else
                {
                    SurveyListS.RemoveAll();
                }
            }
        }

        [DirectMethod]
        public void ResetPlans()
        {
            PlansWorkerCB.Clear();
            PlansRegionCB.Clear();
            PlansMissionCB.Clear();
            //PlansFromDF.SelectedValue = DateTime.Today.AddMonths(-1);
            //PlansToDF.SelectedValue = DateTime.Today.AddMonths(1);
            PlansFromDF.SelectedValue = new DateTime(DateTime.Today.Year, 1, 1);
            PlansToDF.SelectedValue = new DateTime(DateTime.Today.Year, 12, 31);
            FillPlans();
        }

        [DirectMethod]
        public void PlansWindows()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterPlansWorker = new SqlDataAdapter(selectCommPlansWorker, conn);
                adapterPlansWorker.Fill(indexDS, "Plans_Workers");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Workers"]);
                PlansWorkerS.DataSource = indexDV;
                PlansWorkerS.DataBind();
                SqlDataAdapter adapterPlansRegion = new SqlDataAdapter(selectCommRegion, conn);
                adapterPlansRegion.Fill(indexDS, "Plans_Regions");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Regions"]);
                PlansRegionS.DataSource = indexDV;
                PlansRegionS.DataBind();
                SqlDataAdapter adapterPlansMissions = new SqlDataAdapter(selectCommPlansMissions, conn);
                adapterPlansMissions.Fill(indexDS, "Plans_Missions");
                indexDV = new System.Data.DataView(indexDS.Tables["Plans_Missions"]);
                PlansMissionS.DataSource = indexDV;
                PlansMissionS.DataBind();
                conn.Close();
                //ResetPlans();
                if (PlansFromDF.SelectedValue == null)
                {
                    PlansFromDF.SelectedValue = new DateTime(DateTime.Today.Year, 1, 1);
                }
                if (PlansToDF.SelectedValue == null)
                {
                    PlansToDF.SelectedValue = new DateTime(DateTime.Today.Year, 12, 31);
                }
                FillPlans();
                PlansW.Show();
                //PlansGP.GetSelectionModel().Select(0);
            }
        }

        [DirectMethod]
        public void FillSurveyList(String id_plan)
        {
            if (id_plan != null && id_plan != String.Empty && id_plan != "0")
            {
                if (connection_try)
                {
                    conn.Open();
                    SqlDataAdapter adapterPlansSurveyList = new SqlDataAdapter(selectCommSurveyList + " WHERE id_plan = " + id_plan, conn);
                    adapterPlansSurveyList.Fill(indexDS, "Plans_Survey_List");
                    indexDV = new System.Data.DataView(indexDS.Tables["Plans_Survey_List"]);
                    SurveyListS.DataSource = indexDV;
                    SurveyListS.DataBind();
                    conn.Close();
                    SurveyListGP.GetSelectionModel().Select(0);
                }
            }
        }

        [DirectMethod]
        public void SelectedPlans(String id_plan)
        {
            FillSurveyList(id_plan);
        }
        
        [DirectMethod]
        public void DeletePlan(String id_plan)
        {
            String handler = "App.direct.ConfirmDeletePlan(" + id_plan + ");";
            X.Msg.Confirm("Внимание!", "Вы действительно хотите удалить план-задание?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = handler,
                    Text = "Да"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "",
                    Text = "Нет"
                }
            }).Show();
        }

        [DirectMethod]
        public void ConfirmDeletePlan(String id_plan)
        {
            if (id_plan != String.Empty && id_plan != "null" && connection_try)
            {
                conn.Open();
                SqlCommand delete_plan = new SqlCommand("Delete_Plan", conn);
                delete_plan.CommandType = CommandType.StoredProcedure;
                delete_plan.Parameters.AddWithValue("@id_plan", Convert.ToInt32(NotNull(id_plan)));
                delete_plan.ExecuteNonQuery();
                conn.Close();
                FillPlans();
            }
        }

        [DirectMethod]
        public void ReportPlan(String id_plan)
        {
            if (id_plan != null && id_plan != "null" && id_plan != String.Empty && id_plan != "0")
            {
                if (Request.Browser.Cookies)
                {
                    HttpCookie report_on_the_plan = Request.Cookies["Agrochim31_Report_On_The_Plan"];
                    if (report_on_the_plan == null)
                    {
                        report_on_the_plan = new HttpCookie("Agrochim31_Report_On_The_Plan");
                        Response.Cookies.Add(report_on_the_plan);
                    }
                    report_on_the_plan["id_plan"] = id_plan;
                    Response.Cookies.Set(report_on_the_plan);
                    win_the_plan.Reload();
                    win_the_plan.Show();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Ошибка cookies",
                        Message = "Необходимо включить поддержку cookies!!!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
            }
        }

        [DirectMethod]
        public void AcceptAddEditPlan(String id_plan, String title_worker, String title_region, String title_mission, String date_from,
                                      String date_to, String count_days, String count_working_days, String title_chief, String title_matcher, String is_driver,
                                      String title_check_probes, String title_check_maps, String title_check_points, String title_check_act,
                                      String date_check_probes, String date_check_maps, String date_check_points, String date_check_act, String plan_result, String title_plan_closer)
        {
            if (connection_try)
            {
                if (title_worker != null && title_worker != String.Empty && title_worker != "0" &&
                        title_region != null && title_region != String.Empty && title_region != "0" &&
                        title_mission != null && title_mission != String.Empty && title_mission != "0" &&
                        date_from != null && date_from != String.Empty && date_from != "0" &&
                        date_to != null && date_to != String.Empty && date_to != "0" &&
                        count_days != null && count_days != String.Empty && count_days != "0" &&
                        count_working_days != null && count_working_days != String.Empty && count_working_days != "0" &&
                        title_chief != null && title_chief != String.Empty && title_chief != "0" &&
                        title_matcher != null && title_matcher != String.Empty && title_matcher != "0")
                {
                    if (String.Compare("true", is_driver) == 0) { is_driver = "1"; } else { is_driver = "0"; }
                    DateTime dt_date_from = DateTime.ParseExact(date_from.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dt_date_to = DateTime.ParseExact(date_to.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    TimeSpan span = dt_date_to - dt_date_from;
                    Int32 count_w_d = Convert.ToInt32(count_working_days);
                    //if (count_w_d != GetCountWorkingDays(dt_date_from, dt_date_to)) { count_w_d = GetCountWorkingDays(dt_date_from, dt_date_to); }
                    if (count_days != (span.Days + 1).ToString()) { count_days = (span.Days + 1).ToString(); }

                    DateTime dt_date_check_probes = DateTime.Today;
                    if (date_check_probes != null && date_check_probes != String.Empty && date_check_probes != "null")
                    {
                        dt_date_check_probes = DateTime.ParseExact(date_check_probes.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    DateTime dt_date_check_maps = DateTime.Today;
                    if (date_check_maps != null && date_check_maps != String.Empty && date_check_maps != "null")
                    {
                        dt_date_check_maps = DateTime.ParseExact(date_check_maps.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    DateTime dt_date_check_points = DateTime.Today;
                    if (date_check_points != null && date_check_points != String.Empty && date_check_points != "null")
                    {
                        dt_date_check_points = DateTime.ParseExact(date_check_points.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    DateTime dt_date_check_act = DateTime.Today;
                    if (date_check_act != null && date_check_act != String.Empty && date_check_act != "null")
                    {
                        dt_date_check_act = DateTime.ParseExact(date_check_act.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    if (id_plan == null || id_plan == String.Empty || id_plan == "0")
                    {
                        conn.Open();
                        SqlCommand add_plan = new SqlCommand("Add_Plan", conn);
                        add_plan.CommandType = CommandType.StoredProcedure;
                        add_plan.CommandTimeout = 300;
                        add_plan.Parameters.AddWithValue("@title_worker", title_worker);
                        add_plan.Parameters.AddWithValue("@title_region", title_region);
                        add_plan.Parameters.AddWithValue("@title_mission", title_mission);
                        add_plan.Parameters.AddWithValue("@date_from_long", dt_date_from.Ticks);
                        add_plan.Parameters.AddWithValue("@date_to_long", dt_date_to.Ticks);
                        add_plan.Parameters.AddWithValue("@count_days", Convert.ToInt32(count_days));
                        add_plan.Parameters.AddWithValue("@count_working_days", count_w_d);
                        add_plan.Parameters.AddWithValue("@title_chief", title_chief);
                        add_plan.Parameters.AddWithValue("@title_matcher", title_matcher);
                        add_plan.Parameters.AddWithValue("@is_driver", Convert.ToBoolean(Convert.ToInt32(is_driver)));
                        add_plan.Parameters.AddWithValue("@title_check_probes", title_check_probes);
                        add_plan.Parameters.AddWithValue("@title_check_maps", title_check_maps);
                        add_plan.Parameters.AddWithValue("@title_check_points", title_check_points);
                        add_plan.Parameters.AddWithValue("@title_check_act", title_check_act);
                        if (date_check_probes != null && date_check_probes != String.Empty && date_check_probes != "null")
                        {
                            add_plan.Parameters.AddWithValue("@date_check_probes_long", dt_date_check_probes.Ticks);
                        }
                        else
                        {
                            add_plan.Parameters.AddWithValue("@date_check_probes_long", 0);
                        }
                        if (date_check_maps != null && date_check_maps != String.Empty && date_check_maps != "null")
                        {
                            add_plan.Parameters.AddWithValue("@date_check_maps_long", dt_date_check_maps.Ticks);
                        }
                        else
                        {
                            add_plan.Parameters.AddWithValue("@date_check_maps_long", 0);
                        }
                        if (date_check_points != null && date_check_points != String.Empty && date_check_points != "null")
                        {
                            add_plan.Parameters.AddWithValue("@date_check_points_long", dt_date_check_points.Ticks);
                        }
                        else
                        {
                            add_plan.Parameters.AddWithValue("@date_check_points_long", 0);
                        }
                        if (date_check_act != null && date_check_act != String.Empty && date_check_act != "null")
                        {
                            add_plan.Parameters.AddWithValue("@date_check_act_long", dt_date_check_act.Ticks);
                        }
                        else
                        {
                            add_plan.Parameters.AddWithValue("@date_check_act_long", 0);
                        }
                        add_plan.Parameters.AddWithValue("@plan_result", plan_result);
                        add_plan.Parameters.AddWithValue("@title_plan_closer", title_plan_closer);
                        add_plan.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                        add_plan.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        if (Convert.ToInt32(id_plan) < 0)
                        {
                            id_plan = Math.Abs(Convert.ToInt32(id_plan)).ToString();
                            conn.Open();
                            SqlCommand copy_plan = new SqlCommand("Copy_Plan_With_Survey_list", conn);
                            copy_plan.CommandType = CommandType.StoredProcedure;
                            copy_plan.CommandTimeout = 300;
                            copy_plan.Parameters.AddWithValue("@id_plan", id_plan);
                            copy_plan.Parameters.AddWithValue("@title_worker", title_worker);
                            copy_plan.Parameters.AddWithValue("@title_region", title_region);
                            copy_plan.Parameters.AddWithValue("@title_mission", title_mission);
                            copy_plan.Parameters.AddWithValue("@date_from_long", dt_date_from.Ticks);
                            copy_plan.Parameters.AddWithValue("@date_to_long", dt_date_to.Ticks);
                            copy_plan.Parameters.AddWithValue("@count_days", Convert.ToInt32(count_days));
                            copy_plan.Parameters.AddWithValue("@count_working_days", count_w_d);
                            copy_plan.Parameters.AddWithValue("@title_chief", title_chief);
                            copy_plan.Parameters.AddWithValue("@title_matcher", title_matcher);
                            copy_plan.Parameters.AddWithValue("@is_driver", Convert.ToBoolean(Convert.ToInt32(is_driver)));
                            copy_plan.Parameters.AddWithValue("@title_check_probes", title_check_probes);
                            copy_plan.Parameters.AddWithValue("@title_check_maps", title_check_maps);
                            copy_plan.Parameters.AddWithValue("@title_check_points", title_check_points);
                            copy_plan.Parameters.AddWithValue("@title_check_act", title_check_act);
                            if (date_check_probes != null && date_check_probes != String.Empty && date_check_probes != "null")
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_probes_long", dt_date_check_probes.Ticks);
                            }
                            else
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_probes_long", 0);
                            }
                            if (date_check_maps != null && date_check_maps != String.Empty && date_check_maps != "null")
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_maps_long", dt_date_check_maps.Ticks);
                            }
                            else
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_maps_long", 0);
                            }
                            if (date_check_points != null && date_check_points != String.Empty && date_check_points != "null")
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_points_long", dt_date_check_points.Ticks);
                            }
                            else
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_points_long", 0);
                            }
                            if (date_check_act != null && date_check_act != String.Empty && date_check_act != "null")
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_act_long", dt_date_check_act.Ticks);
                            }
                            else
                            {
                                copy_plan.Parameters.AddWithValue("@date_check_act_long", 0);
                            }
                            copy_plan.Parameters.AddWithValue("@plan_result", plan_result);
                            copy_plan.Parameters.AddWithValue("@title_plan_closer", title_plan_closer);
                            copy_plan.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                            copy_plan.ExecuteNonQuery();
                            conn.Close();
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand edit_plan = new SqlCommand("Edit_Plan", conn);
                            edit_plan.CommandType = CommandType.StoredProcedure;
                            edit_plan.CommandTimeout = 300;
                            edit_plan.Parameters.AddWithValue("@id_plan", Convert.ToInt32(id_plan));
                            edit_plan.Parameters.AddWithValue("@title_worker", title_worker);
                            edit_plan.Parameters.AddWithValue("@title_region", title_region);
                            edit_plan.Parameters.AddWithValue("@title_mission", title_mission);
                            edit_plan.Parameters.AddWithValue("@date_from_long", dt_date_from.Ticks);
                            edit_plan.Parameters.AddWithValue("@date_to_long", dt_date_to.Ticks);
                            edit_plan.Parameters.AddWithValue("@count_days", Convert.ToInt32(count_days));
                            edit_plan.Parameters.AddWithValue("@count_working_days", count_w_d);
                            edit_plan.Parameters.AddWithValue("@title_chief", title_chief);
                            edit_plan.Parameters.AddWithValue("@title_matcher", title_matcher);
                            edit_plan.Parameters.AddWithValue("@is_driver", Convert.ToBoolean(Convert.ToInt32(is_driver)));
                            edit_plan.Parameters.AddWithValue("@title_check_probes", title_check_probes);
                            edit_plan.Parameters.AddWithValue("@title_check_maps", title_check_maps);
                            edit_plan.Parameters.AddWithValue("@title_check_points", title_check_points);
                            edit_plan.Parameters.AddWithValue("@title_check_act", title_check_act);
                            if (date_check_probes != null && date_check_probes != String.Empty && date_check_probes != "null")
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_probes_long", dt_date_check_probes.Ticks);
                            }
                            else
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_probes_long", 0);
                            }
                            if (date_check_maps != null && date_check_maps != String.Empty && date_check_maps != "null")
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_maps_long", dt_date_check_maps.Ticks);
                            }
                            else
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_maps_long", 0);
                            }
                            if (date_check_points != null && date_check_points != String.Empty && date_check_points != "null")
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_points_long", dt_date_check_points.Ticks);
                            }
                            else
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_points_long", 0);
                            }
                            if (date_check_act != null && date_check_act != String.Empty && date_check_act != "null")
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_act_long", dt_date_check_act.Ticks);
                            }
                            else
                            {
                                edit_plan.Parameters.AddWithValue("@date_check_act_long", 0);
                            }
                            edit_plan.Parameters.AddWithValue("@plan_result", plan_result);
                            edit_plan.Parameters.AddWithValue("@title_plan_closer", title_plan_closer);
                            edit_plan.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                            edit_plan.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                FillPlans();
            }
        }

        [DirectMethod]
        public void ChangePlansFilter()
        {
            FillPlans();
        }

        [DirectMethod]
        public void ChangeDatePlan(String date_from, String date_to)
        {
            DateTime dt_date_from = DateTime.ParseExact(date_from.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt_date_to = DateTime.ParseExact(date_to.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            if (dt_date_from > dt_date_to) { dt_date_from = dt_date_to; }
            EditPlansDateFromDF.SelectedDate = dt_date_from;
            EditPlansDateToDF.SelectedDate = dt_date_to;
            TimeSpan span = dt_date_to - dt_date_from;
            EditPlansCountDaysNF.Value = (span.Days + 1);
            EditPlansCountWorkingDaysNF.Value = GetCountWorkingDays(dt_date_from, dt_date_to);
        }

        [DirectMethod]
        public void ChangeDayPlan(String date_from, String date_to, String count_days)
        {
            DateTime dt_date_from = DateTime.ParseExact(date_from.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt_date_to = DateTime.ParseExact(date_to.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            dt_date_to = dt_date_from.AddDays(Convert.ToInt32(count_days) - 1);
            EditPlansDateToDF.SelectedDate = dt_date_to;
            EditPlansCountWorkingDaysNF.Value = GetCountWorkingDays(dt_date_from, dt_date_to);
        }

        [DirectMethod]
        public void BindSurveyListData(String id_region)
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterSurveyListOrg = new SqlDataAdapter(selectCommOrganization + " WHERE id_region = " + id_region + "ORDER BY title_organization", conn);
                adapterSurveyListOrg.Fill(indexDS, "SurveyList_Organization");
                indexDV = new System.Data.DataView(indexDS.Tables["SurveyList_Organization"]);
                CombSurveyListOrgS.DataSource = indexDV;
                CombSurveyListOrgS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void AcceptAddEditSurveyList(String id_survey_list, String id_plan, String title_organization, String planned_area, String planned_probes, 
                                            String planned_cuts, String planned_floor_pits, String planned_excavation, String actual_area, String actual_probes,
                                            String actual_cuts, String actual_floor_pits, String actual_excavation)
        {
            if (connection_try)
            {
                if (title_organization != null && title_organization != String.Empty && title_organization != "0" &&
                    id_plan != null && id_plan != String.Empty && id_plan != "0")
                {
                    planned_area = ReplaceValue(planned_area, "2");
                    actual_area = ReplaceValue(actual_area, "2");
                    if (id_survey_list == null || id_survey_list == String.Empty || id_survey_list == "0")
                    {
                        conn.Open();
                        SqlCommand add_survey_list = new SqlCommand("Add_Survey_list", conn);
                        add_survey_list.CommandType = CommandType.StoredProcedure;
                        add_survey_list.CommandTimeout = 300;
                        add_survey_list.Parameters.AddWithValue("@id_plan", Convert.ToInt32(id_plan));
                        add_survey_list.Parameters.AddWithValue("@title_organization", title_organization);
                        add_survey_list.Parameters.AddWithValue("@planned_area", Convert.ToDouble(NotNull(planned_area)));
                        add_survey_list.Parameters.AddWithValue("@planned_probes", Convert.ToInt32(NotNull(planned_probes)));
                        add_survey_list.Parameters.AddWithValue("@planned_cuts", Convert.ToInt32(NotNull(planned_cuts)));
                        add_survey_list.Parameters.AddWithValue("@planned_floor_pits", Convert.ToInt32(NotNull(planned_floor_pits)));
                        add_survey_list.Parameters.AddWithValue("@planned_excavation", Convert.ToInt32(NotNull(planned_excavation)));
                        add_survey_list.Parameters.AddWithValue("@actual_area", Convert.ToDouble(NotNull(actual_area)));
                        add_survey_list.Parameters.AddWithValue("@actual_probes", Convert.ToInt32(NotNull(actual_probes)));
                        add_survey_list.Parameters.AddWithValue("@actual_cuts", Convert.ToInt32(NotNull(actual_cuts)));
                        add_survey_list.Parameters.AddWithValue("@actual_floor_pits", Convert.ToInt32(NotNull(actual_floor_pits)));
                        add_survey_list.Parameters.AddWithValue("@actual_excavation", Convert.ToInt32(NotNull(actual_excavation)));
                        add_survey_list.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        conn.Open();
                        SqlCommand edit_survey_list = new SqlCommand("Edit_Survey_list", conn);
                        edit_survey_list.CommandType = CommandType.StoredProcedure;
                        edit_survey_list.CommandTimeout = 300;
                        edit_survey_list.Parameters.AddWithValue("@id_survey_list", Convert.ToInt32(id_survey_list));
                        edit_survey_list.Parameters.AddWithValue("@id_plan", Convert.ToInt32(id_plan));
                        edit_survey_list.Parameters.AddWithValue("@title_organization", title_organization);
                        edit_survey_list.Parameters.AddWithValue("@planned_area", Convert.ToDouble(NotNull(planned_area)));
                        edit_survey_list.Parameters.AddWithValue("@planned_probes", Convert.ToInt32(NotNull(planned_probes)));
                        edit_survey_list.Parameters.AddWithValue("@planned_cuts", Convert.ToInt32(NotNull(planned_cuts)));
                        edit_survey_list.Parameters.AddWithValue("@planned_floor_pits", Convert.ToInt32(NotNull(planned_floor_pits)));
                        edit_survey_list.Parameters.AddWithValue("@planned_excavation", Convert.ToInt32(NotNull(planned_excavation)));
                        edit_survey_list.Parameters.AddWithValue("@actual_area", Convert.ToDouble(NotNull(actual_area)));
                        edit_survey_list.Parameters.AddWithValue("@actual_probes", Convert.ToInt32(NotNull(actual_probes)));
                        edit_survey_list.Parameters.AddWithValue("@actual_cuts", Convert.ToInt32(NotNull(actual_cuts)));
                        edit_survey_list.Parameters.AddWithValue("@actual_floor_pits", Convert.ToInt32(NotNull(actual_floor_pits)));
                        edit_survey_list.Parameters.AddWithValue("@actual_excavation", Convert.ToInt32(NotNull(actual_excavation)));
                        edit_survey_list.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            FillSurveyList(id_plan);
        }

        [DirectMethod]
        public void DeleteSurveyList(String id_survey_list)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_survey_list = new SqlCommand("Delete_Survey_list", conn);
                delete_survey_list.CommandType = CommandType.StoredProcedure;
                delete_survey_list.CommandTimeout = 300;
                delete_survey_list.Parameters.AddWithValue("@id_survey_list", Convert.ToInt32(NotNull(id_survey_list)));
                delete_survey_list.ExecuteNonQuery();
                conn.Close();
            }
        }

        [DirectMethod]
        public void SurveyListMaxCount()
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = "Внимание!!!",
                Message = "Список обследования в данном плане-задание достиг своего максимума!<br />Создайте новый план-задание!",
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.INFO
            });
        }

        [DirectMethod]
        public void DeleteOrganizationMessage(String id_org, String departments)
        {
            List<Dictionary<String, String>> records = JSON.Deserialize<List<Dictionary<String, String>>>(departments);
            if (records.Count <= 0)
            {
                String handler = "App.direct.DeleteOrganization(" + id_org + ", 'Удалить');";
                X.Msg.Confirm("Внимание!", "Вы действительно хотите удалить данную организацию?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = handler,
                        Text = "Да"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "",
                        Text = "Нет"
                    }
                }).Show();
            }
            else
            {
                String handler = "App.direct.ConfirmDeleteOrganization(" + id_org + ");";
                X.Msg.Confirm("Внимание!", "В данной организации существуют отделения с данными! Продолжить?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = handler,
                        Text = "Да"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "",
                        Text = "Нет"
                    }
                }).Show();
            }
        }

        [DirectMethod]
        public void ConfirmDeleteOrganization(String id_org)
        {
            IdDeleteOrganizationTF.Text = id_org;
            ConfirmTF.Text = String.Empty;
            ConfirmDeleteOrganizationW.Show();
        }

        [DirectMethod]
        public void CancelDeleteOrganization()
        {
            ConfirmDeleteOrganizationW.Close();
        }

        [DirectMethod]
        public void DeleteOrganization(String id_org, String delete_str)
        {
            if (String.Compare(delete_str, "Удалить") == 0 && id_org != String.Empty && connection_try)
            {
                conn.Open();
                SqlCommand get_count_sl = new SqlCommand("Select_SL_By_Organization", conn);
                get_count_sl.CommandType = CommandType.StoredProcedure;
                get_count_sl.Parameters.AddWithValue("@id_organization", Convert.ToInt32(id_org));
                get_count_sl.Parameters.Add("@result", SqlDbType.Int);
                get_count_sl.Parameters["@result"].Direction = ParameterDirection.Output;
                get_count_sl.ExecuteNonQuery();
                if (Convert.ToInt32(get_count_sl.Parameters["@result"].Value) > 0)
                {
                    String handler = "App.direct.DeleteOrganizationWithSurveyList(" + id_org + ");";
                    X.Msg.Confirm("Внимание!", "Хоз-во будет удалено из плана-задания! Продолжить?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = handler,
                            Text = "Да"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "",
                            Text = "Нет"
                        }
                    }).Show();
                }
                else
                {
                    SqlCommand delete_org = new SqlCommand("Delete_Organization", conn);
                    delete_org.CommandType = CommandType.StoredProcedure;
                    delete_org.Parameters.AddWithValue("@id_organization", Convert.ToInt32(id_org));
                    delete_org.ExecuteNonQuery();
                    conn.Close();
                    if (indexDS.Tables["Organization"] != null) { indexDS.Tables["Organization"].Clear(); }
                    FillOrganization(current_id_region);
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
                    ConfirmDeleteOrganizationW.Close();
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Предупреждение!!!",
                    Message = "Введено неправильное слово!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void DeleteOrganizationWithSurveyList(String id_org)
        {
            conn.Open();
            SqlCommand delete_org = new SqlCommand("Delete_Organization", conn);
            delete_org.CommandType = CommandType.StoredProcedure;
            delete_org.Parameters.AddWithValue("@id_organization", Convert.ToInt32(id_org));
            delete_org.ExecuteNonQuery();
            conn.Close();
            if (indexDS.Tables["Organization"] != null) { indexDS.Tables["Organization"].Clear(); }
            FillOrganization(current_id_region);
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
            ConfirmDeleteOrganizationW.Close();
        }

        //формы для введения результатов анализов
        //степень кислотности
        [DirectMethod]
        public void ShowAnalysPhSW()
        {
            if (connection_try)
            {
                //AnalysPhSTourCB.Clear();
                //AnalysPhSYearCB.Clear();
                conn.Open();
                SqlDataAdapter adapterAnalysPhSTourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSTourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysPhSTourS.DataSource = indexDV;
                AnalysPhSTourS.DataBind();
                SqlDataAdapter adapterAnalysPhSYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysPhSYearS.DataSource = indexDV;
                AnalysPhSYearS.DataBind();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                AnalysPhSRegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                AnalysPhSOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                AnalysPhSDepartmentTF.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                adapterAnalysPhS = new SqlDataAdapter(selectCommAnalysPhS + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhS.Fill(indexDS, "View_Analys_PhS");
                indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_PhS"]);
                AnalysPhSS.DataSource = indexDV;
                AnalysPhSS.DataBind();
                conn.Close(); 
                AnalysPhSNumberSampleTF.Text = String.Empty;
                AnalysPhSValuePhSTF.Text = String.Empty;
                AnalysPhSControlTF.Text = String.Empty;
                AnalysPhSFewTF.Text = String.Empty;
                AnalysPhSIdSampleTF.Text = 0.ToString();
                AnalysPhSTourCB.Text = String.Empty;
                AnalysPhSYearCB.Text = String.Empty;
                AnalysPhSW.Show();
                //AnalysPhSGP.GetSelectionModel().Select(0);
                //AnalysPhSTourCB.Select(0);
                //AnalysPhSYearCB.Select(0);
            }
        }

        [DirectMethod]
        public void FillAnalysPhSTour(String tour)
        {
            if (tour != null && tour != "" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysPhS = new SqlDataAdapter(selectCommAnalysPhS + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysPhS.Fill(indexDS, "View_Analys_PhS");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_PhS"]);
                    AnalysPhSS.DataSource = indexDV;
                    AnalysPhSS.DataBind();
                    SqlDataAdapter adapterAnalysPhSYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysPhSYearCB.Fill(indexDS, "Analys_sample");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample"]);
                    conn.Close();
                    AnalysPhSYearS.DataSource = indexDV;
                    AnalysPhSYearS.DataBind();
                    AnalysPhSYearCB.Text = String.Empty;
                }
            }
        }

        [DirectMethod]
        public void FillAnalysPhSYear(String year, String tour)
        {
            if (year != null && year != "null" && year != String.Empty && tour != null && tour != "null" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysPhS = new SqlDataAdapter(selectCommAnalysPhS + " WHERE id_department=" + current_id_department + " AND tour=" + tour + " AND year=" + year, conn);
                    adapterAnalysPhS.Fill(indexDS, "View_Analys_PhS");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_PhS"]);
                    AnalysPhSS.DataSource = indexDV;
                    AnalysPhSS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void AddEditAnalysPhS(String tour, String year, String id_sample, String number_sample, String ph_s_value, String control_value)
        {
            if (connection_try && tour != null && tour != "null" && tour != String.Empty && year != null && year != "null" && year != String.Empty)
            {
                if (number_sample == null || number_sample == "null" || number_sample == String.Empty)
                {
                    number_sample = "1";
                }
                String temp = AdaptationValue(ph_s_value, "ph_s");
                if (ph_s_value != temp) { ph_s_value = temp; }
                temp = AdaptationValue(control_value, "ph_s");
                if (control_value != temp) { control_value = temp; }
                conn.Open();
                SqlCommand add_analys_ph_s = new SqlCommand("Add_Edit_Analys_Ph", conn);
                add_analys_ph_s.CommandType = CommandType.StoredProcedure;
                add_analys_ph_s.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                add_analys_ph_s.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                add_analys_ph_s.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour)));
                add_analys_ph_s.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                add_analys_ph_s.Parameters.AddWithValue("@number_sample", NotNull(number_sample));
                add_analys_ph_s.Parameters.AddWithValue("@ph_s_value", Convert.ToDouble(NotNull(ph_s_value)));
                add_analys_ph_s.Parameters.AddWithValue("@control_value", Convert.ToDouble(NotNull(control_value)));
                add_analys_ph_s.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                add_analys_ph_s.Parameters.Add("@result", SqlDbType.VarChar, 20);
                add_analys_ph_s.Parameters["@result"].Direction = ParameterDirection.Output;
                add_analys_ph_s.ExecuteNonQuery();
                AnalysPhSNumberSampleTF.Text = add_analys_ph_s.Parameters["@result"].Value.ToString();
                //AnalysPhSNumberSampleTF.Text = (Convert.ToInt32(number_sample) + 1).ToString();
                //обновляем существующие циклы и года
                SqlDataAdapter adapterAnalysPhSTourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSTourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysPhSTourS.DataSource = indexDV;
                AnalysPhSTourS.DataBind();
                SqlDataAdapter adapterAnalysPhSYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysPhSYearS.DataSource = indexDV;
                AnalysPhSYearS.DataBind(); 
                conn.Close();
                AnalysPhSIdSampleTF.Text = 0.ToString();
                AnalysPhSValuePhSTF.Text = String.Empty;
                AnalysPhSControlTF.Text = String.Empty;
                AnalysPhSFewTF.Text = String.Empty;
                FillAnalysPhSYear(year, tour);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Результат добавления ph",
                    Message = "Отсутствует цикл или год, либо нет подключения к БД!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void AddFewAnalysPhS(String tour_analys_ph, String year_analys_ph, String ph_s_values)
        {
            String str_values = ph_s_values.Replace('\t', ' ');
            if (str_values.Split(' ').Length > 0)
            {
                String[] values = new String[str_values.Split(' ').Length];
                values = str_values.Split(' ');
                SqlCommand add_analys_ph_s = new SqlCommand("Add_Edit_Analys_Ph", conn);
                for (int i = 0; i < values.Length; i++ )
                {
                    String temp = AdaptationValue(values[i], "ph_s");
                    if (values[i] != temp) { values[i] = temp; }
                    conn.Open();
                    add_analys_ph_s = new SqlCommand("Add_Edit_Analys_Ph", conn);
                    add_analys_ph_s.CommandType = CommandType.StoredProcedure;
                    add_analys_ph_s.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                    add_analys_ph_s.Parameters.AddWithValue("@id_sample", 0);
                    add_analys_ph_s.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour_analys_ph)));
                    add_analys_ph_s.Parameters.AddWithValue("@year", Convert.ToInt32(year_analys_ph));
                    add_analys_ph_s.Parameters.AddWithValue("@number_sample", 0);
                    add_analys_ph_s.Parameters.AddWithValue("@ph_s_value", Convert.ToDouble(NotNull(values[i])));
                    add_analys_ph_s.Parameters.AddWithValue("@control_value", Convert.ToDouble(0));
                    add_analys_ph_s.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                    add_analys_ph_s.Parameters.Add("@result", SqlDbType.VarChar, 20);
                    add_analys_ph_s.Parameters["@result"].Direction = ParameterDirection.Output;
                    add_analys_ph_s.ExecuteNonQuery();
                    conn.Close();
                    if (i == (values.Length - 1))
                    {
                        AnalysPhSNumberSampleTF.Text = add_analys_ph_s.Parameters["@result"].Value.ToString();
                    }
                }
                //обновляем существующие циклы и года
                conn.Open();
                SqlDataAdapter adapterAnalysPhSTourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSTourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysPhSTourS.DataSource = indexDV;
                AnalysPhSTourS.DataBind();
                SqlDataAdapter adapterAnalysPhSYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPhSYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysPhSYearS.DataSource = indexDV;
                AnalysPhSYearS.DataBind();
                conn.Close();
                AnalysPhSIdSampleTF.Text = 0.ToString();
                AnalysPhSValuePhSTF.Text = String.Empty;
                AnalysPhSControlTF.Text = String.Empty;
                AnalysPhSFewTF.Text = String.Empty;
                FillAnalysPhSYear(year_analys_ph, tour_analys_ph);
            }
        }

        [DirectMethod]
        public void SelectAnalysPhS(String id_sample, String number_sample, String ph_s_value, String control_value)
        {
            if (id_sample == null || id_sample == "" || id_sample == "null")
            {
                AnalysPhSIdSampleTF.Text = 0.ToString();
            }
            else { AnalysPhSIdSampleTF.Text = id_sample; }
            if (number_sample == null || number_sample == "" || number_sample == "null")
            {
                AnalysPhSNumberSampleTF.Text = String.Empty;
            }
            else { AnalysPhSNumberSampleTF.Text = number_sample; }
            if (ph_s_value == null || ph_s_value == "" || ph_s_value == "null")
            {
                AnalysPhSValuePhSTF.Text = String.Empty;
            }
            else { AnalysPhSValuePhSTF.Text = ph_s_value; }
            if (control_value == null || control_value == "" || control_value == "null")
            {
                AnalysPhSControlTF.Text = String.Empty;
            }
            else { AnalysPhSControlTF.Text = control_value; }
        }

        [DirectMethod]
        public void DeleteAnalysPhS(String id_sample, String id_ph)
        {
            if (connection_try)
            {
                if (id_ph == null || id_ph == "" || id_ph == String.Empty || id_ph == "0" || id_ph == "null")
                { 
                    X.Msg.Confirm("Внимание!", "Вы действительно хотите удалить данный образец полностью?", new MessageBoxButtonsConfig
                     {
                         Yes = new MessageBoxButtonConfig
                         {
                             Handler = "App.direct.DeleteAnalysSample_PhS(" + id_sample + ");",
                             Text = "Да"
                         },
                         No = new MessageBoxButtonConfig
                         {
                             //Handler = "App.direct.FillAnalysPhSYear(" + AnalysPhSYearCB.Text + ", " + AnalysPhSTourCB.Text + ");",
                             Handler = "",
                             Text = "Нет"
                         }
                     }).Show();  
                }
                else
                {
                    DeleteAnalysSample_PhS(id_sample);
                }
            }            
        }

        [DirectMethod]
        public void DeleteAnalysSample_PhS(String id_sample)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_analys_ph_s = new SqlCommand("Delete_Analys_Ph_S", conn);
                delete_analys_ph_s.CommandType = CommandType.StoredProcedure;
                delete_analys_ph_s.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                delete_analys_ph_s.ExecuteNonQuery();
                conn.Close();
                if (AnalysPhSYearCB.Text != String.Empty && AnalysPhSTourCB.Text != String.Empty)
                {
                    FillAnalysPhSYear(AnalysPhSYearCB.Text, AnalysPhSTourCB.Text);
                }
                else if (AnalysPhSTourCB.Text != String.Empty)
                {
                    FillAnalysPhSTour(AnalysPhSTourCB.Text);
                }
                else
                {
                    conn.Open();
                    adapterAnalysPhS = new SqlDataAdapter(selectCommAnalysPhS + " WHERE id_department=" + current_id_department, conn);
                    adapterAnalysPhS.Fill(indexDS, "View_Analys_PhS");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_PhS"]);
                    AnalysPhSS.DataSource = indexDV;
                    AnalysPhSS.DataBind();
                    conn.Close();
                }
            }
        }

        //фосфор, калий
        [DirectMethod]
        public void ShowAnalysPKW()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterAnalysPKTourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPKTourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysPKTourS.DataSource = indexDV;
                AnalysPKTourS.DataBind();
                SqlDataAdapter adapterAnalysPKYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPKYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysPKYearS.DataSource = indexDV;
                AnalysPKYearS.DataBind();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                AnalysPKRegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                AnalysPKOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                AnalysPKDepartmentTF.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                adapterAnalysPK = new SqlDataAdapter(selectCommAnalysPK + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysPK.Fill(indexDS, "View_Analys_P_K");
                indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_P_K"]);
                AnalysPKS.DataSource = indexDV;
                AnalysPKS.DataBind();
                conn.Close();
                //AnalysPKTourCB.Select(0);
                //AnalysPKYearCB.Select(0);
                AnalysPKNumberSampleTF.Text = String.Empty;
                AnalysPKValueP2O5TF.Text = String.Empty;
                AnalysPKValueK2OTF.Text = String.Empty;
                AnalysPKControlP2O5TF.Text = String.Empty;
                AnalysPKControlK2OTF.Text = String.Empty;
                //AnalysPKFewTF.Text = String.Empty;
                AnalysPKIdSampleTF.Text = 0.ToString();
                AnalysPKW.Show();
            }
        }

        [DirectMethod]
        public void FillAnalysPKTour(String tour)
        {
            if (tour != null && tour != "" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysPK = new SqlDataAdapter(selectCommAnalysPK + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysPK.Fill(indexDS, "View_Analys_P_K");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_P_K"]);
                    AnalysPKS.DataSource = indexDV;
                    AnalysPKS.DataBind();
                    SqlDataAdapter adapterAnalysPKYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysPKYearCB.Fill(indexDS, "Analys_sample");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample"]);
                    conn.Close();
                    AnalysPKYearS.DataSource = indexDV;
                    AnalysPKYearS.DataBind();
                    AnalysPKYearCB.Text = String.Empty;
                }
            }
        }

        [DirectMethod]
        public void FillAnalysPKYear(String year, String tour)
        {
            if (year != null && year != "" && year != String.Empty && tour != null && tour != "" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysPK = new SqlDataAdapter(selectCommAnalysPK + " WHERE id_department=" + current_id_department + " AND tour=" + tour + " AND year=" + year, conn);
                    adapterAnalysPK.Fill(indexDS, "View_Analys_P_K");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_P_K"]);
                    AnalysPKS.DataSource = indexDV;
                    AnalysPKS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void AddEditAnalysPK(String tour, String year, String id_sample, String number_sample, String p2o5_value, String k2o_value, String p2o5_control_value, String k2o_control_value)
        {
            if (connection_try && tour != null && tour != "" && tour != String.Empty && year != null && year != "" && year != String.Empty)
            {
                if (id_sample == null && id_sample == "" && id_sample == String.Empty && id_sample == "0")
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Результат изменения образца",
                        Message = "Выберите редактируемый образец!\n" + number_sample,
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.INFO
                    });
                }
                else
                {
                    if (number_sample != null && number_sample != "" && number_sample != String.Empty && number_sample != "0")
                    {
                        String temp1 = AdaptationValue(p2o5_value, "p2o5");
                        if (p2o5_value != temp1) { p2o5_value = temp1; }
                        temp1 = AdaptationValue(p2o5_control_value, "p2o5");
                        if (p2o5_control_value != temp1) { p2o5_control_value = temp1; }
                        String temp2 = AdaptationValue(k2o_value, "k2o");
                        if (k2o_value != temp2) { k2o_value = temp2; }
                        temp2 = AdaptationValue(k2o_control_value, "k2o");
                        if (k2o_control_value != temp2) { k2o_control_value = temp2; }
                        conn.Open();
                        SqlCommand add_analys_p_k = new SqlCommand("Add_Edit_Analys_P_K", conn);
                        add_analys_p_k.CommandType = CommandType.StoredProcedure;
                        add_analys_p_k.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                        add_analys_p_k.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                        add_analys_p_k.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour)));
                        add_analys_p_k.Parameters.AddWithValue("@year", Convert.ToInt32(NotNull(year)));
                        add_analys_p_k.Parameters.AddWithValue("@number_sample", NotNull(number_sample));
                        add_analys_p_k.Parameters.AddWithValue("@p2o5_value", Convert.ToDouble(NotNull(p2o5_value)));
                        add_analys_p_k.Parameters.AddWithValue("@k2o_value", Convert.ToDouble(NotNull(k2o_value)));
                        add_analys_p_k.Parameters.AddWithValue("@p2o5_control_value", Convert.ToDouble(NotNull(p2o5_control_value)));
                        add_analys_p_k.Parameters.AddWithValue("@k2o_control_value", Convert.ToDouble(NotNull(k2o_control_value)));
                        add_analys_p_k.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                        //add_analys_p_k.Parameters.Add("@result", SqlDbType.VarChar, 20);
                        //add_analys_p_k.Parameters["@result"].Direction = ParameterDirection.Output;
                        add_analys_p_k.ExecuteNonQuery();
                        SqlDataAdapter adapterAnalysPKTourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                        adapterAnalysPKTourCB.Fill(indexDS, "Analys_sample_tours");
                        indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                        AnalysPKTourS.DataSource = indexDV;
                        AnalysPKTourS.DataBind();
                        SqlDataAdapter adapterAnalysPKYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                        adapterAnalysPKYearCB.Fill(indexDS, "Analys_sample_years");
                        indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                        AnalysPKYearS.DataSource = indexDV;
                        AnalysPKYearS.DataBind();
                        //AnalysPKNumberSampleTF.Text = add_analys_p_k.Parameters["@result"].Value.ToString();                  
                        conn.Close();
                        AnalysPKIdSampleTF.Text = 0.ToString();
                        AnalysPKNumberSampleTF.Text = String.Empty;
                        AnalysPKValueP2O5TF.Text = String.Empty;
                        AnalysPKValueK2OTF.Text = String.Empty;
                        AnalysPKControlP2O5TF.Text = String.Empty;
                        AnalysPKControlK2OTF.Text = String.Empty;
                        //AnalysPKFewTF.Text = String.Empty;
                        FillAnalysPKYear(year, tour);
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Результат изменения образца",
                            Message = "Данные введены некорректно!\n" + number_sample,
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });
                    }
                }
            }
        }

        [DirectMethod]
        public void SelectAnalysPK(String id_sample, String number_sample, String p2o5_value, String p2o5_control_value, String k2o_value, String k2o_control_value)
        {
            if (id_sample == null || id_sample == "" || id_sample == "null")
            {
                AnalysPKIdSampleTF.Text = 0.ToString();
            }
            else { AnalysPKIdSampleTF.Text = id_sample; }
            if (number_sample == null || number_sample == "" || number_sample == "null")
            {
                AnalysPKNumberSampleTF.Text = String.Empty;
            }
            else { AnalysPKNumberSampleTF.Text = number_sample; }
            if (p2o5_value == null || p2o5_value == "" || p2o5_value == "null")
            {
                AnalysPKValueP2O5TF.Text = String.Empty;
            }
            else { AnalysPKValueP2O5TF.Text = p2o5_value; }
            if (p2o5_control_value == null || p2o5_control_value == "" || p2o5_control_value == "null")
            {
                AnalysPKControlP2O5TF.Text = String.Empty;
            }
            else { AnalysPKControlP2O5TF.Text = p2o5_control_value; }
            if (k2o_value == null || k2o_value == "" || k2o_value == "null")
            {
                AnalysPKValueK2OTF.Text = String.Empty;
            }
            else { AnalysPKValueK2OTF.Text = k2o_value; }
            if (k2o_control_value == null || k2o_control_value == "" || k2o_control_value == "null")
            {
                AnalysPKControlK2OTF.Text = String.Empty;
            }
            else { AnalysPKControlK2OTF.Text = k2o_control_value; }
        }

        [DirectMethod]
        public void DeleteAnalysPK(String id_sample, String id_p_k)
        {
            if (connection_try)
            {
                if (id_p_k == null || id_p_k == "" || id_p_k == String.Empty || id_p_k == "0" || id_p_k == "null")
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Title = "Внимание!",
                        Message = "В данном образце нет значений фосфора и калия. Удаление записи невозможно!",
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING
                    });
                }
                else
                {
                    conn.Open();
                    SqlCommand delete_analys_p_k = new SqlCommand("Delete_Analys_P_K", conn);
                    delete_analys_p_k.CommandType = CommandType.StoredProcedure;
                    delete_analys_p_k.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                    delete_analys_p_k.ExecuteNonQuery();
                    conn.Close();
                }
                FillAnalysPKYear(AnalysPKYearCB.Text, AnalysPKTourCB.Text);
            }
        }

        //гидролитическая кислотность
        [DirectMethod]
        public void ShowAnalysHAW()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterAnalysHATourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHATourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysHATourS.DataSource = indexDV;
                AnalysHATourS.DataBind();
                SqlDataAdapter adapterAnalysHAYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHAYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysHAYearS.DataSource = indexDV;
                AnalysHAYearS.DataBind();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                AnalysHARegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                AnalysHAOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                AnalysHADepartmentTF.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                adapterAnalysHA = new SqlDataAdapter(selectCommAnalysHA + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHA.Fill(indexDS, "View_Analys_HA");
                indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HA"]);
                AnalysHAS.DataSource = indexDV;
                AnalysHAS.DataBind();
                conn.Close();
                AnalysHANumberSampleTF.Text = String.Empty;
                AnalysHAValueHATF.Text = String.Empty;
                AnalysHAControlTF.Text = String.Empty;
                AnalysHAFewTF.Text = String.Empty;
                AnalysHAIdSampleTF.Text = 0.ToString();
                AnalysHATourCB.Text = String.Empty;
                AnalysHAYearCB.Text = String.Empty;
                AnalysHAW.Show();
                //AnalysHATourCB.Select(0);
                AnalysHAGP.GetSelectionModel().Select(0);
            }
        }

        [DirectMethod]
        public void FillAnalysHATour(String tour)
        {
            if (tour != null && tour != "null" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysHA = new SqlDataAdapter(selectCommAnalysHA + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysHA.Fill(indexDS, "View_Analys_HA");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HA"]);
                    AnalysHAS.DataSource = indexDV;
                    AnalysHAS.DataBind();
                    SqlDataAdapter adapterAnalysHAYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysHAYearCB.Fill(indexDS, "Analys_sample");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample"]);
                    conn.Close();
                    AnalysHAYearS.DataSource = indexDV;
                    AnalysHAYearS.DataBind();
                    AnalysHAYearCB.Text = String.Empty;
                }
            }
        }

        [DirectMethod]
        public void FillAnalysHAYear(String year, String tour)
        {
            if (year != null && year != "null" && year != String.Empty && tour != null && tour != "null" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysHA = new SqlDataAdapter(selectCommAnalysHA + " WHERE id_department=" + current_id_department + " AND tour=" + tour + " AND year=" + year, conn);
                    adapterAnalysHA.Fill(indexDS, "View_Analys_HA");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HA"]);
                    AnalysHAS.DataSource = indexDV;
                    AnalysHAS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void AddEditAnalysHA(String tour, String year, String id_sample, String number_sample, String ha_value, String control_value)
        {
            if (connection_try && tour != null && tour != "null" && tour != String.Empty && year != null && year != "null" && year != String.Empty)
            {
                if (number_sample == null || number_sample == "null" || number_sample == String.Empty)
                {
                    number_sample = "1";
                }
                String temp = AdaptationValue(ha_value, "hydrolytic_acid");
                if (ha_value != temp) { ha_value = temp; }
                temp = AdaptationValue(control_value, "hydrolytic_acid");
                if (control_value != temp) { control_value = temp; }
                conn.Open();
                SqlCommand add_analys_ha = new SqlCommand("Add_Edit_Analys_HA", conn);
                add_analys_ha.CommandType = CommandType.StoredProcedure;
                add_analys_ha.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                add_analys_ha.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                add_analys_ha.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour)));
                add_analys_ha.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                add_analys_ha.Parameters.AddWithValue("@number_sample", NotNull(number_sample));
                add_analys_ha.Parameters.AddWithValue("@ha_value", Convert.ToDouble(NotNull(ha_value)));
                add_analys_ha.Parameters.AddWithValue("@control_value", Convert.ToDouble(NotNull(control_value)));
                add_analys_ha.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                add_analys_ha.Parameters.Add("@result", SqlDbType.VarChar, 20);
                add_analys_ha.Parameters["@result"].Direction = ParameterDirection.Output;
                add_analys_ha.ExecuteNonQuery();
                AnalysHANumberSampleTF.Text = add_analys_ha.Parameters["@result"].Value.ToString();
                //AnalysPhSNumberSampleTF.Text = (Convert.ToInt32(number_sample) + 1).ToString();
                //обновление существующих циклов и годов
                SqlDataAdapter adapterAnalysHATour = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHATour.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysHATourS.DataSource = indexDV;
                AnalysHATourS.DataBind();
                SqlDataAdapter adapterAnalysHAYear = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHAYear.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysHAYearS.DataSource = indexDV;
                AnalysHAYearS.DataBind();
                conn.Close();
                AnalysHAIdSampleTF.Text = 0.ToString();
                AnalysHAValueHATF.Text = String.Empty;
                AnalysHAControlTF.Text = String.Empty;
                AnalysHAFewTF.Text = String.Empty;
                FillAnalysHAYear(year, tour);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Результат добавления HA",
                    Message = "Отсутствует цикл или год, либо нет подключения к БД!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void AddFewAnalysHA(String tour_analys_ha, String year_analys_ha, String ha_values)
        {
            String str_values = ha_values.Replace('\t', ' ');
            if (str_values.Split(' ').Length > 0)
            {
                String[] values = new String[str_values.Split(' ').Length];
                values = str_values.Split(' ');
                SqlCommand add_analys_ha = new SqlCommand("Add_Edit_Analys_HA", conn);
                for (int i = 0; i < values.Length; i++)
                {
                    String temp = AdaptationValue(values[i], "hydrolytic_acid");
                    if (values[i] != temp) { values[i] = temp; }
                    conn.Open();
                    add_analys_ha = new SqlCommand("Add_Edit_Analys_HA", conn);
                    add_analys_ha.CommandType = CommandType.StoredProcedure;
                    add_analys_ha.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                    add_analys_ha.Parameters.AddWithValue("@id_sample", 0);
                    add_analys_ha.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour_analys_ha)));
                    add_analys_ha.Parameters.AddWithValue("@year", Convert.ToInt32(year_analys_ha));
                    add_analys_ha.Parameters.AddWithValue("@number_sample", 0);
                    add_analys_ha.Parameters.AddWithValue("@ha_value", Convert.ToDouble(NotNull(values[i])));
                    add_analys_ha.Parameters.AddWithValue("@control_value", Convert.ToDouble(0));
                    add_analys_ha.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                    add_analys_ha.Parameters.Add("@result", SqlDbType.VarChar, 20);
                    add_analys_ha.Parameters["@result"].Direction = ParameterDirection.Output;
                    add_analys_ha.ExecuteNonQuery();
                    conn.Close();
                    if (i == (values.Length - 1))
                    {
                        AnalysHANumberSampleTF.Text = add_analys_ha.Parameters["@result"].Value.ToString();
                    }
                }
                //обновление существующих циклов и годов
                conn.Open();
                SqlDataAdapter adapterAnalysHATourCB = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHATourCB.Fill(indexDS, "Analys_sample_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_tours"]);
                AnalysHATourS.DataSource = indexDV;
                AnalysHATourS.DataBind();
                SqlDataAdapter adapterAnalysHAYearCB = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHAYearCB.Fill(indexDS, "Analys_sample_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_sample_years"]);
                AnalysHAYearS.DataSource = indexDV;
                AnalysHAYearS.DataBind();
                conn.Close();
                AnalysHAIdSampleTF.Text = 0.ToString();
                AnalysHAValueHATF.Text = String.Empty;
                AnalysHAControlTF.Text = String.Empty;
                AnalysHAFewTF.Text = String.Empty;
                FillAnalysHAYear(year_analys_ha, tour_analys_ha);
            }
        }

        [DirectMethod]
        public void SelectAnalysHA(String id_sample, String number_sample, String ha_value, String control_value)
        {
            if (id_sample == null || id_sample == "" || id_sample == "null")
            {
                AnalysHAIdSampleTF.Text = 0.ToString();
            }
            else { AnalysHAIdSampleTF.Text = id_sample; }
            if (number_sample == null || number_sample == "" || number_sample == "null")
            {
                AnalysHANumberSampleTF.Text = String.Empty;
            }
            else { AnalysHANumberSampleTF.Text = number_sample; }
            if (ha_value == null || ha_value == "" || ha_value == "null")
            {
                AnalysHAValueHATF.Text = String.Empty;
                AnalysHAValuePhSTF.Text = String.Empty;
            }
            else
            {
                AnalysHAValueHATF.Text = ha_value;
                AnalysHAValuePhSTF.Text = GetPhSbyHA(ha_value);
            }
            if (control_value == null || control_value == "" || control_value == "null")
            {
                AnalysHAControlTF.Text = String.Empty;
                AnalysHAControlPhSTF.Text = String.Empty;
            }
            else
            {
                AnalysHAControlTF.Text = control_value;
                AnalysHAControlPhSTF.Text = GetPhSbyHA(control_value);
            }
        }

        [DirectMethod]
        public void DeleteAnalysHA(String id_sample, String id_ha)
        {
            if (connection_try)
            {
                if (id_ha == null || id_ha == "" || id_ha == String.Empty || id_ha == "0" || id_ha == "null")
                {
                    X.Msg.Confirm("Внимание!", "Вы действительно хотите удалить данный образец полностью?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "App.direct.DeleteAnalysSample_HA(" + id_sample + ");",
                            Text = "Да"
                        },
                        No = new MessageBoxButtonConfig
                        {
                            //Handler = "App.direct.FillAnalysHAYear(" + AnalysHAYearCB.Text + ", " + AnalysHATourCB.Text + ");",
                            Handler = "",
                            Text = "Нет"
                        }
                    }).Show();
                }
                else
                {
                    DeleteAnalysSample_HA(id_sample);
                }
            }
        }        

        [DirectMethod]
        public void DeleteAnalysSample_HA(String id_sample)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_analys_ha = new SqlCommand("Delete_Analys_HA", conn);
                delete_analys_ha.CommandType = CommandType.StoredProcedure;
                delete_analys_ha.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                delete_analys_ha.ExecuteNonQuery();
                conn.Close();
                if (AnalysHAYearCB.Text != String.Empty && AnalysHATourCB.Text != String.Empty)
                {
                    FillAnalysHAYear(AnalysHAYearCB.Text, AnalysHATourCB.Text);
                }
                else if (AnalysHATourCB.Text != String.Empty)
                {
                    FillAnalysHATour(AnalysHATourCB.Text);
                }
                else
                {
                    conn.Open();
                    adapterAnalysHA = new SqlDataAdapter(selectCommAnalysHA + " WHERE id_department=" + current_id_department, conn);
                    adapterAnalysHA.Fill(indexDS, "View_Analys_HA");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HA"]);
                    AnalysHAS.DataSource = indexDV;
                    AnalysHAS.DataBind();
                    conn.Close();
                }
            }
        }
        
        //тяжелые металлы
        [DirectMethod]
        public void ShowAnalysHMW()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterAnalysHMTour = new SqlDataAdapter(selectCommAnalysHMTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMTour.Fill(indexDS, "Analys_hm_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_tours"]);
                AnalysHMTourS.DataSource = indexDV;
                AnalysHMTourS.DataBind();
                SqlDataAdapter adapterAnalysHMYear = new SqlDataAdapter(selectCommAnalysHMYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMYear.Fill(indexDS, "Analys_hm_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_years"]);
                AnalysHMYearS.DataSource = indexDV;
                AnalysHMYearS.DataBind();
                SqlDataAdapter adapterAnalysHMElement = new SqlDataAdapter(selectCommAnalysHMElement, conn);
                adapterAnalysHMElement.Fill(indexDS, "Analys_hm_element");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_element"]);
                AnalysHMElementS.DataSource = indexDV;
                AnalysHMElementS.DataBind();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                AnalysHMRegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                AnalysHMOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                AnalysHMDepartmentTF.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                adapterAnalysHM = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHM.Fill(indexDS, "View_Analys_HM");
                indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HM"]);
                AnalysHMS.DataSource = indexDV;
                AnalysHMS.DataBind();
                conn.Close();
                AnalysHMNumberPlotTF.Text = String.Empty;
                AnalysHMElementCB.Text = String.Empty;
                AnalysHMControlTF.Text = String.Empty;
                AnalysHMTourCB.Text = String.Empty;
                AnalysHMYearCB.Text = String.Empty;
                AnalysHMW.Show();
                //AnalysHATourCB.Select(0);
                //AnalysHAGP.GetSelectionModel().Select(0);
            }
        }

        [DirectMethod]
        public void FillAnalysHMTour(String tour)
        {
            if (tour != null && tour != "null" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysHM = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysHM.Fill(indexDS, "View_Analys_HM");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HM"]);
                    AnalysHMS.DataSource = indexDV;
                    AnalysHMS.DataBind();
                    SqlDataAdapter adapterAnalysHMYear = new SqlDataAdapter(selectCommAnalysHMYears + " WHERE id_department=" + current_id_department + " AND tour=" + tour, conn);
                    adapterAnalysHMYear.Fill(indexDS, "Analys_hm_year");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_year"]);
                    conn.Close();
                    AnalysHMYearS.DataSource = indexDV;
                    AnalysHMYearS.DataBind();
                    AnalysHMYearCB.Text = String.Empty;
                }
            }
        }

        [DirectMethod]
        public void FillAnalysHMYear(String year, String tour)
        {
            if (year != null && year != "null" && year != String.Empty && tour != null && tour != "null" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysHM = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department + " AND tour=" + tour + " AND year=" + year, conn);
                    adapterAnalysHM.Fill(indexDS, "View_Analys_HM");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HM"]);
                    AnalysHMS.DataSource = indexDV;
                    AnalysHMS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void FillAnalysHMSignificative(String tour, String year, String id_significative)
        {
            if (year != null && year != "null" && year != String.Empty && tour != null && tour != "null" && tour != String.Empty &&
                id_significative != null && id_significative != "null" && id_significative != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    adapterAnalysHM = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department + " AND tour=" + tour + " AND year=" + year + " AND id_significative=" + id_significative, conn);
                    adapterAnalysHM.Fill(indexDS, "View_Analys_HM");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HM"]);
                    AnalysHMS.DataSource = indexDV;
                    AnalysHMS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void DeleteAnalysHM(String id_hm)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand delete_analys_hm = new SqlCommand("Delete_Analys_HM", conn);
                delete_analys_hm.CommandType = CommandType.StoredProcedure;
                delete_analys_hm.Parameters.AddWithValue("@id_hm", Convert.ToInt32(id_hm));
                delete_analys_hm.ExecuteNonQuery();
                conn.Close();
                if (AnalysHMYearCB.Text != String.Empty && AnalysHMTourCB.Text != String.Empty && AnalysHMElementCB.Text != String.Empty)
                {
                    FillAnalysHMSignificative(AnalysHMTourCB.Text, AnalysHMYearCB.Text, AnalysHMElementCB.Text);
                }
                else if (AnalysHMYearCB.Text != String.Empty && AnalysHMTourCB.Text != String.Empty )
                {
                    FillAnalysHMYear(AnalysHMYearCB.Text, AnalysHMTourCB.Text);
                }
                else if (AnalysHMTourCB.Text != String.Empty)
                {
                    FillAnalysHMTour(AnalysHMTourCB.Text);
                }
                else
                {
                    conn.Open();
                    adapterAnalysHM = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department, conn);
                    adapterAnalysHM.Fill(indexDS, "View_Analys_HM");
                    indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_HM"]);
                    AnalysHMS.DataSource = indexDV;
                    AnalysHMS.DataBind();
                    conn.Close();
                }
            }
        }

        [DirectMethod]
        public void AddEditAnalysHM(String tour, String year, String id_hm, String number_plot, String id_sign, String value, String control_value)
        {
            if (connection_try && tour != null && tour != "null" && tour != String.Empty && year != null && year != "null" && year != String.Empty)
            {
                if (number_plot == null || number_plot == "null" || number_plot == String.Empty || number_plot == "0")
                {
                    number_plot = "1";
                }
                conn.Open();
                adapterSignificative = new SqlDataAdapter(selectCommSignificative, conn);
                adapterSignificative.Fill(indexDS, "Significative");
                String name_significative = indexDS.Tables["Significative"].Select("id_significative=" + id_sign)[0]["name_significative"].ToString();
                String temp = AdaptationValue(value, name_significative);
                if (value != temp) { value = temp; }
                temp = AdaptationValue(control_value, name_significative);
                if (control_value != temp) { control_value = temp; }
                SqlCommand add_analys_hm = new SqlCommand("Add_Edit_Analys_HM", conn);
                add_analys_hm.CommandType = CommandType.StoredProcedure;
                add_analys_hm.Parameters.AddWithValue("@id_hm", Convert.ToInt32(id_hm));
                add_analys_hm.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                add_analys_hm.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour)));
                add_analys_hm.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                add_analys_hm.Parameters.AddWithValue("@number_plot", NotNull(number_plot));
                add_analys_hm.Parameters.AddWithValue("@value", Convert.ToDouble(NotNull(value)));
                add_analys_hm.Parameters.AddWithValue("@control_value", Convert.ToDouble(NotNull(control_value)));
                add_analys_hm.Parameters.AddWithValue("@id_significative", Convert.ToInt32(id_sign));
                add_analys_hm.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                add_analys_hm.Parameters.Add("@result", SqlDbType.VarChar, 20);
                add_analys_hm.Parameters["@result"].Direction = ParameterDirection.Output;
                add_analys_hm.ExecuteNonQuery();
                AnalysHMNumberPlotTF.Text = add_analys_hm.Parameters["@result"].Value.ToString();
                //обновление существующих циклов и годов
                SqlDataAdapter adapterAnalysHMTour = new SqlDataAdapter(selectCommAnalysHMTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMTour.Fill(indexDS, "Analys_hm_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_tours"]);
                AnalysHMTourS.DataSource = indexDV;
                AnalysHMTourS.DataBind();
                SqlDataAdapter adapterAnalysHMYear = new SqlDataAdapter(selectCommAnalysHMYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMYear.Fill(indexDS, "Analys_hm_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_years"]);
                AnalysHMYearS.DataSource = indexDV;
                AnalysHMYearS.DataBind();
                conn.Close();
                AnalysHMIdHMTF.Text = 0.ToString();
                AnalysHMNumberPlotTF.Text = String.Empty;
                AnalysHMValueTF.Text = String.Empty;
                AnalysHMControlTF.Text = String.Empty;
                AnalysHMFewTF.Text = String.Empty;
                FillAnalysHMSignificative(tour, year, id_sign);
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Результат добавления ТМ",
                    Message = "Отсутствует элемент, цикл или год, либо нет подключения к БД!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }

        }

        [DirectMethod]
        public void SelectAnalysHM(String id_hm, String number_plot, String value, String control_value)
        {
            if (id_hm == null || id_hm == "" || id_hm == "null")
            {
                AnalysHMIdHMTF.Text = 0.ToString();
            }
            else { AnalysHMIdHMTF.Text = id_hm; }
            if (number_plot == null || number_plot == "" || number_plot == "null")
            {
                AnalysHMNumberPlotTF.Text = String.Empty;
            }
            else { AnalysHMNumberPlotTF.Text = number_plot; }
            if (value == null || value == "" || value == "null")
            {
                AnalysHMValueTF.Text = String.Empty;
            }
            else
            {
                AnalysHMValueTF.Text = value;
            }
            if (control_value == null || control_value == "" || control_value == "null")
            {
                AnalysHMControlTF.Text = String.Empty;
            }
            else
            {
                AnalysHMControlTF.Text = control_value;
            }
        }

        [DirectMethod]
        public void AddFewAnalysHM(String tour, String year, String id_sign, String hm_values)
        {
            String str_values = hm_values.Replace('\t', ' ');
            if (str_values.Split(' ').Length > 0)
            {
                String[] values = new String[str_values.Split(' ').Length];
                values = str_values.Split(' ');
                conn.Open();
                adapterSignificative = new SqlDataAdapter(selectCommSignificative, conn);
                adapterSignificative.Fill(indexDS, "Significative");
                conn.Close();
                String name_significative = indexDS.Tables["Significative"].Select("id_significative=" + id_sign)[0]["name_significative"].ToString();
                SqlCommand add_analys_hm = new SqlCommand("Add_Edit_Analys_HM", conn);
                for (int i = 0; i < values.Length; i++)
                {
                    String temp = AdaptationValue(values[i], name_significative);
                    if (values[i] != temp) { values[i] = temp; }
                    conn.Open();
                    add_analys_hm = new SqlCommand("Add_Edit_Analys_HM", conn);
                    add_analys_hm.CommandType = CommandType.StoredProcedure;
                    add_analys_hm.Parameters.AddWithValue("@id_hm", 0);
                    add_analys_hm.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                    add_analys_hm.Parameters.AddWithValue("@tour", Convert.ToInt32(NotNull(tour)));
                    add_analys_hm.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                    add_analys_hm.Parameters.AddWithValue("@number_plot", "1");
                    add_analys_hm.Parameters.AddWithValue("@value", Convert.ToDouble(NotNull(values[i])));
                    add_analys_hm.Parameters.AddWithValue("@control_value", Convert.ToDouble(0));
                    add_analys_hm.Parameters.AddWithValue("@id_significative", Convert.ToInt32(id_sign));
                    add_analys_hm.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                    add_analys_hm.Parameters.Add("@result", SqlDbType.VarChar, 20);
                    add_analys_hm.Parameters["@result"].Direction = ParameterDirection.Output;
                    add_analys_hm.ExecuteNonQuery();
                    conn.Close();
                    if (i == (values.Length - 1))
                    {
                        AnalysHMNumberPlotTF.Text = add_analys_hm.Parameters["@result"].Value.ToString();
                    }
                }
                //обновление существующих циклов и годов
                conn.Open();
                SqlDataAdapter adapterAnalysHMTour = new SqlDataAdapter(selectCommAnalysHMTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMTour.Fill(indexDS, "Analys_hm_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_tours"]);
                AnalysHMTourS.DataSource = indexDV;
                AnalysHMTourS.DataBind();
                SqlDataAdapter adapterAnalysHMYear = new SqlDataAdapter(selectCommAnalysHMYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysHMYear.Fill(indexDS, "Analys_hm_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_hm_years"]);
                AnalysHMYearS.DataSource = indexDV;
                AnalysHMYearS.DataBind();
                conn.Close();
                AnalysHMIdHMTF.Text = 0.ToString();
                AnalysHMNumberPlotTF.Text = String.Empty;
                AnalysHMValueTF.Text = String.Empty;
                AnalysHMControlTF.Text = String.Empty;
                AnalysHMFewTF.Text = String.Empty;
                FillAnalysHMSignificative(tour, year, id_sign);
            }
        }

        // микроэлементы 
        [DirectMethod]
        public void ShowAnalysMicroW()
        {
            /*if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterAnalysMicroTour = new SqlDataAdapter(selectCommAnalysMicroTours + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysMicroTour.Fill(indexDS, "Analys_micro_tours");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_micro_tours"]);
                AnalysMicroTourS.DataSource = indexDV;
                AnalysMicroTourS.DataBind();
                SqlDataAdapter adapterAnalysMicroYear = new SqlDataAdapter(selectCommAnalysMicroYears + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysMicroYear.Fill(indexDS, "Analys_micro_years");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_micro_years"]);
                AnalysMicroYearS.DataSource = indexDV;
                AnalysMicroYearS.DataBind();
                SqlDataAdapter adapterAnalysMicroElement = new SqlDataAdapter(selectCommAnalysMicroElement, conn);
                adapterAnalysMicroElement.Fill(indexDS, "Analys_micro_element");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_micro_element"]);
                AnalysMicroElementS.DataSource = indexDV;
                AnalysMicroElementS.DataBind();
                SqlCommand get_names_rod = new SqlCommand("Get_Names_ROD", conn);
                get_names_rod.CommandType = CommandType.StoredProcedure;
                get_names_rod.Parameters.AddWithValue("@id_region", Convert.ToInt32(current_id_region));
                get_names_rod.Parameters.AddWithValue("@id_organization", Convert.ToInt32(current_id_organization));
                get_names_rod.Parameters.AddWithValue("@id_department", Convert.ToInt32(current_id_department));
                get_names_rod.Parameters.Add("@title_region", SqlDbType.VarChar, 50);
                get_names_rod.Parameters.Add("@title_organization", SqlDbType.VarChar, 200);
                get_names_rod.Parameters.Add("@title_department", SqlDbType.VarChar, 200);
                get_names_rod.Parameters["@title_region"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_organization"].Direction = ParameterDirection.Output;
                get_names_rod.Parameters["@title_department"].Direction = ParameterDirection.Output;
                get_names_rod.ExecuteNonQuery();
                AnalysMicroRegionTF.Text = get_names_rod.Parameters["@title_region"].Value.ToString();
                AnalysMicroOrganizationTF.Text = get_names_rod.Parameters["@title_organization"].Value.ToString();
                AnalysMicroDepartmentTF.Text = get_names_rod.Parameters["@title_department"].Value.ToString();
                adapterAnalysMicro = new SqlDataAdapter(selectCommAnalysHM + " WHERE id_department=" + current_id_department, conn);
                adapterAnalysMicro.Fill(indexDS, "View_Analys_Micro");
                indexDV = new System.Data.DataView(indexDS.Tables["View_Analys_Micro"]);
                AnalysMicroS.DataSource = indexDV;
                AnalysMicroS.DataBind();
                conn.Close();
                AnalysMicroNumberPlotTF.Text = String.Empty;
                AnalysMicroElementCB.Text = String.Empty;
                AnalysMicroControlTF.Text = String.Empty;
                AnalysMicroTourCB.Text = String.Empty;
                AnalysMicroYearCB.Text = String.Empty;
                AnalysMicroW.Show();
            }*/
        }
        //отчет по элементам
        [DirectMethod]
        //public void ShowReportAnalysW(String id_region, String id_organization, String title_region, String title_organization, String id_department, String title_department)
        public void ShowReportAnalysW()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRegion = new SqlDataAdapter(selectCommAnalysRegion, conn);
                adapterRegion.Fill(indexDS, "Region");
                conn.Close();
                indexDV = new System.Data.DataView(indexDS.Tables["Region"]);
                ReportAnalysRegionS.DataSource = indexDV;
                ReportAnalysRegionS.DataBind();

                //добавляем элементы
                List<object> ElementsAnalys = new List<object>();
                ElementsAnalys.Add(new { id_element = 1, element = "pH (сол.)" });
                ElementsAnalys.Add(new { id_element = 2, element = "P, K" });
                ElementsAnalys.Add(new { id_element = 3, element = "Hг" });

                ElementsAnalys.Add(new { id_element = 4, element = "Тяжёлые металлы" });
                ElementsAnalys.Add(new { id_element = 5, element = "CU (тм)" });
                ElementsAnalys.Add(new { id_element = 6, element = "Zn (тм)" });
                ElementsAnalys.Add(new { id_element = 7, element = "Cd (тм)" });
                ElementsAnalys.Add(new { id_element = 8, element = "Pb (тм)" });
                ElementsAnalys.Add(new { id_element = 9, element = "Ni (тм)" });
                ElementsAnalys.Add(new { id_element = 10, element = "Hg (тм)" });
                ElementsAnalys.Add(new { id_element = 11, element = "Mg (тм)" });
                ElementsAnalys.Add(new { id_element = 12, element = "Cr (тм)" });
                ElementsAnalys.Add(new { id_element = 13, element = "Fe (тм)" });
                ElementsAnalys.Add(new { id_element = 14, element = "F (тм)" });
                ElementsAnalys.Add(new { id_element = 15, element = "As (тм)" });

                ReportAnalysElementS.DataSource = ElementsAnalys;
                ReportAnalysElementS.DataBind();

                //ReportAnalysYearCB.Text = String.Empty;
                //FillReportAnalysTour(id_department);
                ResetReportAnalys();
                ReportAnalysW.Show();
            }
        }

        [DirectMethod]
        public void FillReportAnalysOrganization(String id_region)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter(selectCommAnalysOrganization + " WHERE id_region = " + id_region, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                indexDV = new System.Data.DataView(indexDS.Tables["Organization"]);
                conn.Close();
                ReportAnalysOrganizationS.DataSource = indexDV;
                ReportAnalysOrganizationS.DataBind();
                ReportAnalysOrganizationCB.Clear();
                ReportAnalysDepartmentCB.Clear();
                ReportAnalysTourCB.Clear();
                ReportAnalysYearCB.Clear();
                //ReportAnalysOrganizationCB.Select(0);
            }
        }

        [DirectMethod]
        public void FillReportAnalysDepartment(String id_organization)
        {
            if (connection_try)
            {
                conn.Open();
                adapterDepartment = new SqlDataAdapter(selectCommAnalysDepartment + " WHERE id_organization = " + id_organization, conn);
                adapterDepartment.Fill(indexDS, "Department");
                indexDV = new System.Data.DataView(indexDS.Tables["Department"]);
                conn.Close();
                ReportAnalysDepartmentS.DataSource = indexDV;
                ReportAnalysDepartmentS.DataBind();
                ReportAnalysDepartmentCB.Clear();
                ReportAnalysTourCB.Clear();
                ReportAnalysYearCB.Clear();
                //ReportAnalysDepartmentCB.Select(0);
            }
        }

        [DirectMethod]
        public void FillReportAnalysTour(String id_department)
        {
            if (id_department != null && id_department != "" && id_department != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    SqlDataAdapter adapterReportAnalysTour = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + id_department, conn);
                    adapterReportAnalysTour.Fill(indexDS, "Analys_tour");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_tour"]);
                    conn.Close();
                    ReportAnalysTourS.DataSource = indexDV;
                    ReportAnalysTourS.DataBind();
                    ReportAnalysTourCB.Clear();
                    ReportAnalysYearCB.Clear();
                    //ReportAnalysTourCB.Select(0);
                }
            }
        }

        [DirectMethod]
        public void FillReportAnalysYear(String tour, String id_department)
        {
            if (tour != null && tour != "" && tour != String.Empty)
            {
                if (connection_try)
                {
                    conn.Open();
                    SqlDataAdapter adapterReportAnalysYear = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department = " + id_department + " AND tour = " + tour, conn);
                    adapterReportAnalysYear.Fill(indexDS, "Analys_year");
                    indexDV = new System.Data.DataView(indexDS.Tables["Analys_year"]);
                    conn.Close();
                    ReportAnalysYearS.DataSource = indexDV;
                    ReportAnalysYearS.DataBind();
                    ReportAnalysYearCB.Clear();
                    //ReportAnalysYearCB.Select(0);
                }
            }
        }

        [DirectMethod]
        public void ResetReportAnalys()
        {
            ReportAnalysRegionCB.Clear();
            ReportAnalysOrganizationCB.Clear();
            ReportAnalysDepartmentCB.Clear();
            ReportAnalysTourCB.Clear();
            ReportAnalysYearCB.Clear();
            ReportAnalysElementCB.Clear();

            //ReportAnalysRegionS.RemoveAll();
            ReportAnalysOrganizationS.RemoveAll();
            ReportAnalysDepartmentS.RemoveAll();
            ReportAnalysTourS.RemoveAll();
            ReportAnalysYearS.RemoveAll();
            //ReportAnalysDF2.Clear();
            //ReportAnalysDF1.Clear();
            ReportAnalysDF1.SelectedValue = DateTime.Today;
            ReportAnalysDF2.SelectedValue = DateTime.Today;
            ReportAnalysElementCB.Select(0);
        }

        [DirectMethod]
        public void CancelReportAnalys()
        {
            ReportAnalysW.Close();
        }

        [DirectMethod]
        public void AcceptReportAnalys()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie report_analys = Request.Cookies["Agrochim31_Report_Analys"];
                if (report_analys == null)
                {
                    report_analys = new HttpCookie("Agrochim31_Report_Analys");
                    Response.Cookies.Add(report_analys);
                }

                if (ReportAnalysRegionCB.Text == String.Empty)
                {
                    report_analys["id_region"] = "0";
                }
                else
                {
                    report_analys["id_region"] = ReportAnalysRegionCB.Value.ToString();
                }

                if (ReportAnalysOrganizationCB.Text == String.Empty)
                {
                    report_analys["id_organization"] = "0";
                }
                else
                {
                    report_analys["id_organization"] = ReportAnalysOrganizationCB.Value.ToString();
                }

                if (ReportAnalysDepartmentCB.Text == String.Empty)
                {
                    report_analys["id_department"] = "0";
                }
                else
                {
                    report_analys["id_department"] = ReportAnalysDepartmentCB.Value.ToString();
                }

                if (ReportAnalysTourCB.Text == String.Empty)
                {
                    report_analys["tour_analys"] = "0";
                }
                else
                {
                    report_analys["tour_analys"] = ReportAnalysTourCB.Value.ToString();
                }

                if (ReportAnalysYearCB.Text == String.Empty)
                {
                    report_analys["year_analys"] = "0";
                }
                else
                {
                    report_analys["year_analys"] = ReportAnalysYearCB.Value.ToString();
                }

                //"dd.MM.yyyy HH:mm:ss"
                report_analys["date_from_analys"] = ReportAnalysDF1.SelectedDate.Ticks.ToString();

                report_analys["date_to_analys"] = ReportAnalysDF2.SelectedDate.AddDays(1).AddMilliseconds(-1).Ticks.ToString();

                Response.Cookies.Set(report_analys);

                //вызов отчётов
                if (ReportAnalysElementCB.Text != String.Empty)
                {
                    switch (Convert.ToInt32(ReportAnalysElementCB.Value))
                    {
                        case 1: {
                            //ReportAnalysW.Close(); 
                            win_ph.Reload(); 
                            win_ph.Show(); 
                            break; }
                        case 2: {
                            //ReportAnalysW.Close();
                            win_p_k.Reload();
                            win_p_k.Show();
                            break; }
                        case 3: {
                            //ReportAnalysW.Close();
                            win_ha.Reload();
                            win_ha.Show();
                            break; }
                        case 4: {

                            break; }
                        case 5: {

                            break; }
                        case 6: {

                            break;}
                        case 7: {

                            break;}
                        case 8: {

                            break;}
                        case 9: {

                            break;}
                        case 10: {

                            break;}
                        case 11: {

                            break;}
                        case 12: {

                            break;}
                        case 13: {

                            break;}
                        case 14: {

                            break;}
                        case 15: {

                            break;}
                    }
                }
                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = (report_analys["date_to_analys"].ToString() + "    " + report_analys["date_from_analys"].ToString()),
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void AcceptReportAnalysControl()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie report_analys = Request.Cookies["Agrochim31_Report_Analys"];
                if (report_analys == null)
                {
                    report_analys = new HttpCookie("Agrochim31_Report_Analys");
                    Response.Cookies.Add(report_analys);
                }

                if (ReportAnalysRegionCB.Text == String.Empty)
                {
                    report_analys["id_region"] = "0";
                }
                else
                {
                    report_analys["id_region"] = ReportAnalysRegionCB.Value.ToString();
                }

                if (ReportAnalysOrganizationCB.Text == String.Empty)
                {
                    report_analys["id_organization"] = "0";
                }
                else
                {
                    report_analys["id_organization"] = ReportAnalysOrganizationCB.Value.ToString();
                }

                if (ReportAnalysDepartmentCB.Text == String.Empty)
                {
                    report_analys["id_department"] = "0";
                }
                else
                {
                    report_analys["id_department"] = ReportAnalysDepartmentCB.Value.ToString();
                }

                if (ReportAnalysTourCB.Text == String.Empty)
                {
                    report_analys["tour_analys"] = "0";
                }
                else
                {
                    report_analys["tour_analys"] = ReportAnalysTourCB.Value.ToString();
                }

                if (ReportAnalysYearCB.Text == String.Empty)
                {
                    report_analys["year_analys"] = "0";
                }
                else
                {
                    report_analys["year_analys"] = ReportAnalysYearCB.Value.ToString();
                }

                //"dd.MM.yyyy HH:mm:ss"
                report_analys["date_from_analys"] = ReportAnalysDF1.SelectedDate.Ticks.ToString();

                report_analys["date_to_analys"] = ReportAnalysDF2.SelectedDate.AddDays(1).AddMilliseconds(-1).Ticks.ToString();

                Response.Cookies.Set(report_analys);

                //вызов отчётов
                if (ReportAnalysElementCB.Text != String.Empty)
                {
                    switch (Convert.ToInt32(ReportAnalysElementCB.Value))
                    {
                        case 1: {
                            //ReportAnalysW.Close();
                            win_ph_control.Reload();
                            win_ph_control.Show();
                            break; }
                        case 2: {
                            //ReportAnalysW.Close();
                            win_p_k_control.Reload();
                            win_p_k_control.Show();
                            break; }
                        case 3: {
                            //ReportAnalysW.Close();
                            win_ha_control.Reload();
                            win_ha_control.Show();
                            break; }
                        case 4: break;
                        case 5: break;
                    }
                }
                /*X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = (report_analys["date_to_analys"].ToString() + "    " + report_analys["date_from_analys"].ToString()),
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });*/
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        public String GetHAbyPhS(String ph_s)
        {
            String result = String.Empty;
            //String temp = AdaptationValue(ph_s, "ph_s");
            String temp = ReplaceValue(ph_s, "2");
            if (ph_s != temp) { ph_s = temp; }
            if (connection_try && ph_s != "0" && ph_s != "null" && ph_s != String.Empty && ph_s != null)
            {
                conn.Open();
                SqlCommand get_value = new SqlCommand("Get_HA_by_pH", conn);
                get_value.CommandType = CommandType.StoredProcedure;
                get_value.Parameters.AddWithValue("@ph_s_value", Convert.ToDouble(ph_s));
                get_value.Parameters.Add("@ha_value", SqlDbType.Float);
                get_value.Parameters["@ha_value"].Direction = ParameterDirection.Output;
                get_value.ExecuteNonQuery();
                result = get_value.Parameters["@ha_value"].Value.ToString();
                conn.Close();
            }
            return result;
        }

        public String GetPhSbyHA(String ha)
        {
            String result = String.Empty;
            //String temp = AdaptationValue(ha, "hydrolytic_acid");
            String temp = ReplaceValue(ha, "2");
            if (ha != temp) { ha = temp; }
            if (connection_try && ha != "0" && ha != "null" && ha != String.Empty && ha != null)
            {
                conn.Open();
                SqlCommand get_value = new SqlCommand("Get_pH_by_HA", conn);
                get_value.CommandType = CommandType.StoredProcedure;
                get_value.Parameters.AddWithValue("@ha_value", Convert.ToDouble(ha));
                get_value.Parameters.Add("@ph_s_value", SqlDbType.Float);
                get_value.Parameters["@ph_s_value"].Direction = ParameterDirection.Output;
                get_value.ExecuteNonQuery();
                result = get_value.Parameters["@ph_s_value"].Value.ToString();
                conn.Close();
            }
            return result;
        }

        [DirectMethod]
        public void GetHAValuebyPhSValue(String ph_s_value)
        {
            if (ph_s_value != "0" && ph_s_value != "null" && ph_s_value != String.Empty && ph_s_value != null)
            {
                AnalysHAValueHATF.Text = GetHAbyPhS(ph_s_value);
            }
            else
            {
                AnalysHAValueHATF.Text = String.Empty;
            }
        }

        [DirectMethod]
        public void GetHAControlbyPhSControl(String ph_s_control)
        {
            if (ph_s_control != "0" && ph_s_control != "null" && ph_s_control != String.Empty && ph_s_control != null)
            {
                AnalysHAControlTF.Text = GetHAbyPhS(ph_s_control);
            }
            else
            {
                AnalysHAControlTF.Text = String.Empty;
            }
        }

        // окно отчета по планам
        [DirectMethod]
        public void ShowReportPlansW()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRegion = new SqlDataAdapter(selectCommRegion, conn);
                adapterRegion.Fill(indexDS, "Region");
                adapterWorker = new SqlDataAdapter(selectCommPlanUsers, conn);
                adapterWorker.Fill(indexDS, "Worker");
                //adapterMissions = new SqlDataAdapter(selectCommMissions, conn);
                //adapterMissions.Fill(indexDS, "Missions");
                conn.Close();
                indexDV = new System.Data.DataView(indexDS.Tables["Region"]);
                ReportPlanRegionS.DataSource = indexDV;
                ReportPlanRegionS.DataBind();
                indexDV = new System.Data.DataView(indexDS.Tables["Worker"]);
                ReportPlanWorkerS.DataSource = indexDV;
                ReportPlanWorkerS.DataBind();
                indexDV = new System.Data.DataView(indexDS.Tables["Missions"]);
                ReportPlanMissionsS.DataSource = indexDV;
                ReportPlanMissionsS.DataBind();

                ResetReportPlan();
                ReportPlanW.Show();
            }
        }

        [DirectMethod]
        public void ResetReportPlan()
        {
            ReportPlanRegionCB.Clear();
            ReportPlanWorkerCB.Clear();
            ReportPlanMissionsCB.Clear();
            ReportPlanDF1.SelectedValue = new DateTime(DateTime.Today.Year, 1, 1);
            ReportPlanDF2.SelectedValue = new DateTime(DateTime.Today.Year, 12,31);
        }

        [DirectMethod]
        public void CancelReportPlan()
        {
            ReportPlanW.Close();
        }
        
        [DirectMethod]
        public void AcceptReportPlan()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie report_plan = Request.Cookies["Agrochim31_Report_Plan"];
                if (report_plan == null)
                {
                    report_plan = new HttpCookie("Agrochim31_Report_Plan");
                    Response.Cookies.Add(report_plan);
                }

                if (ReportPlanRegionCB.Text == String.Empty)
                {
                    report_plan["id_region"] = "0";
                }
                else
                {
                    report_plan["id_region"] = ReportPlanRegionCB.Value.ToString();
                }

                if (ReportPlanWorkerCB.Text == String.Empty)
                {
                    report_plan["id_worker"] = "0";
                }
                else
                {
                    report_plan["id_worker"] = ReportPlanWorkerCB.Value.ToString();
                }

                if (ReportPlanMissionsCB.Text == String.Empty)
                {
                    report_plan["id_mission"] = "0";
                }
                else
                {
                    report_plan["id_mission"] = ReportPlanMissionsCB.Value.ToString();
                }

                //"dd.MM.yyyy HH:mm:ss"
                report_plan["date_from_plan"] = ReportPlanDF1.SelectedDate.Ticks.ToString();

                report_plan["date_to_plan"] = ReportPlanDF2.SelectedDate.AddDays(1).AddMilliseconds(-1).Ticks.ToString();

                Response.Cookies.Set(report_plan);

                if (ReportPlanRegionCB.Text != String.Empty && ReportPlanWorkerCB.Text != String.Empty && ReportPlanMissionsCB.Text != String.Empty)
                {
                    //ReportPlanW.Close();
                    win_plan.Reload();
                    win_plan.Show();
                }
                else if (ReportPlanRegionCB.Text != String.Empty)
                {
                    //ReportPlanW.Close();
                    win_plan_region.Reload();
                    win_plan_region.Show();
                }
                else if (ReportPlanWorkerCB.Text != String.Empty)
                {
                    //ReportPlanW.Close();
                    win_plan_worker.Reload();
                    win_plan_worker.Show();
                }
                else if (ReportPlanMissionsCB.Text != String.Empty)
                {
                    //ReportPlanW.Close();
                    win_plan_mission.Reload();
                    win_plan_mission.Show();
                }
                else 
                {
                    //ReportPlanW.Close();
                    win_plan_null.Reload();
                    win_plan_null.Show();
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void AcceptReportDriver()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie report_driver = Request.Cookies["Agrochim31_Report_Driver"];
                if (report_driver == null)
                {
                    report_driver = new HttpCookie("Agrochim31_Report_Driver");
                    Response.Cookies.Add(report_driver);
                }

                if (ReportPlanRegionCB.Text == String.Empty)
                {
                    report_driver["id_region"] = "0";
                }
                else
                {
                    report_driver["id_region"] = ReportPlanRegionCB.Value.ToString();
                }

                if (ReportPlanWorkerCB.Text == String.Empty)
                {
                    report_driver["id_worker"] = "0";
                }
                else
                {
                    report_driver["id_worker"] = ReportPlanWorkerCB.Value.ToString();
                }

                if (ReportPlanMissionsCB.Text == String.Empty)
                {
                    report_driver["id_mission"] = "0";
                }
                else
                {
                    report_driver["id_mission"] = ReportPlanMissionsCB.Value.ToString();
                }

                //"dd.MM.yyyy HH:mm:ss"
                report_driver["date_from_plan"] = ReportPlanDF1.SelectedDate.Ticks.ToString();

                report_driver["date_to_plan"] = ReportPlanDF2.SelectedDate.AddDays(1).AddMilliseconds(-1).Ticks.ToString();

                Response.Cookies.Set(report_driver);
                //ReportPlanW.Close();
                win_driver.Reload();
                win_driver.Show();                
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Ошибка cookies",
                    Message = "Необходимо включить поддержку cookies!!!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public void ShowRegionsTourForReportW(String type_report)
        {
            if (connection_try)
            {
                conn.Open();
                //очистка записей таблицы
                SqlCommand clear_regions_tour = new SqlCommand("Clear_RegionsTour", conn);
                clear_regions_tour.ExecuteNonQuery();
                //заполнение таблицы
                adapterRegionsTourForReport = new SqlDataAdapter(selectCommRegionsTourForReport, conn);
                adapterRegionsTourForReport.Fill(indexDS, "RegionsTourForReport");
                indexDV = new System.Data.DataView(indexDS.Tables["RegionsTourForReport"]);
                RegionsTourForReportS.DataSource = indexDV;
                RegionsTourForReportS.DataBind();
                conn.Close();
            }
            if (Convert.ToInt32(type_report) == 0)
            {
                GetRegionsTourForReportB.Hidden = false;
                GetRegionsTourForReportHMB.Hidden = true;
            }
            else if (Convert.ToInt32(type_report) == 1)
            {
                GetRegionsTourForReportHMB.Hidden = false;
                GetRegionsTourForReportB.Hidden = true;
            }
            RegionsTourForReportW.Show();
        }

        [DirectMethod]
        public void CancelRegionsTourForReportW()
        {
            RegionsTourForReportW.Close();
        }

        [DirectMethod]
        public void BindRegionsTour(String id_region)
        {
            if (connection_try)
            {
                conn.Open();
                selectReportTours = "SELECT DISTINCT tour FROM View_Plots_Tree WHERE id_region = " + id_region;
                adapterRegionsTour = new SqlDataAdapter(selectReportTours, conn);
                adapterRegionsTour.Fill(indexDS, "RegionsTour");
                indexDV = new System.Data.DataView(indexDS.Tables["RegionsTour"]);
                EditRegionsTourForReportTourS.DataSource = indexDV;
                EditRegionsTourForReportTourS.DataBind();
                EditRegionsTourForReportTourS.Sort("tour", Ext.Net.SortDirection.ASC);
                conn.Close();
            }
        }

        [DirectMethod]
        public void GetRegionsTourForReport(String type_report, String regions_tour_data)
        {
            List<Dictionary<String, String>> records = JSON.Deserialize<List<Dictionary<String, String>>>(regions_tour_data);
            if (records.Count > 0 && connection_try)
            {
                SqlCommand add_edit_regions_tour;
                conn.Open();
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i]["tour"] != null)
                    {
                        /*X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = (records[i]["tour"] + " : " + records[i]["id_region"]),
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });*/
                        String id_region = records[i]["id_region"];
                        String tour = records[i]["tour"];
                        add_edit_regions_tour = new SqlCommand("Add_Edit_RegionsTour", conn);
                        add_edit_regions_tour.CommandType = CommandType.StoredProcedure;
                        add_edit_regions_tour.Parameters.AddWithValue("@id_region", Convert.ToInt32(id_region));
                        add_edit_regions_tour.Parameters.AddWithValue("@tour", Convert.ToInt32(tour));
                        add_edit_regions_tour.ExecuteNonQuery();
                    }
                }
                conn.Close();

            }
            CancelRegionsTourForReportW();
            win_7_1.Reload();
            win_7_1.Show();
        }

        [DirectMethod]
        public void UpdateGroupsBySignificative(String id_significative)
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand update_groups_by_significative = new SqlCommand("Update_Groups_By_Significative", conn);
                update_groups_by_significative.CommandType = CommandType.StoredProcedure;
                update_groups_by_significative.CommandTimeout = 10000;
                update_groups_by_significative.Parameters.AddWithValue("@id_significative", Convert.ToInt32(id_significative));
                update_groups_by_significative.ExecuteNonQuery();
                conn.Close();
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = "Внимание!",
                    Message = "Обновление групп завершено!",
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO
                });
            }
        }

        [DirectMethod]
        public String ColorPhS(String id_plot, String name_significative, String value_significative, String id_method)
        {
            String result = value_significative;
            if (connection_try)
            {
                String color = String.Empty;
                conn.Open();
                SqlCommand get_color = new SqlCommand("GetColorPlot", conn);
                get_color.CommandType = CommandType.StoredProcedure;
                get_color.Parameters.AddWithValue("@name_significative", Convert.ToInt32(id_plot));
                get_color.Parameters.AddWithValue("@name_significative", name_significative);
                get_color.Parameters.AddWithValue("@value_significative", ReplaceValue(value_significative, "2"));
                get_color.Parameters.AddWithValue("@id_method", Convert.ToInt32(id_method));
                get_color.Parameters.Add("@color", SqlDbType.Float);
                get_color.Parameters["@color"].Direction = ParameterDirection.Output;
                get_color.ExecuteNonQuery();
                color = get_color.Parameters["@color"].Value.ToString();
                conn.Close();
                result = "<div style=\"height:20px; margin:1px; padding:0px; width:100%; background-color:#" + color + ";\">" + value_significative + "</div>";
            }
            return result;
        }

        [DirectMethod]
        public void PrintStickers(String id_organization, String planned_probes)
        {
            if (Request.Browser.Cookies && planned_probes != null && planned_probes != "null" && planned_probes != String.Empty)
            {
                HttpCookie report_stickers = Request.Cookies["Agrochim31_Report_Stickers"];
                if (report_stickers == null)
                {
                    report_stickers = new HttpCookie("Agrochim31_Report_Stickers");
                    Response.Cookies.Add(report_stickers);
                }
                report_stickers["id_organization"] = id_organization;
                report_stickers["count_probes"] = planned_probes;
                Response.Cookies.Set(report_stickers);
                win_stickers.Reload();
                win_stickers.Show();
            }
        }

        [DirectMethod]
        public void PrintForm(String id_organization, String value, String date, String number_form)
        {
            if (Request.Browser.Cookies && value != null && value != "null" && value != String.Empty)
            {
                DateTime dt_date = DateTime.ParseExact(date.Replace("\"", ""), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                HttpCookie report_form = Request.Cookies["Agrochim31_Report_Form"];
                if (report_form == null)
                {
                    report_form = new HttpCookie("Agrochim31_Report_Form");
                    Response.Cookies.Add(report_form);
                }
                report_form["id_organization"] = id_organization;
                report_form["value"] = value;
                report_form["year"] = dt_date.Year.ToString();
                Response.Cookies.Set(report_form);
                switch (number_form)
                {
                    case "1":
                        {
                            win_form_1.Reload();
                            win_form_1.Show();
                            break;
                        }
                    case "2":
                        {
                            win_form_2.Reload();
                            win_form_2.Show();
                            break;
                        }
                }
            }
        }

        [DirectMethod]
        public void ResetAnalysToPlot()
        {
            AnalysToPlotRegionCB.Clear();
            AnalysToPlotOrganizationCB.Clear();
            AnalysToPlotDepartmentCB.Clear();
            AnalysToPlotTourCB.Clear();
            AnalysToPlotYearCB.Clear();

            AnalysToPlotOrganizationS.RemoveAll();
            AnalysToPlotDepartmentS.RemoveAll();
            AnalysToPlotTourS.RemoveAll();
            AnalysToPlotYearS.RemoveAll();

            AnalysToPlotPlotsS.RemoveAll();
            AnalysToPlotAnalysesS.RemoveAll();
        }

        [DirectMethod]
        public void ShowAnalysToPlotW()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRegion = new SqlDataAdapter(selectCommAnalysRegion, conn);
                adapterRegion.Fill(indexDS, "Region");
                conn.Close();
                indexDV = new System.Data.DataView(indexDS.Tables["Region"]);
                AnalysToPlotRegionS.DataSource = indexDV;
                AnalysToPlotRegionS.DataBind();

                ResetAnalysToPlot();
                AnalysToPlotW.Show();
            }
        }

        [DirectMethod]
        public void FillAnalysToPlotOrganization(String id_region)
        {
            if (connection_try && id_region != null && id_region != "" && id_region != String.Empty)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter(selectCommAnalysOrganization + " WHERE id_region = " + id_region, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                indexDV = new System.Data.DataView(indexDS.Tables["Organization"]);
                conn.Close();
                AnalysToPlotOrganizationS.DataSource = indexDV;
                AnalysToPlotOrganizationS.DataBind();
                AnalysToPlotOrganizationCB.Clear();
                AnalysToPlotDepartmentCB.Clear();
                AnalysToPlotTourCB.Clear();
                AnalysToPlotYearCB.Clear();
                AnalysToPlotPlotsS.RemoveAll();
                AnalysToPlotAnalysesS.RemoveAll();
            }
        }

        [DirectMethod]
        public void FillAnalysToPlotDepartment(String id_organization)
        {
            if (connection_try && id_organization != null && id_organization != "" && id_organization != String.Empty)
            {
                conn.Open();
                adapterDepartment = new SqlDataAdapter(selectCommAnalysDepartment + " WHERE id_organization = " + id_organization, conn);
                adapterDepartment.Fill(indexDS, "Department");
                indexDV = new System.Data.DataView(indexDS.Tables["Department"]);
                conn.Close();
                AnalysToPlotDepartmentS.DataSource = indexDV;
                AnalysToPlotDepartmentS.DataBind();
                AnalysToPlotDepartmentCB.Clear();
                AnalysToPlotTourCB.Clear();
                AnalysToPlotYearCB.Clear();
                AnalysToPlotPlotsS.RemoveAll();
                AnalysToPlotAnalysesS.RemoveAll();
            }
        }

        [DirectMethod]
        public void FillAnalysToPlotTour(String id_department)
        {
            if (connection_try && id_department != null && id_department != "" && id_department != String.Empty)
            {
                conn.Open();
                SqlDataAdapter adapterReportAnalysTour = new SqlDataAdapter(selectCommAnalysTours + " WHERE id_department=" + id_department, conn);
                adapterReportAnalysTour.Fill(indexDS, "Analys_tour");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_tour"]);
                conn.Close();
                AnalysToPlotTourS.DataSource = indexDV;
                AnalysToPlotTourS.DataBind();
                AnalysToPlotTourCB.Clear();
                AnalysToPlotYearCB.Clear();
                AnalysToPlotPlotsS.RemoveAll();
                AnalysToPlotAnalysesS.RemoveAll();
            }
        }

        [DirectMethod]
        public void FillAnalysToPlotYear(String tour, String id_department)
        {
            if (connection_try && tour != null && tour != "" && tour != String.Empty)
            {
                conn.Open();
                SqlDataAdapter adapterAnalysToPlotYear = new SqlDataAdapter(selectCommAnalysYears + " WHERE id_department = " + id_department + " AND tour = " + tour, conn);
                adapterAnalysToPlotYear.Fill(indexDS, "Analys_year");
                indexDV = new System.Data.DataView(indexDS.Tables["Analys_year"]);
                conn.Close();
                AnalysToPlotYearS.DataSource = indexDV;
                AnalysToPlotYearS.DataBind();
                AnalysToPlotYearCB.Clear();
                AnalysToPlotPlotsS.RemoveAll();
                AnalysToPlotAnalysesS.RemoveAll();
            }
        }

        [DirectMethod]
        public void FillAnalysToPlotData(String year, String id_department)
        {
            if (connection_try && year != null && year != "" && year != String.Empty)
            {
                conn.Open();
                adapterAnalysToPlotPlots = new SqlDataAdapter(selectCommAnalysToPlotPlots + " WHERE id_department = " + id_department + " AND year = " + year, conn);
                adapterAnalysToPlotPlots.Fill(indexDS, "AnalysToPlotPlots");
                adapterAnalysToPlotSamples = new SqlDataAdapter(selectCommAnalysToPlotSamples + " WHERE id_department = " + id_department + " AND year = " + year, conn);
                adapterAnalysToPlotSamples.Fill(indexDS, "AnalysToPlotSamples");
                conn.Close();
                AnalysToPlotPlotsS.DataSource = new System.Data.DataView(indexDS.Tables["AnalysToPlotPlots"]);
                AnalysToPlotAnalysesS.DataSource = new System.Data.DataView(indexDS.Tables["AnalysToPlotSamples"]);
                AnalysToPlotPlotsS.DataBind();
                AnalysToPlotAnalysesS.DataBind();
            }
        }

        [DirectMethod]
        public void AnalysToPlotSetNumberPlot(String number_plot, String analyses_data)
        {
            if (number_plot != null && number_plot != "" && number_plot != String.Empty)
            {
                List<Dictionary<String, String>> analyses_records = JSON.Deserialize<List<Dictionary<String, String>>>(analyses_data);
                if (analyses_records.Count > 0 && connection_try)
                {
                    String id_sample = String.Empty;
                    SqlCommand add_edit_sample_number_plot;
                    conn.Open();
                    for(int i = 0; i<analyses_records.Count; i++)
                    {
                        id_sample = analyses_records[i]["id_sample"];
                        add_edit_sample_number_plot = new SqlCommand("Add_Edit_Sample_Number_Plot", conn);
                        add_edit_sample_number_plot.CommandType = CommandType.StoredProcedure;
                        add_edit_sample_number_plot.Parameters.AddWithValue("@id_sample", Convert.ToInt32(id_sample));
                        add_edit_sample_number_plot.Parameters.AddWithValue("@number_plot", Convert.ToInt32(number_plot));
                        add_edit_sample_number_plot.ExecuteNonQuery();
                    }
                    conn.Close();
                    FillAnalysToPlotData(AnalysToPlotYearCB.Value.ToString(), AnalysToPlotDepartmentCB.Value.ToString());
                }
            }
        }

        [DirectMethod]
        public void AnalysToPlotUpdatePlot(String id_department, String year)
        {
            if (connection_try && year != null && year != "" && year != String.Empty)
            {
                conn.Open();
                SqlCommand analys_to_plot_update_plot = new SqlCommand("Update_Data_From_Analyses", conn);
                analys_to_plot_update_plot.CommandType = CommandType.StoredProcedure;
                analys_to_plot_update_plot.CommandTimeout = 10000;
                analys_to_plot_update_plot.Parameters.AddWithValue("@id_department", Convert.ToInt32(id_department));
                analys_to_plot_update_plot.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                analys_to_plot_update_plot.ExecuteNonQuery();
                conn.Close();
                AnalysToPlotW.Close();
            }
        }

        [DirectMethod]
        public void AnalysToPlotCancel()
        {
            AnalysToPlotW.Close();
        }

        [DirectMethod]
        public void ShowMap(String id_organization, String year, String tour)
        {
            if (Request.Browser.Cookies)
            {
                SetMapParam(id_organization, year, tour);
                HttpCookie cookie_for_map = Request.Cookies["Agrochim31_For_Map"];
                if (cookie_for_map != null)
                {
                    cookie_for_map["hidden"] = 0.ToString();
                    Response.Cookies.Set(cookie_for_map);
                }
                HttpCookie map_xy = Request.Cookies["Agrochim31_Map_XY"];
                if (map_xy != null)
                {
                    map_xy["X"] = 0.ToString();
                    map_xy["Y"] = 0.ToString();
                    Response.Cookies.Set(map_xy);
                    //Response.Cookies.Remove(map_xy.Name);
                }
                HttpCookie map_params = Request.Cookies["Agrochim31_Map_Params"];
                if (map_params != null)
                {
                    map_params["maptheme"] = "null";
                    map_params["showpoints"] = 0.ToString();
                    map_params["x"] = "0";
                    Response.Cookies.Set(map_params);
                }
            }
            /*List<object> Themes = new List<object>();
            Themes.Add(new { name_theme = "null", title_theme = "Пашня" });
            Themes.Add(new { name_theme = "p2o5", title_theme = "Подвижный фосфор" });
            Themes.Add(new { name_theme = "k2o", title_theme = "Подвижный калий" });
            Themes.Add(new { name_theme = "ph_s", title_theme = "Степень кислотности" });
            Themes.Add(new { name_theme = "humus", title_theme = "Органическое вещество" });
            Themes.Add(new { name_theme = "type_soil", title_theme = "Почвенная карта" });
            Themes.Add(new { name_theme = "erosion_soil", title_theme = "Карта эрозии" });
            Themes.Add(new { name_theme = "slope", title_theme = "Карта уклонов" });

            MapThemeS.DataSource = Themes;
            MapThemeS.DataBind();
            MapThemeCB.Select(0);
            AgrochemicalPointsCB.Checked = false;

            //13112015
            MapW.Show();
            MapW.Loader.LoadContent();*/
            //Response.Redirect("OLGISMap.aspx");
        }

        [DirectMethod]
        public void SetMapParam(String id_organization, String year, String tour)
        {
            if (Request.Browser.Cookies && id_organization != null && id_organization != "null" && id_organization != String.Empty && year != null && year != "null" && year != String.Empty)
            {
                HttpCookie cookie_for_map = Request.Cookies["Agrochim31_For_Map"];
                if (cookie_for_map == null)
                {
                    cookie_for_map = new HttpCookie("Agrochim31_For_Map");
                    Response.Cookies.Add(cookie_for_map);
                }
                cookie_for_map["id_organization"] = id_organization;
                cookie_for_map["year"] = year;
                cookie_for_map["tour"] = tour;
                cookie_for_map["code_plot"] = String.Empty;
                cookie_for_map.Expires.AddHours(1);
                Response.Cookies.Set(cookie_for_map);
            }
        }

        [DirectMethod]
        public void ShowPlotOnMap(String year, String tour, String code_plot)
        {
            if (Request.Browser.Cookies && code_plot != null && code_plot != "null" && code_plot != String.Empty && year != null && year != "null" && year != String.Empty)
            {
                HttpCookie cookie_for_map = Request.Cookies["Agrochim31_For_Map"];
                if (cookie_for_map == null)
                {
                    cookie_for_map = new HttpCookie("Agrochim31_For_Map");
                    Response.Cookies.Add(cookie_for_map);
                }
                cookie_for_map["id_organization"] = current_id_organization;
                cookie_for_map["year"] = year;
                cookie_for_map["tour"] = tour;
                cookie_for_map["code_plot"] = code_plot;
                cookie_for_map.Expires.AddHours(1);
                Response.Cookies.Set(cookie_for_map);
                HttpCookie map_params = Request.Cookies["Agrochim31_Map_Params"];
                if (map_params != null)
                {
                    map_params["maptheme"] = "null";
                    map_params["showpoints"] = 0.ToString();
                    map_params["x"] = "0";
                    Response.Cookies.Set(map_params);
                }
            }
            /*List<object> Themes = new List<object>();
            Themes.Add(new { name_theme = "null", title_theme = "Не выбрана" });
            Themes.Add(new { name_theme = "p2o5", title_theme = "Подвижный фосфор" });
            Themes.Add(new { name_theme = "k2o", title_theme = "Подвижный калий" });
            Themes.Add(new { name_theme = "ph_s", title_theme = "Степень кислотности" });
            Themes.Add(new { name_theme = "humus", title_theme = "Органическое вещество" });
            Themes.Add(new { name_theme = "type_soil", title_theme = "Почвенная карта" });
            Themes.Add(new { name_theme = "erosion_soil", title_theme = "Карта эрозии" });
            Themes.Add(new { name_theme = "slope", title_theme = "Карта уклонов" });

            MapW.Show();
            MapW.Loader.LoadContent();*/
            Response.Redirect("OLGISMap.aspx");
        }

        [DirectMethod]
        public void ShowRegionsYearForReportW(String type_report)
        {
            if (connection_try)
            {
                conn.Open();
                //очистка записей таблицы
                SqlCommand clear_regions_year = new SqlCommand("Clear_RegionsYear", conn);
                clear_regions_year.ExecuteNonQuery();
                //заполнение таблицы
                adapterRegionsYearForReport = new SqlDataAdapter(selectCommRegionsYearForReport, conn);
                adapterRegionsYearForReport.Fill(indexDS, "RegionsYearForReport");
                indexDV = new System.Data.DataView(indexDS.Tables["RegionsYearForReport"]);
                RegionsYearForReportS.DataSource = indexDV;
                RegionsYearForReportS.DataBind();
                conn.Close();
            }
            if (Convert.ToInt32(type_report) == 0)
            {
                GetRegionsYearForReportB.Hidden = false;
            }

            RegionsYearForReportW.Show();
        }

        [DirectMethod]
        public void CancelRegionsYearForReportW()
        {
            RegionsYearForReportW.Close();
        }

        [DirectMethod]
        public void BindRegionsYear(String id_region)
        {
            if (connection_try)
            {
                conn.Open();
                selectReportYears = "SELECT DISTINCT year FROM View_Plots_Tree WHERE id_region = " + id_region;
                adapterRegionsYear = new SqlDataAdapter(selectReportYears, conn);
                adapterRegionsYear.Fill(indexDS, "RegionsYear");
                indexDV = new System.Data.DataView(indexDS.Tables["RegionsYear"]);
                EditRegionsYearForReportYearS.DataSource = indexDV;
                EditRegionsYearForReportYearS.DataBind();
                EditRegionsYearForReportYearS.Sort("year", Ext.Net.SortDirection.ASC);
                conn.Close();
            }
        }

        [DirectMethod]
        public void GetRegionsYearForReport(String type_report, String regions_year_data)
        {
            List<Dictionary<String, String>> records = JSON.Deserialize<List<Dictionary<String, String>>>(regions_year_data);
            if (records.Count > 0 && connection_try)
            {
                SqlCommand add_edit_regions_year;
                conn.Open();
                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i]["year"] != null)
                    {
                        /*X.Msg.Show(new MessageBoxConfig
                        {
                            Title = "Ошибка cookies",
                            Message = (records[i]["tour"] + " : " + records[i]["id_region"]),
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.INFO
                        });*/
                        String id_region = records[i]["id_region"];
                        String year = records[i]["year"];
                        add_edit_regions_year = new SqlCommand("Add_Edit_RegionsYear", conn);
                        add_edit_regions_year.CommandType = CommandType.StoredProcedure;
                        add_edit_regions_year.Parameters.AddWithValue("@id_region", Convert.ToInt32(id_region));
                        add_edit_regions_year.Parameters.AddWithValue("@year", Convert.ToInt32(year));
                        add_edit_regions_year.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
            CancelRegionsYearForReportW();
            win_7_3.Reload();
            win_7_3.Show();
        }

        [DirectMethod]
        public void ShowImpornW()
        {
            win_upload.Resizable = false;
            win_upload.Reload();
            win_upload.Show();
        }

        // выполнение хранимых процедур (добавление карт, точек)
        [DirectMethod]
        public void UpdateIdPlotForPoints()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand update_id_plot_for_points = new SqlCommand("Update_IdPlot_For_Points", conn);
                update_id_plot_for_points.CommandType = CommandType.StoredProcedure;
                update_id_plot_for_points.CommandTimeout = 10000;
                update_id_plot_for_points.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление а/х точек завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetPointsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_points_geometry = new SqlCommand("SetPointsGeometry", conn);
                set_points_geometry.CommandType = CommandType.StoredProcedure;
                set_points_geometry.CommandTimeout = 10000;
                set_points_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление почвенных точек завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetPlotsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_plots_geometry = new SqlCommand("SetPlotsGeometry", conn);
                set_plots_geometry.CommandType = CommandType.StoredProcedure;
                set_plots_geometry.CommandTimeout = 10000;
                set_plots_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление участков завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetSoilGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_soil_geometry = new SqlCommand("SetSoilGeometry", conn);
                set_soil_geometry.CommandType = CommandType.StoredProcedure;
                set_soil_geometry.CommandTimeout = 10000;
                set_soil_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление почвенных карт завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetSlopeGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_slope_geometry = new SqlCommand("SetSlopeGeometry", conn);
                set_slope_geometry.CommandType = CommandType.StoredProcedure;
                set_slope_geometry.CommandTimeout = 10000;
                set_slope_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление карт уклонов завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetExposureGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_exposure_geometry = new SqlCommand("SetExposureGeometry", conn);
                set_exposure_geometry.CommandType = CommandType.StoredProcedure;
                set_exposure_geometry.CommandTimeout = 10000;
                set_exposure_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление карт экспозиции.").Show();
            }
        }
        
        [DirectMethod]
        public void SetErosionGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_erosion_geometry = new SqlCommand("SetErosionGeometry", conn);
                set_erosion_geometry.CommandType = CommandType.StoredProcedure;
                set_erosion_geometry.CommandTimeout = 10000;
                set_erosion_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление карт эрозии завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetTypingGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_typing_geometry = new SqlCommand("SetTypingGeometry", conn);
                set_typing_geometry.CommandType = CommandType.StoredProcedure;
                set_typing_geometry.CommandTimeout = 10000;
                set_typing_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление карт типизации завершено.").Show();
            }
        }
        
        [DirectMethod]
        public void SetALSZGeometry()
        {
            if (connection_try)
            {
                conn.Open();

                SqlCommand set_project_plots = new SqlCommand("SetProjectPlotsGeometry", conn);
                set_project_plots.CommandType = CommandType.StoredProcedure;
                set_project_plots.CommandTimeout = 10000;
                set_project_plots.ExecuteNonQuery();

                SqlCommand set_grassing = new SqlCommand("SetGrassingGeometry", conn);
                set_grassing.CommandType = CommandType.StoredProcedure;
                set_grassing.CommandTimeout = 10000;
                set_grassing.ExecuteNonQuery();

                SqlCommand set_water_objects = new SqlCommand("SetWaterObjectsGeometry", conn);
                set_water_objects.CommandType = CommandType.StoredProcedure;
                set_water_objects.CommandTimeout = 10000;
                set_water_objects.ExecuteNonQuery();

                SqlCommand set_woodland_belt = new SqlCommand("SetWoodlandBeltsGeometry", conn);
                set_woodland_belt.CommandType = CommandType.StoredProcedure;
                set_woodland_belt.CommandTimeout = 10000;
                set_woodland_belt.ExecuteNonQuery();

                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление проекта АЛСЗ завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetTerritoryGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_territory_geometry = new SqlCommand("SetTerritoryGeometry", conn);
                set_territory_geometry.CommandType = CommandType.StoredProcedure;
                set_territory_geometry.CommandTimeout = 10000;
                set_territory_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление областей завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetRegionsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_regions_geometry = new SqlCommand("SetRegionGeometry", conn);
                set_regions_geometry.CommandType = CommandType.StoredProcedure;
                set_regions_geometry.CommandTimeout = 10000;
                set_regions_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление районов завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetOrganizationsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_organizations_geometry = new SqlCommand("SetOrganizationGeometry", conn);
                set_organizations_geometry.CommandType = CommandType.StoredProcedure;
                set_organizations_geometry.CommandTimeout = 10000;
                set_organizations_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление организаций завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetFarmsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_farms_geometry = new SqlCommand("SetFarmGeometry", conn);
                set_farms_geometry.CommandType = CommandType.StoredProcedure;
                set_farms_geometry.CommandTimeout = 10000;
                set_farms_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление животноводческих комплексов завершено.").Show();
            }
        }

        [DirectMethod]
        public void SetLagoonsGeometry()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand set_lagoons_geometry = new SqlCommand("SetLagoonGeometry", conn);
                set_lagoons_geometry.CommandType = CommandType.StoredProcedure;
                set_lagoons_geometry.CommandTimeout = 10000;
                set_lagoons_geometry.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление/обновление лагун/площадок/боксов завершено.").Show();
            }
        }

        [DirectMethod]
        public void ReloadMap()
        {
            MapW.Loader.LoadContent();
        }

        [DirectMethod]
        public void CloseMap()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_for_map = Request.Cookies["Agrochim31_For_Map"];
                if (cookie_for_map != null)
                {
                    cookie_for_map["hidden"] = 1.ToString();
                    Response.Cookies.Set(cookie_for_map);
                }
            }
        }

        [DirectMethod]
        public void Import_Soil_Samples()
        {
            if (connection_try)
            {
                conn.Open();
                SqlCommand import_soil_samples = new SqlCommand("Import_Soil_Samples", conn);
                import_soil_samples.CommandType = CommandType.StoredProcedure;
                import_soil_samples.CommandTimeout = 10000;
                import_soil_samples.ExecuteNonQuery();
                conn.Close();
                X.Msg.Notify("Внимание!", "Добавление данных химического анализа образцов почвы завершено.").Show();
            }
        }

        [DirectMethod]
        public void SelectedRowInAboutOrg(String id_organization, String year, String tour)
        {
            if (Request.Browser.Cookies)
            {
                SetMapParam(id_organization, year, tour);
                CenterMap();
            }
        }

        [DirectMethod]
        public void ShowStatisticsAreaRegionW()
        {
            FillStatisticsAreaRegion();
            StatisticsAreaRegionW.Show();
        }

        [DirectMethod]
        public void FillStatisticsAreaRegion()
        {
            if (connection_try)
            {
                conn.Open();
                adapterStatisticsAreaRegion = new SqlDataAdapter(selectCommStatisticsAreaRegion, conn);
                adapterStatisticsAreaRegion.Fill(indexDS, "StatisticsAreaRegion");
                indexDV = new System.Data.DataView(indexDS.Tables["StatisticsAreaRegion"]);
                StatisticsAreaRegionS.DataSource = indexDV;
                StatisticsAreaRegionS.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void CenterMap()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie map_params = Request.Cookies["Agrochim31_Map_Params"];
                if (map_params == null)
                {
                    map_params = new HttpCookie("Agrochim31_Map_Params");
                    Response.Cookies.Add(map_params);
                }
                map_params["x"] = 0.ToString();
                map_params["y"] = 0.ToString();
                map_params["zoom"] = 0.ToString();
                Response.Cookies.Set(map_params);

                HttpCookie cookie_for_map = Request.Cookies["Agrochim31_For_Map"];
                if (cookie_for_map == null)
                {
                    cookie_for_map = new HttpCookie("Agrochim31_For_Map");
                    cookie_for_map["hidden"] = 1.ToString();
                    Response.Cookies.Add(cookie_for_map);
                }
                if (cookie_for_map["hidden"] == 0.ToString())
                {
                    MapW.Loader.LoadContent();
                }
            }
        }
    }
}