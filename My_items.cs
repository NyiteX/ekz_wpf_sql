using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ekz_wpf_sql
{
    internal class My_items
    {
        static public string database_name = "D:/Database.db";

        public int ID { get; set; }
        public string Name { get; set; }
        public string FIO { get; set; }
        public string Izdatel { get; set; }
        public string Ganre { get; set; }
        public string Date { get; set; }
        public int CountSTR { get; set; }
        public int Price { get; set; }

        public My_items(string name, string fio, string izdatel, string ganre,string date,
            int countstr, int price, int iD)
        {
            FIO = fio;
            Name = name;
            Izdatel = izdatel;
            Ganre =ganre;
            Date = date;
            Price = price;
            CountSTR =countstr;
            ID = iD;
        }
    }
}
