/**
 * Below are the colors that are used in the app. The colors are defined in the light and dark mode.
 * There are many other ways to style your app. For example, [Nativewind](https://www.nativewind.dev/), [Tamagui](https://tamagui.dev/), [unistyles](https://reactnativeunistyles.vercel.app), etc.
 */

const tintColorLight = '#0a7ea4';
const tintColorDark = '#fff';

export const Colors = {
  light: {
    text: '#1A1A1A',
    textSecondary: '#666666',
    background: '#FFFFFF',
    surface: '#F5F5F7',
    primary: '#007AFF',
    secondary: '#5856D6',
    accent: '#FF2D55',
    border: '#E5E5EA',
    tint: tintColorLight,
    icon: '#8E8E93',
    tabIconDefault: '#8E8E93',
    tabIconSelected: tintColorLight,
    error: '#FF3B30',
    success: '#34C759',
    card: '#FFFFFF',
  },
  dark: {
    text: '#FFFFFF',
    textSecondary: '#A1A1AA',
    background: '#09090B',
    surface: '#18181B',
    primary: '#3B82F6',
    secondary: '#8B5CF6',
    accent: '#F43F5E',
    border: '#27272A',
    tint: tintColorDark,
    icon: '#71717A',
    tabIconDefault: '#71717A',
    tabIconSelected: tintColorDark,
    error: '#EF4444',
    success: '#22C55E',
    card: '#18181B',
  },
};

export const Fonts = {
  sans: 'normal',
  serif: 'serif',
  rounded: 'normal',
  mono: 'monospace',
};
