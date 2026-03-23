using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Controllers;
using api.Repository;
using api.Dtos.Stock;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
		Task<Stock?> GetByIdAsync(Guid id);
		Task<Stock?> CreateAsync(Stock stockModel);
		Task<Stock?> UpdateAsync(Guid id , UpdateStockRequestDto stockDto);
		Task<Stock?> Delete(Guid id);

		Task<bool> StockExists(Guid id);
	}
}