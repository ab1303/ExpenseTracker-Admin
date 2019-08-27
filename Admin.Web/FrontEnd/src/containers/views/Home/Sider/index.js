import React from 'react';
import classnames from 'classnames';
import { observer } from 'mobx-react';
import { Layout, Icon, Switch } from 'antd';
import styles from './index.scss';
import useRootStore from '@store/useRootStore';
import SiderMenu from './Menu';
function Sider() {
    const { sideBarCollapsed, sideBarTheme, changeSiderTheme } = useRootStore().globalStore;
    const ChangeTheme = (React.createElement("div", { className: classnames(styles.changeTheme, sideBarTheme === 'dark' && styles.dark) },
        "Switch Theme",
        React.createElement(Switch, { checkedChildren: "dark", unCheckedChildren: "light", checked: sideBarTheme === 'dark', onChange: val => changeSiderTheme(val ? 'dark' : 'light') })));
    return (React.createElement(Layout.Sider, { className: styles.sider, trigger: null, theme: sideBarTheme, collapsible: true, collapsed: sideBarCollapsed },
        React.createElement("div", { className: classnames(styles.logoBox, sideBarTheme === 'dark' && styles.dark) },
            React.createElement(Icon, { type: "ant-design" })),
        React.createElement(SiderMenu, null),
        !sideBarCollapsed && ChangeTheme));
}
export default observer(Sider);
//# sourceMappingURL=index.js.map