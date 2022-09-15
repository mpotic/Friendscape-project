using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Models
{
	public class Friendship
	{
		public long Id { get; set; }

		public long UserId { get; set; }
		public Person User { get; set; }

		public long FriendId { get; set; }
		public Person Friend { get; set; }

		public int FriendType{get; set; }
	}
}
