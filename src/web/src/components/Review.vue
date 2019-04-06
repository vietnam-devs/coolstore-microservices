<template>
  <div class="container has-text-centered">
    <div class="columns is-vcentered">
      <div class="column is-5">
        <picture class="image is-square">
          <source v-bind:srcset="product.imageUrl" type="image/webp">
          <img class="lazyload" v-bind:srcset="product.imageUrl" :alt="`Image of ${product.name}`">
        </picture>
      </div>
      <div class="column is-6 is-offset-1">
        <h1 class="title is-2">{{ product.name }}</h1>
        <h2 class="subtitle is-4">{{product.desc}}</h2>
        <p class="is-size-6">${{ product.price }}</p>
        <br>
        <section>
          <b-field class="div-center">
            <b-input
              placeholder="Number"
              v-model="quantity"
              type="number"
              value="1"
              min="1"
              max="20"
              style="width: 4em"
            ></b-input>
            <button @click="addToCart(product.id, quantity)" class="button is-primary">Add To Cart</button>
            <div>
              <span class="tag is-dark">
                <template v-if="product.availability">{{product.availability.quantity}}</template>
                <template v-if="!product.availability">0</template>
              </span>
              <span class="icon has-text-info">
                <i class="fas fa-info-circle"></i>
              </span>
            </div>
          </b-field>
        </section>
        <!-- <p class="has-text-centered"><a class="button is-medium is-info is-outlined" @click="addToCart(product)" aria-label="Add to cart">Add to cart</a></p> -->
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex"

export default {
  name: "review",
  data() {
    return {
      quantity: 1
    };
  },
  computed: {
    product() {
      return { ...this.$store.state.products.product } || {}
    },
    cartId() {
      return this.$store.getters["cart/cartId"] || null
    }
  },
  beforeMount() {
    this.loadProduct(this.$route.params.id);
  },
  methods: {
    loadProduct(id) {
      this.$store.dispatch("products/GET_PRODUCT_BY_ID", { productId: id })
    },
    addToCart(productId, quantity) {
      if (!this.cartId) {
        this.$store.dispatch("cart/ADD_TO_CARD", { productId, quantity })
      } else
        this.$store.dispatch("cart/UPDATE_CARD", {
          cartId: this.cartId,
          productId,
          quantity
        })
    }
  }
}
</script>

<style lang="stylus">
.field.has-addons.div-center {
  justify-content: center;
}
</style>
