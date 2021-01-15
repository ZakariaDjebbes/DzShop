using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _context;

		public GenericRepository(StoreContext context)
		{
			_context = context;
		}

        public void Add(T entity)
        {
			_context.Set<T>().Add(entity);
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
		{
			return await ApplySpecification(specification).CountAsync();
		}

        public void Delete(T enity)
        {
			_context.Set<T>().Remove(enity);
        }

        public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<T> GetEntityWithSpecAsync(ISpecification<T> specification)
		{
			return await ApplySpecification(specification).FirstOrDefaultAsync();
		}

		public async Task<IReadOnlyList<T>> GetListAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetListAllWithSpecAsync(ISpecification<T> specification)
		{
			return await ApplySpecification(specification).ToListAsync();
		}

        public void Update(T entity)
        {
			_context.Set<T>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
		{
			return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification); 
		}
	}
}
