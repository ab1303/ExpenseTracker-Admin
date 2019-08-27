import React from 'react';
import { observer } from 'mobx-react';
import { Layout, Icon } from 'antd';
import styles from './index.scss';
import useRootStore from '@store/useRootStore';
import { GITHUB_LINK } from '@constants/index';
function Header() {
    const { globalStore, authStore } = useRootStore();
    return (React.createElement(Layout.Header, { className: styles.header },
        React.createElement(Icon, { className: styles.trigger, type: globalStore.sideBarCollapsed ? 'menu-unfold' : 'menu-fold', onClick: globalStore.toggleSideBarCollapsed }),
        React.createElement("div", { className: styles.right },
            React.createElement(Icon, { className: styles.rightIcon, type: "github", theme: "outlined", onClick: () => window.open(GITHUB_LINK) }),
            React.createElement(Icon, { className: styles.rightIcon, type: "logout", theme: "outlined", onClick: authStore.logout }))));
}
export default observer(Header);
//# sourceMappingURL=index.js.map