import { useFindGroupChatUserByAppUserIdQuery, useFindGroupChatUsersByChatIdQuery } from '../api/GroupChatUser.api';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

interface UseGroupChatDataResult {
    IasGroupChatUser: GroupChatUserModel | undefined;
    groupChatUsers: GroupChatUserModel[] | undefined;
}

const useGroupChatData = (chatId: number, appUserId: string): UseGroupChatDataResult => {
    const { data: IasGroupChatUser } = useFindGroupChatUserByAppUserIdQuery({ chatId, appUserId });
    const { data: groupChatUsers } = useFindGroupChatUsersByChatIdQuery(chatId);

    return { IasGroupChatUser, groupChatUsers };
}

export default useGroupChatData;