using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dtos;
using api.Dtos.Stock;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Repository;
using api.Interfaces;


namespace api.Controllers
{
	[Route("api/stock")]
	[ApiController]
    public class StockController: ControllerBase
    {
		private readonly ApplicationDBContext _context;
		private readonly IStockRepository _repository;
        public StockController(ApplicationDBContext context, IStockRepository repository)
		{
			_context = context;
			_repository = repository;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var stock = await _repository.GetAllAsync();
			var stockDto = stock.Select(s => s.ToStockDto()).ToList();

			return Ok(stockDto); //200 + todos os stocks
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var stock = await _repository.GetByIdAsync(id);

			if(stock == null)
			{
				return NotFound(); //404
			}

			return Ok(stock.ToStockDto()); //200 + stock especifico
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
		{
			var stockModel = await _repository.CreateAsync(stockDto.ToStockFromCreateDTO());

			if (stockModel == null)
			{
				return StatusCode(500, "Erro ao criar o Stock");
			}

			return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id}, stockModel.ToStockDto());
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
		{
			var stockModel = await _repository.UpdateAsync(id, updateDto);

			if(stockModel == null)
			{
				return NotFound();

			}

			return Ok(stockModel.ToStockDto());
		}
		[HttpDelete]
		[Route("{id}")]

		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var stock = await _repository.Delete(id);

			if(stock == null)
			{
				return NotFound();

			}

			return NoContent(); //204 + nada para retornar
		}
    }
}