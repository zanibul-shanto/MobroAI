import React, { useState } from 'react';
import { 
  View, 
  Text, 
  StyleSheet, 
  ScrollView, 
  Alert, 
  Modal,
  TouchableOpacity,
  SafeAreaView,
  Platform
} from 'react-native';
import { useRouter } from 'expo-router';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useAuthStore } from '@/store/authStore';
import { Colors } from '@/constants/theme';
import { useColorScheme } from '@/hooks/use-color-scheme';
import { Button } from '@/components/ui/Button';
import { Input } from '@/components/ui/Input';
import { api } from '@/api/api';
import { User as UserIcon, LogOut, Lock, Mail, Phone, Shield, Edit3, ChevronRight } from 'lucide-react-native';

const changePasswordSchema = z.object({
  newPassword: z.string().min(6, 'New password must be at least 6 characters'),
  confirmPassword: z.string().min(6, 'Confirm password must be at least 6 characters'),
}).refine((data) => data.newPassword === data.confirmPassword, {
  message: "Passwords don't match",
  path: ["confirmPassword"],
});

const editProfileSchema = z.object({
  fullName: z.string().min(2, 'Name is too short'),
  email: z.string().email('Invalid email address'),
  phoneNumber: z.string().min(10, 'Invalid phone number'),
  role: z.number(),
});

type ChangePasswordForm = z.infer<typeof changePasswordSchema>;
type EditProfileForm = z.infer<typeof editProfileSchema>;

