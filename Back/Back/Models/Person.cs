using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Models
{
	public class Person
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		/// <summary>
		/// Should be set to: 'ADMIN', 'USER', 'REQUEST' (When registering user is written in the database with 'REQUEST' role. If the use is approved the role is changed to 'USER', otherwise the user is deleted.)
		/// </summary>
		public string Role { get; set; }
		public List<Friendship> Friendships { get; set; }
		public List<Friendship> Friends { get; set; }
	}
}
