import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../../user/api/Account.api';
import { useRemoveCommunityDiscussionCommentAsyncMutation } from '../../../api/CommunityDiscussionComment.api';
import type { CommunityDiscussionCommentModel } from '../../../types/CommunityDiscussionCommentModel';

interface DiscussionCommentTitleProps {
    myselfId: string;
    comment: CommunityDiscussionCommentModel;
    dateFormatting: (stringOfDate: string) => string;
}

const DiscussionCommentTitle: React.FC<DiscussionCommentTitleProps> = ({ myselfId, comment, dateFormatting }) => {
    const { t } = useTranslation("communication/community/discussion");

    const { data: user, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const [removeDiscussionCommentAsyncMut] = useRemoveCommunityDiscussionCommentAsyncMutation();

    const deletePostCommentAsync = async (discussionCommentId: number) => {
        await removeDiscussionCommentAsyncMut(discussionCommentId);
    }

    if (isLoading || !user) {
        return (<></>);
    }

    return (
        <div className="post-comments__title">
            <div className="user">
                <div className="username">{user.username}</div>
                <div className="when">{dateFormatting(comment.when.toString())}</div>
            </div>
            {comment?.appUserId === myselfId &&
                <div className="post-comments__menu">
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("Remove")}
                        onClick={async () => await deletePostCommentAsync(comment.id)}
                    />
                </div>
            }
        </div>
    );
}

export default DiscussionCommentTitle;