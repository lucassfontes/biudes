# IPTV Downloader — Android / Chromebook

Versão: **v_2.0.24**
Pacote Android: `br.com.iptvdownloader.app`

Esta variante reaproveita o catálogo Xtream, contas, favoritos, histórico e motor de downloads da versão desktop, usando Avalonia Android.

## O que foi adaptado
- Inicialização Android com `MainActivity`.
- Interface em `UserControl`, adequada ao ciclo de vida Android.
- Permissão de Internet e tráfego HTTP (`usesCleartextTraffic=true`) para servidores Xtream que usam `http://`.
- Downloads pelo motor .NET no Android (sem depender de `curl`).
- Dados, cache e configurações salvos no armazenamento privado do aplicativo.
- Downloads salvos no diretório externo de Downloads do próprio app.
- Título/label: `IPTV Downloader — v_2.0.24`.
- Removida no Android a opção de desligar o computador ao finalizar.

## Gerar APK
Em uma máquina com .NET 8 e Android workload:

```bash
chmod +x build-android-apk.sh
./build-android-apk.sh
```

O APK final será copiado para:

`dist/IPTV_Downloader_v_2.0.24_Android.apk`

## Chromebook
Depois de gerar o APK, copie-o para o Chromebook e instale-o como aplicativo Android. Alguns modelos/políticas do ChromeOS exigem habilitar instalação de APKs externos.
