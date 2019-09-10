<template>
  <section class="form">
    <form novalidate name="newproduct">
      <b-field label="Product Name">
        <b-input v-model="name" placeholder="Product Name" required></b-input>
      </b-field>

      <div class="field">
        <label class="label">Image</label>
        <div class="control is-clearfix">
          <img v-bind:src="imageShow" name="image">
        </div>
        <button class="button is-primary" @click="randomNewImage">Change image</button>
      </div>

      <b-field label="Description">
        <b-input v-model="desc" maxlength="200" type="textarea" placeholder="Description"></b-input>
      </b-field>

      <b-field label="Price">
        <div class="field has-addons">
          <input type="number" value="1" min="1" v-model="price" required class="input is-info">
        </div>
      </b-field>
      <span>
        <button class="button is-primary" @click="save" type="submit">Save</button>
      </span>
    </form>
  </section>
</template>

<script>
export default {
  name: "newcatalog",
  data() {
    return {
      name: "",
      desc: "",
      price: 1,
      imageUrl: "https://picsum.photos/1200/900?image=",
      imageNumberDefault: 0
    };
  },
  beforeMount() {
    this.randomNewImage();
  },
  computed: {
    imageShow() {
      return this.imageUrl + this.imageNumberDefault;
    },
    imageNumber: {
      get() {
        return Math.floor(Math.random() * 500);
      },
      set(newValue) {
        this.imageNumberDefault = newValue;
      }
    }
  },
  methods: {
    getRandomNumber() {
      return Math.floor(Math.random() * 500);
    },
    goBack() {
      this.$router.go(-1);
    },
    checkForm: function(e) {
      if (this.name && this.price) {
        return true;
      }
    },
    randomNewImage(e) {
      this.imageNumber = Math.floor(Math.random() * 500);
      if (!e) return;
      e.preventDefault();
      return;
    },
    save(e) {
      e.preventDefault();
      if (!this.checkForm()) {
        return;
      }
      var model = {
        name: this.name,
        desc: this.desc,
        price: this.price,
        imageUrl: this.imageShow
      };
      this.$store
        .dispatch("products/CREATE_CATEGORY", { model })
        .then(response => {
          this.$router.push("/");
        });
    }
  }
};
</script>

<style lang="stylus">
.form {
  margin: 20px auto;
  padding: 10px;
  max-width: 600px;
}

.margin-left-20 {
  margin-left: 20px;
}
</style>
