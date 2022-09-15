using Back.Infrastructure;
using Back.Interfaces;
using Back.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Repositories
{
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{
		public PersonRepository(DatabaseContext context) : base(context) { }

		public Person GetPersonWithFriends(string email)
		{
			Person person = null;
			try
			{
				person = _context.People.Where(x => x.Email == email)
					.Include(x => x.Friendships).ThenInclude(x => x.Friend).First();
			}
			catch { }

			return person;
		}

		public Person GetPersonByEmail(string email)
		{
			try
			{
				var person = _context.People.Where(x => x.Email == email).First();
				return person;
			}
			catch
			{
				return null;
			}
		}

		public List<Person> SearchByNameAndSurname(string name, string surname)
		{
			List<Person> people = _context.People.
				Where(x => (((x.Name.ToLower().Contains(name.ToLower()) &&
					x.Surname.ToLower().Contains(surname.ToLower())) ||
					(name.ToLower().Contains(x.Name.ToLower()) &&
					surname.ToLower().Contains(x.Surname.ToLower())))
					&& x.Role != "REQUEST")).ToList();

			return people;
		}

		public bool RemovePersonByEmail(string email)
		{
			try
			{
				_context.People.Remove(_context.People.Where(x => x.Email == email).First());
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ChangePersonRole(string email, string role)
		{
			try
			{
				Person person = _context.People.Where(x => x.Email == email).First();
				person.Role = role;
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
