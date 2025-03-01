﻿using Org.BouncyCastle.Utilities.Encoders;

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


public partial class PageNewRequest : UserControl {

	private bool isReqEdit = false;
	private bool isClearMoved = false;

	private int Категорияid = 0;
	private int Маркаid = 0;

	public PageNewRequest() {
		InitializeComponent();
		Data.PropertiesChange.WinHeight = 423;

		this.dg_Запчасти.ItemsSource = Data.TBL.Склад;
		this.dg_Заявка.ItemsSource = Data.TBL.ЗапчастиМоделиЗая;

		for (int i = 0; i < Data.КатегорииList.Count; i++) {
			this.cb_Категории.Items.Add(Data.КатегорииList[i].Data);
		}
		for (int i = 0; i < Data.МаркиАвтоList.Count; i++) {
			this.cb_Марки.Items.Add(Data.МаркиАвтоList[i].Data);
		}
	}


	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Заказ.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
		//this.dg_Запчасти.Columns[3].Visibility = Visibility.Hidden;
		//this.dg_Заявка.Columns[0].Visibility = Visibility.Hidden;
		//this.dg_Заявка.Columns[4].Visibility = Visibility.Hidden;
		//this.dg_Заявка.Columns[5].Visibility = Visibility.Hidden;
		UpdateTable_Зап();
		UpdateTable_Зая();

