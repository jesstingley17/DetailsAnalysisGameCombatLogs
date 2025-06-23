import { faBell } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useNotificationHub } from '../context/NotificationProvider';
import { AppNotification } from '../types/AppNotification';
import { useLazyGetNotificationsByRecipientIdQuery } from '../store/api/core/Notification.api';
import { useSelector } from 'react-redux';

const Notification: React.FC = () => {
    const { t } = useTranslation("notification");

    const navigate = useNavigate();

    const me = useSelector((state: any) => state.user.value);

    const notificationHub = useNotificationHub();

    const [getNotificationsByRecipientIdAsync] = useLazyGetNotificationsByRecipientIdQuery();

    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState<AppNotification[]>([]);

    useEffect(() => {
        if (!me) {
            return;
        }

        const getNotificationsByRecipientId = async () => {
            const notifications = await getNotificationsByRecipientIdAsync(me.id).unwrap();
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
    }, [notificationHub?.notificationHubConnection]);

    const personalChatNotifications = (notification: AppNotification) => {
        const navigateToChats = () => navigate(`/chats?personal=${notification.initiatorId}`);

        return (
            <li className="notification-message" key={notification.id}>{t("PersonalChatNotification")} '{notification.initiatorName}'. <span className="notifications-container__see" onClick={navigateToChats}>See</span></li>
        );
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
                            : notifications.map((notification) => (
                                personalChatNotifications(notification)
                            ))}
                    </ul>
                    <div className="notifications-container__read-all">{t("ReadAll")}</div>
                </div>
            }
        </div>
    );
}

export default Notification;