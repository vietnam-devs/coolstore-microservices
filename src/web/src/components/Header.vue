<template>
  <header
    class="navbar is-light"
    :class="{ 'is-fixed-top': isIndexRoute }"
    role="navigation"
    aria-label="main navigation"
  >
    <div class="container is-flex-touch">
      <div class="navbar-brand">
        <router-link to="/" class="navbar-item">
          <strong>
            <i>CoolStore Shop</i>
          </strong>
        </router-link>
      </div>
      <div class="navbar-end is-flex-touch">
        <div class="navbar-item">
          <div class="field">
            <p class="control">
              <router-link to="/new" class="button is-light">
                <span class="icon cartitem">
                  <i class="fa fa-plus-circle"></i>
                </span>
                <span class="is-hidden-mobile">New</span>
              </router-link>
            </p>
          </div>
        </div>
        <div class="navbar-item">
          <div class="field">
            <p class="control">
              <router-link to="/cart" class="button is-light">
                <span class="icon cartitem">
                  <div class="cartcount" v-if="total &gt; 0">{{ total }}</div>
                  <i class="fa fa-shopping-cart"></i>
                </span>
                <span class="is-hidden-mobile">Cart</span>
              </router-link>
            </p>
          </div>
        </div>
        <div class="navbar-item">
          <div class="field">
            <p class="control">
              <a href="#" @click="logout" class="button is-light">
                <span class="icon cartitem">
                  <i class="fa fa-sign-out-alt"></i>
                </span>
                <span class="is-hidden-mobile">Logout</span>
              </a>
            </p>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>
<script>
import { createNamespacedHelpers } from "vuex";
import { getUser, signoutRedirect } from "../auth/usermanager";
export default {
  name: "AppHeader",
  head() {
    return {
      htmlAttrs: {
        class: this.isIndexRoute && "has-navbar-fixed-top"
      }
    };
  },
  computed: {
    total() {
      return this.$store.getters["cart/itemCount"];
    },
    isIndexRoute() {
      return this.$route.name === "index";
    }
  },
  methods: {
    logout() {
      signoutRedirect(respnose => {
        this.$store.commit("account/LOGOUT");
      });
    }
  }
};
</script>

<style lang="stylus">
.hero-head {
  .navbar {
    &.is-light {
      background-color: #f5f5f5;
    }

    > .container {
      flex-wrap: wrap;
      justify-content: space-between;
    }
  }

  .cartitem {
    position: relative;
    float: right;
  }

  .cartcount {
    font-family: 'Barlow', sans-serif;
    position: absolute;
    background: #ff2211;
    color: white;
    text-align: center;
    padding-top: 2px;
    height: 18px;
    width: @height;
    font-size: 10px;
    margin: -8px 0 0 8px;
    border-radius: 50%;
    font-weight: 700;
  }

  @media (max-width: 600px) {
    .button {
      padding-left: 0.2rem;
      padding-right: @padding-left;

      .icon {
        &:first-child {
          &:not(:last-child) {
            margin-left: 0;
            margin-right: 0;
          }
        }
      }
    }

    .navbar-item {
      padding-left: 0.5rem;
      padding-right: @padding-left;
    }
  }
}
</style>
