import React from 'react';
import { observer } from 'mobx-react';
import { Select } from 'antd';
import useRootStore from '@store/useRootStore';
import { LOCALSTORAGE_KEYS } from '@constants/index';
import { DATA_FORMATS } from '@constants/socket';
function DataFormat() {
    const { socketStore } = useRootStore();
    function handleChange(val) {
        socketStore.setDataFormat(val);
        localStorage.setItem(LOCALSTORAGE_KEYS.DATA_FORMAT, val);
    }
    return (React.createElement(Select, { value: socketStore.dataFormat, style: { width: 120 }, onChange: handleChange }, DATA_FORMATS.map(d => (React.createElement(Select.Option, { key: d, value: d }, d)))));
}
export default observer(DataFormat);
//# sourceMappingURL=DataFormat.js.map