import { List, ListItem, Tooltip, ListItemButton, Avatar, Typography } from "@mui/joy";
import { Link } from "react-router-dom";
import User from "../../../models/user";


interface ISmallUsersList {
    users: User[]
}


export default function SmallUsersList({ users }: ISmallUsersList) {
    return (
        <>
            <Typography level='body3'>Members:</Typography>
            <List sx={{ flexDirection: 'row' }}>
                {
                    Boolean(users) && users.map((user, index) =>
                        <ListItem key={index} >
                            <Tooltip title={`${user.firstName} ${user.lastName}`}>
                                <ListItemButton component={Link} to={`/app/users/${user.id}`}>
                                    <Avatar
                                        size="sm"
                                        src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
                                    />
                                </ListItemButton>
                            </Tooltip>
                        </ListItem>
                    )
                }
            </List>
        </>
    )
}