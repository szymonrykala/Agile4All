import SendIcon from '@mui/icons-material/Send';
import { IconButton, Input, Sheet } from "@mui/joy";
import { FormEventHandler, RefObject, useCallback, useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../hooks';
import { ChatMessage, WSMessage, WSMessageType } from '../../models/chat';
import { add, leaveMessages } from '../../store/chatSlice';
import { useChatContext } from './Context';


interface ITextBox {
    messagesContainer?: RefObject<HTMLDivElement>
}


export default function TextBox({ messagesContainer }: ITextBox) {
    const dispatch = useAppDispatch();
    const [text, setText] = useState<string>('');

    const { socket } = useChatContext();
    const user = useAppSelector(({ session }) => session?.user);


    const sendMessageToWS = useCallback((data: ChatMessage) => {
        if (!socket?.OPEN) {
            console.info('ws connection is closed.');
            return;
        }
        const mess: WSMessage = {
            type: WSMessageType.MESSAGE,
            payload: data
        }

        socket.send(JSON.stringify(mess))
    }, [socket])


    const scrollChat = useCallback(() =>{
        messagesContainer?.current?.scrollTo(0, Number.MAX_SAFE_INTEGER)
    },
        [messagesContainer]
    )


    const submitMessage: FormEventHandler<HTMLFormElement> = useCallback((event) => {
        event.preventDefault();

        const message: ChatMessage = {
            text: text,
            userId: user?.id || -1,
            date: new Date().toLocaleTimeString(),
            sender: user?.email || 'unknown'
        }
        if (text.length > 0) {
            dispatch(add(message));
            dispatch(leaveMessages(50))
            sendMessageToWS(message)
            setText('');
        }

        setTimeout(scrollChat, 300)
    }, [
        text,
        user,
        dispatch,
        sendMessageToWS,
        scrollChat
    ]);


    useEffect(() => {
        const el = document.getElementById('agile-chat-message')

        el?.addEventListener('click', scrollChat);
        return () => {
            el?.removeEventListener('click', scrollChat);
        }
    }, [scrollChat]);


    return (
        <Sheet
            onSubmit={submitMessage}
            component={'form'}
            sx={{
                display: 'flex',
                alignItems: 'center',
                bgcolor: 'inherit',
                gap: 1,
            }}>

            <Input
                id="agile-chat-message"
                name="message"
                color="neutral"
                placeholder="Type message ..."
                size="lg"
                variant="soft"
                value={text}
                type='text'
                onChange={({ target }) => setText(target.value)}
                sx={{
                    width: '-webkit-fill-available',
                }}
            />

            <IconButton
                type='submit'
                size='lg'
                sx={{ justifySelf: 'flex-end' }}
            >
                <SendIcon />
            </IconButton>
        </Sheet>
    )
}