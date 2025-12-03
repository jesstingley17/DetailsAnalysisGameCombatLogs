import type { RootState } from '@/app/Store';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useCreateCommunityPostDislikeMutation, useLazySearchCommunityPostDislikeByPostIdQuery } from '../../api/CommunityPostDislike.api';
import { useCreateCommunityPostLikeMutation, useLazySearchCommunityPostLikeByPostIdQuery } from '../../api/CommunityPostLike.api';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import type { CommunityPostReactionModel } from '../../types/CommunityPostReactionModel';
import logger from '@/utils/Logger';

interface CommunityPostReactionsProps {
    userId: string;
    communityId: number;
    post: CommunityPostModel;
    updatePostAsync: (postId: number, likesCount: number, dislikesCount: number, commentsCount: number) => Promise<void>;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    t: (key: string) => string;
}

const CommunityPostReactions: React.FC<CommunityPostReactionsProps> = ({ userId, communityId, post, updatePostAsync, setShowComments, showComments, t }) => {
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [createPostLike] = useCreateCommunityPostLikeMutation();
    //const [removePostLike] = useRemoveCommunityPostLikeMutation();
    const [searchPostLikeByPostId] = useLazySearchCommunityPostLikeByPostIdQuery();
    const [createPostDislike] = useCreateCommunityPostDislikeMutation();
    //const [removePostDislike] = useRemoveCommunityPostDislikeMutation();
    const [searchPostDislikeByPostId] = useLazySearchCommunityPostDislikeByPostIdQuery();

    const getPostLikesAsync = async (postId: number) => {
        try {
            const postLikes = await searchPostLikeByPostId(postId).unwrap();
            //return await removePostLikeIfExistAsync(postLikes);

            return postLikes.length > 0;
        } catch (e) {
            logger.error("Failed to receive community post likes", e);

            return false;
        }
    }

    const getPostDislikesAsync = async (postId: number) => {
        try {
            const postDislikes = await searchPostDislikeByPostId(postId).unwrap();
            //return await removePostDislikeIfExistAsync(postDislikes);

            return postDislikes.length > 0;
        } catch (e) {
            logger.error("Failed to receive community post dislikes", e);

            return false;
        }
    }

    //const removePostLikeIfExistAsync = async (postLikes: CommunityPostReactionModel[]) => {
    //    try {
    //        for (let i = 0; i < postLikes.length; i++) {
    //            if (postLikes[i].appUserId === userId) {
    //                await removePostLike(postLikes[i].id);
    //                return true;
    //            }
    //        }

    //        return false;
    //    } catch (e) {
    //        logger.error("Failed to remove likes if already exist for current user", e);

    //        return false;
    //    }
    //}

    //const removePostDislikeIfExistAsync = async (postDislikes: CommunityPostReactionModel[]) => {
    //    try {
    //        for (let i = 0; i < postDislikes.length; i++) {
    //            if (postDislikes[i].appUserId === userId) {
    //                await removePostDislike(postDislikes[i].id);
    //                return true;
    //            }
    //        }

    //        return false;
    //    } catch (e) {
    //        logger.error("Failed to remove dislikes if already exist for current user", e);

    //        return false;
    //    }
    //}

    const createPostLikeAsync = useCallback(async () => {
        const postLikeIsExist = await getPostLikesAsync(post?.id);
        if (postLikeIsExist) {
            await updatePostAsync(post?.id, -1, 0, 0);
            return;
        }

        const postDislikeIsExist = await getPostDislikesAsync(post?.id)

        const newPostLike: CommunityPostReactionModel = {
            id: 0,
            createdAt: new Date(),
            communityPostId: post?.id,
            communityId: communityId,
            appUserId: userId
        }

        const createdPostLike = await createPostLike(newPostLike);
        if (createdPostLike.data) {
            if (postDislikeIsExist) {
                await updatePostAsync(post?.id, 1, -1, 0);
            }
            else {
                await updatePostAsync(post?.id, 1, 0, 0);
            }
        }
    }, [post]);

    const createPostDislikeAsync = useCallback(async () => {
        const postDislikeIsExist = await getPostDislikesAsync(post?.id);
        if (postDislikeIsExist) {
            await updatePostAsync(post?.id, 0, -1, 0);
            return;
        }

        const postLikeIsExist = await getPostLikesAsync(post?.id);

        const newPostDislike: CommunityPostReactionModel = {
            id: 0,
            createdAt: new Date(),
            communityPostId: post?.id,
            communityId: communityId,
            appUserId: userId
        }

        const createdPostDislike = await createPostDislike(newPostDislike);
        if (createdPostDislike.data) {
            if (postLikeIsExist) {
                await updatePostAsync(post?.id, -1, 1, 0);
            }
            else {
                await updatePostAsync(post?.id, 0, 1, 0);
            }
        }
    }, [post]);

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    return (
        <div className="posts__reactions">
            <div className="container">
                {userPrivacy?.emailVerified
                    ? <>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__like"
                                icon={faHeart}
                                title={t("Like")}
                                onClick={createPostLikeAsync}
                            />
                            <div className="count">{post?.likeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__dislike"
                                icon={faThumbsDown}
                                title={t("Dislike")}
                                onClick={createPostDislikeAsync}
                            />
                            <div className="count">{post?.dislikeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className={`item__comment${showComments ? '_active' : ''}`}
                                icon={faMessage}
                                title={t("Comment")}
                                onClick={postCommentsHandler}
                            />
                            <div className="count">{post?.commentCount}</div>
                        </div>
                    </>
                    : <VerificationRestriction
                        contentText={t("ReactionsForbidden")}
                        infoText={t("VerificationReactions")}
                    />
                }
            </div>
        </div>
    );
}

export default CommunityPostReactions;