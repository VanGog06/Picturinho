import { applyMiddleware, combineReducers, createStore } from 'redux';
import { createLogger } from 'redux-logger';
import thunkMiddleware from 'redux-thunk';

import { alertReducer } from './alert/AlertReducer';
import { authenticationReducer } from './user/AuthenticationReducer';
import { registrationReducer } from './user/RegistrationReducer';
import { userReducer } from './user/UserReducer';

const loggerMiddleware = createLogger();

export const store = createStore(
  combineReducers({
    alert: alertReducer,
    authentication: authenticationReducer,
    registration: registrationReducer,
    user: userReducer,
  }),
  applyMiddleware(thunkMiddleware, loggerMiddleware)
);
