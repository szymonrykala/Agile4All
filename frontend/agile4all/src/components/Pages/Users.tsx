import { Stack } from "@mui/joy";
import { Outlet } from "react-router";
import User from "../../models/user";
import ParameterBar from "../ParameterBar";
import ParameterBarContextProvider from "../ParameterBar/Context";
import UsersList from "../Users/UsersList";


const sorts = [
    {
        name: 'Role',
        key: 'role'
    },
    {
        name: 'email',
        key: 'email'
    },
    {
        name: 'firstname',
        key: 'firstName'
    },
    {
        name: 'lastname',
        key: 'lastName'
    },
]


const filters = [
    'id', 'email', 'firstName',
    'lastName', 'role'
]


export default function Users() {

    return (
        <ParameterBarContextProvider<User>>
            <Stack spacing={2} >
                <ParameterBar<User> sorts={sorts} filters={filters} init={{ sort: 1, filter: filters.indexOf('email') }} />
                <Outlet />
                <UsersList />
            </Stack>
        </ParameterBarContextProvider>
    )
}