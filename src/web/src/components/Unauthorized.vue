<template>
  <div>
    Unauthorized
    <button @click="login">Login</button>
  </div>
</template>
<script>
import { oidcSettings } from "../oidcConfig";
var applicationUserManager = () => import("oidc-client");
import { login } from "../auth/usermanager";

export default {
  name: "unauthorized",
  beforeMount() {
    if (!this.$store.state.isLoggedIn && !this.$store.state.isLoggedIn) {
      this.login();
    }
  },
  methods: {
    login() {
      login(function(user) {
        user.then(response => {
          console.log("Authorired success!");
        });
      });
    }
  }
};
</script>
