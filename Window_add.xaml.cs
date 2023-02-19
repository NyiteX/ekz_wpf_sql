using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ekz_wpf_sql
{
    /// <summary>
    /// Логика взаимодействия для Window_add.xaml
    /// </summary>
    public partial class Window_add : Window
    {
        int ini = 0;
        public Window_add()
        {
            InitializeComponent();

            dp_date.DisplayDateEnd = DateTime.Now;
        }
        public Window_add(int n)
        {
            InitializeComponent();

            dp_date.DisplayDateEnd = DateTime.Now;
            ini = n;
            if (ini == 1) btn_add.Content = "Сохранить изменения";
        }

        void Insert_()
        {
            if (File.Exists(My_items.database_name))
                using (var connection = new SqliteConnection("Data Source = " + My_items.database_name))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                    command.Connection = connection;

                    command.CommandText = "INSERT INTO Book" +
                        "(Name,FIO,Izdatel,CountSTR,Ganre,Date,Price)" +
                        "VALUES('" + tb_name.Text + "','" + tb_fio.Text + "','" +
                        tb_izdatel.Text + "'," + tb_countSTR.Text + ",'" +
                        tb_ganre.Text + "','" + dp_date.SelectedDate.Value.ToShortDateString() + "'," +
                        tb_price.Text + ")";


                    command.ExecuteNonQuery();
                    MessageBox.Show("Добавлено.");
                    Close();
                }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tb_price.Text.Length > 0 && tb_name.Text.Length > 0 && tb_izdatel.Text.Length > 0
                        && tb_ganre.Text.Length > 0 && tb_fio.Text.Length > 0 && tb_countSTR.Text.Length > 0
                        && dp_date.SelectedDate.ToString().Length > 0)
            {
                if (int.TryParse(tb_countSTR.Text, out _) && int.TryParse(tb_price.Text, out _))
                {
                    if (ini == 0)
                        Insert_();
                    else if (ini == 1)
                    {
                        try
                        {
                            using (SqliteConnection connection = new SqliteConnection("Data Source = " + My_items.database_name))
                            {
                                connection.Open();

                                SqliteCommand command = new SqliteCommand("Data Source = " + My_items.database_name);
                                command.Connection = connection;

                                command.CommandText = "UPDATE Book SET Price = " + tb_price.Text + " WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET CountSTR = " + tb_countSTR.Text + " WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET Date = '" + dp_date.SelectedDate.Value.ToShortDateString() + "' WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET Izdatel = '" + tb_izdatel.Text + "' WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET Ganre = '" + tb_ganre.Text + "' WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET FIO = '" + tb_fio.Text + "' WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE Book SET Name = '" + tb_name.Text + "' WHERE Name = '" + Title + "'";
                                command.ExecuteNonQuery();

                                MessageBox.Show("Completed.");
                                Close();
                            }
                        }catch(Exception k) { MessageBox.Show(k.Message);}
                    }
                }
                else
                    MessageBox.Show("Кривой ввод. Цену и кол-во страниц нужно заполнять цифрами...");
            }
            else
                MessageBox.Show("Кривой ввод. Пустые строки.");
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
