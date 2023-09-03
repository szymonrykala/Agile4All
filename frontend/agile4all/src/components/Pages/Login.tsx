import { Input, Typography } from '@mui/joy';
import { useCallback, useState } from 'react';
import NoSessionPage from './NoSessionPage';
import FormWrapper from '../common/FormWrapper';
import { Link, useNavigate } from "react-router-dom";
import { ILoginData } from '../../client/users';
import { UsersApi } from '../../client';
import useNotification from '../Notification';


function Login() {
    const [errorMsg, setError] = useState<string | undefined>();
    const [data, setData] = useState<ILoginData>({
        email: '',
        password: ''
    });

    const navigate = useNavigate();
    const { error } = useNotification();


    const submitLogin: React.FormEventHandler<HTMLFormElement> = useCallback(async (event) => {
        setError(undefined)
        try {
            if(await UsersApi.login(data)){
                navigate(`/app`)
            }
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
            submitHandler={submitLogin}
            title='Login'
            sx={{ transform: 'translateY(-10vh)' }}
            error={errorMsg}
            footer={
                <Typography component={Link} to='/register' color='primary'>
                    Create an account
                </Typography>
            }
        >
            <Input
                autoFocus
                required
                placeholder='email'
                type='email'
                value={data.email}
                onChange={(evt) => setData({ ...data, email: evt.target.value })}
            />
            <Input
                required
                placeholder='password'
                type='password'
                value={data.password}
                onChange={(evt) => setData({ ...data, password: evt.target.value })}
            />
        </FormWrapper>
    </NoSessionPage >
}

export default Login;



