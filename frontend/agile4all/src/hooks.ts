import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import { UserRole } from './models/user'
import type { RootState, AppDispatch } from './store'

// Use instead of plain `useDispatch` and `useSelector`
export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector


export function useCheckAdmin():boolean {
    const sessionUserRole = useAppSelector(({ session }) => session?.user.role)
    return sessionUserRole === UserRole.ADMIN
}