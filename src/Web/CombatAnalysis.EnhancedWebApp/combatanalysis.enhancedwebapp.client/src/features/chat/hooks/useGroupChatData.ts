import { type RefObject } from 'react';
import { useGetMessagesByGroupChatIdQuery } from '../api/Chat.api';
import { useGetGroupChatMessageCountByChatIdQuery } from '../api/GroupChatMessage.api';
import { useFindGroupChatUserByAppUserIdQuery, useFindGroupChatUsersByChatIdQuery } from '../api/GroupChatUser.api';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

interface UseGroupChatDataResult {
    messages: GroupChatMessageModel[] | undefined;
    count: number | undefined;
    IasGroupChatUser: GroupChatUserModel | undefined;
    groupChatUsers: GroupChatUserModel[] | undefined;
}

const useGroupChatData = (chatId: number, appUserId: string, page: number, pageSizeRef: RefObject<number>): UseGroupChatDataResult => {
    const { data: messages } = useGetMessagesByGroupChatIdQuery({ chatId, page, pageSize: pageSizeRef.current });
    const { data: count } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: IasGroupChatUser } = useFindGroupChatUserByAppUserIdQuery({ chatId, appUserId });
    const { data: groupChatUsers } = useFindGroupChatUsersByChatIdQuery(chatId);

    return { messages, count, IasGroupChatUser, groupChatUsers };
}

export default useGroupChatData;