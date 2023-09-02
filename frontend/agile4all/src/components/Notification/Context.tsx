import { ReactNode, createContext, useCallback, useEffect, useState } from "react";
import Notification, { INotification, NotificationType } from "./Notification";


export interface INotificationContext {
    error(text: any): void,
    info(text: string): void,
}


export const NotificationContext = createContext<INotificationContext>({
    error: () => { },
    info: () => { }
});


interface INotificationContextProvider {
    children: ReactNode
}

export default function NotificationContextProvider({ children }: INotificationContextProvider) {
    const [open, setOpen] = useState<boolean>(false)
    const [data, setData] = useState<Omit<INotification, "onClose">>({ text: "", type: NotificationType.INFO })

    const setError = useCallback((text: any) => {
        setData({ text: String(text), type: NotificationType.ERROR });
        setOpen(true)
    }, [setData])

    const setInfo = useCallback((text: string) => {
        setData({ text: text, type: NotificationType.INFO });
        setOpen(true)
    }, [setData])


    useEffect(() => {
        if (open === false) return;

        const timeoutId = setTimeout(() => setOpen(false), 3000);

        return () => {
            clearTimeout(timeoutId)
        }
    }, [open])


    return <NotificationContext.Provider value={{
        error: setError,
        info: setInfo
    }}>
        {open && <Notification
            onClose={() => setOpen(false)}
            text={data.text}
            type={data.type}
        />}
        {children}
    </NotificationContext.Provider>
}