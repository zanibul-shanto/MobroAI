import { Colors } from '@/constants/theme';
import { useColorScheme } from '@/hooks/use-color-scheme';
import { useAuthStore } from '@/store/authStore';
import { Ionicons } from '@expo/vector-icons';
import { useRouter } from 'expo-router';
import React from 'react';
import { SafeAreaView, ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import Animated, { FadeInDown, FadeInRight } from 'react-native-reanimated';

export default function DashboardScreen() {
  const router = useRouter();
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const user = useAuthStore((state) => state.user);
  const setUser = useAuthStore((state) => state.setUser);
  const setToken = useAuthStore((state) => state.setToken);
  const logout = useAuthStore((state) => state.logout);

  const handleLogout = async () => {
    await logout();
    router.replace('/(auth)/login');
  };

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View style={styles.header}>
          <Animated.View entering={FadeInDown.delay(200)}>
            <Text style={[styles.greeting, { color: colors.textSecondary }]}>Welcome to MorboAiLens</Text>
            <Text style={[styles.userName, { color: colors.text }]}>{user?.fullName || 'User'}</Text>
          </Animated.View>
          <TouchableOpacity onPress={handleLogout} style={styles.logoutButton}>
            <Ionicons name="log-out-outline" size={24} color={colors.primary} />
          </TouchableOpacity>
        </View>

        <Animated.View entering={FadeInDown.delay(400)} style={styles.statsContainer}>
          <View style={[styles.statCard, { backgroundColor: colors.card }]}>
            <Ionicons name="analytics" size={32} color={colors.primary} />
            <Text style={[styles.statValue, { color: colors.text }]}>12</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary }]}>Scan Reports</Text>
          </View>
          <View style={[styles.statCard, { backgroundColor: colors.card }]}>
            <Ionicons name="notifications" size={32} color="#FF9500" />
            <Text style={[styles.statValue, { color: colors.text }]}>3</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary }]}>Alerts</Text>
          </View>
        </Animated.View>

        <Animated.View entering={FadeInDown.delay(600)} style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.text }]}>Quick Actions</Text>
          <View style={styles.actionGrid}>
            <TouchableOpacity style={[styles.actionItem, { backgroundColor: colors.card }]}>
              <View style={[styles.iconBox, { backgroundColor: '#E3F2FD' }]}>
                <Ionicons name="scan" size={24} color="#1E88E5" />
              </View>
              <Text style={[styles.actionText, { color: colors.text }]}>New Scan</Text>
            </TouchableOpacity>
            <TouchableOpacity style={[styles.actionItem, { backgroundColor: colors.card }]}>
              <View style={[styles.iconBox, { backgroundColor: '#F3E5F5' }]}>
                <Ionicons name="medical" size={24} color="#8E24AA" />
              </View>
              <Text style={[styles.actionText, { color: colors.text }]}>Health Tips</Text>
            </TouchableOpacity>
            <TouchableOpacity style={[styles.actionItem, { backgroundColor: colors.card }]}>
              <View style={[styles.iconBox, { backgroundColor: '#E8F5E9' }]}>
                <Ionicons name="calendar" size={24} color="#43A047" />
              </View>
              <Text style={[styles.actionText, { color: colors.text }]}>Schedule</Text>
            </TouchableOpacity>
            <TouchableOpacity style={[styles.actionItem, { backgroundColor: colors.card }]}>
              <View style={[styles.iconBox, { backgroundColor: '#FFF3E0' }]}>
                <Ionicons name="settings" size={24} color="#FB8C00" />
              </View>
              <Text style={[styles.actionText, { color: colors.text }]}>Settings</Text>
            </TouchableOpacity>
          </View>
        </Animated.View>

        <Animated.View entering={FadeInRight.delay(800)} style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.text }]}>Recent Activity</Text>
          {[1, 2, 3].map((i) => (
            <View key={i} style={[styles.activityItem, { backgroundColor: colors.card }]}>
              <View style={styles.activityIcon}>
                <Ionicons name="checkmark-circle" size={20} color="#4CAF50" />
              </View>
              <View style={styles.activityInfo}>
                <Text style={[styles.activityTitle, { color: colors.text }]}>Scan Completed</Text>
                <Text style={[styles.activityTime, { color: colors.textSecondary }]}>2 hours ago</Text>
              </View>
              <Ionicons name="chevron-forward" size={18} color={colors.textSecondary} />
            </View>
          ))}
        </Animated.View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  scrollContent: {
    padding: 24,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 32,
    marginTop: 16,
  },
  greeting: {
    fontSize: 16,
    fontWeight: '500',
  },
  userName: {
    fontSize: 28,
    fontWeight: '800',
  },
  logoutButton: {
    padding: 8,
    borderRadius: 12,
    backgroundColor: '#F0F0F0',
  },
  statsContainer: {
    flexDirection: 'row',
    gap: 16,
    marginBottom: 32,
  },
  statCard: {
    flex: 1,
    padding: 20,
    borderRadius: 24,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.05,
    shadowRadius: 12,
    elevation: 2,
  },
  statValue: {
    fontSize: 24,
    fontWeight: '800',
    marginTop: 12,
  },
  statLabel: {
    fontSize: 14,
    marginTop: 4,
  },
  section: {
    marginBottom: 32,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: '700',
    marginBottom: 16,
  },
  actionGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 16,
  },
  actionItem: {
    width: '47%',
    padding: 16,
    borderRadius: 20,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.03,
    shadowRadius: 8,
    elevation: 1,
  },
  iconBox: {
    padding: 12,
    borderRadius: 16,
    marginBottom: 12,
  },
  actionText: {
    fontSize: 14,
    fontWeight: '600',
  },
  activityItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    borderRadius: 16,
    marginBottom: 12,
  },
  activityIcon: {
    marginRight: 12,
  },
  activityInfo: {
    flex: 1,
  },
  activityTitle: {
    fontSize: 15,
    fontWeight: '600',
  },
  activityTime: {
    fontSize: 13,
    marginTop: 2,
  },
});
