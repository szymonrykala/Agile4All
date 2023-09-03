import { createContext, ReactNode, useCallback, useContext, useEffect, useState } from "react";



interface ITriggers {
    users: boolean
    projects: boolean
    tasks: boolean
}


export interface IResourceTriggerContext extends ITriggers {
    reload(type: keyof ITriggers): void,
}


const ResourceTriggerContext = createContext<IResourceTriggerContext>({
    reload: (type: keyof ITriggers) => { },
    users: false,
    projects: false,
    tasks: false
})


interface IReloadTriggerContextProvider {
    children: ReactNode
}


export function ReloadTriggerContextProvider({
    children
}: IReloadTriggerContextProvider) {
    const [interval, setStateInterval] = useState<NodeJS.Timer | null>(null)
    const [triggers, setTrigger] = useState<ITriggers>({
        users: false,
        projects: false,
        tasks: false
    })

    const trigger = useCallback((type: keyof ITriggers) => {
        setTrigger({ ...triggers, [type]: !triggers[type] })
    }, [triggers])


    useEffect(() => {
        const int = setInterval(() => {
            setTrigger({
                users: !triggers.users,
                tasks: !triggers.tasks,
                projects: !triggers.projects,
            })
        }, 20 * 1000);
        setStateInterval(int)

        return () => {
            interval && clearInterval(interval)
        }
    // eslint-disable-next-line
    }, [triggers])


    return <ResourceTriggerContext.Provider
        value={{
            reload: trigger,
            users: triggers.users,
            projects: triggers.projects,
            tasks: triggers.tasks
        }}
    >
        {children}
    </ResourceTriggerContext.Provider>
}


export function useReloadTrigger() {
    return useContext(ResourceTriggerContext) as IResourceTriggerContext
}