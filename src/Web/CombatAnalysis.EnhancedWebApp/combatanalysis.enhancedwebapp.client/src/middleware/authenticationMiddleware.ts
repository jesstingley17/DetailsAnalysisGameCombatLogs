/* eslint-disable @typescript-eslint/no-explicit-any */
import { pageWithoutAuth, statusCode, unautorizedRedirectTo } from '@/config';
import { updateCustomer } from '@/features/user/store/CustomerSlice';
import { isRejectedWithValue } from '@reduxjs/toolkit';

const authenticationMiddleware = (store: any) => (next: any) => (action: any) => {
    const pathName = window.location.pathname;

    if (!pageWithoutAuth.includes(pathName) && isRejectedWithValue(action)) {
        if (action.payload.status === statusCode["notAuthorized"]) {
            store.dispatch(updateCustomer(null));
            window.location.href = unautorizedRedirectTo + "?shouldBeAuthorize";
        }
    }

    return next(action);
};

export default authenticationMiddleware;