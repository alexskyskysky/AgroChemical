using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace agro_proba
{
    public partial class FileUpload : System.Web.UI.Page
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
                connect_to_server.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/") + "connection.xml"));
                //Выбираем отдельные элементы и выводим их
                XmlNodeList connection = connect_to_server.GetElementsByTagName("item");
                //"Data Source=192.168.0.99;Initial Catalog=Agrochim31;Persist Security Info=True;User ID=sa; Password=Tr1nItr0N"
                connString = "Data Source=" + GetValueFromXml(connect_to_server, "item", "server", "value")
                                          + "; Initial Catalog=" + GetValueFromXml(connect_to_server, "item", "database", "value")
                                          + "; Persist Security Info=True; User ID=" + GetValueFromXml(connect_to_server, "item", "login", "value")
                                          + "; Password=" + GetValueFromXml(connect_to_server, "item", "password", "value");
            }
            catch (Exception){}
            return connString;
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
                StatusLabel.Text = ("Статус: Ошибка соединения с БД" + exc.Message + "\n" + connection.ConnectionString + "\nНастройте параметры подключения к базе данных!");
                rez = false;
            }
            finally
            {
                connection.Dispose();
            }
            return rez;
        }

        public void ImportData(String file_name)
        {
            //try
            //{
                String connection_stirng = SetConnectionString();
                Boolean connection_try = TryConnection(connection_stirng);
                SqlConnection conn = new SqlConnection(connection_stirng);
                if (connection_try)
                {
                    StatusLabel.Text = String.Empty;
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
                        for (int i = 1; i < readLB.Items.Count; i++)
                        {
                            conn.Open();
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
                            conn.Close();
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
                        for (int i = 1; i < readLB.Items.Count; i++)
                        {
                            conn.Open();
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
                            conn.Close();
                        }
                    }

                    else if (ext_file == ".gpx")
                    {
                        stored_proc = "Import_Points";
                        XmlDocument doc = new XmlDocument();
                        doc.Load(file_name);
                        Int32 count_points = doc.GetElementsByTagName("wpt").Count;
                        Int32 current_point = 0;
                        foreach (XmlElement element in doc.GetElementsByTagName("wpt"))
                        {
                            current_point += 1;
                            DateTime date_point = DateTime.Now;
                            XmlNodeList date_time = element.GetElementsByTagName("time");
                            if (date_time.Count > 0)
                            {
                                date_point = DateTime.ParseExact(date_time[0].InnerText, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                date_time = element.GetElementsByTagName("cmt");
                                if (date_time.Count > 0)
                                {
                                    date_point = DateTime.ParseExact(date_time[0].InnerText, "d-MMM-yy H:m:s", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    date_time = element.GetElementsByTagName("desc");
                                    if (date_time.Count > 0)
                                    {
                                        date_point = DateTime.ParseExact(date_time[0].InnerText, "d-MMM-yy H:m:s", System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        StatusLabel.Text = "Статус: не верный формат файла!";
                                        return;
                                    }
                                }
                            }
                            Int64 date_point_ticks = date_point.Ticks;
                            XmlNodeList name = element.GetElementsByTagName("name");
                            XmlNodeList altitude = element.GetElementsByTagName("ele");

                            conn.Open();
                            import_proc = new SqlCommand(stored_proc, conn);
                            import_proc.CommandType = CommandType.StoredProcedure;
                            import_proc.Parameters.AddWithValue("@altitude_point", Convert.ToDouble(altitude[0].InnerText.Replace('.',',')));
                            import_proc.Parameters.AddWithValue("@name_point", name[0].InnerText);
                            import_proc.Parameters.AddWithValue("@longitude", Convert.ToDouble(element.Attributes["lon"].Value.Replace('.', ',')));
                            import_proc.Parameters.AddWithValue("@latitude", Convert.ToDouble(element.Attributes["lat"].Value.Replace('.', ',')));
                            import_proc.Parameters.AddWithValue("@date_time_ticks", date_point_ticks);
                            import_proc.ExecuteNonQuery();
                            conn.Close();
                        }
                        StatusLabel.Text = "Статус: обработка: " + Math.Round(Convert.ToDouble(current_point * 100 / count_points), 2).ToString() + " %";

                    }
                }
                StatusLabel.Text = "Статус: файл обработан!";
            /*}
            catch (Exception ex)
            {
                StatusLabel.Text = "Статус: ошибка в обработке файла: " + ex.Message;
            }*/
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void UploadB_Click(object sender, EventArgs e)
        { 
            if (FileUpload1.HasFile)
            {
                try
                {
                    Random random = new Random();
                    String y = System.DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    String m = System.DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                    String filename = Path.GetFileName(FileUpload1.FileName);
                    String ext_file = Path.GetExtension(filename);
                    if (ext_file != ".slope" && ext_file != ".exposure" && ext_file != ".gpx")
                    {
                        StatusLabel.Text = "Статус: не верное расширение файла!";
                        return;
                    }
                    String new_file_name = DateTime.Now.ToString(@"yyyy-MM-dd-HH-mm-ss-") + random.Next(1, 999) + ext_file;
                    String pathMoth = Server.MapPath("~/Uploads/" + y + "/" + m + "/");
                    String strPath = pathMoth + new_file_name;
                    if (!Directory.Exists(pathMoth))
                    {
                        Directory.CreateDirectory(pathMoth);
                    }
                    FileUpload1.SaveAs(strPath);
                    StatusLabel.Text = "Статус: файл загружен.";
                    ImportData(strPath);
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Статус: ошибка при загрузки и обработки файла: " + ex.Message;
                }
            }
        }
    }
}