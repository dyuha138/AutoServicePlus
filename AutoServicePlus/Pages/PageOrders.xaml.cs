using MaterialDesignThemes.Wpf;

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

public partial class PageOrders : UserControl {
    public PageOrders() {
        InitializeComponent();
    }

	private void b_Add_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.PageNewOrder = new();
		Data.MainWin.HambMenu.Content = Data.MainWin.PageNewOrder;
    }

	private void b_Edit_Click(object sender, RoutedEventArgs e) {

	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {

	}
}
