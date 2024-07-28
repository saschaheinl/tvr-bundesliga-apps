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
