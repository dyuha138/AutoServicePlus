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

	private bool isOrdEdit = false;
	private int статусid = 0;
	private int заявкаid = 0;

	public PageRequests() {
        InitializeComponent();
		//Data.PropertiesChange.WinHeight = 190;
		this.dg_Заявки.ItemsSource = Data.TBL.Заявки;

		for (int i = 0; i < Data.DB.СтатусыList.Count; i++) {
			this.cb_Статусы.Items.Add(Data.DB.СтатусыList[i].Статус);
		}
	}

	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Заявки.Columns[0].Visibility = Visibility.Hidden;
		this.dg_Заявки.Columns[0].Header = "Номер";
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = null;
		string unixtime = "";
		string unixtime2 = "";
		if (this.dp_Date.SelectedDate != null) {
			unixtime = TechFuncs.DateToUnix(this.dp_Date.SelectedDate.Value).ToString();
			unixtime2 = (Convert.ToInt32(unixtime) + 86400).ToString();
			ResTbl = DB.SQLQuery($"SELECT Зая.id, Зая.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заявки Зая\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Зая.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Зая.Сотрудник_id = Сотр.id\r\nWHERE (Зая.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%') AND (Зая.Дата >= '{unixtime}' AND Зая.Дата <= '{unixtime2}')\r\nORDER BY Зая.Дата DESC;");
		} else {
			ResTbl = DB.SQLQuery($"SELECT Зая.id, Зая.Дата, Ст.Статус, Сотр.Фамилия, Сотр.Имя, Сотр.Отчество FROM AutoServicePlus.Заявки Зая\r\nLEFT JOIN AutoServicePlus.Статусы Ст ON Зая.Статус_id = Ст.id\r\nLEFT JOIN AutoServicePlus.Сотрудники Сотр ON Зая.Сотрудник_id = Сотр.id\r\nWHERE Зая.id LIKE '%{this.e_Search.Text}%' OR Ст.Статус LIKE '%{this.e_Search.Text}%' OR Сотр.Фамилия LIKE '%{this.e_Search.Text}%' OR Сотр.Имя LIKE '%{this.e_Search.Text}%' OR Сотр.Отчество LIKE '%{this.e_Search.Text}%'\r\nORDER BY Зая.Дата DESC;");
		}

		Data.TBL.Заявки.Clear();
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.Заявки.Add(new(ResTbl.GetInt(0), TechFuncs.UnixToDate(ResTbl.GetInt(1)), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5)));
			}
		}
		this.dg_Заявки.Items.Refresh();
	}


	public void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		UpdateTable();
	}

	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			if (Open) {
				int we = 377;
				while (we < 513) {
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 35); });
					Thread.Sleep(2);
					we++;
				}
				this.Dispatcher.Invoke(() => { this.cb_Статусы.Visibility = Visibility.Visible; });
			} else {
				this.Dispatcher.Invoke(() => { this.cb_Статусы.Visibility = Visibility.Hidden; });
				int we = 512;
				while (we > 376) {
					this.Dispatcher.Invoke(() => { this.b_Cancel.Margin = new(we, 0, 0, 35); });
					Thread.Sleep(2);
					we--;
				}
			}
		});
	}


	private void b_Add_Click(object sender, RoutedEventArgs e) {
        Data.MainWin.PageNewRequest = new();
        Data.MainWin.HambMenu.Content = Data.MainWin.PageNewRequest;
    }

	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		if (isOrdEdit) {
			isOrdEdit = false;

			TBL_Заявка Заявка = (TBL_Заявка)this.dg_Заявки.SelectedItem;
			int статid = Data.DB.СтатусыList[this.cb_Статусы.SelectedIndex].id;
			if (статусid != статid) {
				Data.TBL.Заявки[Data.TBL.Заявки.ToList().FindIndex(x => x.id == Заявка.id)].Статус = Data.DB.СтатусыList.Find(x => x.id == статid).Статус;
				DB.DB_Заявки.StatusUpdate(Заявка.id, статid);
				UpdateTable();
			}
			AnimateButtons(false);
		} else {
			isOrdEdit = true;

			TBL_Заявка Заявка = (TBL_Заявка)this.dg_Заявки.SelectedItem;
			this.статусid = Data.DB.ЗаявкиList.Find(x => x.id == Заявка.id).Статус_id;
			this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == this.статусid);
			AnimateButtons(true);
		}
	}

	private void b_Cancel_Click(object sender, RoutedEventArgs e) {

	}


	private void dg_Заявки_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.dg_Заявки.SelectedIndex == -1) {
			this.b_Cancel.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
		} else {
			TBL_Заявка Заявка = (TBL_Заявка)this.dg_Заявки.SelectedItem;
			if (Заявка.Статус == "Оформление" || Заявка.Статус == "Оформлен") {
				this.b_Cancel.IsEnabled = true;
			} else {
				this.b_Cancel.IsEnabled = false;
			}
			this.b_Edit.IsEnabled = true;
			if (isOrdEdit) {
				this.cb_Статусы.SelectedIndex = Data.DB.СтатусыList.FindIndex(x => x.id == Data.DB.ЗаказыList.Find(x => x.id == Заявка.id).Статус_id);
			}
		}
	}

	private void dg_Заявки_DoubleClick(object sender, MouseButtonEventArgs e) {
		TBL_Заявка Заявка = (TBL_Заявка)this.dg_Заявки.SelectedItem;
		if (Заявка != null) {
			Pages.PagePartsforReq форма = new(Заявка.id);
			форма.Show();
		}
	}

	private void e_Search_Changed(object sender, TextChangedEventArgs e) {
		UpdateTable();
	}

	private void dp_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
		UpdateTable();
	}

	private void b_Cancel_Click_1(object sender, RoutedEventArgs e) {
		TBL_Заявка Заявка = (TBL_Заявка)this.dg_Заявки.SelectedItem;
		Data.TBL.Заявки[Data.TBL.Заявки.ToList().FindIndex(x => x.id == Заявка.id)].Статус = Data.DB.СтатусыList.Find(x => x.id == 16).Статус;
		DB.DB_Заявки.StatusUpdate(Заявка.id, 16);
		UpdateTable();
	}
}

