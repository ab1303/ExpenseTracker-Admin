import React from 'react'
import { Layout } from 'antd'
import { HashRouter as Router, Switch, Route } from 'react-router-dom'

import styles from './index.scss'
import Error from '@components/Error'
// import menu, { asynchronousComponents } from './menu'
import menu from './menu'
import Header from './Header'
import Users from '@views/Users/index'


import Sider from './Sider'

const TestRender = () => (
    <div>
        Just a fake test
    </div>
);


function Home() {
    return (
        <Layout>
            {/* <Sider /> */}
            <Layout>
                <Header />
                <Layout.Content className={styles.content}>
                    <Router>
                        <Switch>
                            {menu.map(m => {
                                if (!m.path) {
                                    return null
                                }
                                return (
                                    <Route
                                        key={m.id}
                                        exact={m.exact}
                                        path={m.path}
                                        // component={m.component ? asynchronousComponents[m.component] : null}
                                        component={TestRender}
                                    />
                                )
                            })}
                            <Route component={Error} />
                        </Switch>
                    </Router>
                </Layout.Content>
            </Layout>
        </Layout>
    )
}

export default Home
