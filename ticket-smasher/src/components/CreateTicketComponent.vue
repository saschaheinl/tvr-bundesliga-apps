<template>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form class="q-gutter-md" @submit="onSubmit">
      <q-input
        filled
        v-model="ticketForCreation.eventId"
        label="ID des Events, optional"
        hint="Event ID, optional für eventbezogene Tickets"
      />

      <q-select
        filled
        v-model="ticketForCreation.type"
        :options="ticketTypeOptions"
        label="Ticket Type"
        option-label="label"
        option-value="value"
        emit-value
        map-options
      />

      <q-input
        filled
        v-model="ticketForCreation.guestId"
        label="ID des Gasts"
      />

      <q-input
        filled
        v-model="ticketForCreation.includedVisits"
        label="Anzahl besuche"
      />
      <q-btn type="submit" label="OK" />
    </q-form>

    <!-- Modal for displaying QR Code -->
    <q-dialog v-model="showQRCodeModal" persistent>
      <q-card>
        <q-card-section>
          <div class="row justify-center">
            <img :src="qrCodeUrl" alt="QR Code" ref="qrCodeImage" />
          </div>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat label="Schließen" color="primary" v-close-popup />
          <q-btn
            flat
            label="Herunterladen"
            color="primary"
            @click="downloadQRCode"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useQuasar } from 'quasar';
import { Ticket, TicketForCreation, TicketType } from './models';
import QRCode from 'qrcode';
import { TvrTicketApiClient } from 'components/tvrTicketApiClient';

export default defineComponent({
  name: 'CreateTicketComponent',
  setup() {
    const $q = useQuasar();
    const apiClient = new TvrTicketApiClient(process.env.VUE_APP_TICKET_API_BASE_URL ?? '');

    const ticketTypeOptions = [
      { label: 'Freikarte', value: TicketType.Freikarte },
      { label: 'Dauerkarte', value: TicketType.Dauerkarte },
      { label: 'Einzelkarte', value: TicketType.Einzelkarte },
    ];

    const ticketForCreation = ref<TicketForCreation>({
      eventId: undefined,
      type: 0,
      guestId: 0,
      includedVisits: 0,
      price: undefined,
    });

    const showQRCodeModal = ref(false);
    const qrCodeUrl = ref<string>('');
    let ticket = ref<Ticket>();

    async function onSubmit() {
      console.log(ticketForCreation.value);
      try {
        ticket.value = await apiClient.createNewTicket(ticketForCreation.value);
        $q.notify({
          color: 'green-4',
          textColor: 'white',
          icon: 'cloud_done',
          message: 'Ticket erfolgreich erstellt!',
        });
      } catch (e) {
        console.error(e);
        $q.notify({
          color: 'red',
          textColor: 'white',
          icon: 'cloud_error',
          message: 'Ein Fehler ist aufgetreten!'
        });

        return;
      }

      // Generate the QR Code
      qrCodeUrl.value = await QRCode.toDataURL(ticket.value.id.toString());

      // Open the modal
      showQRCodeModal.value = true;
    }

    function downloadQRCode() {
      const link = document.createElement('a');
      link.href = qrCodeUrl.value;
      link.download = 'ticket_qr_code.png';
      link.click();
    }

    return {
      ticketForCreation,
      ticketTypeOptions,
      onSubmit,
      showQRCodeModal,
      qrCodeUrl,
      downloadQRCode,
    };
  },
});
</script>

<style scoped></style>
