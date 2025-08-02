import { GroupChatMessage } from '../GroupChatMessage';
import { GroupChatUser } from '../GroupChatUser';

export type GroupChatData = {
    messages: GroupChatMessage[] | undefined;
    count: number;
    IasGroupChatUser: GroupChatUser;
    groupChatUsers: GroupChatUser[];
    isLoading: boolean;
}