import { GroupChatMessage } from '../GroupChatMessage';
import { GroupChatData } from './GroupChatData';

export interface UseGroupChatDataResult {
    groupChatData: GroupChatData;
    getMoreMessagesAsync(offset: number): Promise<GroupChatMessage[]>;
}