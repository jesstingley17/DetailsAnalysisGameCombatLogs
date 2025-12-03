/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { toast } from 'react-toastify';
import { type Middleware } from 'redux';

const errorHandlingMiddleware: Middleware = (_store: any) => (next: any) => (action: any) => {
    if (action.error) {
        toast.error(`Error: ${action.error.message || 'An unexpected error occurred'}`, {
            position: "top-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }

    return next(action);
}

export default errorHandlingMiddleware;