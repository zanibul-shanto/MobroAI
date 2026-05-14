import { useState, useCallback } from 'react';
import * as Location from 'expo-location';
import { Alert, Platform } from 'react-native';
import { LOCATION_TRACKING_TASK } from '@/services/locationService';
import { Config } from '@/constants/Config';

export const useLocationTracking = () => {
  const [isTracking, setIsTracking] = useState(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  const requestPermissions = async () => {
    try {
      // 1. Request Foreground Permission
      const { status: foregroundStatus } = await Location.requestForegroundPermissionsAsync();
      if (foregroundStatus !== 'granted') {
        setErrorMsg('Foreground location permission is required.');
        Alert.alert('Permission Denied', 'Foreground location permission is required to track your location.');
        return false;
      }

      // 2. Request Background Permission (Android Only focus as requested)
      if (Platform.OS === 'android') {
        const { status: backgroundStatus } = await Location.requestBackgroundPermissionsAsync();
        if (backgroundStatus !== 'granted') {
          setErrorMsg('Background location permission is required.');
          Alert.alert(
            'Background Access Needed',
            'To track your location every 5 minutes while the app is closed, please select "Allow all the time" in the app settings.'
          );
          return false;
        }
      }

      return true;
    } catch (error) {
      console.error('Error requesting permissions:', error);
      setErrorMsg('An error occurred while requesting permissions.');
      return false;
    }
  };

  const startTracking = useCallback(async () => {
    const hasPermission = await requestPermissions();
    if (!hasPermission) return;

    try {
      const isRegistered = await Location.hasStartedLocationUpdatesAsync(LOCATION_TRACKING_TASK);
      if (isRegistered) {
        console.log('Location tracking is already running.');
        setIsTracking(true);
        return;
      }

      await Location.startLocationUpdatesAsync(LOCATION_TRACKING_TASK, {
        accuracy: Location.Accuracy.Balanced,
        // Hint for Android to save battery
        timeInterval: Config.LOCATION_UPDATE_INTERVAL_MS,
        distanceInterval: Config.LOCATION_DISTANCE_INTERVAL_METERS,
        // Keep tracking active in background
        foregroundService: {
          notificationTitle: 'MobroAI Location Tracking',
          notificationBody: 'Tracking your location in the background.',
          notificationColor: '#E6F4FE',
        },
      });

      console.log('Location tracking started successfully.');
      setIsTracking(true);
    } catch (error) {
      console.error('Failed to start location tracking:', error);
      setErrorMsg('Failed to start location tracking.');
    }
  }, []);

  const stopTracking = useCallback(async () => {
    try {
      const isRegistered = await Location.hasStartedLocationUpdatesAsync(LOCATION_TRACKING_TASK);
      if (isRegistered) {
        await Location.stopLocationUpdatesAsync(LOCATION_TRACKING_TASK);
      }
      setIsTracking(false);
      console.log('Location tracking stopped.');
    } catch (error) {
      console.error('Failed to stop location tracking:', error);
    }
  }, []);

  const checkTrackingStatus = useCallback(async () => {
    try {
      const isRegistered = await Location.hasStartedLocationUpdatesAsync(LOCATION_TRACKING_TASK);
      setIsTracking(isRegistered);
      return isRegistered;
    } catch (error) {
      return false;
    }
  }, []);

  return {
    isTracking,
    errorMsg,
    startTracking,
    stopTracking,
    checkTrackingStatus,
  };
};
