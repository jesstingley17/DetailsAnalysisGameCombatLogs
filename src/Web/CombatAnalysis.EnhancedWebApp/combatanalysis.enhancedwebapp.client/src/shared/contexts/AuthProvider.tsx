import { createContext, useContext, useState, type ReactNode } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyAuthenticationQuery } from '../../features/user/api/User.api';
import { useLogoutAsyncMutation } from '../../features/user/api/Account.api';
import { useLazySearchCustomerByUserIdQuery } from '../../features/user/api/Customer.api';
import { useLazyGetUserPrivacyQuery } from '../../features/user/api/Identity.api';
import { updateCustomer } from '../../features/user/store/CustomerSlice';
import { updateUserPrivacy } from '../../features/user/store/UserPrivacySlice';
import { updateUser } from '../../features/user/store/UserSlice';

interface AuthContextType {
    isAuthenticated: boolean;
    checkAuthAsync: () => Promise<void>;
    logoutAsync: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | null>(null);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const [getAuth] = useLazyAuthenticationQuery();
    const [logout] = useLogoutAsyncMutation();
    const [searchCustomer] = useLazySearchCustomerByUserIdQuery();

    const [getUserPrivacyAsyncMut] = useLazyGetUserPrivacyQuery();

    const checkAuthAsync = async () => {
        const response = await getAuth();
        if (response.error !== undefined || !response.data) {
            dispatch(updateUser(null));
            dispatch(updateCustomer(null));
            dispatch(updateUserPrivacy(null));

            return;
        }

        dispatch(updateUser(response.data));
        await getCustomerDataAsync(response.data?.id);

        await getUserPrivacyAsync(response.data.identityUserId);
    }

    const getCustomerDataAsync = async (userId: string) => {
        const response = await searchCustomer(userId);
        if (response.error !== undefined) {
            return;
        }

        dispatch(updateCustomer(response.data));

        setIsAuthenticated(true);
    }

    const getUserPrivacyAsync = async (id: string) => {
        const response = await getUserPrivacyAsyncMut(id);
        if (response.error !== undefined) {
            return;
        }

        dispatch(updateUserPrivacy(response.data));
    }

    const logoutAsync = async () => {
        setIsAuthenticated(false);
        dispatch(updateCustomer(null));
        dispatch(updateUser(null));
        dispatch(updateUserPrivacy(null));

        navigate("/");

        await logout();
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, checkAuthAsync, logoutAsync }}>
            {children}
        </AuthContext.Provider>
    );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};