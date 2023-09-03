import { Sheet, IconButton, Link, Typography, Tooltip } from "@mui/joy";
import DescriptionIcon from '@mui/icons-material/Description';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import FileModel from "../../models/file";
import { UUID } from "../../models/common";


interface IFile {
    data: FileModel,
    onDelete: (id: UUID) => void
}

const style = {
    main: {
        position: 'relative',
        bgcolor: 'inherit',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        maxWidth: "70px"
    },
    deleteButton: {
        position: 'absolute',
        top: '-7px',
        left: '-7px',
        borderRadius: '100%',
        "&:hover": {
            bgcolor: "transparent"
        }
    }
}

export default function File({ data, onDelete }: IFile) {

    return (
        <Sheet sx={style.main}>
            <IconButton
                variant="plain"
                size="md"
                color="neutral"
                component={Link}
                href={"/api" + data.link}
                target='_blank'
            >
                <DescriptionIcon color='primary' sx={{ fontSize: 40 }} />
            </IconButton>
            <IconButton
                variant="plain"
                size="sm"
                sx={style.deleteButton}
                onClick={() => onDelete(data.id)}
            >
                <Tooltip title="usuń">
                    <DeleteForeverIcon
                        sx={{ fontSize: '20px' }}
                        color='warning'
                    />
                </Tooltip>
            </IconButton>
            <Tooltip title={data.name}>
                <Typography level='body-sm' sx={{ overflow: 'hidden', maxWidth: '70px', maxHeight: '20px' }}>
                    {data.name}
                </Typography>
            </Tooltip>
        </Sheet>
    )
}