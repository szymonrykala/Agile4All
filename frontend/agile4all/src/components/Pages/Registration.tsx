import { Input, Typography } from '@mui/joy';
import { useCallback, useState } from 'react';
import NoSessionPage from './NoSessionPage';
import FormWrapper from '../common/FormWrapper';
import { Link, useNavigate } from "react-router-dom";
import { ICreateUserData } from '../../client/users';
import { UsersApi } from '../../client';
import useNotification from '../Notification';



function Registration() {
    const [errorMsg, setError] = useState<string>();
    const [data, setData] = useState<ICreateUserData>({
        firstname: '',
        lastname: '',
        email: '',
        password: '',
    });

    const navigate = useNavigate();
    const { error } = useNotification();


    const submitRegistration: React.FormEventHandler<HTMLFormElement> = useCallback(async (event) => {
        try {
            await UsersApi.register(data);
            navigate('/login');
        } catch (err) {
            setError(String(err));
            error(err)
        }
    }, [navigate, setError, data, error])


    return <NoSessionPage sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: '100'
    }}>
        <FormWrapper
            submitHandler={submitRegistration}
            title='Create an account'
            sx={{ transform: 'translateY(-10vh)' }}
            error={errorMsg}
            footer={
                <Typography component={Link} to='/login' color='primary'>
                    I have an account
                </Typography>
            }
        >
            <Input
                autoFocus
                required
                placeholder='first name'
                type='text'
                value={data.firstname}
                onChange={({ target }) => setData({ ...data, firstname: target.value })}
            />
            <Input
                required
                placeholder='last name'
                type='text'
                value={data.lastname}
                onChange={({ target }) => setData({ ...data, lastname: target.value })}
            />
            <Input
                required
                placeholder='email'
                type='email'
                value={data.email}
                onChange={({ target }) => setData({ ...data, email: target.value })}
            />
            <Input
                required
                placeholder='password'
                type='password'
                value={data.password}
                onChange={({ target }) => setData({ ...data, password: target.value })}
            />
        </FormWrapper>
    </NoSessionPage >
}

export default Registration