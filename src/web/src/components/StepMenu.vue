<template>
  <div class="steps is-large">
      <div class="step-item" v-for="(item, index) in menu" :key="item.name" :data-attr="index" :class="[actualStep &gt; index &amp;&amp; 'is-completed', actualStep == index &amp;&amp; 'is-active']">
          <div class="step-marker"><span class="icon"><i class="fa" :class="item.icon"></i></span></div>
          <div class="step-details" :class="[actualStep &lt; index &amp;&amp; 'has-text-grey-light']">
              <p class="step-title">{{item.title}}</p>
          </div>
      </div>
  </div>
</template>
<script>
export default {
  name: "StepMenu",
  props: {
    actualStep: {
      type: Number
    },
    menu: {
      type: Array,
      required: true
    }
  }
};
</script>

<style lang="stylus">
@require '../../node_modules/bulma-steps/dist/css/bulma-steps.min.css';

$colorGreen = #00d1b2;

.steps {
  &.is-large {
    font-size: inherit;
    margin-bottom: 4rem;

    .step-item {
      .step-marker .icon *, .step-details .step-title {
        font-size: 1.2rem;
      }
    }
  }

  .is-active::before, .is-completed::before {
    background-position: left bottom;
  }

  .is-active .step-marker {
    background-color: white;
    border-color: $colorGreen;
    color: $colorGreen;
  }

  .is-completed .step-marker {
    color: white;
    background-color: $colorGreen;
  }
}
</style>
