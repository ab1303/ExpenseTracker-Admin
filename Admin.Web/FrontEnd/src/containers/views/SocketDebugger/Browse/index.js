import React from 'react';
import { reaction } from 'mobx';
import { observer } from 'mobx-react';
import { AutoSizer } from 'react-virtualized/dist/es/AutoSizer';
import { CellMeasurerCache, CellMeasurer } from 'react-virtualized/dist/es/CellMeasurer';
import { List as VList } from 'react-virtualized/dist/es/List';
import styles from './index.scss';
import useRootStore from '@store/useRootStore';
import { useOnMount } from '@utils/reactExt';
import Message from './Message';
function Browse() {
    const { socketStore } = useRootStore();
    const vList = React.useRef(null);
    const measureCache = new CellMeasurerCache({
        fixedWidth: true,
        minHeight: 43
    });
    function handleMessagesChanged(len) {
        if (len === 0) {
            return measureCache.clearAll();
        }
        if (vList.current) {
            vList.current.scrollToRow(len - 1);
        }
    }
    function listenMessagesLen() {
        return reaction(() => socketStore.messages.length, handleMessagesChanged);
    }
    useOnMount(listenMessagesLen);
    function renderItem({ index, key, parent, style }) {
        const item = socketStore.messages[index];
        return (React.createElement(CellMeasurer, { cache: measureCache, columnIndex: 0, key: key, parent: parent, rowIndex: index },
            React.createElement(Message, { style: style, message: item })));
    }
    const rowCount = socketStore.messages.length;
    return (React.createElement("div", { className: styles.browse },
        React.createElement(AutoSizer, null, ({ width, height }) => (React.createElement(VList, { width: width, height: height, ref: vList, overscanRowCount: 0, rowCount: rowCount, deferredMeasurementCache: measureCache, rowHeight: measureCache.rowHeight, rowRenderer: renderItem })))));
}
export default observer(Browse);
//# sourceMappingURL=index.js.map