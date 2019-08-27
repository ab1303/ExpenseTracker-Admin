import React from 'react';
import { Spin } from 'antd';
import styles from './index.scss';
function PageLoading() {
    return (React.createElement("div", { className: styles.pageLoading },
        React.createElement(Spin, { className: styles.spin })));
}
export default PageLoading;
//# sourceMappingURL=index.js.map