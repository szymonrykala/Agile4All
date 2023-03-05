import { Button, List, Typography } from "@mui/joy";
import Link from "../common/Link";
import TaskCard from "../Tasks/TaskCard";
import CollapsibleListItem from "../common/CollapsibleListItem";
import { Outlet, useParams } from "react-router";
import ParameterBar from "../ParameterBar";
import { useCallback, useEffect, useMemo, useState } from "react";
import { TasksApi } from '../../client';
import Task, { TaskStatus, taskStatusSort } from "../../models/task";
import { useAppDispatch, useAppSelector } from "../../hooks";
import { UUID } from "../../models/common";
import { load } from "../../store/taskSlice";
import { ICreateTaskData } from "../../client/tasks";
import ParameterBarContextProvider, { ISortItem, useParameterBarContext } from "../ParameterBar/Context";
import AddListItem from "../common/AddListItem";
import { useReloadTrigger } from "../common/ReloadTrigger";
import { QueryParams } from "../../client/interface";
import Project from "../../models/project";


interface IProjectTasksListItem {
    project: {
        name: string,
        id: UUID
    }
}

function selectTasks(tasks: Task[], queryParams: QueryParams) {
    if (queryParams?.projectId) {
        return tasks.filter(({ projectId }) => projectId === Number(queryParams.projectId))

    } else if (queryParams?.userId) {
        return tasks.filter(({ userId }) => userId === Number(queryParams.userId))
    } else {
        return []
    }
}

function ProjectTasksListItem({ project }: IProjectTasksListItem) {
    const trigger = useReloadTrigger()
    const queryParams = useParams();
    const dispatch = useAppDispatch();
    const { filter } = useParameterBarContext<Task>();
    const [hideArchived, sethideArchived] = useState<boolean>(false)

    const sessionUser = useAppSelector(({ session }) => session?.user)

    const tasks = useAppSelector(({ tasks }) => selectTasks(
        tasks.filter(({ projectId }) => projectId === project.id),
        queryParams
    ));

    const sortedTasks = useMemo(() => {
        let localTasks = tasks;
        if (hideArchived) {
            localTasks = localTasks.filter(({ status }) => status !== TaskStatus.ARCHIVED)
        }
        localTasks.sort(taskStatusSort)
        if (filter && filter.value) {
            return localTasks.filter((task) => String(task[filter.key]).match(RegExp(filter.value || '', 'gi')))
        }
        return localTasks
    }, [tasks, filter, hideArchived])

    const getTasks = useCallback(async () => {
        try {
            const taskItems: Task[] = await TasksApi.getAll(queryParams) as unknown as Task[]
            dispatch(load(taskItems))

        } catch (err) {
            console.debug(err)
        }
    }, [queryParams, dispatch])


    useEffect(() => {
        getTasks()
    }, [getTasks, trigger.tasks])

    const createTask = useCallback(async (projectId: UUID) => {
        const title = prompt('type a task title');

        const taskUserId = queryParams.userId ? Number(queryParams.userId) : sessionUser?.id || 1

        if (title) {
            const task: ICreateTaskData = {
                name: title,
                description: "What needs to be done??",
                userId: taskUserId,
                projectId: projectId
            }
            await TasksApi.create(task)
            trigger.reload('tasks')
        }
    }, [sessionUser?.id, trigger]);


    const renderedTasks = useMemo(() => {
        const tasks = sortedTasks.map((task, index) => <TaskCard task={task} key={`task-${index}`} />)
        tasks.push(< AddListItem key={'4o5689'} component='div' onClick={() => createTask(project.id)} />)
        return tasks;
    }, [sortedTasks, project.id, createTask])


    return (
        <CollapsibleListItem
            open={true}
            header={
                <>
                    <Typography
                        component={Link}
                        to={`/app/projects/${project.id}`}
                    >
                        {project.name}
                    </Typography>&nbsp;&nbsp;
                    <Button size='sm' variant={hideArchived ? "soft" : "outlined"} onClick={() => sethideArchived(!hideArchived)}>
                        hide archived
                    </Button>
                </>
            }
        >
            {renderedTasks}
        </CollapsibleListItem>
    )
}

const filters = [
    'description', 'name', 'status', 'id'
]

const sorts: ISortItem<Task>[] = [
    {
        name: 'Project',
        key: 'projectId',
    }
]


function selectProjects(projects: Project[], queryParams: QueryParams) {
    if (queryParams?.projectId) {
        return projects.filter(({ id }) => id === Number(queryParams.projectId))

    } else if (queryParams?.userId) {
        return projects.filter(({ users }) =>
            users.map(({ id }) => id).includes(Number(queryParams.userId))
        )
    } else {
        return []
    }
}


export default function Tasks() {
    const queryParams = useParams();
    // const sessionUser = useAppSelector(({ session }) => session?.user)
    const projects = useAppSelector(({ projects }) =>
        selectProjects(projects, queryParams)
    );




    return (
        <ParameterBarContextProvider<Task>>
            <ParameterBar<Task> sorts={sorts} filters={filters} init={{ filter: 0, sort: 0 }} />
            <Outlet />
            <List>
                {projects?.length === 0 ?
                    <Typography p='50px 0px' textAlign='center'>
                        Create a project to see it tasks
                    </Typography>
                    :
                    projects && projects.map((project, index) =>
                        <ProjectTasksListItem project={project} key={`tl${index}`} />
                    )
                }
            </List>
        </ParameterBarContextProvider >
    )
}