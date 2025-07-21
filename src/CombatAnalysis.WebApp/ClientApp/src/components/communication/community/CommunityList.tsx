import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import { useGetCommunitiesCountQuery, useLazyGetCommunitiesWithPaginationQuery, useLazyGetMoreCommunitiesWithPaginationQuery } from '../../../store/api/core/Community.api';
import { Community } from '../../../types/Community';
import { CommunityListProps } from '../../../types/components/communication/community/CommunityListProps';
import CommunityItem from './CommunityItem';

const CommunityList: React.FC<CommunityListProps> = ({ filterContent }) => {
    const user = useSelector((state: any) => state.user.value);

    const pageSizeRef = useRef<HTMLInputElement | string>("1");

    const [communities, setCommunities] = useState<Community[]>([]);
    const [actualCount, setActualCount] = useState(0);

    const { data: count, isLoading: countIsLoading } = useGetCommunitiesCountQuery();
    const { data: userCommunities, isLoading } = useCommunityUserSearchByUserIdQuery(user?.id);

    const [getCommunities] = useLazyGetCommunitiesWithPaginationQuery();
    const [getMoreCommunities] = useLazyGetMoreCommunitiesWithPaginationQuery();

    useEffect(() => {
        if (pageSizeRef.current !== null) {
            pageSizeRef.current = process.env.REACT_APP_COMMUNITY_PAGE_SIZE || "0";
        }

        const getCommunitiesAsync = async () => {
            const response = await getCommunities(pageSizeRef.current === null ? 0 : +pageSizeRef.current).unwrap();

            setCommunities(response);
        }

        getCommunitiesAsync();
    }, []);

    useEffect(() => {
        setActualCount(count === undefined ? 0 : count);
    }, [count]);

    const anotherCommunity = (community: Community) => {
        if (user == null) {
            return true;
        }

        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    const getMoreCommunitiesAsync = async () => {
        const arg = {
            offset: +communities.length,
            pageSize: +pageSizeRef.current 
        };

        const response = await getMoreCommunities(arg).unwrap();
        setCommunities(prevCom => [...prevCom, ...response]);
    }

    if (countIsLoading || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <ul>
                {communities?.filter(community => community.policyType === 0).map((item) => (
                    (anotherCommunity(item) && item.name.toLowerCase().startsWith(filterContent.toLowerCase())) &&
                        <li key={item.id} className="community">
                            <CommunityItem
                                id={item.id}
                                me={user}
                            />
                        </li>
                    ))
                }
            </ul>
            {communities?.length < actualCount &&
                <div className="load-more" onClick={getMoreCommunitiesAsync}>
                    <div className="load-more__content">Load more</div>
                </div>
            }
        </>
    );
}

export default CommunityList;