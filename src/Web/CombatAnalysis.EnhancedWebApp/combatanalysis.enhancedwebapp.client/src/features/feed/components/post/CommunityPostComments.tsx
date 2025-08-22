import { useSearchCommunityPostCommentByPostIdQuery } from '../../api/CommunityPostComment.api';
import CommunityPostCommentContent from './CommunityPostCommentContent';
import CommunityPostCommentTitle from './CommunityPostCommentTitle';

import './PostComments.scss';

interface CommunityPostCommentsProps {
    userId: string;
    postId: number;
    dateFormatting: (stringOfDate: string) => string;
    updatePostAsync: (postId: number, likesCount: number, dislikesCount: number, commentsCount: number) => Promise<void>;
}

const CommunityPostComments: React.FC<CommunityPostCommentsProps> = ({ userId, postId, dateFormatting, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchCommunityPostCommentByPostIdQuery(postId);

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
                    <CommunityPostCommentTitle
                        userId={userId}
                        comment={comment}
                        dateFormatting={dateFormatting}
                        postId={postId}
                        updatePostAsync={updatePostAsync}
                    />
                    <CommunityPostCommentContent
                        userId={userId}
                        comment={comment}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default CommunityPostComments;