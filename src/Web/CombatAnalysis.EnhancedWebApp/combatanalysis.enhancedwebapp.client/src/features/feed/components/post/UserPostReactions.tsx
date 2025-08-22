import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { SetStateAction } from 'react';
import { useCreateUserPostDislikeMutation, useLazySearchUserPostDislikeByPostIdQuery, useRemoveUserPostDislikeMutation } from '../../api/UserPostDislike.api';
import { useCreateUserPostLikeMutation, useLazySearchUserPostLikeByPostIdQuery, useRemoveUserPostLikeMutation } from '../../api/UserPostLike.api';
import type { UserPostModel } from '../../types/UserPostModel';
import type { UserPostReactionModel } from '../../types/UserPostReactionModel';

interface UserPostReactionsProps {
    userId: string;
    post: UserPostModel;
    updatePostAsync: (postId: number, likesCount: number, dislikesCount: number, commentsCount: number) => Promise<void>;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    t: (key: string) => string;
}

const UserPostReactions: React.FC<UserPostReactionsProps> = ({ userId, post, updatePostAsync, setShowComments, showComments, t }) => {
    const [createPostLike] = useCreateUserPostLikeMutation();
    const [removePostLike] = useRemoveUserPostLikeMutation();
    const [searchPostLikeByPostId] = useLazySearchUserPostLikeByPostIdQuery();
    const [createPostDislike] = useCreateUserPostDislikeMutation();
    const [removePostDislike] = useRemoveUserPostDislikeMutation();
    const [searchPostDislikeByPostId] = useLazySearchUserPostDislikeByPostIdQuery();

    const checkPostLikesAsync = async (postId: number) => {
        const postLikes = await searchPostLikeByPostId(postId);
        if (postLikes.data) {
            return await removePostLikeIfExistAsync(postLikes.data);
        }

        return false;
    }

    const checkPostDislikesAsync = async (postId: number) => {
        const postDislikes = await searchPostDislikeByPostId(postId);
        if (postDislikes.data) {
            return await removePostDislikeIfExistAsync(postDislikes.data);
        }

        return false;
    }

    const removePostLikeIfExistAsync = async (postLikes: UserPostReactionModel[]) => {
        for (let i = 0; i < postLikes.length; i++) {
            if (postLikes[i].appUserId === userId) {
                await removePostLike(postLikes[i].id);
                return true;
            }
        }

        return false;
    }

    const removePostDislikeIfExistAsync = async (postDislikes: UserPostReactionModel[]) => {
        for (let i = 0; i < postDislikes.length; i++) {
            if (postDislikes[i].appUserId === userId) {
                await removePostDislike(postDislikes[i].id);
                return true;
            }
        }

        return false;
    }

    const createPostLikeAsync = async () => {
        const postLikeIsExist = await checkPostLikesAsync(post?.id);
        if (postLikeIsExist) {
            await updatePostAsync(post?.id, -1, 0, 0);
            return;
        }

        const postDislikeIsExist = await checkPostDislikesAsync(post?.id)

        const newPostLike: UserPostReactionModel = {
            id: 0,
            createdAt: new Date(),
            userPostId: post?.id,
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
    }

    const createPostDislikeAsync = async () => {
        const postDislikeIsExist = await checkPostDislikesAsync(post?.id);
        if (postDislikeIsExist) {
            await updatePostAsync(post?.id, 0, -1, 0);
            return;
        }

        const postLikeIsExist = await checkPostLikesAsync(post?.id);

        const newPostDislike: UserPostReactionModel = {
            id: 0,
            createdAt: new Date(),
            userPostId: post?.id,
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
    }

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    return (
        <div className="posts__reactions">
            <div className="container">
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
            </div>
        </div>
    );
}

export default UserPostReactions;