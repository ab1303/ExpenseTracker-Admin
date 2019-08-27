import * as tslib_1 from "tslib";
import { observable, action } from 'mobx';
import { StoreExt } from '@utils/reactExt';
import { LOCALSTORAGE_KEYS } from '@constants/index';
export class GlobalStore extends StoreExt {
    constructor() {
        super(...arguments);
        /**
         * 菜单栏折叠
         *
         * @type {boolean}
         * @memberof GlobalStore
         */
        this.sideBarCollapsed = localStorage.getItem(LOCALSTORAGE_KEYS.SIDE_BAR_COLLAPSED) === '1';
        /**
         * 菜单栏主题
         *
         * @type {IGlobalStore.SideBarTheme}
         * @memberof GlobalStore
         */
        this.sideBarTheme = localStorage.getItem(LOCALSTORAGE_KEYS.SIDE_BAR_THEME) || 'light';
        /**
         * 打开的菜单key
         *
         * @type {string[]}
         * @memberof GlobalStore
         */
        this.navOpenKeys = JSON.parse(localStorage.getItem(LOCALSTORAGE_KEYS.NAV_OPEN_KEYS)) || [];
        this.toggleSideBarCollapsed = () => {
            this.sideBarCollapsed = !this.sideBarCollapsed;
            localStorage.setItem(LOCALSTORAGE_KEYS.SIDE_BAR_COLLAPSED, this.sideBarCollapsed ? '1' : '0');
        };
        this.changeSiderTheme = (theme) => {
            this.sideBarTheme = theme;
            localStorage.setItem(LOCALSTORAGE_KEYS.SIDE_BAR_THEME, theme);
        };
        this.setOpenKeys = (openKeys) => {
            this.navOpenKeys = openKeys;
            localStorage.setItem(LOCALSTORAGE_KEYS.NAV_OPEN_KEYS, JSON.stringify(openKeys));
        };
    }
}
tslib_1.__decorate([
    observable
], GlobalStore.prototype, "sideBarCollapsed", void 0);
tslib_1.__decorate([
    observable
], GlobalStore.prototype, "sideBarTheme", void 0);
tslib_1.__decorate([
    observable
], GlobalStore.prototype, "navOpenKeys", void 0);
tslib_1.__decorate([
    action
], GlobalStore.prototype, "toggleSideBarCollapsed", void 0);
tslib_1.__decorate([
    action
], GlobalStore.prototype, "changeSiderTheme", void 0);
tslib_1.__decorate([
    action
], GlobalStore.prototype, "setOpenKeys", void 0);
export default new GlobalStore();
//# sourceMappingURL=index.js.map