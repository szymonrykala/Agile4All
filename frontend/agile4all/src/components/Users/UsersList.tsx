import { List, Sheet, Typography } from '@mui/joy';
import { useMemo } from 'react';
import { useAppSelector } from '../../hooks';
import User from '../../models/user';
import { useParameterBarContext } from '../ParameterBar/Context';
import UserListItem from './UserListItem';




export default function UsersList() {
    const { sort, filter } = useParameterBarContext<User>();
    const users = useAppSelector(({ users }) => users);

    const filteredUsers = useMemo(() => {
        let localUsers = [...users];

        if (sort?.key) {
            if (filter?.value) {
                try {
                    const regexp = new RegExp(filter?.value || "", 'ig')
                    localUsers = localUsers.filter(user => String(user[filter.key]).match(regexp))
                } catch{}
            }
            localUsers.sort((u1, u2) => String(u1[sort.key]).localeCompare('' + u2[sort.key]));
            return localUsers
        }
        return localUsers
    }, [sort?.key, users, filter])


    return (
        <Sheet sx={{ padding: 3, bgcolor: 'background.componentBg', borderRadius: 2 }}>
            <Typography fontSize="sm">Members:</Typography>
            <List
                sx={{ '--List-decorator-size': '56px', gap: 1, maxWidth: '600px' }}
            >
                {filteredUsers.map((user, index) => <UserListItem key={index} user={user} />)}
            </List>
        </Sheet>
    )
}
