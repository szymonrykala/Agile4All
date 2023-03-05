import { DataModel, UUID } from "./common"
import User from "./user"


interface Project extends DataModel {
    name: string,
    description: string,
    users: Array<User>,
}


export default Project