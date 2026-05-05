export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber: string;
  role: 0 | 1 | 2; // 0: Admin, 1: HealthCareOfficer, 2: Parent
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  setUser: (user: User | null) => void;
  setToken: (token: string | null) => void;
  updateUser: (user: User) => Promise<void>;
  logout: () => Promise<void>;
  initialize: () => Promise<void>;
}
