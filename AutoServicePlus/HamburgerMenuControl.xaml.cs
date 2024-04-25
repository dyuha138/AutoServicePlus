using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoServicePlus;

public partial class HamburgerMenuControl : UserControl {


    public HamburgerMenuControl() {
        InitializeComponent();

        Binding binindex = new Binding();
        binindex.Source = Data.HamburgerMenu;
        binindex.Mode = BindingMode.TwoWay;
        binindex.Path = new System.Windows.PropertyPath("SelectedIndex");
		this.hm_Menu.SetBinding(HamburgerMenu.SelectedIndexProperty, binindex);

		binindex = new Binding();
		binindex.Source = Data.HamburgerMenu;
		binindex.Mode = BindingMode.OneWayToSource;
		binindex.Path = new System.Windows.PropertyPath("SelectedOptionsIndex");
		this.hm_Menu.SetBinding(HamburgerMenu.SelectedOptionsIndexProperty, binindex);

		Data.MainWin.SetHambMenu(this.hm_Menu);
    }



    void HamburgerMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e) {


        //HamburgerMenuGlyphItem hmfi = e.InvokedItem as HamburgerMenuGlyphItem;
        //int da = Data.HamburgerMenu.SelectedIndex;

    }
}
