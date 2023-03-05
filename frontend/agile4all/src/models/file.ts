import { DataModel, UUID } from "./common"


interface File extends DataModel{
    name: string,
    link: string,
    userId: UUID,
    modificationDate: string
}

export default File