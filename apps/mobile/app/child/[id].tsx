import React, { useState, useCallback } from 'react';
import {
  View,
  Text,
  StyleSheet,
  FlatList,
  Alert,
  Modal,
  TouchableOpacity,
  ActivityIndicator,
  Platform,
  RefreshControl,
  ScrollView,
  useColorScheme,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useLocalSearchParams, useRouter, useFocusEffect } from 'expo-router';
import { Colors } from '@/constants/theme';
import { VaccineDate } from '@/types/child';
import { Input } from '@/components/ui/Input';
import { Button } from '@/components/ui/Button';
import { getVaccineDates, addVaccineDate, updateVaccineDate, deleteVaccineDate } from '@/api/vaccines';
import { Plus, Edit2, Trash2, Syringe, Calendar, ChevronLeft } from 'lucide-react-native';

export default function ChildVaccinesScreen() {
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const router = useRouter();
  const { id, name } = useLocalSearchParams<{ id: string; name: string }>();

  const [vaccines, setVaccines] = useState<VaccineDate[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingVaccine, setEditingVaccine] = useState<VaccineDate | null>(null);
  const [actionLoading, setActionLoading] = useState(false);
  const [date, setDate] = useState('');
  const [note, setNote] = useState('');
  const [dateError, setDateError] = useState('');

  const fetchVaccines = async () => {
    if (!id) return;
    try {
      const res = await getVaccineDates(id);
      setVaccines(res.data);
    } catch {
      Alert.alert('Error', 'Failed to load vaccine dates');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useFocusEffect(
    useCallback(() => {
      setLoading(true);
      fetchVaccines();
    }, [id])
  );

  const onRefresh = () => {
    setRefreshing(true);
    fetchVaccines();
  };

  const validateDate = (value: string) => {
    if (!/^\d{4}-\d{2}-\d{2}$/.test(value)) {
      setDateError('Use YYYY-MM-DD format');
      return false;
    }
    setDateError('');
    return true;
  };

  const openAddModal = () => {
    setEditingVaccine(null);
    setDate('');
    setNote('');
    setDateError('');
    setModalVisible(true);
  };

  const openEditModal = (vaccine: VaccineDate) => {
    setEditingVaccine(vaccine);
    setDate(vaccine.date.split('T')[0]);
    setNote(vaccine.note ?? '');
    setDateError('');
    setModalVisible(true);
  };

  const handleSave = async () => {
    if (!validateDate(date)) return;
    setActionLoading(true);
    try {
      const payload = { date: new Date(date).toISOString(), note: note || undefined };
      if (editingVaccine) {
        await updateVaccineDate(id, editingVaccine.id, payload);
        Alert.alert('Success', 'Vaccine date updated');
      } else {
        await addVaccineDate(id, payload);
        Alert.alert('Success', 'Vaccine date added');
      }
      setModalVisible(false);
      fetchVaccines();
    } catch {
      Alert.alert('Error', 'Action failed');
    } finally {
      setActionLoading(false);
    }
  };

  const handleDelete = (vaccineId: string) => {
    Alert.alert('Delete', 'Remove this vaccine date?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Delete',
        style: 'destructive',
        onPress: async () => {
          try {
            await deleteVaccineDate(id, vaccineId);
            fetchVaccines();
          } catch {
            Alert.alert('Error', 'Failed to delete');
          }
        },
      },
    ]);
  };

  const formatDate = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleDateString(undefined, {
        year: 'numeric', month: 'long', day: 'numeric',
      });
    } catch {
      return dateString;
    }
  };

  const isUpcoming = (dateString: string) => new Date(dateString) >= new Date();

  const renderItem = ({ item }: { item: VaccineDate }) => (
    <View style={[styles.vaccineCard, { backgroundColor: colors.surface }]}>
      <View style={[
        styles.iconContainer,
        { backgroundColor: isUpcoming(item.date) ? colors.primary + '15' : '#F5F5F5' }
      ]}>
        <Syringe size={24} color={isUpcoming(item.date) ? colors.primary : colors.textSecondary} />
      </View>
      <View style={styles.vaccineInfo}>
        <Text style={[styles.vaccineDate, { color: colors.text }]}>{formatDate(item.date)}</Text>
        <View style={styles.vaccineMeta}>
          <Calendar size={12} color={colors.textSecondary} />
          <Text style={[styles.vaccineMetaText, { color: colors.textSecondary }]}>
            {item.note ? item.note : (isUpcoming(item.date) ? 'Upcoming' : 'Past')}
          </Text>
        </View>
      </View>
      <View style={styles.actionButtons}>
        <TouchableOpacity onPress={() => openEditModal(item)} style={styles.iconButton}>
          <Edit2 size={18} color={colors.primary} />
        </TouchableOpacity>
        <TouchableOpacity onPress={() => handleDelete(item.id)} style={styles.iconButton}>
          <Trash2 size={18} color={colors.error} />
        </TouchableOpacity>
      </View>
    </View>
  );

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]} edges={['top']}>
      <View style={styles.header}>
        <TouchableOpacity onPress={() => router.back()} style={styles.backButton}>
          <ChevronLeft size={28} color={colors.text} />
        </TouchableOpacity>
        <View style={styles.headerText}>
          <Text style={[styles.title, { color: colors.text }]}>{name}</Text>
          <Text style={[styles.subtitle, { color: colors.textSecondary }]}>Vaccine Schedule</Text>
        </View>
        <TouchableOpacity onPress={openAddModal}>
          <View style={[styles.addButton, { backgroundColor: colors.primary }]}>
            <Plus size={24} color="#FFFFFF" />
          </View>
        </TouchableOpacity>
      </View>

      {loading ? (
        <View style={styles.centerContainer}>
          <ActivityIndicator size="large" color={colors.primary} />
        </View>
      ) : (
        <FlatList
          data={vaccines}
          renderItem={renderItem}
          keyExtractor={(item) => item.id}
          contentContainerStyle={styles.listContent}
          showsVerticalScrollIndicator={false}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />
          }
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Syringe size={64} color={colors.border} />
              <Text style={[styles.emptyText, { color: colors.textSecondary }]}>No vaccine dates yet</Text>
              <Button
                title="Add First Date"
                onPress={openAddModal}
                variant="outline"
                style={{ marginTop: 16 }}
              />
            </View>
          }
        />
      )}

      <Modal
        animationType="none"
        transparent
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.surface }]}>
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.text }]}>
                {editingVaccine ? 'Edit Vaccine Date' : 'Add Vaccine Date'}
              </Text>
              <TouchableOpacity onPress={() => setModalVisible(false)}>
                <Text style={{ color: colors.primary, fontWeight: '600' }}>Cancel</Text>
              </TouchableOpacity>
            </View>

            <ScrollView showsVerticalScrollIndicator={false}>
              <Input
                label="Date (YYYY-MM-DD)"
                placeholder="2026-08-15"
                value={date}
                onChangeText={(v) => { setDate(v); if (dateError) validateDate(v); }}
                error={dateError}
                keyboardType="numbers-and-punctuation"
              />
              <Input
                label="Note (optional)"
                placeholder="e.g. MMR, Polio, Hepatitis B"
                value={note}
                onChangeText={setNote}
              />
              <Button
                title={editingVaccine ? 'Update' : 'Add Date'}
                onPress={handleSave}
                loading={actionLoading}
                style={{ marginTop: 24 }}
              />
            </ScrollView>
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 24,
    paddingVertical: 16,
  },
  backButton: {
    marginRight: 8,
  },
  headerText: {
    flex: 1,
  },
  title: {
    fontSize: 28,
    fontWeight: '800',
    letterSpacing: -0.5,
  },
  subtitle: {
    fontSize: 16,
    fontWeight: '500',
  },
  addButton: {
    width: 48,
    height: 48,
    borderRadius: 16,
    justifyContent: 'center',
    alignItems: 'center',
    elevation: 4,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.2,
    shadowRadius: 8,
  },
  listContent: {
    padding: 24,
    paddingTop: 0,
    paddingBottom: 100,
  },
  vaccineCard: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    borderRadius: 24,
    marginBottom: 16,
    ...Platform.select({
      ios: {
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.05,
        shadowRadius: 10,
      },
      android: {
        elevation: 2,
      },
    }),
  },
  iconContainer: {
    width: 52,
    height: 52,
    borderRadius: 18,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 16,
  },
  vaccineInfo: {
    flex: 1,
  },
  vaccineDate: {
    fontSize: 17,
    fontWeight: '700',
    marginBottom: 4,
  },
  vaccineMeta: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  vaccineMetaText: {
    fontSize: 13,
    fontWeight: '500',
  },
  actionButtons: {
    flexDirection: 'row',
    gap: 8,
  },
  iconButton: {
    padding: 8,
    borderRadius: 12,
    backgroundColor: '#F5F5F5',
  },
  centerContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyContainer: {
    alignItems: 'center',
    marginTop: 80,
    gap: 12,
  },
  emptyText: {
    fontSize: 16,
    fontWeight: '500',
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.6)',
    justifyContent: 'flex-end',
  },
  modalContent: {
    borderTopLeftRadius: 36,
    borderTopRightRadius: 36,
    padding: 24,
    paddingBottom: Platform.OS === 'ios' ? 40 : 24,
    maxHeight: '90%',
  },
  modalHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 24,
  },
  modalTitle: {
    fontSize: 22,
    fontWeight: '800',
  },
});
