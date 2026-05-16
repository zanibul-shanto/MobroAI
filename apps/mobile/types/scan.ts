export interface ScanPhoto {
  id: string;
  scanId: string;
  imageData: string;
  createdAt: string;
  updatedAt: string;
}

export type ScanStatus = 0 | 1 | 2 | 3; // 0=Pending, 1=AI_Confirmed, 2=Officer_Verified, 3=Cleared

export interface Scan {
  id: string;
  childId: string;
  uploadedById: string;
  latitude: number | null;
  longitude: number | null;
  status: ScanStatus;
  analysisResultJson: string | null;
  confidenceScore: number;
  createdAt: string;
  updatedAt: string;
}

export interface UploadScanResponse {
  scan: Scan;
  photo: ScanPhoto;
}
