using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServicePlus;

//public class TblData : INotifyPropertyChanged {
//	public string Название { get; set; }
//	public string Псевдоним { get; set; }
//	public string Тип { get; set; }
//	public string Статус { get; set; }

//	public TblData() { }
//	public TblData(string Name, string Alias, string Type, string Status) {
//		this.Название = Name;
//		this.Псевдоним = Alias;
//		this.Тип = Type;
//		this.Статус = Status;
//	}

//	public event PropertyChangedEventHandler PropertyChanged;
//	protected virtual void OnPropertyChanged(string propertyName) {
//		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//	}
//}


public class TBL_Заказ : INotifyPropertyChanged {
	public int id { get; set; }
	public string Дата { get; set; }
	public string Статус { get; set; }
	public string Сотрудник { get; set; }

	public TBL_Заказ() { }
	public TBL_Заказ(int id, string Дата, string Статус, string Сотр_Фам, string Сотр_Имя, string Сотр_Отч) {
		this.id = id;
		this.Дата = Дата;
		this.Статус = Статус;
		this.Сотрудник = $"{Сотр_Фам} {Сотр_Имя[0]}. {Сотр_Отч[0]}.";
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_ЗапчастьМодель : INotifyPropertyChanged {
	public int id { get; set; }
	public string Название { get; set; }
	public string Категория { get; set; }
	public string Марка_Авто { get; set; }
	public string Модель_Авто { get; set; }

	public TBL_ЗапчастьМодель() { }
	public TBL_ЗапчастьМодель(int id, string Название, string Категория, string Марка_Авто, string Модель_Авто) {
		this.id = id;
		this.Название = Название;
		this.Категория = Категория;
		this.Марка_Авто = Марка_Авто;
		this.Модель_Авто = Модель_Авто;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_ЗапчастьМодель2 : INotifyPropertyChanged {
	public int id { get; set; }
	public string Название { get; set; }
	public string Категория { get; set; }
	public int Количество { get; set; }
	public string Марка_Авто { get; set; }
	public string Модель_Авто { get; set; }
	public string Контрагент { get; set; }

	public TBL_ЗапчастьМодель2() { }
	public TBL_ЗапчастьМодель2(int id, string Название, string Категория, int Количество, string Марка_Авто, string Модель_Авто, string Контрагент) {
		this.id = id;
		this.Название = Название;
		this.Категория = Категория;
		this.Количество = Количество;
		this.Марка_Авто = Марка_Авто;
		this.Модель_Авто = Модель_Авто;
		this.Контрагент = Контрагент;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_Запчасть : INotifyPropertyChanged {
	public int id { get; set; }
	public string Название { get; set; }
	public string Категория { get; set; }
	public string Идентификатор { get; set; }

	public TBL_Запчасть() { }
	public TBL_Запчасть(int id, string Название, string Категория, string Идентификатор) {
		this.id = id;
		this.Название = Название;
		this.Категория = Категория;
		this.Идентификатор = Идентификатор;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_ЗапчастьМини : INotifyPropertyChanged {
	public int id { get; set; }
	public string Идентификатор { get; set; }

	public TBL_ЗапчастьМини() { }
	public TBL_ЗапчастьМини(int id, string Идентификатор) {
		this.id = id;
		this.Идентификатор = Идентификатор;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class TBL_ЗапчастьМиниПлюс : INotifyPropertyChanged {
	public int id { get; set; }
	public string Идентификатор { get; set; }
	public string Статус { get; set; }

	public TBL_ЗапчастьМиниПлюс() { }
	public TBL_ЗапчастьМиниПлюс(int id, string Идентификатор, string Статус) {
		this.id = id;
		this.Идентификатор = Идентификатор;
		this.Статус = Статус;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_Склад : INotifyPropertyChanged {
	public int id { get; set; }
	public string Название { get; set; }
	public string Категория { get; set; }
	public int Количество { get; set; }
	public string Марка_Авто { get; set; }
	public string Модель_Авто { get; set; }

	public TBL_Склад() { }
	public TBL_Склад(int id, string Название, string Категория, int Количество, string Марка_Авто, string Модель_Авто) {
		this.id = id;
		this.Название = Название;
		this.Категория = Категория;
		this.Количество = Количество;
		this.Марка_Авто = Марка_Авто;
		this.Модель_Авто = Модель_Авто;
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}


public class TBL_Заявка : INotifyPropertyChanged {
	public int id { get; set; }
	public string Дата { get; set; }
	public string Статус { get; set; }
	public string Сотрудник { get; set; }

	public TBL_Заявка() { }
	public TBL_Заявка(int id, string Дата, string Статус, string Сотр_Фам, string Сотр_Имя, string Сотр_Отч) {
		this.id = id;
		this.Дата = Дата;
		this.Статус = Статус;
		this.Сотрудник = $"{Сотр_Фам} {Сотр_Имя[0]}. {Сотр_Отч[0]}.";
	}

	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}



public class DBM_Заказ {
	public class ЗапчастьМодель {
		public int id { get; set; }
		public int Количество { get; set; }
		public int Контрагент_id { get; set; }

		public ЗапчастьМодель() { }
		public ЗапчастьМодель(int id, int Количество, int Контрагент_id) {
			this.id = id;
			this.Количество = Количество;
			this.Контрагент_id = Контрагент_id;
		}
	}

	public int id { get; set; }
	public long Дата { get; set; }
	public int Статус_id { get; set; }
	public int Сотрудник_id { get; set; }
	public List<ЗапчастьМодель> Запчасти { get; set; }

	public DBM_Заказ() { }
	public DBM_Заказ(int id, long Дата, int Статус_id, int Сотрудник_id, List<ЗапчастьМодель> Запчасти) {
		this.id = id;
		this.Дата = Дата;
		this.Статус_id = Статус_id;
		this.Сотрудник_id = Сотрудник_id;
		this.Запчасти = Запчасти;
	}
}


public class DBM_Заявка {
	public class Запчасть {
		public int id { get; set; }
		public int Модель_id { get; set; }

		public Запчасть() { }
		public Запчасть(int id, int Модель_id) {
			this.id = id;
			this.Модель_id = Модель_id;
		}
	}

	public int id { get; set; }
	public long Дата { get; set; }
	public int Статус_id { get; set; }
	public int Сотрудник_id { get; set; }
	public List<Запчасть> Запчасти { get; set; }

	public DBM_Заявка() { }
	public DBM_Заявка(int id, long Дата, int Статус_id, int Сотрудник_id, List<Запчасть> Запчасти) {
		this.id = id;
		this.Дата = Дата;
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


public class DBM_ЗапчастьМодель {
	public int id { get; set; }
	public string Название { get; set; }
	public int Категория_id { get; set; }
	public int Автомобиль_id { get; set; }
	public int Статус_id { get; set; }

	public DBM_ЗапчастьМодель() { }
	public DBM_ЗапчастьМодель(int id, string Название, int Категория_id, int Автомобиль_id, int Статус_id) {
		this.id = id;
		this.Название = Название;
		this.Категория_id = Категория_id;
		this.Автомобиль_id = Автомобиль_id;
		this.Статус_id = Статус_id;
	}
}



public class DBM_Запчасть {
	public int id { get; set; }
	public int Модель_id { get; set; }
	public string Идентификатор { get; set; }

	public DBM_Запчасть() { }
	public DBM_Запчасть(int id, int Модель_id, string Идентификатор) {
		this.id = id;
		this.Модель_id = Модель_id;
		this.Идентификатор = Идентификатор;
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
	public string Логин {  get; set; }

	public DBM_Сотрудник() { }
	public DBM_Сотрудник(int id, string Фамилия, string Имя, string Отчество, string Логин) {
		this.id = id;
		this.Фамилия = Фамилия;
		this.Имя = Имя;
		this.Отчество = Отчество;
		this.Логин = Логин;
	}

	public string ПолучитьФИО() { return $"{this.Фамилия} {this.Имя} {this.Отчество}"; }
	public string ПолучитьИнициалы() { return $"{this.Фамилия} {this.Имя[0]}. {this.Отчество[0]}."; }
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
	public long Дата { get; set; }
	public int Статус_id { get; set; }
	public int Сотрудник_id { get; set; }

	public DBM_РегистрЗапчасть() { }
	public DBM_РегистрЗапчасть(int id, int Запчасть_id, long Дата, int Статус_id, int Сотрудник_id) {
		this.id = id;
		this.Запчасть_id = Запчасть_id;
		this.Дата = Дата;
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


public class DBM_Статус {
	public int id { get; set; }
	public string Статус { get; set; }
	public string Тип { get; set; }

	public DBM_Статус() { }
	public DBM_Статус(int id, string Статус) {
		this.id = id;
		this.Статус = Статус;
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
		} catch (Exception) { return 0; }
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

