import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import Task from '../models/task'



export const tasksSlice = createSlice({
    name: 'tasks',
    initialState: [] as Task[],
    reducers: {
        add: (tasks, action: PayloadAction<Task>) => {
            tasks.push(action.payload)
            return tasks
        },
        load: (tasks, action: PayloadAction<Task[]>) => {
            return action.payload
        },
        remove: (tasks, action: PayloadAction<Task>) => {
            return tasks.filter(item => item.id !== action.payload.id)
        },
        flush: (tasks) => [],
        update: (tasks, action: PayloadAction<Task>) => {
            const taskIndex = tasks.findIndex(({ id }) => id === action.payload.id)
            tasks.splice(taskIndex, 1, action.payload)
            return tasks
        },
    },
})

export const { add, load, remove, flush, update } = tasksSlice.actions

// Other code such as selectors can use the imported `RootState` type
// export const selectCount = (tasks: RootState) => tasks

export default tasksSlice.reducer