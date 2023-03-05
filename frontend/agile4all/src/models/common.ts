import File from "./file"
import Task from "./task"
import User from "./user"


export type UUID = Required<Readonly<number>>


export interface DataModel {
    [index: string]: string | number | Array<Task | File | User>,
    id: UUID
}

