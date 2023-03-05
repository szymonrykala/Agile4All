import { DataModel, UUID } from "./common"
import File from "./file"


export enum TaskStatus {
    TODO = 'TODO',
    IN_PROGRESS = 'IN_PROGRESS',
    DONE = 'DONE',
    ARCHIVED = 'ARCHIVED'
}

const statusMap = {
    [TaskStatus.IN_PROGRESS]: 1,
    [TaskStatus.TODO]: 2,
    [TaskStatus.DONE]: 3,
    [TaskStatus.ARCHIVED]: 4,
}

export function taskStatusSort(ts1: Task, ts2: Task): number {
    return statusMap[ts1.status] - statusMap[ts2.status]
}



interface Task extends DataModel {
    name: string,
    description: string,
    status: Required<TaskStatus>,
    userId: UUID,
    projectId: UUID
}

export interface FullTask extends Task {
    files: Array<File>
}

export default Task