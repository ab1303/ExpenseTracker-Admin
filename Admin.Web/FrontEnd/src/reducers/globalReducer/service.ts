import { ThunkAction, ThunkDispatch } from 'redux-thunk'
import { AnyAction } from 'redux'
import { actions } from './index'


export class GlobalActionCreatorService {
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
}