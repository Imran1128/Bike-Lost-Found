using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BikeLostAndFound.Data;
using BikeLostAndFound.Interfaces;
using BikeLostAndFound.Migrations;
using BikeLostAndFound.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly MyDbContext _myDbContext;

        public BaseRepository(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<bool> DeleteByAsync(TEntity entity)
        {
            try
            {
                _myDbContext.Set<TEntity>().Remove(entity);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteRangeByAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                _myDbContext.Set<TEntity>().RemoveRange(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            try
            {
                _myDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var res = await _myDbContext.Set<TEntity>().FindAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                return await _myDbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TEntity> GetInsertedObjByAsync(TEntity entity)
        {
            try
            {
                await _myDbContext.Set<TEntity>().AddAsync(entity);
                await SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<bool> InsertByAsync(TEntity entity)
        {
            try
            {
                await _myDbContext.Set<TEntity>().AddAsync(entity);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertRangeByAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await _myDbContext.Set<TEntity>().AddRangeAsync(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateByAsync(TEntity entity)
        {
            try
            {
                var result = _myDbContext.Set<TEntity>().Attach(entity);
                result.State = EntityState.Modified;
                await SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateRangeByAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                _myDbContext.Set<TEntity>().UpdateRange(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _myDbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetSingleData(Expression<Func<TEntity, bool>> Condition)
        {
            try
            {
                _myDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var res = await _myDbContext.Set<TEntity>().FirstOrDefaultAsync(Condition);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}