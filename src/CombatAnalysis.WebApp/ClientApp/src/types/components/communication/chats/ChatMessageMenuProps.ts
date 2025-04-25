import { SetStateAction } from "react";

export interface ChatMessageMenuProps {
    setEditModeIsOn: (value: SetStateAction<boolean>) => void;
    setOpenMessageMenu: (value: SetStateAction<boolean>) => void;
}