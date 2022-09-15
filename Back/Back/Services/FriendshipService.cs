using Back.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Services
{
	public class FriendshipService : IFriendshipService
	{
		IUnitOfWork _unitOfWork;

		public FriendshipService(IUnitOfWork unitOfWork) 
		{
			_unitOfWork = unitOfWork;
		}

		public bool DeleteByFriendId(long friendId)
		{
			bool retVal = _unitOfWork.FriendshipRepo.DeleteWhereFriendId(friendId);
			
			return retVal;
		}

		public bool DeleteByUserId(long userId)
		{
			bool retVal = _unitOfWork.FriendshipRepo.DeleteWhereUserId(userId);

			return retVal;
		}
	}
}
