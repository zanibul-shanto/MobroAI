/**
 * Application configuration constants.
 * You can modify these values to change the behavior of the app.
 */

export const Config = {
  // Interval for background location updates in milliseconds.
  // 5 minutes = 5 * 60 * 1000 = 300000 ms.
  LOCATION_UPDATE_INTERVAL_MS: 10000,

  // Distance interval for location updates in meters.
  // 0 means updates are based on time only (if supported by OS).
  LOCATION_DISTANCE_INTERVAL_METERS: 0,

  // Accuracy level for location tracking.
  // Balanced is usually sufficient for background tracking and saves battery.
  LOCATION_ACCURACY: 'balanced',
};
