import { create } from 'zustand';
import * as SecureStore from 'expo-secure-store';
import { AuthState, User } from '@/types/auth';

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'auth_user';

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,
  isLoading: true,

  setUser: (user) => set({ user, isAuthenticated: !!user }),
  setToken: (token) => set({ token }),

  updateUser: async (updatedUser) => {
    const userJson = JSON.stringify(updatedUser);
    await SecureStore.setItemAsync(USER_KEY, userJson);
    set({ user: updatedUser });
  },

  logout: async () => {
    await SecureStore.deleteItemAsync(TOKEN_KEY);
    await SecureStore.deleteItemAsync(USER_KEY);
    set({ user: null, token: null, isAuthenticated: false });
  },

  initialize: async () => {
    try {
      const token = await SecureStore.getItemAsync(TOKEN_KEY);
      const userJson = await SecureStore.getItemAsync(USER_KEY);
      
      if (token && userJson) {
        set({ 
          token, 
          user: JSON.parse(userJson), 
          isAuthenticated: true,
          isLoading: false 
        });
      } else {
        set({ isLoading: false });
      }
    } catch (error) {
      console.error('Auth initialization failed:', error);
      set({ isLoading: false });
    }
  },
}));

export const saveAuthData = async (token: string, user: User) => {
  await SecureStore.setItemAsync(TOKEN_KEY, token);
  await SecureStore.setItemAsync(USER_KEY, JSON.stringify(user));
};
