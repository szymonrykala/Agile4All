import { List, Stack, Typography } from "@mui/joy";
import Link from "../common/Link";
import CollapsibleProjectCanbanBoard from "../common/CollapsibleListItem";
import { Outlet, useLocation, useParams } from "react-router";
import ParameterBar from "../ParameterBar";
import { useCallback, useEffect, useMemo } from "react";
import { TasksApi } from '../../client';
import Task from "../../models/task";
import { useAppDispatch, useAppSelector } from "../../hooks";
import { UUID } from "../../models/common";
import { load } from "../../store/taskSlice";
import { ICreateTaskData } from "../../client/tasks";
import ParameterBarContextProvider, { ISortItem, useParameterBarContext } from "../ParameterBar/Context";
import AddListItem from "../common/AddListItem";
import { useReloadTrigger } from "../common/ReloadTrigger";
import { QueryParams } from "../../client/interface";
import Project from "../../models/project";
import { mockedTasks } from "../../client/mocks/tasks";
import CanbanTable from "../CanbanTable";


interface IProjectTasksListItem {
    project: {
        name: string,
        id: UUID
    }
}

function tasksReduxSelector(tasks: Task[], queryParams: QueryParams, currentPath: string) {
    let filtered = tasks;

    if (currentPath.includes("/tasks/users")) {
        filtered = filtered.filter(({ userId }) => userId === Number(queryParams.userId))

    }

    if (currentPath.includes("/tasks/projects")) {
        filtered = filtered.filter(({ projectId }) => projectId === Number(queryParams.projectId))
    }

    return filtered
}

function ProjectTasksListItem({ project }: IProjectTasksListItem) {
    const location = useLocation();
    const trigger = useReloadTrigger();
    const queryParams = useParams();
    const dispatch = useAppDispatch();
    const { filter } = useParameterBarContext<Task>();

    const sessionUser = useAppSelector(({ session }) => session?.user);


    const tasks = useAppSelector(({ tasks }) => tasksReduxSelector(
        tasks.filter(({ projectId }) => projectId === project.id),
        queryParams,
        location.pathname
    ));


    const getTasks = useCallback(async () => {
        try {
            let taskItems: Task[];

            if (process.env.REACT_APP_MOCK_MODE) {
                taskItems = mockedTasks
            } else {
                taskItems = await TasksApi.getAll(queryParams) as unknown as Task[]
            }

            dispatch(load(taskItems));

        } catch (err) {
            console.debug(err);
        }
    }, [queryParams, dispatch]);


    const filteredTasks = useMemo(() => {
        if (filter?.value) {
            try {
                const regexp = new RegExp(filter?.value || "", 'ig')
                return tasks.filter(task => String(task[filter.key]).match(regexp))
            } catch { }
        }
        return tasks
    }, [filter, tasks])


    useEffect(() => {
        getTasks();
    }, [getTasks, trigger.tasks])

    const createTask = useCallback(async (projectId: UUID) => {
        const title = prompt('type a task title');

        const taskUserId = queryParams.userId ? Number(queryParams.userId) : sessionUser?.id || 1;

        if (title) {
            const task: ICreateTaskData = {
                name: title,
                description: "What needs to be done??",
                userId: taskUserId,
                projectId: projectId
            }
            await TasksApi.create(task);
            trigger.reload('tasks');
        }
    }, [queryParams.userId, sessionUser?.id, trigger]);


    return (
        <CollapsibleProjectCanbanBoard
            open={true}
            header={
                <Typography
                    component={Link}
                    to={`project-lookup/${project.id}`}
                >
                    {project.name}
                </Typography>
            }
            buttons={< AddListItem key={'4o5689'} onClick={() => createTask(project.id)} />}
        >
            <CanbanTable tasks={filteredTasks} />
        </CollapsibleProjectCanbanBoard>
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



function projectsReduxSelector(projects: Project[], queryParams: QueryParams, currentPath: string) {
    let filtered = projects;

    if (currentPath.includes("/tasks/users")) {
        filtered = filtered.filter(({ users }) =>
            users.map(({ id }) => id).includes(Number(queryParams.userId))
        );
    }

    if (currentPath.includes("/tasks/projects") || currentPath.match(/users\/\d+\/projects\/\d+/)) {
        filtered = filtered.filter(({ id }) => id === Number(queryParams.projectId));
    }

    return filtered;
}


export default function Tasks() {
    const queryParams = useParams();
    const location = useLocation();

    const projects = useAppSelector(({ projects }) =>
        projectsReduxSelector(projects, queryParams, location.pathname)
    );

    return (
        <ParameterBarContextProvider<Task>>
            <Stack spacing={2} >
                <ParameterBar<Task> sorts={sorts} filters={filters} init={{ filter: 0, sort: 0 }} />
                <Outlet />

                <List>
                    {projects?.length === 0 ?
                        <Typography textAlign='center'>
                            Create a project to see it tasks
                        </Typography>
                        :
                        projects && projects.map((project, index) =>
                            <ProjectTasksListItem project={project} key={`tl${index}`} />
                        )
                    }
                </List>
            </Stack>
        </ParameterBarContextProvider >
    )
}