import { CircularProgress, Sheet, Typography } from "@mui/joy";
import { useEffect } from "react";
import { useNavigate } from "react-router";
import { UsersApi } from "../../client";
import { useAppDispatch } from "../../hooks";
import { removeSession } from "../../store/sessionSlice";
import BasePage from "../common/BasePage";



function Logout() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(removeSession())
        UsersApi.logout()
        navigate('/login')
    }, [navigate, dispatch])

    return <BasePage>
        <Sheet sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            width: '100%',
            height: '100vh',
        }}>
            <CircularProgress size="lg" value={25} />
            <Typography level="h4" component='h2'>
                Logging out
            </Typography>

        </Sheet>
    </BasePage>
}

export default Logout;