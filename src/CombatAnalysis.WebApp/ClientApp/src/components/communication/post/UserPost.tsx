import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetUserPostByIdQuery, useUpdateUserPostMutation } from '../../../store/api/post/UserPost.api';
import { useCreateUserPostCommentMutation } from '../../../store/api/post/UserPostComment.api';
import { UserPostProps } from "../../../types/components/communication/post/UserPostProps";
import UserPostComments from './UserPostComments';
import UserPostReactions from './UserPostReactions';
import UserPostTitle from './UserPostTitle';

import '../../../styles/communication/post.scss';

const UserPost: React.FC<UserPostProps> = ({ myself, post }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateUserPostMutation();

    const [createPostComment] = useCreateUserPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetUserPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    useEffect(() => {
        setIsMyPost(post?.appUserId === myself.id);
    }, [post]);

    const updatePostAsync = async (postId: number, likesCount: number, dislikesCount: number, commentsCount: number) => {
        try {
            const userPost = await getPostByIdAsync(postId).unwrap();

            const postForUpdate = {
                ...userPost,
                likeCount: userPost.likeCount + likesCount,
                dislikeCount: userPost.dislikeCount + dislikesCount,
                commentCount: userPost.commentCount + commentsCount
            };

            await updatePost(postForUpdate).unwrap();
        } catch (e) {
            console.error("Failed to update post:", e);

            return post;
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            createdAt: new Date(),
            userPostId: post.id,
            appUserId: myself
        }

        const response = await createPostComment(newPostComment);
        if (response.data) {
            setPostCommentContent("");

            await updatePostAsync(post.id, 0, 0, 1);
        }
    }

    const dateFormatting = (stringOfDate: string) => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        const monthes: any = {
            0: "January",
            1: "February",
            2: "March",
            3: "April",
            4: "May",
            5: "June",
            6: "July",
            7: "August",
            8: "September",
            9: "October",
            10: "November",
            11: "December"
        };

        const formatted = `${date.getDate()} ${monthes[month]}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    return (
        <>
            <div className="posts__card">
                <UserPostTitle
                    myself={myself}
                    post={post}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <UserPostReactions
                    userId={myself}
                    post={post}
                    updatePostAsync={updatePostAsync}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <UserPostComments
                        dateFormatting={dateFormatting}
                        userId={myself}
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

export default memo(UserPost);