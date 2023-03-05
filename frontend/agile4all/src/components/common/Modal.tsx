import { Card, Modal as JoyModal, ModalClose } from '@mui/joy';
import { SxProps } from '@mui/joy/styles/types';
import { ReactNode } from 'react';

interface IModal {
    title: string,
    description: string,
    children: ReactNode,
    sx?: SxProps
}

export default function Modal(props: IModal) {

    return (
        <JoyModal
            aria-labelledby={props.title}
            aria-describedby={props.description}
            open={true}
            onClose={() => window.history.back()}
            sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 999 }}
        >
            <Card sx={{
                transform: 'translateY(-5%)',
                maxWidth: '90vw',
                maxHeight: '90vh',
                gap: 2,
                ...props.sx,
            }}>
                <ModalClose variant="soft" sx={{ zIndex: 2000 }} />

                {props.children}
            </Card>
        </JoyModal>
    )
}