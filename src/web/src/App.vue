<template>
  <no-ssr>
    <div id="app">
      <nav class="navbar navigate-color is-transparent" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
          <router-link to="/" exact class="navbar-item">
            <img class="logo" src="~public/logo.png" alt="logo">
          </router-link>
          <div role="button" class="navbar-burger" data-target="navbarExampleTransparentExample" aria-label="menu" aria-expanded="false">
            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
          </div>
        </div>

        <div id="navbarExampleTransparentExample" class="navbar-menu">
          <div class="navbar-start">
            <span class="navbar-item">
              Cool Store
            </span>
          </div>
        </div>

        <div class="navbar-end">
          <router-link to="/" class="navbar-item">
            Home
          </router-link>
          <router-link to="/new" class="navbar-item">
            New Catalog
          </router-link>
          <router-link to="/cart" class="navbar-item">
            Carts ({{itemCount}})
          </router-link>
          <!-- <div class="navbar-item">
            {{isLogged}}
          </div> -->
          <a href="#" @click="logout" class="navbar-item router-link-exact-active router-link-active">
            Logout
          </a>
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
import { getItem } from './helper/storage'
import NoSSR from 'vue-no-ssr'

export default {
  name: 'app',
  components: {
    'no-ssr': NoSSR
  },
  data() {
    return {
      userId: undefined,
      username: undefined
    }
  },
  computed: {
    itemCount() {
      return this.$store.getters['cart/itemCount']
    },
    isLogged() {
      if (this.userId) return 'Logged!'
    }
  },
  beforeMount() {
    getUser(user => {
      if (user) this.userId = user.sub
    })
    var cartId = getItem('cartId')
    if (cartId) {
      this.$store.dispatch('cart/GET_CART')
    }
  },
  methods: {
    logout() {
      signoutRedirect(respnose => {
        this.$store.commit('account/LOGOUT')
      })
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

.navigate-color {
  background-color: #445f71;
}

.navigate-color .navbar-item {
  color: white;
}
</style>
