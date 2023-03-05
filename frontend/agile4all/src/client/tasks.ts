import { UUID } from "../models/common"
import { TaskStatus } from "../models/task"
import ApiClient from "./ApiClient"



export interface ICreateTaskData {
    projectId?: UUID,
    name: string,
    description: string,
    userId: UUID,
}

export interface IUpdateTaskData {
    name: string,
    description: string,
    userId: UUID,
    status: TaskStatus
}



export default class TasksClient
    extends ApiClient<ICreateTaskData, IUpdateTaskData> {

    public path: string = '/tasks'

    create(body:ICreateTaskData){
        const projectId = body.projectId;
        delete body.projectId

        return this._post(`/projects/${projectId}${this.path}`, body)
    }
}