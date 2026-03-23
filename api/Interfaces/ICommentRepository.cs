using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
		Task<Comment> GetByIdAsync(Guid id);
		Task<Comment?> UpdateAsync(Guid id, Comment commentModel);
		Task<Comment> CreateAsync(Comment commentModel);		
		Task<Comment> Delete(Guid id);
    }
}