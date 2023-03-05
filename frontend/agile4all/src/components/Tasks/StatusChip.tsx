import { Chip } from "@mui/joy";
import { SxProps } from "@mui/joy/styles/types";
import { TaskStatus } from "../../models/task";

interface IStatusChip {
    status: TaskStatus,
    sx?: SxProps,
    variant?: 'soft' | 'solid'
}

export function getStatusColor(status: TaskStatus) {
    switch (status) {
        case TaskStatus.DONE:
            return 'success';
        case TaskStatus.TODO:
            return 'primary';
        case TaskStatus.IN_PROGRESS:
            return 'info';
        case TaskStatus.ARCHIVED:
            return 'neutral';
        default:
            return 'primary';
    }
}


export default function StatusChip(props: IStatusChip) {
    return (
        <Chip
            component='span'
            size="sm"
            variant={props.variant || "solid"}
            sx={props?.sx}
            color={getStatusColor(props.status)}
        >
            {props.status}
        </Chip>
    )
}