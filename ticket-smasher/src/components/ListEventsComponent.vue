<template>
  <div>
    <q-list>
      <q-item v-for="item in items" :key="item.id" clickable>
        <q-item-section>
          {{ item.name }}
        </q-item-section>
      </q-item>
    </q-list>
    <q-spinner v-if="loading" />
    <q-banner v-if="error" class="q-ma-md" type="negative">
      {{ error }}
    </q-banner>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';

interface Item {
  id: number;
  name: string;
}

export default defineComponent({
  name: 'DataList',
  setup() {
    const items = ref<Item[]>([]);
    const loading = ref(true);
    const error = ref<string | null>(null);

    const fetchData = async () => {
      try {
        const response = await fetch('https://api.example.com/list');
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        const data = await response.json();
        items.value = data;
      } catch (err) {
        error.value = 'Failed to load data';
      } finally {
        loading.value = false;
      }
    };

    onMounted(fetchData);

    return {
      items,
      loading,
      error
    };
  }
});
</script>

<style scoped>
/* Add any scoped styles here if necessary */
</style>
