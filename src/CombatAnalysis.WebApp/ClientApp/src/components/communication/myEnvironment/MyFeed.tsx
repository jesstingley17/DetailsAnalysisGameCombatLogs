import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { v4 as uuidv4 } from 'uuid';
import useFetchCommunityPosts from '../../../hooks/useFetchUsersPosts';
import { Post } from '../../../types/Post';
import { CommunityPost as CommunityPostModel } from '../../../types/CommunityPost';
import { UserPost as UserPostModel } from '../../../types/UserPost';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import CommunityPost from '../post/CommunityPost';
import CreateUserPost from '../post/CreateUserPost';
import UserPost from '../post/UserPost';

const MyFeed: React.FC = () => {
    const { t } = useTranslation("communication/feed");

    const myself = useSelector((state: any) => state.user.value);

    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState<Post[]>([]);

    const { posts, communityPosts, count, communityCount, isLoading, getMoreUserPostsAsync, getMoreCommunityPostsAsync } = useFetchCommunityPosts(myself?.id);

    useEffect(() => {
        if (!posts) {
            return;
        }

        userPostsSizeRef.current = posts.length;

        const totalPosts = [...currentPosts, ...posts];
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }, [posts]);

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

    if (isLoading) {
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
                    owner={myself?.username}
                    t={t}
                />
                <ul className="posts">
                    {currentPosts?.map(post => (
                        <li className="posts__item" key={uuidv4()}>
                            {(post as CommunityPostModel).communityId !== undefined
                                ? <CommunityPost
                                    userId={myself?.id}
                                    post={(post as CommunityPostModel)}
                                    communityId={(post as CommunityPostModel).communityId}
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

export default MyFeed;