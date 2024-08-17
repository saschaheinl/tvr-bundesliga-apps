<script lang="ts">
import { defineComponent } from 'vue';
import CreateTicketComponent from 'components/CreateTicketComponent.vue';

export default defineComponent({
  name: 'TicketPage',
  components: { CreateTicketComponent },
});
</script>

<template>
  <create-ticket-component></create-ticket-component>
</template>

<style scoped></style>
