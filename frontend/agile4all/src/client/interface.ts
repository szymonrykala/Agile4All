import { UUID } from "../models/common"


export interface APIPagination {
    pagesCount: number,
    currentPage: number,
    itemsOnPage: number
}


export interface PaginationQueryParams {
    [index: string]: string | number | undefined,
    itemsOnPage?: number,
    currentPage?: number
}


export interface QueryParams {
    [index: string]: string | number | undefined,
    userId?: UUID,
    projectId?: UUID,
    taskId?: UUID
}
