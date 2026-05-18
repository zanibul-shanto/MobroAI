import { api } from './api';
import { UpcomingVaccine, VaccineDate } from '@/types/child';

export const getVaccineDates = (childId: string) =>
  api.get<VaccineDate[]>(`/children/${childId}/vaccines`);

export const addVaccineDate = (childId: string, data: { date: string; note?: string }) =>
  api.post<VaccineDate>(`/children/${childId}/vaccines`, data);

export const updateVaccineDate = (childId: string, id: string, data: { date: string; note?: string }) =>
  api.put<void>(`/children/${childId}/vaccines/${id}`, data);

export const deleteVaccineDate = (childId: string, id: string) =>
  api.delete<void>(`/children/${childId}/vaccines/${id}`);

export const getUpcomingVaccines = (parentId: string, limit = 3) =>
  api.get<UpcomingVaccine[]>(`/children/parent/${parentId}/upcoming-vaccines`, { params: { limit } });
