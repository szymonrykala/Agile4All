import { Button } from "@mui/joy";


interface IFilterItem {
    clicked?: boolean,
    onClick: () => void,
    name: string
}


export default function FilterItem(props: IFilterItem) {
    return (
        <Button
            variant={props.clicked ? "soft" : "outlined"}
            onClick={props.onClick}
            size='sm'
        >
            {props.name}
        </Button>
    )
}