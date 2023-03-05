import UsersClient from './users'
import TasksClient from './tasks'
import ProjectsClient from './projects';
import FilesClient from './files';


export const UsersApi =  new UsersClient();
export const TasksApi =  new TasksClient();
export const ProjectsApi = new ProjectsClient();
export const FilesApi = new FilesClient();