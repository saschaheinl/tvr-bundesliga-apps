<template>
  <q-page class="justify-evenly">
    <div>
      <h4>{{ rat }}</h4>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';

const rat = ref('');

const fetchRat = async () => {
  const apiUrl = process.env.TICKET_API_BASE_URL;
  const response = await fetch(`${apiUrl}`);
  rat.value = await response.text();
};

onMounted(() => {
  fetchRat();
});
</script>
