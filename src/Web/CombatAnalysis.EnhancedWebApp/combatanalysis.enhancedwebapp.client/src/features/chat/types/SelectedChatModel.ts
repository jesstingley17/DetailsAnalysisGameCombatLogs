import type { GroupChatModel } from './GroupChatModel';
import type { PersonalChatModel } from './PersonalChatModel';

export type SelectedChatModel = {
    type: "group" | "personal" | null;
    chat: PersonalChatModel | GroupChatModel | null;
}