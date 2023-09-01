import { Typography } from "@mui/joy";
import { useMemo } from "react";
import { UsersApi } from "../../client";
import { useAppSelector } from "../../hooks";
import ChatContainer from "./ChatContainer";
import Message from "./Message";


function Chat() {
    const messages = useAppSelector(({ chat }) => chat);
    const userId = useMemo(() => UsersApi.getSavedUserId(), []);

    return (
        <ChatContainer>
            {messages.length === 0 ?
                <Typography textAlign='center' level='body-md'>
                    No messages ...
                </Typography> :
                <>
                    {
                        messages.map((message, index) =>
                            <Message {...message}
                                mine={userId === message.userId}
                                key={index}
                            />
                        )
                    }
                </>
            }
        </ChatContainer>
    );
}

export default Chat;