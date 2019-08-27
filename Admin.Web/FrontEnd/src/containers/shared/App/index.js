import React from 'react';
import Loadable from 'react-loadable';
import { HashRouter, Router, Switch, Route } from 'react-router-dom';
import { createHashHistory } from 'history';
import { syncHistoryWithStore } from 'mobx-react-router';
import styles from './index.scss';
import * as store from '@store/index';
import PageLoading from '@components/PageLoading';
import Error from '@components/Error';
import Provider from './Provider';
import IntlWrapper from './IntlWrapper';
const hashHistory = createHashHistory();
const history = syncHistoryWithStore(hashHistory, store.routerStore);
const Home = Loadable({
    loader: () => import(/* webpackChunkName: "home" */ '@views/Home'),
    loading: PageLoading
});
const Login = Loadable({
    loader: () => import(/* webpackChunkName: "login" */ '@views/Login'),
    loading: PageLoading
});
const AppWrapper = ({ children }) => React.createElement("div", { className: styles.appWrapper }, children);
function App() {
    return (React.createElement(Provider, null,
        React.createElement(IntlWrapper, null,
            React.createElement(AppWrapper, null,
                React.createElement(Router, { history: history },
                    React.createElement(HashRouter, null,
                        React.createElement(Switch, null,
                            React.createElement(Route, { exact: true, path: "/login", component: Login }),
                            React.createElement(Route, { path: "/", component: Home }),
                            React.createElement(Route, { component: Error }))))))));
}
export default App;
//# sourceMappingURL=index.js.map