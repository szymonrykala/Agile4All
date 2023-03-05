import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import User from '../models/user'



export const usersSlice = createSlice({
    name: 'users',
    initialState: [] as User[],
    reducers: {
        add: (users, action: PayloadAction<User>) => {
            users.push(action.payload)
            return users;
        },
        load: (users, action: PayloadAction<User[]>) => {
            return action.payload
        },
        remove: (users, action: PayloadAction<User>) => {
            return users.filter(item => item.id !== action.payload.id)
        },
        flush: (users) => {
            return []
        },
        update: (users, action: PayloadAction<User>) => {
            const userIndex = users.findIndex(({ id }) => id === action.payload.id)
            users.splice(userIndex, 1, action.payload)
            return users
        },
    },
})

export const { add, remove, load, flush, update } = usersSlice.actions

// Other code such as selectors can use the imported `RootState` type
// export const selectCount = (users: RootState) => users

export default usersSlice.reducer