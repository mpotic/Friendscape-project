using Back.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.DTO
{
	public class AddingFriendDTO
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public FriendType Type { get; set; }
	}
}
