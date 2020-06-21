import './App.css';

import React from 'react';
import { Alert } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom';

import { Footer } from './components/common/footer/Footer';
import { Header } from './components/common/Header';
import { PrivateRoute } from './components/common/PrivateRoute';
import { Home } from './components/home/Home';
import { Login } from './components/login/Login';
import { Register } from './components/register/Register';
import { IState } from './store/IState';

function App() {
  const alert = useSelector((state: IState) => state.alert);

  return (
    <BrowserRouter>
      <Header />
      <div className="jumbotron">
        <div className="container">
          <div className="col-md-8 offset-md-2">
            {alert.message && (
              <Alert variant={alert.type}>{alert.message}</Alert>
            )}
            <Switch>
              <PrivateRoute exact path="/" component={Home} />
              <Route path="/login" component={Login} />
              <Route path="/register" component={Register} />
              <Redirect from="*" to="/" />
            </Switch>
          </div>
        </div>
      </div>
      <Footer />
    </BrowserRouter>
  );
}

export default App;
