import React from 'react'
import classnames from 'classnames'
import { Layout, Icon, Switch } from 'antd'

import styles from './index.scss'
import SiderMenu from './Menu'
import { connect } from 'react-redux';

import { GlobalActionCreatorService } from '@reducers/globalReducer/service';

const Sider = ({ sideBarCollapsed, sideBarTheme, changeSiderTheme } ) => {
    const ChangeTheme = (
        <div className={classnames(styles.changeTheme, sideBarTheme === 'dark' && styles.dark)}>
            Switch Theme
            <Switch
                checkedChildren="dark"
                unCheckedChildren="light"
                checked={sideBarTheme === 'dark'}
                onChange={val => changeSiderTheme(val ? 'dark' : 'light')}
            />
        </div>
    )
    return (
        <Layout.Sider
            className={styles.sider}
            trigger={null}
            theme={sideBarTheme}
            collapsible
            collapsed={sideBarCollapsed}
        >
            <div className={classnames(styles.logoBox, sideBarTheme === 'dark' && styles.dark)}>
                <Icon type="ant-design" />
            </div>
            <SiderMenu />
            {!sideBarCollapsed && ChangeTheme}
        </Layout.Sider>
    )
}

function mapStateToProps(state) {
    const { globalReducer, authReducer } = state;
    return {
        ...globalReducer,
        ...authReducer,
    };
}

const globalActionCreatorService = new GlobalActionCreatorService();

export default connect(mapStateToProps, {
    changeSiderTheme: globalActionCreatorService.changeSiderTheme,
})(Sider)

