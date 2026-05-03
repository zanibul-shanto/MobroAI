export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber: string;
  role: 'Parent' | 'HealthCareOfficer' | 'Admin';
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  setUser: (user: User | null) => void;
  setToken: (token: string | null) => void;
  logout: () => Promise<void>;
  initialize: () => Promise<void>;
}
