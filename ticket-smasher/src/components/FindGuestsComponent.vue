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
  gap: 1rem; /* Space between form/table and the container edges */
}

.main-content {
  display: flex;
  gap: 1rem; /* Space between form and table */
}

.form-container {
  flex: 1;
  max-width: 600px; /* Set a max-width for the form container */
}

.form {
  display: flex;
  flex-direction: column; /* Stack form fields vertically */
}

.form-fields {
  display: flex;
  flex-direction: column; /* Ensure form fields stack vertically */
}

.form-fields > * {
  margin-bottom: 1rem; /* Default space between form fields */
}

.form-fields > *:last-of-type {
  margin-bottom: 2rem; /* Double space after the last input */
}

.submit-button {
  display: flex;
  justify-content: center; /* Center the button horizontally */
}

.table-container {
  flex: 2; /* Allow table container to take more space */
  overflow: auto; /* Add scroll if needed */
}
</style>
<script lang="ts">
import { defineComponent, reactive, ref } from 'vue';
import { Guest, GuestSearchCriteria } from 'components/models';
import superagent from 'superagent';
import { QTableColumn } from 'quasar';

export default defineComponent({
  setup() {
    const searchCriteria = reactive<GuestSearchCriteria>({
      id: undefined,
      name: undefined,
      emailAddress: undefined,
    });

    const foundGuests = ref<Guest[]>([]);

    const columns = ref<QTableColumn<Guest>[]>([
      {
        name: 'id',
        label: 'ID',
        align: 'left',
        field: (row) => row.id.toString(),
        sortable: true,
        sortOrder: 'ad',
      },
      {
        name: 'name',
        label: 'Name',
        align: 'left',
        field: (row) => row.name,
        sortable: true,
      },
      {
        name: 'mail',
        label: 'E-Mail Adresse',
        align: 'left',
        field: (row) => row.emailAddress,
        sortable: true,
      },
    ]);

    const API_BASE_URL = process.env.VUE_APP_TICKET_API_BASE_URL;

    async function onSubmit() {
      try {
        const response = await superagent
          .get(`${API_BASE_URL}/guests/search`)
          .query({ id: searchCriteria.id })
          .query({ name: searchCriteria.name })
          .query({ mailAddress: searchCriteria.emailAddress });

        foundGuests.value = response.body;
      } catch (e) {
        console.error(e);
      }
    }

    return { onSubmit, searchCriteria, foundGuests, columns };
  },
});
</script>
