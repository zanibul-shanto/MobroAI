import React from 'react';
import { StyleSheet, View, Text, TouchableOpacity } from 'react-native';
import { useRouter } from 'expo-router';
import { Ionicons } from '@expo/vector-icons';
import { useLanguage } from '@/context/LanguageContext';
import { commonStyles } from '@/constants/styles';

export default function HomeScreen() {
  const router = useRouter();
  const { t, language, setLanguage } = useLanguage();

  return (
    <View style={[commonStyles.container, { paddingHorizontal: 30 }]}>
      <View style={styles.langToggle}>
        <TouchableOpacity 
          onPress={() => setLanguage(language === 'en' ? 'bn' : 'en')}
          style={styles.langButton}
        >
          <Ionicons name="language-outline" size={20} color="#007AFF" />
          <Text style={styles.langText}>{language === 'en' ? 'Bangla' : 'English'}</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.content}>
        <View style={styles.logoContainer}>
          <Ionicons name="scan-outline" size={80} color="#007AFF" />
          <Text style={[commonStyles.title, { marginTop: 20 }]}>{t('appName')}</Text>
          <Text style={[commonStyles.subtitle, { marginTop: 8 }]}>{t('tagline')}</Text>
        </View>

        <View style={styles.buttonContainer}>
          <TouchableOpacity 
            style={commonStyles.primaryButton}
            onPress={() => router.push('/signup')}
          >
            <Text style={commonStyles.buttonText}>{t('getStarted')}</Text>
          </TouchableOpacity>

          <TouchableOpacity 
            style={commonStyles.secondaryButton}
            onPress={() => console.log('Login pressed')}
          >
            <Text style={commonStyles.secondaryButtonText}>{t('signIn')}</Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  langToggle: {
    alignItems: 'flex-end',
    marginTop: 60,
  },
  langButton: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 8,
    borderRadius: 8,
    backgroundColor: '#f0f7ff',
  },
  langText: {
    marginLeft: 5,
    color: '#007AFF',
    fontWeight: '600',
    fontSize: 14,
  },
  content: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
    paddingBottom: 60,
  },
  logoContainer: {
    alignItems: 'center',
    marginBottom: 80,
  },
  buttonContainer: {
    width: '100%',
  },
});
