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

export interface EventToCreate {
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
