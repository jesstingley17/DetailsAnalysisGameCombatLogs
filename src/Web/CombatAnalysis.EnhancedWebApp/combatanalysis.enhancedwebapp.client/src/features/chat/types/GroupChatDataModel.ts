import type { GroupChatMessageModel } from './GroupChatMessageModel';
import type { GroupChatUserModel } from './GroupChatUserModel';

export type GroupChatDataModel = {
    messages: GroupChatMessageModel[] | undefined;
    count: number;
    IasGroupChatUser: GroupChatUserModel;
    groupChatUsers: GroupChatUserModel[];
    isLoading: boolean;
}