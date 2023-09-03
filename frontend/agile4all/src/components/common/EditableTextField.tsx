import { Input, Typography } from "@mui/joy";
import { ChangeEventHandler, useCallback } from "react";


interface IEditableTextField {
    onChange: (text: string) => void,
    editable: boolean,
    value: string,
    title: string,
    size?: 'sm' | 'md' | 'lg'
}


export default function EditableTextField(props: IEditableTextField) {

    const changeHandler: ChangeEventHandler<HTMLInputElement> = useCallback(({ target }) => {
        props.onChange(target.value)
    }, [props])


    return (
        <Typography component='label' level='body-sm'>
            {props.title}
            <Input
                id={props.title}
                size={props.size || 'sm'}
                color="neutral"
                disabled={!props.editable}
                variant={props.editable ? 'soft' : "plain"}
                value={props.value}
                onChange={changeHandler}
            />
        </Typography>
    )
}