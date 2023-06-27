import Layout from '../Layout';
import Header from '../Header';
import { ReactNode } from 'react';
import { SxProps } from '@mui/joy/styles/types';


interface INoSessionPage {
    children: ReactNode,
    sx?: SxProps
}

function NoSessionPage({ children, sx }: INoSessionPage) {
    return (
        <Layout.Root
            sx={{
                gridTemplateColumns: {
                    xs: '1fr',
                },
                gridTemplateRows: '64px 1fr',
                height: '100vh',
                overflow: 'hidden',
            }}
        >
            <Layout.Header>
                <Header session={null} />
            </Layout.Header>

            <Layout.Main sx={sx}>

                {children}

            </Layout.Main>
        </Layout.Root>
    )
}

export default NoSessionPage



