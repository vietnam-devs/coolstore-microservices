import React from 'react'
import { Container } from 'reactstrap'

const Footer = () => {
  return (
    <footer className="footer text-center">
      <Container fluid className="clearfix">
        <span className="text-muted d-block text-center text-sm-left d-sm-inline-block">
          Copyright © 2019 CoolStore Application. All rights reserved.
        </span>
      </Container>
    </footer>
  )
}

export default Footer
