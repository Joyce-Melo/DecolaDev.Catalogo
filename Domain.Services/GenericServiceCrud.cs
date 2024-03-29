﻿using DecolaDev.Catalogo.Domain.Core.Entity;
using DecolaDev.Catalogo.Domain.Core.Interfaces.Repository;
using DecolaDev.Catalogo.Domain.Core.Interfaces.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecolaDev.Catalogo.Domain.Services
{
    public class GenericServiceCrud<TKey, TEntity> : IServiceCrud<TKey, TEntity> where TKey : struct where TEntity : BaseEntity<TKey>
    {
        private readonly IRepositoryCrud<TKey, TEntity> _Repository;

        public GenericServiceCrud(IRepositoryCrud<TKey, TEntity> repository)
        {
            _Repository = repository;
        }

        public async Task<TEntity> AddAsync(TEntity obj) => await _Repository.AddAsync(obj);
        public async Task<TEntity> DeleteAsync(TKey id) => await _Repository.DeleteAsync(id);
        public async Task<TEntity> GetAsync(TKey id) => await _Repository.GetAsync(id);
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _Repository.GetAllAsync();
        public async Task<TEntity> UpdateAsync(TEntity obj) => await _Repository.UpdateAsync(obj);
    }
}
