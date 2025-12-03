using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityPostDislikeTestDataFactory
{
    public static CommunityPostDislike Create(int id = 1, int communityId = 1)
    {
        var entity = new CommunityPostDislike
        {
            Id = id,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static CommunityPostDislikeDto CreateDto(int id = 1, int communityId = 1)
    {
        var entityDto = new CommunityPostDislikeDto
        {
            Id = id,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<CommunityPostDislike> CreateCollection()
    {
        var collection = new List<CommunityPostDislike>
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

    public static List<CommunityPostDislikeDto> CreateDtoCollection()
    {
        var collection = new List<CommunityPostDislikeDto>
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
