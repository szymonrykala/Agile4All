import { Alert } from "@mui/joy";
import { useRouteError } from "react-router";



export function ErrorBaner(props: any) {
    const error = useRouteError() as any

    return (
        <Alert color="warning" >
            {error?.message as string}
        </Alert>
    )
}