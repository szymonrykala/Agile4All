import { Avatar, Link as MUILink, Sheet, Typography } from "@mui/joy";
import User from "../../models/user";
import Link from "../common/Link";


interface INamedAvatar {
    user: User
}

export default function NamedAvatar({ user }: INamedAvatar) {
    return (
        <Sheet sx={{
            display: 'flex',
            gap: 1,
            bgcolor: 'inherit'
        }}>
            <Avatar
                alt="Assigned User"
                src="https://cdn-icons-png.flaticon.com/512/2202/2202112.png"
            />
            <Sheet sx={{ bgcolor: 'inherit' }}>
                <Typography level='body-md'>
                    <Link to={`/app/users/${user?.id}`}>
                        {user?.firstName} {user?.lastName}
                    </Link>
                </Typography>
                <Typography level='body-sm'>
                    <MUILink href={`email:${user?.email}`}>
                        {user?.email}
                    </MUILink>
                </Typography>
            </Sheet>
        </Sheet>
    )
}