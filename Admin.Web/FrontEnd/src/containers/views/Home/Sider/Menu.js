import * as tslib_1 from "tslib";
import React from 'react';
import { observer } from 'mobx-react';
import { computed } from 'mobx';
import { Menu, Icon } from 'antd';
import pathToRegexp from 'path-to-regexp';
import styles from './index.scss';
import { RootConsumer } from '@shared/App/Provider';
import { arrayToTree, queryArray } from '@utils/index';
import menu from './../menu';
const { SubMenu } = Menu;
let SiderMenu = class SiderMenu extends React.Component {
    constructor() {
        super(...arguments);
        // 打开的菜单层级记录
        this.levelMap = {};
        this.goto = ({ key }) => {
            const { history } = this.props.routerStore;
            const selectedMenu = menu.find(item => String(item.id) === key);
            if (selectedMenu && selectedMenu.path && selectedMenu.path !== this.currentRoute) {
                history.push(selectedMenu.path);
            }
        };
        this.onOpenChange = (openKeys) => {
            const { navOpenKeys, setOpenKeys } = this.props;
            const latestOpenKey = openKeys.find(key => !navOpenKeys.includes(key));
            const latestCloseKey = navOpenKeys.find(key => !openKeys.includes(key));
            let nextOpenKeys = [];
            if (latestOpenKey) {
                nextOpenKeys = this.getAncestorKeys(latestOpenKey).concat(latestOpenKey);
            }
            if (latestCloseKey) {
                nextOpenKeys = this.getAncestorKeys(latestCloseKey);
            }
            setOpenKeys(nextOpenKeys);
        };
        this.getPathArray = (array, current) => {
            const result = [String(current.id)];
            const getPath = (item) => {
                if (item && item.pid) {
                    result.unshift(String(item.pid));
                    getPath(queryArray(array, String(item.pid), 'id'));
                }
            };
            getPath(current);
            return result;
        };
        // 保持选中
        this.getAncestorKeys = (key) => {
            const map = {};
            const getParent = index => {
                const result = [String(this.levelMap[index])];
                if (this.levelMap[result[0]]) {
                    result.unshift(getParent(result[0])[0]);
                }
                return result;
            };
            for (const index in this.levelMap) {
                if ({}.hasOwnProperty.call(this.levelMap, index)) {
                    map[index] = getParent(index);
                }
            }
            return map[key] || [];
        };
        // 递归生成菜单
        this.getMenus = (menuTree) => {
            return menuTree.map(item => {
                if (item.children) {
                    if (item.pid) {
                        this.levelMap[item.id] = item.pid;
                    }
                    return (React.createElement(SubMenu, { key: String(item.id), title: React.createElement("span", null,
                            item.icon && React.createElement(Icon, { type: item.icon }),
                            React.createElement("span", null, item.title)) }, this.getMenus(item.children)));
                }
                return (React.createElement(Menu.Item, { key: String(item.id) },
                    item.icon && React.createElement(Icon, { type: item.icon }),
                    React.createElement("span", null, item.title)));
            });
        };
    }
    get currentRoute() {
        return this.props.routerStore.location.pathname;
    }
    get menuTree() {
        return arrayToTree(menu, 'id', 'pid');
    }
    get menuProps() {
        const { sideBarCollapsed, navOpenKeys } = this.props;
        return !sideBarCollapsed
            ? {
                onOpenChange: this.onOpenChange,
                openKeys: navOpenKeys
            }
            : {};
    }
    render() {
        this.levelMap = {};
        const { sideBarTheme } = this.props;
        const menuItems = this.getMenus(this.menuTree);
        // 寻找选中路由
        let currentMenu = null;
        for (const item of menu) {
            if (item.path && pathToRegexp(item.path).exec(this.currentRoute)) {
                currentMenu = item;
                break;
            }
        }
        let selectedKeys = null;
        if (currentMenu) {
            selectedKeys = this.getPathArray(menu, currentMenu);
        }
        if (!selectedKeys) {
            selectedKeys = ['1'];
        }
        return (React.createElement(Menu, Object.assign({ className: styles.menu, theme: sideBarTheme, mode: "inline", selectedKeys: selectedKeys, onClick: this.goto }, this.menuProps), menuItems));
    }
};
tslib_1.__decorate([
    computed
], SiderMenu.prototype, "currentRoute", null);
tslib_1.__decorate([
    computed
], SiderMenu.prototype, "menuTree", null);
tslib_1.__decorate([
    computed
], SiderMenu.prototype, "menuProps", null);
SiderMenu = tslib_1.__decorate([
    observer
], SiderMenu);
function Wrapper() {
    return (React.createElement(RootConsumer, null, ({ routerStore, authStore, globalStore }) => (React.createElement(SiderMenu, { routerStore: routerStore, userInfo: authStore.userInfo, sideBarCollapsed: globalStore.sideBarCollapsed, sideBarTheme: globalStore.sideBarTheme, navOpenKeys: globalStore.navOpenKeys, setOpenKeys: globalStore.setOpenKeys }))));
}
export default Wrapper;
//# sourceMappingURL=Menu.js.map