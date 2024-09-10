export interface Todo {
  id: number;
  content: string;
}

export interface Meta {
  totalCount: number;
}

export interface Event {
  id: number;
  name: string;
  league?: string;
  date: string;
}

export interface EventForCreation {
  name: string;
  league?: string;
  date: string;
}

export interface GuestSearchCriteria {
  id?: number;
  name?: string;
  emailAddress?: string;
}

export interface GuestToCreate {
  name: string;
  emailAddress: string;
}

export interface Guest {
  id: number;
  name: string;
  emailAddress: string;
}

export enum TicketType {
  Freikarte,
  Dauerkarte,
  Einzelkarte,
}

export interface TicketForCreation {
  eventId?: number;
  type: TicketType;
  guestId: number;
  includedVisits: number;
  price?: number;
}

export interface Ticket {
  id: number;
  type: TicketType;
  includedVisits: number;
  remainingVisits: number;
  price?: number;
  eventId: number;
  guestId: number;
}
