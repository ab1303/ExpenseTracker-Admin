import React, { createContext } from 'react';
import { Observer } from 'mobx-react';
import * as store from '@store/index';
export const RootContext = createContext(null);
/**
 * 已包含Observer
 * @param param0
 */
export const RootConsumer = ({ children }) => React.createElement(Observer, null, () => children(store));
export default function Provider({ children }) {
    return React.createElement(RootContext.Provider, { value: Object.assign({}, store) }, children);
}
//# sourceMappingURL=Provider.js.map