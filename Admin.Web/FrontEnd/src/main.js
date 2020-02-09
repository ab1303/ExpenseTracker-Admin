import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppRoot } from "./components/AppRoot";
//make sure TypeScript is working
var message = "this is the client";
console.log(message);
ReactDOM.render(React.createElement(AppRoot, null), document.getElementById('root'));
