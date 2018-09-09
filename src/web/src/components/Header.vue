<template>
  <header class="navbar is-light" :class="{ 'is-fixed-top': isIndexRoute }" role="navigation" aria-label="main navigation">
      <div class="container is-flex-touch">
          <div class="navbar-brand">
              <nuxt-link class="navbar-item" exact="exact" :to="{name: 'index'}"><strong><i>PlusGrosLeLogo</i></strong></nuxt-link>
          </div>
          <div class="navbar-end is-flex-touch">
              <div class="navbar-item">
                  <div class="field">
                      <p class="control"><a class="button is-light is-marginless-mobile" target="_blank" href="https://github.com/14nrv/buefy-shop" rel="noopener"><span class="icon"><i class="fab fa-github"></i></span><span class="is-hidden-mobile">Fork</span></a></p>
                  </div>
              </div>
              <div class="navbar-item">
                  <div class="field">
                      <p class="control">
                          <nuxt-link class="button is-light" exact="exact" :to="{name: 'cart'}"><span class="icon cartitem"><div class="cartcount" v-if="total &gt; 0">{{ total }}</div><i class="fa fa-shopping-cart"></i></span><span class="is-hidden-mobile">Cart</span></nuxt-link>
                      </p>
                  </div>
              </div>
          </div>
      </div>
  </header>
</template>
<script>
import { createNamespacedHelpers } from "vuex";
const { mapGetters } = createNamespacedHelpers("cart");
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
    ...mapGetters(["total"]),
    isIndexRoute() {
      return this.$route.name === "index";
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
