import { createSlice } from '@reduxjs/toolkit';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

type GroupChatUserSlice = {
    value: GroupChatUserModel | null;
}

const initialState: GroupChatUserSlice = {
    value: null,
}

export const groupChatUserSlice = createSlice({
    name: 'groupChatUser',
    initialState,
    reducers: {
        updateGroupChatUser: (state, action) => {
            state.value = action.payload
        },
    },
})

export const { updateGroupChatUser } = groupChatUserSlice.actions

export default groupChatUserSlice.reducer