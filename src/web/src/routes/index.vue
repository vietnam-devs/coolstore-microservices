<template>
  <div class="container">
      <div class="section">
          <app-hero></app-hero>
      </div>
      <div class="section capsule is-clearfix">
          <!-- <app-sidebar :pricerange.sync="highprice"></app-sidebar> -->
          <transition-group class="content is-pulled-right" name="items" tag="div">
              <app-product-list-item v-for="product in products" :key="product['_id']" :item="product"></app-product-list-item>
          </transition-group>
      </div>
  </div>
</template>
<script>
import { createNamespacedHelpers } from "vuex";
import Hero from "../components/Hero.vue";
import ProductListItem from "../components/ProductListItem.vue";
import Sidebar from "../components/Sidebar.vue";
const { mapGetters } = createNamespacedHelpers("product");
export default {
  components: {
    AppHero: Hero,
    AppProductListItem: ProductListItem,
    AppSidebar: Sidebar
  },
  data() {
    return {
      highprice: 200,
    }
  },
  computed: {
    products() {
      return this.$store.getters['products/products']
    },
    // ...mapGetters(["products", "highprice"])
  },
  beforeMount() {
    this.loadItems(this.page);
  },
  asyncData({ store }) {
    return [
      store.dispatch("products/GET_LIST_PRODUCT", { page: 0 }),
      store.dispatch("ratings/GET_LIST_RATING")
    ];
  },
  methods: {
    loadItems(page) {
      Promise.all([
        this.$store.dispatch("products/GET_LIST_PRODUCT", { page: 0 }),
        this.$store.dispatch("ratings/GET_LIST_RATING")
      ]);
    },

    formatPrice(value) {
      let val = (value / 1).toFixed(2).replace(".", ",");
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    },

    setRating(productId, rating) {
      this.$store.dispatch("ratings/SET_RATING_FOR_PRODUCT", {
        productId,
        userId: this.userInfo.sub,
        cost: rating
      });
    },

    showReviews(product) {
      this.productReview = product;
      this.$router.push("/review/" + product.id);
    }
  }
};
</script>

<style lang="stylus" scoped>
.content {
  /* no grid support */
  width: 79.7872%;
  /* grid */
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-gap: 1rem;
  padding: 0;
}
</style>
