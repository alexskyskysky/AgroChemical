using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Xml;

namespace agro_proba
{
    /// <summary>
    /// Сводное описание для GEORSSHandler
    /// </summary>
    public class GEORSSHandler : IHttpHandler
    {
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

        public String SetConnectionString()
        {
            String connString = String.Empty;
            try
            {
                //Создаём переменную Xml
                XmlDocument connect_to_server = new XmlDocument();
                //Загружаем параметры с файла
                connect_to_server.LoadXml(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/") + "connection.xml"));
                //Выбираем отдельные элементы и выводим их
                XmlNodeList connection = connect_to_server.GetElementsByTagName("item");
                //"Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N"
                connString = "Data Source=" + GetValueFromXml(connect_to_server, "item", "server", "value")
                                          + "; Initial Catalog=" + GetValueFromXml(connect_to_server, "item", "database", "value")
                                          + "; Persist Security Info=True; User ID=" + GetValueFromXml(connect_to_server, "item", "login", "value")
                                          + "; Password=" + GetValueFromXml(connect_to_server, "item", "password", "value");
            }
            catch (Exception) { }
            return connString;
        }

        public void ProcessRequest(HttpContext context)
        {
            String type_data = "", latitude = "", longitude = "";
            if (!String.IsNullOrEmpty(context.Request.QueryString["t"]))
                type_data = context.Request.QueryString["t"].ToString();
            if (!String.IsNullOrEmpty(context.Request.QueryString["latitude"]))
                latitude = context.Request.QueryString["latitude"].ToString();
            if (!String.IsNullOrEmpty(context.Request.QueryString["longitude"]))
                longitude = context.Request.QueryString["longitude"].ToString();

            //string connString = "server=SARMAT\\W8_SQL8_R2_EXPR;Initial Catalog=Soil-db; User Id=sa;Password=1;";
            String connString = SetConnectionString();
            // Define a connection to the database
            SqlConnection myConn = new SqlConnection(connString);
            // Open the connection
            myConn.Open();

            // Set the query to run against the connection
            SqlCommand myCMD = new SqlCommand("RSS_Get_Data", myConn);
            myCMD.CommandType = System.Data.CommandType.StoredProcedure;
            myCMD.Parameters.AddWithValue("@type_data", type_data);
            myCMD.Parameters.AddWithValue("@point_latitude", Convert.ToDouble(latitude.Replace('.', ',')));
            myCMD.Parameters.AddWithValue("@point_longitude", Convert.ToDouble(longitude.Replace('.', ',')));
            //myCMD.ExecuteNonQuery();

            // Create a reader for the results
            SqlDataReader myReader = myCMD.ExecuteReader();

            // Set the response headers
            context.Response.ContentType = "text/xml";
            context.Response.Charset = "win-1251";
            context.Response.CacheControl = "no-cache";
            context.Response.Expires = 0;

            // Read through the results
            while (myReader.Read())
            {
                // Write the GeoRSS response back to the client
                context.Response.Write(myReader["GeoRSSFeed"].ToString());
            }

            // Close the reader
            myReader.Close();

            // Close the connection
            myConn.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}