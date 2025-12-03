import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import CommunityPost from '../../../feed/components/post/CommunityPost';
import CreateUserPost from '../../../feed/components/post/CreateUserPost';
import UserPost from '../../../feed/components/post/UserPost';
import useFetchPosts from '../../../feed/hooks/useFetchPosts';
import type { CommunityPostModel } from '../../../feed/types/CommunityPostModel';
import type { PostModel } from '../../../feed/types/PostModel';
import type { UserPostModel } from '../../../feed/types/UserPostModel';

const UserFeed: React.FC = () => {
    const { t } = useTranslation("communication/feed");

    const myself = useSelector((state: RootState) => state.user.value);

    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState<PostModel[]>([]);

    const { userPosts, communityPosts, count, communityCount, getMoreUserPostsAsync, getMoreCommunityPostsAsync } = useFetchPosts(myself?.id ?? "");

    useEffect(() => {
        if (!userPosts) {
            return;
        }

        userPostsSizeRef.current = userPosts.length;

        const totalPosts = [...currentPosts, ...userPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }, [userPosts]);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        communityPostsSizeRef.current = communityPosts.length;

        const totalPosts = [...currentPosts, ...communityPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }, [communityPosts]);

    const loadingMoreUserPostsAsync = async () => {
        const newUserPosts = await getMoreUserPostsAsync(userPostsSizeRef.current);
        const newCommunityPosts = await getMoreCommunityPostsAsync(communityPostsSizeRef.current);

        const newPosts = newUserPosts.concat(newCommunityPosts);

        const totalPosts = [...currentPosts, ...newPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }

    if (!myself) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={5}
                    hasSubMenu={true}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div>
                <CreateUserPost
                    user={myself}
                    owner={myself.username}
                    t={t}
                />
                <ul className="posts">
                    {currentPosts?.map(post => (
                        <li className="posts__item" key={post.id}>
                            {(post as CommunityPostModel).communityId !== undefined
                                ? <CommunityPost
                                    userId={myself.id}
                                    communityId={(post as CommunityPostModel).communityId}
                                    post={(post as CommunityPostModel)}
                                />
                                : <UserPost
                                    myself={myself}
                                    post={(post as UserPostModel)}
                                />
                            }
                        </li>
                    ))}
                    {(currentPosts.length < (count + communityCount) && currentPosts.length > 0) &&
                        <li className="load-more" onClick={loadingMoreUserPostsAsync}>
                            <div className="load-more__content">Load more</div>
                        </li>
                    }
                </ul>
            </div>
            <CommunicationMenu
                currentMenuItem={5}
                hasSubMenu={true}
            />
        </>
    );
}

export default UserFeed;