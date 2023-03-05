import { createContext, ReactNode, useCallback, useContext, useState } from "react";


interface IParameterBarContext<T> {
    setFilter(filter: IFilterItem<T>): void,
    setSort(sort: ISortItem<T>): void,
    setSearch(value: string): void
    sort: ISortItem<T> | undefined,
    filter: IFilterItem<T> | undefined,
    search: string
}


export const ParameterBarContext = createContext({
    setFilter: (filter: any) => { },
    setSort: (sort: any) => { },
    setSearch: (value: string) => { },
    sort: undefined,
    filter: undefined,
    search: ''
});


export interface IFilterItem<T> {
    key: keyof T & string
    value: string | undefined,
}

export interface ISortItem<T> {
    name: string,
    key: keyof T & string
}



interface IParameterBarContextProvider {
    children: ReactNode
}

export default function ParameterBarContextProvider<T>({
    children
}: IParameterBarContextProvider) {
    const [filter, setFilter] = useState<IFilterItem<T> | any>(undefined);
    const [sort, setSort] = useState<ISortItem<T> | any>(undefined);
    const [search, setSearch] = useState<string>('');


    const handleSetFilter = useCallback((filter: IFilterItem<T>) => {
        setFilter(filter)
    }, [setFilter])


    const handleSetSort = useCallback((sort: ISortItem<T>) => {
        setSort(sort)
    }, [setSort])


    const handleSetSearch = useCallback((value: string) => {
        setSearch(value)
    }, [setSearch])


    return (
        <ParameterBarContext.Provider value={{
            setFilter: handleSetFilter,
            setSort: handleSetSort,
            setSearch: handleSetSearch,
            filter: filter,
            sort: sort,
            search: search
        }}>
            {children}
        </ParameterBarContext.Provider>
    )
}

export function useParameterBarContext<T>() {
    return useContext(ParameterBarContext) as IParameterBarContext<T>
}