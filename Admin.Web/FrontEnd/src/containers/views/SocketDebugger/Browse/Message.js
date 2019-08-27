import React from 'react';
import { observer, useLocalStore } from 'mobx-react';
import moment from 'moment';
import { Tag } from 'antd';
import styles from './index.scss';
function Message({ message, style }) {
    const selfStore = useLocalStore(() => ({
        get time() {
            return moment(message.time).format('h:mm:ss a');
        },
        get color() {
            if (message.from === 'browser') {
                return '#87d068';
            }
            else if (message.from === 'server') {
                return '#2db7f5';
            }
            return '#108ee9';
        },
        get fromText() {
            if (message.from === 'browser') {
                return 'You';
            }
            else if (message.from === 'server') {
                return 'Server';
            }
            return 'Console';
        },
        get content() {
            if (!message.data) {
                return null;
            }
            return typeof message.data === 'object' ? JSON.stringify(message.data) : message.data;
        }
    }));
    return (React.createElement("div", { className: styles.message, style: style },
        React.createElement("div", { className: styles.messageHeader, style: { marginBottom: !!selfStore.content ? 5 : 0 } },
            message.event && React.createElement(Tag, { color: "#f50" }, message.event),
            React.createElement(Tag, { color: selfStore.color }, selfStore.fromText),
            React.createElement("span", null, selfStore.time)),
        React.createElement("div", { className: styles.content }, selfStore.content)));
}
export default observer(Message);
//# sourceMappingURL=Message.js.map