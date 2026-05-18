import * as z from 'zod';

export const passwordField = z.string().min(6, 'Password must be at least 6 characters');
export const emailField = z.string().email('Invalid email address');
export const fullNameField = z.string().min(2, 'Name is too short');
export const phoneField = z.string().min(10, 'Phone number must be at least 10 digits');
