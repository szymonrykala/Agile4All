import { List, ListItem, ListItemButton, ListItemContent, Typography, Button } from "@mui/joy";
import { Link } from "react-router-dom";
import { UUID } from "../../../models/common";
import KeyboardArrowRightRoundedIcon from '@mui/icons-material/KeyboardArrowRightRounded';
import Task, { taskStatusSort } from "../../../models/task";
import { useAppSelector } from "../../../hooks";
import StatusChip from "../../Tasks/StatusChip";


interface ITaskListDemo {
    projectId: UUID
}


function get3Tasksfilter(tasks: Task[], projectId: UUID) {
    const selected: Task[] = []

    for (const task of [...tasks].sort(taskStatusSort)) {
        if (task.projectId === projectId) {
            selected.push(task)
        }
        if (selected.length >= 3) break;
    }
    return selected;
}


export default function TaskListDemo(props: ITaskListDemo) {
    const tasks = useAppSelector(({ tasks }) => get3Tasksfilter(tasks, props.projectId))

    return (
        <>
            <List size='sm'>
                {
                    tasks.map(({ name, id, status }, index) =>
                        <ListItem key={index} sx={{ alignItems: 'flex-start' }}>
                            <ListItemButton component={Link} to={`${props.projectId}/task-lookup/${id}`}>
                                <ListItemContent>
                                    <Typography level="body-md" sx={{
                                        textOverflow: "ellipsis",
                                        overflow: 'hidden',
                                        whiteSpace: 'nowrap'
                                    }}>
                                        {id}&nbsp;-&nbsp;{name}
                                    </Typography>
                                </ListItemContent>
                                <StatusChip status={status} variant='soft' />
                            </ListItemButton>
                        </ListItem>
                    )
                }
            </List>
            <Button
                size="sm"
                variant="plain"
                endDecorator={<KeyboardArrowRightRoundedIcon fontSize="small" />}
                component={Link}
                to={`/app/tasks/projects/${props.projectId}`}
            >
                More tasks
            </Button>
        </>
    )
}
