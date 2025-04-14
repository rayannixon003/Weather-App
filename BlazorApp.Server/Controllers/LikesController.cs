using Microsoft.AspNetCore.Mvc;
using BlazorApp.Server.Services;
using BlazorApp.Server.Models;
using MongoDB.Driver;

namespace BlazorApp.Server.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class LikesController : ControllerBase
    //{
    //    private readonly MongoDBService _mongoDBService;

    //    public LikesController(MongoDBService mongoDBService)
    //    {
    //        _mongoDBService = mongoDBService;
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> CreateLike([FromBody] Like like)
    //    {
    //        await _mongoDBService.CreateLike(like);
    //        return Ok();
    //    }

    //    [HttpGet("{userId}")]
    //    public async Task<IActionResult> GetLikesByUser(string userId)
    //    {
    //        var likes = await _mongoDBService.GetLikesByUser(userId);
    //        return Ok(likes);
    //    }
    //}
    //[Route("api/likes")]
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly IMongoCollection<Like> _likesCollection;

        public LikesController(IMongoDatabase database)
        {
            _likesCollection = database.GetCollection<Like>("Likes");
        }

        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] Like like)
        {
            try
            {
                if (like == null || string.IsNullOrEmpty(like.UserId))
                {
                    return BadRequest("Invalid like data.");
                }

                like.Timestamp = DateTime.UtcNow;

                await _likesCollection.InsertOneAsync(like);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding like: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

    public class Like
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}