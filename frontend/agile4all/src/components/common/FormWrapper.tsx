import { Button, Sheet, Typography } from "@mui/joy";
import { SxProps } from "@mui/system";
import { ReactNode, useCallback } from "react";


interface IWrapper {
    children: ReactNode,
    title: string,
    footer?: ReactNode,
    sx?: SxProps,
    error?: string,
    submitHandler: React.FormEventHandler<HTMLFormElement>
}


function FormWrapper({ children, title, sx, submitHandler, footer, error }: IWrapper) {

    const onSubmit: React.FormEventHandler<HTMLFormElement> = useCallback((event) => {
        event.preventDefault();
        submitHandler(event)
    }, [submitHandler])

    return (
        <Sheet sx={{
            padding: 3,
            borderRadius: 5,
            bgcolor: 'background.componentBg',
            ...sx
        }}>
            <Typography level='h3' component='h1'>
                {title}
            </Typography>
            &nbsp;
            <Sheet
                onSubmit={onSubmit}
                component='form'
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    bgcolor: 'background.componentBg',
                    gap: 1.
                }}>
                {children}
                {error && <Typography color='danger'> {error}</Typography>}
                <Button type='submit'>
                    Submit
                </Button>
            </Sheet>
            &nbsp;
            {footer}
        </Sheet>
    )
}

export default FormWrapper;