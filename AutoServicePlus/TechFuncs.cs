using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServicePlus {
	class TechFuncs {

		public static string UnixToDate(long UnixTime) {
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dt = dt.AddSeconds(UnixTime);
			return dt.ToLocalTime().ToString().Substring(0, dt.ToLocalTime().ToString().IndexOf(" "));
		}

		public static long GetUnixTime() {
			return DateTimeOffset.Now.ToUnixTimeSeconds();
		}

	}
}
