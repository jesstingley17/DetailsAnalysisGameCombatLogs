import { faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand } from 'reactstrap';
import { useAuth } from '../context/AuthProvider';
import { useLazyAuthorizationQuery } from '../store/api/core/User.api';
//import LanguageSelector from './LanguageSelector';
//import Notification from './Notification';
//import Search from './Search';
import type { RootState } from '../store/Store';

import '../styles/navMenu.scss';

const NavMenu: React.FC = () => {
    const { t } = useTranslation("translate");

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
                console.error("Authentication check failed", error);
            }
        }

        checkAuth();
    }, []);

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
    const handleLogoutClick = useCallback(() => auth?.logoutAsync(), [auth]);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <div className="brand-container">
                        {/*<LanguageSelector />*/}
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
