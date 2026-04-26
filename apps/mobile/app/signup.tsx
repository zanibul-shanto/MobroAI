import React, { useState } from 'react';
import {
  StyleSheet,
  View,
  Text,
  TextInput,
  TouchableOpacity,
  ScrollView,
  KeyboardAvoidingView,
  Platform,
} from 'react-native';
import { Stack, useRouter } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { useLanguage } from '@/context/LanguageContext';

export default function SignupScreen() {
  const router = useRouter();
  const { t } = useLanguage();
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    location: '',
    mobileNo: '',
    nid: '',
  });

  const [showPassword, setShowPassword] = useState(false);

  const handleSignup = () => {
    console.log('Signup Data:', formData);
  };

  const updateField = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <View style={styles.container}>
      <Stack.Screen options={{ 
        headerShown: true, 
        title: t('signup'),
        headerShadowVisible: false,
        headerStyle: { backgroundColor: '#fff' },
        headerTitleStyle: { fontWeight: '600' }
      }} />
      <KeyboardAvoidingView
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.keyboardView}
      >
        <ScrollView
          showsVerticalScrollIndicator={false}
          contentContainerStyle={styles.scrollContent}
        >
          <View style={styles.header}>
            <Text style={styles.title}>{t('createAccount')}</Text>
            <Text style={styles.subtitle}>{t('registerInfo')}</Text>
          </View>

          <View style={styles.form}>
            <View style={styles.nameRow}>
              <View style={[styles.inputGroup, { flex: 1, marginRight: 10 }]}>
                <Text style={styles.label}>{t('firstName')}</Text>
                <TextInput
                  placeholder={t('firstNamePlaceholder')}
                  style={styles.input}
                  value={formData.firstName}
                  onChangeText={(v) => updateField('firstName', v)}
                />
              </View>
              <View style={[styles.inputGroup, { flex: 1 }]}>
                <Text style={styles.label}>{t('lastName')}</Text>
                <TextInput
                  placeholder={t('lastNamePlaceholder')}
                  style={styles.input}
                  value={formData.lastName}
                  onChangeText={(v) => updateField('lastName', v)}
                />
              </View>
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.label}>{t('email')}</Text>
              <TextInput
                placeholder="example@mail.com"
                keyboardType="email-address"
                autoCapitalize="none"
                style={styles.input}
                value={formData.email}
                onChangeText={(v) => updateField('email', v)}
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.label}>{t('password')}</Text>
              <View style={styles.passwordContainer}>
                <TextInput
                  placeholder="••••••••"
                  secureTextEntry={!showPassword}
                  style={[styles.input, { borderBottomWidth: 0, marginBottom: 0, flex: 1 }]}
                  value={formData.password}
                  onChangeText={(v) => updateField('password', v)}
                />
                <TouchableOpacity onPress={() => setShowPassword(!showPassword)}>
                  <Ionicons
                    name={showPassword ? 'eye-off-outline' : 'eye-outline'}
                    size={20}
                    color="#666"
                  />
                </TouchableOpacity>
              </View>
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.label}>{t('location')}</Text>
              <TextInput
                placeholder={t('locationPlaceholder')}
                style={styles.input}
                value={formData.location}
                onChangeText={(v) => updateField('location', v)}
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.label}>{t('mobile')}</Text>
              <TextInput
                placeholder={t('mobilePlaceholder')}
                keyboardType="numeric"
                style={styles.input}
                value={formData.mobileNo}
                onChangeText={(v) => updateField('mobileNo', v)}
              />
            </View>

            <View style={styles.inputGroup}>
              <Text style={styles.label}>{t('nid')}</Text>
              <TextInput
                placeholder={t('nidPlaceholder')}
                keyboardType="numeric"
                style={styles.input}
                value={formData.nid}
                onChangeText={(v) => updateField('nid', v)}
              />
            </View>

            <TouchableOpacity style={styles.signupButton} onPress={handleSignup}>
              <Text style={styles.signupText}>{t('register')}</Text>
            </TouchableOpacity>

            <View style={styles.footer}>
              <Text style={styles.footerText}>{t('alreadyHaveAccount')}</Text>
              <TouchableOpacity onPress={() => router.push('/')}>
                <Text style={styles.loginLink}>{t('login')}</Text>
              </TouchableOpacity>
            </View>
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  keyboardView: {
    flex: 1,
  },
  scrollContent: {
    paddingHorizontal: 25,
    paddingTop: 20,
    paddingBottom: 40,
  },
  header: {
    marginBottom: 30,
  },
  title: {
    fontSize: 28,
    fontWeight: 'bold',
    color: '#333',
  },
  subtitle: {
    fontSize: 15,
    color: '#666',
    marginTop: 5,
  },
  form: {
    width: '100%',
  },
  nameRow: {
    flexDirection: 'row',
  },
  inputGroup: {
    marginBottom: 20,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: '#444',
    marginBottom: 8,
  },
  input: {
    height: 50,
    borderBottomWidth: 1,
    borderBottomColor: '#ddd',
    fontSize: 16,
    color: '#333',
  },
  passwordContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderBottomWidth: 1,
    borderBottomColor: '#ddd',
  },
  signupButton: {
    backgroundColor: '#007AFF',
    height: 55,
    borderRadius: 8,
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 20,
  },
  signupText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: '600',
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'center',
    marginTop: 25,
  },
  footerText: {
    color: '#666',
    fontSize: 14,
  },
  loginLink: {
    color: '#007AFF',
    fontSize: 14,
    fontWeight: '600',
  },
});
