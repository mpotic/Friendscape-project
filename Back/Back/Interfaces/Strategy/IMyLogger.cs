using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Back.Interfaces.Strategy
{
	public interface IMyLogger
	{
		public void DoLog(string message, string path);
	}
}
