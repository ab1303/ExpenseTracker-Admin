import React from 'react';
import { observer } from 'mobx-react';
import { message, Input, Button, Checkbox } from 'antd';
import styles from './index.scss';
import useRootStore from '@store/useRootStore';
import { socketConnect, socketDisconnect } from '@services/websocket';
import { LOCALSTORAGE_KEYS } from '@constants/index';
function Connect() {
    const { socketStore } = useRootStore();
    const [url, setUrl] = React.useState(localStorage.getItem(LOCALSTORAGE_KEYS.SOCKET_URL));
    function handleChange(e) {
        const { value } = e.target;
        setUrl(value);
        localStorage.setItem(LOCALSTORAGE_KEYS.SOCKET_URL, value);
    }
    function handleConnect() {
        if (!url) {
            message.destroy();
            return message.error('Please input socket url!');
        }
        socketConnect(url);
        socketStore.clearMessages();
    }
    return (React.createElement("div", { className: styles.container },
        React.createElement("div", { className: styles.connect },
            React.createElement(Input, { className: styles.socketUrlInput, value: url, onChange: handleChange }),
            socketStore.isSocketIO && (React.createElement(Checkbox, { disabled: socketStore.socketIsConnected, className: styles.checkbox, checked: socketStore.notSupportPolling, onChange: e => socketStore.setNotSupportPolling(e.target.checked) }, "no polling")),
            React.createElement(Button, { className: styles.btn, type: "primary", onClick: handleConnect, disabled: socketStore.socketIsConnected }, "connect"),
            React.createElement(Button, { className: styles.btn, type: "danger", onClick: socketDisconnect, disabled: !socketStore.socketIsConnected }, "disconnect")),
        React.createElement("blockquote", { className: styles.tips },
            "protocol//ip or domain:host (example:",
            ' ',
            socketStore.isSocketIO ? 'wss://showcase.jackple.com' : 'ws://127.0.0.1:3001',
            ")")));
}
export default observer(Connect);
//# sourceMappingURL=Connect.js.map