import { Delete, Edit, LinkRounded, Save } from "@mui/icons-material";
import { Avatar, Button, Divider, IconButton, List, ListItem, ListItemButton, ListItemContent, Option, Select, Sheet, Stack, Typography } from "@mui/joy";
import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { UsersApi } from "../../client";
import { useAppDispatch, useAppSelector, useCheckAdmin } from "../../hooks";
import { UUID } from "../../models/common";
import Project from "../../models/project";
import User, { UserRole } from "../../models/user";
import { remove, update } from "../../store/usersSlice";
import EditableTextField from "../common/EditableTextField";
import NamedAvatar from "../Tasks/NamedAvatar";
import SidePanel from "../common/SidePanel";
import useNotification from "../Notification";



const demoUser: User = {
    id: -1,
    email: 'loading...',
    firstName: 'loading...',
    lastName: 'loading...',
    role: UserRole.STUDENT,
}

function selectProjectsOfUser(projects: Project[], userId: UUID) {
    return projects.filter(({ users }) =>
        users.find(({ id }) => id === userId)
    )
}

export default function UserPanel() {
    const navigate = useNavigate();
    const { userId } = useParams();
    const dispatch = useAppDispatch();

    const reduxUser = useAppSelector(({ users }) => users.find(({ id }) => id === Number(userId))) || demoUser
    const projects = useAppSelector(({ projects }) => selectProjectsOfUser(projects, reduxUser.id))
    const isAdmin = useCheckAdmin()
    const sessionUserId = useAppSelector(({ session }) => session?.user.id)

    const [user, setUser] = useState(reduxUser);
    const [editMode, setEditMode] = useState<boolean>(false);
    const { info, error } = useNotification();


    const updateUserField = useCallback((
        field: string,
        value: string
    ) => {
        setUser({ ...user, [field]: value })
    }, [user, setUser])


    const roleUpdate = useCallback((event: any, newRole: UserRole | null) => {
        if (newRole) setUser({ ...user, role: newRole })
    }, [user, setUser]);


    const saveUser = useCallback(async () => {
        try {
            await UsersApi.update(user.id, {
                firstName: user.firstName,
                lastName: user.lastName,
                role: user.role
            })
            dispatch(update(user));
            info("User has been updated");
        } catch (err) {
            setUser(reduxUser)
            error(err)
        }
    }, [user, dispatch, error, info, reduxUser]);


    const deleteUser = useCallback(async () => {
        if(!window.confirm('Do You want to delete this user?')) return;

        try {
            await UsersApi.delete(user.id);
            dispatch(remove(user));

            if(sessionUserId === userId){
                navigate("/logout");
            }else{
                navigate('../');
                info("User has been deleted");
            }
        } catch (err) {
            error(err)
        }
    }, [user, dispatch, navigate, info, error]);


    useEffect(()=>{
        reduxUser && setUser(reduxUser);
    // displayed task should be replaced only if `taskId` was changed
    // eslint-disable-next-line react-hooks/exhaustive-deps
    },[userId])


    return (
        <SidePanel>
            <NamedAvatar user={user} />
            <Sheet
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    bgcolor: 'inherit',
                    justifyContent: 'space-between'
                }}>

                <Select
                    size="sm"
                    value={user.role}
                    color='neutral'
                    variant="soft"
                    sx={{ minWidth: 120 }}
                    onChange={roleUpdate}
                    disabled={!editMode}
                >
                    <Option value={UserRole.STUDENT}>Student</Option>
                    <Option value={UserRole.ADMIN}>Admin</Option>
                </Select>
                <Stack
                    direction='row'
                    spacing={1}
                    sx={{ flexWrap: 'wrap', justifyContent: 'flex-end' }}
                >
                    <Button
                        size="sm"
                        variant="soft"
                        component={Link}
                        to={`/app/tasks/users/${user.id}`}
                    >
                        Tasks
                    </Button>
                    {(isAdmin || (reduxUser.id === sessionUserId)) && <>
                        <IconButton
                            onClick={() => setEditMode(!editMode)}
                        >
                            <Edit />
                        </IconButton>
                        <IconButton color='success' onClick={saveUser}>
                            <Save />
                        </IconButton>
                        <IconButton color='danger' onClick={deleteUser}>
                            <Delete />
                        </IconButton>
                    </>
                    }
                </Stack>
            </Sheet>

            <Divider />
            <Stack direction='row' spacing={1}>
                <EditableTextField
                    title='Firstname'
                    value={user.firstName}
                    editable={editMode}
                    onChange={(text) => updateUserField('firstName', text)}
                />
                <EditableTextField
                    title='Lastname'
                    value={user.lastName}
                    editable={editMode}
                    onChange={(text) => updateUserField('lastName', text)}
                />
            </Stack>

            <Typography level="body-sm">
                Projects:
            </Typography>
            <List>
                {
                    projects.map((project, index) =>
                        < ListItem key={index}>
                            <ListItemButton component={Link} to={`/app/tasks/users/${user.id}/projects/${project.id}`}>
                                <ListItemContent>
                                    <Typography startDecorator={
                                        <Avatar
                                            title={project.name}
                                            src="https://th.bing.com/th/id/R.75f9b714fb48bdf6c7c758dbc7f391e6?rik=BNp%2bziTPLYnx3g&pid=ImgRaw&r=0"
                                        />
                                    }>
                                        {project.name}
                                    </Typography>
                                </ListItemContent>
                                <LinkRounded />
                            </ListItemButton>
                        </ListItem>
                    )
                }
            </List>
        </SidePanel>
    )
}
