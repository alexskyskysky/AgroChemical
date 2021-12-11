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
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using System.Globalization;

namespace agro_proba
{
    public enum TextCase { Nominative/*Кто? Что?*/, Genitive/*Кого? Чего?*/, Dative/*Кому? Чему?*/, Accusative/*Кого? Что?*/, Instrumental/*Кем? Чем?*/, Prepositional/*О ком? О чём?*/ };

    public static class RuDateAndMoneyConverter
    {
        static string[] monthNamesGenitive =
{
    "", "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"
};

        static string zero = "ноль";
        static string firstMale = "один";
        static string firstFemale = "одна";
        static string firstFemaleAccusative = "одну";
        static string firstMaleGenetive = "одно";
        static string secondMale = "два";
        static string secondFemale = "две";
        static string secondMaleGenetive = "двух";
        static string secondFemaleGenetive = "двух";

        static string[] from3till19 =
{
    "", "три", "четыре", "пять", "шесть",
    "семь", "восемь", "девять", "десять", "одиннадцать",
    "двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
    "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
};
        static string[] from3till19Genetive =
{
    "", "трех", "четырех", "пяти", "шести",
    "семи", "восеми", "девяти", "десяти", "одиннадцати",
    "двенадцати", "тринадцати", "четырнадцати", "пятнадцати",
    "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати"
};
        static string[] tens =
{
    "", "двадцать", "тридцать", "сорок", "пятьдесят",
    "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
};
        static string[] tensGenetive =
{
    "", "двадцати", "тридцати", "сорока", "пятидесяти",
    "шестидесяти", "семидесяти", "восьмидесяти", "девяноста"
};
        static string[] hundreds =
{
    "", "сто", "двести", "триста", "четыреста",
    "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"
};
        static string[] hundredsGenetive =
{
    "", "ста", "двухсот", "трехсот", "четырехсот",
    "пятисот", "шестисот", "семисот", "восемисот", "девятисот"
};
        static string[] thousands =
{
    "", "тысяча", "тысячи", "тысяч"
};
        static string[] thousandsAccusative =
{
    "", "тысячу", "тысячи", "тысяч"
};
        static string[] millions =
{
    "", "миллион", "миллиона", "миллионов"
};
        static string[] billions =
{
    "", "миллиард", "миллиарда", "миллиардов"
};
        static string[] trillions =
{
    "", "трилион", "трилиона", "триллионов"
};
        static string[] rubles =
{
    "", "рубль", "рубля", "рублей"
};
        static string[] copecks =
{
    "", "копейка", "копейки", "копеек"
};
        /// <summary>
        /// «07» января 2004 [+ _year(:года)]
        /// </summary>
        /// <param name="_date"></param>
        /// <param name="_year"></param>
        /// <returns></returns>
        public static string DateToTextLong(DateTime _date, string _year)
        {
            return String.Format("«{0}» {1} {2}",
                                    _date.Day.ToString("D2"),
                                    MonthName(_date.Month, TextCase.Genitive),
                                    _date.Year.ToString()) + ((_year.Length != 0) ? " " : "") + _year;
        }

        /// <summary>
        /// «07» января 2004
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static string DateToTextLong(DateTime _date)
        {
            return String.Format("«{0}» {1} {2}",
                                    _date.Day.ToString("D2"),
                                    MonthName(_date.Month, TextCase.Genitive),
                                    _date.Year.ToString());
        }
        /// <summary>
        /// II квартал 2004
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static string DateToTextQuarter(DateTime _date)
        {
            return NumeralsRoman(DateQuarter(_date)) + " квартал " + _date.Year.ToString();
        }
        /// <summary>
        /// 07.01.2004
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static string DateToTextSimple(DateTime _date)
        {
            return String.Format("{0:dd.MM.yyyy}", _date);
        }
        public static int DateQuarter(DateTime _date)
        {
            return (_date.Month - 1) / 3 + 1;
        }

        static bool IsPluralGenitive(int _digits)
        {
            if (_digits >= 5 || _digits == 0)
                return true;

            return false;
        }
        static bool IsSingularGenitive(int _digits)
        {
            if (_digits >= 2 && _digits <= 4)
                return true;

            return false;
        }
        static int lastDigit(long _amount)
        {
            long amount = _amount;

            if (amount >= 100)
                amount = amount % 100;

            if (amount >= 20)
                amount = amount % 10;

            return (int)amount;
        }
        /// <summary>
        /// Десять тысяч рублей 67 копеек
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        public static string CurrencyToTxt(double _amount, bool _firstCapital)
        {
            //Десять тысяч рублей 67 копеек
            long rublesAmount = (long)Math.Floor(_amount);
            long copecksAmount = ((long)Math.Round(_amount * 100)) % 100;
            int lastRublesDigit = lastDigit(rublesAmount);
            int lastCopecksDigit = lastDigit(copecksAmount);

            string s = NumeralsToTxt(rublesAmount, TextCase.Nominative, true, _firstCapital) + " ";

            if (IsPluralGenitive(lastRublesDigit))
            {
                s += rubles[3] + " ";
            }
            else if (IsSingularGenitive(lastRublesDigit))
            {
                s += rubles[2] + " ";
            }
            else
            {
                s += rubles[1] + " ";
            }

            s += String.Format("{0:00} ", copecksAmount);

            if (IsPluralGenitive(lastCopecksDigit))
            {
                s += copecks[3] + " ";
            }
            else if (IsSingularGenitive(lastCopecksDigit))
            {
                s += copecks[2] + " ";
            }
            else
            {
                s += copecks[1] + " ";
            }

            return s.Trim();
        }
        /// <summary>
        /// 10 000 (Десять тысяч) рублей 67 копеек
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        public static string CurrencyToTxtFull(double _amount, bool _firstCapital)
        {
            //10 000 (Десять тысяч) рублей 67 копеек
            long rublesAmount = (long)Math.Floor(_amount);
            long copecksAmount = ((long)Math.Round(_amount * 100)) % 100;
            int lastRublesDigit = lastDigit(rublesAmount);
            int lastCopecksDigit = lastDigit(copecksAmount);

            string s = String.Format("{0:N0} ({1}) ", rublesAmount, NumeralsToTxt(rublesAmount, TextCase.Nominative, true, _firstCapital));

            if (IsPluralGenitive(lastRublesDigit))
            {
                s += rubles[3] + " ";
            }
            else if (IsSingularGenitive(lastRublesDigit))
            {
                s += rubles[2] + " ";
            }
            else
            {
                s += rubles[1] + " ";
            }

            s += String.Format("{0:00} ", copecksAmount);

            if (IsPluralGenitive(lastCopecksDigit))
            {
                s += copecks[3] + " ";
            }
            else if (IsSingularGenitive(lastCopecksDigit))
            {
                s += copecks[2] + " ";
            }
            else
            {
                s += copecks[1] + " ";
            }

            return s.Trim();
        }
        /// <summary>
        /// 10 000 рублей 67 копеек  
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        public static string CurrencyToTxtShort(double _amount, bool _firstCapital)
        {
            //10 000 рублей 67 копеек        
            long rublesAmount = (long)Math.Floor(_amount);
            long copecksAmount = ((long)Math.Round(_amount * 100)) % 100;
            int lastRublesDigit = lastDigit(rublesAmount);
            int lastCopecksDigit = lastDigit(copecksAmount);

            string s = String.Format("{0:N0} ", rublesAmount);

            if (IsPluralGenitive(lastRublesDigit))
            {
                s += rubles[3] + " ";
            }
            else if (IsSingularGenitive(lastRublesDigit))
            {
                s += rubles[2] + " ";
            }
            else
            {
                s += rubles[1] + " ";
            }

            s += String.Format("{0:00} ", copecksAmount);

            if (IsPluralGenitive(lastCopecksDigit))
            {
                s += copecks[3] + " ";
            }
            else if (IsSingularGenitive(lastCopecksDigit))
            {
                s += copecks[2] + " ";
            }
            else
            {
                s += copecks[1] + " ";
            }

            return s.Trim();
        }
        static string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power)
        {
            string s = "";
            int digits = _digits;

            if (digits >= 100)
            {
                s += _hundreds[digits / 100] + " ";
                digits = digits % 100;
            }
            if (digits >= 20)
            {
                s += _tens[digits / 10 - 1] + " ";
                digits = digits % 10;
            }

            if (digits >= 3)
            {
                s += _from3till19[digits - 2] + " ";
            }
            else if (digits == 2)
            {
                s += _second + " ";
            }
            else if (digits == 1)
            {
                s += _first + " ";
            }

            if (_digits != 0 && _power.Length > 0)
            {
                digits = lastDigit(_digits);

                if (IsPluralGenitive(digits))
                {
                    s += _power[3] + " ";
                }
                else if (IsSingularGenitive(digits))
                {
                    s += _power[2] + " ";
                }
                else
                {
                    s += _power[1] + " ";
                }
            }

            return s;
        }

