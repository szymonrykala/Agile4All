import { Button, Sheet, Typography } from "@mui/joy";
import { confirm as rawConfirm } from "react-confirm-box";

interface IConfirmBox {
    children: string,
    onConfirm: () => void,
    onCancel: () => void
}


function ConfirmBox(props: IConfirmBox) {
    return (
        <Sheet sx={{
            padding: 3,
            borderRadius: 7,
            display: 'flex',
            flexDirection: 'column',
            gap: 3,
            zIndex: 9999,
            transform: 'translateY(-9vh)',
            bgcolor: 'background.componentBg',
        }}>
            <Typography>
                {props.children}
            </Typography>
            <Sheet sx={{
                display: 'flex',
                justifyContent: 'space-evenly',
                bgcolor: 'inherit'
            }}>
                <Button
                    variant="soft"
                    onClick={props.onConfirm}
                >
                    OK
                </Button>
                <Button
                    variant="soft"
                    onClick={props.onCancel}
                >
                    Cancel
                </Button>
            </Sheet>
        </Sheet>
    )
}

const options = {
    render: (message: string, onConfirm: () => void, onCancel: () => void) => {
        return <ConfirmBox
            onConfirm={onConfirm}
            onCancel={onCancel}
        >
            {message}
        </ConfirmBox> as any
    }
}

const confirm = (message: string) => {
    return rawConfirm(message, options)
}

export default confirm;