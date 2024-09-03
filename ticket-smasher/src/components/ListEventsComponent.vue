<template>
  <div>
    <q-card>
      <q-tabs
        v-model="tab"
        dense
        class="text-grey"
        active-color="primary"
        indicator-color="primary"
        align="justify"
        narrow-indicator
      >
        <q-tab name="upcoming" label="Upcoming Events" />
        <q-tab name="previous" label="Previous Events" />
        <q-tab name="all" label="All Events" />
      </q-tabs>

      <q-tab-panels v-model="tab" animated>
        <q-tab-panel name="upcoming">
          <div class="text-h6">Upcoming Events</div>
          <q-btn
            color="primary"
            @click="fetchAllEvents()"
            round
            icon="refresh"
          />
          <q-table
            :rows="upcomingEvents"
            :columns="columns"
            row-key="id"
            :no-data-label="'Es wurden keine Events gefunden.'"
          />
        </q-tab-panel>

        <q-tab-panel name="previous">
          <div class="text-h6">Previous Events</div>
          <q-btn
            color="primary"
            @click="fetchAllEvents()"
            round
            icon="refresh"
          />
          <q-table
            :rows="previousEvents"
            :columns="columns"
            row-key="id"
            :no-data-label="'Es wurden keine Events gefunden.'"
          />
        </q-tab-panel>

        <q-tab-panel name="all">
          <div class="text-h6">All Events</div>
          <q-btn
            color="primary"
            @click="fetchAllEvents()"
            round
            icon="refresh"
          />
          <q-table
            :rows="allEvents"
            :columns="columns"
            row-key="id"
            :no-data-label="'Es wurden keine Events gefunden.'"
          />
        </q-tab-panel>
      </q-tab-panels>
    </q-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { Event } from './models';
import { QTableColumn } from 'quasar';

export default defineComponent({
  methods: { onMounted },
  setup() {
    const tab = ref('upcoming');
    const upcomingEvents = ref<Event[]>([]);
    const previousEvents = ref<Event[]>([]);
    const allEvents = ref<Event[]>([]);
    const columns = ref<QTableColumn<Event>[]>([
      {
        name: 'id',
        label: 'ID',
        align: 'left',
        field: (row: Event) => row.id.toString(),
        sortable: true,
        sortOrder: 'da',
      },
      {
        name: 'name',
        label: 'Name',
        align: 'left',
        field: (row: Event) => row.name,
      },
      {
        name: 'league',
        label: 'Liga',
        align: 'left',
        field: (row: Event) => row.league,
      },
      {
        name: 'date',
        label: 'Startzeit',
        align: 'left',
        field: (row: Event) => formatDate(row.date),
      },
    ]);

    const API_BASE_URL = process.env.VUE_APP_TICKET_API_BASE_URL;

    function formatDate(date: string) {
      const eventDate = new Date(date);
      return `${eventDate.toLocaleDateString('de-DE', {
        day: '2-digit',
        month: 'long',
        year: 'numeric',
      })}, ${eventDate.toLocaleTimeString('de-DE', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
      })} Uhr`;
    }

    async function fetchAllEvents() {
      const upcomingResponse = await fetch(`${API_BASE_URL}/events/upcoming`);
      const previousResponse = await fetch(`${API_BASE_URL}/events/previous`);
      const allEventsResponse = await fetch(`${API_BASE_URL}/events`);

      if (
        !upcomingResponse.ok ||
        !previousResponse.ok ||
        !allEventsResponse.ok
      ) {
        throw new Error('Failed to fetch events.');
      }

      upcomingEvents.value = await upcomingResponse.json();
      previousEvents.value = await previousResponse.json();
      allEvents.value = await allEventsResponse.json();
    }

    onMounted(() => {
      fetchAllEvents();
    });

    return {
      tab,
      upcomingEvents,
      previousEvents,
      allEvents,
      columns,
      fetchAllEvents,
    };
  },
});
</script>

<style scoped>
/* Your styles here */
</style>
