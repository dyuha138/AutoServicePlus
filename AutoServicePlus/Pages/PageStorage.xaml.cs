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
		ResTbl = DB.SQLQuery($"WITH Рег AS (SELECT Запчасть_id, Статус_id, ROW_NUMBER() OVER (PARTITION BY Запчасть_id ORDER BY Дата DESC) AS rn\r\nFROM AutoServicePlus.РегистрЗапчастей)\r\nSELECT ЗапМ.id, ЗапМ.Название, Кат.Название, COUNT(Зап.id), Марки.Марка, Авто.Модель FROM AutoServicePlus.ЗапчастиМодели ЗапМ\r\nINNER JOIN AutoServicePlus.Запчасти Зап ON Зап.Модель_id = ЗапМ.id\r\nINNER JOIN AutoServicePlus.КатегорииЗап Кат ON ЗапМ.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть АвтоЗап ON АвтоЗап.Запчасть_id = Зап.id\r\nLEFT JOIN AutoServicePlus.Автомобили Авто ON АвтоЗап.Автомобиль_id = Авто.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Марки ON Авто.Марка_id = Марки.id\r\nINNER JOIN Рег ON Зап.id = Рег.Запчасть_id AND Рег.rn = 1\r\nWHERE Рег.Статус_id = 2 AND (ЗапМ.id LIKE '%{this.e_Search.Text}%' OR ЗапМ.Название LIKE '%{this.e_Search.Text}%' OR Кат.Название LIKE '%{this.e_Search.Text}%')\r\nGROUP BY ЗапМ.id, Марки.id, Авто.id;");

		Data.TBL.Склад.Clear();
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

	private void dg_Склад_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (dg_Склад.SelectedIndex == -1) {
			this.b_AddReq.IsEnabled = false;
			this.b_AddOrder.IsEnabled = false;
		} else {
			this.b_AddReq.IsEnabled = true;
			this.b_AddOrder.IsEnabled = true;
		}
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

	private void b_AddOrder_Click(object sender, RoutedEventArgs e) {
		DBM_Заказ.ЗапчастьМодель ЗапчастьМодель = null;
		TBL_Склад Запчасть = (TBL_Склад)this.dg_Склад.SelectedItem;
		TBL_ЗапчастьМодель2 Запчасть2 = new();
		int запindex = 0;

		if (Data.DB.TMP_Заказ == null) {
			Data.DB.TMP_Заказ = new(0, TechFuncs.GetUnixTime(), 7, TechFuncs.ПолучитьАйдиВхода(), new());
		}
			запindex = Data.DB.TMP_Заказ.Запчасти.FindIndex(x => x.id == Запчасть.id);
			//ЗапчастьМодель = Data.DB.TMP_Заказ.Запчасти[запindex];

		if (запindex == -1) {
			int контid = DB.DB_Заказы.GetLastAgent(Запчасть.id);
			Запчасть2.id = Запчасть.id;
			Запчасть2.Название = Запчасть.Название;
			Запчасть2.Категория = Запчасть.Категория;
			Запчасть2.Количество = 1;
			Запчасть2.Марка_Авто = Запчасть.Марка_Авто;
			Запчасть2.Модель_Авто = Запчасть.Модель_Авто;
			Запчасть2.Контрагент = Data.КонтрагентыList.Find(x => x.id == контid).Data;
			Data.TBL.ЗапчастиМоделиЗак.Add(Запчасть2);
			Data.DB.TMP_Заказ.Запчасти.Add(new(Запчасть.id, 1, контid));
		} else {
			Data.TBL.ЗапчастиМоделиЗак[Data.TBL.ЗапчастиМоделиЗак.ToList().FindIndex(x => x.Название == Запчасть.Название)].Количество++;
			Data.DB.TMP_Заказ.Запчасти[запindex].Количество++;

		}
	}

	private void b_AddReq_Click(object sender, RoutedEventArgs e) {
		//TBL_Склад Запчастьtmp = (TBL_Склад)this.dg_Запчасти.SelectedItem;
		//TBL_Запчасть Запчасть = new();

		//if (Data.DB.TMP_Заявка == null) {
		//	Data.DB.TMP_Заявка = new(0, TechFuncs.GetUnixTime(), 7, TechFuncs.ПолучитьАйдиВхода(), new());
		//}

		//Запчасть.id = Запчастьtmp.id;
		//Запчасть.Название = Запчастьtmp.Название;
		//Запчасть.Категория = Запчастьtmp.Категория;
		//Запчасть.Идентификатор = (string)this.cb_Запчасти.SelectedItem;
		//Data.TBL.ЗапчастиМоделиЗая.Add(Запчасть);
		//Data.DB.TMP_Заявка.Запчасти.Add(new(Data.ЗапчастиList[this.cb_Запчасти.SelectedIndex].id));

		//Data.TBL.Склад[Запчастьtmp.id].Количество--;
	}
	
}
