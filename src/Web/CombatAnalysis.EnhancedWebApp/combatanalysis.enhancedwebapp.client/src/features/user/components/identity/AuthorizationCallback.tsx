/* eslint-disable @typescript-eslint/no-explicit-any */
import { useAuth } from '@/context/AuthProvider';
import logger from '@/utils/Logger';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLocation, useNavigate } from 'react-router-dom';
import { useLazyAuthorizationCodeExchangeQuery } from '../../api/Identity.api';
import { useLazyStateValidateQuery } from '../../api/User.api';

import './AuthorizationCallback.scss';

const unauthorizedTimeoutLimit = 4000;

const AuthorizationCallback: React.FC = () => {
    const { t } = useTranslation("identity/authorizationCallback");

    const navigate = useNavigate();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);

    const { checkAuthAsync } = useAuth();

    const [stateIsValid, setStateIsValid] = useState(true);
    const [accessRestored, setAcessRestored] = useState(false);
    const [verified, setVerified] = useState(false);

    const [stateValidate] = useLazyStateValidateQuery();
    const [authorizationCodeExchange] = useLazyAuthorizationCodeExchangeQuery();

    useEffect(() => {
        const accessRestored: any = queryParams.get("accessRestored");
        setAcessRestored(accessRestored);

        const verified: any = queryParams.get("verified");
        setVerified(verified);

        const code: any = queryParams.get("code");
        const state: any = queryParams.get("state");

        if (!code && !state && !accessRestored && !verified) {
            navigate("/");
        }

        const validateState = async () => {
            await validateStateAsync(state, code);
        }

        if (code && state) {
            validateState();
        }
    }, []);

    useEffect(() => {
        let timeout: NodeJS.Timeout;
        if (!stateIsValid || accessRestored || verified) {
            timeout = setTimeout(() => {
                navigate("/");
            }, unauthorizedTimeoutLimit);
        }

        return () => {
            clearTimeout(timeout);
        }
    }, [stateIsValid, accessRestored, verified]);

    const navigateToTokenAsync = async (authorizationCode: string) => {
        try {
            await authorizationCodeExchange(authorizationCode).unwrap();
            await checkAuthAsync();

            navigate("/");
        } catch (e) {
            logger.error("Failed to exchange the authorization code", e);

            setStateIsValid(false);
        }
    }

    const validateStateAsync = async (state: string, code: string) => {
        try {
            await stateValidate(state).unwrap();
            await navigateToTokenAsync(code);
        } catch (e) {
            logger.error("Failed to validate the authorzation state", e);

            setStateIsValid(false);
        }
    }

    if (accessRestored) {
        return (
            <div className="authorization-callback">
                <div className="successful">{t("AccessRestored")}</div>
            </div>
        );
    }

    if (verified) {
        return (
            <div className="authorization-callback">
                <div className="successful">{t("Verified")}</div>
            </div>
        );
    }

    return (
        <div className="authorization-callback">
            {stateIsValid
                ? <div className="successful">{t("Authorization")}</div>
                : <div className="failed">{t("Unauthorized")}</div>
            }
        </div>
    );
}

export default memo(AuthorizationCallback);