import { createSlice } from '@reduxjs/toolkit';
import type { AppUserPrivacyModel } from '../types/AppUserPrivacyModel';

type AppUserPrivacySlice = {
    value: AppUserPrivacyModel | null;
}

const initialState: AppUserPrivacySlice = {
    value: null,
}

export const userPrivacySlice = createSlice({
    name: 'userPrivacy',
    initialState,
    reducers: {
        updateUserPrivacy: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateUserPrivacy } = userPrivacySlice.actions

export default userPrivacySlice.reducer