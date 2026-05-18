export interface Child {
  id: string;
  parentId: string;
  fullName: string;
  dateOfBirth: string;
  gender: 0 | 1 | 2; // 0: Male, 1: Female, 2: Other
  createdAt?: string;
  updatedAt?: string;
}

export interface VaccineDate {
  id: string;
  childId: string;
  date: string;
  note?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface UpcomingVaccine {
  childId: string;
  fullName: string;
  date: string;
  note?: string;
}