        /// <summary>
        /// реализовано для падежей: именительный (nominative), родительный (Genitive),  винительный (accusative)
        /// </summary>
        /// <param name="_sourceNumber"></param>
        /// <param name="_case"></param>
        /// <param name="_isMale"></param>
        /// <param name="_firstCapital"></param>
        /// <returns></returns>
        public static string NumeralsToTxt(long _sourceNumber, TextCase _case, bool _isMale, bool _firstCapital)
        {
            string s = "";
            long number = _sourceNumber;
            int remainder;
            int power = 0;

            if ((number >= (long)Math.Pow(10, 15)) || number < 0)
            {
                return "";
            }

            while (number > 0)
            {
                remainder = (int)(number % 1000);
                number = number / 1000;

                switch (power)
                {
                    case 12:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions) + s;
                        break;
                    case 9:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions) + s;
                        break;
                    case 6:
                        s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions) + s;
                        break;
                    case 3:
                        switch (_case)
                        {
                            case TextCase.Accusative:
                                s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative) + s;
                                break;
                            default:
                                s = MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands) + s;
                                break;
                        }
                        break;
                    default:
                        string[] powerArray = { };
                        switch (_case)
                        {
                            case TextCase.Genitive:
                                s = MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, _isMale ? secondMaleGenetive : secondFemaleGenetive, _isMale ? firstMaleGenetive : firstFemale, powerArray) + s;
                                break;
                            case TextCase.Accusative:
                                s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemaleAccusative, powerArray) + s;
                                break;
                            default:
                                s = MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemale, powerArray) + s;
                                break;
                        }
                        break;
                }

                power += 3;
            }

            if (_sourceNumber == 0)
            {
                s = zero + " ";
            }

            if (s != "" && _firstCapital)
                s = s.Substring(0, 1).ToUpper() + s.Substring(1);

            return s.Trim();
        }
        public static string NumeralsDoubleToTxt(double _sourceNumber, int _decimal, TextCase _case, bool _firstCapital)
        {
            long decNum = (long)Math.Round(_sourceNumber * Math.Pow(10, _decimal)) % (long)(Math.Pow(10, _decimal));

            string s = String.Format(" {0} целых {1} сотых", NumeralsToTxt((long)_sourceNumber, _case, true, _firstCapital),
                                                  NumeralsToTxt((long)decNum, _case, true, false));
            return s.Trim();
        }
        /// <summary>
        /// название м-ца
        /// </summary>
        /// <param name="_month">с единицы</param>
        /// <param name="_case"></param>
        /// <returns></returns>
        public static string MonthName(int _month, TextCase _case)
        {
            string s = ""; 

            if (_month > 0 && _month <= 12)
            {
                switch (_case)
                {
                    case TextCase.Genitive:
                        s = monthNamesGenitive[_month];
                        break;
                }
            }

            return s;
        }
        public static string NumeralsRoman(int _number)
        {
            string s = ""; 

            switch (_number)
            {
                case 1: s = "I"; break;
                case 2: s = "II"; break;
                case 3: s = "III"; break;
                case 4: s = "IV"; break;
            }

            return s;
        }
    }

    public partial class Contracts : System.Web.UI.Page
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
        public String selectCommContracts, selectCommPhones, selectCommOrganizations, selectCommOrganization, selectCommRegions, selectCommContractStatus,
                      selectCommContractSubjects, selectCommOrganizationsFull, selectCommSigners, selectCommLettersAttorney;
        public login_data user_reg_data;
        public SqlDataAdapter adapterContracts, adapterPhones, adapterOrganization, adapterRegions, adapterContractStatus, adapterContractSubjects, adapterSigners,
                              adapterLettersAttorney;

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

        public String LongFIOToShortFIO(String long_fio)
        {
            String short_fio = String.Empty;
            if (long_fio != null && long_fio != "null" && long_fio != String.Empty)
            {
                for (int i = 0; i < long_fio.Trim().Split(' ').Length; i++)
                {
                    if (long_fio.Trim().Split(' ')[i] != null && long_fio.Trim().Split(' ')[i] != "null" && long_fio.Trim().Split(' ')[i] != String.Empty)
                    {
                        if (i == 0)
                        {
                            short_fio += long_fio.Trim().Split(' ')[i];
                        }
                        else
                        {
                            short_fio += (" " + long_fio.Trim().Split(' ')[i][0] + ".");
                        }
                    }
                }
            }

            return short_fio;
        }

        public void ChangeBookmark(Word._Document doc, String BmName, String value)
        {
            if (FindBookmark(doc, BmName))
            {
                Word.Range range;
                range = doc.Bookmarks[BmName].Range;
                range.Text = value;
                doc.Bookmarks.Add(BmName, range);
            }
        }

        public Boolean FindBookmark(Word._Document doc, String BmName)
        {
            Boolean result = false;

            for (int i = 0; i < doc.Bookmarks.Count; i++)
            {
                if (doc.Bookmarks.Exists(BmName))
                {
                    result = true;
                    return result;
                }
            }

            return result;
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
                selectCommContracts = "SELECT * FROM View_Contracts";
                selectCommPhones = "SELECT * FROM Phones";
                selectCommOrganizations = "SELECT * FROM View_Organizations";
                selectCommOrganization = "SELECT * FROM ViewOrganization";
                selectCommOrganizationsFull = "SELECT * FROM Organization";

                selectCommRegions = "SELECT * FROM Region WHERE id_territory = 31";
                selectCommContractStatus = "SELECT * FROM ContractStatus";
                selectCommContractSubjects = "SELECT * FROM Contract_Subjects";

                selectCommSigners = "SELECT * FROM View_Signers";
                selectCommLettersAttorney = "SELECT * FROM Letters_attorney";

                //выполнение при первой загрузке
                if (!IsPostBack)
                {
                    FillContracts();
                }

            }

            //авторизация
            AcceptLoginB.Listeners.Click.Handler = "App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());";
            UsernameTF.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
            UserPassword.Listeners.KeyPress.Handler = "if(e.keyCode==13){App.direct.GetAuthorization(#{UsernameTF}.getValue(),#{UserPassword}.getValue());};";
            ExitB.Listeners.Click.Handler = "App.direct.ExitUser();";

            //договоры
            SignerCB.Listeners.Change.Handler = "App.direct.FillLettersAttorney(this.getValue());";
            ClearContractsFiltersB.Listeners.Click.Handler = "this.up('grid').filters.clearFilters();";
            ContractsS.Listeners.DataChanged.Handler = "App.direct.GetTotalResult(#{ContractsGP}.getRowsValues());";
            ContractsS.Listeners.FilterChange.Handler = "App.direct.GetTotalResult(#{ContractsGP}.getRowsValues());";
            ContractsGP.Listeners.CellDblClick.Handler = "App.direct.OpenContract(record.data.number_contract, {isUpload: true});"; // , {isUpload: true}
            AddContractB.Listeners.Click.Handler = "App.direct.ShowAddContractW();";
            EditContractB.Listeners.Click.Handler = "if(#{ContractsGP}.getView().getSelectionModel().getSelection().length > 0) { var record = #{ContractsGP}.getView().getSelectionModel().getSelection()[0]; #{StatusContractCB}.setValue(record.data.id_contract_status); #{RegionContractCB}.setValue(record.data.id_region); " +
                                                    "#{ContractSubjectCB}.setValue(record.data.id_contract_subject); #{SignerCB}.setValue(record.data.id_signer); #{LetterAttorneyCB}.setValue(record.data.id_letter_attorney);" +
                                                    "\nApp.direct.ShowEditContractW(record.data.id_contract, " +
                                                    "record.data.id_organization, record.data.title_organization, record.data.index_number, record.data.number_contract, " +
                                                    "record.data.client_position, record.data.client_leader, record.data.basis_document, record.data.contract_start_date, record.data.contract_end_date, " +
                                                    "record.data.date_finish, record.data.date_selecting, record.data.date_fulfilment, record.data.area, record.data.payment_days, record.data.prepayment, record.data.balance, " +
                                                    "record.data.payment, record.data.client_price, record.data.federal_price, record.data.nds_price, record.data.total_price, record.data.id_signer);} else {alert('Договор не выбран!');}";
            DeleteContractB.Listeners.Click.Handler = "var record = #{ContractsGP}.getView().getSelectionModel().getSelection()[0]; App.direct.DeleteContractMessage(record.data.id_contract, #{ContractsGP}.getRowsValues());";

            AcceptAddContractB.Listeners.Click.Handler = "App.direct.AddContract(#{IndexNumberNF}.getValue(), #{RegionContractCB}.getValue(), #{IdOrganizationContractTF}.getValue(), #{TitleOrganizationContractTF}.getValue(), #{ContractSubjectCB}.getValue(), " +
                                                                                "#{ClientPositionTF}.getValue(), #{ClientLeaderTF}.getValue(), #{BasicDocumentTF}.getValue(), #{SignerCB}.getValue(), #{LetterAttorneyCB}.getValue(), #{ContractStartDateDF}.getValue(), #{ContractEndDateDF}.getValue(), #{AreaNF}.getValue(), #{PaymentDaysNF}.getValue(), " +
                                                                                "#{TotalPriceNF}.getValue(), #{NdsPriceNF}.getValue(), #{ClientPriceNF}.getValue(), #{FederalPriceNF}.getValue(), #{PrepaymentNF}.getValue(), #{BalanceNF}.getValue(), " +
                                                                                "#{PaymentNF}.getValue(), #{DateFinishDF}.getValue(), #{SelectingDateDF}.getValue(), #{DateFulfilmentDF}.getValue(), #{StatusContractCB}.getValue());";
            AcceptEditContractB.Listeners.Click.Handler = "App.direct.EditContract(#{IdContractTF}.getValue(), #{IndexNumberNF}.getValue(), #{RegionContractCB}.getValue(), #{IdOrganizationContractTF}.getValue(), #{TitleOrganizationContractTF}.getValue(), #{ContractSubjectCB}.getValue(), " +
                                                                                "#{ClientPositionTF}.getValue(), #{ClientLeaderTF}.getValue(), #{BasicDocumentTF}.getValue(), #{SignerCB}.getValue(), #{LetterAttorneyCB}.getValue(), #{ContractStartDateDF}.getValue(), #{ContractEndDateDF}.getValue(), #{AreaNF}.getValue(), #{PaymentDaysNF}.getValue(), " +
                                                                                "#{TotalPriceNF}.getValue(), #{NdsPriceNF}.getValue(), #{ClientPriceNF}.getValue(), #{FederalPriceNF}.getValue(), #{PrepaymentNF}.getValue(), #{BalanceNF}.getValue(), " +
                                                                                "#{PaymentNF}.getValue(), #{DateFinishDF}.getValue(), #{SelectingDateDF}.getValue(), #{DateFulfilmentDF}.getValue(), #{StatusContractCB}.getValue());";
            CancelAddEditContractB.Listeners.Click.Handler = "#{AddEditContractW}.close();";

            StatusContractCB.Listeners.Change.Handler = "App.direct.SetDateFinish(this.value);";
            ClientPriceNF.Listeners.Change.Handler = "App.direct.SetBalance(this.value, #{PaymentNF}.getValue());";
            PaymentNF.Listeners.Change.Handler = "App.direct.SetBalance(#{ClientPriceNF}.getValue(), this.value);";

            ContractsM.Fields.Get("total_price").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("nds_price").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("federal_price").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("client_price").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("prepayment").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("payment").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";
            ContractsM.Fields.Get("balance").Convert.Handler = "if(value == null) {return '';} else {return value = value.toFixed(2);}";

            ContractsGP.ViewConfig.GetRowClass.Fn = "getRowClass";

            //очистка ячеек по нажатию DELETE
            RegionContractCB.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            ClientPositionTF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            BasicDocumentTF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            ContractEndDateDF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            DateFinishDF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            DateFulfilmentDF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            AreaNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            SelectingDateDF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            TotalPriceNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            FederalPriceNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            ClientPriceNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            PrepaymentNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            PaymentDaysNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            NdsPriceNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            PaymentNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";
            BalanceNF.Listeners.KeyPress.Handler = "if(e.keyCode==46){ this.clear();}";

            //организации
            SelectOrganizationB.Listeners.Click.Handler = "App.direct.ShowSelectOrganizationW();";
            CancelSelectOrganizationB.Listeners.Click.Handler = "#{OrganizationW}.close();";
            OrganizationGP.Listeners.CellDblClick.Handler = "App.direct.SetContractOrgValues(record.data.id_organization, record.data.title_organization);";
            AddOrganizationB.Listeners.Click.Handler = "App.direct.ShowAddOrganizationW();";
            EditOrganizationB.Listeners.Click.Handler = "var record = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.ShowEditOrganizationW(record.data.id_organization, record.data.code_region, record.data.code_organization, record.data.title_organization, record.data.full_title_organization, record.data.leader, record.data.basis_document, record.data.chief_agronomist, record.data.legal_address, record.data.mailing_address, record.data.email_organization, record.data.inn_organization, record.data.okato_organization, record.data.oktmo_organization, record.data.kpp_organization, record.data.ogrn_organization, record.data.okved_organization, record.data.okpo_organization, record.data.pay_account, record.data.full_bank_name, record.data.bik, record.data.bank_correspond_account);";
            DeleteOrganizationB.Listeners.Click.Handler = "var record = #{OrganizationGP}.getView().getSelectionModel().getSelection()[0]; App.direct.DeleteOrganizationMessage(record.data.id_organization);";
            AcceptAddOrganizationB.Listeners.Click.Handler = "App.direct.AddOrganization(#{TitleOrganizationTF}.getValue(), #{FullTitleOrganizationTF}.getValue(), #{LeaderTF}.getValue(), #{BasisDocumentTF}.getValue(), #{ChiefAgronomistTF}.getValue(), #{LegalAddressTF}.getValue(), #{MailingAddressTF}.getValue(), #{EMailOrganizationTF}.getValue(), #{OKATOOrganizationTF}.getValue(), #{OKTMOOrganizationTF}.getValue(), #{INNOrganizationTF}.getValue(), #{KPPOrganizationTF}.getValue(), #{OGRNOrganizationTF}.getValue(), #{OKVEDOrganizationTF}.getValue(), #{OKPOOrganizationTF}.getValue(), #{PayAccountTF}.getValue(), #{FullBankNameTF}.getValue(), #{BIKTF}.getValue(), #{BankCorrespondingAccountTF}.getValue()); #{AddEditOrganizationW}.close();";
            AcceptEditOrganizationB.Listeners.Click.Handler = "App.direct.EditOrganization(#{IdOrganizationTF}.getValue(), #{TitleOrganizationTF}.getValue(), #{FullTitleOrganizationTF}.getValue(), #{LeaderTF}.getValue(), #{BasisDocumentTF}.getValue(), #{ChiefAgronomistTF}.getValue(), #{LegalAddressTF}.getValue(), #{MailingAddressTF}.getValue(), #{EMailOrganizationTF}.getValue(), #{OKATOOrganizationTF}.getValue(), #{OKTMOOrganizationTF}.getValue(), #{INNOrganizationTF}.getValue(), #{KPPOrganizationTF}.getValue(), #{OGRNOrganizationTF}.getValue(), #{OKVEDOrganizationTF}.getValue(), #{OKPOOrganizationTF}.getValue(), #{PayAccountTF}.getValue(), #{FullBankNameTF}.getValue(), #{BIKTF}.getValue(), #{BankCorrespondingAccountTF}.getValue()); #{AddEditOrganizationW}.close();";
            CancelAddEditOrganizationB.Listeners.Click.Handler = "#{AddEditOrganizationW}.close();";
            AddEditOrganizationW.Listeners.Close.Handler = "App.direct.CloseAddEditOrganization(#{IdOrganizationTF}.getValue());";
            CancelSelectOrganizationB.Listeners.Click.Handler = "#{OrganizationW}.close();";
        }

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

        [DirectMethod]
        public void DeleteOrganizationMessage(String id_org)
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter("SELECT * FROM Department WHERE id_organization=" + id_org, conn);
                adapterOrganization.Fill(indexDS, "Department");
                //OrganizationGP.GetSelectionModel().Select(0);
                //OrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_organization));
                //FillDepartment(current_id_organization);
                conn.Close();
            }

            int records = indexDS.Tables["Department"].Rows.Count;
            if (records <= 0)
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
                    FillOrganization();
                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись удалена!", IconCls = "icon-accept", Clear2 = true });
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
        public void FillContracts()
        {
            if (connection_try)
            {
                conn.Open();
                adapterContracts = new SqlDataAdapter(selectCommContracts, conn);
                adapterContracts.Fill(indexDS, "Contracts");
                indexDV = new System.Data.DataView(indexDS.Tables["Contracts"]);
                ContractsS.DataSource = indexDV;
                ContractsS.DataBind();
                conn.Close();
            }
        }

        public void FillRegions()
        {
            if (connection_try)
            {
                conn.Open();
                adapterRegions = new SqlDataAdapter(selectCommRegions, conn);
                adapterRegions.Fill(indexDS, "Regions");
                indexDV = new System.Data.DataView(indexDS.Tables["Regions"]);
                RegionContractS.DataSource = indexDV;
                RegionContractS.DataBind();
                conn.Close();
            }
        }

        public void FillContractStatus()
        {
            if (connection_try)
            {
                conn.Open();
                adapterContractStatus = new SqlDataAdapter(selectCommContractStatus, conn);
                adapterContractStatus.Fill(indexDS, "ContractStatus");
                indexDV = new System.Data.DataView(indexDS.Tables["ContractStatus"]);
                StatusContractS.DataSource = indexDV;
                StatusContractS.DataBind();
                conn.Close();
            }
        }

        public void FillContractSubjects()
        {
            if (connection_try)
            {
                conn.Open();
                adapterContractSubjects = new SqlDataAdapter(selectCommContractSubjects, conn);
                adapterContractSubjects.Fill(indexDS, "ContractSubjects");
                indexDV = new System.Data.DataView(indexDS.Tables["ContractSubjects"]);
                ContractSubjectS.DataSource = indexDV;
                ContractSubjectS.DataBind();
                conn.Close();
            }
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

        [DirectMethod]
        public void GetTotalResult(String data)
        {
            if (data != "null" && data != null && data != String.Empty)
            {
                List<Dictionary<String, String>> records_contracts = JSON.Deserialize<List<Dictionary<String, String>>>(data);
                if (records_contracts.Count > 0)
                {
                    Int32 count_contracts = 0;
                    Double sum_area = 0;
                    Double sum_price = 0;
                    Double sum_balance_price = 0;
                    for (int r = 0; r < records_contracts.Count; r++)
                    {
                        count_contracts += 1;
                        sum_area += Convert.ToDouble(NotNull(records_contracts[r]["area"]));
                        sum_price += Convert.ToDouble(NotNull(records_contracts[r]["client_price"]));
                        sum_balance_price += Convert.ToDouble(NotNull(records_contracts[r]["balance"]));
                    }
                    CountContractsTF.Text = count_contracts.ToString();
                    SumAreaTF.Text = sum_area.ToString();
                    SumClientPriceTF.Text = sum_price.ToString();
                    SumBalancePriceTF.Text = sum_balance_price.ToString();
                }
            }
        }

        public void SaveFile(String id_contract, Int32 add_edit)
        {
            if (connection_try)
            {
                conn.Open();
                String get_contract_data_str = "SELECT * FROM View_Contracts WHERE id_contract=" + id_contract;
                SqlDataAdapter get_contract_data = new SqlDataAdapter(get_contract_data_str, conn);
                get_contract_data.Fill(indexDS, "Contract");

                conn.Close();
                if (CheckRowsCount(indexDS, "Contract"))
                {
                    String number_contract = indexDS.Tables["Contract"].Rows[0]["index_number"].ToString() + "-" +
                                   indexDS.Tables["Contract"].Rows[0]["code_contract_subject"].ToString() + "/" +
                                   indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString().Split(' ')[0];

                    String fName = indexDS.Tables["Contract"].Rows[0]["index_number"].ToString() + "-" +
                                   indexDS.Tables["Contract"].Rows[0]["code_contract_subject"].ToString() + "_" +
                                   indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString().Split(' ')[0] + ".docx";

                    Word._Application oWord = new Word.Application();
                    try
                    {
                        Word._Document oDoc;
                        if (File.Exists(Server.MapPath("~/Contracts/" + fName)) && add_edit == 1)
                        {
                            oDoc = oWord.Documents.Add(Server.MapPath("~/Contracts/" + fName));
                        }
                        else
                        {
                            add_edit = 0;
                            oDoc = oWord.Documents.Add(Server.MapPath("~/Template/" + indexDS.Tables["Contract"].Rows[0]["code_contract_subject"].ToString() + ".dotx"));
                        }
                        oDoc.Activate();

                        //Ввод данных в шаблон
                        //oDoc.Bookmarks["full_bank_name"].Range.Text = indexDS.Tables["Contract"].Rows[0]["full_bank_name"].ToString();
                        if (add_edit == 0)
                        {
                            ChangeBookmark(oDoc, "area", indexDS.Tables["Contract"].Rows[0]["area"].ToString());

                            ChangeBookmark(oDoc, "bank_correspond_account", "К/С " + indexDS.Tables["Contract"].Rows[0]["bank_correspond_account"].ToString());
                            ChangeBookmark(oDoc, "basis_document", indexDS.Tables["Contract"].Rows[0]["basis_document"].ToString());
                            ChangeBookmark(oDoc, "bik", "БИК " + indexDS.Tables["Contract"].Rows[0]["bik"].ToString());
                            ChangeBookmark(oDoc, "client_position", indexDS.Tables["Contract"].Rows[0]["client_position"].ToString());
                            ChangeBookmark(oDoc, "client_position_info", indexDS.Tables["Contract"].Rows[0]["client_position"].ToString());

                            Double client_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["client_price"].ToString()));
                            if (client_price > 0)
                            {
                                ChangeBookmark(oDoc, "client_price", RuDateAndMoneyConverter.CurrencyToTxtFull(client_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "client_price", "");
                            }

                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "contract_start_date", RuDateAndMoneyConverter.DateToTextLong(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString())));
                            }
                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["contract_end_date"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "contract_end_date", RuDateAndMoneyConverter.DateToTextLong(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["contract_end_date"].ToString())));
                            }

                            ChangeBookmark(oDoc, "email_address", indexDS.Tables["Contract"].Rows[0]["email_address"].ToString());

                            Double federal_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["federal_price"].ToString()));
                            if (federal_price > 0)
                            {
                                ChangeBookmark(oDoc, "federal_price", RuDateAndMoneyConverter.CurrencyToTxtFull(federal_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "federal_price", "");
                            }

                            ChangeBookmark(oDoc, "full_bank_name", indexDS.Tables["Contract"].Rows[0]["full_bank_name"].ToString());
                            ChangeBookmark(oDoc, "inn_organization", "ИНН " + indexDS.Tables["Contract"].Rows[0]["inn_organization"].ToString());
                            ChangeBookmark(oDoc, "kpp_organization", "КПП " + indexDS.Tables["Contract"].Rows[0]["kpp_organization"].ToString());
                            ChangeBookmark(oDoc, "oktmo_organization", "ОКТМО " + indexDS.Tables["Contract"].Rows[0]["oktmo_organization"].ToString());
                            ChangeBookmark(oDoc, "client_leader", indexDS.Tables["Contract"].Rows[0]["client_leader"].ToString());
                            ChangeBookmark(oDoc, "client_leader_info", LongFIOToShortFIO(indexDS.Tables["Contract"].Rows[0]["client_leader"].ToString()));
                            ChangeBookmark(oDoc, "legal_address", indexDS.Tables["Contract"].Rows[0]["legal_address"].ToString());
                            ChangeBookmark(oDoc, "letter_attorney", indexDS.Tables["Contract"].Rows[0]["letter_attorney"].ToString());

                            Double nds_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["nds_price"].ToString()));
                            if (nds_price > 0)
                            {
                                ChangeBookmark(oDoc, "nds_price", RuDateAndMoneyConverter.CurrencyToTxtShort(nds_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "nds_price", "");
                            }

                            ChangeBookmark(oDoc, "number_contract", number_contract);
                            ChangeBookmark(oDoc, "pay_account", "Р/С " + indexDS.Tables["Contract"].Rows[0]["pay_account"].ToString());

                            String payment_days = "10";
                            if (NotNull(indexDS.Tables["Contract"].Rows[0]["payment_days"].ToString()) != "0")
                            {
                                payment_days = indexDS.Tables["Contract"].Rows[0]["payment_days"].ToString();
                            }
                            ChangeBookmark(oDoc, "payment_days_1", payment_days);
                            ChangeBookmark(oDoc, "payment_days_2", payment_days);
                            ChangeBookmark(oDoc, "payment_days_3", payment_days);

                            ChangeBookmark(oDoc, "phone", "Тел. " + indexDS.Tables["Contract"].Rows[0]["phone"].ToString());

                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["date_selecting"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "date_selecting", RuDateAndMoneyConverter.DateToTextQuarter(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["date_selecting"].ToString())));
                            }
                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["date_fulfilment"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "date_fulfilment", RuDateAndMoneyConverter.DateToTextQuarter(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["date_fulfilment"].ToString())));
                            }

                            Double prepayment = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["prepayment"].ToString()));
                            if (prepayment > 0)
                            {
                                ChangeBookmark(oDoc, "prepayment", RuDateAndMoneyConverter.CurrencyToTxtFull(prepayment, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "prepayment", "");
                            }

                            ChangeBookmark(oDoc, "signer_full_name", indexDS.Tables["Contract"].Rows[0]["signer_full_name"].ToString());
                            ChangeBookmark(oDoc, "signer_full_name_info", LongFIOToShortFIO(indexDS.Tables["Contract"].Rows[0]["signer_full_name"].ToString()));
                            ChangeBookmark(oDoc, "title_organization", indexDS.Tables["Contract"].Rows[0]["title_organization"].ToString());
                            ChangeBookmark(oDoc, "title_organization_info", indexDS.Tables["Contract"].Rows[0]["title_organization"].ToString());
                            ChangeBookmark(oDoc, "title_region", indexDS.Tables["Contract"].Rows[0]["title_region"].ToString().Replace("ий", "ого").Replace("ый", "ого"));

                            Double total_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["total_price"].ToString()));
                            if (total_price > 0)
                            {
                                ChangeBookmark(oDoc, "total_price", RuDateAndMoneyConverter.CurrencyToTxtFull(total_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "total_price", "");
                            }
                        }
                        else
                        {
                            ChangeBookmark(oDoc, "area", indexDS.Tables["Contract"].Rows[0]["area"].ToString());

                            ChangeBookmark(oDoc, "bank_correspond_account", "К/С " + indexDS.Tables["Contract"].Rows[0]["bank_correspond_account"].ToString());
                            ChangeBookmark(oDoc, "basis_document", indexDS.Tables["Contract"].Rows[0]["basis_document"].ToString());
                            ChangeBookmark(oDoc, "bik", "БИК " + indexDS.Tables["Contract"].Rows[0]["bik"].ToString());

                            Double client_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["client_price"].ToString()));
                            if (client_price > 0)
                            {
                                ChangeBookmark(oDoc, "client_price", RuDateAndMoneyConverter.CurrencyToTxtFull(client_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "client_price", "");
                            }

                            //dateetetete "08.03.2017 0:00:00"
                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "contract_start_date", RuDateAndMoneyConverter.DateToTextLong(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["contract_start_date"].ToString())));
                            }
                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["contract_end_date"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "contract_end_date", RuDateAndMoneyConverter.DateToTextLong(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["contract_end_date"].ToString())));
                            }

                            ChangeBookmark(oDoc, "email_address", indexDS.Tables["Contract"].Rows[0]["email_address"].ToString());

                            Double federal_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["federal_price"].ToString()));
                            if (federal_price > 0)
                            {
                                ChangeBookmark(oDoc, "federal_price", RuDateAndMoneyConverter.CurrencyToTxtFull(federal_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "federal_price", "");
                            }

                            ChangeBookmark(oDoc, "full_bank_name", indexDS.Tables["Contract"].Rows[0]["full_bank_name"].ToString());
                            ChangeBookmark(oDoc, "inn_organization", "ИНН " + indexDS.Tables["Contract"].Rows[0]["inn_organization"].ToString());
                            ChangeBookmark(oDoc, "kpp_organization", "КПП " + indexDS.Tables["Contract"].Rows[0]["kpp_organization"].ToString());
                            ChangeBookmark(oDoc, "oktmo_organization", "ОКТМО " + indexDS.Tables["Contract"].Rows[0]["oktmo_organization"].ToString());
                            ChangeBookmark(oDoc, "legal_address", indexDS.Tables["Contract"].Rows[0]["legal_address"].ToString());
                            ChangeBookmark(oDoc, "letter_attorney", indexDS.Tables["Contract"].Rows[0]["letter_attorney"].ToString());

                            Double nds_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["nds_price"].ToString()));
                            if (nds_price > 0)
                            {
                                ChangeBookmark(oDoc, "nds_price", RuDateAndMoneyConverter.CurrencyToTxtShort(nds_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "nds_price", "");
                            }

                            ChangeBookmark(oDoc, "number_contract", number_contract);
                            ChangeBookmark(oDoc, "pay_account", "Р/С " + indexDS.Tables["Contract"].Rows[0]["pay_account"].ToString());

                            String payment_days = "10";
                            if (NotNull(indexDS.Tables["Contract"].Rows[0]["payment_days"].ToString()) != "0")
                            {
                                payment_days = indexDS.Tables["Contract"].Rows[0]["payment_days"].ToString();
                            }
                            ChangeBookmark(oDoc, "payment_days_1", payment_days);
                            ChangeBookmark(oDoc, "payment_days_2", payment_days);
                            ChangeBookmark(oDoc, "payment_days_3", payment_days);

                            ChangeBookmark(oDoc, "phone", "Тел. " + indexDS.Tables["Contract"].Rows[0]["phone"].ToString());

                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["date_selecting"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "date_selecting", RuDateAndMoneyConverter.DateToTextQuarter(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["date_selecting"].ToString())));
                            }
                            if (NotNullText(indexDS.Tables["Contract"].Rows[0]["date_fulfilment"].ToString()) != "0")
                            {
                                ChangeBookmark(oDoc, "date_fulfilment", RuDateAndMoneyConverter.DateToTextQuarter(DateTime.Parse(indexDS.Tables["Contract"].Rows[0]["date_fulfilment"].ToString())));
                            }

                            Double prepayment = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["prepayment"].ToString()));
                            if (prepayment > 0)
                            {
                                ChangeBookmark(oDoc, "prepayment", RuDateAndMoneyConverter.CurrencyToTxtFull(prepayment, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "prepayment", "");
                            }

                            ChangeBookmark(oDoc, "title_organization", indexDS.Tables["Contract"].Rows[0]["title_organization"].ToString());
                            ChangeBookmark(oDoc, "title_organization_info", indexDS.Tables["Contract"].Rows[0]["title_organization"].ToString());
                            ChangeBookmark(oDoc, "title_region", indexDS.Tables["Contract"].Rows[0]["title_region"].ToString().Replace("ий", "ого").Replace("ый", "ого"));

                            Double total_price = Convert.ToDouble(NotNull(indexDS.Tables["Contract"].Rows[0]["total_price"].ToString()));
                            if (total_price > 0)
                            {
                                ChangeBookmark(oDoc, "total_price", RuDateAndMoneyConverter.CurrencyToTxtFull(total_price, false));
                            }
                            else
                            {
                                ChangeBookmark(oDoc, "total_price", "");
                            }
                        }
                        oDoc.SaveAs(FileName: Server.MapPath("~/Contracts/" + fName));
                        oDoc.Close();
                        oWord.Quit();
                    }
                    catch (Exception exc)
                    {
                        oWord.Quit();
                    }
                }
            }
        }

        [DirectMethod]
        public void OpenContract(String number_contract)
        {
            try
            {
                if (number_contract != null && number_contract != String.Empty)
                {
                    String sFileName = Server.MapPath("~/Contracts/" + number_contract.Replace('/', '_') + ".docx");
                    FileInfo fileDet = new FileInfo(sFileName);
                    Response.Clear();
                    Response.Charset = "cs1251";
                    Response.ContentEncoding = Encoding.GetEncoding(1251);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileDet.Name);
                    Response.AddHeader("Content-Length", fileDet.Length.ToString());
                    Response.ContentType = "application/ms-word";
                    Response.WriteFile(fileDet.FullName);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [DirectMethod]
        public String NullToEmpty(String value)
        {
            if (value == "null" || value == null || value == "0") { value = String.Empty; }
            return value;
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

        private DateTime ParseDateToDateTime(String datetime)
        {
            DateTime dt = DateTime.Now;
            if (datetime != String.Empty && datetime != null && datetime != "null")
            {
                datetime = datetime.Replace("\"", "");
                String format_date_time = "yyyy-MM-ddTHH:mm:ss";
                if (datetime.Split('.').Length > 1) { format_date_time = "yyyy-MM-ddTHH:mm:ss.fff"; }
                if (datetime.Split('T').Length == 1) { format_date_time = format_date_time.Replace('T', ' '); }
                dt = DateTime.ParseExact(datetime, format_date_time, System.Globalization.CultureInfo.InvariantCulture);
            }
            return dt;
        }

        private string ParseDateFormatForDoc(String datetime)
        {
            try
            {
                string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
                string date = "";
                string day = datetime.Split(' ')[0].Split('.')[0];
                Int32 month_index = Convert.ToInt32(datetime.Split(' ')[0].Split('.')[1]) - 1;
                string month = months[month_index];
                string year = datetime.Split(' ')[0].Split('.')[2];
                date = "«" + day + "» " + month + " " + year + " г.";
                return date;
            }
            catch { return ""; }

        }

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

        [DirectMethod]
        public void ShowAddContractW()
        {
            AddEditContractW.Title = "Новый договор";
            AddEditContractW.Icon = Icon.PageAdd;
            AcceptAddContractB.Hidden = false;
            AcceptEditContractB.Hidden = true;
            NumberContractTF.Hidden = true;

            IdContractTF.Text = String.Empty;
            IndexNumberNF.Clear();
            NumberContractTF.Text = String.Empty;
            StatusContractCB.Clear();
            RegionContractCB.Clear();
            ContractSubjectCB.Clear();
            IdOrganizationContractTF.Text = String.Empty;
            TitleOrganizationContractTF.Text = String.Empty;
            ClientPositionTF.Text = String.Empty;
            ClientLeaderTF.Text = String.Empty;
            BasicDocumentTF.Text = String.Empty;
            SignerCB.Clear();
            LetterAttorneyCB.Clear();
            ContractStartDateDF.Text = String.Empty;
            ContractEndDateDF.Text = String.Empty;
            DateFinishDF.Text = String.Empty;
            DateFulfilmentDF.Text = String.Empty;
            SelectingDateDF.Text = String.Empty;
            AreaNF.Clear();
            PaymentDaysNF.Clear();
            PrepaymentNF.Clear();
            BalanceNF.Clear();
            ClientPriceNF.Clear();
            FederalPriceNF.Clear();
            PaymentNF.Clear();
            TotalPriceNF.Clear();
            NdsPriceNF.Clear();

            FillRegions();
            FillContractStatus();
            FillContractSubjects();
            FillContractSigners();

            AddEditContractW.Show();
        }

        [DirectMethod]
        public void ShowEditContractW(String id_contract, String id_organization, String title_organization, String index_number,
                                      String number_contract, String client_position, String client_leader, String basic_document,
                                      String contract_start_date, String contract_end_date, String date_finish, String date_selecting, String date_fulfilment, String area,
                                      String payment_days, String prepayment, String balance, String payment, String client_price, String federal_price, String nds_price,
                                      String total_price, String id_signer)
        {
            if (!user_reg_data.edit) { return; }
            if (connection_try)
            {
                /*FlagEditing organizationFE = new FlagEditing(conn, "Organization", Convert.ToInt32(id_organization));
                if (organizationFE.GetFlag())
                {
                    X.Msg.Notify("Уведомление", "Организация с кодом " + code_org + " редактируется другим пользователем!").Show();
                    return;
                }*/
                //organizationFE.SetFlag();
                AddEditContractW.Title = "Редактировать договор";
                AddEditOrganizationW.Icon = Icon.PageEdit;
                AcceptAddContractB.Hidden = true;
                AcceptEditContractB.Hidden = false;
                NumberContractTF.Hidden = false;

                IdContractTF.Text = NullToEmpty(id_contract);
                if (NullToEmpty(index_number) != String.Empty)
                {
                    IndexNumberNF.Text = index_number.Replace('.', ',');
                }
                else { IndexNumberNF.Clear(); }
                NumberContractTF.Text = NullToEmpty(number_contract);
                /*StatusContractCB.Clear();
                RegionContractCB.Clear();
                ContractSubjectCB.Clear();*/
                IdOrganizationContractTF.Text = NullToEmpty(id_organization);
                TitleOrganizationContractTF.Text = NullToEmpty(title_organization);

                ClientPositionTF.Text = NullToEmpty(client_position);
                ClientLeaderTF.Text = NullToEmpty(client_leader);
                BasicDocumentTF.Text = NullToEmpty(basic_document);

                ContractStartDateDF.Clear();
                if (NullToEmpty(contract_start_date) != String.Empty)
                {
                    ContractStartDateDF.SelectedValue = ParseDateToDateTime(contract_start_date);
                }
                ContractEndDateDF.Clear();
                if (NullToEmpty(contract_end_date) != String.Empty)
                {
                    ContractEndDateDF.SelectedValue = ParseDateToDateTime(contract_end_date);
                }
                DateFinishDF.Clear();
                if (NullToEmpty(date_finish) != String.Empty)
                {
                    DateFinishDF.SelectedValue = ParseDateToDateTime(date_finish);
                }
                SelectingDateDF.Clear();
                if (NullToEmpty(date_selecting) != String.Empty)
                {
                    SelectingDateDF.SelectedValue = ParseDateToDateTime(date_selecting);
                }
                DateFulfilmentDF.Clear();
                if (NullToEmpty(date_fulfilment) != String.Empty)
                {
                    DateFulfilmentDF.SelectedValue = ParseDateToDateTime(date_fulfilment);
                }
                if (NullToEmpty(area) != String.Empty)
                {
                    AreaNF.Text = area.Replace('.', ',');
                }
                else { AreaNF.Clear(); }
                if (NullToEmpty(payment_days) != String.Empty)
                {
                    PaymentDaysNF.Text = payment_days.Replace('.', ',');
                }
                else { PaymentDaysNF.Clear(); }
                if (NullToEmpty(prepayment) != String.Empty)
                {
                    PrepaymentNF.Text = prepayment.Replace('.', ',');
                }
                else { PrepaymentNF.Clear(); }
                if (NullToEmpty(balance) != String.Empty)
                {
                    BalanceNF.Text = balance.Replace('.', ',');
                }
                else { BalanceNF.Clear(); }
                if (NullToEmpty(client_price) != String.Empty)
                {
                    ClientPriceNF.Text = client_price.Replace('.', ',');
                }
                else { ClientPriceNF.Clear(); }
                if (NullToEmpty(federal_price) != String.Empty)
                {
                    FederalPriceNF.Text = federal_price.Replace('.', ',');
                }
                else { FederalPriceNF.Clear(); }
                if (NullToEmpty(payment) != String.Empty)
                {
                    PaymentNF.Text = payment.Replace('.', ',');
                }
                else { PaymentNF.Clear(); }
                if (NullToEmpty(total_price) != String.Empty)
                {
                    TotalPriceNF.Text = total_price.Replace('.', ',');
                }
                else { TotalPriceNF.Clear(); }
                if (NullToEmpty(nds_price) != String.Empty)
                {
                    NdsPriceNF.Text = nds_price.Replace('.', ',');
                }
                else { NdsPriceNF.Clear(); }

                FillRegions();
                FillContractStatus();
                FillContractSubjects();
                FillContractSigners();
                FillLettersAttorney(id_signer);

                AddEditContractW.Show();
            }
        }

        [DirectMethod]
        public void AddContract(String index_number, String id_region, String id_organization, String title_organization, String id_contract_subject,
                                String client_position, String client_leader, String basic_document, String id_signer, String id_letter_attorney,
                                String contract_start_date, String contract_end_date, String area, String payment_days,
                                String total_price, String nds_price, String client_price, String federal_price, String prepayment, String balance,
                                String payment, String date_finish, String date_selecting, String date_fulfilment, String id_contract_status)
        {
            if (connection_try)
            {
                Int64 start_date = ParseDate(contract_start_date);
                Int64 end_date = ParseDate(contract_end_date);
                Int64 date_finish_date = ParseDate(date_finish);
                Int64 fulfilment_date = ParseDate(date_fulfilment);
                Int64 selecting_date = ParseDate(date_selecting);
                //условие на проверку названия!!!
                if (title_organization != String.Empty &&
                    id_organization != String.Empty &&
                    id_contract_status != String.Empty &&
                    id_contract_subject != String.Empty &&
                    id_signer != String.Empty &&
                    id_letter_attorney != String.Empty)
                {
                    conn.Open();
                    SqlCommand add_contract = new SqlCommand("Add_Contract", conn);
                    add_contract.CommandType = CommandType.StoredProcedure;
                    add_contract.Parameters.AddWithValue("@index_number", Convert.ToInt32(NotNull(index_number)));
                    add_contract.Parameters.AddWithValue("@id_contract_status", id_contract_status);
                    add_contract.Parameters.AddWithValue("@id_region", Convert.ToInt32(NotNull(id_region)));
                    add_contract.Parameters.AddWithValue("@id_organization", Convert.ToInt32(NotNull(id_organization)));
                    add_contract.Parameters.AddWithValue("@id_contract_subject", Convert.ToInt32(NotNull(id_contract_subject)));
                    add_contract.Parameters.AddWithValue("@client_position", NotNullText(client_position));
                    add_contract.Parameters.AddWithValue("@client_leader", NotNullText(client_leader));
                    add_contract.Parameters.AddWithValue("@basis_document", NotNullText(basic_document));
                    add_contract.Parameters.AddWithValue("@id_signer", Convert.ToInt32(NotNull(id_signer)));
                    add_contract.Parameters.AddWithValue("@id_letter_attorney", Convert.ToInt32(NotNull(id_letter_attorney)));
                    add_contract.Parameters.AddWithValue("@contract_start_date_long", start_date);
                    add_contract.Parameters.AddWithValue("@contract_end_date_long", end_date);
                    add_contract.Parameters.AddWithValue("@area", Convert.ToDouble(NotNull(area)));
                    add_contract.Parameters.AddWithValue("@payment_days", Convert.ToInt32(NotNull(payment_days)));
                    add_contract.Parameters.AddWithValue("@total_price", Convert.ToDouble(NotNull(total_price)));
                    add_contract.Parameters.AddWithValue("@nds_price", Convert.ToDouble(NotNull(nds_price)));
                    add_contract.Parameters.AddWithValue("@client_price", Convert.ToDouble(NotNull(client_price)));
                    add_contract.Parameters.AddWithValue("@federal_price", Convert.ToDouble(NotNull(federal_price)));
                    add_contract.Parameters.AddWithValue("@prepayment", Convert.ToDouble(NotNull(prepayment)));
                    add_contract.Parameters.AddWithValue("@balance", Convert.ToDouble(NotNull(balance)));
                    add_contract.Parameters.AddWithValue("@payment", Convert.ToDouble(NotNull(payment)));
                    add_contract.Parameters.AddWithValue("@date_finish_long", date_finish_date);
                    add_contract.Parameters.AddWithValue("@date_selecting_long", selecting_date);
                    add_contract.Parameters.AddWithValue("@date_fulfilment_long", fulfilment_date);
                    add_contract.Parameters.AddWithValue("@id_user", user_reg_data.id_user);

                    add_contract.Parameters.Add("@id_contract", SqlDbType.Int);
                    add_contract.Parameters["@id_contract"].Direction = ParameterDirection.Output;

                    add_contract.ExecuteNonQuery();
                    Int32 id_contract = Convert.ToInt32(add_contract.Parameters["@id_contract"].Value);
                    conn.Close();

                    SaveFile(id_contract.ToString(), 0);

                    AddEditContractW.Close();

                    FillContracts();

                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись добавлена!", IconCls = "icon-accept", Clear2 = true });
                }
                else IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Не заполнены все обязательные поля!", Clear2 = false });
            }
        }

        [DirectMethod]
        public void EditContract(String id_contract, String index_number, String id_region, String id_organization, String title_organization,
                                 String id_contract_subject, String client_position, String client_leader, String basic_document, String id_signer,
                                 String id_letter_attorney, String contract_start_date, String contract_end_date, String area, String payment_days,
                                 String total_price, String nds_price, String client_price, String federal_price, String prepayment, String balance,
                                 String payment, String date_finish, String date_selecting, String date_fulfilment, String id_contract_status)
        {
            if (connection_try)
            {
                Int64 start_date = ParseDate(contract_start_date);
                Int64 end_date = ParseDate(contract_end_date);
                Int64 date_finish_date = ParseDate(date_finish);
                Int64 fulfilment_date = ParseDate(date_fulfilment);
                Int64 selecting_date = ParseDate(date_selecting);
                //условие на проверку названия!!!
                if (title_organization != String.Empty &&
                    id_organization != String.Empty &&
                    id_contract_status != String.Empty &&
                    id_contract_subject != String.Empty &&
                    id_signer != String.Empty &&
                    id_letter_attorney != String.Empty)
                {
                    conn.Open();
                    SqlCommand edit_contract = new SqlCommand("Edit_Contract", conn);
                    edit_contract.CommandType = CommandType.StoredProcedure;
                    edit_contract.Parameters.AddWithValue("@id_contract", Convert.ToInt32(NotNull(id_contract)));
                    edit_contract.Parameters.AddWithValue("@index_number", Convert.ToInt32(NotNull(index_number)));
                    edit_contract.Parameters.AddWithValue("@id_contract_status", id_contract_status);
                    edit_contract.Parameters.AddWithValue("@id_region", Convert.ToInt32(NotNull(id_region)));
                    edit_contract.Parameters.AddWithValue("@id_organization", Convert.ToInt32(NotNull(id_organization)));
                    edit_contract.Parameters.AddWithValue("@id_contract_subject", Convert.ToInt32(NotNull(id_contract_subject)));
                    edit_contract.Parameters.AddWithValue("@client_position", NotNullText(client_position));
                    edit_contract.Parameters.AddWithValue("@client_leader", NotNullText(client_leader));
                    edit_contract.Parameters.AddWithValue("@basis_document", NotNullText(basic_document));
                    edit_contract.Parameters.AddWithValue("@id_signer", Convert.ToInt32(NotNull(id_signer)));
                    edit_contract.Parameters.AddWithValue("@id_letter_attorney", Convert.ToInt32(NotNull(id_letter_attorney)));
                    edit_contract.Parameters.AddWithValue("@contract_start_date_long", start_date);
                    edit_contract.Parameters.AddWithValue("@contract_end_date_long", end_date);
                    edit_contract.Parameters.AddWithValue("@area", Convert.ToDouble(NotNull(area)));
                    edit_contract.Parameters.AddWithValue("@payment_days", Convert.ToInt32(NotNull(payment_days)));
                    edit_contract.Parameters.AddWithValue("@total_price", Convert.ToDouble(NotNull(total_price)));
                    edit_contract.Parameters.AddWithValue("@nds_price", Convert.ToDouble(NotNull(nds_price)));
                    edit_contract.Parameters.AddWithValue("@client_price", Convert.ToDouble(NotNull(client_price)));
                    edit_contract.Parameters.AddWithValue("@federal_price", Convert.ToDouble(NotNull(federal_price)));
                    edit_contract.Parameters.AddWithValue("@prepayment", Convert.ToDouble(NotNull(prepayment)));
                    edit_contract.Parameters.AddWithValue("@balance", Convert.ToDouble(NotNull(balance)));
                    edit_contract.Parameters.AddWithValue("@payment", Convert.ToDouble(NotNull(payment)));
                    edit_contract.Parameters.AddWithValue("@date_finish_long", date_finish_date);
                    edit_contract.Parameters.AddWithValue("@date_selecting_long", selecting_date);
                    edit_contract.Parameters.AddWithValue("@date_fulfilment_long", fulfilment_date);
                    edit_contract.Parameters.AddWithValue("@id_user", user_reg_data.id_user);
                    edit_contract.ExecuteNonQuery();
                    conn.Close();

                    SaveFile(id_contract.ToString(), 1);

                    AddEditContractW.Close();

                    FillContracts();

                    IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
                }
                else IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Не заполнены все обязательные поля!", Clear2 = false });
            }
        }

        public void FillOrganization()
        {
            if (connection_try)
            {
                conn.Open();
                adapterOrganization = new SqlDataAdapter(selectCommOrganizationsFull, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                indexDV = new System.Data.DataView(indexDS.Tables["Organization"]);
                OrganizationS.DataSource = indexDV;
                OrganizationS.DataBind();
                OrganizationS.Sort("title_organization", Ext.Net.SortDirection.Default);
                //OrganizationGP.GetSelectionModel().Select(0);
                //OrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_organization));
                //FillDepartment(current_id_organization);
                conn.Close();
            }
        }

        public void FillContractSigners()
        {
            conn.Open();
            adapterSigners = new SqlDataAdapter(selectCommSigners, conn);
            adapterSigners.Fill(indexDS, "Signers");
            indexDV = new System.Data.DataView(indexDS.Tables["Signers"]);
            SignerS.DataSource = indexDV;
            SignerS.DataBind();
            SignerS.Sort("signer_full_name", Ext.Net.SortDirection.Default);
            conn.Close();
        }

        [DirectMethod]
        public void FillLettersAttorney(String id_signer)
        {
            String q = selectCommLettersAttorney;
            if (NotNull(id_signer) != "0") { q += (" WHERE id_signer=" + id_signer); }
            conn.Open();
            adapterLettersAttorney = new SqlDataAdapter(q, conn);
            adapterLettersAttorney.Fill(indexDS, "LettersAttorney");
            indexDV = new System.Data.DataView(indexDS.Tables["LettersAttorney"]);
            LetterAttorneyS.DataSource = indexDV;
            LetterAttorneyS.DataBind();
            LetterAttorneyS.Sort("letter_attorney", Ext.Net.SortDirection.Default);
            conn.Close();
        }

        [DirectMethod]
        public void ShowSelectOrganizationW()
        {
            FillOrganization();
            OrganizationW.Show();
        }

        [DirectMethod]
        public void SetContractOrgValues(String id_organization, String title_organization)
        {
            IdOrganizationContractTF.Text = id_organization;
            TitleOrganizationContractTF.Text = title_organization;
            OrganizationW.Close();
        }

        [DirectMethod]
        public void DeleteContractMessage(String id_contract)
        {
            String handler = "App.direct.DeleteContract(" + id_contract + ", 'Удалить');";
            X.Msg.Confirm("Внимание!", "Вы действительно хотите удалить данный контракт?", new MessageBoxButtonsConfig
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
        public void DeleteContract(String id_contract, String delete_str)
        {
            if (String.Compare(delete_str, "Удалить") == 0 && id_contract != String.Empty && connection_try)
            {
                conn.Open();
                SqlCommand get_count_sl = new SqlCommand("Delete_Contract", conn);
                get_count_sl.CommandType = CommandType.StoredProcedure;
                get_count_sl.Parameters.AddWithValue("@id_contract", Convert.ToInt32(id_contract));
                get_count_sl.ExecuteNonQuery();
                conn.Close();

                FillContracts();
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
            AddEditOrganizationW.Show();
            TitleOrganizationTF.Focus();
            TitleOrganizationTF.SelectText();
        }

        //подтверждение добавления хозяйства
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
                //OrganizationGP.GetSelectionModel().Select(Convert.ToInt32(current_selected_organization));
                //FillDepartment(current_id_organization);
                conn.Close();
            }
        }

        [DirectMethod]
        public void AddOrganization(String title_organization, String full_title_org, String leader_org, String basis_document, String chief_agronomist, String legal_address, String mailing_address, String email_address, String OKATO_org, String OKTMO_org, String INN_org, String KPP_org, String OGRN_org, String OKVED_org, String OKPO_org, String pay_account_org, String full_bank_name, String bik, String bank_correspond_account)
        {
            if (connection_try)
            {
                conn.Open();
                //adapterOrganization = new SqlDataAdapter(selectCommOrganization + " WHERE id_region=" + current_id_region, conn);
                adapterOrganization = new SqlDataAdapter(selectCommOrganizationsFull, conn);
                adapterOrganization.Fill(indexDS, "Organization");
                System.Data.DataView dv_organization = new System.Data.DataView(indexDS.Tables["Organization"]);
                conn.Close();
                dv_organization.Sort = "code_organization";
                String code_organization = String.Empty;
                for (Int32 i = 1; i <= 999; i++)
                {
                    if (i < 10) { code_organization = current_id_region + "00" + i.ToString(); }
                    else if (i > 9 && i < 100) { code_organization = current_id_region + "0" + i.ToString(); }
                    else if (i > 99 && i < 1000) { code_organization = current_id_region + i.ToString(); }
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

                }
                indexDS.Tables["Organization"].Clear();
                FillOrganization();
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
                FillOrganization();
                IndexSB.SetStatus(new StatusBarStatusConfig { Text = "Запись отредактирована!", IconCls = "icon-accept", Clear2 = true });
            }
        }

        [DirectMethod]
        public void SetDateFinish(String id_status)
        {
            if (id_status == "5")
            {
                DateFinishDF.SelectedDate = DateTime.Now;
            }
        }

        [DirectMethod]
        public void SetBalance(String client_price, String payment)
        {
            Double cp = 0.0, p = 0.0;
            if (NotNull(client_price) != "0") { cp = Convert.ToDouble(NotNull(client_price)); }
            if (NotNull(payment) != "0") { p = Convert.ToDouble(NotNull(payment)); }

            if (cp > 0)
            {
                BalanceNF.Text = (cp - p).ToString();
            }
        }
    }

    public class FlagEditing
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
}