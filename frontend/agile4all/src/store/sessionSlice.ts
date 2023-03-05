import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { Session} from '../models/user'




export const sessionSlice = createSlice({
    name: 'session',
    initialState: null as Session | null,
    reducers: {
        setSession: (user, action: PayloadAction<Session>) => {
            return action.payload
        },
        removeSession: (users) => {
            return null
        }
    },
})

export const { setSession, removeSession } = sessionSlice.actions

// Other code such as selectors can use the imported `RootState` type
// export const selectCount = (users: RootState) => users

export default sessionSlice.reducer