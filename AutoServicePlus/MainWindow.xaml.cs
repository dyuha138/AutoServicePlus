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
	private Pages.PageStorage PageStorage = null;
	private Pages.PageOrders PageOrders = null;
	private Pages.PageAbout PageAbout = null;
	private Pages.PageDB PageDB = null;


	public MainWindow() {
		Data.MainWin = this;
		InitializeComponent();
		Data.HamburgerMenu.Ev_IndexChanged += this.Data_Ev_HambMenuIndexChanged;
		Data.HamburgerMenu.Ev_IndexOptionsChanged += this.Data_Ev_HambMenuOptionsIndexChanged;
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
				}
				this.HambMenu.Content = this.PageOrders;
			break;

			case 3:
				this.Title = "АвтоСервис+: База данных";
				if (this.PageDB == null) {
					this.PageDB = new();
				}
				this.HambMenu.Content = this.PageDB;
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
		}
	}
}