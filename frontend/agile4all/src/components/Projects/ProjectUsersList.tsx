import { List, ListItem, ListItemContent, IconButton, Typography, Stack, Avatar, ListItemDecorator } from "@mui/joy";
import { ReactNode, useCallback, useEffect, useMemo, useState } from "react";
import { ProjectsApi } from "../../client";
import { useAppDispatch, useAppSelector } from "../../hooks";
import { UUID } from "../../models/common";
import User from "../../models/user";
import EditableTextField from "../common/EditableTextField";
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { assignUser, removeUser } from "../../store/projectSlice";


interface IProjectUsersList {
    projectId: UUID,
    users: User[]
}

interface IUserItem {
    onRemove?: () => void
    onAdd?: () => void
    children: ReactNode
}

function UserItem({ children, onRemove, onAdd }: IUserItem) {
    return (
        <ListItem sx={{
            bgcolor: "background.appBody",
            borderRadius: 10
        }}>
            <ListItemDecorator sx={{ alignSelf: 'flex-start' }}>
                <Avatar src="/static/images/avatar/1.jpg" />
            </ListItemDecorator>&nbsp;&nbsp;&nbsp;
            <ListItemContent>
                <Stack direction={'row'} sx={{
                    alignItems: 'center',
                    justifyContent: 'space-between'
                }}>
                    <Typography>
                        {children}
                    </Typography>
                    {onRemove && <IconButton color="danger" onClick={onRemove}>
                        <DeleteIcon />
                    </IconButton>}
                    {onAdd && <IconButton color="success" onClick={onAdd}>
                        <AddIcon />
                    </IconButton>}
                </Stack>
            </ListItemContent>
        </ListItem>
    )
}


const style = {
    assignedUsersList: {
        maxHeight: '200px',
        overflowY: 'scroll',
        bgcolor: "background.componentBg",
        "--List-gap": '5px',
        "--List-padding": "5px"
    },
    searchedUsersList: {
        "--List-gap": '5px',
        "--List-padding": "5px",
        bgcolor: "background.componentBg",
        maxHeight: '350px'
    }
}


export default function ProjectUsersList({ projectId, users }: IProjectUsersList) {
    const [localUsers, setLocalUsers] = useState<User[]>([])

    const projectUsersIds = useMemo(() => localUsers.map(({ id }) => id), [localUsers]);

    const allUsers = useAppSelector(({ users }) => users.filter(user =>
        !projectUsersIds.includes(user.id)
    ))

    const dispatch = useAppDispatch()
    const [search, setSearch] = useState<string>('');

    const filteredAllUsers = useMemo(() => {
        if (search !== '') {
            return allUsers.filter(({ email }) => email.match(search))
        }
        return allUsers
    }, [search, allUsers])


    const addUserToProject = useCallback(async (user: User) => {
        await ProjectsApi.addUser(projectId, user.id)

        dispatch(assignUser({ projectId: projectId, user: user }))
        setLocalUsers([...localUsers, user])
    }, [
        localUsers,
        dispatch,
        setLocalUsers,
        projectId
    ]);

    const removeProjectUser = useCallback(async (id: UUID) => {
        await ProjectsApi.removeUser(projectId, id)

        dispatch(removeUser({ projectId: projectId, userId: id }))
        const newUsers = localUsers.filter((user) => user.id !== id)
        setLocalUsers(newUsers);
    }, [
        projectId,
        localUsers,
        setLocalUsers,
        dispatch
    ])

    useEffect(() => {
        setLocalUsers(users)
    }, [users])

    return (
        <>
            <Typography level='body-sm'>
                Assigned users:
            </Typography>
            <List size="sm" sx={style.assignedUsersList}>
                {
                    localUsers.length > 0 ?
                        localUsers.map((user, index) =>
                            <UserItem
                                key={index}
                                onRemove={() => removeProjectUser(user.id)}
                            >
                                {user.email}
                            </UserItem>
                        )
                        : <Typography component='li'>
                            No users assigned
                        </Typography>
                }
            </List>
            <EditableTextField
                title="filter users"
                editable
                value={search}
                onChange={setSearch}
            />
            <List size="sm" sx={style.searchedUsersList}>
                {
                    filteredAllUsers.map((user, index) =>
                        <UserItem
                            key={`-${index}`}
                            onAdd={() => addUserToProject(user)}
                        >
                            {user.email}
                        </UserItem>
                    )
                }
            </List>
        </>
    )
}