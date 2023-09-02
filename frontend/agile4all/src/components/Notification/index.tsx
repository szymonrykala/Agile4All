import { useContext } from "react";
import { INotificationContext, NotificationContext } from "./Context";


export default function useNotification() {
    return useContext(NotificationContext) as INotificationContext
}
