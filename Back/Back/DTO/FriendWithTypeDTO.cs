using Back.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.DTO
{
	public class FriendWithTypeDTO
	{
		public PersonBasicInfoDTO Friend { get; set; }
		public string Type { get; set; }
	}
}
