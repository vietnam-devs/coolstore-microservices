<template>
    <div>
      Unauthorized
      <button @click="login">Login</button>
    </div>
</template>
<script>
import { oidcSettings } from '../oidcConfig'
var applicationUserManager = () => import('oidc-client')

export default {
  name: 'unauthorized',
  beforeMount() {
    if(!this.$store.state.isLoggedIn && !this.$store.state.isLoggedIn){
      this.login();
    }
  },
  methods: {
    login() {
      applicationUserManager().then(obj => {
        let userManger = new obj.UserManager(oidcSettings)
        userManger.signinRedirect().then(response => {
          console.log('Authorired success!')
        })
      })
    }
  }
}
</script>