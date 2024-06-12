using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

public partial class PageStorage : UserControl {

	public PageStorage() {
        InitializeComponent();
		this.dg_Склад.ItemsSource = Data.TBL.Склад;
		UpdateTable();
    }


	private void _Loaded(object sender, RoutedEventArgs e) {
		this.dg_Склад.Columns[0].Visibility = Visibility.Hidden;
		//this.dg_Склад.Columns[0].Header = "Номер";
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = null;
		ResTbl = DB.SQLQuery($"SELECT ЗапМ.id, ЗапМ.Название, Кат.Название, COUNT(Зап.id), Марки.Марка, Авто.Модель FROM AutoServicePlus.ЗапчастиМодели ЗапМ\r\nINNER JOIN AutoServicePlus.Запчасти Зап ON Зап.Модель_id = ЗапМ.id\r\nINNER JOIN AutoServicePlus.КатегорииЗап Кат ON ЗапМ.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть АвтоЗап ON АвтоЗап.Запчасть_id = Зап.id\r\nLEFT JOIN AutoServicePlus.Автомобили Авто ON АвтоЗап.Автомобиль_id = Авто.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Марки ON Авто.Марка_id = Марки.id\r\nWHERE ЗапМ.id LIKE '%{this.e_Search.Text}%' OR ЗапМ.Название LIKE '%{this.e_Search.Text}%' OR Кат.Название LIKE '%{this.e_Search.Text}%'\r\nGROUP BY ЗапМ.id, Марки.id, Авто.id;");

		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.Склад.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetInt(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
		}
		this.dg_Склад.Items.Refresh();
	}


	public void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		UpdateTable();
	}

	private void dg_Склад_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
		TBL_Склад Склад = (TBL_Склад)this.dg_Склад.SelectedItem;
		if (Склад != null) {
			Pages.PagePartsStore форма = new(Склад.id);
			форма.Show();
		}
	}


	private void e_Search_Changed(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}

	private void dp_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
		UpdateTable();
	}
}
