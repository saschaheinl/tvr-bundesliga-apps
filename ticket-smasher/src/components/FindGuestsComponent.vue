<template>
  <div class="container">
    <div class="main-content">
      <div class="form-container">
        <q-form @submit="onSubmit" class="form">
          <div class="form-fields">
            <q-input
              filled
              type="number"
              v-model="searchCriteria.id"
              label="Suche basierend auf ID des Gasts"
            />

            <q-input
              filled
              v-model="searchCriteria.name"
              label="Suche basierend auf einem Namen"
            />

            <q-input
              filled
              type="email"
              v-model="searchCriteria.emailAddress"
              label="Suche nach E-Mail Adresse"
            />
          </div>
          <div class="submit-button">
            <q-btn
              label="Suchen"
              type="submit"
              color="primary"
              @click="onSubmit"
            />
          </div>
        </q-form>
      </div>
      <div class="table-container">
        <q-table
          :rows="foundGuests"
          :columns="columns"
          row-key="id"
          :no-data-label="'Leider wurden keine Gäste mit diesen Kriterien gefunden.'"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.main-content {
  display: flex;
  gap: 1rem;
}

.form-container {
  flex: 1;
  max-width: 600px;
}

.form {
  display: flex;
  flex-direction: column;
}

.form-fields {
  display: flex;
  flex-direction: column;
}

.form-fields > * {
  margin-bottom: 1rem;
}

.form-fields > *:last-of-type {
  margin-bottom: 2rem;
}

.submit-button {
  display: flex;
  justify-content: center;
}

.table-container {
  flex: 2;
  overflow: auto;
}
</style>
<script lang="ts">
import { defineComponent, reactive, ref } from 'vue';
import { Guest, GuestSearchCriteria } from 'components/models';
import { QTableColumn } from 'quasar';
import { TvrTicketApiClient } from 'components/tvrTicketApiClient';

export default defineComponent({
  setup() {
    const searchCriteria = reactive<GuestSearchCriteria>({
      id: undefined,
      name: undefined,
      emailAddress: undefined
    });
    const foundGuests = ref<Guest[]>([]);
    const columns = ref<QTableColumn<Guest>[]>([
      {
        name: 'id',
        label: 'ID',
        align: 'left',
        field: (row) => row.id.toString(),
        sortable: true,
        sortOrder: 'ad'
      },
      {
        name: 'name',
        label: 'Name',
        align: 'left',
        field: (row) => row.name,
        sortable: true
      },
      {
        name: 'mail',
        label: 'E-Mail Adresse',
        align: 'left',
        field: (row) => row.emailAddress,
        sortable: true
      }
    ]);
    const apiClient = new TvrTicketApiClient(process.env.VUE_APP_TICKET_API_BASE_URL ?? '');

    async function onSubmit() {
      try {
        foundGuests.value = await apiClient.searchGuests(searchCriteria.id, searchCriteria.name, searchCriteria.emailAddress);
      } catch (e) {
        console.error(e);
      }
    }

    return { onSubmit, searchCriteria, foundGuests, columns };
  }
});
</script>
