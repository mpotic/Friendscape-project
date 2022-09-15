using Back.Infrastructure;
using Back.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Back.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected DatabaseContext _context;
		public GenericRepository(DatabaseContext context)
		{
			_context = context;
		}

		public void Add(T entity)
		{
			try
			{
				_context.Set<T>().Add(entity);
			}
			catch (Exception e)
			{ 
				throw new Exception(e.InnerException.Message);
			}
		}

		public void AddRange(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<T> GetAll()
		{
			return _context.Set<T>();
		}

		public T GetById(long id)
		{
			return _context.Set<T>().Find(id);
		}

		public void Remove(T entity)
		{
			throw new NotImplementedException();
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			throw new NotImplementedException();
		}
	}
}
