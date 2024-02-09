using Mazzatech.Data.EF;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.Repositories
{
    public class DbErrorLoggerRepository : IDbErrorLoggerRepository
    {
        private readonly Context _context;
        public DbErrorLoggerRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DbErrorLoggerEntityModel>> Add(DbErrorLoggerEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                return await _context.DbErrorsLoggers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddWithoutReturn(DbErrorLoggerEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DbErrorLoggerEntityModel>> Delete(DbErrorLoggerEntityModel entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return await _context.DbErrorsLoggers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DbErrorLoggerEntityModel>> DeleteById(int id)
        {
            try
            {
                var protocolo = _context.DbErrorsLoggers.Where(x => x.Id == id).First();
                _context.Remove(protocolo);
                _context.SaveChanges();
                
                return await _context.DbErrorsLoggers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DbErrorLoggerEntityModel>> GetAll()
        {
            try
            {
                return await _context.DbErrorsLoggers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DbErrorLoggerEntityModel> GetById(int id)
        {
            try
            {
                return await _context.DbErrorsLoggers.Where(x => x.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DbErrorLoggerEntityModel>> Update(DbErrorLoggerEntityModel entity)
        {
            try
            {
                var dbErrorLogger = await _context.DbErrorsLoggers.Where(x => x.Id == entity.Id).FirstAsync();
                if (dbErrorLogger != null)
                {
                    // Atualiza as propriedades da entidade rastreada
                    _context.Entry(dbErrorLogger).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }

                return await _context.DbErrorsLoggers.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
