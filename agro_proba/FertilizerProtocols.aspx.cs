using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using Ext.Net;

namespace agro_proba
{
    public partial class FertilizerProtocols : System.Web.UI.Page
    {
        public System.Data.DataView indexDV;
        public DataSet indexDS;
        public SqlConnection conn;
        public String connStr;
        public Boolean connection_try;
        public HttpCookie cookie, cookie_login_user;
        public Int32 login_form_show;
        public String current_id_farm_org, current_id_farm, current_id_lagoon, current_id_protocol, current_selected_farm_org, current_selected_farm,
                      current_id_result, current_selected_lagoon, current_selected_protocol, current_selected_result, current_id_region, current_password;
        public String selectCommFarmOrgs, selectCommFarms, selectCommLagoons, selectCommProtocols, selectCommRegions, selectCommTypeFarm, selectCommOrgFertilizer,
                      selectCommSignFert, selectCommUnitsSF, selectCommDocuments, selectCommTypeLagoon;
        public login_data user_reg_data;
        public SqlDataAdapter adapterFertilizer, adapterSignFert, adapterUnitsSF, adapterDocuments, adapterProtocolInfo, adapterProtocolResults;
        public Window FertilizerProtocolW, ReportProtocolsW, ResultsStatisticsW;

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
        protected void Page_Load(object sender, EventArgs e)
        {
            indexDS = new DataSet();
            //connStr = "Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N";
            connStr = SetConnectionString();
            conn = new SqlConnection(connStr);
            connection_try = TryConnection(connStr);

            if (connection_try)
            {
                if (Request.Browser.Cookies)
                {
                    cookie = Request.Cookies["Agrochim31"];
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("Agrochim31");
                        cookie["current_id_region"] = "0";
                        cookie["current_id_farm_org"] = "0";
                        cookie["current_id_farm"] = "0";
                        cookie["current_id_lagoon"] = "0";
                        cookie["current_id_protocol"] = "0";
                        cookie["current_selected_farm_org"] = "0";
                        cookie["current_selected_farm"] = "0";
                        cookie["current_selected_lagoon"] = "0";
                        cookie["current_selected_protocol"] = "0";
                        cookie["current_password"] = "0";
                        cookie["login_form_show"] = "1";
                        cookie.Expires = DateTime.Now.AddMonths(24);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        current_id_region = cookie["current_id_region"];
                        current_id_farm_org = cookie["current_id_farm_org"];
                        current_id_farm = cookie["current_id_farm"];
                        current_id_lagoon = cookie["current_id_lagoon"];
                        current_id_protocol = cookie["current_id_protocol"];
                        current_selected_farm_org = cookie["current_selected_farm_org"];
                        current_selected_farm = cookie["current_selected_farm"];
                        current_selected_lagoon = cookie["current_selected_lagoon"];
                        current_selected_protocol = cookie["current_selected_protocol"];
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

                //заполнение данными
                selectCommFarmOrgs = "SELECT * FROM Farm_organization";
                selectCommFarms = "SELECT * FROM View_Farms";
                selectCommLagoons = "SELECT * FROM View_Lagoons";
                selectCommTypeLagoon = "SELECT * FROM Type_lagoon";

                selectCommProtocols = "SELECT * FROM View_Protocols";
                selectCommRegions = "SELECT * FROM Region WHERE id_territory = 31";
                selectCommTypeFarm = "SELECT * FROM Type_farm";
                selectCommOrgFertilizer = "SELECT * FROM Fertilizer WHERE id_kind_fertilizer = 2";

                selectCommSignFert = "SELECT * FROM Significative_Fertilizer";
                selectCommUnitsSF = "SELECT * FROM Units_Significative_Fertilizer";
                selectCommDocuments = "SELECT * FROM Normative_documents";

                //выполнение при первой загрузке
                if (!IsPostBack)
                {
                    FillFarmOrgs();
                    FillRegions();
                }

                FillFertilizer();
                FillSignFert();
                FillUnitsSF();
                FillDocuments();
            }

            //авторизация
            AcceptLoginB.Listeners.Click.Handler = "App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());";
            UsernameTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
            UserPassword.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";

            FarmOrganizationGP.Listeners.ViewReady.Handler = "App.direct.SelectFarmOrg();";
            FarmOrganizationGP.Listeners.Select.Handler = "App.direct.SelectedFarmOrg(record.data.id_farm_organization, record.data.title_farm_organization, this.getStore().indexOf(record));";
            //FarmOrganizationGP.Listeners.CellClick.Handler = "var record = #{FarmOrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.SelectedFarmOrg(record.data.id_farm_organization, record.data.title_farm_organization, this.getStore().indexOf(record));";
            RegionCB.Listeners.Change.Handler = "App.direct.FilterFarm(this.value);";
            ResetRegionB.Listeners.Click.Handler = "#{RegionCB}.clear(); App.direct.FilterFarm(0);";
            FarmGP.Listeners.Select.Handler = "App.direct.SelectedFarm(record.data.id_farm, record.data.title_farm, this.getStore().indexOf(record));";
            //FarmGP.Listeners.CellClick.Handler = "var record = #{FarmGP}.getView().getSelectionModel().getSelection()[0]; App.direct.SelectedFarm(record.data.id_farm, record.data.title_farm, this.getStore().indexOf(record));";
            LagoonsGP.Listeners.Select.Handler = "App.direct.SelectedLagoon(record.data.id_lagoon, this.getStore().indexOf(record));";
            //LagoonsGP.Listeners.CellClick.Handler = "var record = #{LagoonsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.SelectedLagoon(record.data.id_lagoon, this.getStore().indexOf(record));";
            ProtocolsGP.Listeners.Select.Handler = "App.direct.SelectedProtocol(record.data.id_protocol, this.getStore().indexOf(record));";
            //ProtocolsGP.Listeners.CellClick.Handler = "var record = #{ProtocolsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.SelectedProtocol(record.data.id_protocol, this.getStore().indexOf(record));";

            //обработка организаций
            AddFarmOrganizationB.Listeners.Click.Handler = "var store = #{FarmOrganizationGP}.store; store.insert(0, {id_farm_organization : '0'}); #{FarmOrganizationGP}.editingPlugin.startEdit(0, 0);";
            FarmOrganizationS.Listeners.Update.Handler = "App.direct.AddEditFarmOrganization(record.data.id_farm_organization, record.data.title_farm_organization, record.data.address);";
            DeleteFarmOrganizationB.Listeners.Click.Handler = "var record = #{FarmOrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalFarmOrganization(record.data.id_farm_organization);";
            Farm_org_RE.Listeners.CancelEdit.Handler = "App.direct.FillFarmOrgs();";

            //обработка площадок
            AddFarmB.Listeners.Click.Handler = "var store = #{FarmGP}.store; store.insert(0, {id_farm : '0'}); #{FarmGP}.editingPlugin.startEdit(0, 0);";
            FarmS.Listeners.Update.Handler = "App.direct.AddEditFarm(record.data.id_farm, record.data.title_region, record.data.title_type_farm, " +
                                             "record.data.number_farm, record.data.title_farm, record.data.location_farm, record.data.animal_population);";
            DeleteFarmB.Listeners.Click.Handler = "var record = #{FarmGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalFarm(record.data.id_farm);";
            Farm_RE.Listeners.CancelEdit.Handler = "var record = #{FarmOrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.FillFarms(record.data.id_farm_organization);";
            FarmGP.Listeners.BeforeEdit.Handler = "App.direct.FillRegionsFromFarm(); App.direct.FillTypeFarm();";

            //обработка лагун
            AddLagoonB.Listeners.Click.Handler = "var store = #{LagoonsGP}.store; store.insert(0, {id_lagoon : '0'}); #{LagoonsGP}.editingPlugin.startEdit(0, 0);";
            LagoonsS.Listeners.Update.Handler = "App.direct.AddEditLagoon(record.data.id_lagoon, record.data.title_type_lagoon, record.data.lagoon_number, record.data.lagoon_name, record.data.lagoon_volume);";
            DeleteLagoonB.Listeners.Click.Handler = "var record = #{LagoonsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalLagoon(record.data.id_lagoon);";
            Lagoon_RE.Listeners.CancelEdit.Handler = "var record = #{FarmGP}.getView().getSelectionModel().getSelection()[0]; App.direct.FillLagoons(record.data.id_farm);";
            LagoonsGP.Listeners.BeforeEdit.Handler = "var record = this.getView().getSelectionModel().getSelection()[0]; App.direct.BindTypeLagoons();";

            //печать протоколов
            PrintProtocolB.Listeners.Click.Handler = "App.direct.PrintFertilizerProtocol();";

            //выход пользователя
            ExitB.Listeners.Click.Handler = "App.direct.ExitUser();";

            //Отчёты
            ReportProtocolsFertilizerB.Listeners.Click.Handler = "App.direct.ShowReportProtocolsW(0);";
            ReportResultsProtocolsStatisticsB.Listeners.Click.Handler = "App.direct.ShowReportProtocolsW(1);";
            ResetReportProtocolsB.Listeners.Click.Handler = "App.direct.ResetReportProtocols();";
            CancelReportProtocolsB.Listeners.Click.Handler = "App.direct.CancelReportProtocols();";
            AcceptReportProtocolsB.Listeners.Click.Handler = "App.direct.AcceptReportProtocols();";
            ReportProtocolsFarmOrgCB.Listeners.Select.Handler = "App.direct.SelectReportFarm(#{ReportProtocolsFarmOrgCB}.getValue(), #{ReportProtocolsRegionCB}.getValue());";
            ReportProtocolsRegionCB.Listeners.Select.Handler = "App.direct.SelectReportFarm(#{ReportProtocolsFarmOrgCB}.getValue(), #{ReportProtocolsRegionCB}.getValue());";

            //работа с протоколами
            AddProtocolB.Listeners.Click.Handler = "App.direct.ProtocolEditWindowAdd();";
            EditProtocolB.Listeners.Click.Handler = "var record = #{ProtocolsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ProtocolEditWindowEdit(record.data.id_protocol);";
            ProtocolsGP.Listeners.CellDblClick.Handler = "App.direct.ProtocolEditWindowEdit(record.data.id_protocol);";
            CopyProtocolB.Listeners.Click.Handler = "var record = #{ProtocolsGP}.getView().getSelectionModel().getSelection()[0]; #{CopyIdProtocolTF}.setValue(record.data.id_protocol); #{PasteProtocolB}.show();";
            PasteProtocolB.Listeners.Click.Handler = "var record = #{LagoonsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.CopyFertilizerProtocol(record.data.id_lagoon, #{CopyIdProtocolTF}.getValue()); #{PasteProtocolB}.hide();";
            DeleteProtocolB.Listeners.Click.Handler = "var record = #{ProtocolsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalProtocol(record.data.id_protocol);";
            ProtocolEditAddResultB.Listeners.Click.Handler = "var store = #{ProtocolEditResultsGP}.store; store.insert(0, {id_result : '0'}); #{ProtocolEditResultsGP}.editingPlugin.startEdit(0, 0);";
            //ProtocolEditGridS.Listeners.Update.Handler = "";
            ProtocolEditDeleteResultB.Listeners.Click.Handler = "var record = #{ProtocolEditResultsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.WindowRemovalResult(record.data.id_result);";
            ProtocolEditAddB.Listeners.Click.Handler = "var record = #{LagoonsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.AddProtocol(#{ProtocolEditTF}.getValue(), #{ProtocolEditFertilizerCB}.getValue(), #{ProtocolEditOrganizationTF}.getValue(), record.data.id_lagoon, #{ProtocolEditSizeNF}.getValue(), #{ProtocolEditSizeUnitTF}.getValue(), #{ProtocolEditValueNF}.getValue(), #{ProtocolEditValueUnitTF}.getValue(), #{ProtocolEditDeviationNF}.getValue(), #{ProtocolEditDeviationUnitTF}.getValue(), #{ProtocolEditOrganizationProbeTF}.getValue(), #{ProtocolEditDocumentTF}.getValue(), #{ProtocolEditSelectingDateDF}.getValue(), #{ProtocolEditReceptionDateDF}.getValue(), #{ProtocolEditTestingTimeBeginDF}.getValue(), #{ProtocolEditTestingTimeEndDF}.getValue(), #{ProtocolEditSelectingDocumentTF}.getValue(), #{ProtocolEditTestingDocumentTF}.getValue(), #{ProtocolEditCommentTA}.getValue(), #{ProtocolEditHardenerPositionTF}.getValue(), #{ProtocolEditHardenerTF}.getValue(), #{AdditionalTF}.getValue(), #{ProtocolEditResultsGP}.getRowsValues());";
            ProtocolEditSaveB.Listeners.Click.Handler = "var record = #{ProtocolsGP}.getView().getSelectionModel().getSelection()[0]; var record_lagoon = #{LagoonsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.EditProtocol(record.data.id_protocol, #{ProtocolEditTF}.getValue(), #{ProtocolEditFertilizerCB}.getValue(), #{ProtocolEditOrganizationTF}.getValue(), record_lagoon.data.id_lagoon, #{ProtocolEditSizeNF}.getValue(), #{ProtocolEditSizeUnitTF}.getValue(), #{ProtocolEditValueNF}.getValue(), #{ProtocolEditValueUnitTF}.getValue(), #{ProtocolEditDeviationNF}.getValue(), #{ProtocolEditDeviationUnitTF}.getValue(), #{ProtocolEditOrganizationProbeTF}.getValue(), #{ProtocolEditDocumentTF}.getValue(), #{ProtocolEditSelectingDateDF}.getValue(), #{ProtocolEditReceptionDateDF}.getValue(), #{ProtocolEditTestingTimeBeginDF}.getValue(), #{ProtocolEditTestingTimeEndDF}.getValue(), #{ProtocolEditSelectingDocumentTF}.getValue(), #{ProtocolEditTestingDocumentTF}.getValue(), #{ProtocolEditCommentTA}.getValue(), #{ProtocolEditHardenerPositionTF}.getValue(), #{ProtocolEditHardenerTF}.getValue(), #{AdditionalTF}.getValue(), #{ProtocolEditResultsGP}.getRowsValues());";
            ProtocolEditCancelB.Listeners.Click.Handler = "#{ProtocolEditW}.close();";

            FertilizerProtocolW = new Window()
            {
                ID = "FertilizerProtocolW",
                Title = "Протокол",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportFertilizerProtocol.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(FertilizerProtocolW);

            ReportProtocolsW = new Window()
            {
                ID = "ReportFertilizerProtocolsW",
                Title = "Протоколы",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportFertilizerProtocols.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(ReportProtocolsW);

            ResultsStatisticsW = new Window()
            {
                ID = "ReportResultsProtocolsStatisticsW",
                Title = "Статистика",
                Width = Unit.Pixel(1000),
                Height = Unit.Pixel(700),
                Modal = false,
                AutoRender = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true,
                Loader = new ComponentLoader
                {
                    Url = "ReportResultsProtocolsStatistics.aspx",
                    Mode = LoadMode.Frame,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            this.Form.Controls.Add(ResultsStatisticsW);
        }

        [DirectMethod]
        public void FillFarmOrgs()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterFarmOrgs = new SqlDataAdapter(selectCommFarmOrgs, conn);
                adapterFarmOrgs.Fill(indexDS, "FarmOrgs");
                indexDV = new System.Data.DataView(indexDS.Tables["FarmOrgs"]);
                FarmOrganizationS.DataSource = indexDV;
                FarmOrganizationS.DataBind();
                conn.Close();
                FarmOrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_farm_org));
            }
        }

        public void FillRegions()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterRegions = new SqlDataAdapter(selectCommRegions, conn);
                adapterRegions.Fill(indexDS, "Regions");
                indexDV = new System.Data.DataView(indexDS.Tables["Regions"]);
                RegionS.DataSource = indexDV;
                RegionS.DataBind();
                conn.Close();
                //RegionCB.Select(Convert.ToInt32(NotNull(current_id_region)));
            }
        }

