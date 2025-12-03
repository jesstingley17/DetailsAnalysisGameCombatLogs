using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityPostLikeTestDataFactory
{
    public static CommunityPostLike Create(int id = 1, int communityId = 1)
    {
        var entity = new CommunityPostLike
        {
            Id = id,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static CommunityPostLikeDto CreateDto(int id = 1, int communityId = 1)
    {
        var entityDto = new CommunityPostLikeDto
        {
            Id = id,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<CommunityPostLike> CreateCollection()
    {
        var collection = new List<CommunityPostLike>
        {
            new () {
                Id = 1,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }

    public static List<CommunityPostLikeDto> CreateDtoCollection()
    {
        var collection = new List<CommunityPostLikeDto>
        {
            new () {
                Id = 1,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }
}
