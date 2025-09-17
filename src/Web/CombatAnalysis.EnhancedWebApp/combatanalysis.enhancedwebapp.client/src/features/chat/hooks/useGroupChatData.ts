import logger from '@/utils/Logger';
import { useEffect, useState, type RefObject } from 'react';
import { useLazyGetMessagesByGroupChatIdQuery } from '../api/Chat.api';
import { useGetGroupChatMessageCountByChatIdQuery } from '../api/GroupChatMessage.api';
import { useFindGroupChatUsersByChatIdQuery, useFindGroupChatUserByAppUserIdQuery } from '../api/GroupChatUser.api';
import type { GroupChatDataModel } from '../types/GroupChatDataModel';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';

interface UseGroupChatDataResult {
    groupChatData: GroupChatDataModel | null;
    getMessagesAsync: (page: number) => Promise<GroupChatMessageModel[]>;
}

const useGroupChatData = (chatId: number, appUserId: string, pageSizeRef: RefObject<number>): UseGroupChatDataResult => {
    const [groupChatData, setGroupChatData] = useState<GroupChatDataModel | null>(null);
    const [chatMessages, setChatMessages] = useState<GroupChatMessageModel[]>([]);

    const [getMessagesByGroupChatIdAsync] = useLazyGetMessagesByGroupChatIdQuery();
    const { data: count, isLoading: countIsLoading } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: IasGroupChatUser, isLoading: findMeInChatLoading } = useFindGroupChatUserByAppUserIdQuery({ chatId, appUserId });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUsersByChatIdQuery(chatId);

    useEffect(() => {
        if (!IasGroupChatUser) {
            return;
        }

        const getMessages = async () => {
            const messages = await getMessagesAsync(1);
            setChatMessages(messages);
        }

        getMessages();
    }, [IasGroupChatUser]);

    useEffect(() => {
        if (!countIsLoading && !findMeInChatLoading && !usersIsLoading && groupChatUsers && IasGroupChatUser) {
            setGroupChatData({
                messages: chatMessages,
                count: count ?? 0,
                IasGroupChatUser,
                groupChatUsers,
                isLoading: false,
            });
        }
    }, [chatMessages, count, IasGroupChatUser, groupChatUsers, countIsLoading, findMeInChatLoading, usersIsLoading]);

    const getMessagesAsync = async (page: number): Promise<GroupChatMessageModel[]> => {
        if (!IasGroupChatUser) {
            return [];
        }

        try {
            const arg = {
                chatId,
                page,
                pageSize: pageSizeRef.current
            };

            const messages = await getMessagesByGroupChatIdAsync(arg).unwrap();

            return messages;
        } catch (e) {
            logger.error("Failed get chat messages", e);

            return [];
        }
    }

    return { groupChatData, getMessagesAsync };
}

export default useGroupChatData;