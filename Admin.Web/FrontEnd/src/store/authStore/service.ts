import { StoreExt } from '@utils/reactExt'
import { ThunkAction, ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'

import { actions } from './index'
import { LOCALSTORAGE_KEYS } from '@constants/index';
import { routerStore } from '@store/index';

export class AuthStoreService extends StoreExt {
    login = (params: IAuthStore.LoginParams): ThunkAction<Promise<void>, {}, IAuthStore.LoginParams, AnyAction> => {
        return async (dispatch: ThunkDispatch<{}, {}, AnyAction>): Promise<void> => {
            try {
                const res = await this.api.auth.login(params)
                localStorage.setItem(LOCALSTORAGE_KEYS.USERINFO, JSON.stringify(res))
                routerStore.replace('/')

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
                routerStore.replace('/login')

                dispatch({
                    type: actions.logout,
                })
            } catch (err) {
                console.error(err)
            }
        }
    }
}