		if (Data.TBL.ЗапчастиМоделиЗая.Count > 0) {
			this.b_Clear.IsEnabled = true;
			this.b_Done.IsEnabled = true;
		}
	}


	private void UpdateTable_Зап() {
		SQLResultTable ResTbl = null;
		bool edit = false;
		string sql = "WITH Рег AS (SELECT Запчасть_id, Статус_id, ROW_NUMBER() OVER (PARTITION BY Запчасть_id ORDER BY Дата DESC) AS rn\r\n    FROM AutoServicePlus.РегистрЗапчастей)\r\nSELECT ЗапМ.id, ЗапМ.Название, Кат.Название, COUNT(Зап.id), Марки.Марка, Авто.Модель FROM AutoServicePlus.ЗапчастиМодели ЗапМ\r\nINNER JOIN AutoServicePlus.Запчасти Зап ON Зап.Модель_id = ЗапМ.id\r\nINNER JOIN AutoServicePlus.КатегорииЗап Кат ON ЗапМ.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть АвтоЗап ON АвтоЗап.Запчасть_id = Зап.id\r\nLEFT JOIN AutoServicePlus.Автомобили Авто ON АвтоЗап.Автомобиль_id = Авто.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Марки ON Авто.Марка_id = Марки.id\r\nINNER JOIN Рег ON Зап.id = Рег.Запчасть_id AND Рег.rn = 1\r\nWHERE Рег.Статус_id = 2 AND ";

		if (this.cb_Категории.SelectedIndex != -1 && this.cb_Марки.SelectedIndex != -1) {
			sql += $"ЗапМ.Категория_id = {this.Категорияid} AND Авто.Марка_id = {this.Маркаid}";
			edit = true;
		} else if (cb_Категории.SelectedIndex != -1) {
			sql += $"ЗапМ.Категория_id = {this.Категорияid}";
			edit = true;
		} else if (cb_Марки.SelectedIndex != -1) {
			sql += $"Авто.Марка_id = {this.Маркаid}";
			edit = true;
		}

		if (edit) {
			sql += $" AND ЗапМ.Название LIKE '%{this.e_Search.Text}%';";
		} else {
			sql += $"ЗапМ.Название LIKE '%{this.e_Search.Text}%'";
		}

		sql += "\r\nGROUP BY ЗапМ.id, Марки.id, Авто.id;";

		ResTbl = DB.SQLQuery(sql);
		Data.TBL.Склад.Clear();

		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.Склад.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetInt(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
		}

		if (Data.DB.TMP_Заявка != null) {
			for (int i = 0; i < Data.TBL.Склад.Count; i++) {
				List<DBM_Заявка.Запчасть> Запчасти = Data.DB.TMP_Заявка.Запчасти.FindAll(x => x.Модель_id == Data.TBL.Склад[i].id);
				if (Запчасти.Count != 0) {
					Data.TBL.Склад[i].Количество -= Запчасти.Count;
				}
			}
		}

		this.dg_Запчасти.Items.Refresh();
	}

	private void UpdateTable_Зая() {
		this.dg_Заявка.Items.Refresh();
	}


	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			int hetbl = 0;
			this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Заявка.ActualHeight); });

			if (Open) {
				int he = 90;
				int he2 = 105;
				this.Dispatcher.Invoke(() => { this.nud_NumЗаявка.Visibility = Visibility.Hidden; });
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
			} else {
				int he = 50;
				int he2 = 8;
				Task.Factory.StartNew(() => {
					while (he < 91) {
						this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
						Thread.Sleep(7);
						he++;
					}
					this.Dispatcher.Invoke(() => { this.nud_NumЗаявка.Visibility = Visibility.Visible; });
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
			}
		});
	}


	private void b_Edit_Click(object sender, RoutedEventArgs e) {
	//	if (isReqEdit) {
	//		isReqEdit = false;
	//		Data.DB.TMP_Заявка.Запчасти[this.dg_Заявка.SelectedIndex].Количество = Convert.ToInt32(this.nud_NumЗаявка.Value);
	//		Data.TBL.ЗапчастиМоделиЗая[this.dg_Заявка.SelectedIndex].Количество = Convert.ToInt32(this.nud_NumЗаявка.Value);
	//		UpdateTable_Зая();
	//		AnimateButtons(false);
	//	} else {
	//		isReqEdit = true;
	//		this.nud_NumЗаявка.Value = Convert.ToInt32(Data.DB.TMP_Заявка.Запчасти[this.dg_Заявка.SelectedIndex].Количество);
	//		AnimateButtons(true);
	//	}
	}

	private void b_Addmini_Click(object sender, RoutedEventArgs e) {
		TBL_Склад Запчастьtmp = (TBL_Склад)this.dg_Запчасти.SelectedItem;
		TBL_Запчасть Запчасть = new();

		if (Data.DB.TMP_Заявка == null) {
			Data.DB.TMP_Заявка = new(0, TechFuncs.GetUnixTime(), 7, TechFuncs.ПолучитьАйдиВхода(), new());
		}
		int запid = Data.ЗапчастиList[this.cb_Запчасти.SelectedIndex].id;

		Запчасть.id = запid;
		Запчасть.Название = Запчастьtmp.Название;
		Запчасть.Категория = Запчастьtmp.Категория;
		Запчасть.Идентификатор = (string)this.cb_Запчасти.SelectedItem;
		Data.TBL.ЗапчастиМоделиЗая.Add(Запчасть);
		Data.DB.TMP_Заявка.Запчасти.Add(new(запid, Запчастьtmp.id));

		Data.TBL.Склад[Data.TBL.Склад.ToList().FindIndex(x => x.id == ((TBL_Склад)this.dg_Запчасти.SelectedItem).id)].Количество --;

		//this.dg_Запчасти.SelectedIndex = -1;
		//this.nud_NumЗаявка.Visibility = Visibility.Hidden;
		//this.cb_Запчасти.Visibility = Visibility.Hidden;
		//this.g_minibut.Visibility = Visibility.Hidden;
		FillЗапчасти();
		this.dg_Запчасти.Items.Refresh();
		this.b_Clear.IsEnabled = true;
		this.b_Done.IsEnabled = true;
		
		//this.b_Add.IsEnabled = false;
	}

	private void b_Cancelmini_Click(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.SelectedIndex = -1;
		//this.nud_NumЗаявка.Visibility = Visibility.Hidden;
		this.cb_Запчасти.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
	}

	private void b_Done_Click(object sender, RoutedEventArgs e) {
		DB.DB_Заявки.Add(Data.DB.TMP_Заявка);
		int запid = 0;
		long udate = TechFuncs.GetUnixTime();
		int logid = TechFuncs.ПолучитьАйдиВхода();

		for (int i = 0; i < Data.DB.TMP_Заявка.Запчасти.Count; i++) {
			запid = Data.DB.TMP_Заявка.Запчасти[i].id;
			DB.DB_РегистрЗапчастей.Add(new(0, запid, udate, 3, logid));
		}

		Data.DB.TMP_Заявка = null;
		Data.TBL.ЗапчастиМоделиЗая.Clear();
		Data.MainWin.Dispatcher.Invoke(() => { Data.MainWin.HambMenu.Content = Data.MainWin.PageRequests; });
		//UpdateTable_Зая();
	}


	private void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {

	}

	private void b_Exit_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageRequests;
		});
	}

	private void b_Clear_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Заявка != null) {
			Data.TBL.ЗапчастиМоделиЗая.Clear();
			Data.DB.TMP_Заявка.Запчасти.Clear();
			this.b_Clear.IsEnabled = false;
			this.b_Done.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
			this.b_Del.IsEnabled = false;
			this.cb_Запчасти.Visibility = Visibility.Hidden;
			this.g_minibut.Visibility = Visibility.Hidden;
			UpdateTable_Зая();
			UpdateTable_Зап();
			this.cb_Запчасти.Items.Clear();
			//FillЗапчасти();
		}
	}

	private void b_Del_Click(object sender, RoutedEventArgs e) {
		Data.DB.TMP_Заявка.Запчасти.RemoveAt(this.dg_Заявка.SelectedIndex);
		Data.TBL.ЗапчастиМоделиЗая.RemoveAt(this.dg_Заявка.SelectedIndex);
		//Data.TBL.Склад[Data.TBL.Склад.ToList().FindIndex(x => x.id == ((TBL_Склад)this.dg_Заявка.SelectedItem).id)].Количество++;
		Data.TBL.Склад[Data.TBL.Склад.ToList().FindIndex(x => x.id == ((TBL_Склад)this.dg_Запчасти.SelectedItem).id)].Количество++;
		UpdateTable_Зая();
		this.dg_Запчасти.Items.Refresh();
		this.dg_Заявка.SelectedIndex = -1;
		this.b_Del.IsEnabled = false;
		this.b_Edit.IsEnabled = false;
		FillЗапчасти();

		if (Data.DB.TMP_Заявка.Запчасти.Count == 0) {
			this.b_Clear.IsEnabled = false;
			this.b_Done.IsEnabled = false;
		}

		if (isReqEdit) {
			isReqEdit = false;
			AnimateButtons(false);
		}
	}

	private void FillЗапчасти() {
		List<DBM_Запчасть> запlist = DB.DB_Запчасти.GetListfromModelid(((TBL_Склад)this.dg_Запчасти.SelectedItem).id);
		Data.ЗапчастиList.Clear();
		this.cb_Запчасти.Items.Clear();
		for (int i = 0; i < запlist.Count; i++) {
			if ((Data.DB.TMP_Заявка == null || Data.DB.TMP_Заявка.Запчасти.FindIndex(x => x.id == запlist[i].id) == -1) && !DB.DB_Запчасти.isRequired(запlist[i].id)) {
				Data.ЗапчастиList.Add(new(запlist[i].id, запlist[i].Идентификатор));
				this.cb_Запчасти.Items.Add(запlist[i].Идентификатор);
			}
		}
	}

	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable_Зап();
	}

	private void dg_Заявка_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		this.b_Del.IsEnabled = true;
		//this.b_Edit.IsEnabled = true;
	}

	private void nud_NumЗап_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e) {
		if (this.nud_NumЗап.Value != null && this.cb_Запчасти.SelectedIndex != -1) {
			this.b_Addmini.IsEnabled = true;
		} else { this.b_Addmini.IsEnabled = false; }
	}

	private void cb_Запчасти_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		//if (this.nud_NumЗап.Value != null && this.cb_Запчасти.SelectedIndex != -1) {
		if (this.cb_Запчасти.SelectedIndex != -1) {
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
		if (this.dg_Запчасти.SelectedIndex != -1) {
			//this.nud_NumЗап.Visibility = Visibility.Visible;
			this.cb_Запчасти.Visibility = Visibility.Visible;
			this.g_minibut.Visibility = Visibility.Visible;
			FillЗапчасти();
		}
	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {

	}
}

