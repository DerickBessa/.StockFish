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

namespace api.Controllers
{
	[Route("api/stock")]
	[ApiController]
    public class StockController: ControllerBase
    {
		private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var stocks = await _context.Stocks.ToListAsync();
			var stockDto = stocks.Select(s => s.ToStockDto());

			return Ok(stockDto); //200 + todos os stocks
		}

		[HttpGet("{id}")]

		// para a pessoa que vai ler isso daqui, isso nao foi feito por IA
		// eu realmente gosto de colocar comentarios!
		// me ajuda a lembrar quando vou reaplicar ou revisar o codigo.
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var stock = await _context.Stocks.FindAsync(id);

			if(stock == null)
			{
				return NotFound(); //404
			}

			return Ok(stock.ToStockDto()); //200 + stock especifico
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
		{
			var stockModel = stockDto.ToStockFromCreateDTO();
			_context.Stocks.Add(stockModel);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id}, stockModel.ToStockDto());
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
		{
			var stockModel = await _context.Stocks.FindAsync(id);

			if(stockModel == null)
			{
				return NotFound();
			}

			stockModel.Symbol = updateDto.Symbol;
			stockModel.CompanyName = updateDto.CompanyName;
			stockModel.Purchase = updateDto.Purchase;
			stockModel.LastDiv = updateDto.LastDiv;
			stockModel.Industry = updateDto.Industry;
			stockModel.MarketCap = updateDto.MarketCap;

			await _context.SaveChangesAsync();

			return Ok(stockModel.ToStockDto());
		}
		[HttpDelete]
		[Route("{id}")]

		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var stock = await _context.Stocks.FindAsync(id);

			if(stock == null)
			{
				return NotFound();
			}

			_context.Remove(stock);
			await _context.SaveChangesAsync();
			
			return NoContent(); //204 + nada para retornar
		}
    }
}