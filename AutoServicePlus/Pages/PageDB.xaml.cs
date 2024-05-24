using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoServicePlus.Pages;

public partial class PageDB : UserControl {
    public PageDB() {
        InitializeComponent();

        //DB.DB_Заказы.UpdateListfromTable();

        
        //DBM_Заказ да = Data.DB.ЗаказыList.ElementAt(0);


        //List<DBM_Заказ.Запчасть> ll = new();
        //ll.Add(new(1, 5));

        //DB.DB_Заказы.Add(new(0, 54813, 1, 1, 2, ll));


    }
}
