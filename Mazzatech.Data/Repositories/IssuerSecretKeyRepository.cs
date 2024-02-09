using Mazzatech.Data.EF;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.Repositories
{
    public class IssuerSecretKeyRepository : IIssuerSecretKeyRepository
    {
        private readonly AuthenticateContext _context;
        public IssuerSecretKeyRepository(AuthenticateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IssuerSecretKeyEntityModel>> Add(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                return await _context.IssuerSecretKeys.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerSecretKeyEntityModel>> Delete(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return await _context.IssuerSecretKeys.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerSecretKeyEntityModel>> DeleteById(int id)
        {
            try
            {
                var iSKDb = _context.Users.Where(x => x.Id == id).First();
                _context.Remove(iSKDb);
                _context.SaveChanges();

                return await _context.IssuerSecretKeys.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<IssuerSecretKeyEntityModel>> GetAll()
        {
            try
            {
                return await _context.IssuerSecretKeys.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IssuerSecretKeyEntityModel> GetById(int id)
        {
            try
            {
                return await _context.IssuerSecretKeys.Where(x => x.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IssuerSecretKeyEntityModel> GetSecretKeyByIssuer(string issuer)
        {
            try
            {
                return await _context.IssuerSecretKeys.Where(x => x.Issuer == issuer).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerSecretKeyEntityModel>> Update(IssuerSecretKeyEntityModel entity)
        {
            try
            {
                var iUSKTDb = await _context.IssuerSecretKeys.Where(x => x.Id == entity.Id).FirstAsync();
                if (iUSKTDb != null)
                {
                    // Atualiza as propriedades da entidade rastreada
                    _context.Entry(iUSKTDb).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }

                return await _context.IssuerSecretKeys.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
