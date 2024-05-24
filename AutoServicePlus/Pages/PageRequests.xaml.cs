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

public partial class PageRequests : UserControl {
    public PageRequests() {
        InitializeComponent();
		Data.PropertiesChange.WinHeight = 190;
	}

    private void b_Add_Click(object sender, RoutedEventArgs e) {
        Data.MainWin.PageNewRequest = new();
        Data.MainWin.HambMenu.Content = Data.MainWin.PageNewRequest;
    }
}

