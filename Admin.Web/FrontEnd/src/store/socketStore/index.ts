import { StoreExt } from '@utils/reactExt'
import { LOCALSTORAGE_KEYS } from '@constants/index'
import { SOCKER_TYPES, DATA_FORMATS } from '@constants/socket'

/**
 * socket debugger store
 *
 * @export
 * @class SocketStore
 * @extends {StoreExt}
 */
export class SocketStore extends StoreExt {
    socketType: ISocketStore.SocketType =
        (localStorage.getItem(LOCALSTORAGE_KEYS.SOCKET_TYPE) as ISocketStore.SocketType) || SOCKER_TYPES[0]
    dataFormat: ISocketStore.DataFormatType =
        (localStorage.getItem(LOCALSTORAGE_KEYS.DATA_FORMAT) as ISocketStore.DataFormatType) || DATA_FORMATS[0]
    socketIsConnected: boolean = false
    messages: ISocketStore.Message[] = []
    notSupportPolling: boolean = localStorage.getItem(LOCALSTORAGE_KEYS.NOT_SUPPORT_POLLING) === 'true'
    isSocketIO: boolean = this.socketType === SOCKER_TYPES[0]
}

const actions = {
    setSocketType: 'setSocketType',
    setDataFormat: 'setDataFormat',
    setSocketIsConnected: 'setSocketIsConnected',
    clearMessages: 'clearMessages',
    addMessage: 'addMessage',
    setNotSupportPolling: 'setNotSupportPolling',
};

const actionHandlers = {
    [actions.setSocketType]: (state: SocketStore, action) => {
        const { type } = action.model;
        return { ...state, socketType: type }
    },
    [actions.setDataFormat]: (state: SocketStore, action) => {
        const { dataFormat } = action.model;
        return { ...state, dataFormat }
    },
    [actions.setSocketIsConnected]: (state: SocketStore, action) => {
        const { socketIsConnected } = action.model;
        return { ...state, socketIsConnected }
    },
    [actions.clearMessages]: (state: SocketStore, action) => {
        return { ...state, messages: [] }
    },
    [actions.addMessage]: (state: SocketStore, action) => {
        const { message } = action.model;
        if (!message.time) {
            message.time = new Date().getTime()
        }
        return { ...state, messages: [...state.messages, message] }
    },
    [actions.setNotSupportPolling]: (state: SocketStore, action) => {
        const { val } = action.model;
        localStorage.setItem(LOCALSTORAGE_KEYS.NOT_SUPPORT_POLLING, String(val))
        return { ...state, notSupportPolling: val }
    },
};

export default function (state = new SocketStore(), action) {
    const handler = actionHandlers[action.type];
    return handler ? handler(state, action) : state;
}

