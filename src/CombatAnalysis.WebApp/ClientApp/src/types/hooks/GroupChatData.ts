import { GroupChatMessage } from '../GroupChatMessage';
import { GroupChatUser } from '../GroupChatUser';

export interface GroupChatData {
    messages: GroupChatMessage[] | undefined;
    count: number;
    meInChat: GroupChatUser;
    groupChatUsers: GroupChatUser[];
    isLoading: boolean;
}