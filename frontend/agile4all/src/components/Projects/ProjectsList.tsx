import { List, ListItem } from '@mui/joy';
import { useCallback, useMemo } from 'react';
import { useAppSelector, useCheckAdmin } from '../../hooks';
import Project from '../../models/project';
import { useParameterBarContext } from '../ParameterBar/Context';
import ProjectListItem from './ProjectListItem';
import { ProjectsApi } from '../../client';
import AddListItem from '../common/AddListItem';
import { useReloadTrigger } from '../common/ReloadTrigger';
import useNotification from '../Notification';


interface IProjectList { }

export default function ProjectsList(props: IProjectList) {
    const { info, error } = useNotification();
    const { sort } = useParameterBarContext<Project>();
    const projects: Project[] = useAppSelector(({ projects }) => projects);

    const { reload } = useReloadTrigger()
    const isAdmin = useCheckAdmin()


    const createProject = useCallback(async () => {
        const name = prompt('Project name');
        if (name) {
            try{
                await ProjectsApi.create({
                    name: name,
                    description: 'Share a detailed description'
                })
                reload('projects');
                info("Projekt dodany");
            }catch(err){
                error(err)
            }
        }
    }, [reload, error, info]);


    const sortedProjests = useMemo(() => {
        const localProjects = [...projects];

        if (sort?.key) {
            return localProjects.sort((a, b) => String(a[sort.key]).localeCompare(b[sort.key] as any));
        } else {
            return projects
        }
    }, [sort?.key, projects])


    return (
        <List
            sx={{
                display: 'grid',
                gridTemplateColumns: 'repeat(auto-fill, minmax(350px, 1fr))',
                gap: 2,
            }}
        >
            {sortedProjests.map((project, index) => <ProjectListItem key={index} data={project} />)}
            <ListItem>
                {
                    isAdmin && <AddListItem onClick={() => createProject()} size="lg" sx={{ margin: "0px auto" }} />
                }
            </ListItem>
        </List>
    )
}
