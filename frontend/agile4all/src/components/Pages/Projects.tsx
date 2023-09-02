import { Stack } from "@mui/joy";
import { Outlet } from "react-router";
import ParameterBarContextProvider, { ISortItem } from "../ParameterBar/Context";
import Project from "../../models/project";
import ParameterBar from "../ParameterBar";
import ProjectsList from "../Projects/ProjectsList";


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

    return (
        <ParameterBarContextProvider<Project>>
            <Stack spacing={1} p={1} >
                <ParameterBar<Project> sorts={sorts} init={{ sort: 0 }} />
                <Outlet />
                <ProjectsList />
            </Stack>
        </ParameterBarContextProvider>
    )
}
