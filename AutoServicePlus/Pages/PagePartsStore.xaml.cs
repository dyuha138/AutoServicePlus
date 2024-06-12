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
using MahApps.Metro.Controls;

namespace AutoServicePlus.Pages;


public partial class PagePartsStore : MetroWindow {

	private int Модель_id = 0;
	public ObservableCollection<TBL_ЗапчастьМини> ЗапчастиМини = new();

	public PagePartsStore(int Модель_id) {
        InitializeComponent();
        this.dg_Запчасти.ItemsSource = this.ЗапчастиМини;
		this.Модель_id = Модель_id;
		UpdateTable();
    }

	private void _Loaded(object sender, RoutedEventArgs e) {
		//this.dg_Запчасти.Columns[0].Visibility = Visibility.Hidden;
	}


	private void UpdateTable() {
		SQLResultTable ResTbl = DB.SQLQuery($"SELECT Зап.id, Зап.Идентификатор FROM AutoServicePlus.Запчасти Зап;");
		if (ResTbl != null) {
			while (ResTbl.NextRow()) {
				this.ЗапчастиМини.Add(new(ResTbl.GetInt(0), ResTbl.GetStr(1)));
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
