import { UserActions, UserActionTypes } from './UserActions';

const localStorageUser = localStorage.getItem("user");
let user = {};
if (localStorageUser) {
  user = { ...JSON.parse(localStorageUser), loggedIn: false };
}
const initialState = user ? { loggedIn: true, user } : {};

export const authenticationReducer = (
  state = initialState,
  action: UserActions
) => {
  switch (action.type) {
    case UserActionTypes.LOGIN_REQUEST:
      return {
        ...state,
        loggingIn: true,
      };
    case UserActionTypes.LOGIN_SUCCESS:
      return {
        ...state,
        loggingIn: false,
        loggedIn: true,
        user: action.user,
      };
    case UserActionTypes.LOGIN_FAILURE:
      return {
        ...state,
        loggingIn: false,
        loggedIn: false,
        user: {},
      };
    case UserActionTypes.LOGOUT:
      return {
        ...state,
        loggingIn: false,
        loggedIn: false,
        user: {},
      };
    default:
      return { ...state };
  }
};
