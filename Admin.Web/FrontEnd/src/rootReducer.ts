// reducers.js
import { combineReducers } from 'redux'
import { connectRouter } from 'connected-react-router'

import * as reducers from './reducers/index';


const createRootReducer = (history) => combineReducers({
  router: connectRouter(history),
  ...reducers,
  // rest of your reducers
})

export default createRootReducer