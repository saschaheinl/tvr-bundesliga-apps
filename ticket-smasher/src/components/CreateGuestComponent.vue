<template>
  <q-form @submit="onSubmit">
    <q-input
      filled
      v-model="guestForCreation.name"
      label="Name des Gasts"
      hint='bspw. "1. BC Beuel".'
    />
    <q-input
      filled
      v-model="guestForCreation.emailAddress"
      label="E-Mail-Adresse des Gasts"
      hint='bspw. "info@bcbeuel.de"'
    />
    <div>
      <q-btn label="Anlegen" type="submit" color="primary" />
    </div>
  </q-form>
</template>
<script lang="ts">
import { defineComponent, reactive } from 'vue';
import { useQuasar } from 'quasar';
import { GuestToCreate } from 'components/models';
import { TvrTicketApiClient } from 'components/tvrTicketApiClient';

export default defineComponent({
  name: 'CreateGuestComponent',
  setup() {
    const $q = useQuasar();
    const guestForCreation = reactive<GuestToCreate>({
      name: '',
      emailAddress: '',
    });

    const apiClient = new TvrTicketApiClient(process.env.VUE_APP_TICKET_API_BASE_URL ?? '');

    const onSubmit = async () => {
      if (
        guestForCreation.name.length == 0 ||
        guestForCreation.emailAddress.length == 0
      ) {
        $q.notify({
          color: 'red',
          textColor: 'white',
          icon: 'cloud_error',
          message: 'Bitte gib alle Informationen an, die notwendig sind.',
        });

        return;
      }

      const guestToCreate: GuestToCreate = {
        name: guestForCreation.name,
        emailAddress: guestForCreation.emailAddress,
      };

      try {
        await apiClient.createNewGuest(guestToCreate);
        $q.notify({
          color: 'green-4',
          textColor: 'white',
          icon: 'cloud_done',
          message: 'Gast angelegt!',
        });
      } catch (e) {
        $q.notify({
          color: 'red',
          textColor: 'white',
          icon: 'cloud_error',
          message: 'Ein Fehler ist aufgetreten!',
        });
      }
    };

    return {
      onSubmit,
      guestForCreation,
    };
  },
});
</script>
