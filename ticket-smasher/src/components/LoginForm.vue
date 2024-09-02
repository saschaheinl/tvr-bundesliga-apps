<template>
  <div class="login-page">
    <q-card square class="shadow-24" style="width:400px;height:400px;">
      <q-card-section class="bg-custom-blue">
        <h4 class="text-h5 text-white q-my-md">TV Refrath Ticket Smasher</h4>
      </q-card-section>
      <q-card-section>
        <q-form class="q-px-sm q-pt-xl" @submit="login">
          <q-input
            v-model="email"
            label="E-Mail-Adresse"
            square
            type="email"
          >
            <template v-slot:prepend>
              <q-icon name="email" />
            </template>
          </q-input>

          <q-input
            square
            clearable
            v-model="password"
            :type="'password'"
            label="Passwort"
          >
            <template v-slot:prepend>
              <q-icon name="lock" />
            </template>

          </q-input>
          <q-card-actions class="q-px-lg">
            <q-btn
              unelevated
              size="lg"
              :color="buttonColor"
              class="full-width text-white"
              label="Einloggen"
              type="submit"
            />
          </q-card-actions>
        </q-form>
      </q-card-section>


    </q-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { getAuth, signInWithEmailAndPassword } from 'firebase/auth';
import { initializeApp, getApps, getApp } from 'firebase/app';
import { useQuasar } from 'quasar';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'LoginForm',
  setup() {
    const $q = useQuasar();
    const router = useRouter();
    const email = ref<string>('');
    const password = ref<string>('');
    const passwordFieldType = ref<'text' | 'password'>('password');

    const visibilityIcon = ref('visibility');
    const title = ref('Anmeldung');
    const backgroundStyle = ref('background: linear-gradient(#005293, #00395d);');
    const buttonColor = ref('custom-blue');

    const required = (val: string): string | boolean => {
      return (val && val.length > 0) || 'Dieses Feld muss ausgefüllt werden';
    };

    const login = () => {
      const encodedPassword = btoa(password.value);
      window.sessionStorage.setItem('email', email.value);
      window.sessionStorage.setItem('password', encodedPassword);

      let app;
      if (!getApps().length) {
        app = initializeApp({
          apiKey: process.env.VUE_APP_FIREBASE_API_KEY ?? '',
          authDomain: process.env.VUE_APP_FIREBASE_AUTH_DOMAIN ?? '',
        });
      } else {
        app = getApp();
      }

      const auth = getAuth(app);

      signInWithEmailAndPassword(auth, email.value, password.value)
        .then(() => {
          $q.notify({
            icon: 'done',
            color: 'positive',
            message: 'Anmeldung erfolgreich',
          });

          router.push('/');
        })
        .catch((error) => {
          $q.notify({
            icon: 'error',
            color: 'negative',
            message: error.message,
          });
        });
    };

    return {
      email,
      password,
      passwordFieldType,
      visibilityIcon,
      title,
      backgroundStyle,
      buttonColor,
      required,
      login,
    };
  },
});
</script>

<style scoped>
.login-page {
  font-family: 'Roboto', sans-serif;
}

.bg-custom-blue {
  background-color: #005293;
}

.custom-blue {
  background-color: #005293;
  color: white;
}
</style>
