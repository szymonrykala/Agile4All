import { ListItem, IconButton } from "@mui/joy";
import AddIcon from "@mui/icons-material/Add";



interface IAddListItem {
    onClick(): void
    component?: React.ElementType<any>
}

export default function AddListItem({ onClick, component }: IAddListItem) {

    return (
        <ListItem {...component && { 'component': component }}>
            <IconButton
                onClick={onClick}
                size='lg'
            >
                <AddIcon />
            </IconButton>
        </ListItem >
    )
}