import React, { useState, useCallback } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ScrollView,
  Alert,
  ActivityIndicator,
  Image,
  Modal,
  FlatList,
  Platform,
  useColorScheme,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useFocusEffect } from 'expo-router';
import * as ImagePicker from 'expo-image-picker';
import * as Location from 'expo-location';
import { useAuthStore } from '@/store/authStore';
import { Colors } from '@/constants/theme';
import { api } from '@/api/api';
import { uploadScan } from '@/api/scans';
import { Child } from '@/types/child';
import { Button } from '@/components/ui/Button';
import { Camera, ImageIcon, ChevronDown, CheckCircle, Baby, MapPin } from 'lucide-react-native';

const SCAN_STATUS_LABELS = ['Pending', 'AI Confirmed', 'Officer Verified', 'Cleared'] as const;

export default function ScanScreen() {
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const { user } = useAuthStore();

  const [children, setChildren] = useState<Child[]>([]);
  const [selectedChild, setSelectedChild] = useState<Child | null>(null);
  const [imageUri, setImageUri] = useState<string | null>(null);
  const [childPickerVisible, setChildPickerVisible] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [successScanId, setSuccessScanId] = useState<string | null>(null);

  const fetchChildren = useCallback(async () => {
    if (!user) return;
    try {
      const response = await api.get(`/children/parent/${user.id}`);
      setChildren(Array.isArray(response.data) ? response.data : []);
    } catch {
      // non-critical — user sees empty picker
    }
  }, [user]);

  useFocusEffect(
    useCallback(() => {
      fetchChildren();
    }, [fetchChildren])
  );

  async function pickFromGallery() {
    const { status } = await ImagePicker.requestMediaLibraryPermissionsAsync();
    if (status !== 'granted') {
      Alert.alert('Permission required', 'Allow photo library access to pick a scan image.');
      return;
    }
    const result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ['images'],
      allowsEditing: true,
      quality: 0.85,
    });
    if (!result.canceled) setImageUri(result.assets[0].uri);
  }

  async function pickFromCamera() {
    const { status } = await ImagePicker.requestCameraPermissionsAsync();
    if (status !== 'granted') {
      Alert.alert('Permission required', 'Allow camera access to take a scan photo.');
      return;
    }
    const result = await ImagePicker.launchCameraAsync({
      allowsEditing: true,
      quality: 0.85,
    });
    if (!result.canceled) setImageUri(result.assets[0].uri);
  }

  async function handleSubmit() {
    if (!selectedChild) {
      Alert.alert('Select a child', 'Please choose which child this scan is for.');
      return;
    }
    if (!imageUri) {
      Alert.alert('No image', 'Please take or pick a photo before uploading.');
      return;
    }

    setUploading(true);
    let latitude: number | undefined;
    let longitude: number | undefined;

    try {
      const { status } = await Location.requestForegroundPermissionsAsync();
      if (status === 'granted') {
        const loc = await Location.getCurrentPositionAsync({ accuracy: Location.Accuracy.Balanced });
        latitude = loc.coords.latitude;
        longitude = loc.coords.longitude;
      }
    } catch {
      // location is optional — continue without it
    }

    try {
      const result = await uploadScan(selectedChild.id, imageUri, latitude, longitude);
      setSuccessScanId(result.scan.id);
      setImageUri(null);
      setSelectedChild(null);
    } catch (error: any) {
      const message = error?.message || 'Upload failed. Please try again.';
      if (message.includes('401')) {
        Alert.alert('Session expired', 'Please log out and log in again.');
      } else if (message.includes('404')) {
        Alert.alert('Not found', 'Child not found on the server.');
      } else {
        Alert.alert('Upload failed', message);
      }
    } finally {
      setUploading(false);
    }
  }

  function resetForm() {
    setSuccessScanId(null);
    setImageUri(null);
    setSelectedChild(null);
  }

  if (!user) return null;

  if (successScanId) {
    return (
      <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]} edges={['top']}>
        <View style={styles.successContainer}>
          <View style={[styles.successIcon, { backgroundColor: '#E8F5E9' }]}>
            <CheckCircle size={56} color="#43A047" />
          </View>
          <Text style={[styles.successTitle, { color: colors.text }]}>Scan Uploaded</Text>
          <Text style={[styles.successSubtitle, { color: colors.textSecondary }]}>
            Your scan has been submitted and is pending AI analysis.
          </Text>
          <View style={[styles.scanIdBox, { backgroundColor: colors.surface }]}>
            <Text style={[styles.scanIdLabel, { color: colors.textSecondary }]}>Scan ID</Text>
            <Text style={[styles.scanIdValue, { color: colors.text }]} numberOfLines={1}>{successScanId}</Text>
          </View>
          <Button title="New Scan" onPress={resetForm} style={{ marginTop: 32, width: '100%' }} />
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]} edges={['top']}>
      <ScrollView contentContainerStyle={styles.scrollContent} showsVerticalScrollIndicator={false}>
        <View style={styles.header}>
          <Text style={[styles.title, { color: colors.text }]}>New Scan</Text>
          <Text style={[styles.subtitle, { color: colors.textSecondary }]}>
            Upload a measles scan photo for AI analysis
          </Text>
        </View>

        {/* Child Selector */}
        <View style={styles.section}>
          <Text style={[styles.sectionLabel, { color: colors.textSecondary }]}>Select Child</Text>
          <TouchableOpacity
            style={[styles.childSelector, { backgroundColor: colors.surface, borderColor: colors.border }]}
            onPress={() => setChildPickerVisible(true)}
          >
            {selectedChild ? (
              <View style={styles.selectedChildRow}>
                <View style={[styles.miniAvatar, { backgroundColor: '#E3F2FD' }]}>
                  <Baby size={16} color="#1E88E5" />
                </View>
                <Text style={[styles.selectedChildName, { color: colors.text }]}>{selectedChild.fullName}</Text>
              </View>
            ) : (
              <Text style={[styles.placeholderText, { color: colors.textSecondary }]}>Choose a child...</Text>
            )}
            <ChevronDown size={20} color={colors.textSecondary} />
          </TouchableOpacity>
        </View>

        {/* Image Picker */}
        <View style={styles.section}>
          <Text style={[styles.sectionLabel, { color: colors.textSecondary }]}>Scan Photo</Text>
          {imageUri ? (
            <View style={styles.previewContainer}>
              <Image source={{ uri: imageUri }} style={styles.imagePreview} resizeMode="cover" />
              <TouchableOpacity
                style={[styles.changePhotoButton, { backgroundColor: colors.surface }]}
                onPress={pickFromGallery}
              >
                <Text style={[styles.changePhotoText, { color: colors.primary }]}>Change Photo</Text>
              </TouchableOpacity>
            </View>
          ) : (
            <View style={styles.pickerRow}>
              <TouchableOpacity
                style={[styles.pickerButton, { backgroundColor: colors.surface, borderColor: colors.border }]}
                onPress={pickFromCamera}
              >
                <Camera size={28} color={colors.primary} />
                <Text style={[styles.pickerButtonLabel, { color: colors.text }]}>Camera</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.pickerButton, { backgroundColor: colors.surface, borderColor: colors.border }]}
                onPress={pickFromGallery}
              >
                <ImageIcon size={28} color={colors.primary} />
                <Text style={[styles.pickerButtonLabel, { color: colors.text }]}>Gallery</Text>
              </TouchableOpacity>
            </View>
          )}
        </View>

        {/* Location note */}
        <View style={[styles.locationNote, { backgroundColor: colors.surface }]}>
          <MapPin size={16} color={colors.textSecondary} />
          <Text style={[styles.locationNoteText, { color: colors.textSecondary }]}>
            Current location will be attached automatically if permission is granted.
          </Text>
        </View>

        {/* Submit */}
        <Button
          title={uploading ? 'Uploading...' : 'Submit Scan'}
          onPress={handleSubmit}
          loading={uploading}
          style={{ marginTop: 32 }}
        />
      </ScrollView>

      {/* Child Picker Modal */}
      <Modal
        animationType="none"
        transparent
        visible={childPickerVisible}
        onRequestClose={() => setChildPickerVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.surface }]}>
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.text }]}>Select Child</Text>
              <TouchableOpacity onPress={() => setChildPickerVisible(false)}>
                <Text style={{ color: colors.primary, fontWeight: '600' }}>Cancel</Text>
              </TouchableOpacity>
            </View>

            {children.length === 0 ? (
              <View style={styles.emptyChildren}>
                <Baby size={40} color={colors.border} />
                <Text style={[styles.emptyChildrenText, { color: colors.textSecondary }]}>
                  No children added yet. Add children in the Children tab.
                </Text>
              </View>
            ) : (
              <FlatList
                data={children}
                keyExtractor={(item) => item.id}
                renderItem={({ item }) => (
                  <TouchableOpacity
                    style={[
                      styles.childOption,
                      selectedChild?.id === item.id && { backgroundColor: colors.primary + '15' },
                    ]}
                    onPress={() => {
                      setSelectedChild(item);
                      setChildPickerVisible(false);
                    }}
                  >
                    <View style={[
                      styles.miniAvatar,
                      { backgroundColor: item.gender === 0 ? '#E3F2FD' : (item.gender === 1 ? '#FCE4EC' : '#F5F5F5') }
                    ]}>
                      <Baby size={16} color={item.gender === 0 ? '#1E88E5' : (item.gender === 1 ? '#D81B60' : '#757575')} />
                    </View>
                    <Text style={[styles.childOptionName, { color: colors.text }]}>{item.fullName}</Text>
                    {selectedChild?.id === item.id && (
                      <CheckCircle size={18} color={colors.primary} />
                    )}
                  </TouchableOpacity>
                )}
              />
            )}
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: {
    padding: 24,
    paddingBottom: 48,
  },
  header: {
    marginBottom: 32,
  },
  title: {
    fontSize: 28,
    fontWeight: '800',
    letterSpacing: -0.5,
  },
  subtitle: {
    fontSize: 15,
    fontWeight: '500',
    marginTop: 4,
  },
  section: {
    marginBottom: 24,
  },
  sectionLabel: {
    fontSize: 14,
    fontWeight: '600',
    marginBottom: 10,
    marginLeft: 4,
  },
  childSelector: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 16,
    borderRadius: 20,
    borderWidth: 1,
    ...Platform.select({
      ios: { shadowColor: '#000', shadowOffset: { width: 0, height: 2 }, shadowOpacity: 0.04, shadowRadius: 8 },
      android: { elevation: 1 },
    }),
  },
  selectedChildRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
  },
  miniAvatar: {
    width: 32,
    height: 32,
    borderRadius: 10,
    justifyContent: 'center',
    alignItems: 'center',
  },
  selectedChildName: {
    fontSize: 16,
    fontWeight: '600',
  },
  placeholderText: {
    fontSize: 15,
    fontWeight: '500',
  },
  pickerRow: {
    flexDirection: 'row',
    gap: 16,
  },
  pickerButton: {
    flex: 1,
    paddingVertical: 28,
    borderRadius: 20,
    borderWidth: 1,
    alignItems: 'center',
    gap: 10,
    ...Platform.select({
      ios: { shadowColor: '#000', shadowOffset: { width: 0, height: 2 }, shadowOpacity: 0.04, shadowRadius: 8 },
      android: { elevation: 1 },
    }),
  },
  pickerButtonLabel: {
    fontSize: 14,
    fontWeight: '700',
  },
  previewContainer: {
    alignItems: 'center',
    gap: 12,
  },
  imagePreview: {
    width: '100%',
    height: 260,
    borderRadius: 20,
    backgroundColor: '#F0F0F0',
  },
  changePhotoButton: {
    paddingVertical: 10,
    paddingHorizontal: 24,
    borderRadius: 12,
  },
  changePhotoText: {
    fontSize: 14,
    fontWeight: '600',
  },
  locationNote: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    gap: 10,
    padding: 14,
    borderRadius: 16,
  },
  locationNoteText: {
    flex: 1,
    fontSize: 13,
    fontWeight: '500',
    lineHeight: 20,
  },
  successContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 32,
  },
  successIcon: {
    width: 100,
    height: 100,
    borderRadius: 32,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 24,
  },
  successTitle: {
    fontSize: 26,
    fontWeight: '800',
    letterSpacing: -0.5,
    marginBottom: 8,
  },
  successSubtitle: {
    fontSize: 15,
    fontWeight: '500',
    textAlign: 'center',
    lineHeight: 22,
    marginBottom: 24,
  },
  scanIdBox: {
    width: '100%',
    padding: 16,
    borderRadius: 16,
    gap: 4,
  },
  scanIdLabel: {
    fontSize: 12,
    fontWeight: '600',
    textTransform: 'uppercase',
    letterSpacing: 0.5,
  },
  scanIdValue: {
    fontSize: 13,
    fontWeight: '600',
    fontVariant: ['tabular-nums'],
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
    maxHeight: '70%',
  },
  modalHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 20,
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: '800',
  },
  childOption: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    padding: 14,
    borderRadius: 16,
    marginBottom: 8,
  },
  childOptionName: {
    flex: 1,
    fontSize: 16,
    fontWeight: '600',
  },
  emptyChildren: {
    alignItems: 'center',
    paddingVertical: 32,
    gap: 12,
  },
  emptyChildrenText: {
    fontSize: 14,
    fontWeight: '500',
    textAlign: 'center',
    lineHeight: 20,
  },
});
