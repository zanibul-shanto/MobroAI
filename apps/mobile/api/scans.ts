import { api } from './api';
import { UploadScanResponse } from '@/types/scan';

export async function uploadScan(
  childId: string,
  imageUri: string,
  latitude?: number,
  longitude?: number,
): Promise<UploadScanResponse> {
  const form = new FormData();
  form.append('file', { uri: imageUri, name: 'scan.jpg', type: 'image/jpeg' } as any);
  form.append('childId', childId);
  if (latitude != null) form.append('latitude', String(latitude));
  if (longitude != null) form.append('longitude', String(longitude));

  const { data } = await api.post<UploadScanResponse>('/scans/upload', form, {
    headers: {
      'Content-Type': 'multipart/form-data',
      'X-Tunnel-Skip-AntiPhishing-Page': 'true',
    },
  });
  return data;
}
