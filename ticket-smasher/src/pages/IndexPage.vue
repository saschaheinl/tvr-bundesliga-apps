<template>
  <q-page class="justify-evenly">
    <div>
      <h4>{{ rat }}</h4>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { TvrTicketApiClient } from 'components/tvrTicketApiClient';

const rat = ref('');
const apiClient = new TvrTicketApiClient(process.env.TICKET_API_BASE_URL ?? '');

onMounted(async () => {
  rat.value = await apiClient.getRath();
});
</script>
