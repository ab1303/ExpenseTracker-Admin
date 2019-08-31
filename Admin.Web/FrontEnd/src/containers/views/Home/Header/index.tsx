import React from 'react'
import { connect } from 'react-redux';
import { Layout, Icon } from 'antd'

import styles from './index.scss'
import { GITHUB_LINK } from '@constants/index'
import { AuthStoreService } from '@reducers/authReducer/service'

const Header = props => {
    return (
        <Layout.Header className={styles.header}>
            <Icon
                className={styles.trigger}
                type={props.globalReducer.sideBarCollapsed ? 'menu-unfold' : 'menu-fold'}
                onClick={props.globalReducer.toggleSideBarCollapsed}
            />
            <div className={styles.right}>
                <Icon
                    className={styles.rightIcon}
                    type="github"
                    theme="outlined"
                    onClick={() => window.open(GITHUB_LINK)}
                />
                <Icon className={styles.rightIcon} type="logout" theme="outlined" onClick={props.logout} />
            </div>
        </Layout.Header>
    )
}


function mapStateToProps(state) {
    const { globalReducer, authReducer } = state;
    return {
        globalReducer,
        authReducer,
    };
}

const authService = new AuthStoreService();

export default connect(mapStateToProps, {
    login: authService.login,
    logout: authService.logout
})(Header)
