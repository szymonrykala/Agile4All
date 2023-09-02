import InfoIcon from '@mui/icons-material/Info';
import ReportIcon from '@mui/icons-material/Report';
import { Alert, IconButton, Typography } from '@mui/joy';
import CloseRoundedIcon from '@mui/icons-material/CloseRounded';
import { useMemo } from 'react';


export enum NotificationType {
    INFO = "info",
    ERROR = "error"
}

export interface INotification {
    text: string
    type: NotificationType
    onClose():void
}

function getIcon(type: NotificationType) {
    switch (type) {
        case NotificationType.INFO:
            return <InfoIcon />
        case NotificationType.ERROR:
            return <ReportIcon />
    }
}

function getColor(type: NotificationType) {
    switch (type) {
        case NotificationType.INFO:
            return "neutral"
        case NotificationType.ERROR:
            return "danger"
    }
}

const styles = {
    position: "absolute",
    bottom: "5%",
    left: "5%",
    zIndex: 999999,
    minWidth: "200px",
    opacity: 0.8,
    transition: "0.3s ease-out",
    "&:hover": {
        opacity: 1
    }
}


export default function Notification(props: INotification) {
    const color = useMemo(() => getColor(props.type), [props.type])

    return <Alert
        sx={styles}
        startDecorator={getIcon(props.type)}
        variant="soft"
        color={color}
        endDecorator={
            <IconButton variant="soft" color={color} onClick={props.onClose}>
                <CloseRoundedIcon />
            </IconButton>
        }
    >
        <Typography level="body-sm" color={color}>
            {props.text}
        </Typography>
    </Alert>
}