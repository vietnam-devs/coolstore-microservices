<template>
  <div class="card is-radius">
    <div class="card-image">
      <router-link exact :to="{ name: 'reviewproduct', params: {id: item.id } }">
        <picture class="image">
          <source :data-srcset="item.imageUrl">
          <img class="lazyloaded" :src="item.imageUrl" :alt="`Image of ${item.name}`">
        </picture>
      </router-link>
    </div>
    <div class="card-content">
      <div class="media">
        <div class="media-content">
          <router-link exact :to="{ name: 'reviewproduct', params: {id: item.id } }">
            <p class="title is-5">{{ item.name }}</p>
            <p class="item-price">{{ item.price | usdollar }}</p>
          </router-link>
        </div>
        <div class="media-right">
          <p class="field">
            <button
              class="button icon is-large add"
              @click="addToCart(item.id, 1)"
              aria-label="Add to cart"
            >
              <span class="fa-stack">
                <i class="fa fa-circle fa-stack-2x"></i>
                <i class="fa fa-cart-plus fa-stack-1x fa-inverse"></i>
              </span>
            </button>
          </p>
        </div>
      </div>
      <div>
        <star-rating
          @rating-selected="setRating(item.id, $event)"
          v-bind:star-size="20"
          v-if="ratings && ratings[item.id]"
          v-model="ratings[item.id].cost"
          v-bind:show-rating="false"
        ></star-rating>
      </div>
    </div>
  </div>
</template>
<script>
import { createNamespacedHelpers } from "vuex";
import StarRating from "vue-star-rating";
const { mapActions } = createNamespacedHelpers("cart");
export default {
  name: "Card",
  components: {
    StarRating
  },
  filters: {
    usdollar: value => `$${value}`
  },
  props: {
    item: {
      type: Object,
      required: true
    },
    ratings: {
      type: Object,
      required: true
    }
  },
  computed: {
    cartId() {
      return this.$store.getters["cart/cartId"] || null;
    },
    userInfo() {
      return this.$store.getters["account/userInfo"] || {};
    }
  },
  methods: {
    addToCart(productId, quantity) {
      if (!this.cartId) {
        this.$store.dispatch("cart/ADD_TO_CARD", { productId, quantity });
      } else
        this.$store.dispatch("cart/UPDATE_CARD", {
          cartId: this.cartId,
          productId,
          quantity
        });
    },
    setRating(productId, rating) {
      this.$store.dispatch("ratings/SET_RATING_FOR_PRODUCT", {
        productId,
        userId: this.userInfo.sub,
        cost: rating
      });
    }
  }
};
</script>

<style scoped lang="stylus">
.card {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  .image {
    img {
      padding-top: 1.5rem;
    }
  }

  .card-content {
    width: 100%;
  }

  .title, .subtitle {
    color: inherit;
  }

  .title {
    margin-bottom: 0.5rem;
  }

  .button {
    border: 0;
    padding: 0;

    .fa-circle {
      transition: color 0.5s;
    }

    .fa-cart-plus {
      font-size: 1.4rem;
    }

    &:hover {
      .fa-circle {
        color: #209cee;
      }
    }

    &.icon {
      cursor: pointer;
    }
  }

  a {
    color: inherit;

    &:hover {
      color: #3273dc;
    }
  }
}

.lazyload, .lazyloading {
  opacity: 0;
}

.lazyloaded {
  opacity: 1;
  transition: opacity 150ms;
}
</style>
