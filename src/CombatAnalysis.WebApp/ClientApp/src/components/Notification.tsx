import { faBell, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useNotificationHub } from '../context/NotificationProvider';
import { useLazyGetUnreadNotificationsByRecipientIdQuery } from '../store/api/core/Notification.api';
import { AppNotification } from '../types/AppNotification';

const Notification: React.FC = () => {
    const { t } = useTranslation("notification");

    const navigate = useNavigate();

    const me = useSelector((state: any) => state.user.value);

    const notificationHub = useNotificationHub();

    const [getUnreadNotificationsByRecipientIdAsync] = useLazyGetUnreadNotificationsByRecipientIdQuery();

    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState<AppNotification[]>([]);

    useEffect(() => {
        if (!me) {
            return;
        }

        const getNotificationsByRecipientId = async () => {
            const notifications = await getUnreadNotificationsByRecipientIdAsync(me.id).unwrap();
            setNotifications(notifications);
        }

        getNotificationsByRecipientId();
    }, [me]);

    useEffect(() => {
        if (!notificationHub || !notificationHub.notificationHubConnection) {
            return;
        }

        notificationHub.subscribeToNotifications((notification: AppNotification) => {
            setNotifications(prevNot => [...prevNot, notification]);
        });

        notificationHub.subscribeToRecipientNotifications(async () => {
            const notifications = await getUnreadNotificationsByRecipientIdAsync(me.id).unwrap();
            setNotifications(notifications);
        });
    }, [notificationHub?.notificationHubConnection]);

    const personalChatNotifications = (notification: AppNotification) => {
        const navigateToChats = () => seeNotification(notification);
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

    const getNotificationTime = (notification: AppNotification) => {
        const date = new Date(notification.createdAt);
        const time = `${date.getHours()}:${date.getMinutes()}`;

        return time;
    }

    const seeNotification = (notification: AppNotification) => {
        if (!notificationHub || !me) {
            return;
        }

        notificationHub.notificationHubConnection?.invoke("ReadNotification", notification.id, me.id).then(() => navigate(`/chats?personal=${notification.initiatorId}`));
    }

    const removeNotificationAsync = async (notification: AppNotification) => {
        if (!notificationHub || !me) {
            return;
        }

        await notificationHub.notificationHubConnection?.invoke("RemoveNotification", notification.id, me.id);
    }

    const readAllNotifications = async () => {
        if (!notificationHub) {
            return;
        }

        await notificationHub.notificationHubConnection?.invoke("ReadRecipientNotifications", me?.id);
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
                            : notifications.map((notification) => personalChatNotifications(notification))}
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