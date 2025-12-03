import { createContext, useContext } from 'react';
import type { AuthContextModel } from '../types/AuthContextModel';

export const AuthContext = createContext<AuthContextModel | null>(null);

export const useAuth = () => useContext(AuthContext);