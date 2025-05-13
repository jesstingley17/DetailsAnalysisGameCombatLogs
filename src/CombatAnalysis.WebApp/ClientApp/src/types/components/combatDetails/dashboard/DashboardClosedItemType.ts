import { SetStateAction } from "react";

export interface DashboardClosedItemType {
    id: number,
    name: string,
    setItemClosed: (value: SetStateAction<boolean>) => void,
}