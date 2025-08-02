import { SetStateAction } from 'react';

export interface CommonItemProps {
    connector: any;
    name: string;
    setName(value: SetStateAction<string>): void;
    description: string | "";
    setDescription(value: SetStateAction<string>): void;
    useDescription: boolean;
}