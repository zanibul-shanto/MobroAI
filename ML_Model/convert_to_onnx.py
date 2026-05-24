"""
Convert any Keras .h5 model to ONNX format.
Uses Keras 3 built-in export with PyTorch backend.

Usage:
  python3 ML_Model/convert_to_onnx.py --input ML_Model/model.h5
  python3 ML_Model/convert_to_onnx.py --input ML_Model/model.h5 --output ML_Model/model.onnx

Arguments:
  --input   Path to the Keras .h5 model file (required)
  --output  Path for the output .onnx file (optional, defaults to same path as input with .onnx extension)
"""

import os
os.environ["KERAS_BACKEND"] = "torch"
os.environ["PYTHONIOENCODING"] = "utf-8"

import argparse
import numpy as np
import keras
import onnx

parser = argparse.ArgumentParser(description="Convert Keras .h5 model to ONNX")
parser.add_argument("--input", required=True, help="Path to the .h5 model file")
parser.add_argument("--output", default=None, help="Path for the output .onnx file")
args = parser.parse_args()

H5_PATH = args.input
ONNX_PATH = args.output or os.path.splitext(H5_PATH)[0] + ".onnx"

print(f"Input : {H5_PATH}")
print(f"Output: {ONNX_PATH}")

print("\nLoading model ...")
model = keras.saving.load_model(H5_PATH)
print(f"Model loaded: {model.name}  input={model.input_shape}")

# Build dummy input from model's input shape (skip batch dim = None)
input_shape = model.input_shape  # e.g. (None, 224, 224, 3)
dummy_shape = (1,) + tuple(d if d is not None else 1 for d in input_shape[1:])
dummy = np.zeros(dummy_shape, dtype="float32")
model(dummy, training=False)
print(f"Forward pass done (dummy shape: {dummy_shape})")

print("\nExporting to ONNX ...")
model.export(ONNX_PATH, format="onnx")

m = onnx.load(ONNX_PATH)
onnx.checker.check_model(m)
print(f"\nDone. Saved & verified: {ONNX_PATH}")