export default function ProfileScreen() {
  const router = useRouter();
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const { user, logout, updateUser } = useAuthStore();
  const [passwordModalVisible, setPasswordModalVisible] = useState(false);
  const [profileModalVisible, setProfileModalVisible] = useState(false);
  const [loading, setLoading] = useState(false);

  const { 
    control: passwordControl, 
    handleSubmit: handlePasswordSubmit, 
    reset: resetPassword, 
    formState: { errors: passwordErrors } 
  } = useForm<ChangePasswordForm>({
    resolver: zodResolver(changePasswordSchema),
  });

  const { 
    control: profileControl, 
    handleSubmit: handleProfileSubmit, 
    formState: { errors: profileErrors } 
  } = useForm<EditProfileForm>({
    resolver: zodResolver(editProfileSchema),
    defaultValues: {
      fullName: user?.fullName || '',
      email: user?.email || '',
      phoneNumber: user?.phoneNumber || '',
      role: user?.role || 1,
    }
  });

  const onChangePassword = async (data: ChangePasswordForm) => {
    setLoading(true);
    try {
      await api.post('/users/change-password', {
        newPassword: data.newPassword,
        confirmPassword: data.confirmPassword
      });
      Alert.alert('Success', 'Password changed successfully');
      setPasswordModalVisible(false);
      resetPassword();
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to change password';
      Alert.alert('Error', message);
    } finally {
      setLoading(false);
    }
  };

  const onUpdateProfile = async (data: EditProfileForm) => {
    setLoading(true);
    try {
      await api.put(`/users/${user?.id}`, data);
      const updatedUser = { ...user, ...data } as any;
      await updateUser(updatedUser);
      Alert.alert('Success', 'Profile updated successfully');
      setProfileModalVisible(false);
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to update profile';
      Alert.alert('Error', message);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    Alert.alert(
      'Logout',
      'Are you sure you want to logout?',
      [
        { text: 'Cancel', style: 'cancel' },
        { 
          text: 'Logout', 
          style: 'destructive', 
          onPress: async () => {
            await logout();
            router.replace('/(auth)/login');
          } 
        }
      ]
    );
  };

  const getRoleLabel = (role: number) => {
    switch (role) {
      case 0: return 'Admin';
      case 1: return 'Health Care Officer';
      case 2: return 'Parent';
      default: return 'User';
    }
  };

  if (!user) return null;

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]}>
      <ScrollView contentContainerStyle={styles.scrollContent} showsVerticalScrollIndicator={false}>
        <View style={styles.header}>
          <View
            style={[styles.avatarGradient, { backgroundColor: colors.primary }]}
          >
            <UserIcon size={48} color="#FFFFFF" />
          </View>
          <Text style={[styles.userName, { color: colors.text }]}>{user.fullName}</Text>
          <View style={[styles.roleBadge, { backgroundColor: colors.primary + '15' }]}>
            <Shield size={14} color={colors.primary} />
            <Text style={[styles.roleText, { color: colors.primary }]}>{getRoleLabel(user.role)}</Text>
          </View>
        </View>

        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.textSecondary }]}>Account Information</Text>
          <View style={[styles.card, { backgroundColor: colors.surface }]}>
            <InfoRow icon={<Mail size={20} color={colors.primary} />} label="Email" value={user.email} colors={colors} />
            <View style={[styles.divider, { backgroundColor: colors.border }]} />
            <InfoRow icon={<Phone size={20} color={colors.primary} />} label="Phone" value={user.phoneNumber} colors={colors} />
          </View>
        </View>

        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.textSecondary }]}>Settings</Text>
          <View style={[styles.card, { backgroundColor: colors.surface }]}>
            <ActionRow 
              icon={<Edit3 size={20} color={colors.primary} />} 
              label="Edit Profile" 
              onPress={() => setProfileModalVisible(true)} 
              colors={colors} 
            />
            <View style={[styles.divider, { backgroundColor: colors.border }]} />
            <ActionRow 
              icon={<Lock size={20} color={colors.primary} />} 
              label="Change Password" 
              onPress={() => setPasswordModalVisible(true)} 
              colors={colors} 
            />
          </View>
        </View>

        <View style={styles.footer}>
          <TouchableOpacity 
            style={[styles.logoutButton, { borderColor: colors.error + '40' }]} 
            onPress={handleLogout}
          >
            <LogOut size={20} color={colors.error} />
            <Text style={[styles.logoutText, { color: colors.error }]}>Log Out</Text>
          </TouchableOpacity>
          <Text style={[styles.versionText, { color: colors.textSecondary }]}>Version 1.0.0</Text>
        </View>
      </ScrollView>

      {/* Edit Profile Modal */}
      <Modal
        animationType="none"
        transparent={true}
        visible={profileModalVisible}
        onRequestClose={() => setProfileModalVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.surface }]}>
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.text }]}>Edit Profile</Text>
              <TouchableOpacity onPress={() => setProfileModalVisible(false)}>
                <Text style={{ color: colors.primary, fontWeight: '600' }}>Cancel</Text>
              </TouchableOpacity>
            </View>

            <ScrollView showsVerticalScrollIndicator={false}>
              <Controller
                control={profileControl}
                name="fullName"
                render={({ field: { onChange, onBlur, value } }) => (
                  <Input
                    label="Full Name"
                    placeholder="John Doe"
                    onBlur={onBlur}
                    onChangeText={onChange}
                    value={value}
                    error={profileErrors.fullName?.message}
                  />
                )}
              />

              <Controller
                control={profileControl}
                name="email"
                render={({ field: { onChange, onBlur, value } }) => (
                  <Input
                    label="Email Address"
                    placeholder="john@example.com"
                    onBlur={onBlur}
                    onChangeText={onChange}
                    value={value}
                    keyboardType="email-address"
                    autoCapitalize="none"
                    error={profileErrors.email?.message}
                  />
                )}
              />

              <Controller
                control={profileControl}
                name="phoneNumber"
                render={({ field: { onChange, onBlur, value } }) => (
                  <Input
                    label="Phone Number"
                    placeholder="01800000000"
                    onBlur={onBlur}
                    onChangeText={onChange}
                    value={value}
                    keyboardType="phone-pad"
                    error={profileErrors.phoneNumber?.message}
                  />
                )}
              />

              <Button 
                title="Save Changes" 
                onPress={handleProfileSubmit(onUpdateProfile)} 
                loading={loading}
                style={{ marginTop: 16 }}
              />
            </ScrollView>
          </View>
        </View>
      </Modal>

      {/* Change Password Modal */}
      <Modal
        animationType="none"
        transparent={true}
        visible={passwordModalVisible}
        onRequestClose={() => setPasswordModalVisible(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.surface }]}>
            <View style={styles.modalHeader}>
              <Text style={[styles.modalTitle, { color: colors.text }]}>Change Password</Text>
              <TouchableOpacity onPress={() => setPasswordModalVisible(false)}>
                <Text style={{ color: colors.primary, fontWeight: '600' }}>Cancel</Text>
              </TouchableOpacity>
            </View>

            <Controller
              control={passwordControl}
              name="newPassword"
              render={({ field: { onChange, onBlur, value } }) => (
                <Input
                  label="New Password"
                  placeholder="••••••••"
                  onBlur={onBlur}
                  onChangeText={onChange}
                  value={value}
                  error={passwordErrors.newPassword?.message}
                  isPassword
                />
              )}
            />

            <Controller
              control={passwordControl}
              name="confirmPassword"
              render={({ field: { onChange, onBlur, value } }) => (
                <Input
                  label="Confirm Password"
                  placeholder="••••••••"
                  onBlur={onBlur}
                  onChangeText={onChange}
                  value={value}
                  error={passwordErrors.confirmPassword?.message}
                  isPassword
                />
              )}
            />

            <Button 
              title="Update Password" 
              onPress={handlePasswordSubmit(onChangePassword)} 
              loading={loading}
              style={{ marginTop: 16 }}
            />
          </View>
        </View>
      </Modal>
    </SafeAreaView>
  );
}

