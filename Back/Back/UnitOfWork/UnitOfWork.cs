using Back.Infrastructure;
using Back.Interfaces;
using Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public DatabaseContext _context { get; private set; }
		public IPersonRepository PersonRepo { get; private set; }
		public IFriendshipRepository FriendshipRepo { get; private set; }

		public UnitOfWork(DatabaseContext context)
		{
			_context = context;
			PersonRepo = new PersonRepository(context);
			FriendshipRepo = new FriendshipRepository(context);
		}

		public int Complete()
		{
			try
			{
				return _context.SaveChanges();
			}
			catch (Exception e)
			{
				if(e.InnerException.Message.Contains("Cannot insert duplicate key row in object")) 
				{
					throw new Exception("The data already exists and can not be duplicated!");
				}

				throw e.InnerException;
			}
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
