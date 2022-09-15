using Back.Infrastructure;
using Back.Interfaces;
using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Back.Repositories
{
	public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
	{
		public FriendshipRepository(DatabaseContext context) : base(context) { }

		public bool DeleteWhereFriendId(long friendId)
		{
			var removal = _context.Friendships.Where(x => x.FriendId == friendId);
			
			if (removal == null)
			{
				return false;
			}

			_context.Friendships.RemoveRange(removal);

			return true;
		}

		public bool DeleteWhereUserId(long userId)
		{
			var removal = _context.Friendships.Where(x => x.UserId == userId);

			if (removal == null)
			{
				return false;
			}

			_context.Friendships.RemoveRange(removal);

			return true;
		}
	}
}
