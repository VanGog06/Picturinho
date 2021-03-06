import './App.css';

import React from 'react';
import { Alert } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom';

import { Users } from './components/admin/users/Users';
import { Albums } from './components/albums/Albums';
import { CreateAlbum } from './components/albums/create/CreateAlbum';
import { AlbumDetails } from './components/albums/details/AlbumDetails';
import { Footer } from './components/common/footer/Footer';
import { Header } from './components/common/Header';
import { PrivateRoute } from './components/common/PrivateRoute';
import { Explore } from './components/explore/Explore';
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
          <div className="col-md-12">
            {alert.message && (
              <Alert variant={alert.type}>{alert.message}</Alert>
            )}
            <Switch>
              <PrivateRoute exact path="/" component={Home} />
              <Route path="/login" component={Login} />
              <Route path="/register" component={Register} />
              <PrivateRoute path="/albums/create" component={CreateAlbum} />
              <PrivateRoute path="/albums/:id" component={AlbumDetails} />
              <PrivateRoute path="/albums" component={Albums} />
              <PrivateRoute path="/explore" component={Explore} />
              <PrivateRoute path="/admin/users" component={Users} />
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
