using Mazzatech.Data.EF;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mazzatech.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticateContext _context;
        public UserRepository(AuthenticateContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntityModel>> Add(UserEntityModel entity)
        {
            try
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();

                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserEntityModel>> Delete(UserEntityModel entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserEntityModel>> DeleteById(int id)
        {
            try
            {
                var protocolo = _context.Users.Where(x => x.Id == id).First();
                _context.Remove(protocolo);
                _context.SaveChanges();

                return await _context.Users.ToListAsync();
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

        public async Task<IEnumerable<UserEntityModel>> GetAll()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserEntityModel> GetById(int id)
        {
            try
            {
                return await _context.Users.Where(x => x.Id == id).FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsValidUser(UserEntityModel userEntityModel)
        {
            try
            {
                return _context.Users.Where(x => x.UserName == userEntityModel.UserName && x.Password == userEntityModel.Password).Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserEntityModel>> Update(UserEntityModel entity)
        {
            try
            {
                var userDb = await _context.Users.Where(x => x.Id == entity.Id).FirstAsync();
                if (userDb != null)
                {
                    // Atualiza as propriedades da entidade rastreada
                    _context.Entry(userDb).CurrentValues.SetValues(entity);
                    _context.SaveChanges();
                }

                return await _context.Users.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
