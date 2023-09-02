import { Close } from "@mui/icons-material";
import { Box, BoxProps, IconButton } from "@mui/joy";
import Link from "./Link";
import { useCallback, useEffect } from "react";
import { useNavigate } from "react-router";


const styles = {
    position: "fixed",
    right: 0,
    top: 48,

    display: "flex",
    flexFlow: "column",

    p: "16px",
    gap: 3,
    height: "100vh",
    width: {
        xs: "calc(92vw - 16px)",
        sm: "350px",
        md: "400px",
        lg: "400px"
    },

    bgcolor: 'background.componentBg',
    borderLeft: '1px solid',
    borderColor: 'divider',
    animation: "0.2s ease-out slideInFromRight",
    zIndex: 900,
}


export default function SidePanel(props: BoxProps) {
    const navigate = useNavigate()

    const escHandler = useCallback((ev: KeyboardEvent) => {
        if (ev.key === 'Escape') {
            ev.preventDefault()
            navigate("../")
        }
    }, [navigate])


    useEffect(() => {
        window.addEventListener('keydown', escHandler)
        return () => window.removeEventListener('keydown', escHandler)
    }, [escHandler])


    return (
        <Box
            {...props}
            sx={styles}
        >
            <Box>
                <IconButton
                    variant="outlined"
                    size="sm"
                    component={Link} to="../"
                >
                    <Close />
                </IconButton>
            </Box>
            {props.children}
        </Box>
    );
}
