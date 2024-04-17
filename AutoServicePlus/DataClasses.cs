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
	public int id { get; set; }
	public int Контрагент { get; set; }
	public long ДатаЗаказа { get; set; }
	public int Статус { get; set; }

	public DBM_Заказ() { }
	public DBM_Заказ(int id, int Контрагент, long ДатаЗаказа, int Статус) {
		this.id = id;
		this.Контрагент = Контрагент;
		this.ДатаЗаказа = ДатаЗаказа;
		this.Статус = Статус;
	}
}


public class DBM_Запчасть {
	public int id { get; set; }
	public string Название { get; set; }
	public int Категория { get; set; }
	public int Автомобиль { get; set; }
	public int Статус { get; set; }

	public DBM_Запчасть() { }
	public DBM_Запчасть(int id, string Название, int Категория, int Автомобиль, int Статус) {
		this.id = id;
		this.Название = Название;
		this.Категория = Категория;
		this.Автомобиль = Автомобиль;
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



public class EventMsg : EventArgs {
	public readonly string Message;
	public EventMsg(string msg) { Message = msg; }
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

