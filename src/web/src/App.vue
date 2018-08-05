<template>
  <no-ssr>
    <div id="app">
      <nav class="navbar navigate-color is-transparent" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
          <router-link to="/" exact class="navbar-item">
            <img class="logo" src="~public/logo.png" alt="logo">
          </router-link>
          <div class="navbar-burger burger" onclick="document.querySelector('.navbar-menu').classList.toggle('is-active'); document.querySelector('.navbar-burger').classList.toggle('is-active');">            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
            <span aria-hidden="true"></span>
          </div>
        </div>
        <div  class="navbar-menu">
          <div class="navbar-start">
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
import NoSSR from 'vue-no-ssr'

export default {
  name: 'app',
  components: {
    'no-ssr': NoSSR
  },
  data() {
    return {
      username: undefined
    };
  },
  computed: {
    itemCount() {
      return this.$store.getters['cart/itemCount']
    },
    isLogged() {
      let userInfo = this.$store.getters["account/userInfo"] || {}
      if (userInfo.sub) return 'Logged!'
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

@media screen and (max-width: 1087px) {
  .navbar-menu {
    background-color: #445f71;
  }
}

@media screen and (max-width: 1087px) and (min-width: 678px) {
  .navbar-burger {
    display: none;
  }

  .navbar, .navbar-menu, .navbar-start, .navbar-end {
    align-items: stretch;
    display: flex;
  }

  .navbar-menu {
    margin-left: auto;
  }

  .navbar > .container {
    display: flex;
    justify-content: space-between;
    padding-left: 0.75rem;
    padding-right: 0.75rem;
  }
}
</style>
