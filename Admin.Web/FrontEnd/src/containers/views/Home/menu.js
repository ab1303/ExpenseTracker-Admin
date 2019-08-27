import Loadable from 'react-loadable';
import PageLoading from '@components/PageLoading';
const loadComponent = (loader) => Loadable({ loader, loading: PageLoading });
export const asynchronousComponents = {
    SocketDebugger: loadComponent(() => import(/* webpackChunkName: "socket-debugger" */ '@views/SocketDebugger')),
    Users: loadComponent(() => import(/* webpackChunkName: "users" */ '@views/Users'))
};
export const menu = [
    {
        id: 1,
        path: '/',
        title: 'SocketDebugger',
        icon: 'coffee',
        component: 'SocketDebugger',
        exact: true
    },
    {
        id: 2,
        path: '/users',
        title: 'Users',
        icon: 'user',
        component: 'Users',
        exact: true
    }
];
export default menu;
//# sourceMappingURL=menu.js.map