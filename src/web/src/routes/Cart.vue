<template>
  <div>
    <div v-if="cart.items.length > 0" class="tile is-ancestor">
      <div class="is-8 tile">
        <div class="tile is-parent cart-list">
          <table class="table is-fullwidth">
            <thead>
              <tr>
                <th class="item-image">
                  <abbr title="Product">PRODUCT</abbr>
                </th>
                <th>
                  <abbr title="action"></abbr>
                </th>
                <th>
                  <abbr title="Price">PRICE</abbr>
                </th>
                <th class="item-quantity">
                  <abbr title="Quantity">QUANTITY</abbr>
                </th>
                <th class="item-action">
                  <abbr title="action"></abbr>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="items" v-for="(product, id, index) in items">
                <th>
                  <img
                    class="media-object"
                    v-if="product.productName"
                    v-bind:src="productImageUrl + index "
                    v-bind:alt="product.name"
                  >
                </th>
                <th>
                  <span>{{product.productName}}</span>
                </th>
                <td>
                  <span>${{product.price}}</span>
                </td>
                <td>
                  <input
                    v-bind:disabled="cart.isCheckout"
                    class="quantity-button"
                    type="button"
                    value="-"
                    @click="increeQuantityProduct(product)"
                  >
                  <input class="quantity-field" type="text" readonly v-model="product.quantity">
                  <input
                    v-bind:disabled="cart.isCheckout"
                    class="quantity-button"
                    type="button"
                    value="+"
                    @click="product.quantity++; updateProduct(product.productId, product.quantity)"
                  >
                </td>
                <td>
                  <span
                    v-bind:disabled="cart.isCheckout"
                    @click="removeProduct(product.productId)"
                    class="icon pointer"
                  >
                    <i class="fas fa-backspace"></i>
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div class="tile is-4 is-parent">
        <table class="table is-fullwidth">
          <tbody>
            <tr>
              <th>Shopping Summary</th>
            </tr>
            <tr>
              <td>Cart Total: {{cart.cartTotal | toCurrency}}</td>
            </tr>
            <tr>
              <td>Promotional Item Savings: {{cart.cartItemPromoSavings | toCurrency}}</td>
            </tr>
            <tr>
              <td>Subtotal: {{subtotal | toCurrency}}</td>
            </tr>
            <tr>
              <td>Shipping: {{cart.shippingTotal | toCurrency}}</td>
            </tr>
            <tr>
              <td>Promotional Shipping Savings: {{cart.shippingPromoSavings | toCurrency}}</td>
            </tr>
            <tr>
              <td>Total Order Amount: {{cart.cartTotal | toCurrency}}</td>
            </tr>
            <tr>
              <td>
                <button
                  v-bind:disabled="cart.isCheckout"
                  class="button is-success"
                  @click="checkout"
                  type="button"
                >Checkout</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="cart.items.length == 0" class="tile is-ancestor">
      <div class="tile">
        <div class="tile is-parent">
          <div class="div-center has-text-centered">
            <h1>You have not added any items to your Shopping Cart!</h1>
            <router-link to="/" class="btn btn-default btn-lg">Return to Store</router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { getUser } from "../auth/usermanager";
export default {
  name: "cart",
  data() {
    return {
      productImageUrl: "https://picsum.photos/120/75?image="
    };
  },
  computed: {
    cart() {
      let cart = this.$store.getters["cart/cartReducer"] || {};
      cart = cart || {};
      cart.items = cart.items || [];
      return cart;
    },
    items() {
      let cartReducer = this.$store.getters["cart/cartReducer"] || {};
      cartReducer.itemsFlat = cartReducer.itemsFlat || [];
      return cartReducer.itemsFlat;
    },
    cartId() {
      return this.$store.getters["cart/cartId"];
    },
    subtotal() {
      var subtotal = 0;
      if (this.$store.state.cart.items) {
        this.$store.state.cart.items.forEach(item => {
          subtotal += item.price * item.quantity;
        });
      }
      return subtotal;
    }
  },
  beforeMount() {
    this.getCart();
  },
  methods: {
    getCart() {
      if (!this.cartId) return;
      this.$store.dispatch("cart/GET_CART", { cartId: this.cartId });
    },

    removeProduct(productId) {
      if (this.cart.isCheckout) return;
      if (!this.cartId) return;
      let cartId = this.cartId;
      this.$store.dispatch("cart/REMOVE_FROM_CARD", { cartId, productId });
    },

    updateProduct(productId, quantity) {
      if (!this.cartId) return;
      let cartId = this.cartId;
      this.$store.dispatch("cart/UPDATE_PRODUCT_QUANTITY", {
        cartId,
        productId,
        quantity
      });
    },

    checkout() {
      if (!this.cartId) return;
      let cartId = this.cartId;
      this.$store.dispatch("cart/CHECKOUT_CART", { cartId });
    },

    increeQuantityProduct(product) {
      if (product.quantity > 1) {
        product.quantity--;
        this.updateProduct(product.productId, product.quantity);
      }
    }
  }
};
</script>
<style lang="stylus">
.div-center {
  margin: 0 auto;
}

.pointer {
  cursor: pointer;
}

.cart-list .quantity-button {
  color: #555;
  width: 35px;
  height: 30px;
  padding: 3px;
  border: none;
  outline: none;
  cursor: pointer;
  font-size: 14px;
  background: #eee;
  text-align: center;
  font-weight: normal;
  white-space: nowrap;
  display: inline-block;
  background-image: none;
}

.cart-list .quantity-field {
  width: 35px;
  height: 30px;
  outline: none;
  margin: 0 -4px;
  font-size: 14px;
  text-align: center;
  border: 1px solid #eee;
}

.item-image {
  width: 150px;
}

.item-quantity {
  width: 125px;
}

.item-action {
  width: 50px;
}
</style>
