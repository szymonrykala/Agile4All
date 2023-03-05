import { useCallback, useEffect } from "react";
import { ProjectsApi, UsersApi } from "../../client";
import { useAppDispatch } from "../../hooks";
import Project from "../../models/project";
import User from "../../models/user";
import { load as loadUsersSlice } from "../../store/usersSlice";
import { load as loadProjectsSlice } from "../../store/projectSlice";
import { useReloadTrigger } from "./ReloadTrigger";




export default function ResourceLoader() {
    const dispatch = useAppDispatch();
    const trigger = useReloadTrigger();

    const loadUsers = useCallback(async () => {
        const users = await UsersApi.getAll() as unknown as User[];
        dispatch(loadUsersSlice(users));
    }, [dispatch])

    useEffect(() => {
        loadUsers();
    }, [loadUsers, trigger.users])


    const loadProjects = useCallback(async () => {
        const projects = await ProjectsApi.getAll() as Project[]
        dispatch(loadProjectsSlice(projects))
    }, [dispatch])

    useEffect(() => {
        loadProjects()
    }, [loadProjects, trigger.projects])

    
    return <></>
}