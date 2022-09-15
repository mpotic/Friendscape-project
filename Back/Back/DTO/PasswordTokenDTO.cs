using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.DTO
{
	public class PasswordTokenDTO
	{
		public string NewPassword { get; set; }
		public string OldPassword { get; set; }
		public string Token { get; set; }
	}
}
