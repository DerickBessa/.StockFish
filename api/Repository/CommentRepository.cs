using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using api.Dtos.Comment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
		private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
		{
			_context = context;
		}

		
		public async Task<List<Comment>> GetAllAsync()
		{
			return await _context.Comments.ToListAsync();
		}

		public async Task<Comment?> CreateAsync(Comment commentModel)
		{	

			_context.Comments.Add(commentModel);
			await _context.SaveChangesAsync();

			return commentModel;
		}

		public async Task<Comment?> Delete(Guid id)
		{
			var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

			if(comment == null)
			{
				return null;
			}

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
			return comment;
		}

		public async Task<Comment?> GetByIdAsync(Guid id)
		{
			return await _context.Comments.FindAsync(id);
		}

		public async Task<Comment?> UpdateAsync(Guid id, Comment commentModel)
		{
			var existingComment = await _context.Comments.FindAsync(id);

			if(existingComment == null)
			{
				return null;
			}

			existingComment.Title = commentModel.Title;
			existingComment.Content = commentModel.Content;

			await _context.SaveChangesAsync();

			return existingComment;
		}
	}
}