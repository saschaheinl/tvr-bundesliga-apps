<template>
  <q-item clickable tag="router-link" :to="link">
    <q-item-section avatar>
      <q-icon :name="icon" />
    </q-item-section>
    <q-item-section>
      <q-item-label>{{ title }}</q-item-label>
      <q-item-label caption>{{ caption }}</q-item-label>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';

export interface EssentialLinkProps {
  title: string;
  caption?: string;
  icon: string;
  link: string;
}

defineProps<EssentialLinkProps>();
</script>
