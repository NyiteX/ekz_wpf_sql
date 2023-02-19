using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ekz_wpf_sql
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<My_items> items = new List<My_items>();
        public Window1()
        {
            InitializeComponent();


            Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");
        }

        void Select_func(string s)
        {
            if (File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = s;
                    command.ExecuteNonQuery();

                    MessageBox.Show("Completed");
                }
        }
        void Load_inf(string s)
        {
            if (File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = s;
                    SqliteDataReader reader = command.ExecuteReader();

                    items.Clear();
                    listbox_books.Items.Clear();
                    if (reader.HasRows)
                    {                       
                        while (reader.Read())
                        {
                            string name = reader.GetValue(0).ToString();
                            string fio = reader.GetValue(1).ToString();
                            string izdatel = reader.GetValue(2).ToString();
                            string ganre = reader.GetValue(3).ToString();
                            string date = reader.GetValue(4).ToString();
                            int count = Convert.ToInt32(reader.GetValue(5));
                            int price = Convert.ToInt32(reader.GetValue(6));
                            int id = Convert.ToInt32(reader.GetValue(7));

                            items.Add(new My_items(name, fio, izdatel, ganre, date, count, price, id));

                            listbox_books.Items.Add(items.LastOrDefault());
                        }
                    }

                }

        }

        private void add_books_Click(object sender, RoutedEventArgs e)
        {

            Window_add form2 = new Window_add();
            form2.ShowDialog();
            Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");

        }

        private void sell_book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listbox_books.SelectedIndex != -1)
                {
                    Select_func("INSERT INTO SellBooks(BookID) VALUES(" + items[listbox_books.SelectedIndex].ID + ")");

                    Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void delete_Book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listbox_books.SelectedIndex != -1)
                {
                    Select_func("DELETE FROM Book WHERE Name = '" + items[listbox_books.SelectedIndex].Name + "'");

                    Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void tb_search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tb_search.Text == "Search...") tb_search.Text = "";
        }

        private void tb_search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_search.Text == "") tb_search.Text = "Search...";
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (CB_search.SelectedIndex == 0)
                Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book WHERE Name = '" + tb_search.Text + "'");
            else if (CB_search.SelectedIndex == 1)
                Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book WHERE FIO = '" + tb_search.Text + "'");
            else if (CB_search.SelectedIndex == 2)
                Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book WHERE Ganre = '" + tb_search.Text + "'");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");
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

        private void sell_book_list_Click(object sender, RoutedEventArgs e)
        {
            string command = "SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,Book.ID " +
                "FROM Book,SellBooks WHERE SellBooks.BookID = Book.ID " +
                "GROUP BY Book.ID " +
                "HAVING COUNT(SellBooks.BookID) = (SELECT MAX(SellBooks.BookID)FROM SellBooks,Book WHERE Book.ID = SellBooks.BookID)";
            Load_inf(command);
        }

        private void btn_load_sell_books(object sender, RoutedEventArgs e)
        {
            Load_inf("SELECT Name, FIO, Izdatel, Ganre, Date, CountSTR, Price, Book.ID FROM Book,SellBooks WHERE SellBooks.BookID = Book.ID");
            
        }

        private void edit_book_btn_Click(object sender, RoutedEventArgs e)
        {
            if (listbox_books.SelectedIndex != -1)
            {
                Window_add form2 = new Window_add(1); //1 for edit func activate
                form2.tb_name.Text = items[listbox_books.SelectedIndex].Name;
                form2.tb_fio.Text = items[listbox_books.SelectedIndex].FIO;
                form2.tb_ganre.Text = items[listbox_books.SelectedIndex].Ganre;
                form2.tb_izdatel.Text = items[listbox_books.SelectedIndex].Izdatel;
                form2.tb_countSTR.Text = items[listbox_books.SelectedIndex].CountSTR.ToString();
                form2.tb_price.Text = items[listbox_books.SelectedIndex].Price.ToString();
                try
                {
                    form2.dp_date.SelectedDate = DateTime.ParseExact(items[listbox_books.SelectedIndex].Date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                form2.Title = items[listbox_books.SelectedIndex].Name;

                form2.ShowDialog();
                Load_inf("SELECT Name,FIO,Izdatel,Ganre,Date,CountSTR,Price,ID FROM Book");
            }
        }
    }
}
