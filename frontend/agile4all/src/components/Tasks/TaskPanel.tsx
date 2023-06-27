import { Box, Divider, IconButton, Option, Select, Typography } from "@mui/joy";
import Task, { TaskStatus } from "../../models/task";
import { getStatusColor } from "./StatusChip";
import NamedAvatar from "./NamedAvatar";
import { useCallback, useMemo, useState } from "react";
import FilesPanel from "../FilesPanel";
import EditIcon from "@mui/icons-material/Edit";
import SaveIcon from "@mui/icons-material/Save";
import DeleteIcon from "@mui/icons-material/Delete";
import EditableTextArea from "../common/EditableTextArea";
import confirm from "../common/Confirm";
import { UUID } from "../../models/common";
import { useAppDispatch, useAppSelector } from "../../hooks";
import { TasksApi } from "../../client";
import { remove, update } from "../../store/taskSlice";
import { useParams } from "react-router";
import SidePanel from "../common/SidePanel";



const demoTaskData = {
    description: `loading...`,
    name: `loading...`,
    id: -1,
    projectId: -1,
    status: TaskStatus.ARCHIVED,
    userId: -1
}


export default function TaskPanel() {
    const [editMode, setEditMode] = useState<boolean>(false);
    const dispatch = useAppDispatch();
    const { taskId } = useParams()
    const initTask = useAppSelector(({ tasks }) => tasks.find(({ id }) => id === Number(taskId)))

    const project = useAppSelector(({ projects }) => projects.find(({ id }) => id === initTask?.projectId));

    const user = useMemo(() => {
        return project?.users.find(({ id }) => id === initTask?.userId)
    }, [
        initTask?.userId,
        project?.users
    ])


    const [task, setTask] = useState<Task>(initTask || demoTaskData);


    const deleteTask = useCallback(async () => {
        const proceed = await confirm('Do You want to delete this task?')
        if (!proceed) return;

        await TasksApi.delete(task.id)
        dispatch(remove(task))
    }, [task, dispatch])


    const updateTask = useCallback(async () => {
        if (task !== initTask) {
            dispatch(update(task))
            try {
                await TasksApi.update(task.id, {
                    description: task.description,
                    name: task.name,
                    status: task.status,
                    userId: Number(task.userId)
                })
            } catch (err) { alert(err) }
        }

    }, [task, dispatch, initTask])


    const updateAssignedUser = useCallback((event: any, user: UUID | null) => {
        if (!user) return;

        setTask({ ...task, userId: user })
    }, [task])

    const statusUpdate = useCallback((event: any, newStatus: TaskStatus | null) => {
        if (newStatus) setTask({ ...task, status: newStatus })
    }, [task]);


    return (
        <SidePanel>

            {user && <NamedAvatar user={user} />}

            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'space-between'
                }}>

                <Select
                    size="sm"
                    value={task.status}
                    color={getStatusColor(task.status)}
                    variant="soft"
                    sx={{ minWidth: 120 }}
                    onChange={statusUpdate}
                >
                    <Option value={TaskStatus.DONE}>Done!</Option>
                    <Option value={TaskStatus.IN_PROGRESS}>In progress</Option>
                    <Option value={TaskStatus.TODO}>To Do</Option>
                    <Option value={TaskStatus.ARCHIVED}>archived</Option>
                </Select>

                <Box sx={{ display: 'flex', gap: 1, bgcolor: 'inherit' }}>
                    <IconButton onClick={() => setEditMode(!editMode)}>
                        <EditIcon />
                    </IconButton>

                    <IconButton onClick={updateTask} color='success'>
                        <SaveIcon />
                    </IconButton>

                    <IconButton onClick={deleteTask} color='danger'>
                        <DeleteIcon />
                    </IconButton>
                </Box>

            </Box>

            <Typography component='label' level='body3'>
                Assigned user
                <Select
                    disabled={!editMode}
                    size="sm"
                    value={task.userId}
                    variant={editMode ? "soft" : 'plain'}
                    onChange={updateAssignedUser}
                >
                    {
                        (project?.users || []).map((user, index) =>
                            <Option key={index} value={user.id}>
                                {user.email}
                            </Option>
                        )
                    }
                </Select>
            </Typography>

            <EditableTextArea
                title="Title:"
                editable={editMode}
                value={task.name}
                onChange={(value) => setTask({ ...task, name: value })}
            />

            <EditableTextArea
                title="Description:"
                editable={editMode}
                value={task.description}
                onChange={(value) => setTask({ ...task, description: value })}
            />

            <Divider />
            <FilesPanel files={[]} />
        </SidePanel>
    )
}