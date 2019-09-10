<template>
  <div class="steps is-large">
    <div
      class="step-item"
      v-for="(item, index) in menu"
      :key="item.name"
      :data-attr="index"
      :class="[actualStep &gt; index &amp;&amp; 'is-completed', actualStep == index &amp;&amp; 'is-active']"
    >
      <div class="step-marker">
        <span class="icon">
          <i class="fa" :class="item.icon"></i>
        </span>
      </div>
      <div class="step-details" :class="[actualStep &lt; index &amp;&amp; 'has-text-grey-light']">
        <p class="step-title">{{item.title}}</p>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AppSwitch",
  computed: {
    trigger: {
      get() {
        return this.$store.state.product.sale;
      },
      set() {
        return this.$store.dispatch("product/switchSale");
      }
    }
  }
};
</script>

<style lang="stylus" scoped>
</style>
