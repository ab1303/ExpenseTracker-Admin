import { isPlainObject } from 'lodash'
import { initialUserInfo } from './syncUserInfo'

export class AuthStore {
    /**
     *
     * @type {IAuthStore.UserInfo}
     * @memberof AuthStore
     */
    userInfo: IAuthStore.UserInfo = initialUserInfo
}

export const actions = {
    login: 'login',
    logout: 'logout'
}

const actionHandlers = {
    [actions.login]: (state: AuthStore, action) => {
        const { payload } = action
        const result = isPlainObject(payload) ? payload : {}
        return {
            userInfo: { ...result }
        }
    },
    [actions.logout]: (state: AuthStore, action) => {
        return {
            userInfo: { ...state, userInfo: {} }
        }
    }
}

export default function(state = new AuthStore(), action) {
    const handler = actionHandlers[action.type]
    return handler ? handler(state, action) : state
}
