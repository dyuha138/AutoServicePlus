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


public partial class PageNewOrder : UserControl {

	private bool isOrdEdit = false;
	private bool isClearMoved = false;

	private int Категорияid = 0;
	private int Маркаid = 0;

	public PageNewOrder() {
        InitializeComponent();
		Data.PropertiesChange.WinHeight = 343;
		this.dg_Запчасти.ItemsSource = Data.TBL.TBLData_Запчасти;
		this.dg_Заказ.ItemsSource = Data.TBL.TBLData_Запчасти2;

		DB.DB_Справочники.UpdateListfromTable(Data.КатегорииList, "КатегорииЗап");
		for (int i = 0; i < Data.КатегорииList.Count; i++) {
			this.cb_Категории.Items.Add(Data.КатегорииList[i].Data);
		}

		DB.DB_Справочники.UpdateListfromTable(Data.МаркиАвтоList, "МаркиАвто");
		for (int i = 0; i < Data.МаркиАвтоList.Count; i++) {
			this.cb_Марки.Items.Add(Data.МаркиАвтоList[i].Data);
		}

		DB.DB_Справочники.UpdateListfromTable(Data.КонтрагентыList, "Контрагенты");
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
	}


	private void UpdateTable_Зап() {
		SQLResultTable ResTbl = null;
		bool edit = false;
		string sql = "SELECT Зап.id, Зап.Название, Кат.Название, 0, Marks.Марка, Cars.Модель, 0 FROM AutoServicePlus.Запчасти Зап\r\nINNER JOIN AutoServicePlus.КатегорииЗап Кат ON Зап.Категория_id = Кат.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть AutoPart ON AutoPart.Запчасть_id = Зап.id\r\nLEFT JOIN AutoServicePlus.Автомобили Cars ON AutoPart.Автомобиль_id = Cars.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Marks ON Cars.Марка_id = Marks.id";

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
		Data.TBL.TBLData_Запчасти.Clear();

		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.TBLData_Запчасти.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetStr(3), ResTbl.GetStr(4), ResTbl.GetStr(5), ResTbl.GetStr(6)));
			}
		}
		this.dg_Запчасти.Items.Refresh();
	}

	private void UpdateTable_Зак() {
		this.dg_Заказ.Items.Refresh();
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


	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable_Зап();
	}

	private void dg_Запчасти_Selected(object sender, SelectionChangedEventArgs e) {
		this.nud_NumЗап.Visibility = Visibility.Visible;
		this.cb_Поставщики.Visibility = Visibility.Visible;
		this.g_minibut.Visibility = Visibility.Visible;
	}

	private void b_Cancelmini_Click(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.SelectedIndex = -1;
		this.nud_NumЗап.Visibility = Visibility.Hidden;
		this.cb_Поставщики.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
	}


	private void b_Add_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Заказ == null) {
			Data.DB.TMP_Заказ = new(0, TechFuncs.GetUnixTime(), 7, 1, new());
		}

		TBL_Запчасть Запчасть = (TBL_Запчасть)this.dg_Запчасти.SelectedItem;
		TBL_Запчасть Запчасть2 = Data.TBL.TBLData_Запчасти.ToList().Find(x => x.id == Запчасть.id);
		Запчасть2.Количество = this.nud_NumЗап.Value.ToString();
		Запчасть2.Контрагент = Data.КонтрагентыList[this.cb_Поставщики.SelectedIndex].Data;
		Data.TBL.TBLData_Запчасти2.Add(Запчасть2);

		Data.DB.TMP_Заказ.Запчасти.Add(new(Запчасть.id, Convert.ToInt32(Запчасть2.Количество), Data.КонтрагентыList[this.cb_Поставщики.SelectedIndex].id));

		this.dg_Запчасти.SelectedIndex = -1;
		this.nud_NumЗап.Visibility = Visibility.Hidden;
		this.cb_Поставщики.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
		this.b_Clear.IsEnabled = true;
	}



	private void b_Done_Click(object sender, RoutedEventArgs e) {
		DB.DB_Заказы.Add(Data.DB.TMP_Заказ);
		Data.DB.TMP_Заказ = null;
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders;
		});
		Data.TBL.TBLData_Запчасти2.Clear();
		UpdateTable_Зак();
	}



	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		Task.Factory.StartNew(() => {
			int hetbl = 0;
			this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Заказ.ActualHeight); });

			if (isOrdEdit) {
				isOrdEdit = false;
				this.Dispatcher.Invoke(() => {
					Data.DB.TMP_Заказ.Запчасти[this.dg_Заказ.SelectedIndex].Количество = Convert.ToInt32(this.nud_NumЗаказ.Value);
					Data.TBL.TBLData_Запчасти2[this.dg_Заказ.SelectedIndex].Количество = this.nud_NumЗаказ.Value.ToString();
					UpdateTable_Зак();
				});

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
			} else {
				isOrdEdit = true;
				this.Dispatcher.Invoke(() => { this.nud_NumЗаказ.Value = Convert.ToInt32(Data.DB.TMP_Заказ.Запчасти[this.dg_Заказ.SelectedIndex].Количество); });

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
			}
		});
	}

	private void b_Exit_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders;
		});
	}

	private void b_Clear_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Заказ.Запчасти != null) {
			Data.TBL.TBLData_Запчасти2.Clear();
			Data.DB.TMP_Заказ.Запчасти.Clear();
			this.b_Clear.IsEnabled = false;
			UpdateTable_Зак();
		}
	}

	private void b_Del_Click(object sender, RoutedEventArgs e) {
		Data.DB.TMP_Заказ.Запчасти.RemoveAt(this.dg_Заказ.SelectedIndex);
		Data.TBL.TBLData_Запчасти2.RemoveAt(this.dg_Заказ.SelectedIndex);
		UpdateTable_Зак();
		this.dg_Запчасти.SelectedIndex = -1;
		this.b_Del.IsEnabled = false;
		this.b_Edit.IsEnabled = false;
	}

	private void dg_Заказ_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		this.b_Del.IsEnabled = true;
		this.b_Edit.IsEnabled = true;
	}
}
