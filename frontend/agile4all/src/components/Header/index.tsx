import MenuIcon from '@mui/icons-material/Menu';
import GroupRoundedIcon from '@mui/icons-material/GroupRounded';
import LogoutIcon from '@mui/icons-material/Logout';
import Box from '@mui/joy/Box';
import Typography from '@mui/joy/Typography';
import IconButton from '@mui/joy/IconButton';
import Divider from '@mui/joy/Divider';
import ColorSchemeToggle from '../SideNav/ColorSchemeToggle';
import { Dispatch, SetStateAction } from 'react';
import { Link } from 'react-router-dom';
import { Session } from '../../models/user';
import MessageIcon from '@mui/icons-material/Message';
import { useChatContext } from '../Chat/Context';




interface IHeader {
    setDrawerOpen?: Dispatch<SetStateAction<boolean>>
    session: Session | null
}


export default function Header(props: IHeader) {
    const { chatOpen, toggleChat } = useChatContext();

    return (
        <>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    alignItems: 'center',
                    gap: 1.5,
                }}
            >
                <IconButton
                    variant="outlined"
                    size="sm"
                    onClick={() => props.setDrawerOpen && props.setDrawerOpen(true)}
                >
                    <MenuIcon />
                </IconButton>
                <IconButton
                    size="sm"
                    variant="solid"
                    sx={{ display: { xs: 'none', sm: 'inline-flex' } }}
                >
                    <GroupRoundedIcon />
                </IconButton>
                <Typography component="h1" fontWeight="xl">
                    Agile4All
                </Typography>
            </Box>

            <Box sx={{ display: 'flex', flexDirection: 'row', gap: 1.5 }}>
                <ColorSchemeToggle />

                {props.session && <>
                    <IconButton
                        variant={chatOpen ? 'soft' : 'outlined'}
                        onClick={toggleChat}
                    >
                        <MessageIcon />
                    </IconButton>

                    <Divider />
                    <IconButton
                        size="sm"
                        variant="outlined"
                        color="danger"
                        component={Link}
                        to="/logout"
                    >
                        <LogoutIcon />
                    </IconButton>
                </>
                }
            </Box>
        </>
    );
}