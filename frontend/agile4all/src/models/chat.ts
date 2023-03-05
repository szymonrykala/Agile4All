import { UUID } from "./common";



export interface ChatMessage {
    text: string,
    date: string,
    userId: UUID,
    sender: string
}

export enum WSMessageType {
    PING = "PING",
    MESSAGE = "MESSAGE",
    LOAD = "LOAD"
}

export interface LoadWSMessage {
    type: WSMessageType.LOAD,
    payload: ChatMessage[]
}

export interface WSMessage {
    type: WSMessageType.MESSAGE,
    payload: ChatMessage
}

export interface PingWSMessage {
    type: WSMessageType.PING,
    payload: null
}
