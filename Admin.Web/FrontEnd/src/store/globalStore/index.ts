import { StoreExt } from '@utils/reactExt'
import { LOCALSTORAGE_KEYS } from '@constants/index'

class GlobalStore extends StoreExt {
    sideBarCollapsed: boolean = localStorage.getItem(LOCALSTORAGE_KEYS.SIDE_BAR_COLLAPSED) === '1'

    sideBarTheme: IGlobalStore.SideBarTheme =
        (localStorage.getItem(LOCALSTORAGE_KEYS.SIDE_BAR_THEME) as IGlobalStore.SideBarTheme) || 'light'

    navOpenKeys: string[] = JSON.parse(localStorage.getItem(LOCALSTORAGE_KEYS.NAV_OPEN_KEYS)) || []
}

const actions = {
    toggleSideBarCollapsed: 'toggleSideBarCollapsed',
    changeSiderTheme: 'changeSiderTheme',
    setOpenKeys: 'setOpenKeys',
}


const actionHandlers = {
    [actions.toggleSideBarCollapsed]: (state: GlobalStore, action) => {
        const { sideBarCollapsed } = action.model;
        localStorage.setItem(LOCALSTORAGE_KEYS.SIDE_BAR_COLLAPSED, sideBarCollapsed ? '1' : '0');
        return { ...state, sideBarCollapsed: !state.sideBarCollapsed };
    },
    [actions.changeSiderTheme]: (state: GlobalStore, action) => {
        const { theme } = action.model;

        localStorage.setItem(LOCALSTORAGE_KEYS.SIDE_BAR_THEME, theme);
        return { ...state, sideBarTheme: theme };
    },
    [actions.setOpenKeys]: (state: GlobalStore, action) => {
        const { openKeys } = action.model;
        localStorage.setItem(LOCALSTORAGE_KEYS.NAV_OPEN_KEYS, JSON.stringify(openKeys))
        return { ...state, navOpenKeys: openKeys };
    }
};

export default function (state = new GlobalStore(), action) {
    const handler = actionHandlers[action.type];
    return handler ? handler(state, action) : state;
}
