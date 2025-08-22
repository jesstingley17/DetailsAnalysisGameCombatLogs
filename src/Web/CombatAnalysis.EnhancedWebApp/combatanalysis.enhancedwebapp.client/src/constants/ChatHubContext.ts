import { createContext } from 'react';
import type { ChatHubModel } from "../context/ChatHubModel";

const ChatHubContext = createContext<ChatHubModel | null>(null);

export default ChatHubContext;