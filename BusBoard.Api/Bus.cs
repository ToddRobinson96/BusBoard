using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.Api
{
	public class Bus
	{
		public string lineName { get; set; }
		public string destinationName { get; set; }
		public DateTime expectedArrival { get; set; }
	}
}
