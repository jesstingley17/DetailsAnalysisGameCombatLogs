import type { RootState } from '@/app/Store';
import APP_CONFIG from "@/config/appConfig";
import { useAuth } from '@/shared/contexts/AuthProvider';
import logger from '@/utils/Logger';
import { faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { useLazyAuthorizationQuery } from '../../features/user/api/User.api';
import LanguageSelector from './LanguageSelector';
//import Notification from './Notification';
//import Search from './Search';

import './NavMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation('translate');

    const me = useSelector((state: RootState) => state.user.value);

    const auth = useAuth();

    const [authorization] = useLazyAuthorizationQuery();

    const navigate = useNavigate();

    const [showSearchBar, setShowSearchBar] = useState(false);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                await auth?.checkAuthAsync();
            } catch (error) {
                logger.error("Authentication check failed", error);
            }
        }

        checkAuth();
    }, []);

    const loginAsync = async () => {
        const identityServerAuthPath = APP_CONFIG.identity.authPath;

        await redirectToIdentityAsync(identityServerAuthPath || "");
    }

    const registrationAsync = async () => {
        const identityServerRegistrationPath = APP_CONFIG.identity.registryPath;

        await redirectToIdentityAsync(identityServerRegistrationPath || "");
    }

    const redirectToIdentityAsync = async (identityPath: string) => {
        try {
            const authUri = await authorization(identityPath).unwrap();

            window.location.href = authUri.uri;
        } catch (e) {
            logger.error("Failed to redirect to Identity server", e);
        }
    } 

    const handleLoginClick = useCallback(async () => await loginAsync(), [navigate]);
    const handleRegistrationClick = useCallback(async () => await registrationAsync(), [navigate]);
    const handleLogoutClick = useCallback(() => auth?.logoutAsync(), [auth]);

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
                        {/*{(me !== null && showSearchBar) &&*/}
                        {/*    <Search*/}
                        {/*        me={me}*/}
                        {/*        t={t}*/}
                        {/*    />*/}
                        {/*}*/}
                    </div>
                    <div className="main-elements">
                        {auth?.isAuthenticated
                            ? <div className="authorized">
                                {/*<Notification />*/}
                                <div className="username">{me?.username}</div>
                                <div className="authorized__logout" onClick={handleLogoutClick}>{t("Logout")}</div>
                            </div>
                            : <div className="authorization">
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
