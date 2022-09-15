using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces
{
	public interface IFriendshipService
	{
		public bool DeleteByFriendId(long friendId);
		public bool DeleteByUserId(long userId);
	}
}
