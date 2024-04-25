using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServicePlus;

public class TblData : INotifyPropertyChanged {
	public string Название { get; set; }
	public string Псевдоним { get; set; }
	public string Тип { get; set; }
	public string Статус { get; set; }

	public TblData() { }
	public TblData(string Name, string Alias, string Type, string Status) {
		this.Название = Name;
		this.Псевдоним = Alias;
		this.Тип = Type;
		this.Статус = Status;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class DBM_Заказ {
	public class Запчасть {
		public int id { get; set; }
		public int Количество { get; set; }
		
		public Запчасть() { }
		public Запчасть(int id, int Количество) {
			this.id = id;
			this.Количество = Количество;
		}
	}

	public int id { get; set; }
	public long Дата { get; set; }
	public int Контрагент_id { get; set; }
	public int Статус_id { get; set; }
	public int Сотрудник_id { get; set; }
	public List<Запчасть> Запчасти { get; set; }

	public DBM_Заказ() { }
	public DBM_Заказ(int id, long Дата, int Контрагент_id, int Статус_id, int Сотрудник_id, List<Запчасть> Запчасти) {
		this.id = id;
		this.Дата = Дата;
		this.Контрагент_id = Контрагент_id;
		this.Статус_id = Статус_id;
		this.Сотрудник_id = Сотрудник_id;
		this.Запчасти = Запчасти;
	}
}

public class DBMtmp_ЗаказЗапчасть {
	public int Заказ_id { get; set; }
	public int Запчасть_id { get; set; }
	public int Количество { get; set; }

	public DBMtmp_ЗаказЗапчасть() { }
	public DBMtmp_ЗаказЗапчасть(int Заказ_id, int Запчасть_id, int Количество) {
		this.Заказ_id = Заказ_id;
		this.Запчасть_id = Запчасть_id;
		this.Количество = Количество;
	}
}


public class DBM_Запчасть {
	public int id { get; set; }
	public string Название { get; set; }
	public int Категория_id { get; set; }
	public int Автомобиль_id { get; set; }
	public int Статус_id { get; set; }

	public DBM_Запчасть() { }
	public DBM_Запчасть(int id, string Название, int Категория_id, int Автомобиль_id, int Статус_id) {
		this.id = id;
		this.Название = Название;
		this.Категория_id = Категория_id;
		this.Автомобиль_id = Автомобиль_id;
		this.Статус_id = Статус_id;
	}
}


public class DBM_Автомобиль {
	public int id { get; set; }
	public int Марка_id { get; set; }
	public string Модель { get; set; }

	public DBM_Автомобиль() { }
	public DBM_Автомобиль(int id, int Марка_id, string Модель) {
		this.id = id;
		this.Марка_id = Марка_id;
		this.Модель = Модель;
	}
}


public class DBM_Сотрудник {
	public int id { get; set; }
	public string Фамилия { get; set; }
	public string Имя { get; set; }
	public string Отчество { get; set; }

	public DBM_Сотрудник() { }
	public DBM_Сотрудник(int id, string Фамилия, string Имя, string Отчество) {
		this.id = id;
		this.Фамилия = Фамилия;
		this.Имя = Имя;
		this.Отчество = Отчество;
	}

	public string ПолучитьФИО() { return $"{this.Фамилия} {this.Имя} {this.Отчество}"; }
}

public class DBM_Контрагент {
	public int id { get; set; }
	public string Название { get; set; }
	public string Адрес { get; set; }
	public string Контакт { get; set; }
	public string Реквезиты { get; set; }

	public DBM_Контрагент() { }
	public DBM_Контрагент(int id, string Название, string Адрес, string Контакт, string Реквезиты) {
		this.id = id;
		this.Название = Название;
		this.Адрес = Адрес;
		this.Контакт = Контакт;
		this.Реквезиты = Реквезиты;
	}
}


public class DBM_РегистрЗапчасть {
	public int id { get; set; }
	public int Запчасть_id { get; set; }
	public int Количество { get; set; }
	public long Дата { get; set; }
	public int Операция_id { get; set; }
	public int Статус_id { get; set; }
	public int Сотрудник_id { get; set; }

	public DBM_РегистрЗапчасть() { }
	public DBM_РегистрЗапчасть(int id, int Запчасть_id, int Количество, long Дата, int Операция_id, int Статус_id, int Сотрудник_id) {
		this.id = id;
		this.Запчасть_id = Запчасть_id;
		this.Количество = Количество;
		this.Дата = Дата;
		this.Операция_id = Операция_id;
		this.Статус_id = Статус_id;
		this.Сотрудник_id = Сотрудник_id;
	}
}


	public class DBM_СвязьMtM {
	public int id { get; set; }
	public int id_1 { get; set; }
	public int id_2 { get; set; }

	public DBM_СвязьMtM() { }
	public DBM_СвязьMtM(int id, int id_1, int id_2) {
		this.id = id;
		this.id_1 = id_1;
		this.id_2 = id_2;
	}
}


public class DBM_СвязьMtM_int {
	public int id { get; set; }
	public int id_1 { get; set; }
	public int id_2 { get; set; }
	public int Data { get; set; }

	public DBM_СвязьMtM_int() { }
	public DBM_СвязьMtM_int(int id, int id_1, int id_2, int Data) {
		this.id = id;
		this.id_1 = id_1;
		this.id_2 = id_2;
		this.Data = Data;
	}
}



public class ComboBoxDBData {
	public int id { get; set; }
	public string Data { get; set; }

	public ComboBoxDBData() { }
	public ComboBoxDBData(int id, string Data) {
		this.id = id;
		this.Data = Data;
	}
}




public class SQLResultTable {

	public int RowRead = -1;
	public int Rows = 0;
	public int Colums = 0;
	private List<TRow> Table = new List<TRow>();

	public class TRow {
		public List<string> Row = new List<string>();
	}

	public SQLResultTable(MySqlDataReader R) {
		if (R != null && !R.IsClosed && R.HasRows) {
			Colums = R.FieldCount;
			string strl = null;
			while (R.Read()) {
				TRow Rowl = new TRow();
				for (int i = 0; i < Colums; i++) {
					strl = R.GetValue(i).ToString();
					if (strl == "" || strl == null) { strl = null; }
					Rowl.Row.Add(strl);
				}
				Table.Add(Rowl);
				Rows++;
			}
		}
	}

	public bool NextRow() { if (RowRead < Rows - 1) { RowRead++; return true; } else { return false; } }
	public bool PrevRow() { if (RowRead > 0) { RowRead--; return true; } else { return false; } }
	public void SetRow(int Row) { this.RowRead = Row; }
	public bool HasRows() { if (Rows > 0) { return true; } else { return false; } }

	public string GetStr(int Column) {
		return Table.ElementAt(this.RowRead).Row.ElementAt(Column);
	}
	public int GetInt(int Column) {
		try {
			return Convert.ToInt32(Table.ElementAt(this.RowRead).Row.ElementAt(Column));
		} catch (Exception ex) { return 0; }
	}
	public long GetLong(int Column) {
		try {
			return Convert.ToInt64(Table.ElementAt(this.RowRead).Row.ElementAt(Column));
		} catch (Exception) { return 0; }
	}
	public short GetShort(int Column) {
		try {
			return Convert.ToInt16(Table.ElementAt(this.RowRead).Row.ElementAt(Column));
		} catch (Exception) { return 0; }
	}
	public bool GetBool(int Column) {
		//try { return Convert.ToBoolean(Convert.ToInt32(Table.ElementAt(this.RowRead).Row.ElementAt(Column)));
		try {
			return Convert.ToBoolean(Table.ElementAt(this.RowRead).Row.ElementAt(Column));
		} catch (Exception) { return false; }
	}
}

