using Org.BouncyCastle.Utilities.Encoders;

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

	public PageNewRequest() {
		InitializeComponent();
		Data.PropertiesChange.WinHeight = 423;

	}

	private void b_Edit_Click(object sender, RoutedEventArgs e) {
		Task.Factory.StartNew(() => {
			int hetbl = 0;
			this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Запрос.ActualHeight); });

			if (isReqEdit) {
				isReqEdit = false;
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
				isReqEdit = true;
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



	private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e) {
		//da.Content = dg_Запрос.ActualHeight;
		//da.Content = this.ActualWidth.ToString();
		//da2.Content = this.ActualHeight.ToString();
	}

	private void VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e) {
		if ((bool)e.NewValue) {
			//da.Content = "yes";
		} else {
			//da.Content = "no";
		}
	}

	private void b_Exit_Click(object sender, RoutedEventArgs e) {
		Data.MainWin.Dispatcher.Invoke(() => {
			Data.MainWin.HambMenu.Content = Data.MainWin.PageRequests;
		});
	}
}

