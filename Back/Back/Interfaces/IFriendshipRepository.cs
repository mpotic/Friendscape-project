using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces
{
	public interface IFriendshipRepository : IGenericRepository<Friendship>
	{
		public bool DeleteWhereFriendId(long friendId);
		public bool DeleteWhereUserId(long userId);
	}
}
