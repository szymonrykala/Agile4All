import { Link as MUILink } from '@mui/joy'
import { SxProps } from '@mui/joy/styles/types'
import { ReactNode } from 'react'
import { Link as RouterLink } from 'react-router-dom'


interface ILink {
    children: ReactNode,
    to: string,
    sx?: SxProps
}

export default function Link(props: ILink) {
    return <MUILink
        {...props}
        component={RouterLink}
    />
}