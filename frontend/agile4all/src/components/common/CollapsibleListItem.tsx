import { IconButton, List, ListItem, ListItemContent, ListItemDecorator, Typography } from "@mui/joy";
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import React, { ReactNode } from "react";


interface ICollapsibleListItem {
    header: ReactNode,
    children: ReactNode[],
    open?: boolean,
    footer?: ReactNode,
}


export default function CollapsibleListItem(props: ICollapsibleListItem) {
    const [open, setOpen] = React.useState<boolean>(Boolean(props.open));

    return (
        <>
            <ListItem sx={{ width: 'fit-content' }}>
                <ListItemContent >
                    <Typography >
                        {props.header}&nbsp;&nbsp;
                    </Typography>
                </ListItemContent>

                <IconButton
                    color={open ? 'neutral' : 'primary'}
                    sx={{ borderRadius: '100%' }}
                    onClick={() => setOpen(!open)}
                >
                    {
                        open ?
                            <ArrowUpwardIcon />
                            : <ArrowDownwardIcon />
                    }
                </IconButton>
            </ListItem>
            <ListItem nested sx={{
                display: open ? 'flex' : 'none',
                marginBottom: 1.5
            }}>
                <List sx={{
                    display: 'grid',
                    gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
                    alignItems: 'stretch',
                    justifyItems: 'stretch',
                    // gap: 2,
                }}>
                    {
                        props.children.map((node, id) => <ListItem
                            sx={{
                                display: 'flex',
                                alignItems: 'stretch',
                                justifyContent: 'stretch',
                            }}
                            key={id}
                        >
                            {node}
                        </ListItem>)
                    }
                </List>
                <ListItemDecorator>
                    {props.footer}
                </ListItemDecorator>
            </ListItem>
        </>
    )
}
