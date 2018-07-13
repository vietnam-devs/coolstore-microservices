<template>
    <div>
        <div class="row example-container">
            <div class="col-md-8 list-view-container">

                <div v-if="cart.shoppingCartItemList.length > 0">
                    <!-- <div pf-list-view items="items" config="config"
                        action-buttons="actionButtons">

                        <div class="list-view-pf-description">

                                <div class="media">
                                <div class="media-left">
                                    <img	class="media-object" src="/app/imgs/{{item.product.name}}.jpg"
                                            alt="{{item.product.name}}">
                                </div>
                                <div class="media-body">
                                
                                    <h2 class="media-heading">{{item.product.name}}</h2>
                                    <div class="p-t-8">
                                        <span class="label label-default">Quantity: {{item.quantity}}</span>
                                    </div>
                                    <div class="p-t-8">
                                        {{item.product.desc}}
                                    </div>
                                </div>
                                </div>
                        </div>
                    </div>-->
                </div>
                <div v-if="cart.shoppingCartItemList.length == 0">
                    <h1>You have not added any items to your Shopping Cart!</h1>
                    <router-link to="/" class="btn btn-default btn-lg">
                        Return to Store
                    </router-link>
                </div>
            </div>

            <div class="col-md-4 panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Shopping Summary</h3>
                </div>
                <div class="panel-body">

                    <h3>Cart Total: {{subtotal}}</h3>
                    <h3>Promotional Item Savings: {{cart.cartItemPromoSavings}}</h3>
                    <h3>Subtotal: {{cart.cartItemTotal}}</h3>
                    <h3>Shipping: {{cart.shippingTotal}}</h3>
                    <h3>Promotional Shipping Savings: {{cart.shippingPromoSavings}}</h3>
                    <h2>
                        Total Order Amount: <strong>{{cart.cartTotal}}</strong>
                    </h2>
                </div>
                <div class="panel-footer">
                    <button v-if="isLoggedIn()" v-bind:disabled="cart.shoppingCartItemList.length <= 0"
                        class="btn btn-primary btn-lg" data-toggle="modal"
                        data-target="#checkoutModal" type="button">Checkout</button>

                    <button v-if="!isLoggedIn()" @click="login"
                            class="btn btn-primary btn-lg" type="button">Sign in to purchase</button>

                    <button title="SSO has not been configured" class="btn btn-primary btn-lg" type="button">Sign in unavailable</button>

                    <a href="#/" class="btn btn-default btn-lg">Keep Shopping</a>
                </div>
            </div>
        </div>


        <div class="modal fade" id="checkoutModal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"
                            aria-hidden="true">
                            <span class="pficon pficon-close"></span>
                        </button>
                        <h4 class="modal-title" id="myModalLabel">Final Order Summary</h4>
                    </div>
                    <div class="modal-body">
                        <h1>Thank you for your order!</h1>
                        <p>
                            Your order total of <strong>{{cart.cartTotal}}</strong>
                            will be processed when you click Checkout.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <a @click="checkout" data-dismiss="modal"
                            class="btn btn-primary btn-lg">Checkout</a>
                        <a data-dismiss="modal"
                            class="btn btn-default btn-lg">Cancel</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name: "cart",
    data () {
        return {
            cart: {
                shoppingCartItemList: [],
            },
            items: [],
            subtotal: 0,
        };
    },
    methods: {
        reset(){
            this.$store.dispatch('RESET_CART').then((data)=>{
                this.cart = data;
                this.items = this.cart.shoppingCartItemList;

                this.subtotal = 0;
                this.cart.shoppingCartItemList.forEach(function (item) {
                    this.subtotal += (item.quantity * item.product.price);
                });
            })
        },

        performAction(action, item) {
            this.$store.dispatch('REMOVE_FROM_CARD', item.product, item.quantity).then((newCart)=>{
                cart.removeFromCart(item.product, item.quantity).then(function (newCart) {
                    this.reset();
                });
            }, function (err) {
                    Notifications.error("Error removing from cart: " + err.statusText);
            })
        },

        checkout(){
            this.$store.dispatch('CHECKOUT_CART').then((cartData)=>{
            }, function (err) {
                Notifications.error("Error checking out: " + err.statusText);
            })
        },

        isLoggedIn() {
            return true;
        }
    }
};
</script>