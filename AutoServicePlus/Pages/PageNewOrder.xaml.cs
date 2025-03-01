﻿using System;
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


public partial class PageNewOrder : UserControl {

	private bool isOrdEdit = false;
	private bool isClearMoved = false;

	private int Категорияid = 0;
	private int Маркаid = 0;

	public PageNewOrder() {
        InitializeComponent();
		Data.PropertiesChange.WinHeight = 343;
		this.dg_Запчасти.ItemsSource = Data.TBL.ЗапчастиМодели;
		this.dg_Заказ.ItemsSource = Data.TBL.ЗапчастиМоделиЗак;

		for (int i = 0; i < Data.КатегорииList.Count; i++) {
			this.cb_Категории.Items.Add(Data.КатегорииList[i].Data);
		}
		for (int i = 0; i < Data.МаркиАвтоList.Count; i++) {
			this.cb_Марки.Items.Add(Data.МаркиАвтоList[i].Data);
		}
		for (int i = 0; i < Data.КонтрагентыList.Count; i++) {
			this.cb_Поставщики.Items.Add(Data.КонтрагентыList[i].Data);
		}

	}

	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Заказ.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Запчасти.Columns[3].Visibility = Visibility.Hidden;
		this.dg_Заказ.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Заказ.Columns[4].Visibility = Visibility.Hidden;
		this.dg_Заказ.Columns[5].Visibility = Visibility.Hidden;
		UpdateTable_Зап();
		UpdateTable_Зак();

