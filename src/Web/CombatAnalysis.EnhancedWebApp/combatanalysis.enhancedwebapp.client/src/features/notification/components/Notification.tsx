import type { RootState } from '@/app/Store';
import { useNotificationHub } from '@/shared/hooks/useNotificationHub';
import logger from '@/utils/Logger';
import { faBell, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyGetUnreadNotificationsByRecipientIdQuery } from '../api/Notification.api';
import type { AppNotificationModel } from '../types/AppNotificationModel';
import type { NotificationHubContextModel } from '../types/NotificationHubContextModel';

const Notification: React.FC = () => {
    const { t } = useTranslation('notification');

    const navigate = useNavigate();

    const user = useSelector((state: RootState) => state.user.value);

    const notificationHub = useNotificationHub();

    const [getUnreadNotificationsByRecipientIdAsync] = useLazyGetUnreadNotificationsByRecipientIdQuery();

    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState<AppNotificationModel[]>([]);

    useEffect(() => {
        if (!notificationHub) {
            return;
        }

        (async () => {
            await notificationHub.connectToNotificationAsync();
        })();

        return () => {
            (async () => {
                await notificationHub.disconnectNotificationsHubAsync();
            })();
        }
    }, []);

    useEffect(() => {
        if (!user) {
            return;
        }

        (async () => {
            try {
                const notifications = await getUnreadNotificationsByRecipientIdAsync(user.id).unwrap();
                setNotifications(notifications);
            } catch (e) {
                logger.error("Faield to receive user notifications", e);
            }
        })();
    }, [user]);

    useEffect(() => {
        if (!notificationHub || !notificationHub.notificationHubConnectionRef.current) {
            return;
        }

        notificationHub.subscribeToNotifications((notification: AppNotificationModel) => {
            setNotifications(prevNot => [...prevNot, notification]);
        });

        notificationHub.subscribeToRecipientNotifications(async () => {
            if (!user) {
                return;
            }

            try {
                const notifications = await getUnreadNotificationsByRecipientIdAsync(user.id).unwrap();
                setNotifications(notifications);
            } catch (e) {
                logger.error("Faield to receive user notifications", e);
            }
        });
    }, [notificationHub?.notificationHubConnectionRef.current]);

    const personalChatNotifications = (notificationHub: NotificationHubContextModel | null, notification: AppNotificationModel) => {
        const navigateToChats = async () => await seeNotificationAsync(notificationHub, notification);
        const removeNotification = async () => await removeNotificationAsync(notification);

        return (
            <li className="notification-message" key={notification.id}>
                <div className="notification-message__time">{getNotificationTime(notification)}</div>
                <div>{t("PersonalChatNotification")} '{notification.initiatorName}'. <span className="notifications-container__see" onClick={navigateToChats}>{t("See")}</span></div>
                <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Remove") || ""}
                    onClick={removeNotification}
                />
            </li>
        );
    }

    const getNotificationTime = (notification: AppNotificationModel) => {
        const date = new Date(notification.createdAt);
        const time = `${date.getHours()}:${date.getMinutes()}`;

        return time;
    }

    const seeNotificationAsync = async (notification1Hub: NotificationHubContextModel | null, notification: AppNotificationModel) => {
        if (!notificationHub || !user) {
            return;
        }

        notificationHub.notificationHubConnectionRef.current?.invoke("ReadNotification", notification.id, user.id)
            .then(() => navigate(`/chats?personal=${notification.initiatorId}`))
            .catch((e) => logger.error("Faield to read notification", e));
    }

    const removeNotificationAsync = async (notification: AppNotificationModel) => {
        if (!notificationHub || !user) {
            return;
        }

        await notificationHub.notificationHubConnectionRef.current?.invoke("RemoveNotification", notification.id, user.id);
    }

    const readAllNotifications = async () => {
        if (!notificationHub) {
            return;
        }

        await notificationHub.notificationHubConnectionRef.current?.invoke("ReadRecipientNotifications", user?.id);
    }

    return (
        <div className="notifications">
            <div className="notifications__alert">
                <FontAwesomeIcon
                    icon={faBell}
                    title={t("Notifications") || ""}
                    onClick={() => setShowNotifications(!showNotifications)}
                />
                {notifications?.length > 0 &&
                    <div className="notifications-count">
                        <div>{notifications?.length > 9 ? '9+' : notifications?.length}</div>
                    </div>
                }
            </div>
            {showNotifications &&
                <div className="notifications-container">
                    <div className="notifications-container__title">{t("Notifications")}</div>
                    <ul className="notifications-container__content">
                        {notifications.length === 0
                            ? <li className="notifications-container__empty">{t("Empty")}</li>
                            : notifications.map((notification) => personalChatNotifications(notificationHub, notification))}
                    </ul>
                    {notifications?.length > 0 &&
                        <div className="notifications-container__read-all" onClick={readAllNotifications}>{t("ClearAll")}</div>
                    }
                </div>
            }
        </div>
    );
}

export default Notification;