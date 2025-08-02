/* eslint-disable @typescript-eslint/no-explicit-any */
import { isRejectedWithValue } from '@reduxjs/toolkit';
import { pageWithoutAuth, statusCode, unautorizedRedirectTo } from '../config';
import { updateCustomer } from '../store/slicers/CustomerSlice';

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