<template lang="pug">
 <aside class="is-light is-radius">
    <div class="sidearea">
        <label class="subtitle is-5" for="pricerange">
            Highest Price:<span> ${{ pricerange }}</span>
        </label>
        <input class="slider" id="pricerange" type="range" :value="pricerange" :min="min" :max="max" step="1" @input="updateHighprice($event.target.value)" />
        <span class="min is-pulled-left">${{ min }}</span>
        <span class="max is-pulled-right">${{ max }}</span>
    </div>
    <!-- <app-switch v-if="!sale"></app-switch>
    <div class="sidearea"><label class="subtitle is-5" for="category">Categories</label>
        <div class="select">
          <select id="category" @input="setCategory($event.target.value)">
            <option v-for="category in categories" :key="category" :selected="category === categorySelected" :value="category">{{ category }}</option>
              </select>
              </div>
    </div> -->
    <div class="sidearea">
        <h4 class="subtitle is-5">Special Sale!</h4>
        <p>Shop now because half our items are greatly reduced</p>
    </div>
    <div class="sidearea">
        <h4 class="subtitle is-5">Contact Us</h4>
        <p>Questions? Call us at 1-888-555-SHOP, we're happy to be of service.</p>
    </div>
</aside>
</template> 
<script>
import { createNamespacedHelpers } from "vuex";
// import Switch from "./Switch.vue";
// const { mapActions, mapGetters } = createNamespacedHelpers("products/products");
export default {
  name: "Sidebar",
  components: {
    // AppSwitch: Switch
  },
  props: {
    sale: {
      type: Boolean,
      default: false
    },
    pricerange: {
      type: [Number, String],
      default: 300
    }
  },
  data() {
    return {
      min: 1,
      max: 4000,
      categories: [],
      categorySelected: {}
    };
  },
  methods: {
    updateHighprice(highprice) {
      this.$store.dispatch("products/GET_LIST_PRODUCT", {
        page: 0,
        highprice
      });
    }
    // ...mapActions(["updateHighprice", "setCategory"])
  },
  computed: {
    // ...mapGetters(["categories", "categorySelected"])
  }
};
</script>

<style lang="stylus" scoped>
aside {
  float: left;
  width: 19.1489%;
  padding: 1.5rem;
  position: sticky;
}

.sidearea {
  border-bottom: 1px solid #ccc;
  padding: 20px 0;

  &:first-of-type {
    padding-top: 0;
    padding-bottom: 40px;
  }

  &:last-of-type {
    border: none;
    padding-bottom: 0;
  }

  .subtitle {
    padding-bottom: 10px;
    margin-bottom: 0;
    display: block;
  }
}

span {
  font-family: 'Barlow', sans-serif;
}

.min, .max {
  font-size: 12px;
  color: #565656;
}
</style>
