import React from 'react';
import styles from './index.scss';
import Handler from './Handler';
import Browse from './Browse';
function SocketDebugger() {
    return (React.createElement("div", { className: styles.container },
        React.createElement(Handler, null),
        React.createElement(Browse, null)));
}
export default SocketDebugger;
//# sourceMappingURL=index.js.map