		if (Data.TBL.ЗапчастиМоделиЗак.Count > 0) {
			this.b_Clear.IsEnabled = true;
			this.b_Done.IsEnabled = true;
		}
	}


	private void UpdateTable_Зап() {
		SQLResultTable ResTbl = null;
		bool edit = false;
		string sql = "SELECT Зап.id, Зап.Название, Кат.Название, Marks.Марка, Cars.Модель FROM AutoServicePlus.ЗапчастиМодели Зап\r\nINNER JOIN AutoServicePlus.КатегорииЗап Кат ON Зап.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть AutoPart ON AutoPart.Запчасть_id = Зап.id\r\nLEFT JOIN AutoServicePlus.Автомобили Cars ON AutoPart.Автомобиль_id = Cars.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Marks ON Cars.Марка_id = Marks.id";

		if (cb_Категории.SelectedIndex != -1 && cb_Марки.SelectedIndex != -1) {
			sql += $"\r\nWHERE Зап.Категория_id = {this.Категорияid} AND Cars.Марка_id = {this.Маркаid}";
			edit = true;
		} else if (cb_Категории.SelectedIndex != -1) {
			sql += $"\r\nWHERE Зап.Категория_id = {this.Категорияid}";
			edit = true;
		} else if (cb_Марки.SelectedIndex != -1) {
			sql += $"\r\nWHERE Cars.Марка_id = {this.Маркаid}";
			edit = true;
		}

		if (edit) {
			sql += $" AND Зап.Название LIKE '%{this.e_Search.Text}%';";
		} else {
			sql += $"\r\nWHERE Зап.Название LIKE '%{this.e_Search.Text}%';";
		}

		ResTbl = DB.SQLQuery(sql);
		Data.TBL.ЗапчастиМодели.Clear();

		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.ЗапчастиМодели.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4)));
			}
		}
		this.dg_Запчасти.Items.Refresh();
	}

	private void UpdateTable_Зак() {
		this.dg_Заказ.Items.Refresh();
	}


	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			int hetbl = 0;
			this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Заказ.ActualHeight); });

			if (Open) {
				int he = 50;
				int he2 = 8;
				Task.Factory.StartNew(() => {
					while (he < 91) {
						this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
						Thread.Sleep(7);
						he++;
					}
					this.Dispatcher.Invoke(() => { this.nud_NumЗаказ.Visibility = Visibility.Visible; });
				});
				if (hetbl < 152) {
					isClearMoved = true;
					Task.Factory.StartNew(() => {
						while (he2 < 106) {
							this.Dispatcher.Invoke(() => { this.b_Clear.Margin = new(0, 0, he2, 5); });
							Thread.Sleep(3);
							he2++;
						}
					});
				}
			} else {
				int he = 90;
				int he2 = 105;
				this.Dispatcher.Invoke(() => { this.nud_NumЗаказ.Visibility = Visibility.Hidden; });
				Task.Factory.StartNew(() => {
					while (he > 49) {
						this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
						Thread.Sleep(7);
						he--;
					}
				});
				if (isClearMoved) {
					isClearMoved = false;
					Task.Factory.StartNew(() => {
						while (he2 > 7) {
							this.Dispatcher.Invoke(() => { this.b_Clear.Margin = new(0, 0, he2, 5); });
							Thread.Sleep(3);
							he2--;
						}
					});
				}
			}
		});
	}

	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		if (isOrdEdit) {
			isOrdEdit = false;
			Data.DB.TMP_Заказ.Запчасти[this.dg_Заказ.SelectedIndex].Количество = Convert.ToInt32(this.nud_NumЗаказ.Value);
			Data.TBL.ЗапчастиМоделиЗак[this.dg_Заказ.SelectedIndex].Количество = Convert.ToInt32(this.nud_NumЗаказ.Value);
			UpdateTable_Зак();
			AnimateButtons(false);
		} else {
			isOrdEdit = true;
			this.nud_NumЗаказ.Value = Convert.ToInt32(Data.DB.TMP_Заказ.Запчасти[this.dg_Заказ.SelectedIndex].Количество);
			AnimateButtons(true);
		}
	}


	private void b_Addmini_Click(object sender, RoutedEventArgs e) {
		DBM_Заказ.ЗапчастьМодель ЗапчастьМодель = null;
		TBL_ЗапчастьМодель Запчасть = (TBL_ЗапчастьМодель)this.dg_Запчасти.SelectedItem;
		TBL_ЗапчастьМодель2 Запчасть2 = new();
		int запindex = 0;

		if (Data.DB.TMP_Заказ == null) {
			Data.DB.TMP_Заказ = new(0, TechFuncs.GetUnixTime(), 7, TechFuncs.ПолучитьАйдиВхода(), new());
		}
		запindex = Data.DB.TMP_Заказ.Запчасти.FindIndex(x => x.id == Запчасть.id && x.Контрагент_id == Data.КонтрагентыList[this.cb_Поставщики.SelectedIndex].id);
		

		if (запindex == -1) {
			Запчасть2.id = Запчасть.id;
			Запчасть2.Название = Запчасть.Название;
			Запчасть2.Категория = Запчасть.Категория;
			Запчасть2.Количество = Convert.ToInt32(this.nud_NumЗап.Value);
			Запчасть2.Марка_Авто = Запчасть.Марка_Авто;
			Запчасть2.Модель_Авто = Запчасть.Модель_Авто;
			Запчасть2.Контрагент = Data.КонтрагентыList.Find(x => x.id == Data.КонтрагентыList[this.cb_Поставщики.SelectedIndex].id).Data;
			Data.TBL.ЗапчастиМоделиЗак.Add(Запчасть2);
			Data.DB.TMP_Заказ.Запчасти.Add(new(Запчасть.id, 1, Data.КонтрагентыList[this.cb_Поставщики.SelectedIndex].id));
		} else {
			Data.TBL.ЗапчастиМоделиЗак[Data.TBL.ЗапчастиМоделиЗак.ToList().FindIndex(x => x.Название == Запчасть.Название && x.Контрагент == (string)this.cb_Поставщики.SelectedItem)].Количество += Convert.ToInt32(this.nud_NumЗап.Value);
			Data.DB.TMP_Заказ.Запчасти[запindex].Количество += Convert.ToInt32(this.nud_NumЗап.Value);

		}

		this.dg_Запчасти.SelectedIndex = -1;
		this.nud_NumЗап.Visibility = Visibility.Hidden;
		this.cb_Поставщики.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
		this.b_Clear.IsEnabled = true;
		this.b_Done.IsEnabled = true;
		//this.b_Add.IsEnabled = false;
		UpdateTable_Зак();
	}

	private void b_Cancelmini_Click(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.SelectedIndex = -1;
		this.nud_NumЗап.Visibility = Visibility.Hidden;
		this.cb_Поставщики.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
	}

	private void b_Done_Click(object sender, RoutedEventArgs e) {
		DB.DB_Заказы.Add(Data.DB.TMP_Заказ);
		Data.DB.TMP_Заказ = null;
		Data.MainWin.Dispatcher.Invoke(() => { Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders; });
		Data.TBL.ЗапчастиМоделиЗак.Clear();
		UpdateTable_Зак();
	}
	

	private void b_Exit_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders;
		});
	}

	private void b_Clear_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Заказ != null) {
			Data.TBL.ЗапчастиМоделиЗак.Clear();
			Data.DB.TMP_Заказ.Запчасти.Clear();
			this.b_Clear.IsEnabled = false;
			this.b_Done.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
			this.b_Del.IsEnabled = false;
			UpdateTable_Зак();
		}
	}

	private void b_Del_Click(object sender, RoutedEventArgs e) {
		Data.DB.TMP_Заказ.Запчасти.RemoveAt(this.dg_Заказ.SelectedIndex);
		Data.TBL.ЗапчастиМоделиЗак.RemoveAt(this.dg_Заказ.SelectedIndex);
		UpdateTable_Зак();
		this.dg_Запчасти.SelectedIndex = -1;
		this.b_Del.IsEnabled = false;
		this.b_Edit.IsEnabled = false;

		if (Data.DB.TMP_Заказ.Запчасти.Count == 0) {
			this.b_Clear.IsEnabled = false;
			this.b_Done.IsEnabled = false;
		}

		if (isOrdEdit) {
			isOrdEdit = false;
			AnimateButtons(false);
		}
	}


	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable_Зап();
	}

	private void dg_Заказ_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		this.b_Del.IsEnabled = true;
		this.b_Edit.IsEnabled = true;
	}

	private void nud_NumЗап_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e) {
		if (this.nud_NumЗап.Value != null && this.cb_Поставщики.SelectedIndex != -1) {
			this.b_Addmini.IsEnabled = true;
		} else { this.b_Addmini.IsEnabled = false; }
    }

	private void cb_Поставщики_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.nud_NumЗап.Value != null && this.cb_Поставщики.SelectedIndex != -1) {
			this.b_Addmini.IsEnabled = true;
		} else { this.b_Addmini.IsEnabled = false; }
	}

	private void cb_Категория_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (cb_Категории.SelectedIndex == -1) {
			UpdateTable_Зап(); return;
		}
		this.Категорияid = Data.КатегорииList[cb_Категории.SelectedIndex].id;
		UpdateTable_Зап();
	}

	private void cb_Марка_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (cb_Марки.SelectedIndex == -1) {
			UpdateTable_Зап(); return;
		}
		this.Маркаid = Data.МаркиАвтоList[cb_Марки.SelectedIndex].id;
		UpdateTable_Зап();
	}
	private void dg_Запчасти_Selected(object sender, SelectionChangedEventArgs e) {
		this.nud_NumЗап.Visibility = Visibility.Visible;
		this.cb_Поставщики.Visibility = Visibility.Visible;
		this.g_minibut.Visibility = Visibility.Visible;
	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {

	}
}
