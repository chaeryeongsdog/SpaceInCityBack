using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models; // 假設有一個 `Message` 模型

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST api/messages
        [HttpPost("PostMessage")]
        public async Task<IActionResult> PostMessage([FromBody] string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Content cannot be empty.");
            }

            // 檢查資料表中是否已經有相同的 Content
            var existingMessage = await _context.Messages
                .FirstOrDefaultAsync(m => m.Content == content);

            if (existingMessage != null)
            {
                // 如果已經有相同的字串，則將 Count + 1
                existingMessage.Count += 1;
                _context.Messages.Update(existingMessage); // 更新資料
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Content already exists, count incremented.", Count = existingMessage.Count });
            }
            else
            {
                // 如果沒有相同的字串，則新增一條資料，並將 Count 設為 1
                var newMessage = new Message
                {
                    Content = content,
                    Count = 1
                };
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Content added.", Count = newMessage.Count });
            }
        }

        [HttpGet("GetMessage")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            // 從資料庫中抓取所有 Message 資料
            var messages = await _context.Messages.ToListAsync();

            if (messages == null || messages.Count == 0)
            {
                return NotFound("No messages found.");
            }

            return Ok(messages); // 回傳資料
        }
    }
}
