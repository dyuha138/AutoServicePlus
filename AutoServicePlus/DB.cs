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
	public static event EventHandler<Twident_Msg> Ev_Status;


	public static void UpdateStringConnection(bool isTest) {
		if (isTest) {
			constr = $"Data Source={Data.DB.DataConnect.Server_IP},{Data.DB.DataConnect.Server_Port}";
		} else {
			constr = $"Data Source={Data.DB.DataConnect.Server_IP};Port={Data.DB.DataConnect.Server_Port};Database={Data.DB.DataConnect.DB_Name};User ID={Data.DB.DataConnect.Server_Login};Password={Data.DB.DataConnect.Server_Pass};Persist Security Info=True";
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
				Ev_Status?.Invoke(null, new Twident_Msg("Ошибка подключения к серверу"));
				return false;
			}
		}
		return true;
	}
	public static bool Open() {
		UpdateStringConnection(false);
		//string str = null;
		try {
			//Ev_Status?.Invoke(null, new Twident_Msg("Подключение..."));
			SQLConnection = new MySqlConnection(constr);
			SQLConnection.Open();
		} catch (Exception ex) {
			if (ex.Message.Contains("Access denied for user")) {
				Ev_Status?.Invoke(null, new Twident_Msg("Неправильные логин или пароль"));
				return false;
			} else {
				Ev_Status?.Invoke(null, new Twident_Msg("Ошибка подключения к серверу"));
				return false;
			}
		}
		//Ev_Status?.Invoke(this, new Twident_Msg("Загрузка данных..."));
		//DB_Заказы.UpdateListfromTable();
		//DB_Заявки.UpdateListfromTable();
		DB_Статусы.UpdateListfromTable();
		DB_Сотрудники.UpdateListfromTable();

		DB_Справочники.UpdateListfromTable(Data.КатегорииList, "КатегорииЗап");
		DB_Справочники.UpdateListfromTable(Data.МаркиАвтоList, "МаркиАвто");
		DB_Справочники.UpdateListfromTable(Data.КонтрагентыList, "Контрагенты");

		Ev_Status?.Invoke(null, new Twident_Msg(""));
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
			}
			return null;
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
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Заказы (Дата, Статус_id, Сотрудник_id) VALUES ({l.Дата}, {l.Статус_id}, {l.Сотрудник_id}); SELECT LAST_INSERT_ID();");
			R.NextRow();
			Data.DB.ЗаказыList.Add(new(R.GetInt(0), l.Дата, l.Статус_id, l.Сотрудник_id, l.Запчасти));

			int idl = R.GetInt(0);

			StringBuilder sb = new();
			for(int i = 0; i < l.Запчасти.Count; i++) {
				sb.AppendLine($"INSERT INTO ЗаказЗапчасть (Заказ_id, Запчасть_id, Количество, Контрагент_id) VALUES ({idl}, {l.Запчасти[i].id}, {l.Запчасти[i].Количество}, {l.Запчасти[i].Контрагент_id});");
			}
			R = DB.SQLQuery(sb.ToString());
			
			return idl;
		}

		public static bool RowUpdate(DBM_Заказ l) {
			int i = Data.DB.ЗаказыList.FindIndex(x => x.id == l.id);
			DBM_Заказ ll = Data.DB.ЗаказыList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE Заказы SET Дата = {l.Дата}, Статус_id = {l.Дата}, Сотрудник_id = {l.Сотрудник_id} WHERE id = {l.id};");

				Data.DB.ЗаказыList.RemoveAt(i);
				Data.DB.ЗаказыList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static bool StatusUpdate(int id, int status) {
			int i = Data.DB.ЗаказыList.FindIndex(x => x.id == id);
			DBM_Заказ ll = Data.DB.ЗаказыList.ElementAt(i);
			if (ll != null) {
				//DB.DB_РегистрЗапчастей.AddOrder(id);

				DB.SQLQuery($"UPDATE Заказы SET Статус_id = {status} WHERE id = {id};");

				Data.DB.ЗаказыList.RemoveAt(i);
				ll.Статус_id = status;
				Data.DB.ЗаказыList.Insert(i, ll);
				return true;
			} else {
				return false;
			}
		}

		public static int GetLastAgent(int Запчасть_id) {
			SQLResultTable R = DB.SQLQuery($"SELECT ЗакЗап.Контрагент_id FROM AutoServicePlus.ЗаказЗапчасть ЗакЗап \r\nINNER JOIN AutoServicePlus.ЗапчастиМодели ЗакМ ON ЗакЗап.Запчасть_id = ЗакМ.id\r\nWHERE ЗакМ.id = {Запчасть_id}\r\nORDER BY ЗакЗап.id DESC\r\nLIMIT 1;");
			if (R != null) {
				R.NextRow();
				return R.GetInt(0);
			}
			return 0;
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = null;
			SQLResultTable R2 = null;

			R = DB.SQLQuery("SELECT * FROM Заказы;");
			if (R != null) {
				Data.DB.ЗаказыList.Clear();
				while (R.NextRow()) {
					Data.DB.ЗаказыList.Add(new(R.GetInt(0), R.GetLong(1), R.GetInt(2), R.GetInt(3), new()));

					R2 = DB.SQLQuery($"SELECT Запчасть_id, Количество, Контрагент_id FROM ЗаказЗапчасть WHERE Заказ_id = {R.GetInt(0)};");
					while (R2.NextRow()) {
						Data.DB.ЗаказыList[R.RowRead].Запчасти.Add(new(R2.GetInt(0), R2.GetInt(1), R2.GetInt(2)));
					}
				}
			}
		}
	}


	public static class DB_Заявки {

		public static int Add(DBM_Заявка l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Заявки (Дата, Статус_id, Сотрудник_id) VALUES ({l.Дата}, {l.Статус_id}, {l.Сотрудник_id}); SELECT LAST_INSERT_ID();");
			R.NextRow();
			//Data.DB.ЗаявкиList.Add(new(R.GetInt(0), l.Дата, l.Статус_id, l.Сотрудник_id, l.Запчасти));

			int idl = R.GetInt(0);

			StringBuilder sb = new();
			for (int i = 0; i < l.Запчасти.Count; i++) {
				sb.AppendLine($"INSERT INTO ЗаявкаЗапчасть (Заявка_id, Запчасть_id) VALUES ({idl}, {l.Запчасти[i].id});");
			}
			R = DB.SQLQuery(sb.ToString());

			return idl;
		}

		public static bool RowUpdate(DBM_Заявка l) {
			int i = Data.DB.ЗаявкиList.FindIndex(x => x.id == l.id);
			DBM_Заявка ll = Data.DB.ЗаявкиList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE Заявки SET Дата = {l.Дата}, Статус_id = {l.Дата}, Сотрудник_id = {l.Сотрудник_id} WHERE id = {l.id};");

				Data.DB.ЗаявкиList.RemoveAt(i);
				Data.DB.ЗаявкиList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static bool StatusUpdate(int id, int status) {
			int i = Data.DB.ЗаявкиList.FindIndex(x => x.id == id);
			DBM_Заявка ll = Data.DB.ЗаявкиList.ElementAt(i);
			if (ll != null) {
				//DB.DB_РегистрЗапчастей.AddOrder(id);

				DB.SQLQuery($"UPDATE Заявки SET Статус_id = {status} WHERE id = {id};");

				Data.DB.ЗаявкиList.RemoveAt(i);
				ll.Статус_id = status;
				Data.DB.ЗаявкиList.Insert(i, ll);
				return true;
			} else {
				return false;
			}
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = null;
			SQLResultTable R2 = null;

			R = DB.SQLQuery("SELECT * FROM Заявки;");
			if (R != null) {
				Data.DB.ЗаявкиList.Clear();
				while (R.NextRow()) {
					Data.DB.ЗаявкиList.Add(new(R.GetInt(0), R.GetLong(1), R.GetInt(2), R.GetInt(3), new()));

					R2 = DB.SQLQuery($"SELECT Запчасть_id FROM ЗаявкаЗапчасть WHERE Заявка_id = {R.GetInt(0)};");
					while (R2.NextRow()) {
						Data.DB.ЗаявкиList[R.RowRead].Запчасти.Add(new(R2.GetInt(0)));
					}
				}
			}
		}
	}


	public static class DB_ЗапчастиМодели {

		public static int Add(DBM_ЗапчастьМодель l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO ЗапчастиМодели (Название, Категория_id) VALUES ('{l.Название}', {l.Категория_id}); SELECT LAST_INSERT_ID();");
			Data.DB.ЗапчастиМоделиList.Add(new(R.GetInt(0), l.Название, l.Категория_id, l.Автомобиль_id, l.Статус_id));
			R.NextRow();
			int idl = R.GetInt(0);
			R = DB.SQLQuery($"INSERT INTO АвтомобильЗапчасть (Автомобиль_id, Запчасть_id) VALUES ({l.Автомобиль_id}, {idl});");
			return idl;
		}

		public static bool Remove(int id) {
			int R = DB.SQLQueryNon($"DELETE FROM ЗапчастиМодели WHERE id = {id};");
			if (R > 0) {
				DB.SQLQueryNon($"DELETE FROM АвтомобильЗапчасть WHERE Запчасть_id = {id};");
				Data.DB.ЗапчастиМоделиList.RemoveAt(Data.DB.ЗапчастиList.FindIndex(x => x.id == id));
				return true;
			} else {
				return false;
			}
		}

		public static bool RowUpdate(DBM_ЗапчастьМодель l) {
			int i = Data.DB.ЗапчастиМоделиList.FindIndex(x => x.id == l.id);
			DBM_ЗапчастьМодель ll = Data.DB.ЗапчастиМоделиList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE ЗапчастиМодели SET Название = '{l.Название}', Категория_id = {l.Категория_id} WHERE id = {l.id};");
				DB.SQLQuery($"UPDATE АвтомобильЗапчасть SET Автомобиль_id = {l.Автомобиль_id} WHERE Запчасть_id = {l.id};");

				Data.DB.ЗапчастиМоделиList.RemoveAt(i);
				Data.DB.ЗапчастиМоделиList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static DBM_ЗапчастьМодель Find(int id) {
			SQLResultTable R = DB.SQLQuery($"SELECT * FROM ЗапчастиМодели WHERE id = {id};");
			if (R != null) {
				R.NextRow();
				return new(R.GetInt(0), R.GetStr(1), R.GetInt(2), R.GetInt(3), R.GetInt(4));
			}
			return null;
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM ЗапчастиМодели;");
			if (R != null) {
				Data.DB.ЗапчастиМоделиList.Clear();
				while (R.NextRow()) {
					Data.DB.ЗапчастиМоделиList.Add(new(R.GetInt(0), R.GetStr(1), R.GetInt(2), R.GetInt(3), R.GetInt(4)));
				}
			}
		}
	}


	public static class DB_Запчасти {

		public static int Add(DBM_Запчасть l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Запчасти (Модель_id, Идентификатор) VALUES ({l.Модель_id}, '{l.Идентификатор}'); SELECT LAST_INSERT_ID();");
			//Data.DB.ЗапчастиList.Add(new(R.GetInt(0), l.Модель_id, l.Идентификатор));
			R.NextRow();
			return R.GetInt(0);
		}

		public static bool Remove(int id) {
			int R = DB.SQLQueryNon($"DELETE FROM Запчасти WHERE id = {id};");
			if (R > 0) {
				Data.DB.ЗапчастиList.RemoveAt(Data.DB.ЗапчастиList.FindIndex(x => x.id == id));
				return true;
			} else {
				return false;
			}
		}

		public static bool RowUpdate(DBM_Запчасть l) {
			int i = Data.DB.ЗапчастиList.FindIndex(x => x.id == l.id);
			DBM_Запчасть ll = Data.DB.ЗапчастиList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE Запчасти SET Модель_id = {l.Модель_id}, Идентификатор = '{l.Идентификатор}' WHERE id = {l.id};");

				Data.DB.ЗапчастиList.RemoveAt(i);
				Data.DB.ЗапчастиList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static DBM_Запчасть Find(int id) {
			SQLResultTable R = DB.SQLQuery($"SELECT * FROM Запчасти WHERE id = {id};");
			if (R != null) {
				R.NextRow();
				return new(R.GetInt(0), R.GetInt(1), R.GetStr(2));
			}
			return null;
		}

		public static int isExists(string Идентификатор) {
			SQLResultTable R = DB.SQLQuery($"SELECT id FROM Запчасти WHERE Идентификатор = '{Идентификатор}';");
			if (R != null) {
				R.NextRow();
				return R.GetInt(0);
			}
			return 0;
		}

		public static List<DBM_Запчасть> GetListfromModelid(int Модель_id) {
			List<DBM_Запчасть> l = new();
			SQLResultTable R = DB.SQLQuery($"SELECT * FROM Запчасти WHERE Модель_id = {Модель_id};");
			if (R != null) {
				while (R.NextRow()) {
					l.Add(new(R.GetInt(0), R.GetInt(1), R.GetStr(2)));
				}
				return l;
			}
			return null;
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM Запчасти;");
			if (R != null) {
				Data.DB.ЗапчастиList.Clear();
				while (R.NextRow()) {
					Data.DB.ЗапчастиList.Add(new(R.GetInt(0), R.GetInt(1), R.GetStr(2)));
				}
			}
		}
	}


	public static class DB_РегистрЗапчастей {

		public static int Add(DBM_РегистрЗапчасть l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO РегистрЗапчастей (Запчасть_id, Дата, Статус_id, Сотрудник_id) VALUES ({l.Запчасть_id}, {l.Дата}, {l.Статус_id}, {l.Сотрудник_id}); SELECT LAST_INSERT_ID();");
			Data.DB.РегистрЗапчастейList.Add(new DBM_РегистрЗапчасть(R.GetInt(0), l.Запчасть_id, l.Дата, l.Статус_id, l.Сотрудник_id));
			R.NextRow();
			return R.GetInt(0);
		}

		public static bool AddOrder(int Заказ_id) {
			DBM_Заказ Заказ = Data.DB.ЗаказыList.Find(x => x.id == Заказ_id);
			List<DBM_Заказ.ЗапчастьМодель> Запчасти = Заказ.Запчасти;
			long udate = TechFuncs.GetUnixTime();
			int logid = TechFuncs.ПолучитьАйдиВхода();

			for (int i = 0; i < Запчасти.Count; i++) {
				Add(new(0, Запчасти[i].id, udate, Заказ.Статус_id, logid));
			}

			return true;
		}



		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM РегистрЗапчастей;");
			if (R != null) {
				Data.DB.РегистрЗапчастейList.Clear();
				while (R.NextRow()) {
					Data.DB.РегистрЗапчастейList.Add(new DBM_РегистрЗапчасть(R.GetInt(0), R.GetInt(1), R.GetLong(2), R.GetInt(3), R.GetInt(4)));
				}
			}
		}
	}


	public static class DB_Справочники {
		public static void UpdateListfromTable(List<ComboBoxDBData> list, string Table) {
			SQLResultTable R = DB.SQLQuery($"SELECT * FROM {Table};");
			if (R != null) {
				list.Clear();
				while (R.NextRow()) {
					list.Add(new(R.GetInt(0), R.GetStr(1)));
				}
			}
		}
	}


	public static class DB_Статусы {
		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM Статусы;");
			if (R != null) {
				Data.DB.СтатусыList.Clear();
				while (R.NextRow()) {
					Data.DB.СтатусыList.Add(new(R.GetInt(0), R.GetStr(1)));
				}
			}
		}
	}


	public static class DB_Сотрудники {


		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM Сотрудники;");
			if (R != null) {
				Data.DB.СотрудникиList.Clear();
				while (R.NextRow()) {
					Data.DB.СотрудникиList.Add(new(R.GetInt(0), R.GetStr(1), R.GetStr(2), R.GetStr(3), R.GetStr(4)));
				}
			}
		}
	}
}
