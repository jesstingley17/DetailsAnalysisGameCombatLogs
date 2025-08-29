import { createContext, useContext } from 'react';
import type { ChatHubContextModel } from '../types/ChatHubModel';

export const ChatHubContext = createContext<ChatHubContextModel | null>(null);

export const useChatHub = () => useContext(ChatHubContext);