import { StoreExt } from '@utils/reactExt'
import { IPageParams } from 'shared/types/pageParams'

export class UserStore extends StoreExt {
    getUsersloading: boolean = false

    users: IUserStore.IUser[] = []

    total: number = 0

    page: IPageParams = { index: 1, size: 30 }
}

export const actions = {
    getUsers: 'getUsers',
    handleTableChange: 'handleTableChange',
    modifyUser: 'modifyUser',
    deleteUser: 'deleteUser',
    showUserListLoading: 'SHOW_USER_LIST_LOADING',
    hideUserListLoading: 'HIDE_USER_LIST_LOADING'
}

const actionHandlers = {
    [actions.getUsers]: (state: UserStore, action) => {
        const { users } = action.payload
        return { ...state, users }
    },
    [actions.handleTableChange]: (state: UserStore, action) => {
        const { users, index, size } = action.payload
        return {
            ...state,
            users,
            page: {
                index,
                size
            }
        }
    },
    [actions.showUserListLoading]: (state: UserStore, action) => {
        return { ...state, getUsersloading: true }
    },
    [actions.modifyUser]: (state: UserStore, action) => {
        const { user } = action.payload
        return { ...state, users: [...state.users.filter(u => u._id !== user._id), user] }
    },
    [actions.deleteUser]: (state: UserStore, action) => {
        const { id } = action.payload
        return { ...state, users: [...state.users.filter(u => u._id !== id)] }
    },
    [actions.hideUserListLoading]: (state: UserStore, action) => {
        return { ...state, getUsersloading: false }
    }
}

export default function(state = new UserStore(), action) {
    const handler = actionHandlers[action.type]
    return handler ? handler(state, action) : state
}
