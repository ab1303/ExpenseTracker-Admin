import React from 'react';
import styles from './index.scss';
import Type from './Type';
import DataFormat from './DataFormat';
import Connect from './Connect';
import Send from './Send';
function Handler() {
    return (React.createElement("div", { className: styles.handler },
        React.createElement("div", { className: styles.head },
            React.createElement(Type, null),
            React.createElement(DataFormat, null)),
        React.createElement(Connect, null),
        React.createElement(Send, null)));
}
export default Handler;
//# sourceMappingURL=index.js.map