import { useCallback, useEffect } from "react";
import { ProjectsApi, UsersApi } from "../../client";
import { useAppDispatch } from "../../hooks";
import Project from "../../models/project";
import User from "../../models/user";
import { load as loadUsersSlice } from "../../store/usersSlice";
import { load as loadProjectsSlice } from "../../store/projectSlice";
import { useReloadTrigger } from "./ReloadTrigger";
import { mockedProjects } from "../../client/mocks/project";
import { mockedUsers } from "../../client/mocks/user";
import useNotification from "../Notification";




export default function ResourceLoader() {
    const { error } = useNotification();
    const dispatch = useAppDispatch();
    const trigger = useReloadTrigger();

    const loadUsers = useCallback(async () => {
        let users: User[]

        try {
            if (process.env.NODE_ENV === "development" && process.env.REACT_APP_MOCK_API === "true") {
                users = mockedUsers
            } else {
                users = await UsersApi.getAll() as unknown as User[];
            }
            dispatch(loadUsersSlice(users));
        } catch {
            error("Could not get users list");
        }
    }, [dispatch, error])

    const loadProjects = useCallback(async () => {
        let projects: Project[];

        try{
            if (process.env.NODE_ENV === "development" && process.env.REACT_APP_MOCK_API === "true") {
                projects = mockedProjects
            } else {
                projects = await ProjectsApi.getAll() as Project[]
            }
            
            dispatch(loadProjectsSlice(projects))
        }catch{
            error("Could not get projects list");
        }
    }, [dispatch, error])


    useEffect(() => {
        loadProjects()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [loadProjects, trigger.projects])

    useEffect(() => {
        loadUsers();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [loadUsers, trigger.users])


    return <></>
}