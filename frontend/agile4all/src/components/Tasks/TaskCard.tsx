import { Sheet, Typography } from "@mui/joy";
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
            p:2,
            borderRadius:10,
            bgcolor: 'background.componentBg',
            width: '100%',
        }}>
            <Typography level='body2' marginBottom={1}>
                <StatusChip status={task.status} />
                &nbsp;
                <Link to={`/app/users/${task.userId}`}>
                    {`${user?.firstName} ${user?.lastName}`}
                </Link>
            </Typography>

            <Typography
                level='body2'
                sx={{
                    overflow: 'hidden',
                    wordBreak: 'break-word',
                    textOverflow: 'ellipsis',
                }}
                marginBottom={1}
            >
                <Link to={`${task.id}`}>
                    A4A-{task.id}-
                </Link>
                {task.name}
            </Typography>

            <Typography
                level='body2'
                textOverflow='ellipsis'
                overflow='hidden'
                maxHeight='60px'
            >
                {task.description}
            </Typography>
            <br/>
            <Link to={`${task.id}`}>see more...</Link>
        </Sheet>
    )
}