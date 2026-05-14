import { api } from './api';

export interface LocationData {
  latitude: number;
  longitude: number;
  withChild: boolean;
  timestamp?: number;
}

/**
 * Saves the user's current location to the database.
 * @param data The location data to save.
 */
export const saveLocation = async (data: LocationData) => {
  try {
    const response = await api.post('/locations', data);
    return response.data;
  } catch (error) {
    console.error('Failed to save location:', error);
    throw error;
  }
};
