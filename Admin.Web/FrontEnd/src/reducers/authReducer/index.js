import * as tslib_1 from "tslib";
import { observable, action, reaction } from 'mobx';
import { isPlainObject } from 'lodash';
import { StoreExt } from '@utils/reactExt';
import { routerStore } from './../';
import { initialUserInfo, syncUserInfo } from './syncUserInfo';
import { LOCALSTORAGE_KEYS } from '@constants/index';
export class AuthStore extends StoreExt {
    constructor() {
        super();
        /**
         * 用户信息
         *
         * @type {IAuthStore.UserInfo}
         * @memberof AuthStore
         */
        this.userInfo = initialUserInfo;
        this.login = (params) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            try {
                const res = yield this.api.auth.login(params);
                this.setUserInfo(isPlainObject(res) ? res : {});
                localStorage.setItem(LOCALSTORAGE_KEYS.USERINFO, JSON.stringify(res));
                routerStore.replace('/');
            }
            catch (err) {
                console.error(err);
            }
        });
        this.logout = () => {
            this.setUserInfo({});
            localStorage.removeItem(LOCALSTORAGE_KEYS.USERINFO);
            routerStore.replace('/login');
        };
        /**
         * 初始化用户信息
         *
         * @memberof AuthStore
         */
        this.setUserInfo = (userInfo) => {
            this.userInfo = userInfo;
            return userInfo;
        };
        reaction(() => this.userInfo, syncUserInfo);
    }
}
tslib_1.__decorate([
    observable
], AuthStore.prototype, "userInfo", void 0);
tslib_1.__decorate([
    action
], AuthStore.prototype, "login", void 0);
tslib_1.__decorate([
    action
], AuthStore.prototype, "setUserInfo", void 0);
export default new AuthStore();
//# sourceMappingURL=index.js.map