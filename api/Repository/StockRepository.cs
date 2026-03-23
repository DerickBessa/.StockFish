using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
	public class StockRepository : IStockRepository
	{
		private readonly ApplicationDBContext _context;
		public StockRepository(ApplicationDBContext context)
		{
			_context = context;

		}
		public Task<List<Stock>> GetAllAsync()
		{

			return _context.Stocks.Include(c =>c.Comments).ToListAsync();
		}

		public async Task<Stock?> CreateAsync(Stock stockModel)
		{
			_context.Stocks.Add(stockModel);
			await _context.SaveChangesAsync();

			return stockModel;
		}

		public async Task<Stock?> Delete(Guid id)
		{
			var stock = await _context.Stocks.FindAsync(id);

			if(stock == null)
			{
				return null;
			}

			_context.Stocks.Remove(stock);
			await _context.SaveChangesAsync();
			return stock;
		}

		public async Task<Stock?> GetByIdAsync(Guid id)
		{
			return await _context.Stocks.Include(c =>c.Comments).FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<Stock?> UpdateAsync(Guid id, UpdateStockRequestDto stockDto)
		{
			var stock = await _context.Stocks.FindAsync(id);

			if(stock == null)
			{
				return null;
			}

			stock.Symbol = stockDto.Symbol;
			stock.CompanyName = stockDto.CompanyName;
			stock.Purchase = stockDto.Purchase;
			stock.LastDiv = stockDto.LastDiv;
			stock.Industry = stockDto.Industry;
			stock.MarketCap = stockDto.MarketCap;

			await _context.SaveChangesAsync();

			return stock;
		}

		public Task<bool> StockExists(Guid id)
		{
			return _context.Stocks.AnyAsync(s => s.Id == id);
		}
	}
}