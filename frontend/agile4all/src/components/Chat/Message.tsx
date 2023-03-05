import { Sheet, Typography } from "@mui/joy";
import { Link } from "react-router-dom";
import { ChatMessage } from "../../models/chat";


interface IMessage extends ChatMessage {
    mine?: boolean,
}

export default function Message({ text, mine, sender = '', userId = -1, date }: IMessage) {
    return <Sheet
        color={mine ? "primary" : 'neutral'}
        variant="soft"
        sx={{
            padding: '5px 8px',
            borderRadius: 5,
            width: 'max-content',
            maxWidth: '80%',
            alignSelf: mine ? 'flex-end' : 'flex-start',
            overflowWrap: 'break-word',
            wordBreak: 'break-word',
        }}
    >
        {!mine && <Typography
            level='body2'
            color='neutral'
            component={Link}
            to={`/app/users/${userId}`}
        >
            {sender}
        </Typography>}
        {text}
        <Typography
            fontSize={10}
            color='neutral'
            level='body2'
            sx={{
                textAlign: mine ? 'right' : 'left',
                marginTop: '3px'
            }}
        >
            {date}
        </Typography>
    </Sheet>
}