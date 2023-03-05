
export class ApiError extends Error {
    message: string = 'There is a problem with API :/'

    constructor(message: string | undefined = undefined) {
        super()
        if (message) this.message = message
    }
}


export class NoValidUserSessionError extends ApiError {
    message: string = 'No active user session found. Please login again.'
}

export class BadCredentialsError extends ApiError {
    message: string = 'Provided credentials are invalid'
}

export class UserRegistrationError extends ApiError {
    message: string = 'Could not register user right now'
}