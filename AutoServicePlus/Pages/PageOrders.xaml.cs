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
	private int статусid = 0;
	private int заказid = 0;

    public PageOrders() {
        InitializeComponent();
		this.dg_Заказы.ItemsSource = Data.TBL.Заказы;

		for (int i = 0; i < Data.DB.СтатусыList.Count; i++) {
			this.cb_Статусы.Items.Add(Data.DB.СтатусыList[i].Статус);
		}
	}

	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Заказы.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Заказы.Columns[0].Header = "Номер";
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = null;
		string unixtime = "";
		string unixtime2 = "";
		if (this.dp_Date.SelectedDate != null) {
			unixtime = TechFuncs.DateToUnix(this.dp_Date.SelectedDate.Value).ToString();
			unixtime2 = (Convert.ToInt32(unixtime) + 86400).ToString();
			ResTbl = DB.SQLQuery($"SELECT Зак.id, Зак.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заказы Зак\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Зак.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Зак.Сотрудник_id = Сотр.id\r\nWHERE (Зак.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%') AND (Зак.Дата >= '{unixtime}' AND Зак.Дата <= '{unixtime2}')\r\nORDER BY Зак.Дата DESC;");
		} else {
			ResTbl = DB.SQLQuery($"SELECT Зак.id, Зак.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заказы Зак\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Ord.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Зак.Сотрудник_id = Сотр.id\r\nWHERE Зак.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%'\r\nORDER BY Зак.Дата DESC;");
		}

		Data.TBL.Заказы.Clear();
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.Заказы.Add(new(ResTbl.GetInt(0), TechFuncs.UnixToDate(ResTbl.GetInt(1)), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
		}
		this.dg_Заказы.Items.Refresh();
	}


	public void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		UpdateTable();
	}

	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			if (Open) {
				int we = 370;
				while (we < 506) {
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 48); });
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
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 48); });
					Thread.Sleep(2);
					we--;
				}
			}
		});
	}


	private void b_Add_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.PageNewOrder = new();
		Data.MainWin.HambMenu.Content = Data.MainWin.PageNewOrder;
    }

	private void b_AddParts_Click(object sender, RoutedEventArgs e) {
		int закid = ((TBL_Заказ)this.dg_Заказы.SelectedItem).id;
		if (this.dg_Заказы.SelectedIndex != -1 && Data.MainWin.PageNewParts == null && закid != заказid) {
			Data.MainWin.PageNewParts = new(закid);
		}
		Data.MainWin.HambMenu.Content = Data.MainWin.PageNewParts;
		заказid = закid;
	}

	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		if (isOrdEdit) {
			isOrdEdit = false;

			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			int статid = Data.DB.СтатусыList[this.cb_Статусы.SelectedIndex].id;
			if (статусid != статid) {
				Data.TBL.Заказы[Data.TBL.Заказы.ToList().FindIndex(x => x.id == Заказ.id)].Статус = Data.DB.СтатусыList.Find(x => x.id == статid).Статус;
				DB.DB_Заказы.StatusUpdate(Заказ.id, статid);
				UpdateTable();
			}
			AnimateButtons(false);
		} else {
			isOrdEdit = true;

			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			this.статусid = Data.DB.ЗаказыList.Find(x => x.id == Заказ.id).Статус_id;
			this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == this.статусid);
			AnimateButtons(true);
		}
	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {
		
	}


	private void dg_Заказы_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.dg_Заказы.SelectedIndex == -1) {
			this.b_Cancel.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
			this.b_AddParts.IsEnabled = false;
		} else {
			TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
			if (Заказ.Статус == "Оформление" || Заказ.Статус == "Оформлен" || Заказ.Статус == "В пути") {
				this.b_Cancel.IsEnabled = true;
			} else {
				this.b_Cancel.IsEnabled = false;
			}
			if (Заказ.Статус == "Приход") {
				this.b_AddParts.IsEnabled = true;
			} else {
				this.b_AddParts.IsEnabled = false;
			}
			this.b_Edit.IsEnabled = true;
			if (isOrdEdit) {
				this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == Data.DB.ЗаказыList.Find(x => x.id == Заказ.id).Статус_id);
			}
		}
	}

	private void dg_Заказы_DoubleClick(object sender, MouseButtonEventArgs e) {
		TBL_Заказ Заказ = (TBL_Заказ)this.dg_Заказы.SelectedItem;
		if (Заказ != null) {
			Pages.PagePartsforOrder форма = new(Заказ.id);
			форма.Show();
		}
		//if (this.dg_Заказы.SelectedIndex != -1 && Data.MainWin.PagePartsforOrder == null) {
			//Data.MainWin.PagePartsforOrder =  new(Заказ.id);
			//Data.MainWin.PagePartsforOrder.Show();
		//}
	}

	private void e_Search_Changed(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}

	private void dp_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
		UpdateTable();
	}
}
