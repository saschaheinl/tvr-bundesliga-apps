<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title> TVR Ticket Smasher </q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header> Navigation </q-item-label>

        <EssentialLink
          v-for="link in linksList"
          :key="link.title"
          v-bind="link"
          :icon="link.icon"
          :link="link.link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import EssentialLink, {
  EssentialLinkProps,
} from 'components/EssentialLink.vue';

defineOptions({
  name: 'MainLayout',
});

const linksList: EssentialLinkProps[] = [
  {
    title: 'Startseite',
    icon: 'home',
    link: '/',
  },
  {
    title: 'Events',
    caption: 'Events anlegen oder anzeigen',
    icon: 'event',
    link: '/events',
  },
  {
    title: 'Gäste',
    caption: 'Gäste suchen und anlegen',
    icon: 'people',
    link: '/guests',
  },
  {
    title: 'Tickets',
    caption: 'Tickets suchen und erstellen',
    icon: 'confirmation_number',
    link: '/tickets'
  }
];

const leftDrawerOpen = ref(false);

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}
</script>
