import { SetStateAction } from 'react';

export type CreateGroupChatProps = {
    setShowCreateGroupChat(value: SetStateAction<boolean>): void;
}