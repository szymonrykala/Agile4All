import { configureStore } from '@reduxjs/toolkit'
import chatSlice from './chatSlice';
import projectSlice from './projectSlice';
import sessionSlice from './sessionSlice';
import taskSlice from './taskSlice';
import usersSlice from './usersSlice';


const store = configureStore({
  reducer: {
    tasks: taskSlice,
    projects: projectSlice,
    users: usersSlice,
    session: sessionSlice,
    chat: chatSlice
  },
})


export default store;

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof store.getState>