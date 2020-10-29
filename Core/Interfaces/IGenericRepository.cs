using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetListAllAsync();
		Task<T> GetEntityWithSpecAsync(ISpecification<T> specification);
		Task<IReadOnlyList<T>> GetListAllWithSpecAsync(ISpecification<T> specification);
	}
}