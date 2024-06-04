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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace AutoServicePlus;


public partial class Login : MetroWindow {
	public Login() {
		InitializeComponent();
		Data.LoadSettings();
		this.e_Login.Text = Data.DB.DataConnect.Server_Login;
		this.e_Pass.Password = Data.DB.DataConnect.Server_Pass;

		DB.Ev_Status += this.StatusSet;
	}


	private void StatusSet(object? sender, Twident_Msg e) {
		this.Dispatcher.Invoke(new Action(() => { this.l_Out.Content = e.Message; }));
	}

	private void b_Login_Click(object sender, RoutedEventArgs e) {
		this.pr_Ring.IsActive = true;
		this.l_Out.Visibility = Visibility.Collapsed;
		this.b_Login.IsEnabled = false;

		Data.DB.DataConnect.Server_Login = this.e_Login.Text;
		Data.DB.DataConnect.Server_Pass = this.e_Pass.Password;
		//Data.MainWin = new();
		//Data.MainWin.Show();

		Task.Factory.StartNew(() => {
			if (DB.Open()) {
				Data.SaveSettings();

				this.Dispatcher.Invoke(() => {
					Data.MainWin = new();
					Data.MainWin.Show();
					this.Close();
				});
			} else {
				this.Dispatcher.Invoke(() => {
					this.pr_Ring.IsActive = false;
					this.l_Out.Visibility = Visibility.Visible;
				});
			}
		});
	}
}
