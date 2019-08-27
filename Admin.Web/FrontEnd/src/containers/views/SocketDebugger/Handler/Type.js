import React from 'react';
import { observer } from 'mobx-react';
import { Radio } from 'antd';
import useRootStore from '@store/useRootStore';
import { LOCALSTORAGE_KEYS } from '@constants/index';
import { SOCKER_TYPES } from '@constants/socket';
function Type() {
    const { socketStore } = useRootStore();
    function handleTypeChange(e) {
        const { value } = e.target;
        socketStore.setSocketType(value);
        localStorage.setItem(LOCALSTORAGE_KEYS.SOCKET_TYPE, value);
    }
    return (React.createElement(Radio.Group, { onChange: handleTypeChange, value: socketStore.socketType, disabled: socketStore.socketIsConnected }, SOCKER_TYPES.map(s => (React.createElement(Radio.Button, { value: s, key: s }, s)))));
}
export default observer(Type);
//# sourceMappingURL=Type.js.map