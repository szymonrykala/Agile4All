import { IconButton, BoxProps, IconButtonPropsSizeOverrides } from "@mui/joy";
import AddIcon from "@mui/icons-material/Add";



interface IAddListItem extends BoxProps {
    onClick(): void
    size?: IconButtonPropsSizeOverrides
}

export default function AddListItem(props: IAddListItem) {

    return (
        <IconButton
            onClick={props.onClick}
            size={props?.size as any || "md"}
            sx={props.sx}
        >
            <AddIcon />
        </IconButton>
    )
}