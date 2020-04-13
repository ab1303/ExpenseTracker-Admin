import './index.scss';
import React from 'react';
import ReactDOM from 'react-dom';
// import registerServiceWorker from './sw'
import App from '@shared/App';
// registerServiceWorker()
const render = (Component) => {
    ReactDOM.render(React.createElement(Component, null), document.getElementById('app'));
};
render(App);
//# sourceMappingURL=index.js.map