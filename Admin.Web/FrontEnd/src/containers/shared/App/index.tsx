import React from 'react'
// import Loadable from 'react-loadable'
import { Switch, Route } from 'react-router-dom'

import styles from './index.scss'
// import PageLoading from '@components/PageLoading'
import Error from '@components/Error'
import IntlWrapper from './IntlWrapper'

import { Provider } from 'react-redux'
import { ConnectedRouter } from 'connected-react-router'
import configureStore, { history } from '../../../configureStore'

// const Home = Loadable({
//     loader: () => import(/* webpackChunkName: "home" */ '@views/Home'),
//     loading: PageLoading
// })
// const Login = Loadable({
//     loader: () => import(/* webpackChunkName: "login" */ '@views/Login'),
//     loading: PageLoading
// })

import Home from "@views/Home"
import Login from "@views/Login"



const AppWrapper = ({ children }: { children?: React.ReactNode }) => <div className={styles.appWrapper}>{children}</div>

const store = configureStore({  })

const App = () => {
    return (
        <Provider store={store}>
            <IntlWrapper>
                <AppWrapper>
                <ConnectedRouter history={history}>
                        <Switch>
                            <Route exact path="/login" component={Login} />
                            <Route path="/" component={Home} />
                            <Route component={Error} />
                        </Switch>
                    </ConnectedRouter>
                </AppWrapper>
            </IntlWrapper>
        </Provider>
    )
}

export default App
