using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Post;

[ServiceFilter(typeof(RequireAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class UserPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public UserPostController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("count/{appUserId}")]
    public async Task<IActionResult> Count(string appUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/count/{appUserId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet("countByListOfUserId/{collectionUserId}")]
    public async Task<IActionResult> CountByListOfAppUsers(string collectionUserId)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/countByListOfUserId/{collectionUserId}");
        var count = await responseMessage.Content.ReadFromJsonAsync<int>();

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var responseMessage = await _httpClient.GetAsync("UserPost");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpGet("getByUserId")]
    public async Task<IActionResult> GetByUserId(string appUserId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getByUserId?appUserId={appUserId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByUserId")]
    public async Task<IActionResult> GetMoreByUserId(string appUserId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getMoreByUserId?appUserId={appUserId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(string appUserId, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getNewPosts?appUserId={appUserId}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getByListOfUserId")]
    public async Task<IActionResult> GetByListOfUserIds(string collectionUserId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getByListOfUserId?collectionUserId={collectionUserId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByListOfUserId")]
    public async Task<IActionResult> GetMoreByListOfUserIds(string collectionUserId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getMoreByListOfUserId?collectionUserId={collectionUserId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewByListOfUserId")]
    public async Task<IActionResult> GetNewByListOfUserIds(string collectionUserId, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"UserPost/getNewByListOfUserId?collectionUserId={collectionUserId}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("UserPost", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var post = await responseMessage.Content.ReadFromJsonAsync<UserPostModel>();

            return Ok(post);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserPostModel model)
    {
        var responseMessage = await _httpClient.PutAsync("UserPost", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var responseMessage = await _httpClient.DeletAsync($"UserPost/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            return Ok();
        }

        return BadRequest();
    }
}
