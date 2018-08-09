<template>
  <div>
    <div class="inside">
      <h1>&nbsp;</h1>
      <div class="tile is-ancestor columns is-multiline">
        <div v-for="(product, index) in products" class="tile column is-3">
          <article class="tile tile is-child border-box">
            <img @click="showReviews(product)" class="img-responsive" v-bind:src="product.imageUrl" />
            <p @click="showReviews(product)" class="name">{{product.name}}</p>
            <section class="price">
                <b-field>
                    <span>${{product.price}}</span>
                    <span class="tag-right"><star-rating @rating-selected ="setRating(product.id, $event)" v-bind:star-size="20" v-if="ratings[product.id]" v-model="ratings[product.id].cost" v-bind:show-rating="false"></star-rating></span>
                </b-field>
            </section>
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
    }
  },

  computed: {
    // ...mapState('user', ['addresses', 'creditCards']),
    // ...mapState('vendor', ['products', 'ratings']),
    products() {
      return this.$store.getters['products/products']
    },
    ratings() {
      let ratingSet = this.$store.getters["ratings/ratingSet"]
      let productSet = this.$store.getters["products/products"]
      return productSet.reduce((obj, item) => {
        ratingSet[item.id] = ratingSet[item.id] || {}
        obj[item.id] = ratingSet[item.id]
        return obj
      }, {})
    },
    userInfo() {
      return this.$store.getters["account/userInfo"] || {}
    }
  },

  beforeMount() {
    this.loadItems(this.page)
  },

  asyncData({ store }) {
    return [
      store.dispatch("products/GET_LIST_PRODUCT", { page: 0 }),
      store.dispatch("ratings/GET_LIST_RATING")
    ]
  },

  methods: {
    loadItems(page) {
      Promise.all([
        this.$store.dispatch("products/GET_LIST_PRODUCT", { page: 0 }),
        this.$store.dispatch("ratings/GET_LIST_RATING")
      ])
    },

    formatPrice(value) {
      let val = (value / 1).toFixed(2).replace('.', ',')
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
    },

    setRating(productId, rating) {
      this.$store.dispatch("ratings/SET_RATING_FOR_PRODUCT", {
        productId,
        userId: this.userInfo.sub,
        cost: rating
      })
    },

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