import { createContext, ReactNode, useCallback, useContext, useEffect, useState } from "react";
import { useAppDispatch } from "../../hooks";
import { LoadWSMessage, PingWSMessage, WSMessage, WSMessageType } from "../../models/chat";
import { add, load } from "../../store/chatSlice";



interface IChatContext {
    socket: WebSocket | null,
    toggleChat: () => void
    chatOpen: boolean
}

const _socket = new WebSocket(process.env.REACT_APP_WS_API_URL as string);


export const ChatContext = createContext<IChatContext>({
    socket: null,
    chatOpen: false,
    toggleChat: () => false
});


interface IChatContextProvider {
    children: ReactNode
}


export function ChatContextProvider({ children }: IChatContextProvider) {
    const dispatch = useAppDispatch()
    const [socket, setSocket] = useState<WebSocket>(_socket);
    const [chatOpen, setOpen] = useState<boolean>(false)

    const toggleChat = useCallback(() => setOpen(!chatOpen),
        [chatOpen]
    )

    const recieveLoadMessage = useCallback((data: LoadWSMessage) => {
        const messages = data.payload
        dispatch(load(messages))
    }, [dispatch])


    const recieveMessage = useCallback((data: WSMessage) => {
        const message = data.payload
        dispatch(add(message))
    }, [dispatch])

    const reconnectWS = useCallback(() => {
        if ([socket.CLOSED, socket.CLOSING].includes(socket.readyState)) {
            console.debug('creating new connection to ws')
            const conn = new WebSocket(process.env.REACT_APP_WS_API_URL as string)
            setSocket(conn)
        }
    }, [socket, setSocket])


    const pingServer = useCallback(() => {
        const ping: PingWSMessage = {
            type: WSMessageType.PING,
            payload: null
        }
        if ([socket.CLOSED, socket.CLOSING].includes(socket.readyState)) reconnectWS()
        try {
            socket.send(JSON.stringify(ping));
            console.debug('WS Ping message sent')
        } catch (e) {
            console.debug(`Error while ws ping ${e}`)
        }
    }, [socket,reconnectWS])


    useEffect(() => reconnectWS(), [reconnectWS])


    useEffect(() => {
        const int = setInterval(pingServer, 60 * 1000)
        pingServer()
        return () => clearInterval(int)
    }, [pingServer])


    useEffect(() => {
        if ([
            socket.CLOSING,
            socket.CLOSED
        ].includes(socket.readyState)) return;

        socket.addEventListener('open', (event) => {
            console.debug('WS connected')

            socket.addEventListener('close', (event) => {
                console.debug('WS disconnected')
            });
        });

        socket.addEventListener('message', (event: MessageEvent) => {
            let data = null;
            try {
                data = JSON.parse(event.data) as LoadWSMessage | WSMessage
            } catch {
                console.warn(`Could not parse WS event ${event}`)
                return;
            }

            switch (data.type) {
                case WSMessageType.LOAD:
                    recieveLoadMessage(data)
                    break;
                case WSMessageType.MESSAGE:
                    recieveMessage(data)
                    break;
                default:
                    console.debug(`Unsupported event type: ${event}`)
            }
        });

        socket.addEventListener('error', (event) => {
            console.debug('Error from server ', event);
        });

    }, [socket.readyState, socket, recieveLoadMessage, recieveMessage])


    return (
        <ChatContext.Provider value={{ socket, chatOpen, toggleChat }}>
            {children}
        </ChatContext.Provider>
    )
}


export function useChatContext(): IChatContext {
    return useContext(ChatContext);
}