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

	public static List<DBM_Заказ> ЗаказыList = new();
	public static List<DBM_Запчасть> ЗапчастиList = new();


	public static HamburgerMenuC HamburgerMenu = new();


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

	


	public static string Server_IP = null;
	public static string Server_Port = "1382";
	public static string Server_Name = "DHMYSQL";
	public static string DB_Name = "Minecraft_Mods";
	public static string Server_Login = null;
	public static string Server_Pass = null;
	public static bool EnterSucc = false;

	public static ObservableCollection<TblData> tblDataList = new();
	public static List<ComboBoxDBData> TypeList = new();
	public static List<ComboBoxDBData> StatusList = new();
	public static MainWindow MainWin = null;



	public static void LoadDefSettings() {
		Server_IP = "dyuhahome.ddns.net";
		Server_Port = "1382";
		Server_Name = "DHMYSQL";
		DB_Name = "";
		Server_Login = "";
		Server_Pass = "";
	}


	public static void SaveSettings() {
		RegistryKey RegL = Registry.CurrentUser;
		RegistryKey Reg = RegL.OpenSubKey("SOFTWARE\\AutoServicePlus", true);

		if (Reg == null) {
			Reg = RegL.CreateSubKey("SOFTWARE\\AutoServicePlus");
		}
		Reg.SetValue("Server IP", Server_IP, RegistryValueKind.String);
		Reg.SetValue("Server Port", Server_Port, RegistryValueKind.String);
		Reg.SetValue("Server Name", Server_Name, RegistryValueKind.String);
		Reg.SetValue("Login", Server_Login, RegistryValueKind.String);
		Reg.SetValue("Pass", Server_Pass, RegistryValueKind.String);

		Reg.Flush();
		Reg.Close();
		RegL.Close();
	}


	public static void LoadSettings() {
		RegistryKey RegL = Registry.CurrentUser;
		RegistryKey Reg = RegL.OpenSubKey("SOFTWARE\\AutoServicePlus");

		if (Reg != null) {
			Server_IP = (string)Reg.GetValue("Server IP");
			Server_Port = (string)Reg.GetValue("Server Port");
			Server_Name = (string)Reg.GetValue("Server Name");
			Server_Login = (string)Reg.GetValue("Login");
			Server_Pass = (string)Reg.GetValue("Pass");
			Reg.Close();
			RegL.Close();
		} else {
			LoadDefSettings();
			SaveSettings();
		}
	}
}
