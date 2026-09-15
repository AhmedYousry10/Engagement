export interface DetailCard {
  id: number;
  title: string;
  body: string;
  sortOrder: number;
}

export interface DetailCardUpsert {
  title: string;
  body: string;
  sortOrder: number;
}
