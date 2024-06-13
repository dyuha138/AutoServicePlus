using System.Text;
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

namespace AutoServicePlus;


public partial class MainWindow : MetroWindow {

	public HamburgerMenu HambMenu = null;
	public Pages.PageStorage PageStorage = null;
	public Pages.PageOrders PageOrders = null;
	public Pages.PageNewOrder PageNewOrder = null;
	//public Pages.PagePartsforOrder PagePartsforOrder = null;
	public Pages.PageRequests PageRequests = null;
	public Pages.PageNewRequest PageNewRequest = null;
	public Pages.PageNewParts PageNewParts = null;
	//public Pages.PagePartsStore PagePartsStore = null;
	public Pages.PageAbout PageAbout = null;
	//public Pages.PageDB PageDB = null;


	public MainWindow() {
		Data.MainWin = this;
		InitializeComponent();
		Data.HamburgerMenu.Ev_IndexChanged += this.Data_Ev_HambMenuIndexChanged;
		Data.HamburgerMenu.Ev_IndexOptionsChanged += this.Data_Ev_HambMenuOptionsIndexChanged;
		//Data.MainWin.SetBinding(MinHeightProperty, "WinHeight");
		//DB.Open();
	}

	public void SetHambMenu(HamburgerMenu hambmenu) {
		if (HambMenu == null) {	HambMenu = hambmenu; }
	}



	private void Data_Ev_HambMenuIndexChanged(object sender, Twident_Int e) {
		switch (e.Value) {
			case 0:
				this.Title = "АвтоСервис+: Склад";
				if (this.PageStorage == null) {
					this.PageStorage = new();
				}
				this.HambMenu.Content = this.PageStorage;
			break;

			case 2:
				this.Title = "АвтоСервис+: Заказы запчастей";
				if (this.PageOrders == null) {
					this.PageOrders = new();
					//this.PageOrders.dg_Заказы.Columns[0].Visibility = Visibility.Hidden;
				}
				
				this.HambMenu.Content = this.PageOrders;
			break;

			case 3:
				this.Title = "АвтоСервис+: Заявки на отгрузку";
				if (this.PageRequests == null) {
					this.PageRequests = new();
				}
				this.HambMenu.Content = this.PageRequests;
			break;

			case 4:
				//this.Title = "АвтоСервис+: База данных";
				//if (this.PageDB == null) {
				//	this.PageDB = new();
				//}
				//this.HambMenu.Content = this.PageDB;
			break;
		}
	}

	private void Data_Ev_HambMenuOptionsIndexChanged(object sender, Twident_Int e) {
		switch (e.Value) {
			case 0:
				this.Title = "АвтоСервис+: О программе";
				if (this.PageAbout == null) {
					this.PageAbout = new();
				}
				this.HambMenu.Content = this.PageAbout;
			break;
			case 1:
				DB.Close();
				//this.Close();
				Application.Current.Shutdown();
			break;
		}
	}

	private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
		DB.Close();
		Application.Current.Shutdown();

	}
}