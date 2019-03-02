<template>
</template>
<script>
import Router from 'vue-router'
export default {
  name: 'callback',
  computed: {
    callbackUrl() {
      return this.$store.state.account.callbackUrl || '/'
    }
  },
  mounted() {
    if (this.$route.hash) {
      var accessToken = this.getKeyValueFromUrl(
        this.$route.hash.substring(1),
        'access_token'
      )
      var idToken = this.getKeyValueFromUrl(
        this.$route.hash.substring(1),
        'id_token'
      )
      this.$store.commit('account/LOGIN_SUCCESS', { accessToken, idToken })
      var router = new Router({
        mode: 'history'
      })
      this.$router.push(this.callbackUrl)
    }
  },
  methods: {
    getKeyValueFromUrl(source, key) {
      let value = null
      source.split('&').forEach(function(part) {
        var item = part.split('=')
        if (item[0] && item[0].trim() == key.trim()) {
          value = item[1]
        }
      })
      return value
    }
  }
}
</script>
