import { createContext, useContext } from 'react';
import type { ChatHubModel } from '../types/ChatHubModel';

const ChatHubContext = createContext<ChatHubModel | null>(null);

export const useChatHub = () => useContext(ChatHubContext);