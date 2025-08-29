import type { NotificationHubContextModel } from '@/features/notification/types/NotificationHubContextModel';
import { createContext, useContext } from 'react';

export const NotificationHubContext = createContext<NotificationHubContextModel | null>(null);

export const useNotificationHub = () => useContext(NotificationHubContext);