<template>
  <q-page class="justify-evenly">
    <find-guests-component></find-guests-component>
    <create-guest-component></create-guest-component>
  </q-page>
</template>

<script setup lang="ts">
import FindGuestsComponent from 'components/FindGuestsComponent.vue';
import CreateGuestComponent from 'components/CreateGuestComponent.vue';

defineOptions({
  name: 'GuestPage',
});
</script>
