using Back.Interfaces.Strategy;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.LogStrategy
{
	public class WarningLogger : IMyLogger
	{
		public void DoLog(string message, string path)
		{
			Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.File(path).CreateLogger();
			log.Warning(message);
			log.Dispose();
		}
	}
}
