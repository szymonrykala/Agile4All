import Layout from '../Layout';
import Header from '../Header';
import { ReactNode } from 'react';
import { SxProps } from '@mui/joy/styles/types';


interface IBasePage {
    children: ReactNode,
    sx?: SxProps
}

function BasePage({ children, sx }: IBasePage) {
    return <>
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
    </>
}

export default BasePage



