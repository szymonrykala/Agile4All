import Avatar from '@mui/joy/Avatar';
import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';
import ListDivider from '@mui/joy/ListDivider';
import Sheet from '@mui/joy/Sheet';


// Icons import
import Project from '../../../models/project';
import { Link } from 'react-router-dom';
import SmallUsersList from './SmallUsersList';
import TaskListDemo from './TaskListDemo';



interface IProjectItem {
    data: Project
}


export default function ProjectListItem({ data }: IProjectItem) {


    return (
        <Sheet
            component="li"
            sx={{
                bgcolor: 'background.componentBg',
                borderRadius: 2,
                p: 2,
                listStyle: 'none',
            }}
        >
            <Box sx={{ display: 'flex', gap: 2 }}>
                <Avatar
                    title={data.name}
                    src="https://th.bing.com/th/id/R.75f9b714fb48bdf6c7c758dbc7f391e6?rik=BNp%2bziTPLYnx3g&pid=ImgRaw&r=0"
                />
                <Typography component={Link} to={String(data.id)} color='primary'>
                    {data.name}
                </Typography>
            </Box>
            <br />
            <Box>
                <Typography level="body2" sx={{ maxHeight: '200px', overflowY: 'clip' }}>
                    {data.description}
                </Typography>
            </Box>

            <ListDivider component="div" sx={{ my: 1 }} />

            <TaskListDemo projectId={data.id} />

            <ListDivider component="div" sx={{ my: 2 }} />

            <SmallUsersList users={data.users} />

        </Sheet>
    )
}