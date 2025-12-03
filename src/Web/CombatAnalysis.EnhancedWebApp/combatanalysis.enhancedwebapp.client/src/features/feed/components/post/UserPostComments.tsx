import { useSearchUserPostCommentByPostIdQuery } from '../../api/UserPostComment.api';
import UserPostCommentContent from './UserPostCommentContent';
import UserPostCommentTitle from './UserPostCommentTitle';

import './PostComments.scss';

interface UserPostCommentsProps {
    userId: string;
    postId: number;
    dateFormatting: (stringOfDate: string) => string;
    updatePostAsync: (postId: number, likesCount: number, dislikesCount: number, commentsCount: number) => Promise<void>;
}

const UserPostComments: React.FC<UserPostCommentsProps> = ({ userId, postId, dateFormatting, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchUserPostCommentByPostIdQuery(postId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (postComments?.length === 0) {
        return (<></>);
    }

    return (
        <ul className="post-comments">
            {postComments?.map((comment) => (
                    <li key={comment.id} className="post-comments__card">
                        <UserPostCommentTitle
                            userId={userId}
                            comment={comment}
                            dateFormatting={dateFormatting}
                            postId={postId}
                            updatePostAsync={updatePostAsync}
                        />
                        <UserPostCommentContent
                            userId={userId}
                            comment={comment}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default UserPostComments;