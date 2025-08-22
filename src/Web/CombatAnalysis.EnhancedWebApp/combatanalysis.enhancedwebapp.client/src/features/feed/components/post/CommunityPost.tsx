import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazyGetCommunityPostByIdQuery, useUpdateCommunityPostMutation } from '../../api/CommunityPost.api';
import { useCreateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import CommunityPostComments from './CommunityPostComments';
import CommunityPostReactions from './CommunityPostReactions';
import CommunityPostTitle from './CommunityPostTitle';

import './Post.scss';

interface CommunityPostProps {
    userId: string;
    communityId: number;
    post: CommunityPostModel;
    dateFormatting: (stringOfDate: string) => string;
}

const CommunityPost: React.FC<CommunityPostProps> = ({ userId, communityId, post, dateFormatting }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateCommunityPostMutation();

    const [createPostComment] = useCreateCommunityPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetCommunityPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    useEffect(() => {
        setIsMyPost(post?.appUserId === userId);
    }, [post]);

    const updatePostAsync = async (postId: number, likesCount: number, dislikesCount: number, commentsCount: number): Promise<void> => {
        try {
            const communityPost = await getPostByIdAsync(postId).unwrap();

            const postForUpdate = {
                ...communityPost,
                likeCount: communityPost.likeCount + likesCount,
                dislikeCount: communityPost.dislikeCount + dislikesCount,
                commentCount: communityPost.commentCount + commentsCount
            };

            await updatePost(postForUpdate).unwrap();
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment: CommunityPostCommentModel = {
            id: 0,
            content: postCommentContent,
            commentType: 0,
            createdAt: new Date(),
            communityPostId: post.id,
            communityId: communityId,
            appUserId: userId
        }

        const response = await createPostComment(newPostComment);
        if (response.data) {
            setPostCommentContent("");

            await updatePostAsync(post.id, 0, 0, 1);
        }
    }

    return (
        <>
            <div className="posts__card">
                <CommunityPostTitle
                    post={post}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <CommunityPostReactions
                    userId={userId}
                    communityId={communityId}
                    post={post}
                    updatePostAsync={updatePostAsync}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <CommunityPostComments
                        dateFormatting={dateFormatting}
                        userId={userId}
                        postId={post.id}
                        updatePostAsync={updatePostAsync}
                    />
                    <div className="add-new-comment">
                        <div className="add-new-comment__title">
                            {showAddComment
                                ? <div>{t("AddComment")}</div>
                                : <div className="open-add-comment" onClick={() => setShowAddComment((item) => !item)}>{t("Add")}</div>
                            }
                        </div>
                        {showAddComment &&
                            <div className="add-new-comment__content">
                            <textarea className="form-control" rows={3} cols={60} onChange={e => setPostCommentContent(e.target.value)} value={postCommentContent} />
                                <div className="actions">
                                    <div className="add-comment" onClick={createPostCommentAsync}>{t("Add")}</div>
                                    <div className="hide" onClick={() => setShowAddComment((item) => !item)}>{t("Hide")}</div>
                                </div>
                            </div>
                        }
                    </div>
                </>
            }
        </>
    );
}

export default CommunityPost;