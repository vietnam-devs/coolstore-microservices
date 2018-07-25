<template>
    <div>
        <div v-if="cart.shoppingCartItemList.length == 0" class="tile is-ancestor">
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
                            <tr v-for="(product, index) in items">
                                <th>
                                    <img class="media-object" v-if="product.name" v-bind:src="productImageUrl + index " v-bind:alt="product.name">
                                    
                                </th>
                                <th>
                                    <span>{{product.name}}</span>
                                </th>
                                <td>
                                    <span>${{product.price}}</span>
                                </td>
                                <td>
                                    <input class="quantity-button" type="button" value="-" @click="product.quantity--">
                                    <input class="quantity-field" type="number" readonly v-model="product.quantity">
                                    <input class="quantity-button" type="button" value="+" @click="product.quantity++">
                                </td>
                                <td>
                                    <span class="icon pointer">
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
                            <td>Cart Total: {{subtotal}}</td>
                        </tr>
                        <tr>
                            <td>Promotional Item Savings: {{cart.cartItemPromoSavings}}</td>
                        </tr>
                        <tr>
                            <td>Subtotal: {{cart.cartItemTotal}}</td>
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
                                <button v-if="isLoggedIn()" v-bind:disabled="cart.shoppingCartItemList.length <= 0"
                                    class="button is-success" data-toggle="modal"
                                    data-target="#checkoutModal" type="button">Checkout</button>
                            </td>
                        </tr>                        
                    </tbody>
                </table>                
            </div>
        </div>

        <div v-if="cart.shoppingCartItemList.length > 0" class="tile is-ancestor">
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
      cart: {
        shoppingCartItemList: []
      },
      items: [
        {
          name: 'IPhone 8',
          desc: 'IPhone 8',
          price: 900,
          availability: null,
          rating: null,
          id: 'ba16da71-c7dd-4eac-9ead-5c2c2244e69f',
          links: [
            {
              href:
                'http://localhost:8080/api/v1/products/ba16da71-c7dd-4eac-9ead-5c2c2244e69f',
              rel: 'self',
              method: 'GET'
            }
          ]
        },
        {
          name: 'IPhone X',
          desc: 'IPhone X',
          price: 1000,
          availability: null,
          rating: null,
          id: '13d02035-2286-4055-ad2d-6855a60efbbb',
          links: [
            {
              href:
                'http://localhost:8080/api/v1/products/13d02035-2286-4055-ad2d-6855a60efbbb',
              rel: 'self',
              method: 'GET'
            }
          ]
        },
        {
          name: 'MacBook Pro 2019',
          desc: 'MacBook Pro 2019',
          price: 4000,
          availability: null,
          rating: null,
          id: 'b8f0a771-339f-4602-a862-f7a51afd5b79',
          links: [
            {
              href:
                'http://localhost:8080/api/v1/products/b8f0a771-339f-4602-a862-f7a51afd5b79',
              rel: 'self',
              method: 'GET'
            }
          ]
        }
      ],
      subtotal: 0,
      productImageUrl: 'https://picsum.photos/120/75?image='
    }
  },
  beforeMount() {
    this.reset()
  },
  methods: {
    reset() {
    //   return getUser(user => {
    //     var authenid = user.sub
    //     this.$store.dispatch('RESET_CART', authenid).then(data => {
    //       if (data) {
    //         this.cart = data
    //         this.items = this.cart.shoppingCartItemList

    //         this.subtotal = 0
    //         this.cart.shoppingCartItemList.forEach(function(item) {
    //           this.subtotal += item.quantity * item.product.price
    //         })
    //       }
    //     })
    //   })
    },

    performAction(action, item) {
      this.$store
        .dispatch('REMOVE_FROM_CARD', item.product, item.quantity)
        .then(
          newCart => {
            cart
              .removeFromCart(item.product, item.quantity)
              .then(function(newCart) {
                this.reset()
              })
          },
          function(err) {
            Notifications.error('Error removing from cart: ' + err.statusText)
          }
        )
    },

    checkout() {
      this.$store.dispatch('CHECKOUT_CART').then(cartData => {}, function(err) {
        Notifications.error('Error checking out: ' + err.statusText)
      })
    },

    isLoggedIn() {
      return true
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