import { ProjectsApi } from "."
import { UUID } from "../models/common"
import Project from "../models/project"
import User, { Session, UserRole } from "../models/user"
import ApiClient, { ResponseData } from "./ApiClient"
import { BadCredentialsError, NoValidUserSessionError, UserRegistrationError } from "./exceptions"
import { mockedUserProjects } from "./mocks/project"
import { mockedUserMain, userLoginResp } from "./mocks/user"


export interface ILoginData {
    email: string,
    password: string
}

export interface ICreateUserData extends ILoginData {
    firstname: string,
    lastname: string,
}

export interface IUpdateUserData {
    firstName: string,
    lastName: string,
    role: UserRole
}

interface ILoginResponse extends ResponseData {
    token: string,
    userId: UUID
}


export default class UsersClient
    extends ApiClient<ICreateUserData, IUpdateUserData> {
    private userIdKey = 'userId'

    public path: string = '/users'

    saveUserId(userId: UUID) {
        localStorage.setItem(this.userIdKey, String(userId))
    }

    getSavedUserId(): number {
        return Number(localStorage.getItem(this.userIdKey) || -1)
    }

    async readUserFromSession(): Promise<Session> {
        const userId: number = this.getSavedUserId()

        if (userId === -1) throw new NoValidUserSessionError()

        try {
            if(process.env.REACT_APP_MOCK_MODE){
                return {
                    user: mockedUserMain,
                    projects: mockedUserProjects
                }
            }

            const user = await this.getOne(userId) as User
            const userProjects = await ProjectsApi.getAll({ userId: userId }) as Project[]

            return {
                user: user,
                projects: userProjects
            }
        } catch (error) {
            console.error(error)
            throw new NoValidUserSessionError()
        }
    }

    async login(data: ILoginData) {
        try {
            let resp: ILoginResponse;
            if(process.env.REACT_APP_MOCK_MODE){
                resp = userLoginResp
            }else{
                resp = await this._post(`${this.path}/login`, data) as ILoginResponse;
            }

            this.authToken = resp.token
            this.saveUserId(resp.userId)
            return true
        } catch (error) {
            console.debug(error)
            if (true) {
                throw new BadCredentialsError()
            }
        }
    }

    logout() {
        this.clearAuthToken()
        localStorage.removeItem(this.userIdKey)
    }

    async register(data: ICreateUserData) {
        try {
            await this._post(`${this.path}`, data)
        } catch (error) {
            console.debug(error)
            if (true) {
                throw new UserRegistrationError()
            }
        }
    }
}

