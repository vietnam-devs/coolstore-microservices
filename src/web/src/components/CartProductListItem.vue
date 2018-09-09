<template>
    <div class="box">
        <article class="media">
            <div class="media-left">
                <picture class="image is-64x64">
                    <source :srcset="`products/${item.img}.webp`" type="image/webp" /><img :src="`products/${item.img}.png`" :alt="`Image of ${item.name}`" /></picture>
            </div>
            <div class="media-content">
                <div class="content">
                    <p><strong>{{ item.name }}</strong><br /><span class="itemCount">{{ item.count }}</span> x {{ item.price | usdollar }} = ${{ item.count * item.price }}</p>
                </div>
                <nav class="level is-mobile">
                    <div class="level-left"><a class="level-item removeItem" @click="removeItem(item)" title="Remove"><span class="icon is-small"><i class="fa fa-trash-alt"></i></span></a><a class="level-item"><span class="icon is-small"><i class="fa fa-retweet"></i></span></a><a class="level-item"><span class="icon is-small"><i class="fa fa-heart"></i></span></a></div>
                </nav>
            </div>
        </article>
    </div>
</template>
<script>
import { createNamespacedHelpers } from "vuex";
const { mapActions } = createNamespacedHelpers("cart");
export default {
  name: "CartProductListItem",
  filters: {
    usdollar: function(value) {
      return `$${value}`;
    }
  },
  props: {
    item: {
      type: Object,
      required: true
    }
  },
  methods: {
    ...mapActions(["removeItem"])
  }
};
</script>