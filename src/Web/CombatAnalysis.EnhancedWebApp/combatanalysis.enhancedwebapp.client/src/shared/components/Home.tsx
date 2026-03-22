import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import { useLazyAuthorizationQuery } from '@/features/user/api/User.api';
import logger from '@/utils/Logger';
import {
    faArrowRight,
    faChartLine,
    faCheck,
    faComments,
    faTriangleExclamation,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

import './Home.scss';

const shouldBeAuthorizeTimeout = 10000;

const Home: React.FC = () => {
    const { t } = useTranslation('home');

    const navigate = useNavigate();
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);

    const myself = useSelector((state: RootState) => state.user.value);

    const auth = useAuth();

    const [authorization] = useLazyAuthorizationQuery();

    const [shouldBeAuthorize, setShouldBeAuthorize] = useState(false);

    useEffect(() => {
        (async () => {
            const shouldBeAuthorize = searchParams.get("shouldBeAuthorize") !== null;
            if (!shouldBeAuthorize) {
                return;
            }

            setShouldBeAuthorize(!auth?.isAuthenticated);
            if (auth?.isAuthenticated) {
                navigate("/");
            }
        })();
    }, [auth?.isAuthenticated]);

    useEffect(() => {
        let timeoutId: NodeJS.Timeout;

        if (shouldBeAuthorize) {
            timeoutId = setTimeout(() => {
                setShouldBeAuthorize(false);
            }, shouldBeAuthorizeTimeout);
        }

        return () => clearTimeout(timeoutId);
    }, [shouldBeAuthorize]);

    const loginHandle = async () => {
        try {
            const identityRedirect = await authorization(APP_CONFIG.identity.loginPath).unwrap();

            const uri = identityRedirect.uri;
            window.location.href = uri;
        } catch (e) {
            logger.error("Failed to redirect to the Login page", e);
        }
    }

    const navigateToFeed = () => navigate("/feed");
    const navigateToGameCombatLogs = () => navigate("/game-combat-logs");

    return (
        <div className="home">
            <div className="home__bg" aria-hidden />
            <div className="home__grain" aria-hidden />

            <header className="home__hero">
                <div className="home__heroGlow home__heroGlow--a" aria-hidden />
                <div className="home__heroGlow home__heroGlow--b" aria-hidden />
                <div className="home__heroInner">
                    <span className="home__badge">{t("HeroBadge")}</span>
                    <h1 className="home__headline">{t("HeroHeadline")}</h1>
                    <p className="home__sub">{t("HeroSub")}</p>
                    <div className="home__heroActions">
                        {!myself && (
                            <button
                                type="button"
                                className="home__btn home__btn--ghost"
                                onClick={loginHandle}
                                title={t("GoToLogin") || ""}
                            >
                                <FontAwesomeIcon icon={faTriangleExclamation} className="home__btnIcon" />
                                {t("ShouldAuthorize")}
                            </button>
                        )}
                        <button
                            type="button"
                            className="home__btn home__btn--primary"
                            onClick={navigateToGameCombatLogs}
                        >
                            {t("Analyzing")}
                            <FontAwesomeIcon icon={faArrowRight} className="home__btnArrow" />
                        </button>
                        {myself !== null && (
                            <button
                                type="button"
                                className="home__btn home__btn--secondary"
                                onClick={navigateToFeed}
                            >
                                {t("Communication")}
                                <FontAwesomeIcon icon={faArrowRight} className="home__btnArrow" />
                            </button>
                        )}
                    </div>
                </div>
            </header>

            <section className="home__cards" aria-label="Features">
                <article className="homeCard homeCard--social">
                    <div className="homeCard__iconWrap">
                        <FontAwesomeIcon icon={faComments} className="homeCard__icon" />
                    </div>
                    <h2 className="homeCard__title">{t("Communication")}</h2>
                    <ul className="homeCard__list">
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("ExploreFeed")}</li>
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("ChattingWithFriends")}</li>
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("CreateJoinCommunity")}</li>
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("AddFriedns")}</li>
                    </ul>
                    {myself !== null ? (
                        <button
                            type="button"
                            className="homeCard__cta"
                            onClick={navigateToFeed}
                            data-testid="go-to-communication"
                        >
                            {t("Open")}
                            <FontAwesomeIcon icon={faArrowRight} />
                        </button>
                    ) : (
                        <button type="button" className="homeCard__cta homeCard__cta--muted" onClick={loginHandle}>
                            {t("Login")}
                            <FontAwesomeIcon icon={faArrowRight} />
                        </button>
                    )}
                </article>

                <article className="homeCard homeCard--logs">
                    <div className="homeCard__iconWrap homeCard__iconWrap--accent">
                        <FontAwesomeIcon icon={faChartLine} className="homeCard__icon" />
                    </div>
                    <h2 className="homeCard__title">{t("Analyzing")}</h2>
                    <ul className="homeCard__list">
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("SaveAnalyzing")}</li>
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("ExploreAnalyzing")}</li>
                        <li><FontAwesomeIcon icon={faCheck} className="homeCard__check" />{t("ShareAnalyzing")}</li>
                    </ul>
                    <button
                        type="button"
                        className="homeCard__cta homeCard__cta--accent"
                        onClick={navigateToGameCombatLogs}
                        data-testid="go-to-combat-logs"
                    >
                        {t("Open")}
                        <FontAwesomeIcon icon={faArrowRight} />
                    </button>
                </article>
            </section>

            {shouldBeAuthorize && (
                <div className="home__toast" data-testid="should-be-authorize" role="status">
                    <div className="home__toastInner">
                        {t("YouNeed")}{' '}
                        <button type="button" className="home__toastLink" onClick={loginHandle}>
                            {t("Login")}
                        </button>{' '}
                        {t("InApp")}
                    </div>
                </div>
            )}
        </div>
    );
}

export default Home;
