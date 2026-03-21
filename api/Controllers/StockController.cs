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
		public IActionResult GetAll()
		{
			var stocks = _context.Stocks.ToList()
			.Select(s => s.ToStockDto());

			return Ok(stocks); //200 + todos os stocks
		}

		[HttpGet("{id}")]

		// para a pessoa que vai ler isso daqui, isso nao foi feito por IA
		// eu realmente gosto de colocar comentarios!
		// me ajuda a lembrar quando vou reaplicar ou revisar o codigo.
		public IActionResult GetById([FromRoute] Guid id)
		{
			var stock = _context.Stocks.Find(id);

			if(stock == null)
			{
				return NotFound(); //404
			}

			return Ok(stock.ToStockDto()); //200 + stock especifico
		}

		[HttpPost]
		public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
		{
			var stockModel = stockDto.ToStockFromCreateDTO();
			_context.Stocks.Add(stockModel);
			_context.SaveChanges();

			return CreatedAtAction(nameof(GetById), new{ id = stockModel.Id}, stockModel.ToStockDto());
		}

		[HttpPut]
		[Route("{id}")]
		public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto updateDto)
		{
			var stockModel = _context.Stocks.Find(id);

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

			_context.SaveChanges();

			return Ok(stockModel.ToStockDto());
		}
		[HttpDelete]
		[Route("{id}")]

		public IActionResult Delete([FromRoute] Guid id)
		{
			var stock = _context.Stocks.Find(id);

			if(stock == null)
			{
				return NotFound();
			}

			_context.Remove(stock);
			_context.SaveChanges();
			
			return NoContent(); //204 + nada para retornar
		}
    }
}