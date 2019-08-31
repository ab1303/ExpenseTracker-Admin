import * as tslib_1 from "tslib";
import { observable, action, runInAction } from 'mobx';
import { StoreExt } from '@utils/reactExt';
export class UserStore extends StoreExt {
    constructor() {
        super(...arguments);
        /**
         * 加载用户列表时的loading
         *
         * @type {boolean}
         * @memberof UserStore
         */
        this.getUsersloading = false;
        /**
         * 用户列表
         *
         * @type {IUserStore.IUser[]}
         * @memberof UserStore
         */
        this.users = [];
        /**
         * table pageIndex
         *
         * @type {number}
         * @memberof UserStore
         */
        this.pageIndex = 1;
        /**
         * table pageSize
         *
         * @type {number}
         * @memberof UserStore
         */
        this.pageSize = 30;
        /**
         * users total
         *
         * @type {number}
         * @memberof UserStore
         */
        this.total = 0;
        /**
         * 加载用户列表
         *
         * @memberof UserStore
         */
        this.getUsers = () => tslib_1.__awaiter(this, void 0, void 0, function* () {
            this.getUsersloading = true;
            try {
                const res = yield this.api.user.getUsers({ pageIndex: this.pageIndex, pageSize: this.pageSize });
                runInAction('SET_USER_LIST', () => {
                    this.users = res.users;
                    this.total = res.total;
                });
            }
            catch (err) { }
            runInAction('HIDE_USER_LIST_LOADING', () => {
                this.getUsersloading = false;
            });
        });
        this.createUser = (user) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            yield this.api.user.createUser(user);
        });
        this.modifyUser = (user) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            yield this.api.user.modifyUser(user);
        });
        this.deleteUser = (_id) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            yield this.api.user.deleteUser({ _id });
            this.getUsers();
        });
        this.changePageIndex = (pageIndex) => {
            this.pageIndex = pageIndex;
            this.getUsers();
        };
        this.changePageSize = (pageSize) => {
            this.pageSize = pageSize;
            this.getUsers();
        };
        this.handleTableChange = (pagination) => {
            const { current, pageSize } = pagination;
            if (current !== this.pageIndex) {
                this.changePageIndex(current);
            }
            if (pageSize !== this.pageSize) {
                this.changePageSize(pageSize);
            }
        };
    }
}
tslib_1.__decorate([
    observable
], UserStore.prototype, "getUsersloading", void 0);
tslib_1.__decorate([
    observable
], UserStore.prototype, "users", void 0);
tslib_1.__decorate([
    observable
], UserStore.prototype, "pageIndex", void 0);
tslib_1.__decorate([
    observable
], UserStore.prototype, "pageSize", void 0);
tslib_1.__decorate([
    observable
], UserStore.prototype, "total", void 0);
tslib_1.__decorate([
    action
], UserStore.prototype, "getUsers", void 0);
tslib_1.__decorate([
    action
], UserStore.prototype, "changePageIndex", void 0);
tslib_1.__decorate([
    action
], UserStore.prototype, "changePageSize", void 0);
export default new UserStore();
//# sourceMappingURL=index.js.map