using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace agro_proba
{
    /// <summary>
    /// Сводное описание для OLGISMapService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class OLGISMapService : System.Web.Services.WebService
    {
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

        public class OLGISMapColor
        {
            public Int32 id_color { get; set; }
            public Int32 red { get; set; }
            public Int32 green { get; set; }
            public Int32 blue { get; set; }
            public Double opacity { get; set; }
            public String description { get; set; }
        }

        [WebMethod]
        public String GetColors()
        {
            List<OLGISMapColor> colors = new List<OLGISMapColor>();

            String conn_str = SetConnectionString();
            String get_colors_str = "SELECT * FROM Colors";

            DataSet colorsDS = new DataSet();
            SqlConnection this_conn = new SqlConnection(conn_str);
            SqlDataAdapter get_colors = new SqlDataAdapter(get_colors_str, this_conn);
            get_colors.Fill(colorsDS, "Colors");

            if (colorsDS.Tables["Colors"].Rows.Count > 0)
            {
                for (int i = 0; i < colorsDS.Tables["Colors"].Rows.Count; i++)
                {
                    OLGISMapColor color = new OLGISMapColor();
                    color.id_color = Convert.ToInt32(colorsDS.Tables["Colors"].Rows[i]["id_color"].ToString());
                    color.red = Convert.ToInt32(colorsDS.Tables["Colors"].Rows[i]["red"].ToString());
                    color.green = Convert.ToInt32(colorsDS.Tables["Colors"].Rows[i]["green"].ToString());
                    color.blue = Convert.ToInt32(colorsDS.Tables["Colors"].Rows[i]["blue"].ToString());
                    color.opacity = Convert.ToDouble(colorsDS.Tables["Colors"].Rows[i]["opacity"].ToString());
                    color.description = colorsDS.Tables["Colors"].Rows[i]["description"].ToString();
                    colors.Add(color);
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod]
        public void UpdateColor(Int32 id_color, Int32 red, Int32 green, Int32 blue, Double opacity, String description)
        {
            String conn_str = SetConnectionString();
            SqlConnection this_conn = new SqlConnection(conn_str);
            this_conn.Open();
            SqlCommand edit_color = new SqlCommand("Edit_Color", this_conn);
            edit_color.CommandType = CommandType.StoredProcedure;
            edit_color.Parameters.AddWithValue("@id_color", id_color);
            edit_color.Parameters.AddWithValue("@red", red);
            edit_color.Parameters.AddWithValue("@green", green);
            edit_color.Parameters.AddWithValue("@blue", blue);
            edit_color.Parameters.AddWithValue("@opacity", opacity);
            edit_color.Parameters.AddWithValue("@description", description);
            edit_color.ExecuteNonQuery();
            this_conn.Close();
        }

        [WebMethod]
        public void InsertColor(Int32 red, Int32 green, Int32 blue, Double opacity, String description)
        {
            String conn_str = SetConnectionString();
            SqlConnection this_conn = new SqlConnection(conn_str);
            this_conn.Open();
            SqlCommand add_color = new SqlCommand("Add_Color", this_conn);
            add_color.CommandType = CommandType.StoredProcedure;
            add_color.Parameters.AddWithValue("@red", red);
            add_color.Parameters.AddWithValue("@green", green);
            add_color.Parameters.AddWithValue("@blue", blue);
            add_color.Parameters.AddWithValue("@opacity", opacity);
            add_color.Parameters.AddWithValue("@description", description);
            add_color.ExecuteNonQuery();
            this_conn.Close();
        }

        [WebMethod]
        public void DeleteColor(Int32 id_color)
        {
            String conn_str = SetConnectionString();
            SqlConnection this_conn = new SqlConnection(conn_str);
            this_conn.Open();
            SqlCommand delete_color = new SqlCommand("Delete_Color", this_conn);
            delete_color.CommandType = CommandType.StoredProcedure;
            delete_color.Parameters.AddWithValue("@id_color", id_color);
            delete_color.ExecuteNonQuery();
            this_conn.Close();
        }
    }
}
