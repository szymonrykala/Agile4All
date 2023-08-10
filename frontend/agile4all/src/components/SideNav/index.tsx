import List from '@mui/joy/List';
import ListItem from '@mui/joy/ListItem';
import ListItemButton from '@mui/joy/ListItemButton';
import ListItemDecorator from '@mui/joy/ListItemDecorator';
import ListItemContent from '@mui/joy/ListItemContent';
import PeopleRoundedIcon from '@mui/icons-material/PeopleRounded';
import AssignmentIndRoundedIcon from '@mui/icons-material/AssignmentIndRounded';
import { Link, useLocation, useResolvedPath } from 'react-router-dom';
import { useState } from 'react';
import { useAppSelector } from '../../hooks';
import TaskIcon from '@mui/icons-material/Task';
import { KeyboardArrowDown, KeyboardArrowRight, KeyboardArrowUp } from '@mui/icons-material';
import ListSubheader from '@mui/joy/ListSubheader';


const clicked = {
  variant: "soft",
  color: "primary"
}


function ProjectsTasksListsItem() {
  const [folded, setFolded] = useState(false);

  const loc = useLocation();
  const path = useResolvedPath(loc);
  const session = useAppSelector(({ session }) => session)

  return (
    <ListItem nested>
      <ListItemButton onClick={() => setFolded(!folded)}>
        <ListItemDecorator>
          <TaskIcon />

        </ListItemDecorator>
        <ListItemContent>
          Tasks
        </ListItemContent>
        {folded ? <KeyboardArrowDown /> : <KeyboardArrowUp />}
      </ListItemButton>
      <List sx={{
        height: folded ? "0px" : "unset",
        overflow: "hidden"
      }}>
        {session?.projects ?
          [
            session.projects.map(project =>
              <ListItem key={project.id}>
                <ListItemButton
                  {...path.pathname.includes(`/app/tasks/users/${session.user.id}/projects/${project.id}`) ? clicked : Object()}
                  component={Link} to={`/app/tasks/users/${session.user.id}/projects/${project.id}`}
                >
                  <ListItemDecorator sx={{ color: 'inherit' }}>
                    <KeyboardArrowRight fontSize="small" />
                  </ListItemDecorator>
                  <ListItemContent>{project.name}</ListItemContent>
                </ListItemButton>
              </ListItem>
            ),
            <ListItem key="all_tasks_item">
              <ListItemButton
                {...path.pathname === `/app/tasks/users/${session.user.id}` ? clicked : Object()}
                component={Link} to={`/app/tasks/users/${session.user.id}`}
              >
                <ListItemDecorator sx={{ color: 'inherit' }}>
                  <KeyboardArrowRight fontSize="small" />
                </ListItemDecorator>
                <ListItemContent>All tasks</ListItemContent>
              </ListItemButton>
            </ListItem>
          ]
          :
          <ListItem>
            <small><i>No projects assigned yet ...</i></small>
          </ListItem>
        }
      </List>
    </ListItem>
  )
}


const navLinks = [
  {
    name: 'Projects',
    link: '/app/projects',
    Icon: AssignmentIndRoundedIcon
  }, {
    name: 'Users',
    link: '/app/users',
    Icon: PeopleRoundedIcon
  }
];

export default function SideNav() {
  const loc = useLocation();
  const path = useResolvedPath(loc);


  return (
    <List
      sx={{
        '--List-nestedInsetStart': '13px',
      }}
      size='md'
    >
      <ListSubheader sx={{
        textColor: "neutral.500",
        fontSize: '10px'
      }}>
        Menu
      </ListSubheader>

      <ProjectsTasksListsItem />

      {
        navLinks.map(({ link, name, Icon }) => (
          <ListItem key={name}>
            <ListItemButton
              {...path.pathname === link ? clicked : Object()}
              component={Link} to={link}
            >
              <ListItemDecorator sx={{ color: 'inherit' }}>
                <Icon fontSize="small" />
              </ListItemDecorator>
              <ListItemContent>{name}</ListItemContent>
            </ListItemButton>
          </ListItem>
        ))
      }
    </List>
  );
}