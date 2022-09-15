using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces.Strategy
{
	public interface IStrategyLogger
	{
		public void DoLog(string message, string path = "C:/Users/Milos/Documents/FAKULTET/3 godina/6_semestar/RVA/PROJECT/Back/Back/Logs/Logs.txt");
		public void SetStrategy(string strat);
	}
}
