import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { UUID } from '../models/common'
import Project from '../models/project'
import User from '../models/user'


interface IAssignUser {
    projectId: UUID,
    user: User
}

interface IRemoveUser {
    projectId: UUID,
    userId: UUID
}


export const projectSlice = createSlice({
    name: 'projects',
    initialState: [] as Project[],
    reducers: {
        add: (projects, action: PayloadAction<Project>) => {
            projects.push(action.payload)
            return projects
        },
        load: (projects, action: PayloadAction<Project[]>) => {
            return action.payload
        },
        remove: (projects, action: PayloadAction<Project>) => {
            return projects.filter(item => item.id !== action.payload.id)
        },
        flush: (projects) => {
            return []
        },
        update: (projects, action: PayloadAction<Project>) => {
            const index = projects.findIndex(({ id }) => id === action.payload.id)
            projects.splice(index, 1, action.payload)
            return projects
        },
        assignUser: (projects, action: PayloadAction<IAssignUser>) => {
            const index = projects.findIndex(({ id }) => id === action.payload.projectId)
            if (index) {
                projects[index].users.push(action.payload.user)
            }
            return projects
        },
        removeUser: (projects, action: PayloadAction<IRemoveUser>) => {
            const index = projects.findIndex(({ id }) => id === action.payload.projectId)

            if (index !== -1) {
                projects[index].users =  projects[index].users.filter((user)=> user.id !== action.payload.userId)
            }
            return projects
        },
    },
})

export const { add, load, remove, flush, update, assignUser, removeUser } = projectSlice.actions

// Other code such as selectors can use the imported `RootState` type
// export const selectCount = (projects: RootState) => projects

export default projectSlice.reducer