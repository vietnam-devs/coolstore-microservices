import React from 'react'
import './landing.css'

const LandingPage = () => (
  <div className="landing">
    <h1>Vietnam Developer Group</h1>
    <p>
      The projects and communities in Vietnam come together to get better things
      in open-source world.
    </p>

    <p>
      <h3>Microservices and Cloud-native projects</h3>
      <ul>
        <li>
          <a href="https://github.com/vietnam-devs/coolstore-microservices">
            coolstore-microservices
          </a>
        </li>
        <li>
          <a href="https://github.com/cloudnative-netcore/netcorekit">
            netcorekit
          </a>
        </li>
      </ul>
    </p>

    <p>
      <h3>Code Quality and Clean Code projects</h3>
      <ul>
        <li>
          <a href="https://github.com/thangchung/awesome-dotnet-core">
            awesome-dotnet-core
          </a>
        </li>
        <li>
          <a href="https://github.com/thangchung/clean-code-dotnet">
            clean-code-dotnet
          </a>
        </li>
      </ul>
    </p>
  </div>
)

export default LandingPage
