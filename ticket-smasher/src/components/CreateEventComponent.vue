<template>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form
      @submit="onSubmit"
      @reset="onReset"
      class="q-gutter-md"
    >
      <q-input
        filled
        v-model="eventName"
        label="Name des Events"
        hint='Name des Events, dass du anlegen möchtest.'
        lazy-rules
        :rules="[ val => val && val.length > 0 || 'Bitte gib etwas ein.']"
      />

      <q-input
        filled
        v-model="eventLeague"
        label="Liga"
        hint='Liga, in der das Event stattfindet (optional).'
      />

      <q-input filled v-model="eventDate" hint="Zeitpunkt, an dem das Event beginnt.">
        <template v-slot:prepend>
          <q-icon name="event" class="cursor-pointer">
            <q-popup-proxy cover transition-show="scale" transition-hide="scale">
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
            <q-popup-proxy cover transition-show="scale" transition-hide="scale">
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
        <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
      </div>
    </q-form>

  </div>
</template>
<script>
import { useQuasar, date as quasarDate } from 'quasar';
import { ref } from 'vue';

export default {
  setup() {
    const $q = useQuasar();

    const eventName = ref(null);
    const eventLeague = ref(null);
    const eventDate = ref(quasarDate.formatDate(new Date(), 'YYYY-MM-DD HH:mm'));

    return {
      eventName,
      eventDate,
      eventLeague,

      async onSubmit() {
        try {
          const isoDate = new Date(eventDate.value.replace(' ', 'T')).toISOString();
          const response = await fetch('https://localhost:7188/events', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({
              name: eventName.value,
              league: eventLeague.value,
              date: isoDate
            })
          });

          console.log(JSON.stringify(await response.json()));
          if (!response.ok) {
            throw response.error;
          }

          $q.notify({
            color: 'green-4',
            textColor: 'white',
            icon: 'cloud_done',
            message: 'Event angelegt!'
          });
        } catch (e) {
          $q.notify({
            color: 'red',
            textColor: 'white',
            icon: 'cloud_error',
            message: $`Ein Fehler ist aufgetreten: ${e.message}`
          });
        }
      },

      onReset() {
        eventName.value = null;
        eventLeague.value = null;
        eventDate.value = quasarDate.formatDate(new Date(), 'YYYY-MM-DD HH:mm');
      }
    };
  }
};
</script>
