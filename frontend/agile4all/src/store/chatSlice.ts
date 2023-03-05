import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { ChatMessage } from '../models/chat'



export const chatSlice = createSlice({
    name: 'chat',
    initialState: [] as ChatMessage[],
    reducers: {
        add: (messages, action: PayloadAction<ChatMessage>) => {
            messages.push(action.payload);
            return messages;
        },
        load: (messages, action: PayloadAction<ChatMessage[]>) => {
            return action.payload;
        },
        leaveMessages: (messages, action: PayloadAction<number>) => {
            return messages.slice(-action.payload)
        },
        reset: (messages) => {
            return []
        },
    },
})

export const { add, load, leaveMessages, reset } = chatSlice.actions

// Other code such as selectors can use the imported `RootState` type
// export const selectCount = (users: RootState) => users

export default chatSlice.reducer