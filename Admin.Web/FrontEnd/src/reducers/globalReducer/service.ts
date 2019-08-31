import { ThunkAction, ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import { actions } from './index'


class GlobalActionCreatorService {
    changeSiderTheme = (theme: string): ThunkAction<void, {}, string, AnyAction> => {
        return (dispatch: ThunkDispatch<{}, {}, AnyAction>): void => {
            try {
                dispatch({
                    type: actions.changeSiderTheme,
                    payload: {
                        theme,
                    }
                })
            } catch (err) {
                console.error(err)
            }
        }
    }

    setOpenKeys = (theme: string): ThunkAction<void, {}, string, AnyAction> => {
        return (dispatch: ThunkDispatch<{}, {}, AnyAction>): void => {
            try {
                dispatch({
                    type: actions.setOpenKeys,
                    payload: {
                        theme,
                    }
                })
            } catch (err) {
                console.error(err)
            }
        }
    }
}

export default new GlobalActionCreatorService();
