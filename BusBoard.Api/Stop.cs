using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBoard.Api
{
	public class Stop
	{
		public string naptanId { get; set; }
		public float distance { get; set; }
		public string commonName { get; set; }
		public List<Bus> buses { get; set; }
	}
}
