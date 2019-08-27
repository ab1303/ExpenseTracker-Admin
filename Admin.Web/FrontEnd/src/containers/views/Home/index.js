import React from 'react';
import { Layout } from 'antd';
import { HashRouter as Router, Switch, Route } from 'react-router-dom';
import styles from './index.scss';
import Error from '@components/Error';
import menu, { asynchronousComponents } from './menu';
import Header from './Header';
import Sider from './Sider';
function Home() {
    return (React.createElement(Layout, null,
        React.createElement(Sider, null),
        React.createElement(Layout, null,
            React.createElement(Header, null),
            React.createElement(Layout.Content, { className: styles.content },
                React.createElement(Router, null,
                    React.createElement(Switch, null,
                        menu.map(m => {
                            if (!m.path) {
                                return null;
                            }
                            return (React.createElement(Route, { key: m.id, exact: m.exact, path: m.path, component: m.component ? asynchronousComponents[m.component] : null }));
                        }),
                        React.createElement(Route, { component: Error })))))));
}
export default Home;
//# sourceMappingURL=index.js.map