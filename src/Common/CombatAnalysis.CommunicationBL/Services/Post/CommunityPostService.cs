using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostService(ICommunityPostRepository repository, IMapper mapper,
    IService<CommunityPostLikeDto, int> postLikeService, IService<CommunityPostDislikeDto, int> postDislikeService,
    IService<CommunityPostCommentDto, int> postCommentService, ISqlContextService sqlContextService) : ICommunityPostService
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IService<CommunityPostLikeDto, int> _postLikeService = postLikeService;
    private readonly IService<CommunityPostDislikeDto, int> _postDislikeService = postDislikeService;
    private readonly IService<CommunityPostCommentDto, int> _postCommentService = postCommentService;
    private readonly ISqlContextService _sqlContextService = sqlContextService;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityPostDto?> CreateAsync(CommunityPostDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CommunityPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, CommunityPostDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(id, item.Id);

        CheckParams(item);

        var map = _mapper.Map<CommunityPost>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var transaction = await _sqlContextService.UseTransactionAsync();
        try
        {
            await DeletePostLikesAsync(id);
            await DeletePostDislikesAsync(id);
            await DeletePostComentsAsync(id);
            transaction.CreateSavepoint("BeforeDeletePost");

            await _repository.DeleteAsync(id);

            await transaction.CommitAsync();
        }
        catch (ArgumentException)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePost");
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePost");
        }
    }

    public async Task<IEnumerable<CommunityPostDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostDto>>(allData);

        return result;
    }

    public async Task<CommunityPostDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetByParamAsync<TValue>(Expression<Func<CommunityPostDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityPost, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetByCommunityIdAsync(int communityId, int pageSize = 100)
    {
        var result = await _repository.GetByCommunityIdAsync(communityId, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetMoreByCommunityIdAsync(int communityId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByCommunityIdAsync(communityId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetNewByCommunityIdAsync(int communityId, DateTimeOffset checkFrom)
    {
        var result = await _repository.GetNewByCommunityIdAsync(communityId, checkFrom);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetByListOfCommunityIdAsync(string communityIds, int pageSize = 100)
    {
        var result = await _repository.GetByListOfCommunityIdAsync(communityIds, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetMoreByListOfCommunityIdAsync(string communityIds, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByListOfCommunityIdAsync(communityIds, offset, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetNewByListOfCommunityIdAsync(string communityIds, DateTimeOffset checkFrom)
    {
        var result = await _repository.GetNewByListOfCommunityIdAsync(communityIds, checkFrom);
        var map = _mapper.Map<List<CommunityPostDto>>(result);

        return map;
    }

    public async Task<int> CountByCommunityIdAsync(int communityId)
    {
        var count = await _repository.CountByCommunityIdAsync(communityId);

        return count;
    }

    public async Task<int> CountByListOfCommunityIdAsync(int[] communityIds)
    {
        var count = await _repository.CountByListOfCommunityIdAsync(communityIds);

        return count;
    }

    private async Task DeletePostLikesAsync(int postId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(c => c.CommunityPostId, postId);
        foreach (var item in postLikes)
        {
            await _postLikeService.DeleteAsync(item.Id);
        }
    }

    private async Task DeletePostDislikesAsync(int postId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(c => c.CommunityPostId, postId);
        foreach (var item in postDislikes)
        {
            await _postDislikeService.DeleteAsync(item.Id);
        }
    }

    private async Task DeletePostComentsAsync(int postId)
    {
        var postComments = await _postCommentService.GetByParamAsync(c => c.CommunityPostId, postId);
        foreach (var item in postComments)
        {
            await _postCommentService.DeleteAsync(item.Id);
        }
    }

    private static void CheckParams(CommunityPostDto item)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(item.PostType, nameof(item.PostType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.PublicType, nameof(item.PublicType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.Restrictions, nameof(item.Restrictions));
        ArgumentOutOfRangeException.ThrowIfNegative(item.LikeCount, nameof(item.LikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DislikeCount, nameof(item.DislikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CommentCount, nameof(item.CommentCount));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(item.CommunityId, nameof(item.CommunityId));

        ArgumentException.ThrowIfNullOrEmpty(item.CommunityName, nameof(item.CommunityName));
        ArgumentException.ThrowIfNullOrEmpty(item.Owner, nameof(item.Owner));
        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
