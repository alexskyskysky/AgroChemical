using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Xml;

namespace agro_proba
{
    public partial class TestJS_new : System.Web.UI.Page
    {
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
            catch (Exception exc)
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public List<String> GetSpatialData()
        {
            SqlConnection conn;
            Boolean connection_try = false;
            String connStr;
            connStr = SetConnectionString();
            conn = new SqlConnection(connStr);
            connection_try = TryConnection(connStr);
            List<String> spatialDataList = new List<String>();
            DataSet layersDS = new DataSet();
            String feature_str = String.Empty;
            if (connection_try)
            {
                conn.Open();
                String querry_string = "exec [dbo].[GetTerritoryGeoJSON];";
                SqlDataAdapter gerions_geo_data = new SqlDataAdapter(querry_string, conn);
                gerions_geo_data.Fill(layersDS, "Territory");
                conn.Close();

                if (layersDS.Tables["Territory"].Rows.Count > 0)
                {
                    for (int i = 0; i < layersDS.Tables["Territory"].Rows.Count; i++)
                    {
                        feature_str = "layer:territory|id_feature:" + i.ToString();
                        for (int j = 1; j < layersDS.Tables["Territory"].Columns.Count; j++)
                        {
                            feature_str += "|";
                            feature_str += (layersDS.Tables["Territory"].Columns[j].ColumnName.ToString() + ":" + layersDS.Tables["Territory"].Rows[i][j].ToString());
                        }
                        spatialDataList.Add(feature_str);
                    }
                }
            }
            return spatialDataList;
        }
    }
}