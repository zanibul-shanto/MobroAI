import * as SecureStore from 'expo-secure-store';
import { API_BASE_URL } from './api';
import { UploadScanResponse } from '@/types/scan';

export async function uploadScan(
  childId: string,
  imageUri: string,
  latitude?: number,
  longitude?: number,
): Promise<UploadScanResponse> {
  const token = await SecureStore.getItemAsync('auth_token');

  const form = new FormData();
  form.append('file', { uri: imageUri, name: 'scan.jpg', type: 'image/jpeg' } as any);
  form.append('childId', childId);
  if (latitude != null) form.append('latitude', String(latitude));
  if (longitude != null) form.append('longitude', String(longitude));

  const response = await fetch(`${API_BASE_URL}/scans/upload`, {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${token}`,
      'X-Tunnel-Skip-AntiPhishing-Page': 'true',
      // Content-Type intentionally omitted — fetch sets multipart boundary automatically
    },
    body: form,
  });

  if (!response.ok) {
    const text = await response.text().catch(() => response.statusText);
    throw new Error(`Upload failed (${response.status}): ${text}`);
  }

  return response.json();
}
