import { mockedUserMain } from "./user";


export const mockedFile1 = {
    id: 876,
    name: "design.xml",
    link: "http://0.0.0.0/api/files/876",
    userId: mockedUserMain.id,
    modificationDate: "07-12-2023T12:23:00"
}


export const mockedFile2 = {
    id: 646,
    name: "requirements.txt",
    link: "http://0.0.0.0/api/files/646",
    userId: mockedUserMain.id,
    modificationDate: "17-11-2023T12:23:00"
}


export const mockedFile3 = {
    id: 456,
    name: "analysys.docx",
    link: "http://0.0.0.0/api/files/456",
    userId: mockedUserMain.id,
    modificationDate: "06-02-2023T11:23:00"
}


export const mockedFiles = [
    mockedFile1,
    mockedFile2,
    mockedFile3
]