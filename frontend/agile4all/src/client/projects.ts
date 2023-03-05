import { UUID } from "../models/common"
import ApiClient from "./ApiClient"


interface ICreateProjectData {
    name: string,
    description: string,
}

interface IUpdateProjectData {
    name: string,
    description: string,
}



export default class ProjectsClient
    extends ApiClient<ICreateProjectData, IUpdateProjectData> {

    public path: string = '/projects'

    addUser(projectId: UUID, userId: UUID) {
        return this._put(`${this.path}/${projectId}/users/${userId}`)
    }

    removeUser(projectId: UUID, userId: UUID) {
        return this._delete(`${this.path}/${projectId}/users/${userId}`)
    }
}