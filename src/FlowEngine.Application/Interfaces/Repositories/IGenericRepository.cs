using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FlowEngine.Domain.Common;

namespace FlowEngine.Application.Interfaces.Repositories
{
    // T, bir BaseEntity olmak zorundadır (Where T : BaseEntity)
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        
        // Veri tabanına sorgu atarken filtreleme yapabilmek için (Linq expression)
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T entity); // Update genelde async olmaz, sadece state değiştirir.
        void Delete(T entity);
    }
}