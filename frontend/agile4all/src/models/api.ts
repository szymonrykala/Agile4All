import { UUID } from "./common"



export interface APIError{
    message: Readonly<Required<string>>
}


export interface AuthPayload{
    email: string,
    password: string
}


export interface UserTokenPayload {
    readonly token: string,
    readonly userId: UUID
}