using Mazzatech.Data.EF;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.Repositories
{
    public class IssuerUserSecretKeyTokenRepository : IIssuerUserSecretKeyTokenRepository
    {
        private readonly AuthenticateContext _context;
        public IssuerUserSecretKeyTokenRepository(AuthenticateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Add(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                return await _context.IssuerUserSecretKeyTokens.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Delete(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return await _context.IssuerUserSecretKeyTokens.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> DeleteById(int id)
        {
            try
            {
                var iUSKTDb = _context.Users.Where(x => x.Id == id).First();
                _context.Remove(iUSKTDb);
                _context.SaveChanges();

                return await _context.IssuerUserSecretKeyTokens.ToListAsync();
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

        public async Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> GetAll()
        {
            try
            {
                return await _context.IssuerUserSecretKeyTokens.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IssuerUserSecretKeyTokenEntityModel> GetById(int id)
        {
            try
            {
                return await _context.IssuerUserSecretKeyTokens.Where(x => x.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetTokenById(int id)
        {
            try
            {
                return await _context.IssuerUserSecretKeyTokens.Where(x => x.Id == id).Select(y => y.Token).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IssuerUserSecretKeyTokenEntityModel>> Update(IssuerUserSecretKeyTokenEntityModel entity)
        {
            try
            {
                var iUSKTDb = await _context.IssuerUserSecretKeyTokens.Where(x => x.Id == entity.Id).FirstAsync();
                if (iUSKTDb != null)
                {
                    // Atualiza as propriedades da entidade rastreada
                    _context.Entry(iUSKTDb).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }

                return await _context.IssuerUserSecretKeyTokens.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
