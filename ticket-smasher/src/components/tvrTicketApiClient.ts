import axios, { AxiosInstance } from 'axios';
import {
  EventForCreation,
  Guest,
  Event,
  GuestToCreate,
  TicketForCreation,
  Ticket,
} from 'components/models';
import { FirebaseApp, initializeApp } from 'firebase/app';
import { getAuth, signInWithEmailAndPassword, User } from 'firebase/auth';
import { useQuasar } from 'quasar';

export class TvrTicketApiClient {
  private readonly apiClient: AxiosInstance;
  private readonly firebaseConfig;
  private readonly app: FirebaseApp;
  private user?: User;
  private readonly $q = useQuasar();

  constructor(baseUrl: string) {
    this.apiClient = axios.create({
      baseURL: baseUrl,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.firebaseConfig = {
      apiKey: process.env.FIREBASE_API_KEY ?? '',
      authDomain: process.env.FIREBASE_AUTH_DOMAIN ?? '',
    };

    this.app = initializeApp(this.firebaseConfig);
  }

  async refreshToken() {
    if (!this.user) {
      const userMailAddress =
        this.$q.sessionStorage.getItem<string>('userMailAddress');
      const password = this.$q.sessionStorage.getItem<string>('password');

      if (userMailAddress === null || password === null) {
        throw new Error();
      }

      await this.getNewToken(userMailAddress, password);

      return;
    }

    this.apiClient.defaults.headers.common[
      'Authorization'
    ] = `Bearer ${await this.user?.getIdToken()}`;
  }

  async getNewToken(currentUser: string, password: string) {
    const auth = getAuth(this.app);
    const userCredentials = await signInWithEmailAndPassword(
      auth,
      currentUser,
      password
    );
    this.user = userCredentials.user;

    this.apiClient.defaults.headers.common[
      'Authorization'
    ] = `Bearer ${await this.user.getIdToken()}`;
  }

  async getRath(): Promise<string> {
    const response = await this.apiClient.get('', {
      method: 'get',
    });
    if (response.status !== 200) {
      return 'Es gibt nur ein Gas: Vollgas! Es gibt nur ein Rat: Refrath!';
    }

    return response.data;
  }

  async getAllEvents(): Promise<Event[]> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient('/events', {
      method: 'get',
    });

    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient('/events', {
        method: 'get',
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting all Events from API. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewEvent(event: EventForCreation): Promise<Event> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.post('/events', event);
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.post('/events', event);
    }

    if (response.status !== 201 || 202 || 204) {
      console.log(
        `Error while creating event in API. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getPreviousEvents(): Promise<Event[]> {
    if (!this.user) {
      await this.refreshToken();
    }
    let response = await this.apiClient.get('/events/previous', {
      method: 'get',
    });

    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get('/events/previous', {
        method: 'get',
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting previous events in the API. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getEventById(id: number): Promise<Event | undefined> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get(`/events/${id}`, {
      method: 'get',
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get(`/events/${id}`, {
        method: 'get',
      });
    }

    if (response.status === 404) {
      return undefined;
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting the event by its ID. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async updateEventById(id: number, event: EventForCreation): Promise<Event> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.put(`/events/${id}`, event);
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.put(`/events/${id}`, event);
    }

    if (response.status !== 201 || 204 || 204) {
      console.log(
        `Error while creating event in API. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getUpcomingEvents(): Promise<Event[]> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get('/events/upcoming', {
      method: 'get',
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get('/events/upcoming', {
        method: 'get',
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting previoud events in the API. Receiv HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewGuest(guest: GuestToCreate): Promise<Guest> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.post('/guests', guest);
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.post('/guests', guest);
    }

    if (response.status !== 201 || 204 || 204) {
      console.log(
        `Error while creating gues in API. Received HTTP status ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getAllGuests(): Promise<Guest[]> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get('/guests', {
      method: 'get',
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get('/guests', {
        method: 'get',
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting all guests. Received unexpected status code ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getGuestById(id: number): Promise<Guest | undefined> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get(`/guests/${id}`, {
      method: 'get',
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get(`/guests/${id}`, {
        method: 'get',
      });
    }

    if (response.status === 404) {
      return undefined;
    }
    if (response.status !== 200) {
      console.log(
        `Error while getting guest with ID ${id}. Received unexpected status code ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async searchGuests(
    id?: number,
    name?: string,
    mailAddress?: string
  ): Promise<Guest[]> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get('/guests/search', {
      method: 'get',
      params: {
        id: id,
        name: name,
        mailAddress: mailAddress,
      },
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get('/guests/search', {
        method: 'get',
        params: {
          id: id,
          name: name,
          mailAddress: mailAddress,
        },
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while searching guests by the provided parameters. Received status code ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewTicket(ticket: TicketForCreation): Promise<Ticket> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.post('/tickets', ticket);
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.post('/tickets', ticket);
    }

    if (response.status !== 200) {
      console.log(
        `Error while creating ticket. Received status code ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getTicketById(id: number): Promise<Ticket> {
    if (!this.user) {
      await this.refreshToken();
    }

    let response = await this.apiClient.get(`/tickets/${id}`, {
      method: 'get',
    });
    if (response.status === 401) {
      await this.refreshToken();
      response = await this.apiClient.get(`/tickets/${id}`, {
        method: 'get',
      });
    }

    if (response.status !== 200) {
      console.log(
        `Error while getting ticket. Received status code ${response.status}`
      );
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }
}
