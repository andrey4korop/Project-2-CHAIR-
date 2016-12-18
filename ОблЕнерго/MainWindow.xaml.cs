using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ОблЕнерго
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(Tick);
            Timer.Interval = new TimeSpan(0, 0, 30);
            SuperTimer = new DispatcherTimer();
            SuperTimer.Tick += new EventHandler(SuperTick);
            SuperTimer.Interval = new TimeSpan(0, 0, 5);
        }
        public DispatcherTimer Timer;
        public DispatcherTimer SuperTimer;
        public bool isLogin = false;
        //public bool b = false;
        public string GET(string uri)
        {
            string ret = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                request.Method = "GET";
                request.CookieContainer = cooks;
                request.Timeout = 5000;
                //request.UserAgent = "NinjaSafeInternet";
                HttpWebResponse respouse = request.GetResponse() as HttpWebResponse;

                StreamReader stream = new StreamReader(respouse.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                
                ret = (string)stream.ReadToEnd();
                stream.Close();
                return ret;
            }
            catch
            {

                return ret;
            }

        }

        public CookieContainer cooks = new CookieContainer();


        public string POST(string Url, string Data)
        {
            string ret = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";

                req.CookieContainer = cooks;
                req.Timeout = 5000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.GetEncoding("windows-1251").GetBytes(Data);
                req.ContentLength = sentData.Length;
                Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                //cookie = res.Cookies.;

                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                //Кодировка указывается в зависимости от кодировки ответа сервера
                //Char[] read = new Char[256];
                ret = sr.ReadToEnd();

                return ret;
            }
            catch
            {

                return ret;
            }

        }



        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            string data = "login=" + login.Text + "&password=" + password.Text;

            if (POST("http://www.oblenergo.odessa.ua:54380/index.php?log=1", data) != "")
            {
                test.Text = "Вход выполнен успешно. ";
                isLogin = true;
            }
            else
            {
                test.Text += "Отсуствует соединение с интернетом. ";
            }
        }

        private void run_Click(object sender, RoutedEventArgs e)
        {
            if (isLogin == true)
            {
                Timer.Start();
                test.Text += "Ожидание начала торгов. ";
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            SuperTimer.Stop();
            test.Text += "Программа остановлена. ";
        }





        private void addTend_Click(object sender, RoutedEventArgs e)
        {
            if (number_add.Text != "" && isLogin == true)
            {
                getInfo(number_add.Text);
                test.Text += "Тендер добавлен. ";
               
            }
        }

        public void getInfo(string num)
        {
            List<Table> result = new List<Table>();
            result = (List<Table>)dataGrid.ItemsSource;
            string Number = num;
            string name;
            string TimeEnd = "";
            string Money = "";
            string row;
            string nnn;
            string hui;
            string s;
            string search = @"<div class=""main"">";
            string searchend;
            int indexS;
            int indexEnd;
            s = GET("http://www.oblenergo.odessa.ua:54380/index.php?id=&progect=" + num);
            if (s != "")
            {
                indexS = s.IndexOf(search);
                s = s.Substring(indexS, s.Length - indexS);
                search = "<tr><th colspan=2>";
                indexS = s.IndexOf(search);
                searchend = "</th></tr>";
                indexEnd = s.IndexOf(searchend, indexS + 1);
                name = s.Substring(indexS + search.Length, indexEnd - indexS - search.Length);


                search = "Цінова пропозиція неврахованих робіт, грн. без ПДВ</center></td></tr><tr><td><center>1</center></td><td><center>";
                indexS = s.IndexOf(search);
                if (indexS > 0)
                {
                    searchend = "</center></td>";
                    indexEnd = s.IndexOf(searchend, indexS + search.Length);
                    Money = s.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                    // test.Text += "Debug 1: "+Money;
                }
                s = GET("http://www.oblenergo.odessa.ua:54380/index.php?id=16");
                while (s.IndexOf("<tr >") > 0)
                {
                    search = "<tr >";
                    indexS = s.IndexOf(search);
                    searchend = "</tr>";
                    indexEnd = s.IndexOf(searchend, indexS);
                    row = s.Substring(indexS, indexEnd - indexS + searchend.Length);
                    s = s.Remove(indexS, row.Length);

                    search = "<td><center>";
                    indexS = row.IndexOf(search);
                    searchend = "</center></td>";
                    indexEnd = row.IndexOf(searchend, indexS);
                    nnn = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                    if (nnn == num)
                    {
                        search = "<td><center>";
                        indexS = row.IndexOf(search);
                        searchend = "</center></td>";
                        indexEnd = row.IndexOf(searchend, indexS);
                        hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                        row = row.Remove(indexS, hui.Length);

                        search = "<td><center>";
                        indexS = row.IndexOf(search);
                        searchend = "</center></td>";
                        indexEnd = row.IndexOf(searchend, indexS);
                        hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                        row = row.Remove(indexS, hui.Length);

                        search = "<td><center>";
                        indexS = row.IndexOf(search);
                        searchend = "</center></td>";
                        indexEnd = row.IndexOf(searchend, indexS);
                        hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                        row = row.Remove(indexS, hui.Length);

                        search = "<td><center>";
                        indexS = row.IndexOf(search);
                        searchend = "</center></td>";
                        indexEnd = row.IndexOf(searchend, indexS);
                        TimeEnd = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                        row = row.Remove(indexS, indexEnd - indexS + searchend.Length);
                        if (Money == "")
                        {
                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            Money = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                            //  test.Text += "Debug 2: " + Money;
                            row = row.Remove(indexS, hui.Length);
                        }

                    }

                }
                if (Money == "" || TimeEnd == "")
                {
                    s = GET("http://www.oblenergo.odessa.ua:54380/index.php?id=15");
                    while (s.IndexOf("<tr >") > 0)
                    {
                        search = "<tr >";
                        indexS = s.IndexOf(search);
                        searchend = "</tr>";
                        indexEnd = s.IndexOf(searchend, indexS);
                        row = s.Substring(indexS, indexEnd - indexS + searchend.Length);
                        s = s.Remove(indexS, row.Length);

                        search = "<td><center>";
                        indexS = row.IndexOf(search);
                        searchend = "</center></td>";
                        indexEnd = row.IndexOf(searchend, indexS);
                        nnn = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                        if (nnn == num)
                        {
                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                            row = row.Remove(indexS, hui.Length);

                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                            row = row.Remove(indexS, hui.Length);

                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                            row = row.Remove(indexS, hui.Length);

                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            hui = row.Substring(indexS, indexEnd - indexS + searchend.Length);
                            row = row.Remove(indexS, hui.Length);

                            search = "<td><center>";
                            indexS = row.IndexOf(search);
                            searchend = "</center></td>";
                            indexEnd = row.IndexOf(searchend, indexS);
                            TimeEnd = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                            row = row.Remove(indexS, indexEnd - indexS + searchend.Length);
                            if (Money == "")
                            {
                                search = "<td><center>";
                                indexS = row.IndexOf(search);
                                searchend = "</center></td>";
                                indexEnd = row.IndexOf(searchend, indexS);
                                Money = row.Substring(indexS + search.Length, indexEnd - indexS - search.Length);
                                //test.Text += "Debug 3: " + Money;
                                row = row.Remove(indexS, hui.Length);
                            }

                        }



                    }
                }
                while (Money.IndexOf(" ") > 0)
                {
                    Money = Money.Remove(Money.IndexOf(" "), 1);
                    // test.Text += "Debug 4: " + Money;
                }
                if (Money.IndexOf(".") > 0)
                {
                    Money = Money.Remove(Money.IndexOf("."), 3);
                    //test.Text += "Debug 5: " + Money;
                }

                if (result == null)
                {

                    result = new List<Table>();
                    result.Add(new Table(Number, name, TimeEnd, Money));
                }
                else
                {
                    bool flag = true;
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result.ElementAt(i).Number == Number)
                        {
                            result.ElementAt(i).Money = Money;
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        result.Add(new Table(Number, name, TimeEnd, Money));
                    }
                }

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = result;
                
                //test.Text = name+Money+TimeEnd;
            }
            else
            {

                test.Text += "Отсуствует соединение с интернетом. ";
            }
        }



        public void Tick(object sender, EventArgs e)
        {
            List<Table> result = new List<Table>();
            result = (List<Table>)dataGrid.ItemsSource;
            if (result != null)
            {

                for (int i = 0; i < result.Count; i++)
                {
                    if (result.ElementAt(i).Enable)
                    {
                        getInfo(result.ElementAt(i).Number);
                        result = (List<Table>)dataGrid.ItemsSource;
                        int day = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(0, 2));
                        int month = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(3, 2));
                        int year = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(6, 4));
                        int hour = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(11, 2));
                        int minute = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(14, 2));
                        int second = 0;
                        
                        int dm = Convert.ToInt16(time__left.Text);
                        if ( dm >= 60)
                        {
                            dm -= 60;
                        }

                        DateTime timeend = new DateTime(year, month, day, hour, minute, second);
                        DateTime timestart = timeend.AddMinutes(-dm);
                        //test.Text += timestart.Hour.ToString() + timestart.Minute.ToString() + "   now:" + DateTime.Now.ToString();

                        TimeSpan howmany = timestart.Subtract(DateTime.Now);
                        test.Text += "Начало через : " + howmany.Days.ToString() + "д. " + howmany.Hours.ToString()+"ч. " + howmany.Minutes.ToString() + "м. ";
                        //test.Text += hour.ToString() + minute.ToString() + day.ToString() + month.ToString() + year.ToString() + "   now:" + DateTime.Now.ToString();
                        if (DateTime.Now.CompareTo(timeend) > 0)
                        {
                            test.Text += "Тендер не является актуальным. Он завершен. ";
                            result.ElementAt(i).Enable = false;
                        }
                        else
                        {
                            if ((DateTime.Now.CompareTo(timestart) >= 0))
                            {
                                test.Text += "Программа начинает свою работу. ";
                                SuperTimer.Start();
                                Timer.Stop();
                            }
                        }
                        
                    }
                }
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = result;

            }
            else
            {
                Timer.Stop();
            }
        }

        public void SuperTick(object sender, EventArgs e)
        {
            test.Text += "Выполняется проверка. ";
            List<Table> result = new List<Table>();
            result = (List<Table>)dataGrid.ItemsSource;
            if (result != null)
            {
                bool flag = true;
                for (int i = 0; i < result.Count; i++)
                {

                    if (result.ElementAt(i).Enable)
                    {
                        flag = false;
                        getInfo(result.ElementAt(i).Number);
                        result = (List<Table>)dataGrid.ItemsSource;
                        int day = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(0, 2));
                        int month = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(3, 2));
                        int year = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(6, 4));
                        int hour = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(11, 2));
                        int minute = Convert.ToInt16(result.ElementAt(i).TimeEnd.Substring(14, 2));
                        int dm = Convert.ToInt16(time__left.Text);
                        if (dm >= 60)
                        {
                            dm -= 60;
                        }
                        int second = 0;
                        if ((DateTime.Now.CompareTo(new DateTime(year, month, day, hour - 1, minute + dm, second)) >= 0))
                        {
                            if (result.ElementAt(i).myPrice > Convert.ToInt32(result.ElementAt(i).Money))
                            {
                                if (Convert.ToInt32(result.ElementAt(i).Money) * (1 - Convert.ToInt16(percent.Text) / 100) > Convert.ToInt32(result.ElementAt(i).Minimal))
                                {
                                    //test.Text += "Отправляем свою цену: ";
                                    int ppp = (int)(Convert.ToInt32(result.ElementAt(i).Money) - (Convert.ToInt32(result.ElementAt(i).Money)) * Convert.ToInt16(percent.Text) / 100);
                                    string query = "price=" + ppp.ToString() + "&list=&price2=&list2=";
                                    result.ElementAt(i).myPrice = ppp;
                                    test.Text += "Отправляем свою цену: " + ppp + ". ";
                                    POST("http://www.oblenergo.odessa.ua:54380/index.php?id=15&progect=" + result.ElementAt(i).Number + "&new=1#main", query);
                                }
                                else
                                {
                                    test.Text += "Цена ниже минимальной. Программа прекращает понижение цены и завершает свою работу. ";
                                    result.ElementAt(i).Enable = false;
                                }
                            }

                        }
                        if (DateTime.Now.CompareTo(new DateTime(year, month, day, hour, minute, second)) > 0)
                        {
                            result.ElementAt(i).Enable = false;
                            test.Text += "Тендер не является актуальным. Он завершен. ";
                        }
                    }

                }
                if (flag)
                {
                    SuperTimer.Stop();
                    Timer.Start();
                }
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = result;

            }
            else
            {
                SuperTimer.Stop();
                Timer.Start();
            }
        }

        Regex inputRegex = new Regex(@"^[0-9]$");
        private void checkint(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Match match = inputRegex.Match(e.Text);
            //и проверяем или выполняется условие
            //если введенный символ не подходит нашему правилу
            if (!match.Success)
            {
                //то обработка события прекращается и ввода неправильного символа не происходит
                e.Handled = true;
            }

        }
    }
}
