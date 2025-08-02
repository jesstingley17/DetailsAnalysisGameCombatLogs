import { createSlice } from '@reduxjs/toolkit';
import type { CustomerModel } from '../types/CustomerModel';

type CustomerSlice = {
    value: CustomerModel | null;
}

const initialState: CustomerSlice = {
    value: null,
}

export const customerSlice = createSlice({
    name: 'customer',
    initialState,
    reducers: {
        updateCustomer: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateCustomer } = customerSlice.actions

export default customerSlice.reducer