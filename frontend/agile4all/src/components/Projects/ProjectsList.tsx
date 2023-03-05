import { List } from '@mui/joy';
import { ReactNode, useMemo } from 'react';
import { useAppSelector } from '../../hooks';
import Project from '../../models/project';
import { useParameterBarContext } from '../ParameterBar/Context';
import ProjectListItem from './ProjectListItem';


interface IProjectList {
    children?: ReactNode
}

export default function ProjectsList({ children }: IProjectList) {
    const { sort } = useParameterBarContext<Project>();
    const projects: Project[] = useAppSelector(({ projects }) => projects);

    const sortedProjests = useMemo(() => {
        const localProjects = [...projects];

        if (sort?.key) {
            return localProjects.sort();
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
            {children}
        </List>
    )
}
