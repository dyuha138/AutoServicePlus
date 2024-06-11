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
using MahApps.Metro.Controls;

namespace AutoServicePlus.Pages;


public partial class PagePartsforOrder : MetroWindow {
	private int Заказ_id = 0;
    public PagePartsforOrder(int Заказ_id) {
        InitializeComponent();
        this.dg_Запчасти.ItemsSource = Data.TBL.TBLData_ЗапчастиМодель;
		this.Заказ_id = Заказ_id;
		UpdateTable();
    }

	private void _Loaded(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = DB.SQLQuery($"SELECT Parts.id, Parts.Название, Cat.Название, OrdPart.Количество, Marks.Марка, Cars.Модель, Конт.Название FROM AutoServicePlus.ЗапчастиМодели Parts\r\nLEFT JOIN AutoServicePlus.КатегорииЗап Cat ON Parts.Категория_id = Cat.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть AutoPart ON AutoPart.Запчасть_id = Parts.id\r\nLEFT JOIN AutoServicePlus.Автомобили Cars ON AutoPart.Автомобиль_id = Cars.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Marks ON Cars.Марка_id = Marks.id\r\nINNER JOIN AutoServicePlus.ЗаказЗапчасть OrdPart ON OrdPart.Запчасть_id = Parts.id\r\nLEFT JOIN AutoServicePlus.Контрагенты Конт ON OrdPart.Контрагент_id = Конт.id\r\nWHERE OrdPart.Заказ_id = {this.Заказ_id} AND (Parts.Название LIKE '%{this.e_Search.Text}%' OR Cat.Название LIKE '%{this.e_Search.Text}%' OR Marks.Марка LIKE '%{this.e_Search.Text}%' OR Cars.Модель LIKE '%{this.e_Search.Text}%' OR Конт.Название LIKE '%{this.e_Search.Text}%');");
		Data.TBL.TBLData_ЗапчастиМодель.Clear();
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.TBLData_ЗапчастиМодель.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5), ResTbl.GetStr(6)));
			}
		}
		this.dg_Запчасти.Items.Refresh();
	}

	private void _Closed(object sender, EventArgs e) {
		Data.MainWin.PagePartsforOrder = null;
	}

	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}
}
