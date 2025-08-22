import { useEffect, useState, type RefObject } from 'react';
import { useLazyGetMessagesByGroupChatIdQuery, useLazyGetMoreMessagesByGroupChatIdQuery } from '../api/Chat.api';
import { useGetGroupChatMessageCountByChatIdQuery } from '../api/GroupChatMessage.api';
import { useFindGroupChatUsersByChatIdQuery, useFindMeInChatQuery } from '../api/GroupChatUser.api';
import type { GroupChatDataModel } from '../types/GroupChatDataModel';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';

interface UseGroupChatDataResult {
    groupChatData: GroupChatDataModel | null;
    getMoreMessagesAsync: (offset: number) => Promise<GroupChatMessageModel[]>;
}

const useGroupChatData = (chatId: number, appUserId: string, pageSizeRef: RefObject<number>): UseGroupChatDataResult => {
    const [groupChatData, setGroupChatData] = useState<GroupChatDataModel | null>(null);
    const [chatMessages, setChatMessages] = useState<GroupChatMessageModel[]>([]);

    const [getMoreMessagesByGroupChatIdAsync] = useLazyGetMoreMessagesByGroupChatIdQuery();
    const [getMessagesByGroupChatIdAsync] = useLazyGetMessagesByGroupChatIdQuery();
    const { data: count, isLoading: countIsLoading } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: IasGroupChatUser, isLoading: findMeInChatLoading } = useFindMeInChatQuery({ chatId, appUserId });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUsersByChatIdQuery(chatId);

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
        if (!countIsLoading && !findMeInChatLoading && !usersIsLoading && groupChatUsers) {
            setGroupChatData({
                messages: chatMessages,
                count: count ?? 0,
                IasGroupChatUser,
                groupChatUsers,
                isLoading: false,
            });
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

    const getMoreMessagesAsync = async (offset: number): Promise<GroupChatMessageModel[]> => {
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