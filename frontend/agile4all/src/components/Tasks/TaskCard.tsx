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
        }}>
            <Stack spacing={0.5}>

                <Box display="flex" justifyContent="space-between">
                    <Link to={`task-lookup/${task.id}`}>A4A-{task.id}</Link>
                    <StatusChip status={task.status}/>
                </Box>

                <Typography
                    level='body-sm'
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
                        level='body-sm'
                        textOverflow='ellipsis'
                        overflow='hidden'
                        maxHeight='40px'
                    >
                        {task.description}
                    </Typography>
                    <Link sx={{ fontSize: 13 }} to={`task-lookup/${task.id}`}>see more...</Link>
                </span>

                <span>
                    <small>asignee:&nbsp;</small>
                    <Link sx={{ fontSize: 14 }} to={`task-lookup/${task.id}`}>
                        {`${user?.firstName} ${user?.lastName}`}
                    </Link>
                </span>
            </Stack>
        </Sheet>
    )
}