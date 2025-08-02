import { SetStateAction } from "react";
import { GroupChatMessage } from '../../../GroupChatMessage';
import { PersonalChatMessage } from '../../../PersonalChatMessage';

export interface ChatMessageMenuProps {
    message: PersonalChatMessage | GroupChatMessage;
    setEditModeIsOn(value: SetStateAction<boolean>): void;
    setOpenMessageMenu(value: SetStateAction<boolean>): void;
    updateMessageMarkedTypeAsync(type: number): Promise<void>;
}