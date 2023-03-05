import { Sheet, IconButton, Link, Typography, Tooltip } from "@mui/joy";
import DescriptionIcon from '@mui/icons-material/Description';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import FileModel from "../../models/file";
import { UUID } from "../../models/common";
import { Maximize } from "@mui/icons-material";


interface IFile {
    data: FileModel,
    onDelete: (id: UUID) => void
}

export default function File({ data, onDelete }: IFile) {

    return (
        <Sheet
            sx={{
                position: 'relative',
                bgcolor: 'inherit',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center'
            }}
        >
            <IconButton
                variant="plain"
                size="lg"
                color="neutral"
                component={Link}
                href={data.link}
                target='_blank'
            >
                <DescriptionIcon color='primary' sx={{ fontSize: 50 }} />
            </IconButton>
            <IconButton
                variant="soft"
                size="sm"
                color="danger"
                sx={{
                    position: 'absolute',
                    top: '-7px',
                    left: '-7px',
                    borderRadius: '100%'
                }}
                onClick={() => onDelete(data.id)}
            >
                <DeleteForeverIcon
                    sx={{ fontSize: '25px' }}
                    color='warning'
                />
            </IconButton>
            <Tooltip title={data.name}>
                <Typography level='body3' sx={{ overflowY: 'clip', maxWidth: '60px', maxHeight: '20px' }}>
                    {data.name}
                </Typography>
            </Tooltip>

        </Sheet>
    )
}