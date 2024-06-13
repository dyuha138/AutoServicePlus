﻿using System;
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
using MahApps.Metro.Controls;

namespace AutoServicePlus.Pages;


public partial class PagePartsforReq : MetroWindow {

	private int Заявка_id = 0;
	private ObservableCollection<TBL_ЗапчастьМодель2> ЗапчастиМодели = new();

	public PagePartsforReq(int Заявка_id) {
        InitializeComponent();
        this.dg_Запчасти.ItemsSource = this.ЗапчастиМодели;
		this.Заявка_id = Заявка_id;
		this.Title += " №" + Заявка_id;
		UpdateTable();
    }

	private void _Loaded(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = DB.SQLQuery($"SELECT ЗапМ.id, ЗапМ.Название, Кат.Название, ЗакЗап.Количество, Марки.Марка, Авто.Модель, Конт.Название FROM AutoServicePlus.ЗапчастиМодели ЗапМ\r\nLEFT JOIN AutoServicePlus.КатегорииЗап Кат ON ЗапМ.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть АвтоЗап ON АвтоЗап.Запчасть_id = ЗапМ.id\r\nLEFT JOIN AutoServicePlus.Автомобили Авто ON АвтоЗап.Автомобиль_id = Авто.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Марки ON Авто.Марка_id = Марки.id\r\nINNER JOIN AutoServicePlus.ЗаявкаЗапчасть ЗаяЗап ON ЗаяЗап.Запчасть_id = ЗапМ.id\r\nWHERE ЗаяЗап.Заказ_id = {this.Заявка_id} AND (ЗапМ.Название LIKE '%{this.e_Search.Text}%' OR Кат.Название LIKE '%{this.e_Search.Text}%' OR Марки.Марка LIKE '%{this.e_Search.Text}%' OR Авто.Модель LIKE '%{this.e_Search.Text}%');");
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				this.ЗапчастиМодели.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetInt(3), ResTbl.GetStr(4), ResTbl.GetStr(5), ResTbl.GetStr(6)));
			}
		}
		this.dg_Запчасти.Items.Refresh();
	}

	private void _Closed(object sender, EventArgs e) {
		//Data.MainWin.PagePartsforOrder = null;
	}

	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}
}
