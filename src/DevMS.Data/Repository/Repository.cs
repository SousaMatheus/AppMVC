using AppMVC.Inicial.Models;
using DevMS.Business.Interfaces;
using DevMS.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevMS.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<T> DbSet;

        public Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = Db.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual async Task<T> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public virtual async Task<List<T>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }
        public virtual async Task Adicionar(T entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }
        public virtual async Task Atualizar(T entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }
        public virtual async Task Remover(Guid id)
        {
            DbSet.Remove(new T { Id = id });
            await SaveChanges();
        }
        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
