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




export default function ResourceLoader() {
    const dispatch = useAppDispatch();
    const trigger = useReloadTrigger();

    const loadUsers = useCallback(async () => {
        let users:User[]

        if(process.env.REACT_APP_MOCK_MODE){
            users = mockedUsers
        }else{
            users = await UsersApi.getAll() as unknown as User[];
        }
        dispatch(loadUsersSlice(users));
    }, [dispatch])

    useEffect(() => {
        loadUsers();
    }, [loadUsers, trigger.users])


    const loadProjects = useCallback(async () => {
        let projects:Project[];

        if(process.env.REACT_APP_MOCK_MODE){
            projects = mockedProjects
        }else{
            projects = await ProjectsApi.getAll() as Project[]
        }

        dispatch(loadProjectsSlice(projects))
    }, [dispatch])

    useEffect(() => {
        loadProjects()
    }, [loadProjects, trigger.projects])


    return <></>
}