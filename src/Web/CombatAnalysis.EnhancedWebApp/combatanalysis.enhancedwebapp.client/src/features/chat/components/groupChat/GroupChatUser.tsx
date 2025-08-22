import { useGetUserByIdQuery } from '../../../user/api/Account.api';

const GroupChatUser: React.FC<{ userId: string }> = ({ userId }) => {
    const { data: user, isLoading } = useGetUserByIdQuery(userId);

    if (isLoading) {
        return (<></>);
    }

    return (
        <div className="group-chat-user">
            {user?.username}
        </div>
    );
}

export default GroupChatUser;