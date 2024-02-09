namespace Mazzatech.Domain.Interfaces.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Add(TEntity entity);
        Task<IEnumerable<TEntity>> Update(TEntity entity);
        Task<IEnumerable<TEntity>> Delete(TEntity entity);
        Task<IEnumerable<TEntity>> DeleteById(int id);
        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        void Dispose();
    }
}
