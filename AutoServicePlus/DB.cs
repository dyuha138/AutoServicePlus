using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace AutoServicePlus;

class DB {
	public static MySqlConnection SQLConnection = null;
	private static string constr = null;
	public static event EventHandler<EventMsg> Ev_Status;


	public static void UpdateStringConnection(bool isTest) {
		if (isTest) {
			constr = $"Data Source={Data.Server_IP},{Data.Server_Port}";
		} else {
			constr = $"Data Source={Data.Server_IP};Port={Data.Server_Port};Database={Data.DB_Name};User ID={Data.Server_Login};Password={Data.Server_Pass};Persist Security Info=True";
		}
	}


	public static bool TestConnect() {
		UpdateStringConnection(true);
		try {
			SQLConnection = new MySqlConnection(constr);
			SQLConnection.Open();
		} catch (Exception ex) {
			if (ex.Message.Contains("пользователя")) {
				//SQLConnection.Close();
				return true;
			} else {
				return false;
			}
		}
		return true;
	}
	public static bool Open() {
		UpdateStringConnection(false);
		string str = null;
		try {
			//Ev_Status?.Invoke(null, new EventMsg("Подключение..."));
			SQLConnection = new MySqlConnection(constr);
			SQLConnection.Open();
		} catch (Exception ex) {
			if (ex.Message.Contains("Access denied for user")) {
				Ev_Status?.Invoke(null, new EventMsg("Неправильные логин или пароль"));
				return false;
			} else {
				Ev_Status?.Invoke(null, new EventMsg("Ошибка подключения к серверу"));
				return false;
			}
		}
		//Ev_Status?.Invoke(this, new EventMsg("Загрузка данных..."));
		//AllListsUpdate();
		DB_Заказы.UpdateListfromTable();

		Ev_Status?.Invoke(null, new EventMsg(""));
		return true;
	}


	public static bool Close() {
		try {
			SQLConnection.Close();
		} catch (Exception) {
			return false;
		}
		return true;
	}


	public static SQLResultTable SQLQuery(string SQL) {
		MySqlCommand DBQ = new MySqlCommand(SQL, SQLConnection);
		MySqlDataReader R = null;
		SQLResultTable ResTbl = null;
		try {
			R = DBQ.ExecuteReader();
		} catch (Exception ex) {
			if (ex.Message.Contains("Duplicate entry")) {
				Ev_Status?.Invoke(null, new("Запись с таким псевдонимом уже существует"));
				return null;
			}
		}

		if (R.HasRows) {
			ResTbl = new SQLResultTable(R);
			R.Close();
			return ResTbl;
		} else {
			R.Close();
			return null;
		}
	}

	public static int SQLQueryNon(string SQL) {
		MySqlCommand DBQ = new MySqlCommand(SQL, SQLConnection);
		return DBQ.ExecuteNonQuery();
	}




	public static class DB_Заказы {

		public static int Add(DBM_Заказ l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Заказы (, , , ) VALUES ('{l.Контрагент}', '{l.ДатаЗаказа}', {l.Статус}); SELECT LAST_INSERT_ID();");
			Data.ЗаказыList.Add(new DBM_Заказ(R.GetInt(0), l.Контрагент, l.ДатаЗаказа, l.Статус));
			return R.GetInt(0);
		}

		public static bool Remove(int id) {
			int R = DB.SQLQueryNon("DELETE FROM Заказы WHERE id = '" + id + "';");
			if (R > 0) {
				Data.ЗаказыList.RemoveAt(Data.ЗаказыList.FindIndex(x => x.id == id));
				return true;
			} else {
				return false;
			}
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM Заказы;");
			if (R != null) {
				Data.ЗаказыList.Clear();
				while (R.NextRow()) {
					Data.ЗаказыList.Add(new DBM_Заказ(R.GetInt(0), R.GetInt(1), R.GetLong(2), R.GetInt(3)));
				}
			}
		}

	}

}
