import Box from "@mui/joy/Box"
import Task, { TaskStatus } from "../../models/task"
import { ReactNode, useMemo } from "react"
import { Stack, Typography } from "@mui/joy"
import TaskCard from "../Tasks/TaskCard"


interface ICanbanColumn {
    name: string
    children: ReactNode
}

function CanbanColumn(props: ICanbanColumn) {

    return (
        <Box padding={1}>
            <Box>
                <Typography level="body-sm">
                    {props.name}
                </Typography>
            </Box>
            <Stack spacing={1}>
                {props.children}
            </Stack>
        </Box>
    )
}

interface ICanbanTable {
    tasks: Task[]
}

const STATUS_COLUMNS = [
    TaskStatus.TODO,
    TaskStatus.IN_PROGRESS,
    TaskStatus.DONE,
    TaskStatus.ARCHIVED
]


export default function CanbanTable(props: ICanbanTable) {

    const tasks = useMemo(() => {
        const initStatusMap = Object.values(TaskStatus).reduce((acc: any, status) => {
            acc[status] = [];
            return acc;
        }, {})

        const grouppedTasks = props.tasks.reduce(
            (acc, task) => {
                acc[task.status].push(task);
                return acc;
            },
            initStatusMap
        )

        return grouppedTasks
    }, [props.tasks])

    return (
        <Box sx={{
            display: "grid",
            gridTemplateColumns: {
                md: "repeat(4, minmax(0px,1fr))"
            },
        }}>
            {
                STATUS_COLUMNS.map(status =>
                    <CanbanColumn key={status} name={status}>
                        {
                            tasks[status].map((task:Task) =>
                                <TaskCard task={task} key={`task-${task.id}`} />
                            )
                        }
                    </CanbanColumn>
                )
            }
        </Box>
    )
}