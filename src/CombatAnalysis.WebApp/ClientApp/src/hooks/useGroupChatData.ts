import { useEffect, useState } from 'react';
import { useGetGroupChatMessageCountByChatIdQuery } from '../store/api/chat/GroupChatMessage.api';
import { useFindGroupChatUserByChatIdQuery, useFindMeInChatQuery } from '../store/api/chat/GroupChatUser.api';
import { useLazyGetMessagesByGroupChatIdQuery, useLazyGetMoreMessagesByGroupChatIdQuery } from '../store/api/core/Chat.api';
import { GroupChatMessage } from '../types/GroupChatMessage';
import { GroupChatData } from '../types/hooks/GroupChatData';
import { UseGroupChatDataResult } from '../types/hooks/UseGroupChatDataResult';

const useGroupChatData = (chatId: number, appUserId: string, pageSizeRef: any): UseGroupChatDataResult => {
    const [groupChatData, setGroupChatData] = useState<GroupChatData>({
        messages: [],
        count: 0,
        IasGroupChatUser: { id: "", appUserId: "", chatId: 0, username: "", unreadMessages: 0 },
        groupChatUsers: [],
        isLoading: true,
    });
    const [chatMessages, setChatMessages] = useState<GroupChatMessage[]>([]);

    const [getMoreMessagesByGroupChatIdAsync] = useLazyGetMoreMessagesByGroupChatIdQuery();
    const [getMessagesByGroupChatIdAsync] = useLazyGetMessagesByGroupChatIdQuery();
    const { data: count, isLoading: countIsLoading } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: IasGroupChatUser, isLoading: findMeInChatLoading } = useFindMeInChatQuery({ chatId, appUserId });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chatId);

    useEffect(() => {
        if (!IasGroupChatUser) {
            return;
        }

        const getMessages = async () => {
            await getMessagesAsync();
        }

        getMessages();
    }, [IasGroupChatUser]);

    useEffect(() => {
        if (!countIsLoading && !findMeInChatLoading && !usersIsLoading) {
            setGroupChatData(() => ({
                messages: chatMessages,
                count,
                IasGroupChatUser,
                groupChatUsers,
                isLoading: false,
            }));
        }
    }, [chatMessages, count, IasGroupChatUser, groupChatUsers, countIsLoading, findMeInChatLoading, usersIsLoading]);

    const getMessagesAsync = async () => {
        const arg = {
            chatId,
            groupChatUserId: IasGroupChatUser.id,
            pageSize: pageSizeRef.current
        };

        const messages = await getMessagesByGroupChatIdAsync(arg).unwrap();
        setChatMessages(messages);
    }

    const getMoreMessagesAsync = async (offset: number): Promise<GroupChatMessage[]> => {
        const arg = {
            chatId,
            groupChatUserId: IasGroupChatUser.id,
            offset,
            pageSize: pageSizeRef.current
        };

        const moreMessages = await getMoreMessagesByGroupChatIdAsync(arg).unwrap();

        return moreMessages;
    }

    return { groupChatData, getMoreMessagesAsync };
}

export default useGroupChatData;