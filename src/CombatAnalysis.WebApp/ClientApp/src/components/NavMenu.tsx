import { faBell, faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { useAuth } from '../context/AuthProvider';
import { useNotificationHub } from '../context/NotificationProvider';
import { useLazyAuthorizationQuery } from '../store/api/core/User.api';
import { AppNotification } from '../types/AppNotification';
import LanguageSelector from './LanguageSelector';
import Search from './Search';

import '../styles/navMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation("translate");

    const me = useSelector((state: any) => state.user.value);

    const notificationHub = useNotificationHub();

    const { isAuthenticated, checkAuthAsync, logoutAsync } = useAuth();

    const [authorization] = useLazyAuthorizationQuery();

    const navigate = useNavigate();

    const [showSearchBar, setShowSearchBar] = useState(false);
    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState<AppNotification[]>([]);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                await checkAuthAsync();
            } catch (error) {
                console.error("Authentication check failed", error);
            }
        }

        checkAuth();
    }, []);

    useEffect(() => {
        if (!notificationHub || !notificationHub.notificationHubConnection) {
            return;
        }

        notificationHub.subscribeToNotifications((notification: AppNotification) => {
            setNotifications(prevNot => [...prevNot, notification]);
        });
    }, [notificationHub?.notificationHubConnection]);

    const loginAsync = async () => {
        const identityServerAuthPath = process.env.REACT_APP_IDENTITY_SERVER_AUTH_PATH;

        await redirectToIdentityAsync(identityServerAuthPath || "");
    }

    const registrationAsync = async () => {
        const identityServerRegistrationPath = process.env.REACT_APP_IDENTITY_SERVER_REGISTRY_PATH;

        await redirectToIdentityAsync(identityServerRegistrationPath || "");
    }

    const redirectToIdentityAsync = async (identityPath: string) => {
        const response = await authorization(identityPath);

        if (response.data !== undefined) {
            const uri = response.data.uri;
            window.location.href = uri;
        }
    } 

    const handleLoginClick = useCallback(async () => await loginAsync(), [navigate]);
    const handleRegistrationClick = useCallback(async () => await registrationAsync(), [navigate]);
    const handleLogoutClick = useCallback(() => logoutAsync(), [logoutAsync]);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <div className="brand-container">
                        <LanguageSelector />
                        <div className="brand">
                            <NavbarBrand
                                tag={Link}
                                to="/"
                            >
                                Wow Analysis
                            </NavbarBrand>
                            {me !== null &&
                                <FontAwesomeIcon
                                    icon={showSearchBar ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                                    title={(showSearchBar ? t("HideSearchCommunity") : t("ShowSearchCommunity")) || ""}
                                    onClick={() => setShowSearchBar(!showSearchBar)}
                                />
                            }
                        </div>
                        {(me !== null && showSearchBar) &&
                            <Search
                                me={me}
                                t={t}
                            />
                        }
                    </div>
                    <div className="main-elements">
                        {showNotifications && 
                            <div className="notifications">
                                <div>{t("Notifications")}</div>
                                <ul>
                                    {notifications.length === 0
                                        ? <li className="notifications__empty">{t("Empty")}</li>
                                        : notifications.map((notification) => (
                                            <li>{notification.title}</li>
                                        ))}
                                </ul>
                            </div>
                        }
                        {isAuthenticated
                            ? <div className="authorized">
                                <FontAwesomeIcon
                                    icon={faBell}
                                    title={t("Notifications") || ""}
                                    onClick={() => setShowNotifications(!showNotifications)}
                                />
                                <div className="username">{me?.username}</div>
                                <div className="authorized__logout" onClick={handleLogoutClick}>{t("Logout")}</div>
                            </div>
                            : <div className="authorization">
                                <FontAwesomeIcon
                                    icon={faBell}
                                    title={t("Notifications") || ""}
                                    onClick={() => setShowNotifications(!showNotifications)}
                                />
                                <div className="authorization__login" onClick={handleLoginClick}>{t("Login")}</div>
                                <div className="authorization__registration" onClick={handleRegistrationClick}>{t("Registration")}</div>
                            </div>
                        }
                    </div>
                </Container>
            </Navbar>
        </header>
    );
}

export default NavMenu;
