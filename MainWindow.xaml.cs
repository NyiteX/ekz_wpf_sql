using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ekz_wpf_sql
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;
        public MainWindow()
        {
            InitializeComponent();

            Ini();
        }

        void Ini()
        {
            if (!File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = "CREATE TABLE Book" +
                "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                "Name TEXT NOT NULL UNIQUE, " +
                "FIO TEXT NOT NULL, " +
                "Izdatel TEXT NOT NULL," +
                "CountSTR INTEGER NOT NULL, " +
                "Ganre TEXT NOT NULL, " +
                "Date TEXT NOT NULL, " +
                "Price INTEGER NOT NULL)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE Client" +
                "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                "Name TEXT NOT NULL, " +
                "Login TEXT NOT NULL UNIQUE, " +
                "Pass TEXT NOT NULL)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE BooksClient" +
                "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                "ClientID INTEGER REFERENCES Client(ID), " +
                "BookID INTEGER REFERENCES Book(ID) UNIQUE)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE SeriesBooks" +
                                    "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                                    "Name TEXT NOT NULL, " +
                                    "BookID INTEGER REFERENCES Book(ID) UNIQUE)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE SellBooks" +
                "(ID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                "BookID INTEGER REFERENCES Book(ID))";
                    command.ExecuteNonQuery();
                }
        }

        void Save()
        {          
            if (File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = "INSERT INTO Client(Name,Login,Pass) " +
                        "VALUES('" + Name_TB.Text + "','" + Login_TB.Text + "','" + Pass_TB.Text + "')";
                    command.ExecuteNonQuery();
                }
        }
        bool Login_check()
        {
            if (File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = "SELECT * FROM Client";
                    SqliteDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        if(reader.HasRows)
                        {
                            bool f = false;
                            if (Login_TB.Text == Convert.ToString(reader.GetValue(2))) f = true;
                            else f = false;
                            if (Pass_TB.Text == Convert.ToString(reader.GetValue(3))) f = true;
                            else f = false;
                            if (f) return true;
                        }
                    }
                }
            return false;
        }

        string Proverka_probel(string str)
        {
            str = str.Replace(" ", ""); ;

            return str;
        }
        bool Proverka_kol(string str)
        {
            bool b = false;
            for (int i = 0; i < str.Length; i++)
                if (str[i] != ' ') b = true;

            return b;
        }

        private void Name_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name_TB.Text == "FirstName")
                Name_TB.Text = "";
        }

        private void Name_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Name_TB.Text == "" || !Proverka_kol(Name_TB.Text))
                Name_TB.Text = "FirstName";
            Name_TB.Text = Proverka_probel(Name_TB.Text);
        }

        private void Login_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TB.Text == "Login")
                Login_TB.Text = "";
        }

        private void Login_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Login_TB.Text == "" || !Proverka_kol(Login_TB.Text))
                Login_TB.Text = "Login";
            Login_TB.Text = Proverka_probel(Login_TB.Text);
        }

        private void Pass_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_TB.Text == "Password")
                Pass_TB.Text = "";
        }

        private void Pass_TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Pass_TB.Text == "" || !Proverka_kol(Pass_TB.Text))
                Pass_TB.Text = "Password";
            Pass_TB.Text = Proverka_probel(Pass_TB.Text);
        }

        private void BTN_Login_Click(object sender, RoutedEventArgs e)
        {
            if (login_maybe.IsChecked == false)
            {
                if (Login_TB.Text.Length > 0 && Name_TB.Text.Length > 0 && Pass_TB.Text.Length > 0
                    && Login_TB.Text != "Login" && Name_TB.Text != "FirstName" && Pass_TB.Text != "Password")
                {
                    Save();
                    MessageBox.Show("Добавлено");
                }
                else MessageBox.Show("Кривой ввод. Пустые строки.");
            }          
            else
            {
                try
                {
                    if (Login_TB.Text.Length > 0 && Pass_TB.Text.Length > 0
                        && Login_TB.Text != "Login" && Pass_TB.Text != "Password")
                    {
                        if (!Login_check()) MessageBox.Show("ti durak");
                        else
                        {

                            thread = new Thread(() =>
                            {
                                Window1 form = new Window1();

                                form.ShowDialog();
                            });
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start();
                            
                        }
                    }
                    else MessageBox.Show("Кривой ввод. Пустые строки.");
                }catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
        private void login_maybe_Unchecked(object sender, RoutedEventArgs e)
        {
            line_name.Visibility = Visibility.Visible;
            Name_TB.Visibility = Visibility.Visible;

            BTN_Login.Content = "Sign up";
        }

        private void login_maybe_Checked(object sender, RoutedEventArgs e)
        {
            line_name.Visibility = Visibility.Hidden;
            Name_TB.Visibility = Visibility.Hidden;

            BTN_Login.Content = "Log in";
        }

        private void btn_x_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
