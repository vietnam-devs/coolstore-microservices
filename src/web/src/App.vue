<template>
  <no-ssr>
    <section id="app" class="hero">
      <div class="hero-head">
        <cs-header></cs-header>
      </div>
      <div class="hero-body">
        <transition name="fade" mode="out-in">
          <router-view class="view"></router-view>
        </transition>
      </div>
      <div class="hero-footer">
        <cs-footer></cs-footer>
      </div>
    </section>
  </no-ssr>
</template>

<script>
import Header from "./components/Header.vue";
import Footer from "./components/Footer.vue";
import NoSSR from "vue-no-ssr";

export default {
  name: "app",
  components: {
    "no-ssr": NoSSR
  },
  data() {
    return {
      username: undefined
    };
  },
  computed: {
    isLogged() {
      let userInfo = this.$store.getters["account/userInfo"] || {};
      if (userInfo.sub) return "Logged!";
    }
  },
  components: {
    "cs-footer": Footer,
    "cs-header": Header
  }
};
</script>

<style lang="stylus">
@require './layouts/css/_transition';
@require './layouts/css/_slider';

html {
  overflow-y: auto;
}

.section {
  padding: 3rem 0 1.5rem;
}

.hero {
  min-height: 100vh;

  .hero-body {
    flex: 1;
  }

  .hero-footer {
    margin-bottom: 0.5rem;
  }
}

.is-light {
  background-color: #f5f5f5;
  color: #363636;
}

$card-radius = 5px;

.is-radius {
  border-radius: $card-radius;
}

@media (max-width: 600px) {
  aside {
    width: 100% !important;
    margin-bottom: 10px !important;
  }

  .content {
    width: 100% !important;
    grid-template-columns: 1fr !important;
  }
}

@media (min-width: 601px) and (max-width: 900px) {
  .content {
    grid-template-columns: repeat(2, 1fr) !important;
  }
}
</style>
