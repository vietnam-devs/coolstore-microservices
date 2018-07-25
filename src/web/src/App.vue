<template>
  <no-ssr>
    <div id="app">
      <nav class="navbar is-transparent" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
          <router-link to="/" exact class="navbar-item">
            <img class="logo" src="~public/logo-48.png" alt="logo">
          </router-link>
          <div role="button" class="navbar-burger" data-target="navbarExampleTransparentExample" aria-label="menu" aria-expanded="false">
            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
          </div>
        </div>

        <div id="navbarExampleTransparentExample" class="navbar-menu">
          <div class="navbar-start">
            <router-link to="/" exact class="navbar-item">
              Home
            </router-link>
            <router-link to="/cart" class="navbar-item">
              Your Shopping Cart
            </router-link>
          </div>
        </div>

        <div class="navbar-end">
          <div class="navbar-item">
            Hi {{userId}}
          </div>
          <div class="navbar-item">
            <a class="button is-inverted" @click="logout">Logout</a>
          </div>
        </div>
      </nav>

      <transition name="fade" mode="out-in">
        <router-view class="view"></router-view>
      </transition>
      <cs-footer></cs-footer>
    </div>
  </no-ssr>
</template>

<script>
import Footer from './components/Footer.vue'
import { getUser, signoutRedirect } from './auth/usermanager'

export default {
  name: 'app',
  data() {
    return {
      userId: undefined,
      username: undefined
    }
  },
  beforeMount() {
    getUser(user => {
      if (user) this.userId = user.sub
    })
  },
  methods: {
    logout() {
      this.$store.commit('LOGOUT')
      signoutRedirect(respnose => {})
    }
  },
  components: {
    'cs-footer': Footer
  }
}
</script>

<style lang="stylus">
.fade-enter-active, .fade-leave-active {
  transition: all 0.2s ease;
}

.fade-enter, .fade-leave-active {
  opacity: 0;
}
</style>
