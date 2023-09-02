import { IconButton, Box } from "@mui/joy";
import FileModel from "../../models/file";
import UploadIcon from '@mui/icons-material/Upload';
import File from './File';
import { useCallback, useEffect, useRef, useState } from "react";
import { FilesApi } from "../../client";
import { UUID } from "../../models/common";
import { useParams } from "react-router";
import { mockedFiles } from "../../client/mocks/files";
import useNotification from "../Notification";


interface IFilesPanel {
    files?: FileModel[],
}

export default function FilesPanel(props: IFilesPanel) {
    const queryParams = useParams();
    const [files, setFiles] = useState<FileModel[]>(props.files || [])
    const fileInputRef: React.MutableRefObject<HTMLInputElement | null> = useRef(null)
    const { info, error } = useNotification();

    const fetchFiles = useCallback(async () => {
        let resp: FileModel[];

        if (process.env.NODE_ENV === "development" && process.env.REACT_APP_MOCK_API === "true") {
            resp = mockedFiles
        } else {
            resp = await FilesApi.getAll(queryParams) as unknown as FileModel[]
        }

        setFiles(resp)

    }, [queryParams])

    useEffect(() => {
        fetchFiles()
    }, [fetchFiles])



    const uploadFile = useCallback(async () => {
        if (!fileInputRef?.current) return;

        const file = fileInputRef.current.files?.item(0)
        if (!file) {
            error('Problem z załadowaniem pliku');
            return;
        }

        try {
            await FilesApi.uploadFile(file, queryParams);
            fetchFiles();
            info("Plik dodano.");
        } catch (err) {
            error(err);
        }
        fileInputRef.current.value = ''

    }, [queryParams, fetchFiles, info, error]);



    useEffect(() => {
        if (!fileInputRef?.current) return;
        const fileInput = fileInputRef.current

        fileInput.addEventListener('change', uploadFile)

        return () => {
            fileInput.removeEventListener('change', uploadFile)
        }
    }, [uploadFile])



    const loadFile = useCallback(() => {
        if (fileInputRef?.current) {
            fileInputRef.current.click()
        }
    }, []);



    const deleteFile = useCallback(async (fileId: UUID) => {
        try{
            await FilesApi.delete(fileId)
            setFiles(files.filter(({ id }) => id !== fileId));
            info("Plik usunięto.");
        }catch(err){
            error(err);
        }

    }, [files, setFiles, info, error]);



    return (
        <Box sx={{
            display: 'flex',
            alignItems: 'flex-start',
            gap: 2,
            bgcolor: 'inherit',
            overflowX: 'auto',
        }}>
            {
                files.map((file, index) => <File key={index}
                    data={file}
                    onDelete={deleteFile}
                />)
            }
            <Box sx={{ bgcolor: 'inherit' }}>
                <input style={{ display: 'none' }} type='file' name="file" ref={fileInputRef} />
                <IconButton
                    onClick={loadFile}
                    variant="soft"
                    size="lg"
                >
                    <UploadIcon fontSize="large" />
                </IconButton>
            </Box>
        </Box>
    )
}