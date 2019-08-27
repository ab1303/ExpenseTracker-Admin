import React from 'react';
import styles from './index.scss';
import Header from './Header';
import UserTable from './UserTable';
import AutoSizer from '@components/AutoSizer';
export default function Users() {
    return (React.createElement("div", { className: styles.container },
        React.createElement(Header, null),
        React.createElement(AutoSizer, { className: styles.tableBox }, ({ height }) => React.createElement(UserTable, { scrollY: height - 120 }))));
}
//# sourceMappingURL=index.js.map