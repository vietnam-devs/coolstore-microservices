<template>
    <div class="content">
        <transition name="fade">
            <form class="payment" v-if="status !== 'failure'" @submit.prevent="beforePay">
                <h3>Please enter your payment details:</h3>
                <div class="field"><label class="label" for="email">Email</label>
                    <div class="control has-icons-left has-icons-right"><input class="input" id="email" type="email" required="required" v-model="userEmail" placeholder="name@example.com" name="email" v-validate="'required|email'" :class="{ 'is-danger': errors.has('email') }" /><span class="icon is-small is-left"><i class="fa fa-envelope"></i></span>
                        <span
                            class="icon is-small is-right" v-if="errors.has('email')"><i class="fa fa-exclamation-triangle"></i></span>
                            <p class="help is-danger" v-if="errors.has('email')">{{ errors.first('email') }}</p>
                    </div>
                </div>
                <div class="field"><label class="label" for="card">Credit Card</label>
                    <p class="help">Test using this credit card:&nbsp;<strong>4242 4242 4242 4242,<br></strong>and enter any 5 digits for the zip code</p>
                </div>
                <div class="field">
                    <card class="stripe-card input" id="card" :class="{ 'complete': isStripeCardCompleted }" :stripe="stripePublishableKey" @change="setIsStripeCardCompleted($event.complete)"></card>
                </div>
                <div class="field"><button class="button is-success pay-with-stripe" :disabled="!isStripeCardCompleted || errors.any()" :class="{ 'is-loading': isLoading }">Pay with credit card</button></div>
            </form>
            <div class="statusFailure has-text-centered" v-if="status === 'failure'">
                <h3>Oh No!</h3>
                <p>Something went wrong!</p><button class="button" @click="clearCheckout">Please try again</button></div>
        </transition>
    </div>
</template>
<script>
import { Card } from "vue-stripe-elements-plus";
import { createNamespacedHelpers } from "vuex";
const { mapActions, mapGetters } = createNamespacedHelpers("checkout");
const STRIPE_URL = process.env.STRIPE_URL;
export default {
  name: "Checkout",
  components: {
    Card
  },
  computed: {
    ...mapGetters(["isStripeCardCompleted", "status", "isLoading"])
  },
  props: {
    total: {
      type: [Number, String],
      required: true
    },
    stripeUrl: {
      type: String,
      default: STRIPE_URL
    }
  },
  data() {
    return {
      userEmail: undefined,
      stripePublishableKey: process.env.STRIPE_PUBLISHABLE_KEY
    };
  },
  methods: {
    ...mapActions([
      "clearCheckout",
      "pay",
      "setIsStripeCardCompleted",
      "setStatus"
    ]),
    async beforePay() {
      const isAllFieldsValid = await this.$validator.validateAll();
      if (!isAllFieldsValid) {
        this.setStatus("failure");
        return;
      }
      await this.pay({
        userEmail: this.userEmail,
        total: this.total,
        url: this.stripeUrl
      });
    }
  }
};
</script>

<style scoped lang="stylus">
.payment {
  border: 1px solid #ccc;
  max-width: 500px;
  padding: 50px;
  display: flex;
  flex-direction: column;
  margin: 0 auto;
}

.stripe-card {
  margin-bottom: 10px;

  &.input {
    display: block;
  }
}

/* -- transition -- */
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.25s ease-out;
}

.fade-enter, .fade-leave-to {
  opacity: 0;
}
</style>