        [DirectMethod]
        public void FillFarms(String id_farm_org)
        {
            LagoonsS.RemoveAll();
            ProtocolsS.RemoveAll();
            if (connection_try)
            {
                String selectComm = selectCommFarms + " WHERE id_farm_organization=" + id_farm_org;
                if (RegionCB.Value.ToString() != "" && NotNull(current_id_region) != "0")
                {
                    /*FarmS.ClearFilter();
                    FarmS.Filter("id_region", current_id_region);*/
                    selectComm += (" AND id_region=" + NotNull(current_id_region));
                }

                conn.Open();
                SqlDataAdapter adapterFarms = new SqlDataAdapter(selectComm, conn);
                adapterFarms.Fill(indexDS, "Farms");
                indexDV = new System.Data.DataView(indexDS.Tables["Farms"]);
                FarmS.DataSource = indexDV;
                FarmS.DataBind();
                FarmS.Sort("title_farm", Ext.Net.SortDirection.Default);
                conn.Close();
                FarmGP.GetSelectionModel().Select(Convert.ToInt32(NotNull(current_selected_farm)));
            }
        }

        [DirectMethod]
        public void FillLagoons(String id_farm)
        {
            ProtocolsS.RemoveAll();
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterLagoons = new SqlDataAdapter(selectCommLagoons + " WHERE id_farm=" + NotNull(id_farm), conn);
                adapterLagoons.Fill(indexDS, "Lagoons");
                indexDV = new System.Data.DataView(indexDS.Tables["Lagoons"]);
                LagoonsS.DataSource = indexDV;
                LagoonsS.DataBind();
                LagoonsS.Sort("lagoon_number", Ext.Net.SortDirection.Default);
                conn.Close();
                LagoonsGP.GetSelectionModel().Select(Convert.ToInt32(NotNull(current_selected_lagoon)));
            }
        }

        [DirectMethod]
        public void FillProtocols(String id_lagoon)
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterProtocols = new SqlDataAdapter(selectCommProtocols + " WHERE id_lagoon=" + NotNull(id_lagoon), conn);
                adapterProtocols.Fill(indexDS, "Protocols");
                indexDV = new System.Data.DataView(indexDS.Tables["Protocols"]);
                ProtocolsS.DataSource = indexDV;
                ProtocolsS.DataBind();
                ProtocolsS.Sort("number_protocol", Ext.Net.SortDirection.Default);
                conn.Close();
                ProtocolsGP.GetSelectionModel().Select(Convert.ToInt32(NotNull(current_selected_protocol)));
            }
        }

        [DirectMethod]
        public void SelectFarmOrg()
        {
            if (connection_try)
            {
                FarmOrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_farm_org));
            }
        }

        [DirectMethod]
        public void SelectedFarmOrg(String id_farm_organization, String title_farm_organization, String record_id)
        {
            if (connection_try)
            {
                String old_farm_org = String.Empty;
                if (Request.Browser.Cookies)
                {
                    try
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        old_farm_org = cookie["current_id_farm_org"].ToString();
                        cookie["current_id_farm_org"] = id_farm_organization;
                        cookie["current_selected_farm_org"] = record_id;
                        Response.Cookies.Set(cookie);
                    }
                    catch { }
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
                current_id_farm_org = id_farm_organization;
                current_selected_farm_org = record_id;

                if (String.Compare(old_farm_org, id_farm_organization) != 0)
                {
                    //FarmGP.GetSelectionModel().Select(0);
                    current_id_farm = "0";
                    current_selected_farm = "0";
                }
                FillFarms(current_id_farm_org);
            }
        }

        [DirectMethod]
        public void FilterFarm(String id_region)
        {
            /*FarmS.ClearFilter();
            FarmS.Filter("id_region", id_region);*/
            current_id_region = id_region;
            if (Request.Browser.Cookies)
            {
                try
                {
                    cookie = Request.Cookies["Agrochim31"];
                    cookie["current_id_region"] = current_id_region;
                    Response.Cookies.Set(cookie);
                }
                catch { }
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
            FillFarms(current_id_farm_org);
        }

        [DirectMethod]
        public void SelectedFarm(String id_farm, String title_farm, String record_id)
        {
            if (connection_try)
            {
                String old_farm = String.Empty;
                if (Request.Browser.Cookies)
                {
                    try
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        old_farm = cookie["current_id_farm"].ToString();
                        cookie["current_id_farm"] = id_farm;
                        cookie["current_selected_farm"] = record_id;
                        Response.Cookies.Set(cookie);
                    }
                    catch { }
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
                current_id_farm = id_farm;
                current_selected_farm = record_id;

                if (String.Compare(old_farm, id_farm) != 0)
                {
                    //LagoonsGP.GetSelectionModel().Select(0);
                    current_id_lagoon = "0";
                    current_selected_lagoon = "0";
                }
                FillLagoons(current_id_farm);
            }
        }

        [DirectMethod]
        public void SelectedLagoon(String id_lagoon, String record_id)
        {
            if (connection_try)
            {
                String old_lagoon = String.Empty;
                if (Request.Browser.Cookies)
                {
                    try
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        old_lagoon = cookie["current_id_lagoon"].ToString();
                        cookie["current_id_lagoon"] = id_lagoon;
                        cookie["current_selected_lagoon"] = record_id;
                        Response.Cookies.Set(cookie);
                    }
                    catch { }
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
                current_id_lagoon = id_lagoon;
                current_selected_lagoon = record_id;

                if (String.Compare(old_lagoon, id_lagoon) != 0)
                {
                    //ProtocolsGP.GetSelectionModel().Select(0);
                    current_id_protocol = "0";
                    current_selected_protocol = "0";
                }
                FillProtocols(current_id_lagoon);
            }
        }

        [DirectMethod]
        public void SelectedProtocol(String id_protocol, String record_id)
        {
            if (connection_try)
            {
                String old_protocol = String.Empty;
                if (Request.Browser.Cookies)
                {
                    try
                    {
                        cookie = Request.Cookies["Agrochim31"];
                        old_protocol = cookie["current_id_protocol"].ToString();
                        cookie["current_id_protocol"] = id_protocol;
                        cookie["current_selected_protocol"] = record_id;
                        Response.Cookies.Set(cookie);
                    }
                    catch { }
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
                current_id_protocol = id_protocol;
                current_selected_protocol = record_id;
            }
        }

        [DirectMethod]
        public void PrintFertilizerProtocol()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie cookie_report_plan = Request.Cookies["Agrochim31_Report_Protocol"];
                if (cookie_report_plan == null)
                {
                    cookie_report_plan = new HttpCookie("Agrochim31_Report_Protocol");
                    cookie_report_plan["id"] = current_id_protocol;
                    Response.Cookies.Add(cookie_report_plan);
                }
                else
                {
                    cookie_report_plan["id"] = current_id_protocol;
                    Response.Cookies.Set(cookie_report_plan);
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
            FertilizerProtocolW.Reload();
            FertilizerProtocolW.Show();
        }

        [DirectMethod]
        public void RemoveNo()
        {
            IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Удаление отменено!", IconCls = "icon-cancel", Clear2 = true });
        }

        [DirectMethod]
        public void AddEditFarmOrganization(String id_farm_organization, String title_farm_organization, String address)
        {
            if (connection_try)
            {
                if (NotNull(id_farm_organization) == "0") { id_farm_organization = "0"; }
                if (NotNullText(title_farm_organization) != "0")
                {
                    conn.Open();
                    SqlCommand add_edit_farm_organization = new SqlCommand("Add_Edit_Farm_Organization", conn);
                    add_edit_farm_organization.CommandType = CommandType.StoredProcedure;
                    add_edit_farm_organization.Parameters.AddWithValue("@id_farm_organization", Convert.ToInt32(NotNull(id_farm_organization)));
                    add_edit_farm_organization.Parameters.AddWithValue("@title_farm_organization", title_farm_organization);
                    add_edit_farm_organization.Parameters.AddWithValue("@address", NotNullText(address));
                    add_edit_farm_organization.ExecuteNonQuery();
                    conn.Close();
                    FillFarmOrgs();
                }
            }
        }

        [DirectMethod]
        public void WindowRemovalFarmOrganization(String id_farm_organization)
        {
            if (connection_try)
            {
                X.Msg.Confirm("Удаление записи", "Удалить выбранную организацию?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.DeleteFarmOrganization(" + id_farm_organization + ");",
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
        public void DeleteFarmOrganization(String id_farm_organization)
        {
            if (connection_try)
            {
                if (NotNull(id_farm_organization) != "0")
                {
                    conn.Open();
                    SqlCommand delete_farm_organization = new SqlCommand("Delete_Farm_Organization", conn);
                    delete_farm_organization.CommandType = CommandType.StoredProcedure;
                    delete_farm_organization.Parameters.AddWithValue("@id_farm_organization", Convert.ToInt32(NotNull(id_farm_organization)));
                    delete_farm_organization.ExecuteNonQuery();
                    conn.Close();
                    FillFarmOrgs();
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Организация удалена!", IconCls = "icon-accept", Clear2 = true });
                }
            }
        }

        [DirectMethod]
        public void AddEditFarm(String id_farm, String title_region, String title_type_farm, String number_farm, String title_farm,
                                String location_farm, String animal_population)
        {
            if (connection_try)
            {
                if (NotNull(id_farm) == "0") { id_farm = "0"; }
                if (NotNullText(title_region) != "0" && NotNullText(title_type_farm) != "0" && NotNullText(number_farm) != "0")
                {
                    conn.Open();
                    SqlCommand add_edit_farm = new SqlCommand("Add_Edit_Farm", conn);
                    add_edit_farm.CommandType = CommandType.StoredProcedure;
                    add_edit_farm.Parameters.AddWithValue("@id_farm", Convert.ToInt32(NotNull(id_farm)));
                    add_edit_farm.Parameters.AddWithValue("@id_farm_organization", Convert.ToInt32(NotNull(current_id_farm_org)));
                    add_edit_farm.Parameters.AddWithValue("@title_region", NotNullText(title_region));
                    add_edit_farm.Parameters.AddWithValue("@title_type_farm", NotNullText(title_type_farm));
                    add_edit_farm.Parameters.AddWithValue("@number_farm", NotNullText(number_farm));
                    add_edit_farm.Parameters.AddWithValue("@title_farm", title_farm);
                    add_edit_farm.Parameters.AddWithValue("@location_farm", location_farm);
                    add_edit_farm.Parameters.AddWithValue("@animal_population", NotNull(animal_population));
                    add_edit_farm.ExecuteNonQuery();
                    conn.Close();
                    FillFarms(current_id_farm_org);
                }
            }
        }

        [DirectMethod]
        public void WindowRemovalFarm(String id_farm)
        {
            if (connection_try)
            {
                X.Msg.Confirm("Удаление записи", "Удалить выбранную площадку?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.DeleteFarm(" + id_farm + ");",
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
        public void DeleteFarm(String id_farm)
        {
            if (connection_try)
            {
                if (NotNull(id_farm) != "0")
                {
                    conn.Open();
                    SqlCommand delete_farm = new SqlCommand("Delete_Farm", conn);
                    delete_farm.CommandType = CommandType.StoredProcedure;
                    delete_farm.Parameters.AddWithValue("@id_farm", Convert.ToInt32(NotNull(id_farm)));
                    delete_farm.ExecuteNonQuery();
                    conn.Close();
                    FillFarms(current_id_farm_org);
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Площадка удалена!", IconCls = "icon-accept", Clear2 = true });
                }
            }
        }

        [DirectMethod]
        public void AddEditLagoon(String id_lagoon, String title_type_lagoon, String lagoon_number, String lagoon_name, String lagoon_volume)
        {
            if (connection_try)
            {
                if (NotNull(id_lagoon) == "0") { id_lagoon = "0"; }
                if (NotNull(lagoon_number) != "0" && NotNull(lagoon_volume) != "0")
                {
                    conn.Open();
                    SqlCommand add_edit_lagoon = new SqlCommand("Add_Edit_Lagoon", conn);
                    add_edit_lagoon.CommandType = CommandType.StoredProcedure;
                    add_edit_lagoon.Parameters.AddWithValue("@id_lagoon", Convert.ToInt32(NotNull(id_lagoon)));
                    add_edit_lagoon.Parameters.AddWithValue("@id_farm", Convert.ToInt32(NotNull(current_id_farm)));
                    add_edit_lagoon.Parameters.AddWithValue("@title_type_lagoon", NotNullText(title_type_lagoon));
                    add_edit_lagoon.Parameters.AddWithValue("@lagoon_number", Convert.ToInt32(NotNull(lagoon_number)));
                    add_edit_lagoon.Parameters.AddWithValue("@lagoon_name", lagoon_name);
                    add_edit_lagoon.Parameters.AddWithValue("@lagoon_volume", Convert.ToDouble(NotNull(lagoon_volume).Replace('.', ',')));
                    add_edit_lagoon.ExecuteNonQuery();
                    conn.Close();
                    FillLagoons(current_id_farm);
                }
            }
        }

        [DirectMethod]
        public void WindowRemovalLagoon(String id_lagoon)
        {
            if (connection_try)
            {
                X.Msg.Confirm("Удаление записи", "Удалить выбранную лагуну?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.DeleteLagoon(" + id_lagoon + ");",
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
        public void DeleteLagoon(String id_lagoon)
        {
            if (connection_try)
            {
                if (NotNull(id_lagoon) != "0")
                {
                    conn.Open();
                    SqlCommand delete_lagoon = new SqlCommand("Delete_Lagoon", conn);
                    delete_lagoon.CommandType = CommandType.StoredProcedure;
                    delete_lagoon.Parameters.AddWithValue("@id_lagoon", Convert.ToInt32(NotNull(id_lagoon)));
                    delete_lagoon.ExecuteNonQuery();
                    conn.Close();
                    FillLagoons(current_id_farm);
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Лагуна удалена!", IconCls = "icon-accept", Clear2 = true });
                }
            }
        }

        [DirectMethod]
        public void FillRegionsFromFarm()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterRegion = new SqlDataAdapter(selectCommRegions, conn);
                adapterRegion.Fill(indexDS, "RegionsFarm");
                indexDV = new System.Data.DataView(indexDS.Tables["RegionsFarm"]);
                Title_region_S.DataSource = indexDV;
                Title_region_S.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void FillTypeFarm()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterTypeFarm = new SqlDataAdapter(selectCommTypeFarm, conn);
                adapterTypeFarm.Fill(indexDS, "TypeFarm");
                indexDV = new System.Data.DataView(indexDS.Tables["TypeFarm"]);
                Type_farm_S.DataSource = indexDV;
                Type_farm_S.DataBind();
                conn.Close();
            }
        }

        [DirectMethod]
        public void ShowReportProtocolsW(Int32 type_report)
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterFarmOrgs = new SqlDataAdapter(selectCommFarmOrgs, conn);
                adapterFarmOrgs.Fill(indexDS, "ReportFarmOrgs");
                indexDV = new System.Data.DataView(indexDS.Tables["ReportFarmOrgs"]);
                ReportProtocolsFarmOrgS.DataSource = indexDV;
                ReportProtocolsFarmOrgS.DataBind();
                ReportProtocolsFarmOrgS.Sort("title_farm_organization", Ext.Net.SortDirection.Default);

                SqlDataAdapter adapterRegions = new SqlDataAdapter(selectCommRegions, conn);
                adapterRegions.Fill(indexDS, "ReportRegions");
                indexDV = new System.Data.DataView(indexDS.Tables["ReportRegions"]);
                ReportPlanRegionS.DataSource = indexDV;
                ReportPlanRegionS.DataBind();
                ReportPlanRegionS.Sort("title_region", Ext.Net.SortDirection.Default);

                SqlDataAdapter adapterFertilizer = new SqlDataAdapter(selectCommOrgFertilizer, conn);
                adapterFertilizer.Fill(indexDS, "ReportFertilizer");
                indexDV = new System.Data.DataView(indexDS.Tables["ReportFertilizer"]);
                ReportProtocolsFertilizerS.DataSource = indexDV;
                ReportProtocolsFertilizerS.DataBind();
                ReportProtocolsFertilizerS.Sort("title_fertilizer", Ext.Net.SortDirection.Default);
                conn.Close();

                if (type_report == 0)
                {
                    TypeReportTF.Text = "0";
                    ReportProtocolsFertilizerCB.AllowBlank = true;
                }
                else if (type_report == 1)
                {
                    TypeReportTF.Text = "1";
                    ReportProtocolsFertilizerCB.AllowBlank = false;

                }
                ReportProtocolsFertilizerCB.ReRender();

                ResetReportProtocols();
                FilterReportProtocolsW.Show();
            }
        }

        [DirectMethod]
        public void SelectReportFarm(String id_farm_org, String id_region)
        {
            if (connection_try)
            {
                String selectComm = selectCommFarms;
                if (NotNull(id_farm_org) != "0" || NotNull(id_region) != "0")
                {
                    selectComm += " WHERE";
                }
                if (NotNull(id_farm_org) != "0" && NotNull(id_region) != "0")
                {
                    selectComm += (" id_farm_organization=" + NotNull(id_farm_org));
                    selectComm += (" AND id_region=" + NotNull(id_region));
                }
                else if (NotNull(id_farm_org) != "0" && NotNull(id_region) == "0")
                {
                    selectComm += (" id_farm_organization=" + NotNull(id_farm_org));
                }
                else if (NotNull(id_farm_org) == "0" && NotNull(id_region) != "0")
                {
                    selectComm += (" id_region=" + NotNull(id_region));
                }

                conn.Open();
                SqlDataAdapter adapterFarms = new SqlDataAdapter(selectComm, conn);
                adapterFarms.Fill(indexDS, "ReportFarms");
                indexDV = new System.Data.DataView(indexDS.Tables["ReportFarms"]);
                ReportProtocolsFarmS.DataSource = indexDV;
                ReportProtocolsFarmS.DataBind();
                ReportProtocolsFarmS.Sort("title_farm", Ext.Net.SortDirection.Default);
                conn.Close();
            }
        }

        [DirectMethod]
        public void ResetReportProtocols()
        {
            ReportProtocolsFarmOrgCB.Clear();
            ReportProtocolsRegionCB.Clear();
            ReportProtocolsFarmCB.Clear();
            ReportProtocolsFertilizerCB.Clear();
            ReportProtocolsDF1.SelectedValue = new DateTime(DateTime.Today.Year, 1, 1);
            ReportProtocolsDF2.SelectedValue = new DateTime(DateTime.Today.Year, 12, 31);
        }

        [DirectMethod]
        public void CancelReportProtocols()
        {
            FilterReportProtocolsW.Close();
        }

        [DirectMethod]
        public void AcceptReportProtocols()
        {
            if (Request.Browser.Cookies)
            {
                HttpCookie report_protocols = Request.Cookies["Agrochim31_Report_ProtocolsFertilizer"];
                if (report_protocols == null)
                {
                    report_protocols = new HttpCookie("Agrochim31_Report_ProtocolsFertilizer");
                    Response.Cookies.Add(report_protocols);
                }

                if (ReportProtocolsFarmOrgCB.Text == String.Empty)
                {
                    report_protocols["id_farm_organization"] = "0";
                }
                else
                {
                    report_protocols["id_farm_organization"] = ReportProtocolsFarmOrgCB.Value.ToString();
                }

                if (ReportProtocolsRegionCB.Text == String.Empty)
                {
                    report_protocols["id_region"] = "0";
                }
                else
                {
                    report_protocols["id_region"] = ReportProtocolsRegionCB.Value.ToString();
                }

                if (ReportProtocolsFarmCB.Text == String.Empty)
                {
                    report_protocols["id_farm"] = "0";
                }
                else
                {
                    report_protocols["id_farm"] = ReportProtocolsFarmCB.Value.ToString();
                }

                if (ReportProtocolsFertilizerCB.Text == String.Empty)
                {
                    report_protocols["id_fertilizer"] = "0";
                }
                else
                {
                    report_protocols["id_fertilizer"] = ReportProtocolsFertilizerCB.Value.ToString();
                }

                //"dd.MM.yyyy HH:mm:ss"
                report_protocols["date_from_protocols"] = ReportProtocolsDF1.SelectedDate.Ticks.ToString();

                report_protocols["date_to_protocols"] = ReportProtocolsDF2.SelectedDate.AddDays(1).AddMilliseconds(-1).Ticks.ToString();

                Response.Cookies.Set(report_protocols);

                CancelReportProtocols();

                if (TypeReportTF.Text == "0")
                {
                    ReportProtocolsW.Reload();
                    ReportProtocolsW.Show();
                }
                else if (TypeReportTF.Text == "1")
                {
                    ResultsStatisticsW.Reload();
                    ResultsStatisticsW.Show();
                }
            }

        }
        [DirectMethod]
        public void ProtocolEditWindowAdd()
        {
            ProtocolEditTF.Text = "";
            ProtocolEditFertilizerCB.Clear();
            ProtocolEditOrganizationTF.Text = "";
            ProtocolEditSizeNF.Clear();
            ProtocolEditSizeUnitTF.Text = "";
            ProtocolEditValueNF.Clear();
            ProtocolEditValueUnitTF.Text = "";
            ProtocolEditDeviationNF.Clear();
            ProtocolEditDeviationUnitTF.Text = "";
            ProtocolEditOrganizationProbeTF.Text = "";
            ProtocolEditDocumentTF.Text = "";
            ProtocolEditSelectingDateDF.Clear();
            ProtocolEditReceptionDateDF.Clear();
            ProtocolEditTestingTimeBeginDF.Text = "";
            ProtocolEditTestingTimeEndDF.Text = "";
            ProtocolEditSelectingDocumentTF.Text = "";
            ProtocolEditTestingDocumentTF.Text = "";
            ProtocolEditHardenerPositionTF.Text = "";
            ProtocolEditHardenerTF.Text = "";
            ProtocolEditCommentTA.Text = "";
            ProtocolEditGridS.RemoveAll();

            ProtocolEditAddB.Hidden = false;
            ProtocolEditSaveB.Hidden = true;

            ProtocolEditW.Show();
        }

        [DirectMethod]
        public void ProtocolEditWindowEdit(String id_protocol)
        {
            ProtocolEditTF.Text = "";
            ProtocolEditFertilizerCB.Clear();
            ProtocolEditOrganizationTF.Text = "";
            ProtocolEditSizeNF.Clear();
            ProtocolEditSizeUnitTF.Text = "";
            ProtocolEditValueNF.Clear();
            ProtocolEditValueUnitTF.Text = "";
            ProtocolEditDeviationNF.Clear();
            ProtocolEditDeviationUnitTF.Text = "";
            ProtocolEditOrganizationProbeTF.Text = "";
            ProtocolEditDocumentTF.Text = "";
            ProtocolEditSelectingDateDF.Clear();
            ProtocolEditReceptionDateDF.Clear();
            ProtocolEditTestingTimeBeginDF.Text = "";
            ProtocolEditTestingTimeEndDF.Text = "";
            ProtocolEditSelectingDocumentTF.Text = "";
            ProtocolEditTestingDocumentTF.Text = "";
            ProtocolEditHardenerPositionTF.Text = "";
            ProtocolEditHardenerTF.Text = "";
            ProtocolEditCommentTA.Text = "";
            ProtocolEditGridS.RemoveAll();

            ProtocolEditAddB.Hidden = true;
            ProtocolEditSaveB.Hidden = false;

            if (connection_try)
            {
                String selectProtocolInfo = "SELECT * FROM View_Protocols WHERE id_protocol=" + id_protocol;
                conn.Open();
                adapterProtocolInfo = new SqlDataAdapter(selectProtocolInfo, conn);
                adapterProtocolInfo.Fill(indexDS, "ProtocolInfo");
                conn.Close();

                ProtocolEditTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["number_protocol"].ToString();
                ProtocolEditFertilizerCB.SelectedItem.Value = indexDS.Tables["ProtocolInfo"].Rows[0]["id_fertilizer"].ToString();
                ProtocolEditFertilizerCB.SelectedItem.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["title_fertilizer"].ToString();
                ProtocolEditFertilizerCB.UpdateSelectedItems();
                ProtocolEditOrganizationTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["organization_applicant_name"].ToString();
                ProtocolEditSizeNF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["batch_size"].ToString();
                ProtocolEditSizeUnitTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["units_batch"].ToString();
                ProtocolEditValueNF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["sample_weight"].ToString();
                ProtocolEditValueUnitTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["units_sample_weight"].ToString();
                ProtocolEditDeviationNF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["sample_sediment_weight"].ToString();
                ProtocolEditDeviationUnitTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["units_sample_sediment_weight"].ToString();
                ProtocolEditOrganizationProbeTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["organization_selected_name"].ToString();
                ProtocolEditDocumentTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["accompanying_document"].ToString();
                ProtocolEditSelectingDateDF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["sample_selecting_date"].ToString();
                ProtocolEditReceptionDateDF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["sample_reception_date"].ToString();
                ProtocolEditTestingTimeBeginDF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["testing_time_begin"].ToString();
                ProtocolEditTestingTimeEndDF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["testing_time_end"].ToString();
                ProtocolEditSelectingDocumentTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["normative_document_selection"].ToString();
                ProtocolEditTestingDocumentTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["normative_document_testing"].ToString();
                ProtocolEditHardenerPositionTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["hardener_position"].ToString();
                ProtocolEditHardenerTF.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["hardener"].ToString();
                ProtocolEditCommentTA.Text = indexDS.Tables["ProtocolInfo"].Rows[0]["comment"].ToString();

                //ProtocolEditResultsGP.GetSelectionModel().Select(Convert.ToInt32(NotNull(current_selected_result)));
            }

            FillProtocolResults(id_protocol);

            ProtocolEditW.Show();
        }

        [DirectMethod]
        public void WindowRemovalResult(String id_result)
        {
            if (connection_try)
            {
                X.Msg.Confirm("Удаление записи", "Удалить выбранный результат?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "#{ProtocolEditGridS}.remove(#{ProtocolEditResultsGP}.getView().getSelectionModel().getSelection()[0]); App.direct.DeleteResult(" + id_result + ");",
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
        public void DeleteResult(String id_result)
        {
            if (connection_try)
            {
                if (NotNull(id_result) != "0")
                {
                    conn.Open();
                    SqlCommand delete_lagoon = new SqlCommand("Delete_Result", conn);
                    delete_lagoon.CommandType = CommandType.StoredProcedure;
                    delete_lagoon.Parameters.AddWithValue("@id_result", Convert.ToInt32(NotNull(id_result)));
                    delete_lagoon.ExecuteNonQuery();
                    conn.Close();
                    //FillLagoons(current_id_farm);
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Результат удален!", IconCls = "icon-accept", Clear2 = true });
                }
            }
        }

        [DirectMethod]
        public void FillFertilizer()
        {
            if (connection_try)
            {
                conn.Open();
                adapterFertilizer = new SqlDataAdapter(selectCommOrgFertilizer, conn);
                adapterFertilizer.Fill(indexDS, "OrgFertilizer");
                conn.Close();

                indexDV = new System.Data.DataView(indexDS.Tables["OrgFertilizer"]);
                ProtocolEditFertilizerS.DataSource = indexDV;
                /*FertilizerS.DataBind();
                FertilizerS.Sort("title_fertilizer", Ext.Net.SortDirection.Default);*/
            }
        }

        [DirectMethod]
        public void FillSignFert()
        {
            if (connection_try)
            {
                conn.Open();
                adapterSignFert = new SqlDataAdapter(selectCommSignFert, conn);
                adapterSignFert.Fill(indexDS, "SignificativeFertilizer");
                conn.Close();

                indexDV = new System.Data.DataView(indexDS.Tables["SignificativeFertilizer"]);
                Title_significative_fertilizerS.DataSource = indexDV;
                /*Title_significative_fertilizerS.DataBind();
                Title_significative_fertilizerS.Sort("title_significative_fertilizer", Ext.Net.SortDirection.Default);*/
            }
        }

        [DirectMethod]
        public void FillUnitsSF()
        {
            if (connection_try)
            {
                conn.Open();
                adapterUnitsSF = new SqlDataAdapter(selectCommUnitsSF, conn);
                adapterUnitsSF.Fill(indexDS, "UnitsSF");
                conn.Close();

                indexDV = new System.Data.DataView(indexDS.Tables["UnitsSF"]);
                UnitsSignificativeFertilizerS.DataSource = indexDV;
                /*UnitsSignificativeFertilizerS.DataBind();
                UnitsSignificativeFertilizerS.Sort("unit_s_f", Ext.Net.SortDirection.Default);*/
            }
        }

        [DirectMethod]
        public void FillDocuments()
        {
            if (connection_try)
            {
                conn.Open();
                adapterDocuments = new SqlDataAdapter(selectCommDocuments, conn);
                adapterDocuments.Fill(indexDS, "Documents");
                conn.Close();

                indexDV = new System.Data.DataView(indexDS.Tables["Documents"]);
                DocumentS.DataSource = indexDV;
                /*DocumentS.DataBind();
                DocumentS.Sort("title_normative_document", Ext.Net.SortDirection.Default);*/
            }
        }

        [DirectMethod]
        public void FillProtocolResults(String id_protocol)
        {
            if (connection_try)
            {
                String selectCommProtocolResults = "SELECT * FROM View_Results_Protocol WHERE id_protocol=" + id_protocol;
                conn.Open();
                adapterProtocolResults = new SqlDataAdapter(selectCommProtocolResults, conn);
                adapterProtocolResults.Fill(indexDS, "ProtocolResults");
                conn.Close();

                indexDV = new System.Data.DataView(indexDS.Tables["ProtocolResults"]);
                ProtocolEditGridS.DataSource = indexDV;
                ProtocolEditGridS.DataBind();
            }
        }

        [DirectMethod]
        public void AddProtocol(String number_protocol, String id_fertilizer, String organization_applicant_name, String id_lagoon, String batch_size, String units_batch, String sample_weight, String units_sample_weight, String sample_sediment_weight, String units_sample_sediment_weight, String organization_selected_name, String accompanying_document, String sample_selecting_date, String sample_reception_date, String testing_time_begin, String testing_time_end, String normative_document_selection, String normative_document_testing, String comment, String hardener_position, String hardener, String additional, String results_data)
        {
            if (connection_try)
            {
                String result_protocol = "0", query_result = "0";
                Int32 id_user = user_reg_data.id_user;
                Int64 sample_selecting_date_long = ParseDate(sample_selecting_date);
                Int64 sample_reception_date_long = ParseDate(sample_reception_date);
                Int64 testing_time_begin_long = ParseDate(testing_time_begin);
                Int64 testing_time_end_long = ParseDate(testing_time_end);

                conn.Open();
                SqlCommand add_protocol = new SqlCommand("Add_Protocol", conn);
                add_protocol.CommandType = CommandType.StoredProcedure;
                add_protocol.CommandTimeout = 300;
                add_protocol.Parameters.AddWithValue("@number_protocol", number_protocol);
                add_protocol.Parameters.AddWithValue("@id_fertilizer", Convert.ToInt32(id_fertilizer));
                add_protocol.Parameters.AddWithValue("@organization_applicant_name", organization_applicant_name);
                add_protocol.Parameters.AddWithValue("@id_lagoon", Convert.ToInt32(id_lagoon));
                add_protocol.Parameters.AddWithValue("@batch_size", float.Parse(NotNull(batch_size.Replace(',', '.'))));
                add_protocol.Parameters.AddWithValue("@units_batch", units_batch);
                add_protocol.Parameters.AddWithValue("@sample_weight", float.Parse(NotNull(sample_weight.Replace(',', '.'))));
                add_protocol.Parameters.AddWithValue("@units_sample_weight", units_sample_weight);
                add_protocol.Parameters.AddWithValue("@sample_sediment_weight", float.Parse(NotNull(sample_sediment_weight.Replace(',', '.'))));
                add_protocol.Parameters.AddWithValue("@units_sample_sediment_weight", units_sample_sediment_weight);
                add_protocol.Parameters.AddWithValue("@organization_selected_name", organization_selected_name);
                add_protocol.Parameters.AddWithValue("@accompanying_document", accompanying_document);
                add_protocol.Parameters.AddWithValue("@sample_selecting_date_long", sample_selecting_date_long);
                add_protocol.Parameters.AddWithValue("@sample_reception_date_long", sample_reception_date_long);
                add_protocol.Parameters.AddWithValue("@testing_time_begin_long", testing_time_begin_long);
                add_protocol.Parameters.AddWithValue("@testing_time_end_long", testing_time_end_long);
                add_protocol.Parameters.AddWithValue("@normative_document_selection", normative_document_selection);
                add_protocol.Parameters.AddWithValue("@normative_document_testing", normative_document_testing);
                add_protocol.Parameters.AddWithValue("@id_user", id_user);
                add_protocol.Parameters.AddWithValue("@comment", comment);
                add_protocol.Parameters.AddWithValue("@hardener_position", hardener_position);
                add_protocol.Parameters.AddWithValue("@hardener", hardener);
                add_protocol.Parameters.AddWithValue("@additional", additional);

                add_protocol.Parameters.Add("@id_protocol", SqlDbType.Int);
                add_protocol.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                add_protocol.Parameters["@id_protocol"].Direction = ParameterDirection.Output;
                add_protocol.Parameters["@result"].Direction = ParameterDirection.Output;

                add_protocol.ExecuteNonQuery();
                Int32 id_protocol = Convert.ToInt32(add_protocol.Parameters["@id_protocol"].Value);
                result_protocol = add_protocol.Parameters["@result"].Value.ToString();

                if (result_protocol == "0")
                {
                    X.Msg.Notify("Результат добавления протокола", "Протокол успешно добавлен!").Show();

                    List<Dictionary<String, String>> results = JSON.Deserialize<List<Dictionary<String, String>>>(results_data);
                    if (results.Count > 0)
                    {
                        for (int i = 0; i < results.Count; ++i)
                        {
                            if (((NotNull(results[i]["id_result"])) == "0" && results[i]["id_result"] != "0") || (NotNull(id_protocol.ToString())) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Обратитесь к администратору.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["title_significative_fertilizer"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбрано наименование показателя.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["unit_s_f"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбраны единицы измерения.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["title_normative_document"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбран нормативный документ.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                        }
                        for (int j = 0; j < results.Count; ++j)
                        {
                            SqlCommand add_edit_result = new SqlCommand("Add_Edit_Result", conn);
                            add_edit_result.CommandType = CommandType.StoredProcedure;
                            add_edit_result.CommandTimeout = 300;

                            add_edit_result.Parameters.AddWithValue("@id_result", Convert.ToInt32(NotNull(results[j]["id_result"])));
                            add_edit_result.Parameters.AddWithValue("@id_protocol", id_protocol);
                            add_edit_result.Parameters.AddWithValue("@title_significative_fertilizer", results[j]["title_significative_fertilizer"]);
                            add_edit_result.Parameters.AddWithValue("@unit_s_f", results[j]["unit_s_f"]);
                            add_edit_result.Parameters.AddWithValue("@value", Math.Round(float.Parse(NotNull(results[j]["value"].Replace(',', '.'))), 4));
                            add_edit_result.Parameters.AddWithValue("@deviation", results[j]["deviation"]);
                            add_edit_result.Parameters.AddWithValue("@title_normative_document", results[j]["title_normative_document"]);
                            add_edit_result.Parameters.AddWithValue("@number_of_digits", Convert.ToInt32(NotNull(results[j]["number_of_digits"])));
                            add_edit_result.Parameters.AddWithValue("@minimum", NotNull(results[j]["minimum"]));

                            add_edit_result.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                            add_edit_result.Parameters["@result"].Direction = ParameterDirection.Output;
                            add_edit_result.ExecuteNonQuery();
                            query_result = add_edit_result.Parameters["@result"].Value.ToString();

                            if (query_result != "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результат добавления результатов протокола",
                                    Message = "Некоторые результаты не добавлены! Обратитесь к администратору.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                            }
                        }
                    }
                }
                else
                {
                    X.Msg.Notify("Результат добавления протокола", "Протокол не добавлен!\n" + result_protocol).Show();
                }
                conn.Close();

                FillProtocols(id_lagoon);
                ProtocolEditW.Close();
            }
        }

        [DirectMethod]
        public void EditProtocol(String id_protocol, String number_protocol, String id_fertilizer, String organization_applicant_name, String id_lagoon, String batch_size, String units_batch, String sample_weight, String units_sample_weight, String sample_sediment_weight, String units_sample_sediment_weight, String organization_selected_name, String accompanying_document, String sample_selecting_date, String sample_reception_date, String testing_time_begin, String testing_time_end, String normative_document_selection, String normative_document_testing, String comment, String hardener_position, String hardener, String additional, String results_data)
        {
            if (connection_try)
            {
                String result_protocol = "0", query_result = "0";
                Int32 id_user = user_reg_data.id_user;
                Int64 sample_selecting_date_long = ParseDate(sample_selecting_date);
                Int64 sample_reception_date_long = ParseDate(sample_reception_date);
                Int64 testing_time_begin_long = ParseDate(testing_time_begin);
                Int64 testing_time_end_long = ParseDate(testing_time_end);

                conn.Open();
                SqlCommand edit_protocol = new SqlCommand("Edit_Protocol", conn);
                edit_protocol.CommandType = CommandType.StoredProcedure;
                edit_protocol.CommandTimeout = 300;
                edit_protocol.Parameters.AddWithValue("@id_protocol", Convert.ToInt32(id_protocol));
                edit_protocol.Parameters.AddWithValue("@number_protocol", number_protocol);
                edit_protocol.Parameters.AddWithValue("@id_fertilizer", Convert.ToInt32(id_fertilizer));
                edit_protocol.Parameters.AddWithValue("@organization_applicant_name", organization_applicant_name);
                edit_protocol.Parameters.AddWithValue("@id_lagoon", Convert.ToInt32(id_lagoon));
                edit_protocol.Parameters.AddWithValue("@batch_size", float.Parse(NotNull(batch_size.Replace(',', '.'))));
                edit_protocol.Parameters.AddWithValue("@units_batch", units_batch);
                edit_protocol.Parameters.AddWithValue("@sample_weight", float.Parse(NotNull(sample_weight.Replace(',', '.'))));
                edit_protocol.Parameters.AddWithValue("@units_sample_weight", units_sample_weight);
                edit_protocol.Parameters.AddWithValue("@sample_sediment_weight", float.Parse(NotNull(sample_sediment_weight.Replace(',', '.'))));
                edit_protocol.Parameters.AddWithValue("@units_sample_sediment_weight", units_sample_sediment_weight);
                edit_protocol.Parameters.AddWithValue("@organization_selected_name", organization_selected_name);
                edit_protocol.Parameters.AddWithValue("@accompanying_document", accompanying_document);
                edit_protocol.Parameters.AddWithValue("@sample_selecting_date_long", sample_selecting_date_long);
                edit_protocol.Parameters.AddWithValue("@sample_reception_date_long", sample_reception_date_long);
                edit_protocol.Parameters.AddWithValue("@testing_time_begin_long", testing_time_begin_long);
                edit_protocol.Parameters.AddWithValue("@testing_time_end_long", testing_time_end_long);
                edit_protocol.Parameters.AddWithValue("@normative_document_selection", normative_document_selection);
                edit_protocol.Parameters.AddWithValue("@normative_document_testing", normative_document_testing);
                edit_protocol.Parameters.AddWithValue("@id_user", id_user);
                edit_protocol.Parameters.AddWithValue("@comment", comment);
                edit_protocol.Parameters.AddWithValue("@hardener_position", hardener_position);
                edit_protocol.Parameters.AddWithValue("@hardener", hardener);
                edit_protocol.Parameters.AddWithValue("@additional", additional);

                edit_protocol.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                edit_protocol.Parameters["@result"].Direction = ParameterDirection.Output;

                edit_protocol.ExecuteNonQuery();
                result_protocol = edit_protocol.Parameters["@result"].Value.ToString();

                if (result_protocol == "0")
                {
                    X.Msg.Notify("Результат редактирования протокола", "Протокол успешно изменён!").Show();

                    List<Dictionary<String, String>> results = JSON.Deserialize<List<Dictionary<String, String>>>(results_data);
                    if (results.Count > 0)
                    {
                        for (int i = 0; i < results.Count; ++i)
                        {
                            if (((NotNull(results[i]["id_result"])) == "0" && results[i]["id_result"] != "0") || (NotNull(id_protocol.ToString())) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Обратитесь к администратору.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["title_significative_fertilizer"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбрано наименование показателя.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["unit_s_f"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбраны единицы измерения.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                            if ((NotNull(results[i]["title_normative_document"])) == "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результаты не добавлены!",
                                    Message = "Некоторые результаты не добавлены! Не везде выбран нормативный документ.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                                conn.Close();
                                return;
                            }
                        }
                            for (int j = 0; j < results.Count; ++j)
                        {
                            SqlCommand add_edit_result = new SqlCommand("Add_Edit_Result", conn);
                            add_edit_result.CommandType = CommandType.StoredProcedure;
                            add_edit_result.CommandTimeout = 300;

                            add_edit_result.Parameters.AddWithValue("@id_result", Convert.ToInt32(NotNull(results[j]["id_result"])));
                            add_edit_result.Parameters.AddWithValue("@id_protocol", id_protocol);
                            add_edit_result.Parameters.AddWithValue("@title_significative_fertilizer", results[j]["title_significative_fertilizer"]);
                            add_edit_result.Parameters.AddWithValue("@unit_s_f", results[j]["unit_s_f"]);
                            add_edit_result.Parameters.AddWithValue("@value", Math.Round(float.Parse(NotNull(results[j]["value"].Replace(',', '.'))), 4));
                            add_edit_result.Parameters.AddWithValue("@deviation", results[j]["deviation"]);
                            add_edit_result.Parameters.AddWithValue("@title_normative_document", results[j]["title_normative_document"]);
                            add_edit_result.Parameters.AddWithValue("@number_of_digits", Convert.ToInt32(NotNull(results[j]["number_of_digits"])));
                            add_edit_result.Parameters.AddWithValue("@minimum", NotNull(results[j]["minimum"]));

                            add_edit_result.Parameters.Add("@result", SqlDbType.VarChar, 1250);
                            add_edit_result.Parameters["@result"].Direction = ParameterDirection.Output;
                            add_edit_result.ExecuteNonQuery();
                            query_result = add_edit_result.Parameters["@result"].Value.ToString();

                            if (query_result != "0")
                            {
                                X.Msg.Show(new MessageBoxConfig
                                {
                                    //ERROR_NUMBER() + ', ' + ERROR_SEVERITY() + ', ' + ERROR_STATE() + ', ' + ERROR_PROCEDURE() + ', ' + ERROR_LINE() + ', ' + ERROR_MESSAGE()
                                    Title = "Результат редактирования результатов протокола",
                                    Message = "Некоторые результаты не добавлены! Обратитесь к администратору.\n" + query_result,
                                    Buttons = MessageBox.Button.OK,
                                    Icon = MessageBox.Icon.INFO
                                });
                            }
                        }
                    }
                }
                else
                {
                    X.Msg.Notify("Результат редактирования протокола", "Протокол не изменён!\n" + result_protocol).Show();
                }
                conn.Close();

                FillProtocols(id_lagoon);
                ProtocolEditW.Close();
            }
        }

        private Int64 ParseDate(String datetime)
        {
            Int64 datetime_long = 0;
            if (datetime != String.Empty && datetime != null && datetime != "null")
            {
                datetime = datetime.Replace("\"", "");
                String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                if (datetime.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                datetime_long = DateTime.ParseExact(datetime, format_date_time, System.Globalization.CultureInfo.InvariantCulture).Ticks;
            }
            return datetime_long;
        }

        [DirectMethod]
        public void CopyFertilizerProtocol(String id_lagoon, String id_protocol)
        {
            if (connection_try)
            {
                if (NotNull(id_lagoon) != "0" && NotNull(id_protocol) != "0")
                {
                    conn.Open();
                    SqlCommand copy_protocol = new SqlCommand("Copy_Fertilizer_Protocol", conn);
                    copy_protocol.CommandType = CommandType.StoredProcedure;
                    copy_protocol.Parameters.AddWithValue("@id_lagoon", Convert.ToInt32(NotNull(id_lagoon)));
                    copy_protocol.Parameters.AddWithValue("@id_protocol", Convert.ToInt32(NotNull(id_protocol)));
                    copy_protocol.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                    copy_protocol.ExecuteNonQuery();
                    conn.Close();

                    FillProtocols(id_lagoon);
                }
            }
        }

        [DirectMethod]
        public void WindowRemovalProtocol(String id_protocol)
        {
            if (connection_try)
            {
                X.Msg.Confirm("Удаление записи", "Удалить выбранный протокол?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "App.direct.DeleteProtocol(" + id_protocol + ");",
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
        public void DeleteProtocol(String id_protocol)
        {
            if (connection_try)
            {
                if (NotNull(id_protocol) != "0")
                {
                    conn.Open();
                    SqlCommand delete_protocol = new SqlCommand("Delete_Protocol", conn);
                    delete_protocol.CommandType = CommandType.StoredProcedure;
                    delete_protocol.Parameters.AddWithValue("@id_protocol", Convert.ToInt32(NotNull(id_protocol)));
                    delete_protocol.ExecuteNonQuery();
                    conn.Close();
                    FillProtocols(current_id_lagoon);
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Протокол удалён!", IconCls = "icon-accept", Clear2 = true });
                }
            }
        }

        [DirectMethod]
        public void BindTypeLagoons()
        {
            if (connection_try)
            {
                conn.Open();
                SqlDataAdapter adapterTypeLagoons = new SqlDataAdapter(selectCommTypeLagoon, conn);
                adapterTypeLagoons.Fill(indexDS, "Type_lagoon");
                indexDV = new System.Data.DataView(indexDS.Tables["Type_lagoon"]);
                EditTypeLagoonS.DataSource = indexDV;
                EditTypeLagoonS.DataBind();
                conn.Close();
            }
        }
    }
}