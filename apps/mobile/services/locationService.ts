import * as TaskManager from 'expo-task-manager';
import * as Location from 'expo-location';
import { saveLocation } from '@/api/location';
import { Config } from '@/constants/Config';
import * as SecureStore from 'expo-secure-store';

export const LOCATION_TRACKING_TASK = 'LOCATION_TRACKING_TASK';

// Local variable to keep track of the last update in memory for efficiency.
let lastUpdateTime = 0;
const LAST_UPDATE_KEY = 'last_location_update_timestamp';

TaskManager.defineTask(LOCATION_TRACKING_TASK, async ({ data, error }) => {
  if (error) {
    console.error('Location Task Error:', error);
    return;
  }

  if (data) {
    const { locations } = data as { locations: Location.LocationObject[] };
    if (locations && locations.length > 0) {
      const location = locations[0];
      const now = Date.now();

      // Check if enough time has passed since the last update
      // We check both memory and SecureStore for robustness across app restarts
      if (lastUpdateTime === 0) {
        const storedLastUpdate = await SecureStore.getItemAsync(LAST_UPDATE_KEY);
        lastUpdateTime = storedLastUpdate ? parseInt(storedLastUpdate, 10) : 0;
      }

      if (now - lastUpdateTime >= Config.LOCATION_UPDATE_INTERVAL_MS) {
        try {
          console.log(`[Location Service] Sending update: ${location.coords.latitude}, ${location.coords.longitude}`);
          
          await saveLocation({
            latitude: location.coords.latitude,
            longitude: location.coords.longitude,
            withChild: true, // Default value as per user example
            timestamp: location.timestamp,
          });

          // Update last update time
          lastUpdateTime = now;
          await SecureStore.setItemAsync(LAST_UPDATE_KEY, now.toString());
        } catch (apiError) {
          console.error('[Location Service] Failed to send location to API:', apiError);
        }
      } else {
        const remaining = Math.round((Config.LOCATION_UPDATE_INTERVAL_MS - (now - lastUpdateTime)) / 1000);
        console.log(`[Location Service] Skipping update. Next update in ${remaining}s`);
      }
    }
  }
});
