import * as tslib_1 from "tslib";
import { observable, action, computed } from 'mobx';
import { StoreExt } from '@utils/reactExt';
import { LOCALSTORAGE_KEYS } from '@constants/index';
import { SOCKER_TYPES, DATA_FORMATS } from '@constants/socket';
/**
 * socket debugger store
 *
 * @export
 * @class SocketStore
 * @extends {StoreExt}
 */
export class SocketStore extends StoreExt {
    constructor() {
        super(...arguments);
        this.socketType = localStorage.getItem(LOCALSTORAGE_KEYS.SOCKET_TYPE) || SOCKER_TYPES[0];
        this.dataFormat = localStorage.getItem(LOCALSTORAGE_KEYS.DATA_FORMAT) || DATA_FORMATS[0];
        this.socketIsConnected = false;
        this.messages = [];
        this.notSupportPolling = localStorage.getItem(LOCALSTORAGE_KEYS.NOT_SUPPORT_POLLING) === 'true';
        this.setSocketType = (type) => {
            this.socketType = type;
        };
        this.setDataFormat = (dataFormat) => {
            this.dataFormat = dataFormat;
        };
        this.setSocketIsConnected = (socketIsConnected) => {
            this.socketIsConnected = socketIsConnected;
        };
        this.clearMessages = () => {
            this.messages = [];
        };
        this.addMessage = (message) => {
            if (!message.time) {
                message.time = new Date().getTime();
            }
            this.messages.push(message);
        };
        this.setNotSupportPolling = (val) => {
            this.notSupportPolling = val;
            localStorage.setItem(LOCALSTORAGE_KEYS.NOT_SUPPORT_POLLING, String(val));
        };
    }
    get isSocketIO() {
        return this.socketType === SOCKER_TYPES[0];
    }
}
tslib_1.__decorate([
    observable
], SocketStore.prototype, "socketType", void 0);
tslib_1.__decorate([
    observable
], SocketStore.prototype, "dataFormat", void 0);
tslib_1.__decorate([
    observable
], SocketStore.prototype, "socketIsConnected", void 0);
tslib_1.__decorate([
    observable
], SocketStore.prototype, "messages", void 0);
tslib_1.__decorate([
    observable
], SocketStore.prototype, "notSupportPolling", void 0);
tslib_1.__decorate([
    computed
], SocketStore.prototype, "isSocketIO", null);
tslib_1.__decorate([
    action
], SocketStore.prototype, "setSocketType", void 0);
tslib_1.__decorate([
    action
], SocketStore.prototype, "setDataFormat", void 0);
tslib_1.__decorate([
    action
], SocketStore.prototype, "setSocketIsConnected", void 0);
tslib_1.__decorate([
    action
], SocketStore.prototype, "clearMessages", void 0);
tslib_1.__decorate([
    action
], SocketStore.prototype, "addMessage", void 0);
tslib_1.__decorate([
    action
], SocketStore.prototype, "setNotSupportPolling", void 0);
export default new SocketStore();
//# sourceMappingURL=index.js.map