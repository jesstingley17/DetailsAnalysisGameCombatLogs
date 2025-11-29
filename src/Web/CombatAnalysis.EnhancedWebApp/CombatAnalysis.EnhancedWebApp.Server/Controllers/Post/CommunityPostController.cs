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
public class CommunityPostController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public CommunityPostController(IOptions<Cluster> cluster, IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Communication;
    }

    [HttpGet("count/{communityId}")]
    public async Task<IActionResult> Count(int communityId)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/count/{communityId}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }

        return BadRequest();
    }

    [HttpGet("countByListOfCommunities/{communityIds}")]
    public async Task<IActionResult> CountByListOfAppUsers(string communityIds)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/countByListOfCommunities/{communityIds}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var count = await responseMessage.Content.ReadFromJsonAsync<int>();

            return Ok(count);
        }

        return BadRequest();
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/{id}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityPost = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

            return Ok(communityPost);
        }

        return BadRequest();
    }

    [HttpGet("getByCommunityId")]
    public async Task<IActionResult> GetByCommunityId(int communityId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByCommunityId?communityId={communityId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByCommunityId")]
    public async Task<IActionResult> GetMoreByCommunityId(int communityId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getMoreByCommunityId?communityId={communityId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewPosts")]
    public async Task<IActionResult> GetNewPosts(int communityId, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getNewPosts?communityId={communityId}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getByListOfCommunityId")]
    public async Task<IActionResult> GetByListOfCommunityIds(string collectionCommunityId, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getByListOfCommunityId?collectionCommunityId={collectionCommunityId}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getMoreByListOfCommunityId")]
    public async Task<IActionResult> GetMoreByListOfCommunityIds(string collectionCommunityId, int offset, int pageSize)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getMoreByListOfCommunityId?collectionCommunityId={collectionCommunityId}&offset={offset}&pageSize={pageSize}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpGet("getNewByListOfCommunityId")]
    public async Task<IActionResult> GetNewByListOfCommunityId(string collectionCommunityId, string checkFrom)
    {
        var responseMessage = await _httpClient.GetAsync($"CommunityPost/getNewByListOfCommunityId?collectionCommunityId={collectionCommunityId}&checkFrom={checkFrom}");
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var posts = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CommunityPostModel>>();

            return Ok(posts);
        }

        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PostAsync("CommunityPost", JsonContent.Create(model));
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        else if (responseMessage.IsSuccessStatusCode)
        {
            var communityPost = await responseMessage.Content.ReadFromJsonAsync<CommunityPostModel>();

            return Ok(communityPost);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Update(CommunityPostModel model)
    {
        var responseMessage = await _httpClient.PutAsync("CommunityPost", JsonContent.Create(model));
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
        var responseMessage = await _httpClient.DeletAsync($"CommunityPost/{id}");
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
