﻿using DomainLayer.Contracts;
using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(StoreDbContext _dbContext) 
        : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
           await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }


        #region With Specifications

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
         => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>() , specifications).ToListAsync();
        
        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();        

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();
        
        #endregion

    }
}
