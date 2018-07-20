<template>
  <div>
    <div class="container">
      <h1>&nbsp;</h1>
      <div class="tile is-ancestor columns is-multiline">
        <div v-for="(product, index) in products" class="tile column is-4">
          <article class="tile tile is-child box">
            <p class="title" @click="showReviews(product)">{{product.name}}</p>
            <p class="subtitle">{{product.desc}}</p>
            <img class="img-responsive img-circle" v-bind:src="productImageUrl + index" />            
            <section>
                <b-field>
                    <span>${{product.price}}</span>
                    <span class="tag-right"><star-rating v-bind:star-size="20" v-if="product.rating" v-model="product.rating.rate" v-bind:show-rating="false"></star-rating></span>
                </b-field>
            </section>
            <section>
              <b-field>
                <b-input placeholder="Number" v-model="product.quantity" type="number" value="1" min="1" max="20" style="width: 4em">
                </b-input>
                <button @click="addToCart(product, product.quantity)" class="button is-primary">Add To Cart</button>
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
            </section>
          </article>
        </div>
      </div>
    </div>
    <!-- <Review v-bind:product="productReview" v-show="isModalVisible" @close="closeModal"/> -->
  </div>
</template>

<script>
import StarRating from "vue-star-rating";
import Review from "../components/Review.vue";
import { watchList } from "../api";
export default {
  name: "home",
  components: {
    StarRating,
    Review
  },
  data() {
    return {
      isModalVisible: false,
      productReview: {},
      productImageUrl: "https://picsum.photos/400/300?image=",
    };
  },

  computed: {
    products() {
      return this.$store.state.products;
    }
  },

  beforeMount() {
    this.loadItems(this.page);
  },

  methods: {
    loadItems(page) {
      this.$store.dispatch("FETCH_LIST_DATA", { page });
    },

    formatPrice(value) {
      let val = (value / 1).toFixed(2).replace(".", ",");
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    },

    rateFunction(itemId, rating) {
      this.$store.dispatch("SET_RATING_ITEM", { itemId, rating });
    },

    showReviews(product) {
      this.productReview = product;
      this.$router.push("/review/"+product.id)
      this.showModal();
    },

    showModal() {
      this.isModalVisible = true;
    },

    closeModal() {
      this.isModalVisible = false;
    },

    addToCart(product, quantity) {
      this.$store.dispatch("ADD_TO_CARD", { product, quantity }).then(
        data => {
          // this.$notify({
          //     group: 'noti',
          //     title: 'Success!',
          //     text: 'Hello user! This is a notification!',
          //     type: 'success'
          // });
        },
        error => {
          // this.$notify({
          //     group: 'noti',
          //     title: 'Error',
          //     text: error,
          //     type: 'error'
          // });
        }
      );
    }
  }
};
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
</style>