import { Box, Avatar, Typography, IconButton, Button, Sheet } from "@mui/joy";
import { Link, useNavigate, useParams } from "react-router-dom";
import FilesPanel from "../FilesPanel";
import EditIcon from "@mui/icons-material/Edit";
import SaveIcon from "@mui/icons-material/Save";
import DeleteIcon from "@mui/icons-material/Delete";
import { useCallback, useEffect, useState } from "react";
import EditableTextField from "../common/EditableTextField";
import EditableTextArea from "../common/EditableTextArea";
import Project from "../../models/project";
import ProjectUsersList from "./ProjectUsersList";
import { useAppDispatch, useAppSelector, useCheckAdmin } from "../../hooks";
import { ProjectsApi } from "../../client";
import { remove, update } from "../../store/projectSlice";
import SmallUsersList from "./ProjectListItem/SmallUsersList";
import SidePanel from "../common/SidePanel";



const demoProject: Project = {
    id: -1,
    name: 'loading...',
    description: 'loading...',
    users: []
}


export default function ProjectPanel() {
    const [editMode, setEditMode] = useState<boolean>(false);

    const { projectId } = useParams();
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const isAdmin = useCheckAdmin();

    const reduxProject: Project = useAppSelector(({ projects }) =>
        projects.find(({ id }) => id === Number(projectId)
        )) || demoProject;

    const [project, setProject] = useState<Project>(reduxProject)


    const saveProject = useCallback(async () => {
        if ((!editMode) || (JSON.stringify(project) === JSON.stringify(reduxProject))) return;
        try {
            await ProjectsApi.update(project.id, {
                name: project.name,
                description: project.description
            })
            dispatch(update(project));
        } catch {
            setProject(reduxProject);
        }
    }, [project, dispatch, editMode, reduxProject])


    const deleteProject = useCallback(async () => {
        await ProjectsApi.delete(project.id)
        dispatch(remove(project))
        navigate('../')
    }, [project, dispatch, navigate])


    useEffect(() => {
        setProject({ ...reduxProject })
        // displayed project should be replaced only if `projectId` was changed
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [projectId]);


    return (
        <SidePanel>
            <Box sx={{ display: 'flex', gap: 2 }}>
                <Avatar
                    title={project.name}
                    src="https://th.bing.com/th/id/R.75f9b714fb48bdf6c7c758dbc7f391e6?rik=BNp%2bziTPLYnx3g&pid=ImgRaw&r=0"
                    sx={{ borderRadius: 'sm' }}
                />
                <Typography component={Link} to='.' color='primary'>
                    {project.name}
                </Typography>
            </Box>

            <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end' }}>
                <Button
                    size='sm'
                    variant="soft"
                    component={Link}
                    to={`/app/tasks/projects/${project.id}`}
                >
                    Show tasks
                </Button>
                {
                    isAdmin && <>
                        <IconButton onClick={() => setEditMode(!editMode)}>
                            <EditIcon />
                        </IconButton>

                        <IconButton onClick={saveProject} color='success'>
                            <SaveIcon />
                        </IconButton>

                        <IconButton onClick={deleteProject} color='danger'>
                            <DeleteIcon />
                        </IconButton>
                    </>
                }
            </Box>

            <EditableTextField
                title='Project name'
                value={project.name}
                editable={editMode}
                onChange={(text) => setProject({ ...project, name: text })}
                size='md'
            />

            <EditableTextArea
                title="Description:"
                editable={editMode}
                value={project.description}
                onChange={(value) => setProject({ ...project, description: value })}
            />

            <Sheet>
                {isAdmin ?
                    <ProjectUsersList projectId={Number(projectId)} users={project.users} />
                    : <SmallUsersList users={project.users} />
                }
            </Sheet>

            <FilesPanel projectId={project?.id} />
        </SidePanel>
    )
}
