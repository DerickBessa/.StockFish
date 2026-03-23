using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Mappers
{
    public static class CommentMapper
    {
		public static CommentDto ToCommentDto(this Comment commentModel)
		{
			return new CommentDto
			{
				Id = commentModel.Id,
				Title = commentModel.Title,
				Content = commentModel.Content,
				CreatedOn = commentModel.CreatedOn,
				StockId = commentModel.StockId
			};
		}

		public static Comment ToCommentFromCreate(this CreateCommentRequestDto comment, Guid stockId)
		{
			return new Comment
			{
				Title = comment.Title,
				Content = comment.Content,
				StockId = stockId
			};
		}

		public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto comment)
		{
			return new Comment
			{
				Title = comment.Title,
				Content = comment.Content
			};
		}
    }
}