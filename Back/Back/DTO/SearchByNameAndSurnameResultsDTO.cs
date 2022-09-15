using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.DTO
{
	public class SearchByNameAndSurnameResultsDTO
	{
		public List<Person> People { get; set; }
	}
}
