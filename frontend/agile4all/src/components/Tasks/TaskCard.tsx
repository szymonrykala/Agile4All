import { Box, Sheet, Stack, Typography } from "@mui/joy";
import { useAppSelector } from "../../hooks";
import Project from "../../models/project";
import Task from "../../models/task";
import User from "../../models/user";
import Link from "../common/Link";
import StatusChip from "./StatusChip";


interface ITaskCard {
    task: Task
}


function selectUserOfTask(projects: Project[], task: Task): User | undefined {
    const project = projects.find(({ id }) => id === task.projectId)
    return project?.users.find(({ id }) => id === task.userId)
}

export default function TaskCard({ task }: ITaskCard) {
    const user = useAppSelector(({ projects }) => selectUserOfTask(projects, task))

    return (
        <Sheet sx={{
            p: 1,
            borderRadius: 10,
            bgcolor: 'background.componentBg',
            width: '100%',
        }}>
            <Stack spacing={0.8}>

                <Box display="flex" justifyContent="space-between">
                    <Link to={`${task.id}`}>A4A-{task.id}</Link>
                    <StatusChip status={task.status} />
                </Box>

                <Typography
                    level='body2'
                    sx={{
                        overflow: 'hidden',
                        wordBreak: 'break-word',
                        textOverflow: 'ellipsis',
                    }}
                >
                    {task.name}
                </Typography>

                <span>
                    <Typography
                        level='body2'
                        textOverflow='ellipsis'
                        overflow='hidden'
                        maxHeight='60px'
                    >
                        {task.description}
                    </Typography>
                    <Link sx={{ fontSize: 13 }} to={`${task.id}`}>see more...</Link>
                </span>

                <span>
                    <small>asignee:&nbsp;</small>
                    <Link sx={{ fontSize: 14 }} to={`user-lookup/${task.userId}`}>
                        {`${user?.firstName} ${user?.lastName}`}
                    </Link>
                </span>
            </Stack>
        </Sheet>
    )
}