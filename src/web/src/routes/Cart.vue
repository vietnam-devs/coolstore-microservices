<template>
    <div>
        <div v-if="cart.items.length > 0" class="tile is-ancestor">
            <div class="is-8 tile">
                <div class="tile is-parent cart-list">
                    <table class="table is-fullwidth">
                        <thead>
                            <tr>
                                <th class="item-image"><abbr title="Product">PRODUCT</abbr></th>
                                <th><abbr title="action"></abbr></th>
                                <th><abbr title="Price">PRICE</abbr></th>
                                <th class="item-quantity"><abbr title="Quantity">QUANTITY</abbr></th>
                                <th class="item-action"><abbr title="action"></abbr></th>
                            </tr>
                        </thead>                        
                        <tbody>
                            <tr v-if="items" v-for="(product, index) in items">
                                <th>
                                    <img class="media-object" v-if="product.productName" v-bind:src="productImageUrl + index " v-bind:alt="product.name">
                                </th>
                                <th>
                                    <span>{{product.productName}}</span>
                                </th>
                                <td>
                                    <span>${{product.price}}</span>
                                </td>
                                <td>
                                    <input class="quantity-button" type="button" value="-" @click="product.quantity--; updateProduct(product.productId, product.quantity)">
                                    <input class="quantity-field" type="number" readonly v-model="product.quantity">
                                    <input class="quantity-button" type="button" value="+" @click="product.quantity++; updateProduct(product.productId, product.quantity)">
                                </td>
                                <td>
                                    <span @click="removeProduct(product, product.quantity)" class="icon pointer">
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
                            {{cart}}
                            <th>Shopping Summary</th>
                        </tr>  
                        <tr>
                            <td>Cart Total: {{cart.cartTotal}}</td>
                        </tr>
                        <tr>
                            <td>Promotional Item Savings: {{cart.cartItemPromoSavings}}</td>
                        </tr>
                        <tr>
                            <td>Subtotal: {{subtotal}}</td>
                        </tr>  
                        <tr>
                            <td>Shipping: {{cart.shippingTotal}}</td>
                        </tr> 
                        <tr>
                            <td>Promotional Shipping Savings: {{cart.shippingPromoSavings}}</td>
                        </tr> 
                        <tr>
                            <td>Total Order Amount: {{cart.cartTotal}}</td>
                        </tr> 
                        <tr>
                            <td>
                                <button v-bind:disabled="cart.items.length <= 0"
                                    class="button is-success" data-toggle="modal"
                                    data-target="#checkoutModal" type="button">Checkout</button>
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
                        <router-link to="/" class="btn btn-default btn-lg">
                            Return to Store
                        </router-link>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import { getUser } from '../auth/usermanager'
export default {
  name: 'cart',
  data() {
    return {
      cart: this.$store.state.cart,
      items: this.$store.state.cart.items,
      subtotal: 0,
      productImageUrl: 'https://picsum.photos/120/75?image='
    }
  },
  beforeMount() {
    this.getCart()
  },
  methods: {
    getCart() {
      this.$store.dispatch('GET_CART').then(data => {
        if (data) {
          this.cart = data
          this.items = this.cart.items
        }
      })
    },

    removeProduct(product, quantity) {
      this.$store.dispatch('REMOVE_FROM_CARD', product, product.quantity).then(
        newCart => {
          this.$store.commit('SET_CART', newCart.data)
        },
        function(err) {}
      )
    },

    updateProduct(productId, quantity) {
      this.$store
        .dispatch('UPDATE_PRODUCT_QUANTITY', { productId, quantity })
        .then(
          newCart => {
            this.$store.commit('SET_CART', newCart.data)
          },
          function(err) {}
        )
    },

    checkout() {
      this.$store
        .dispatch('CHECKOUT_CART')
        .then(cartData => {}, function(err) {})
    }
  }
}
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