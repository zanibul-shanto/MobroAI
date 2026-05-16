import React from 'react';
import { 
  View, 
  Text, 
  StyleSheet, 
  ScrollView, 
  TouchableOpacity, 
  SafeAreaView,
  Platform
} from 'react-native';
import { useRouter } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { useAuthStore } from '@/store/authStore';
import { Colors } from '@/constants/theme';
import { useColorScheme } from '@/hooks/use-color-scheme';

export default function DashboardScreen() {
  const router = useRouter();
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const { user, logout } = useAuthStore();

  return (
    <SafeAreaView style={[styles.container, { backgroundColor: colors.background }]}>
      <ScrollView contentContainerStyle={styles.scrollContent} showsVerticalScrollIndicator={false}>
        <View style={styles.header}>
          <View>
            <Text style={[styles.greeting, { color: colors.textSecondary }]}>Welcome to MorboAiLens</Text>
            <Text style={[styles.userName, { color: colors.text }]}>{user?.fullName || 'User'}</Text>
          </View>
          <TouchableOpacity
            onPress={logout}
            style={[styles.profileButton, { backgroundColor: colors.primary + '10' }]}
          >
            <Ionicons name="log-out-outline" size={24} color={colors.primary} />
          </TouchableOpacity>
        </View>

        <View style={styles.statsContainer}>
          <View
            style={[styles.statCard, { backgroundColor: '#4facfe' }]}
          >
            <Ionicons name="analytics" size={32} color="#FFFFFF" />
            <Text style={styles.statValue}>12</Text>
            <Text style={styles.statLabel}>Scan Reports</Text>
          </View>
          <View
            style={[styles.statCard, { backgroundColor: '#fa709a' }]}
          >
            <Ionicons name="notifications" size={32} color="#FFFFFF" />
            <Text style={styles.statValue}>3</Text>
            <Text style={styles.statLabel}>Alerts</Text>
          </View>
        </View>

        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.text }]}>Quick Actions</Text>
          <View style={styles.actionGrid}>
            <ActionItem
              icon="scan"
              label="New Scan"
              color="#1E88E5"
              bgColor="#E3F2FD"
              colors={colors}
              onPress={() => router.push('/(tabs)/scan' as any)}
            />
            <ActionItem 
              icon="medical" 
              label="Health Tips" 
              color="#8E24AA" 
              bgColor="#F3E5F5" 
              colors={colors} 
            />
            <ActionItem 
              icon="calendar" 
              label="Schedule" 
              color="#43A047" 
              bgColor="#E8F5E9" 
              colors={colors} 
            />
            <ActionItem 
              icon="settings" 
              label="Settings" 
              color="#FB8C00" 
              bgColor="#FFF3E0" 
              onPress={() => router.push('/(tabs)/profile')}
              colors={colors} 
            />
          </View>
        </View>

        <View style={styles.section}>
          <Text style={[styles.sectionTitle, { color: colors.text }]}>Recent Activity</Text>
          {[1, 2, 3].map((i) => (
            <TouchableOpacity key={i} style={[styles.activityItem, { backgroundColor: colors.surface }]}>
              <View style={[styles.activityIcon, { backgroundColor: '#4CAF5015' }]}>
                <Ionicons name="checkmark-circle" size={20} color="#4CAF50" />
              </View>
              <View style={styles.activityInfo}>
                <Text style={[styles.activityTitle, { color: colors.text }]}>Scan Completed</Text>
                <Text style={[styles.activityTime, { color: colors.textSecondary }]}>{i * 2} hours ago</Text>
              </View>
              <Ionicons name="chevron-forward" size={18} color={colors.textSecondary} />
            </TouchableOpacity>
          ))}
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

function ActionItem({ icon, label, color, bgColor, onPress, colors }: any) {
  return (
    <TouchableOpacity 
      style={[styles.actionItem, { backgroundColor: colors.surface }]} 
      onPress={onPress}
    >
      <View style={[styles.iconBox, { backgroundColor: bgColor }]}>
        <Ionicons name={icon} size={24} color={color} />
      </View>
      <Text style={[styles.actionText, { color: colors.text }]}>{label}</Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  scrollContent: {
    padding: 24,
    paddingTop: Platform.OS === 'android' ? 40 : 10,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 32,
  },
  greeting: {
    fontSize: 16,
    fontWeight: '500',
  },
  userName: {
    fontSize: 28,
    fontWeight: '800',
    letterSpacing: -0.5,
  },
  profileButton: {
    width: 48,
    height: 48,
    borderRadius: 24,
    justifyContent: 'center',
    alignItems: 'center',
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
    ...Platform.select({
      ios: {
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 8 },
        shadowOpacity: 0.15,
        shadowRadius: 12,
      },
      android: {
        elevation: 6,
      },
    }),
  },
  statValue: {
    fontSize: 26,
    fontWeight: '800',
    marginTop: 12,
    color: '#FFFFFF',
  },
  statLabel: {
    fontSize: 13,
    marginTop: 4,
    color: '#FFFFFFCC',
    fontWeight: '600',
  },
  section: {
    marginBottom: 32,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: '800',
    marginBottom: 16,
    letterSpacing: -0.5,
  },
  actionGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 16,
  },
  actionItem: {
    width: '47%',
    padding: 16,
    borderRadius: 24,
    alignItems: 'center',
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
  iconBox: {
    padding: 12,
    borderRadius: 18,
    marginBottom: 12,
  },
  actionText: {
    fontSize: 14,
    fontWeight: '700',
  },
  activityItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 16,
    borderRadius: 20,
    marginBottom: 12,
    ...Platform.select({
      ios: {
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.03,
        shadowRadius: 8,
      },
      android: {
        elevation: 1,
      },
    }),
  },
  activityIcon: {
    width: 40,
    height: 40,
    borderRadius: 12,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 16,
  },
  activityInfo: {
    flex: 1,
  },
  activityTitle: {
    fontSize: 15,
    fontWeight: '700',
  },
  activityTime: {
    fontSize: 13,
    marginTop: 2,
  },
});

