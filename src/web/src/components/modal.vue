<script>
import { mapState } from "vuex";

export default {
  name: "modal",
  props: ["product"],

  data() {
    return {
      reviews: [
        {
          title: "review 1 title",
          username: "review 1 user name",
          content: "review 1 content",
          createDate: new Date()
        }
      ]
    };
  },

  methods: {
    close() {
      this.$emit("close");
    },
    onShowProduct(product) {
      debugger;
      this.product = product;
    }
  }
};
</script>
<template>
  <transition name="modal-fade">
    <div class="modal-backdrop">
      <div class="modal"
        role="dialog"
        aria-labelledby="modalTitle"
        aria-describedby="modalDescription"
      >
        <div class="modal-header">
            <button type="button" class="close" @click="close" aria-label="Close"><span aria-hidden="true">&times;</span>
            </button>
            <h1 class="modal-title">Reviews for {{product.name}}</h1>
            {{product123}}
        </div>
        <div class="modal-body">
            <div  style="padding: 10% 0;" v-if="!reviews" >
                <div  class="spinner spinner-lg"></div>
            </div>
            <h1 v-if="reviews && reviews.length <= 0">No reviews for <strong>{{product.name}}</strong>. <a href="#">Perhaps you'd like to leave one?</a></h1>
            <h1 v-if="reviews && reviews.length > 0">Top Customer Reviews</h1>
            <div v-if="reviews" style="height: 400px">
                <div v-for="review in reviews">
                    <div style="float:right;">
                        <img class="img-circle" src="https://www.gravatar.com/avatar/dummy?d=mm">
                    </div>
                    <div style="display:inline-block;" class="star-rating" star-rating rating-value="review.rating"
                        data-max="5" data-item-id="">
                    </div>
                    <div style="display:inline-block;"><h3>({{review.title}})</h3></div>
                    <p>By <a href="http://#">{{review.username}} on {{ review.createDate }}</a></p>
                    <p><span style="color: #c45500; font-size: 0.8em">Verified Purchase</span></p>
                    <h4>{{review.content}}</h4>
                    <a href="#">Comment</a>&nbsp;&nbsp;|&nbsp;&nbsp;12 people found this helpful. Was this review helpful to you?&nbsp;<button>Yes</button><button>No</button>&nbsp;<a href="#">Report Abuse</a>
                    <hr>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-primary" @click="close">Close</button>
        </div>

      </div>
    </div>
  </transition>
</template>

<style lang="stylus">
.modal-backdrop {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.3);
    display: flex;
    justify-content: center;
    align-items: center;
}

.modal {
    background: #ffffff;
    box-shadow: 2px 2px 20px 1px;
    overflow-x: auto;
    display: flex;
    flex-direction: column;
}

.modal-header, .modal-footer {
    padding: 15px;
    display: flex;
}

.modal-header {
    border-bottom: 1px solid #eeeeee;
    color: #4aae9b;
    justify-content: space-between;
}

.modal-footer {
    border-top: 1px solid #eeeeee;
    justify-content: flex-end;
}

.modal-body {
    position: relative;
    padding: 20px 10px;
}

.btn-close {
    border: none;
    font-size: 20px;
    padding: 20px;
    cursor: pointer;
    font-weight: bold;
    color: #4aae9b;
    background: transparent;
}

.btn-green {
    color: white;
    background: #4aae9b;
    border: 1px solid #4aae9b;
    border-radius: 2px;
}
</style>