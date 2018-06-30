<template>
  <div id="app" class="container">
    <header class="header">
      <nav class="inner">
        <router-link to="/" exact>
          <img class="logo" src="~public/logo-48.png" alt="logo">
        </router-link>
        <a class="github" href="https://github.com/vietnam-devs/coolstore-microservices" target="_blank" rel="noopener">
          Built with Vue.js
        </a>
      </nav>
    </header>
    <transition name="fade" mode="out-in">
      <router-view class="view"></router-view>
    </transition>
    <cs-footer></cs-footer>
  </div>
</template>

<script>
import Footer from './components/Footer.vue'

export default {
  name: 'app',
  components: {
    'cs-footer': Footer
  }
}
</script>

<style lang="stylus">
body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
  background-color: lighten(#fcfcfc, 30%);
  margin: 0;
  padding-top: 55px;
  color: #34495e;
}

a {
  color: #34495e;
  text-decoration: none;
}

.header {
  background-color: #8400ff;
  position: fixed;
  z-index: 999;
  height: 55px;
  top: 0;
  left: 0;
  right: 0;

  .inner {
    max-width: 800px;
    box-sizing: border-box;
    margin: 0px auto;
    padding: 15px 5px;
  }

  a {
    color: rgba(255, 255, 255, 0.8);
    line-height: 24px;
    transition: color 0.15s ease;
    display: inline-block;
    vertical-align: middle;
    font-weight: 300;
    letter-spacing: 0.075em;
    margin-right: 1.8em;

    &:hover {
      color: #fff;
    }

    &.router-link-active {
      color: #fff;
      font-weight: 400;
    }

    &:nth-child(6) {
      margin-right: 0;
    }
  }

  .github {
    color: #fff;
    font-size: 0.9em;
    font-weight: bold;
    margin: 0;
    float: right;
  }
}

.logo {
  width: 24px;
  margin-right: 10px;
  display: inline-block;
  vertical-align: middle;
}

.fade-enter-active, .fade-leave-active {
  transition: all 0.2s ease;
}

.fade-enter, .fade-leave-active {
  opacity: 0;
}
</style>
