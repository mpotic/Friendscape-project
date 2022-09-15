using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Miscellaneous
{
	public enum FriendType
	{
		BEST_FRIEND = 0,
		GOOD_FRIEND = 1,
		ACQUAINTANCE = 2,
		FRIEND = 3
	}

	public static class EFriendType
	{
		public static FriendType DetermineFriendType(string type)
		{
			FriendType friendType = FriendType.ACQUAINTANCE;
			switch (type)
			{
				case "BEST_FRIEND":
					friendType = FriendType.BEST_FRIEND;
					break;

				case "GOOD_FRIEND":
					friendType = FriendType.GOOD_FRIEND;
					break;

				case "ACQUAINTANCE":
					friendType = FriendType.ACQUAINTANCE;
					break;

				case "FRIEND":
					friendType = FriendType.FRIEND;
					break;

				default:
					friendType = FriendType.ACQUAINTANCE;
					break;
			}

			return friendType;
		}
	}
}
