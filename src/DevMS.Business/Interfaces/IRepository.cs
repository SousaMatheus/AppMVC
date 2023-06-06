using DevMS.Business.Models;
using System.Linq.Expressions;

namespace DevMS.Business.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Adicionar(T entity);
        Task <T> ObterPorId(Guid id);
        Task <List<T>> ObterTodos();
        Task Atualizar(T entity);
        Task Remover(Guid id);
        Task <IEnumerable<T>> Buscar (Expression<Func<T, bool>> predicate);
        Task <int> SaveChanges();
    }
}
