import { Avatar, ListItem, ListItemButton, ListItemContent, ListItemDecorator, Typography } from '@mui/joy';
import User from '../../models/user';
import { Link } from 'react-router-dom';



interface IUserListItem {
    user: User
}

export default function UserListItem({ user }: IUserListItem) {

    return (
        <ListItem>
            <ListItemButton component={Link} to={`${user.id}`}>
                <ListItemDecorator sx={{ alignSelf: 'flex-start' }}>
                    <Avatar src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png" />
                </ListItemDecorator>
                <ListItemContent>
                    <Typography>{user.firstName} {user.lastName}</Typography>
                    <Typography level="body2" noWrap>
                        {user.role}&nbsp;|&nbsp;{user.email}
                    </Typography>
                </ListItemContent>
            </ListItemButton>
        </ListItem>

    )
}