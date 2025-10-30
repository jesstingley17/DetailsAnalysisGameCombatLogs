using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityService(ICommunityRepository communityRepository, ISqlContextService sqlContextService,
    IService<InviteToCommunityDto, int> inviteToCommunityService, IService<CommunityUserDto, string> communityUserService,
    ICommunityPostService postService, IService<CommunityPostCommentDto, int> postCommentService,
    IService<CommunityPostLikeDto, int> postLikeService, IService<CommunityPostDislikeDto, int> postDislikeService,
    IService<CommunityDiscussionDto, int> communityDiscussionService, IMapper mapper) : ICommunityService
{
    private readonly ICommunityRepository _repository = communityRepository;
    private readonly IService<CommunityUserDto, string> _communityUserService = communityUserService;
    private readonly IService<InviteToCommunityDto, int> _inviteToCommunityService = inviteToCommunityService;
    private readonly ICommunityPostService _postService = postService;
    private readonly IService<CommunityPostCommentDto, int> _postCommentService = postCommentService;
    private readonly IService<CommunityPostLikeDto, int> _postLikeService = postLikeService;
    private readonly IService<CommunityPostDislikeDto, int> _postDislikeService = postDislikeService;
    private readonly IService<CommunityDiscussionDto, int> _communityDiscussionService = communityDiscussionService;
    private readonly ISqlContextService _sqlContextService = sqlContextService;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDto?> CreateAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunicationDAL.Entities.Community.Community>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync(true);
        try
        {
            await DeleteCommunityPostsAsync(id);
            await DeleteCommunityDiscussionsAsync(id);
            await DeleteInvitesToCommunityAsync(id);
            await DeleteCommunityUsersAsync(id);

            transaction.CreateSavepoint("BeforeDeleteCommunity");

            await _repository.DeleteAsync(id);

            await transaction.CommitAsync();
        }
        catch (ArgumentException)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteCommunity");
        }
        catch (Exception)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteCommunity");
        }
    }

    public async Task<IEnumerable<CommunityDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDto>>(allData);

        return result;
    }

    public async Task<CommunityDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDto>> GetByParamAsync<TValue>(Expression<Func<CommunityDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunicationDAL.Entities.Community.Community, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunicationDAL.Entities.Community.Community>(item);
        await _repository.UpdateAsync(map);
    }

    public async Task<IEnumerable<CommunityDto>> GetAllWithPaginationAsync(int pageSize)
    {
        var result = await _repository.GetAllWithPaginationAsync(pageSize);
        var map = _mapper.Map<IEnumerable<CommunityDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityDto>> GetMoreWithPaginationAsync(int offset, int pageSize)
    {
        var result = await _repository.GetMoreWithPaginationAsync(offset, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityDto>>(result);

        return map;
    }

    public async Task<int> CountAsync()
    {
        var result = await _repository.CountAsync();

        return result;
    }

    private async Task DeleteCommunityPostsAsync(int communityId)
    {
        var posts = await _postService.GetByParamAsync(c => c.CommunityId, communityId);
        foreach (var item in posts)
        {
            await DeleteCommunityPostCommentsAsync(item.Id);
            await DeleteCommunityPostLikesAsync(item.Id);
            await DeleteCommunityPostDislikesAsync(item.Id);

            await _postService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteCommunityPostCommentsAsync(int communityPostId)
    {
        var postComments = await _postCommentService.GetByParamAsync(c => c.CommunityPostId, communityPostId);
        foreach (var item in postComments)
        {
            await _postService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteCommunityPostLikesAsync(int communityPostId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(c => c.CommunityPostId, communityPostId);
        foreach (var item in postLikes)
        {
            await _postService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteCommunityPostDislikesAsync(int communityPostId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(c => c.CommunityPostId, communityPostId);
        foreach (var item in postDislikes)
        {
            await _postService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteCommunityDiscussionsAsync(int communityId)
    {
        var communityDiscussions = await _communityDiscussionService.GetByParamAsync(c => c.CommunityId, communityId);
        foreach (var item in communityDiscussions)
        {
            await _communityDiscussionService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteInvitesToCommunityAsync(int communityId)
    {
        var invitesToCommunity = await _inviteToCommunityService.GetByParamAsync(c => c.CommunityId, communityId);
        foreach (var item in invitesToCommunity)
        {
            await _inviteToCommunityService.DeleteAsync(item.Id);
        }
    }

    private async Task DeleteCommunityUsersAsync(int communityId)
    {
        var communityUsers = await _communityUserService.GetByParamAsync(c => c.CommunityId, communityId);
        foreach (var item in communityUsers)
        {
            await _communityUserService.DeleteAsync(item.Id);
        }
    }
}
