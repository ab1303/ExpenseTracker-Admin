import './index.scss'

import React from 'react'
import ReactDOM from 'react-dom'

// import registerServiceWorker from './sw'
import App from '@shared/App'


// registerServiceWorker()
const render = (Component: React.ComponentType) => {
    ReactDOM.render(<Component />, document.getElementById('app'))
}

render(App)
