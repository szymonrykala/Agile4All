import { ResponseData } from "../client/ApiClient"
import { DataModel } from "./common"
import Project from "./project"

export enum UserRole {
    STUDENT = 'STUDENT',
    ADMIN = 'ADMIN'
}

interface User extends DataModel{
    email: string,
    firstName: string,
    lastName: string,
    role: UserRole
}

export interface Session extends ResponseData{
    user: User,
    projects: Project[]
}

export default User