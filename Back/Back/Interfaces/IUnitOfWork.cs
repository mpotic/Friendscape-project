using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IPersonRepository PersonRepo {get;}
		IFriendshipRepository FriendshipRepo { get; }
		int Complete();
	}
}
