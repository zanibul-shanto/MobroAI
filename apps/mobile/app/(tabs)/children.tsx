import React, { useState } from 'react';
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
  useColorScheme
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { useAuthStore } from '@/store/authStore';
import { Colors } from '@/constants/theme';
import { api } from '@/api/api';
import { Child } from '@/types/child';
import { Button } from '@/components/ui/Button';
import { Input } from '@/components/ui/Input';
import { Plus, Edit2, Trash2, Baby, Calendar, ChevronRight } from 'lucide-react-native';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { fullNameField } from '@/validation/schemas';
import { GENDER_COLORS } from '@/constants/enums';
import { useFetchData } from '@/hooks/useFetchData';

const childSchema = z.object({
  fullName: fullNameField,
  dateOfBirth: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, 'Use YYYY-MM-DD format'),
  gender: z.number().min(0).max(2),
});

type ChildForm = z.infer<typeof childSchema>;

export default function ChildrenScreen() {
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const { user } = useAuthStore();
  const router = useRouter();
  const { data: children, loading, refreshing, onRefresh, refetch: fetchChildren } = useFetchData(
    async () => {
      if (!user) return [] as Child[];
      const response = await api.get(`/children/parent/${user.id}`);
      return Array.isArray(response.data) ? response.data as Child[] : [] as Child[];
    },
    [] as Child[],
    [user]
  );
  const [modalVisible, setModalVisible] = useState(false);
  const [editingChild, setEditingChild] = useState<Child | null>(null);
  const [actionLoading, setActionLoading] = useState(false);

  const { control, handleSubmit, reset, setValue, formState: { errors } } = useForm<ChildForm>({
    resolver: zodResolver(childSchema),
    defaultValues: {
      fullName: '',
      dateOfBirth: '',
      gender: 0,
    }
  });

  const onSubmit = async (data: ChildForm) => {
    if (!user) return;
    setActionLoading(true);
    try {
      const payload = {
        ...data,
        parentId: user.id,
        dateOfBirth: new Date(data.dateOfBirth).toISOString(),
      };

      if (editingChild) {
        await api.put(`/children/${editingChild.id}`, payload);
        Alert.alert('Success', 'Child info updated');
      } else {
        await api.post('/children', payload);
        Alert.alert('Success', 'Child added successfully');
      }
      setModalVisible(false);
      fetchChildren();
    } catch (error: any) {
      const message = error.response?.data?.message || 'Action failed';
      Alert.alert('Error', message);
    } finally {
      setActionLoading(false);
    }
  };

  const handleEdit = (child: Child) => {
    setEditingChild(child);
    setValue('fullName', child.fullName);
    try {
      const date = new Date(child.dateOfBirth).toISOString().split('T')[0];
      setValue('dateOfBirth', date);
    } catch (e) {
      setValue('dateOfBirth', '');
    }
    setValue('gender', child.gender);
    setModalVisible(true);
  };

  const handleDelete = (id: string) => {
    Alert.alert(
      'Delete Child',
      'Are you sure you want to remove this child?',
      [
        { text: 'Cancel', style: 'cancel' },
        { 
          text: 'Delete', 
          style: 'destructive', 
          onPress: async () => {
            try {
              await api.delete(`/children/${id}`);
              fetchChildren();
            } catch {
              Alert.alert('Error', 'Failed to delete');
            }
          }
        }
      ]
    );
  };

  const openAddModal = () => {
    setEditingChild(null);
    reset({
      fullName: '',
      dateOfBirth: '',
      gender: 0,
    });
    setModalVisible(true);
  };

  const formatDate = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleDateString();
    } catch {
      return 'N/A';
    }
  };

  const renderChildItem = ({ item }: { item: Child }) => (
    <View style={[styles.childCard, { backgroundColor: colors.surface }]}>
      <TouchableOpacity
        style={styles.childTappable}
        onPress={() => router.push({ pathname: '/child/[id]', params: { id: item.id, name: item.fullName } })}
        activeOpacity={0.7}
      >
        <View style={[styles.avatarContainer, { backgroundColor: (GENDER_COLORS[item.gender] ?? GENDER_COLORS[2]).bg }]}>
          <Baby size={24} color={(GENDER_COLORS[item.gender] ?? GENDER_COLORS[2]).icon} />
        </View>
        <View style={styles.childInfo}>
          <Text style={[styles.childName, { color: colors.text }]}>{item.fullName}</Text>
          <View style={styles.childMeta}>
            <Calendar size={12} color={colors.textSecondary} />
            <Text style={[styles.childMetaText, { color: colors.textSecondary }]}>
              {formatDate(item.dateOfBirth)}
            </Text>
          </View>
        </View>
        <ChevronRight size={16} color={colors.textSecondary} style={{ marginRight: 4 }} />
      </TouchableOpacity>
      <View style={styles.actionButtons}>
        <TouchableOpacity onPress={() => handleEdit(item)} style={styles.iconButton}>
          <Edit2 size={18} color={colors.primary} />
        </TouchableOpacity>
        <TouchableOpacity onPress={() => handleDelete(item.id)} style={styles.iconButton}>
          <Trash2 size={18} color={colors.error} />
        </TouchableOpacity>
      </View>
    </View>
  );

  if (!user) return null;

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]} edges={['top']}>
      <View style={styles.header}>
        <View>
          <Text style={[styles.title, { color: colors.text }]}>Children</Text>
          <Text style={[styles.subtitle, { color: colors.textSecondary }]}>
            {user.role === 1 ? 'Managed Children' : 'My Children'}
          </Text>
        </View>
        <TouchableOpacity onPress={openAddModal}>
          <View
            style={[styles.addButton, { backgroundColor: colors.primary }]}
          >
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
          data={children}
          renderItem={renderChildItem}
          keyExtractor={(item) => item.id}
          contentContainerStyle={styles.listContent}
          showsVerticalScrollIndicator={false}
          refreshControl={
            <RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />
          }
          ListEmptyComponent={
            <View style={styles.emptyContainer}>
              <Baby size={64} color={colors.border} />
              <Text style={[styles.emptyText, { color: colors.textSecondary }]}>No children added yet</Text>
              <Button 
                title="Add Your First Child" 
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
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.surface }]}>
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.text }]}>
                {editingChild ? 'Edit Child' : 'Add New Child'}
              </Text>
              <TouchableOpacity onPress={() => setModalVisible(false)}>
                <Text style={{ color: colors.primary, fontWeight: '600' }}>Cancel</Text>
              </TouchableOpacity>
            </View>

            <ScrollView showsVerticalScrollIndicator={false}>
              <Controller
                control={control}
                name="fullName"
                render={({ field: { onChange, onBlur, value } }) => (
                  <Input
                    label="Full Name"
                    placeholder="Enter child's full name"
                    onBlur={onBlur}
                    onChangeText={onChange}
                    value={value}
                    error={errors.fullName?.message}
                  />
                )}
              />

              <Controller
                control={control}
                name="dateOfBirth"
                render={({ field: { onChange, onBlur, value } }) => (
                  <Input
                    label="Date of Birth (YYYY-MM-DD)"
                    placeholder="2020-05-20"
                    onBlur={onBlur}
                    onChangeText={onChange}
                    value={value}
                    error={errors.dateOfBirth?.message}
                  />
                )}
              />

              <View style={styles.fieldGroup}>
                <Text style={[styles.fieldLabel, { color: colors.textSecondary }]}>Gender</Text>
                <Controller
                  control={control}
                  name="gender"
                  render={({ field: { onChange, value } }) => (
                    <View style={styles.genderContainer}>
                      {[0, 1, 2].map((g) => (
                        <TouchableOpacity
                          key={g}
                          onPress={() => onChange(g)}
                          style={[
                            styles.genderOption,
                            { 
                              borderColor: value === g ? colors.primary : colors.border,
                              backgroundColor: value === g ? colors.primary + '10' : 'transparent'
                            }
                          ]}
                        >
                          <Text style={[
                            styles.genderText, 
                            { color: value === g ? colors.primary : colors.textSecondary }
                          ]}>
                            {g === 0 ? 'Male' : (g === 1 ? 'Female' : 'Other')}
                          </Text>
                        </TouchableOpacity>
                      ))}
                    </View>
                  )}
                />
              </View>

              <Button 
                title={editingChild ? "Update Child" : "Add Child"} 
                onPress={handleSubmit(onSubmit)} 
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
    padding: 24,
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
  childCard: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 24,
    marginBottom: 16,
    overflow: 'hidden',
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
  childTappable: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
  },
  avatarContainer: {
    width: 52,
    height: 52,
    borderRadius: 18,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 16,
  },
  childInfo: {
    flex: 1,
  },
  childName: {
    fontSize: 17,
    fontWeight: '700',
    marginBottom: 4,
  },
  childMeta: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  childMetaText: {
    fontSize: 13,
    fontWeight: '500',
  },
  metaDivider: {
    width: 4,
    height: 4,
    borderRadius: 2,
    backgroundColor: '#CCCCCC',
    marginHorizontal: 4,
  },
  actionButtons: {
    flexDirection: 'row',
    gap: 8,
    paddingRight: 12,
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
  fieldGroup: {
    marginBottom: 16,
  },
  fieldLabel: {
    fontSize: 14,
    fontWeight: '500',
    marginBottom: 8,
    marginLeft: 4,
  },
  genderContainer: {
    flexDirection: 'row',
    gap: 12,
  },
  genderOption: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 12,
    borderWidth: 1,
    alignItems: 'center',
  },
  genderText: {
    fontSize: 14,
    fontWeight: '600',
  },
});

