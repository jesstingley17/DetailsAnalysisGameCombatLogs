import { createSlice } from '@reduxjs/toolkit';
import type { AppUserModel } from '../types/AppUserModel';

type AppUserSlice = {
    value: AppUserModel | null;
}

const initialState: AppUserSlice = {
    value: null,
}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        updateUser: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateUser } = userSlice.actions

export default userSlice.reducer