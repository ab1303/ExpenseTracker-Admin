import React from 'react';
import { message, notification } from 'antd';
import * as api from '@services/api';
/**
 * extends component
 *
 * @export
 * @class ComponentExt
 * @extends {React.Component<P, S>}
 * @template P
 * @template S
 */
export class ComponentExt extends React.Component {
    constructor() {
        super(...arguments);
        this.api = api;
        this.$message = message;
        this.$notification = notification;
    }
}
/**
 * extends store
 *
 * @export
 * @class StoreExt
 */
export class StoreExt {
    constructor() {
        this.api = api;
        this.$message = message;
        this.$notification = notification;
    }
}
/**
 * componentDidMount in hook way
 *
 * @export
 * @param {() => any} onMount
 * @returns
 */
export function useOnMount(onMount) {
    return React.useEffect(() => {
        if (onMount) {
            onMount();
        }
    }, []);
}
/**
 * componentWillUnmount in hook way
 *
 * @export
 * @param {() => any} onUnmount
 * @returns
 */
export function useOnUnmount(onUnmount) {
    return React.useEffect(() => {
        return () => onUnmount && onUnmount();
    }, []);
}
//# sourceMappingURL=reactExt.js.map