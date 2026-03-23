using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Route("api/comment")]
	[ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentrepo;
		private readonly IStockRepository _stockrepo;
		public CommentController(ICommentRepository commentrepo, IStockRepository stockRepo)
		{
			_commentrepo = commentrepo;	
			_stockrepo = stockRepo;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var comments = await _commentrepo.GetAllAsync();
			var commentDtos = comments.Select(s => s.ToCommentDto()).ToList();

			return Ok(commentDtos);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id)
		{
			var comment = await _commentrepo.GetByIdAsync(id);

			if(comment == null)
			{
				return NotFound();
			}

			return Ok(comment.ToCommentDto());
		}
		[HttpPost("{stockId}")]
		public async Task<IActionResult> Create([FromRoute] Guid stockId,CreateCommentRequestDto commentDto)
		{
			if (!await _stockrepo.StockExists(stockId))
			{
				return BadRequest("Stock Doesn't Exists"); //400
			}
			var commentModel = commentDto.ToCommentFromCreate(stockId);
			await _commentrepo.CreateAsync(commentModel);

			return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());
		}

		[HttpPut]
		[Route("{id}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCommentRequestDto updateDto)
		{
			var comment = await _commentrepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());

			if(comment == null)
			{
				return NotFound("Comment not Found");
			}

			return Ok(comment.ToCommentDto());
		}

		[HttpDelete]
		[Route("{id}")]

		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var comment = await _commentrepo.Delete(id);

			if(comment == null)
			{
				return NotFound();
			}

			return Ok(comment.ToCommentDto);
		}
    }
}