function InfoRow({ icon, label, value, colors }: any) {
  return (
    <View style={styles.infoRow}>
      <View style={[styles.iconContainer, { backgroundColor: colors.primary + '10' }]}>
        {icon}
      </View>
      <View style={styles.infoTextContainer}>
        <Text style={[styles.infoLabel, { color: colors.textSecondary }]}>{label}</Text>
        <Text style={[styles.infoValue, { color: colors.text }]}>{value}</Text>
      </View>
    </View>
  );
}

function ActionRow({ icon, label, onPress, colors }: any) {
  return (
    <TouchableOpacity style={styles.actionRow} onPress={onPress}>
      <View style={[styles.iconContainer, { backgroundColor: colors.primary + '10' }]}>
        {icon}
      </View>
      <Text style={[styles.actionLabel, { color: colors.text }]}>{label}</Text>
      <ChevronRight size={20} color={colors.textSecondary} />
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  scrollContent: {
    padding: 20,
    paddingBottom: 40,
  },
  header: {
    alignItems: 'center',
    marginTop: 20,
    marginBottom: 32,
  },
  avatarGradient: {
    width: 100,
    height: 100,
    borderRadius: 50,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
    ...Platform.select({
      ios: {
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 4 },
        shadowOpacity: 0.2,
        shadowRadius: 8,
      },
      android: {
        elevation: 8,
      },
    }),
  },
  userName: {
    fontSize: 26,
    fontWeight: '800',
    marginBottom: 8,
    letterSpacing: -0.5,
  },
  roleBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 14,
    paddingVertical: 6,
    borderRadius: 20,
    gap: 6,
  },
  roleText: {
    fontSize: 13,
    fontWeight: '700',
    textTransform: 'uppercase',
    letterSpacing: 0.5,
  },
  section: {
    marginBottom: 24,
  },
  sectionTitle: {
    fontSize: 14,
    fontWeight: '700',
    marginBottom: 12,
    marginLeft: 4,
    textTransform: 'uppercase',
    letterSpacing: 1,
  },
  card: {
    borderRadius: 24,
    padding: 8,
    ...Platform.select({
      ios: {
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.05,
        shadowRadius: 10,
      },
      android: {
        elevation: 2,
      },
    }),
  },
  infoRow: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 12,
    gap: 16,
  },
  actionRow: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 12,
    gap: 16,
  },
  iconContainer: {
    width: 44,
    height: 44,
    borderRadius: 14,
    justifyContent: 'center',
    alignItems: 'center',
  },
  infoTextContainer: {
    flex: 1,
  },
  infoLabel: {
    fontSize: 12,
    fontWeight: '500',
    marginBottom: 2,
  },
  infoValue: {
    fontSize: 16,
    fontWeight: '600',
  },
  actionLabel: {
    flex: 1,
    fontSize: 16,
    fontWeight: '600',
  },
  divider: {
    height: 1,
    marginHorizontal: 12,
  },
  footer: {
    alignItems: 'center',
    marginTop: 20,
    gap: 16,
  },
  logoutButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    paddingVertical: 14,
    paddingHorizontal: 32,
    borderRadius: 16,
    borderWidth: 1.5,
    width: '100%',
  },
  logoutText: {
    fontSize: 16,
    fontWeight: '700',
  },
  versionText: {
    fontSize: 12,
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
    maxHeight: '80%',
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
    letterSpacing: -0.5,
  },
});

