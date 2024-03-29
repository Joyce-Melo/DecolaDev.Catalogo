﻿using AutoMapper;
using DecolaDev.Catalogo.Domain.Core.Entity;
using DecolaDev.Catalogo.Domain.Core.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DecolaDev.Catalogo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public abstract class GenericControllerCrud<TKey, TEntity, TModel> : ControllerBase where TKey : struct where TEntity : BaseEntity<TKey>
    {
        private readonly IServiceCrud<TKey, TEntity> _Service;
        protected readonly IMapper _Mapper;

        protected GenericControllerCrud(
            IServiceCrud<TKey, TEntity> service,
            IMapper mapper)
        {
            _Service = service;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<TEntity> entities = await _Service.GetAllAsync();
            IEnumerable<TModel> models = _Mapper.Map<IEnumerable<TModel>>(entities);
            return Ok(models);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(TKey id)
        {
            TEntity entity = await _Service.GetAsync(id);

            if (entity == null) return NotFound();

            TModel model = _Mapper.Map<TModel>(entity);
            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] TModel model)
        {
            TEntity entity = await _Service.AddAsync(_Mapper.Map<TEntity>(model));

            string action = Url.Action("Get", this.ControllerContext.ActionDescriptor.ControllerName, new { id = entity.Id });

            return Created(action, _Mapper.Map<TModel>(entity));
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] TModel model) => Ok(await _Service.UpdateAsync(_Mapper.Map<TEntity>(model)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TKey id) => Ok(await _Service.DeleteAsync(id));
    }
}