import { api } from '@/api/api';
import { Button } from '@/components/ui/Button';
import { Input } from '@/components/ui/Input';
import { Colors } from '@/constants/theme';
import { useColorScheme } from '@/hooks/use-color-scheme';
import { saveAuthData, useAuthStore } from '@/store/authStore';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link, useRouter } from 'expo-router';
import React, { useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import {
  Alert,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  StyleSheet,
  Text,
  TouchableOpacity,
  View
} from 'react-native';
import Animated, { FadeInDown, FadeInUp } from 'react-native-reanimated';
import * as z from 'zod';

const loginSchema = z.object({
  identifier: z.string().min(1, 'Email or Phone Number is required'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
});

type LoginForm = z.infer<typeof loginSchema>;

export default function LoginScreen() {
  const router = useRouter();
  const colorScheme = useColorScheme() ?? 'light';
  const colors = Colors[colorScheme];
  const [loading, setLoading] = useState(false);
  const setUser = useAuthStore((state) => state.setUser);
  const setToken = useAuthStore((state) => state.setToken);

  const { control, handleSubmit, formState: { errors } } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
  });

  const onLogin = async (data: LoginForm) => {
    setLoading(true);
    try {
      const response = await api.post('/auth/login', data);
      const { accessToken, user } = response.data;

      await saveAuthData(accessToken, user);
      setToken(accessToken);
      setUser(user);

      router.replace('/(tabs)');
    } catch (error: any) {
      const message = error.response?.data?.message || 'Invalid credentials. Please try again.';
      Alert.alert('Login Failed', message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={[styles.container, { backgroundColor: colors.background }]}
    >
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <Animated.View entering={FadeInDown.delay(200).duration(1000)} style={styles.header}>
          <Text style={[styles.title, { color: colors.text }]}>Welcome to MorboAiLens</Text>
          <Text style={[styles.subtitle, { color: colors.textSecondary }]}>
            Sign in to your account to continue
          </Text>
        </Animated.View>

        <Animated.View entering={FadeInUp.delay(400).duration(1000)} style={styles.form}>
          <Controller
            control={control}
            name="identifier"
            render={({ field: { onChange, onBlur, value } }) => (
              <Input
                label="Email or Phone Number"
                placeholder="Enter email or phone"
                onBlur={onBlur}
                onChangeText={onChange}
                value={value}
                error={errors.identifier?.message}
                autoCapitalize="none"
              />
            )}
          />

          <Controller
            control={control}
            name="password"
            render={({ field: { onChange, onBlur, value } }) => (
              <Input
                label="Password"
                placeholder="••••••••"
                onBlur={onBlur}
                onChangeText={onChange}
                value={value}
                error={errors.password?.message}
                isPassword
              />
            )}
          />

          <Button
            title="Login"
            onPress={handleSubmit(onLogin)}
            loading={loading}
            style={styles.loginButton}
          />

          <TouchableOpacity
            onPress={() => router.push('/(auth)/forgot-password')}
            style={styles.forgotPassword}
          >
            <Text style={[styles.forgotPasswordText, { color: colors.primary }]}>
              Forgot Password?
            </Text>
          </TouchableOpacity>

          <View style={styles.footer}>
            <Text style={{ color: colors.textSecondary }}>Don&apos;t have an account? </Text>
            <Link href="/(auth)/register" asChild>
              <TouchableOpacity>
                <Text style={{ color: colors.primary, fontWeight: 'bold' }}>Sign Up</Text>
              </TouchableOpacity>
            </Link>
          </View>
        </Animated.View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  scrollContent: {
    flexGrow: 1,
    padding: 24,
    justifyContent: 'center',
  },
  header: {
    marginBottom: 40,
  },
  title: {
    fontSize: 32,
    fontWeight: '800',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
  },
  form: {
    width: '100%',
  },
  loginButton: {
    marginTop: 16,
  },
  forgotPassword: {
    marginTop: 16,
    alignItems: 'center',
  },
  forgotPasswordText: {
    fontSize: 14,
    fontWeight: '600',
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'center',
    marginTop: 24,
  },
});
