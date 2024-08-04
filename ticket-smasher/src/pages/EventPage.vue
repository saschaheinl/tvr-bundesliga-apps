<template>
  <q-page class="justify-evenly">
    <create-event-component></create-event-component>
    <list-events-component></list-events-component>
  </q-page>
</template>

<script setup lang="ts">
import CreateEventComponent from 'components/CreateEventComponent.vue';
import ListEventsComponent from 'components/ListEventsComponent.vue';

defineOptions({
  name: 'EventPage',
});
</script>
