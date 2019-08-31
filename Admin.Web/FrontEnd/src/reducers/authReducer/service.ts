import { ThunkAction, ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import * as api from '@services/api'

import { actions } from './index'
import { LOCALSTORAGE_KEYS } from '@constants/index';

class AuthStoreService {
    login = (params: IAuthStore.LoginParams): ThunkAction<Promise<void>, {}, IAuthStore.LoginParams, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                const res = await api.auth.login(params)
                localStorage.setItem(LOCALSTORAGE_KEYS.USERINFO, JSON.stringify(res))

                // TODO
                // routerStore.replace('/')

                dispatch({
                    type: actions.login,
                    payload: res
                })
            } catch (err) {
                console.error(err)
            }
        }
    }

    logout = (): ThunkAction<Promise<void>, {}, IAuthStore.LoginParams, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                localStorage.removeItem(LOCALSTORAGE_KEYS.USERINFO)

                // TODO
                // routerStore.replace('/login')

                dispatch({
                    type: actions.logout,
                })
            } catch (err) {
                console.error(err)
            }
        }
    }
}


export default new AuthStoreService();