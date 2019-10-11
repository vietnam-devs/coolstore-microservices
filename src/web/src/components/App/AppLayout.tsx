import React, { useState } from 'react'
import { withRouter } from 'react-router-dom'
import { Switch, Route } from 'react-router-dom'
import { Jumbotron, Container, Button } from 'reactstrap'
import { Navbar, Nav, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap'
import styled from 'styled-components'

import requireAuth from 'components/HOC/requireAuth'

import Home from 'pages/Home'
import ProductDetail from 'pages/ProductDetail'
import Callback from 'pages/Callback/Callback'
import SilentCallback from 'pages/Callback/SilentCallback'
import { AuthService } from 'services'

const StyledHeader = styled.a`
  text-decoration: none;
  &:hover {
    text-decoration: none;
  }
`

const StyledNavBar = styled(Navbar)`
  margin-top: -3rem;
`

const AppLayout = ({ location }: any) => {
  const [toggle, setToggle] = useState(false)

  return (
    <>
      <Jumbotron fluid>
        <Container fluid>
          <div>
            <h1 className="display-4">
              <StyledHeader href="/">CoolStore Microservices</StyledHeader>
            </h1>
            <p className="lead">
              A containerised microservices application consisting of services based on .NET Core, NodeJS and more
              running on Service Mesh. It demonstrates how to wire up small microservices into a larger application
              using microservice architectural principals
            </p>
          </div>
        </Container>
      </Jumbotron>

      <div>
        <StyledNavBar color="light" light>
          <span className="lead">
            Search Product&nbsp;<input type="textbox"></input>
          </span>
          <Nav className="ml-auto">
            <UncontrolledDropdown nav inNavbar>
              <DropdownToggle nav caret>
                Bob
              </DropdownToggle>
              <DropdownMenu right>
                <DropdownItem>
                  <a
                    href="#"
                    onClick={() => {
                      AuthService.signOut()
                    }}
                  >
                    Logout
                  </a>
                </DropdownItem>
              </DropdownMenu>
            </UncontrolledDropdown>
          </Nav>
        </StyledNavBar>
      </div>

      <Switch>
        <Route exact path={'/'} component={requireAuth(Home)} />
        <Route path={'/product/:id'} component={requireAuth(ProductDetail)} />
        <Route path={'/auth/callback'} component={Callback} />
        <Route path={'/auth/silent-callback'} component={SilentCallback} />
      </Switch>
    </>
  )
}

export default withRouter(AppLayout)
