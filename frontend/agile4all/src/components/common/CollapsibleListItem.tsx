import { IconButton, ListItem, ListItemContent, ListItemDecorator, Stack, Typography } from "@mui/joy";
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import React, { ReactNode } from "react";


interface ICollapsibleListItem {
    header: ReactNode,
    buttons: ReactNode,
    children: ReactNode,
    open?: boolean,
    footer?: ReactNode,
}


export default function CollapsibleProjectCanbanBoard(props: ICollapsibleListItem) {
    const [open, setOpen] = React.useState<boolean>(Boolean(props.open));

    return (
        <>
            <ListItem>
                <ListItemContent >
                    <Typography >
                        {props.header}&nbsp;&nbsp;
                    </Typography>
                </ListItemContent>
                <Stack spacing={2} direction="row">
                    {props.buttons}
                    <IconButton
                        size="md"
                        color='neutral'
                        sx={{ borderRadius: '100%' }}
                        onClick={() => setOpen(!open)}
                    >
                        {
                            open ?
                                <ArrowUpwardIcon />
                                : <ArrowDownwardIcon />
                        }
                    </IconButton>
                </Stack>
            </ListItem>
            <ListItem sx={{
                display: open ? 'unset' : 'none',
            }}>
                {props.children}

                <ListItemDecorator>
                    {props.footer}
                </ListItemDecorator>
            </ListItem>
        </>
    )
}
