import { Alert } from 'react-native';
import { useCallback, useState, DependencyList } from 'react';
import { useFocusEffect } from 'expo-router';

export function useFetchData<T>(
  fetcher: () => Promise<T>,
  initialData: T,
  deps: DependencyList = []
) {
  const [data, setData] = useState<T>(initialData);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  const load = useCallback(async () => {
    try {
      setData(await fetcher());
    } catch {
      Alert.alert('Error', 'Failed to load data. Please try again.');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, deps);

  useFocusEffect(
    useCallback(() => {
      setLoading(true);
      load();
    }, [load])
  );

  const onRefresh = useCallback(() => {
    setRefreshing(true);
    load();
  }, [load]);

  return { data, loading, refreshing, onRefresh, refetch: load };
}
