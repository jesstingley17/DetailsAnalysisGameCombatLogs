using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostService(IUserPostRepository repository, IMapper mapper,
    IService<UserPostLikeDto, int> postLikeService, IService<UserPostDislikeDto, int> postDislikeService,
    IService<UserPostCommentDto, int> postCommentService, ISqlContextService sqlContextService) : IUserPostService
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IService<UserPostLikeDto, int> _postLikeService = postLikeService;
    private readonly IService<UserPostDislikeDto, int> _postDislikeService = postDislikeService;
    private readonly IService<UserPostCommentDto, int> _postCommentService = postCommentService;
    private readonly ISqlContextService _sqlContextService = sqlContextService;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostDto?> CreateAsync(UserPostDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(int id, UserPostDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<UserPost>(item);
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

    public async Task<IEnumerable<UserPostDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostDto>>(allData);

        return result;
    }

    public async Task<UserPostDto?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDto>> GetByParamAsync<TValue>(Expression<Func<UserPostDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<UserPost, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDto>> GetByAppUserIdAsync(string appUserId, int pageSize = 100)
    {
        var result = await _repository.GetByAppUserIdAsync(appUserId, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetMoreByAppUserIdAsync(string appUserId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByAppUserIdAsync(appUserId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetNewByAppUserIdAsync(string appUserId, DateTimeOffset checkFrom)
    {
        var result = await _repository.GetNewByAppUserIdAsync(appUserId, checkFrom);
        var map = _mapper.Map<List<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetByListOfAppUserIdAsync(string appUserIds, int pageSize = 100)
    {
        var result = await _repository.GetByListOfAppUserIdAsync(appUserIds, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetMoreByListOfAppUserIdAsync(string appUserIds, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByListOfAppUserIdAsync(appUserIds, offset, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetNewByListOfAppUserIdAsync(string appUserIds, DateTimeOffset checkFrom)
    {
        var result = await _repository.GetNewByListOfAppUserIdAsync(appUserIds, checkFrom);
        var map = _mapper.Map<List<UserPostDto>>(result);

        return map;
    }

    public async Task<int> CountByAppUserIdAsync(string appUserId)
    {
        var count = await _repository.CountByAppUserIdAsync(appUserId);

        return count;
    }

    public async Task<int> CountByListOfAppUserIdAsync(string[] appUserIds)
    {
        var count = await _repository.CountByListOfAppUserIdAsync(appUserIds);

        return count;
    }

    private async Task DeletePostLikesAsync(int postId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(c => c.UserPostId, postId);
        foreach (var item in postLikes)
        {
            await _postLikeService.DeleteAsync(item.Id);
        }
    }

    private async Task DeletePostDislikesAsync(int postId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(c => c.UserPostId, postId);
        foreach (var item in postDislikes)
        {
            await _postDislikeService.DeleteAsync(item.Id);
        }
    }

    private async Task DeletePostComentsAsync(int postId)
    {
        var postComments = await _postCommentService.GetByParamAsync(c => c.UserPostId, postId);
        foreach (var item in postComments)
        {
            await _postCommentService.DeleteAsync(item.Id);
        }
    }

    private static void CheckParams(UserPostDto item)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(item.Id, 1, nameof(item.Id));
        ArgumentOutOfRangeException.ThrowIfNegative(item.PublicType, nameof(item.PublicType));
        ArgumentOutOfRangeException.ThrowIfNegative(item.LikeCount, nameof(item.LikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(item.DislikeCount, nameof(item.DislikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(item.CommentCount, nameof(item.CommentCount));

        ArgumentException.ThrowIfNullOrEmpty(item.Tags, nameof(item.Tags));
        ArgumentException.ThrowIfNullOrEmpty(item.Owner, nameof(item.Owner));
        ArgumentException.ThrowIfNullOrEmpty(item.Content, nameof(item.Content));
        ArgumentException.ThrowIfNullOrEmpty(item.AppUserId, nameof(item.AppUserId));
    }
}
