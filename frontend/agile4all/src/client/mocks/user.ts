import { UUID } from "../../models/common"
import { UserRole } from "../../models/user"


export const USER_ID = 100 as UUID


export const userLoginResp = {
    token: "qwertyuiopasdfghjklzxcvbnm",
    userId: USER_ID
}


export const mockedUserMain = {
    id: USER_ID,
    email: process.env.REACT_APP_TEST_USER as string,
    firstName: "Firstname",
    lastName: "Lastname",
    role: UserRole.ADMIN
}


export const mockedUser2 = {
    id: 10,
    email: "10_user@test.com",
    firstName: "10_Firstname",
    lastName: "10_Lastname",
    role: UserRole.STUDENT
}


export const mockedUser3 = {
    id: 90,
    email: "90_user@test.com",
    firstName: "90_Firstname",
    lastName: "90_Lastname",
    role: UserRole.ADMIN
}


export const mockedUsers = [
    mockedUserMain,
    mockedUser2,
    mockedUser3
]


export function userById(id: number) {
    return mockedUsers.filter(user => user.id === id)[0]
}