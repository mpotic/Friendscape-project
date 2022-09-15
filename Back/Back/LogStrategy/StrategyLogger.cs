using Back.Interfaces.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.LogStrategy
{
	public class StrategyLogger : IStrategyLogger
	{
		IMyLogger myLogger { get; set; }
		
		public void DoLog(string message, string path = "C:/Users/Milos/Documents/FAKULTET/3 godina/6_semestar/RVA/PROJECT/Back/Back/Logs/Logs.txt") 
		{
			this.myLogger.DoLog(message, path);
		}

		public void SetStrategy(string strat) 
		{
			switch (strat)
			{
				case "INFO":
					myLogger = new InfoLogger();
					break;

				case "WARNING":
					myLogger = new WarningLogger();
					break;

				case "ERROR":
					myLogger = new ErrorLogger();
					break;

				default:
					myLogger = new InfoLogger();
					break;
			}
		}
	}
}
