<template>
  <div>
    <div class="inside">
      <h1>&nbsp;</h1>
      <div class="tile is-ancestor columns is-multiline">
        <div v-for="(product, index) in products" class="tile column is-3">
          <article class="tile tile is-child border-box" @click="showReviews(product)">
            <img class="img-responsive" v-bind:src="productImageUrl + index" />
            <p class="name">{{product.name}}</p>
            <section class="price">
                <b-field>
                    <span>${{product.price}}</span>
                    <!-- <span class="tag-right"><star-rating v-bind:star-size="20" v-if="ratings[product.id]" v-model="ratings[product.id].cost" v-bind:show-rating="false"></star-rating></span> -->
                    <span class="tag-right"><star-rating v-bind:star-size="20" v-model="defaultStar" v-bind:show-rating="false"></star-rating></span>
                </b-field>
            </section>
            <!-- <section>
              <b-field>
                <b-input placeholder="Number" v-model="product.quantity" type="number" value="1" min="1" max="20" style="width: 4em">
                </b-input>
                <button @click="addToCart(product.id, product.quantity)" class="button is-primary">Add To Cart</button>
                <div class="tag-right">
                    <span class="tag is-dark ">
                        <template v-if="product.availability">{{product.availability.quantity}}</template>
                        <template v-if="!product.availability">0</template>
                    </span>
                    <span class="icon has-text-info">
                    <i class="fas fa-info-circle"></i>
                    </span>
                </div>
              </b-field>
            </section> -->
          </article>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import StarRating from 'vue-star-rating'
import Review from '../components/Review.vue'
import { setItem } from '../helper/storage'
import { mapState } from 'vuex'

export default {
  name: 'home',
  components: {
    StarRating,
    Review
  },
  data() {
    return {
      isModalVisible: false,
      productReview: {},
      productImageUrl: 'https://picsum.photos/400/300?image=',
      defaultStar: 0
    }
  },

  computed: {
    // ...mapState('user', ['addresses', 'creditCards']),
    // ...mapState('vendor', ['products', 'ratings']),
    products() {
      return this.$store.getters['products/products']
    },
    ratings() {
      return this.$store.getters['ratings/ratingSet']
    }
  },

  beforeMount() {
    this.loadItems(this.page)
  },

  asyncData({ store }) {
    return store.dispatch('products/GET_LIST_PRODUCT', { page: 0 })
  },

  methods: {
    loadItems(page) {
      this.$store.dispatch('products/GET_LIST_PRODUCT', { page: 0 })
    },

    formatPrice(value) {
      let val = (value / 1).toFixed(2).replace('.', ',')
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
    },

    // rateFunction(itemId, rating) {
    //   this.$store.dispatch('SET_RATING_ITEM', { itemId, rating })
    // },

    showReviews(product) {
      this.productReview = product
      this.$router.push('/review/' + product.id)
    },
    
  }
}
</script>
<style lang="stylus">
.tag-right {
  margin-left: auto;
}

.img-circle {
  border-radius: 50%;
}

.img-responsive {
  width: 100%;
}

.star-width {
  width: 20%;
}

.border-box {
  padding: 5px;
  border: 1px solid #BBB;
}

.name {
  margin-top: 10px;
  height: 60px;
}

.price {
  padding-bottom: 10px;
}

.inside {
  margin-left: 20px;
  margin-right: 20px;
}
</style>