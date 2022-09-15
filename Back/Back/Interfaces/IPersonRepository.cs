using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces
{
	public interface IPersonRepository : IGenericRepository<Person>
	{
		Person GetPersonWithFriends(string email);
		Person GetPersonByEmail(string email);
		List<Person> SearchByNameAndSurname(string name, string surname);
		bool RemovePersonByEmail(string email);
		bool ChangePersonRole(string email, string role);
	}
}
