import axios, { AxiosInstance } from 'axios';
import { EventForCreation, Guest, Event, GuestToCreate, TicketForCreation, Ticket } from 'components/models';

export class TvrTicketApiClient {
  private readonly apiClient: AxiosInstance;

  constructor(baseUrl: string) {
    this.apiClient = axios.create({
      baseURL: baseUrl,
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  async getRath(): Promise<string> {
    const response = await this.apiClient.get('', {
      method: 'get'
    });
    if(response.status !== 200){
      return 'Es gibt nur ein Gas: Vollgas! Es gibt nur ein Rat: Refrath!';
    }

    return response.data;
  }

  async getAllEvents(): Promise<Event[]> {
    const response = await this.apiClient('/events', {
      method: 'get'
    });
    if (response.status !== 200 ){
      console.log(`Error while getting all Events from API. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewEvent(event: EventForCreation): Promise<Event> {
    const response = await this.apiClient.post('/events',event);
    if (response.status !== 201 || 204 || 204){
      console.log(`Error while creating event in API. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getPreviousEvents():Promise<Event[]>{
    const response = await this.apiClient.get('/events/previous', {
      method: 'get'
    });
    if(response.status !== 200){
      console.log(`Error while getting previous events in the API. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getEventById(id: number): Promise<Event | undefined> {
    const response = await this.apiClient.get(`/events/${id}`, {
      method: 'get'
    });
    if (response.status === 404){
      return undefined;
    }

    if (response.status !== 200) {
      console.log(`Error while getting the event by its ID. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async updateEventById(id: number, event: EventForCreation): Promise<Event> {
    const response = await this.apiClient.put(`/events/${id}`,event);
    if (response.status !== 201 || 204 || 204){
      console.log(`Error while creating event in API. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getUpcomingEvents(): Promise<Event[]> {
    const response = await this.apiClient.get('/events/upcoming', {
      method: 'get'
    });
    if(response.status !== 200){
      console.log(`Error while getting previoud events in the API. Receiv HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewGuest(guest: GuestToCreate): Promise<Guest> {
    const response =  await this.apiClient.post('/guests', guest);
    if (response.status !== 201 || 204 || 204){
      console.log(`Error while creating gues in API. Received HTTP status ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getAllGuests(): Promise<Guest[]> {
    const response = await this.apiClient.get('/guests', {
      method: 'get'
    });
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
    const response = await this.apiClient.get(`/guests/${id}`, {
      method: 'get'
    });
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

  async searchGuests(id?: number, name?: string, mailAddress?: string): Promise<Guest[]> {
    const response = await this.apiClient.get('/guests/search', {
      method:'get',
      params: {
        id: id,
        name: name,
        mailAddress: mailAddress
      }
    });
    if (response.status !== 200){
      console.log(`Error while searching guests by the provided parameters. Received status code ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async createNewTicket(ticket: TicketForCreation): Promise<Ticket> {
    const response = await this.apiClient.post('/tickets', ticket);
    if (response.status !== 200){
      console.log(`Error while creating ticket. Received status code ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }

  async getTicketById(id: number): Promise<Ticket> {
    const response = await this.apiClient.get(`/tickets/${id}`, {
      method: 'get'
    });
    if(response.status !== 200) {
      console.log(`Error while getting ticket. Received status code ${response.status}`);
      console.log(response.data);

      throw new Error();
    }

    return response.data;
  }
}
