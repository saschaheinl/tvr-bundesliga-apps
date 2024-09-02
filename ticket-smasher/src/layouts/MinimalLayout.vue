<template>
  <q-layout view="lHh Lpr fff"> <!-- No header, drawer, or footer -->
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">

defineOptions({
  name: 'MinimalLayout',
});

</script>
