import { StoreExt } from '@utils/reactExt'
import { ThunkAction, ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import { actions } from 'reducers/userStore'

import { IPageParams } from 'shared/types/pageParams'
import { PaginationConfig } from 'antd/lib/pagination'

export class UserStoreService extends StoreExt {
    getUsers = (page: IPageParams): ThunkAction<Promise<void>, {}, IPageParams, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                dispatch({ type: actions.showUserListLoading })

                const res = await this.api.user.getUsers({ pageIndex: page.index, pageSize: page.size })
                dispatch({
                    type: actions.getUsers,
                    payload: {
                        users: res.users,
                        total: res.total
                    }
                })

                dispatch({ type: actions.hideUserListLoading })
            } catch (err) {
                console.error(err)
            }
        }
    }

    createUser = (user: IUserStore.IUser): ThunkAction<Promise<void>, {}, IUserStore.IUser, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                dispatch({ type: actions.showUserListLoading })

                await this.api.user.createUser(user)

                dispatch({ type: actions.hideUserListLoading })
            } catch (err) {
                console.error(err)
            }
        }
    }

    modifyUser = (user: IUserStore.IUser): ThunkAction<Promise<void>, {}, IUserStore.IUser, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                dispatch({ type: actions.showUserListLoading })

                await this.api.user.modifyUser(user)

                dispatch({ type: actions.modifyUser, paylaod: { user } })
                dispatch({ type: actions.hideUserListLoading })
            } catch (err) {
                console.error(err)
            }
        }
    }

    deleteUser = (id: number): ThunkAction<Promise<void>, {}, number, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                await this.api.user.deleteUser({ id })
                dispatch({ type: actions.modifyUser, paylaod: { id } })
            } catch (err) {
                console.error(err)
            }
        }
    }

    handleTableChange = (pagination: PaginationConfig): ThunkAction<Promise<void>, {}, PaginationConfig, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                const { current, pageSize } = pagination

                dispatch({ type: actions.showUserListLoading })

                const res = await this.api.user.getUsers({ pageIndex: current, pageSize: pageSize })

                dispatch({
                    type: actions.handleTableChange,
                    payload: {
                        users: res.users,
                        index: current,
                        size: pageSize
                    }
                })

                dispatch({ type: actions.hideUserListLoading })
            } catch (err) {
                console.error(err)
            }
        }
    }
}