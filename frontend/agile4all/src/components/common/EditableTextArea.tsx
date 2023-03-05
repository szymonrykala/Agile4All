import { Textarea, Typography } from "@mui/joy";
import { ChangeEventHandler, useCallback } from "react";


interface IEditableTextArea {
    onChange: (text: string) => void,
    editable: boolean,
    value: string,
    title: string
}


export default function EditableTextArea(props: IEditableTextArea) {

    const changeHandler: ChangeEventHandler<HTMLTextAreaElement> = useCallback(({ target }) => {
        props.onChange(target.value)
    }, [props])

    return (
        <Typography component='label' level='body3'>
            {props.title}
            <Textarea
                size='sm'
                disabled={!props.editable}
                value={props.value}
                color="neutral"
                variant={props.editable ? 'soft' : "plain"}
                maxRows={15}
                placeholder='Type a text'
                onChange={changeHandler}
                sx={{
                    // bgcolor: 'inherit'
                }}
            />
        </Typography>
    )
}