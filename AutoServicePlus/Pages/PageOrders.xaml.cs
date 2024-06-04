using MaterialDesignThemes.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
		this.dg_Заказы.ItemsSource = Data.TBL.TBLData_Заказы;
		//this.dg_Заказы.Columns[0].Visibility = Visibility.Hidden;
		//UpdateTable();
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = DB.SQLQuery("SELECT Ord.id, Ord.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заказы Ord\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Ord.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Ord.Сотрудник_id = Сотр.id;");
		if (ResTbl != null) {
			Data.TBL.TBLData_Заказы.Clear();
			while (ResTbl.NextRow()) {
				Data.TBL.TBLData_Заказы.Add(new(ResTbl.GetInt(0), TechFuncs.UnixToDate(ResTbl.GetInt(1)), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
			this.dg_Заказы.Items.Refresh();
		}
	}


	private void _Loaded(object sender, RoutedEventArgs e) {
		this.dg_Заказы.Columns[0].Visibility = Visibility.Hidden;

	}


	public void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		UpdateTable();
	}

	private void b_Add_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.PageNewOrder = new();
		Data.MainWin.HambMenu.Content = Data.MainWin.PageNewOrder;
    }

	private void b_Edit_Click(object sender, RoutedEventArgs e) {

	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {

	}

	private void dg_Заказы_SelectionChanged(object sender, SelectionChangedEventArgs e) {

	}

	private void dg_Заказы_DoubleClick(object sender, MouseButtonEventArgs e) {
		TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
		if (Data.MainWin.PagePartsforOrder == null) {
			Data.MainWin.PagePartsforOrder = new(Заказ.id);
			Data.MainWin.PagePartsforOrder.Show();
		}
	}

	private void e_Search_Changed(object sender, TextChangedEventArgs e) {

	}
}
