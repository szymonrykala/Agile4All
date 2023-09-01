import CloseIcon from '@mui/icons-material/Close';
import MinimizeIcon from '@mui/icons-material/Minimize';
import { Divider, IconButton, Sheet, Typography } from "@mui/joy";
import { ReactNode, useRef, useState } from "react";
import { useChatContext } from './Context';
import TextBox from './TextBox';



interface IChatContainer {
    children: ReactNode
}

const maxWidth = { md: '450px', sx: undefined }

export default function ChatContainer({ children }: IChatContainer) {
    const { chatOpen, toggleChat } = useChatContext()
    const [minimized, setMinimized] = useState<boolean>(false)

    const messagesContainer = useRef(null);

    return chatOpen ? (
        <Sheet
            variant='outlined'
            sx={{
                zIndex: 9999,
                width: '93%',
                maxWidth: maxWidth,
                maxHeight: { md: '550px', sm: undefined },
                position: 'fixed',
                bottom: '0px',
                right: '2vw',
                borderRadius: 5,
                padding: 1,
                bgcolor: 'background.componentBg',
                gap: 1,
                display: 'grid',
                ...minimized ? {} : {
                    height: '92vh',
                    gridTemplateRows: {
                        xs: '35px 1px 1fr 65px'
                    }
                },
            }}
        >
            <Sheet sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                bgcolor: 'inherit'
            }}>
                <Typography level='title-md' component='h2'>
                    Agile Chat
                </Typography>
                <span>
                    <IconButton
                        size='sm'
                        variant={minimized ? 'solid' : 'soft'}
                        onClick={() => setMinimized(!minimized)}
                    >
                        <MinimizeIcon />
                    </IconButton>
                    &nbsp;
                    <IconButton
                        size='sm'
                        color='danger'
                        onClick={toggleChat}
                    >
                        <CloseIcon />
                    </IconButton>
                </span>
            </Sheet>
            {!minimized && <>
                <Divider />
                <Sheet
                    ref={messagesContainer}
                    sx={{
                        display: 'flex',
                        flexFlow: 'column',
                        gap: '3px',
                        maxWidth: maxWidth,
                        bgcolor: 'inherit',
                        overflowY: 'auto'
                    }}>
                    {children}
                </Sheet>
                <TextBox messagesContainer={messagesContainer} />
            </>}
        </Sheet>
    ) : <></>
}

