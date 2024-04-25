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
				Ev_Status?.Invoke(null, new Twident_Msg("Ошибка подключения к серверу"));
				return false;
			}
		}
		return true;
	}
	public static bool Open() {
		UpdateStringConnection(false);
		string str = null;
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
		//AllListsUpdate();
		//DB_Заказы.UpdateListfromTable();

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
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Заказы (Дата, Контрагент_id, Статус_id, Сотрудник_id) VALUES ({l.Дата}, {l.Контрагент_id}, {l.Статус_id}, {l.Сотрудник_id}); SELECT LAST_INSERT_ID();");
			Data.ЗаказыList.Add(new DBM_Заказ(R.GetInt(0), l.Дата, l.Контрагент_id, l.Статус_id, l.Сотрудник_id, l.Запчасти));

			int idl = R.GetInt(0);

			StringBuilder sb = new();
			for(int i = 0; i < l.Запчасти.Count; i++) {
				sb.AppendLine($"INSERT INTO ЗаказЗапчасть (Заказ_id, Запчасть_id, Количество) VALUES ({idl}, {l.Запчасти.ElementAt(i).id}, {l.Запчасти.ElementAt(i).Количество});");
			}
			R = DB.SQLQuery(sb.ToString());
			
			return idl;
		}

		public static bool Remove(int id) {
			int R = DB.SQLQueryNon($"DELETE FROM Заказы WHERE id = {id};");
			if (R > 0) {
				Data.ЗаказыList.RemoveAt(Data.ЗаказыList.FindIndex(x => x.id == id));
				return true;
			} else {
				return false;
			}
		}

		public static bool RowUpdate(DBM_Заказ l) {
			int i = Data.ЗаказыList.FindIndex(x => x.id == l.id);
			DBM_Заказ ll = Data.ЗаказыList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE Заказы SET Дата = {l.Дата}, Контрагент_id = {l.Контрагент_id}, Статус_id = {l.Дата}, Сотрудник_id = {l.Сотрудник_id} WHERE id = {l.id};");

				Data.ЗаказыList.RemoveAt(i);
				Data.ЗаказыList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM ЗаказЗапчасть;");
			List<DBMtmp_ЗаказЗапчасть> ll = new();
			List<DBMtmp_ЗаказЗапчасть> llt = new();
			List<DBM_Заказ.Запчасть> ll2 = new();

			if (R != null) {
				while (R.NextRow()) {
					ll.Add(new DBMtmp_ЗаказЗапчасть(R.GetInt(1), R.GetInt(2), R.GetInt(3)));
				}
			}

			R = DB.SQLQuery("SELECT * FROM Заказы;");
			if (R != null) {
				Data.ЗаказыList.Clear();
				while (R.NextRow()) {
					ll2.Clear();
					llt = ll.FindAll(x => x.Заказ_id == R.GetInt(0));

					for (int i = 0; i < llt.Count; i++) {
						ll2.Add(new DBM_Заказ.Запчасть(llt.ElementAt(i).Запчасть_id, llt.ElementAt(i).Количество));
					}
					ll.RemoveAll(x => x.Заказ_id == R.GetInt(0));

					Data.ЗаказыList.Add(new DBM_Заказ(R.GetInt(0), R.GetLong(1), R.GetInt(2), R.GetInt(3), R.GetInt(4), ll2));
				}
			}
		}
	}


	public static class DB_Запчасти {

		public static int Add(DBM_Запчасть l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO Запчасти (Название, Категория_id) VALUES ({l.Название}, {l.Категория_id}); SELECT LAST_INSERT_ID();");
			Data.ЗапчастиList.Add(new DBM_Запчасть(R.GetInt(0), l.Название, l.Категория_id, l.Автомобиль_id, l.Статус_id));
			int idl = R.GetInt(0);
			R = DB.SQLQuery($"INSERT INTO АвтомобильЗапчасть (Автомобиль_id, Запчасть_id) VALUES ({l.Автомобиль_id}, {idl});");
			return idl;
		}

		public static bool Remove(int id) {
			int R = DB.SQLQueryNon($"DELETE FROM Запчасти WHERE id = {id};");
			if (R > 0) {
				DB.SQLQueryNon($"DELETE FROM АвтомобильЗапчасть WHERE Запчасть_id = {id};");
				Data.ЗапчастиList.RemoveAt(Data.ЗапчастиList.FindIndex(x => x.id == id));
				return true;
			} else {
				return false;
			}
		}

		public static bool RowUpdate(DBM_Запчасть l) {
			int i = Data.ЗапчастиList.FindIndex(x => x.id == l.id);
			DBM_Запчасть ll = Data.ЗапчастиList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE Запчасти SET Название = {l.Название}, Категория = {l.Категория_id} WHERE id = {l.id};");
				DB.SQLQuery($"UPDATE АвтомобильЗапчасть SET Автомобиль_id = {l.Автомобиль_id} WHERE Запчасть_id = {l.id};");

				Data.ЗапчастиList.RemoveAt(i);
				Data.ЗапчастиList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM Запчасти;");
			if (R != null) {
				Data.ЗапчастиList.Clear();
				while (R.NextRow()) {
					Data.ЗапчастиList.Add(new DBM_Запчасть(R.GetInt(0), R.GetStr(1), R.GetInt(2), R.GetInt(3), R.GetInt(4)));
				}
			}
		}
	}


	public static class DB_РегистрЗапчастей {

		public static int Add(DBM_РегистрЗапчасть l) {
			SQLResultTable R = DB.SQLQuery($"INSERT INTO РегистрЗапчастей (Запчасть_id, Количество, Дата, Операция_id, Статус_id, Сотрудник_id) VALUES ({l.Запчасть_id}, {l.Количество}, {l.Дата}, {l.Операция_id}, {l.Статус_id}, {l.Сотрудник_id}); SELECT LAST_INSERT_ID();");
			Data.РегистрЗапчастейList.Add(new DBM_РегистрЗапчасть(R.GetInt(0), l.Запчасть_id, l.Количество, l.Дата, l.Операция_id, l.Статус_id, l.Сотрудник_id));
			return R.GetInt(0);
		}

		//public static bool Remove(int id) {
		//	int R = DB.SQLQueryNon($"DELETE FROM РегистрЗапчастей WHERE id = {id};");
		//	if (R > 0) {
		//		Data.ЗаказыList.RemoveAt(Data.ЗаказыList.FindIndex(x => x.id == id));
		//		return true;
		//	} else {
		//		return false;
		//	}
		//}

		public static bool RowUpdate(DBM_РегистрЗапчасть l) {
			int i = Data.РегистрЗапчастейList.FindIndex(x => x.id == l.id);
			DBM_РегистрЗапчасть ll = Data.РегистрЗапчастейList.ElementAt(i);
			if (ll != null) {
				DB.SQLQuery($"UPDATE РегистрЗапчастей SET Запчасть_id = {l.Запчасть_id}, Количество = {l.Количество}, Дата = {l.Дата}, Операция_id = {l.Операция_id}, Статус_id = {l.Статус_id}, Сотрудник_id = {l.Сотрудник_id} WHERE id = {l.id};");

				Data.РегистрЗапчастейList.RemoveAt(i);
				Data.РегистрЗапчастейList.Insert(i, l);
				return true;
			} else {
				return false;
			}
		}

		public static void UpdateListfromTable() {
			SQLResultTable R = DB.SQLQuery("SELECT * FROM РегистрЗапчастей;");
			if (R != null) {
				Data.РегистрЗапчастейList.Clear();
				while (R.NextRow()) {
					Data.РегистрЗапчастейList.Add(new DBM_РегистрЗапчасть(R.GetInt(0), R.GetInt(1), R.GetInt(2), R.GetLong(3), R.GetInt(4), R.GetInt(5), R.GetInt(6)));
				}
			}
		}
	}

}
