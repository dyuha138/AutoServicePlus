using MaterialDesignThemes.Wpf;

using Org.BouncyCastle.Pqc.Crypto.Lms;

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

	private bool isOrdEdit = false;

    public PageOrders() {
        InitializeComponent();
		this.dg_Заказы.ItemsSource = Data.TBL.TBLData_Заказы;

		for (int i = 0; i < Data.DB.СтатусыList.Count; i++) {
			this.cb_Статусы.Items.Add(Data.DB.СтатусыList[i].Статус);
		}
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = null;
		string unixtime = "";
		string unixtime2 = "";
		if (this.dp_Date.SelectedDate != null) {
			unixtime = TechFuncs.DateToUnix(this.dp_Date.SelectedDate.Value).ToString();
			unixtime2 = (Convert.ToInt32(unixtime) + 86400).ToString();
			ResTbl = DB.SQLQuery($"SELECT Ord.id, Ord.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заказы Ord\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Ord.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Ord.Сотрудник_id = Сотр.id\r\nWHERE (Ord.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%') AND (Ord.Дата >= '{unixtime}' AND Ord.Дата <= '{unixtime2}');");
		} else {
			ResTbl = DB.SQLQuery($"SELECT Ord.id, Ord.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заказы Ord\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Ord.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Ord.Сотрудник_id = Сотр.id\r\nWHERE Ord.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%';");
		}

		Data.TBL.TBLData_Заказы.Clear();
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.TBLData_Заказы.Add(new(ResTbl.GetInt(0), TechFuncs.UnixToDate(ResTbl.GetInt(1)), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
		}
		this.dg_Заказы.Items.Refresh();
	}


	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Заказы.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Заказы.Columns[0].Header = "Номер";

	}


	public void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		UpdateTable();
	}

	private void b_Add_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.PageNewOrder = new();
		Data.MainWin.HambMenu.Content = Data.MainWin.PageNewOrder;
    }


	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			if (Open) {
				int we = 370;
				while (we < 506) {
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 33); });
					Thread.Sleep(2);
					we++;
				}
				this.Dispatcher.Invoke(() => {
					this.cb_Статусы.Visibility = Visibility.Visible;
					//this.g_minibut.Visibility = Visibility.Visible;
				});
			} else {
				this.Dispatcher.Invoke(() => {
					this.cb_Статусы.Visibility = Visibility.Hidden;
					//this.g_minibut.Visibility = Visibility.Hidden;
				});
				int we = 505;
				while (we > 369) {
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 33); });
					Thread.Sleep(2);
					we--;
				}
			}
		});
	}


	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		if (isOrdEdit) {
			isOrdEdit = false;

			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			int статid = Data.DB.СтатусыList[this.cb_Статусы.SelectedIndex].id;
			Data.TBL.TBLData_Заказы[Data.TBL.TBLData_Заказы.ToList().FindIndex(x => x.id == Заказ.id)].Статус = Data.DB.СтатусыList.Find(x => x.id == статid).Статус;
			DB.DB_Заказы.StatusUpdate(Заказ.id, статid);
			UpdateTable();
			AnimateButtons(true);
		} else {
			isOrdEdit = true;

			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == Data.DB.ЗаказыList.Find(x => x.id == Заказ.id).Статус_id);
			AnimateButtons(false);
		}
	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {
		
	}

	private void b_Cancelmini_Click(object sender, RoutedEventArgs e) {
		//isOrdEdit = false;
		//AnimateButtons(false);		
	}

	private void b_Donemini_Click(object sender, RoutedEventArgs e) {

	}

	private void dg_Заказы_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.dg_Заказы.SelectedIndex == -1) {
			this.b_Cancel.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
		} else {
			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			if (Заказ.Статус == "Оформление" || Заказ.Статус == "Оформлен" || Заказ.Статус == "В пути") {
				this.b_Cancel.IsEnabled = true;
			} else {
				this.b_Cancel.IsEnabled = false;
			}
			this.b_Edit.IsEnabled = true;
			if (isOrdEdit) {
				this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == Data.DB.ЗаказыList.Find(x => x.id == Заказ.id).Статус_id);
			}
		}
	}

	private void dg_Заказы_DoubleClick(object sender, MouseButtonEventArgs e) {
		TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
		if (this.dg_Заказы.SelectedIndex != -1 && Data.MainWin.PagePartsforOrder == null) {
			Data.MainWin.PagePartsforOrder = new(Заказ.id);
			Data.MainWin.PagePartsforOrder.Show();
		}
	}

	private void e_Search_Changed(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}

	private void dp_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
		UpdateTable();
	}
}
