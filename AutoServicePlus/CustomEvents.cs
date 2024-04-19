using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoServicePlus
{
	// -- Класс эвента, который просто перенаправляет сообщение --
	public class Twident_Msg : EventArgs {
		public readonly string Message;
		public Twident_Msg(string msg) { Message = msg; }
	}



	// -- Класс эвента, который ничего не передаёт, чтобы просто что-то вызвать --
	public class Twident_Null : EventArgs {
		public Twident_Null() { }
	}



	// -- Класс эвента статуса работы --
	/// <summary>
	/// Эвент статуса работы
	/// </summary>
	/// <returns>
	/// Принятые в проекте коды сообщения<br></br>
	/// 0 - обычное сообщение (для прогресса, например)<br></br>
	/// 1 - успех (сообщение обычно будет гореть зелёным цветом)<br></br>
	/// 2 - некритичная ошибка (жёлтый)<br></br>
	/// 3 - критическая ошибка (красный)<br></br>
	/// </returns>
	public class Twident_Status : EventArgs {
		public readonly int StatusCode;
		public readonly string Message;
		public readonly string Description;
		public readonly bool? Permanent;
		public Twident_Status(int StatusCode, string Message, string Description, bool? Permanent) {
			this.StatusCode = StatusCode;
			this.Message = Message;
			this.Description = Description;
			this.Permanent = Permanent;
		}
	}



	// -- Класс эвента для передачи була --
	public class Twident_Bool : EventArgs {
		public readonly bool Handle;
		public Twident_Bool(bool Handle) {
			this.Handle = Handle;
		}
	}


	// -- Класс эвента для передачи числа --
	public class Twident_Int : EventArgs {
		public readonly int Value;
		public Twident_Int(int Value) {
			this.Value = Value;
		}
	}

}
