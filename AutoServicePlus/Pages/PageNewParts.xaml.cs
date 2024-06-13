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


public partial class PageNewParts : UserControl {

	
	private bool isOrdEdit = false;
	private bool isClearMoved = false;

	private int Заказ_id = 0;

	public PageNewParts(int Заказ_id) {
        InitializeComponent();
        this.dg_Запчасти.ItemsSource = Data.TBL.Запчасти;
		this.dg_ЗапчастиЗаказ.ItemsSource = Data.TBL.ЗапчастиМоделиЗак;
		this.Заказ_id = Заказ_id;
		UpdateTable_Зак();
		Data.MainWin.Title = "Автосервис+: Оформление прихода запчастей";
    }

	private void _Loaded(object sender, RoutedEventArgs e) {
		this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
		this.dg_ЗапчастиЗаказ.Columns[0].Visibility = Visibility.Hidden;
	}


	private void UpdateTable_Зак() {
		SQLResultTable ResTbl = DB.SQLQuery($"SELECT Parts.id, Parts.Название, Cat.Название, OrdPart.Количество, Marks.Марка, Cars.Модель, Конт.Название FROM AutoServicePlus.ЗапчастиМодели Parts\r\nLEFT JOIN AutoServicePlus.КатегорииЗап Cat ON Parts.Категория_id = Cat.id\r\nLEFT JOIN AutoServicePlus.АвтомобильЗапчасть AutoPart ON AutoPart.Запчасть_id = Parts.id\r\nLEFT JOIN AutoServicePlus.Автомобили Cars ON AutoPart.Автомобиль_id = Cars.id\r\nLEFT JOIN AutoServicePlus.МаркиАвто Marks ON Cars.Марка_id = Marks.id\r\nINNER JOIN AutoServicePlus.ЗаказЗапчасть OrdPart ON OrdPart.Запчасть_id = Parts.id\r\nLEFT JOIN AutoServicePlus.Контрагенты Конт ON OrdPart.Контрагент_id = Конт.id\r\nWHERE OrdPart.Заказ_id = {this.Заказ_id} AND (Parts.Название LIKE '%{this.e_Search.Text}%' OR Cat.Название LIKE '%{this.e_Search.Text}%' OR Marks.Марка LIKE '%{this.e_Search.Text}%' OR Cars.Модель LIKE '%{this.e_Search.Text}%' OR Конт.Название LIKE '%{this.e_Search.Text}%');");
		Data.TBL.ЗапчастиМоделиЗак.Clear();
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				Data.TBL.ЗапчастиМоделиЗак.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1), ResTbl.GetStr(2), ResTbl.GetInt(3), ResTbl.GetStr(4), ResTbl.GetStr(5), ResTbl.GetStr(6)));
			}
		}

		//if (Data.DB.TMP_Запчасти != null) {
		//	for (int i = 0; i < Data.TBL.Склад.Count; i++) {
		//		List<DBM_Заявка.Запчасть> Запчасти = Data.DB.TMP_Запчасти.FindAll(x => x.Модель_id == Data.TBL.Склад[i].id);
		//		if (Запчасти.Count != 0) {
		//			Data.TBL.Склад[i].Количество -= Запчасти.Count;
		//		}
		//	}
		//}

		this.dg_ЗапчастиЗаказ.Items.Refresh();
	}


	private void UpdateTable_Зап() {
		this.dg_Запчасти.Items.Refresh();
	}

	private void _Closed(object sender, EventArgs e) {
		Data.MainWin.PageNewParts = null;
	}


	private void AnimateButtons(bool Open) {
		Task.Factory.StartNew(() => {
			int hetbl = 0;
			this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Запчасти.ActualHeight); });

			if (Open) {
				int he = 50;
				int he2 = 8;
				Task.Factory.StartNew(() => {
					while (he < 91) {
						this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
						Thread.Sleep(7);
						he++;
					}
					this.Dispatcher.Invoke(() => { this.e_ИдентификаторEdit.Visibility = Visibility.Visible; });
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
				this.Dispatcher.Invoke(() => { this.e_ИдентификаторEdit.Visibility = Visibility.Hidden; });
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

	private void b_Addmini_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Запчасти == null) {
			Data.DB.TMP_Запчасти = new();
		}

		TBL_ЗапчастьМодель2 ЗапчастьМодельTBL = (TBL_ЗапчастьМодель2)this.dg_ЗапчастиЗаказ.SelectedItem;
		//DBM_ЗапчастьМодель ЗапчастьМодель = DB.DB_ЗапчастиМодели.Find(ЗапчастьМодельTBL.id);

		if (DB.DB_Запчасти.isExists(this.e_Идентификатор.Text) == 0 && Data.TBL.Запчасти.ToList().FindIndex(x => x.Идентификатор == this.e_Идентификатор.Text) == -1) {
			Data.DB.TMP_Запчасти.Add(new(0, ЗапчастьМодельTBL.id, this.e_Идентификатор.Text));
			Data.TBL.Запчасти.Add(new(0, ЗапчастьМодельTBL.Название, ЗапчастьМодельTBL.Категория, this.e_Идентификатор.Text));

			int колво = Data.TBL.ЗапчастиМоделиЗак[this.dg_ЗапчастиЗаказ.SelectedIndex].Количество;
			if (колво == 1) {
				Data.TBL.ЗапчастиМоделиЗак.RemoveAt(this.dg_ЗапчастиЗаказ.SelectedIndex);
				this.dg_ЗапчастиЗаказ.SelectedIndex = -1;
			} else {
				Data.TBL.ЗапчастиМоделиЗак[this.dg_ЗапчастиЗаказ.SelectedIndex].Количество = колво - 1;
			}

			UpdateTable_Зап();
			this.dg_ЗапчастиЗаказ.Items.Refresh();
			this.e_Идентификатор.Text = null;
			//this.e_Идентификатор.Visibility = Visibility.Hidden;
			//this.g_minibut.Visibility = Visibility.Hidden;
			this.b_Clear.IsEnabled = true;
			if (Data.TBL.ЗапчастиМоделиЗак.Count == 0) {
				this.b_Done.IsEnabled = true;
				this.e_Идентификатор.Visibility = Visibility.Hidden;
				this.g_minibut.Visibility = Visibility.Hidden;
			}
			//this.b_Add.IsEnabled = false;
			this.l_Out.Content = "";

		} else {
			this.l_Out.Content = "Такая запчасть уже существует";
		}
	}

	private void b_Cancelmini_Click(object sender, RoutedEventArgs e) {
		this.dg_ЗапчастиЗаказ.SelectedIndex = -1;
		this.e_Идентификатор.Text = null;
		this.e_Идентификатор.Visibility = Visibility.Hidden;
		this.g_minibut.Visibility = Visibility.Hidden;
	}

	private void b_Done_Click(object sender, RoutedEventArgs e) {
		long udate = TechFuncs.GetUnixTime();
		int logid = TechFuncs.ПолучитьАйдиВхода();
		int запid = 0;
		for (int i = 0; i < Data.DB.TMP_Запчасти.Count; i++) {
			запid = DB.DB_Запчасти.Add(Data.DB.TMP_Запчасти[i]);
			DB.DB_РегистрЗапчастей.Add(new(0, запid, udate, 2, logid));
		}
		Data.DB.TMP_Запчасти.Clear();
		DB.DB_Заказы.StatusUpdate(this.Заказ_id, 2);

		Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders;
		Data.MainWin.PageNewParts = null;
	}


	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		if (isOrdEdit) {
			isOrdEdit = false;
			if (Data.TBL.Запчасти.ToList().FindIndex(x => x.Идентификатор == this.e_Идентификатор.Text) == -1) {
				Data.DB.TMP_Запчасти[this.dg_Запчасти.SelectedIndex].Идентификатор = this.e_ИдентификаторEdit.Text;
				Data.TBL.Запчасти[this.dg_Запчасти.SelectedIndex].Идентификатор = this.e_ИдентификаторEdit.Text;
				UpdateTable_Зап();
				this.l_Out.Content = "";
				AnimateButtons(false);
			} else {
				this.l_Out.Content = "Такая запчасть уже существует";
			}
		} else {
			isOrdEdit = true;
			this.e_ИдентификаторEdit.Text = ((TBL_Запчасть)this.dg_Запчасти.SelectedItem).Идентификатор;
			AnimateButtons(true);
		}
	}

	private void b_Clear_Click(object sender, RoutedEventArgs e) {
		if (Data.DB.TMP_Запчасти != null) {
			Data.TBL.Запчасти.Clear();
			Data.DB.TMP_Запчасти.Clear();
			this.b_Clear.IsEnabled = false;
			this.b_Done.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
			this.b_Del.IsEnabled = false;
			this.e_Идентификатор.Visibility = Visibility.Hidden;
			this.g_minibut.Visibility = Visibility.Hidden;
			UpdateTable_Зак();
			UpdateTable_Зап();
		}
	}

	private void b_Del_Click(object sender, RoutedEventArgs e) {
		int indexзапмод = Data.TBL.ЗапчастиМоделиЗак.ToList().FindIndex(x => x.Название == ((TBL_Запчасть)this.dg_Запчасти.SelectedItem).Название);
		if (indexзапмод != -1) {
			int колво = Convert.ToInt32(Data.TBL.ЗапчастиМоделиЗак[indexзапмод].Количество);
			Data.TBL.ЗапчастиМоделиЗак[indexзапмод].Количество = ++колво;
		} else {

			//Data.TBL.ЗапчастиМоделиЗак.Add(new(Data.DB.TMP_Запчасти[indexзапмод].Модель_id, Data.TBL.ЗапчастиМодели))
		}

		Data.DB.TMP_Запчасти.RemoveAt(this.dg_Запчасти.SelectedIndex);
		Data.TBL.Запчасти.RemoveAt(this.dg_Запчасти.SelectedIndex);
		UpdateTable_Зап();
		this.dg_ЗапчастиЗаказ.Items.Refresh();
		this.dg_Запчасти.SelectedIndex = -1;
		this.b_Del.IsEnabled = false;
		this.b_Edit.IsEnabled = false;

		if (isOrdEdit) {
			isOrdEdit = false;
			AnimateButtons(false);
		}
	}


	private void e_Search_TextChanged(object sender, TextChangedEventArgs e) {
		UpdateTable_Зак();
	}

	private void e_Идентификатор_TextChanged(object sender, TextChangedEventArgs e) {
		if (this.e_Идентификатор.Text != null && this.e_Идентификатор.Text != "") {
			this.b_Addmini.IsEnabled = true;
		} else { this.b_Addmini.IsEnabled = false; }
	}

	private void dg_Запчасти_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.dg_Запчасти.SelectedIndex != -1) {
			this.b_Del.IsEnabled = true;
			this.b_Edit.IsEnabled = true;
		} else {
			this.b_Del.IsEnabled = false;
			this.b_Edit.IsEnabled = false;
		}
	}

	private void dg_ЗапчастиМодели_Selected(object sender, SelectionChangedEventArgs e) {
		if (this.dg_ЗапчастиЗаказ.SelectedIndex != -1) {
			this.e_Идентификатор.Visibility = Visibility.Visible;
			this.g_minibut.Visibility = Visibility.Visible;
		} else {
			this.e_Идентификатор.Visibility = Visibility.Hidden;
			this.g_minibut.Visibility = Visibility.Hidden;
		}
		
	}

	private void b_Exit_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageOrders;
		});
	}
}
