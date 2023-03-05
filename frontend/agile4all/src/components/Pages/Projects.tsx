import { Stack } from "@mui/joy";
import { Outlet } from "react-router";
import ParameterBarContextProvider, { ISortItem } from "../ParameterBar/Context";
import Project from "../../models/project";
import ParameterBar from "../ParameterBar";
import ProjectsList from "../Projects/ProjectsList";
import { useCallback } from "react";
import { ProjectsApi } from "../../client";
import AddListItem from "../common/AddListItem";
import { useReloadTrigger } from "../common/ReloadTrigger";



const sorts: ISortItem<Project>[] = [
    {
        name: 'Project name',
        key: 'name'
    },
    {
        name: 'Project Id',
        key: 'id'
    }
]

export default function Projects() {
    const {reload} = useReloadTrigger()

    const createProject = useCallback(async () => {
        const name = prompt('Project name');
        if (name) {
            await ProjectsApi.create({
                name: name,
                description: 'Share a detailed description'
            })
            reload('projects')
        }
    }, [reload]);


    return (
        <ParameterBarContextProvider<Project>>
            <Stack spacing={2} >
                <ParameterBar<Project> sorts={sorts} init={{ sort: 0 }} />
                <Outlet />
                <ProjectsList>
                    <AddListItem onClick={createProject} />
                </ProjectsList>
            </Stack>

        </ParameterBarContextProvider>
    )
}
