<template>
  <q-layout view="lHh Lpr fff">
    <q-page
      class="window-height window-width row justify-center items-center"
      style="background: linear-gradient(#005293, #00395d);"
    >
      <div class="column q-pa-lg">
        <div class="row">
          <login-form />
        </div>
      </div>
    </q-page>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import LoginForm from 'components/LoginForm.vue';

export default defineComponent({
  name: 'LoginPage',
  components: {
    LoginForm,
  },
});
</script>
