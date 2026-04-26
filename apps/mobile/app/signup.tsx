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
import { commonStyles } from '@/constants/styles';

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
    <View style={[commonStyles.container, { paddingHorizontal: 25 }]}>
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
            <Text style={commonStyles.title}>{t('createAccount')}</Text>
            <Text style={[commonStyles.subtitle, { marginTop: 5 }]}>{t('registerInfo')}</Text>
          </View>

          <View style={styles.form}>
            <View style={styles.nameRow}>
              <View style={[commonStyles.inputGroup, { flex: 1, marginRight: 10 }]}>
                <Text style={commonStyles.label}>{t('firstName')}</Text>
                <TextInput
                  placeholder={t('firstNamePlaceholder')}
                  style={commonStyles.input}
                  value={formData.firstName}
                  onChangeText={(v) => updateField('firstName', v)}
                />
              </View>
              <View style={[commonStyles.inputGroup, { flex: 1 }]}>
                <Text style={commonStyles.label}>{t('lastName')}</Text>
                <TextInput
                  placeholder={t('lastNamePlaceholder')}
                  style={commonStyles.input}
                  value={formData.lastName}
                  onChangeText={(v) => updateField('lastName', v)}
                />
              </View>
            </View>

            <View style={commonStyles.inputGroup}>
              <Text style={commonStyles.label}>{t('email')}</Text>
              <TextInput
                placeholder="example@mail.com"
                keyboardType="email-address"
                autoCapitalize="none"
                style={commonStyles.input}
                value={formData.email}
                onChangeText={(v) => updateField('email', v)}
              />
            </View>

            <View style={commonStyles.inputGroup}>
              <Text style={commonStyles.label}>{t('password')}</Text>
              <View style={styles.passwordContainer}>
                <TextInput
                  placeholder="••••••••"
                  secureTextEntry={!showPassword}
                  style={[commonStyles.input, { flex: 1 }]}
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

            <View style={commonStyles.inputGroup}>
              <Text style={commonStyles.label}>{t('location')}</Text>
              <TextInput
                placeholder={t('locationPlaceholder')}
                style={commonStyles.input}
                value={formData.location}
                onChangeText={(v) => updateField('location', v)}
              />
            </View>

            <View style={commonStyles.inputGroup}>
              <Text style={commonStyles.label}>{t('mobile')}</Text>
              <TextInput
                placeholder={t('mobilePlaceholder')}
                keyboardType="numeric"
                style={commonStyles.input}
                value={formData.mobileNo}
                onChangeText={(v) => updateField('mobileNo', v)}
              />
            </View>

            <View style={commonStyles.inputGroup}>
              <Text style={commonStyles.label}>{t('nid')}</Text>
              <TextInput
                placeholder={t('nidPlaceholder')}
                keyboardType="numeric"
                style={commonStyles.input}
                value={formData.nid}
                onChangeText={(v) => updateField('nid', v)}
              />
            </View>

            <TouchableOpacity style={commonStyles.primaryButton} onPress={handleSignup}>
              <Text style={commonStyles.buttonText}>{t('register')}</Text>
            </TouchableOpacity>

            <View style={styles.footer}>
              <Text style={commonStyles.text}>{t('alreadyHaveAccount')}</Text>
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
  keyboardView: {
    flex: 1,
  },
  scrollContent: {
    paddingTop: 20,
    paddingBottom: 40,
  },
  header: {
    marginBottom: 30,
  },
  form: {
    width: '100%',
  },
  nameRow: {
    flexDirection: 'row',
  },
  passwordContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderBottomWidth: 1,
    borderBottomColor: '#ddd',
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'center',
    marginTop: 25,
    gap: 4,
  },
  loginLink: {
    color: '#007AFF',
    fontSize: 14,
    fontWeight: '600',
  },
});
