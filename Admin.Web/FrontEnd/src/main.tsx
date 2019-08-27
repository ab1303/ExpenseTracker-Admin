import * as React from 'react';
import * as ReactDOM from 'react-dom'
import {AppRoot} from "./components/AppRoot";

//make sure TypeScript is working
const message: string = "this is the client";
console.log(message);

ReactDOM.render(<AppRoot/>, document.getElementById('root'));