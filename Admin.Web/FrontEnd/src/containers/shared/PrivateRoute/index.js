import * as tslib_1 from "tslib";
import React from 'react';
import { observer } from 'mobx-react';
import { Route } from 'react-router-dom';
import { useOnMount } from '@utils/reactExt';
import useRootStore from '@store/useRootStore';
function PrivateRoute(_a) {
    var { component: Component } = _a, rest = tslib_1.__rest(_a, ["component"]);
    const { routerStore, authStore } = useRootStore();
    function checkLocalUserInfo() {
        if (!authStore.userInfo.token) {
            routerStore.history.replace('/login');
        }
    }
    useOnMount(checkLocalUserInfo);
    return React.createElement(Route, Object.assign({}, rest, { render: props => React.createElement(Component, Object.assign({}, props, rest)) }));
}
export default observer(PrivateRoute);
//# sourceMappingURL=index.js.map