using Back.Interfaces.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Back.LogStrategy
{
	public class ErrorLogger : IMyLogger
	{
		public void DoLog(string message, string path)
		{
			Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.File(path).CreateLogger();
			log.Error(message);
			log.Dispose();
		}
	}
}
