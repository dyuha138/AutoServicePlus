using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoServicePlus;





public class Data {

	public static HamburgerMenuC HamburgerMenu = new();
	public static PropertiesChangeC PropertiesChange = new();
	//public PropertiesChangeC PropertiesChange2 = PropertiesChange;
	public static List<ComboBoxDBData> TypeList = new();
	public static List<ComboBoxDBData> StatusList = new();
	public static List<ComboBoxDBData> АвтоFullList = new();
	public static List<ComboBoxDBData> МаркиАвтоList = new();
	public static List<ComboBoxDBData> КатегорииList = new();
	public static List<ComboBoxDBData> КонтрагентыList = new();
	public static MainWindow MainWin = null;

	//public static PropertiesChangeC GetPropertiesChange() { return PropertiesChange; }


	public class PropertiesChangeC : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		private double _WinHeight;
		public double WinHeight {
			get { return _WinHeight; }
			set {
				_WinHeight = value;
				MainWin.MinHeight = value;
				//PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WinHeight)));
			}
		}

		protected void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
		}
	}


	public class HamburgerMenuC : INotifyPropertyChanged {

		public event EventHandler<Twident_Int> Ev_IndexChanged;
		public event EventHandler<Twident_Int> Ev_IndexOptionsChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public int SelectedIndex {
			get { return _selectedIndex; }
			set {
				if (_selectedIndex != value) {
					_selectedIndex = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedIndex"));
					Ev_IndexChanged?.Invoke(null, new(value));
				}
				//SetProperty(ref _selectedIndex, value);
				//Ev_IndexChanged?.Invoke(null, new(value));
			}
		}
		private int _selectedIndex = -1;

		public int SelectedOptionsIndex {
			get { return _selectedOptionsIndex; }
			set {
				if (_selectedOptionsIndex != value) {
					_selectedOptionsIndex = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedOptionsIndex"));
					Ev_IndexOptionsChanged?.Invoke(null, new(value));
				}
				//SetProperty(ref _selectedOptionsIndex, value);
				//Ev_IndexChanged?.Invoke(null, new(value));
			}
		}
		private int _selectedOptionsIndex = -1;

		//protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
		//	if (object.Equals(storage, value)) return false;

		//	storage = value;
		//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		//	return true;
		//}
	}



	public static class TBL {
		public static ObservableCollection<TBL_Заказ> TBLData_Заказы = new();
		public static ObservableCollection<TBL_Запчасть> TBLData_Запчасти = new();
		public static ObservableCollection<TBL_Запчасть> TBLData_Запчасти2 = new();
	}


	public static class DB {
		public static class DataConnect {
			public static string Server_IP = null;
			public static string Server_Port = null;
			public static string Server_Name = null;
			public static string DB_Name = null;
			public static string Server_Login = null;
			public static string Server_Pass = null;
		}

		public static ObservableCollection<TblData> tblDataList = new();

		public static List<DBM_Заказ> ЗаказыList = new();
		public static List<DBM_Запчасть> ЗапчастиList = new();
		public static List<DBM_РегистрЗапчасть> РегистрЗапчастейList = new();
		public static List<DBM_Статус> СтатусыList = new();

		public static DBM_Заказ TMP_Заказ = null;
	}







	public static void LoadDefSettings() {
		DB.DataConnect.Server_IP = "dyuhahome.ddns.net";
		DB.DataConnect.Server_Port = "1382";
		DB.DataConnect.Server_Name = "DHMYSQL";
		DB.DataConnect.DB_Name = "AutoServicePlus";
		DB.DataConnect.Server_Login = "";
		DB.DataConnect.Server_Pass = "";
	}


	public static void SaveSettings() {
		RegistryKey RegL = Registry.CurrentUser;
		RegistryKey Reg = RegL.OpenSubKey("SOFTWARE\\AutoServicePlus", true);

		if (Reg == null) {
			Reg = RegL.CreateSubKey("SOFTWARE\\AutoServicePlus");
		}
		Reg.SetValue("Server IP", DB.DataConnect.Server_IP, RegistryValueKind.String);
		Reg.SetValue("Server Port", DB.DataConnect.Server_Port, RegistryValueKind.String);
		Reg.SetValue("Server Name", DB.DataConnect.Server_Name, RegistryValueKind.String);
		Reg.SetValue("DB Name", DB.DataConnect.DB_Name, RegistryValueKind.String);
		Reg.SetValue("Login", DB.DataConnect.Server_Login, RegistryValueKind.String);
		Reg.SetValue("Pass", DB.DataConnect.Server_Pass, RegistryValueKind.String);

		Reg.Flush();
		Reg.Close();
		RegL.Close();
	}


	public static void LoadSettings() {
		RegistryKey RegL = Registry.CurrentUser;
		RegistryKey Reg = RegL.OpenSubKey("SOFTWARE\\AutoServicePlus");

		if (Reg != null) {
			DB.DataConnect.Server_IP = (string)Reg.GetValue("Server IP");
			DB.DataConnect.Server_Port = (string)Reg.GetValue("Server Port");
			DB.DataConnect.Server_Name = (string)Reg.GetValue("Server Name");
			DB.DataConnect.DB_Name = (string)Reg.GetValue("DB Name");
			DB.DataConnect.Server_Login = (string)Reg.GetValue("Login");
			DB.DataConnect.Server_Pass = (string)Reg.GetValue("Pass");
			Reg.Close();
			RegL.Close();
		} else {
			LoadDefSettings();
			SaveSettings();
		}
	}
}