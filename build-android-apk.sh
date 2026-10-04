#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"

echo "== IPTV Downloader Android v_2.0.24 =="
if ! command -v dotnet >/dev/null 2>&1; then
  echo "ERRO: .NET 8 SDK não encontrado."
  echo "Instale o .NET 8 SDK e execute novamente."
  exit 1
fi

dotnet workload install android

dotnet restore src/IPTVDownloader.Android/IPTVDownloader.Android.csproj
dotnet publish src/IPTVDownloader.Android/IPTVDownloader.Android.csproj -c Release -f net8.0-android -p:AndroidPackageFormat=apk

APK=$(find src/IPTVDownloader.Android/bin/Release/net8.0-android -type f -name '*.apk' | head -n 1 || true)
if [ -z "$APK" ]; then
  echo "ERRO: APK não encontrado após o build."
  exit 1
fi
mkdir -p dist
cp "$APK" "dist/IPTV_Downloader_v_2.0.24_Android.apk"
echo ""
echo "APK criado em: dist/IPTV_Downloader_v_2.0.24_Android.apk"
