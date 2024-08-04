<template>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
      <q-input
        filled
        v-model="eventName"
        label="Name des Events"
        hint="Name des Events, dass du anlegen möchtest."
        lazy-rules
        :rules="[(val: string) => !!val && val.length > 0 || 'Bitte gib etwas ein.']"
      />

      <q-input
        filled
        v-model="eventLeague"
        label="Liga"
        hint="Liga, in der das Event stattfindet (optional)."
      />

      <q-input
        filled
        v-model="eventDate"
        hint="Zeitpunkt, an dem das Event beginnt."
      >
        <template v-slot:prepend>
          <q-icon name="event" class="cursor-pointer">
            <q-popup-proxy
              cover
              transition-show="scale"
              transition-hide="scale"
            >
              <q-date v-model="eventDate" mask="YYYY-MM-DD HH:mm">
                <div class="row items-center justify-end">
                  <q-btn v-close-popup label="Close" color="primary" flat />
                </div>
              </q-date>
            </q-popup-proxy>
          </q-icon>
        </template>

        <template v-slot:append>
          <q-icon name="access_time" class="cursor-pointer">
            <q-popup-proxy
              cover
              transition-show="scale"
              transition-hide="scale"
            >
              <q-time v-model="eventDate" mask="YYYY-MM-DD HH:mm" format24h>
                <div class="row items-center justify-end">
                  <q-btn v-close-popup label="Close" color="primary" flat />
                </div>
              </q-time>
            </q-popup-proxy>
          </q-icon>
        </template>
      </q-input>

      <div>
        <q-btn label="Submit" type="submit" color="primary" />
        <q-btn
          label="Reset"
          type="reset"
          color="primary"
          flat
          class="q-ml-sm"
        />
      </div>
    </q-form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useQuasar, date as quasarDate } from 'quasar';
import { EventToCreate } from './models';

export default defineComponent({
  name: 'CreateEventComponent',
  setup() {
    const $q = useQuasar();

    const eventName = ref<string>('');
    let eventLeague: string | undefined = undefined;
    const eventDate = ref<string>(
      quasarDate.formatDate(new Date(), 'YYYY-MM-DD HH:mm')
    );
    const API_BASE_URL = process.env.TICKET_API_BASE_URL;

    const onSubmit = async () => {
      const isoDate = new Date(eventDate.value.replace(' ', 'T')).toISOString();
      const eventToCreate: EventToCreate = {
        name: eventName.value,
        league: eventLeague,
        date: isoDate,
      };

      const response = await fetch(`${API_BASE_URL}/events`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(eventToCreate),
      });

      if (!response.ok) {
        $q.notify({
          color: 'red',
          textColor: 'white',
          icon: 'cloud_error',
          message: `Ein Fehler ist aufgetreten: ${response.status}: ${response.statusText}`,
        });
      }

      $q.notify({
        color: 'green-4',
        textColor: 'white',
        icon: 'cloud_done',
        message: 'Event angelegt!',
      });
    };

    const onReset = () => {
      eventName.value = '';
      eventLeague = undefined;
      eventDate.value = quasarDate.formatDate(new Date(), 'YYYY-MM-DD HH:mm');
    };

    return {
      eventName,
      eventDate,
      eventLeague,
      onSubmit,
      onReset,
    };
  },
});
</script>
