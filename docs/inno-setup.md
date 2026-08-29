# Inno Setup — download e teste local

O instalador do Windows Configurations é gerado pelo [Inno Setup 6](https://jrsoftware.org/isinfo.php), a partir do script `Installer/setup.iss`. Este guia cobre a instalação da ferramenta e o teste na sua máquina, no mesmo fluxo usado pelo GitHub Action.

## Qual versão baixar

Use o **Inno Setup 6** (série 6.x). Não use o Inno Setup 5.

1. Abra a [página de download](https://jrsoftware.org/isdl.php).
2. Baixe o instalador **Inno Setup** (`innosetup-6.x.x.exe`). O QuickStart Pack não é necessário.
3. Na instalação, mantenha marcado **Inno Setup Preprocessor**. O `setup.iss` usa `#define` e `#ifndef`.

O compilador de linha de comando deve ficar em:

```text
C:\Program Files (x86)\Inno Setup 6\ISCC.exe
```

Se o caminho for outro, ajuste os comandos abaixo.

## Publicar o aplicativo

Abra o PowerShell na raiz do repositório (`Windows.Configurations`) e publique em Release, self-contained, x64:

```powershell
dotnet publish Windows.Configurations.csproj -c Release -r win-x64 --self-contained true -o publish
```

A pasta `publish\` precisa existir e conter `Windows.Configurations.exe` antes de compilador o instalador. O `setup.iss` lê essa pasta por padrão (`..\publish` relativo a `Installer\`).

## Compilar o instalador

Pela linha de comando (recomendado, igual ao CI):

```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" /DMyAppVersion=1.0.0 Installer\setup.iss
```

Pelo Inno Setup Compiler (GUI):

1. Abra `Installer\setup.iss`.
2. Compile (`Build` → `Compile`, ou Ctrl+F9).
3. Sem `/DMyAppVersion`, a versão usada é `1.0.0` (valor padrão do script).

O `.exe` gerado sai em:

```text
artifacts\Windows.Configurations_1.0.0_Setup.exe
```

Se passar outra versão, o nome do arquivo acompanha, por exemplo `Windows.Configurations_1.0.12_Setup.exe`.

## Testar o instalador

Feche o Windows Configurations se ele estiver aberto. O script pede privilégio de administrador.

Instalação com assistente:

```powershell
.\artifacts\Windows.Configurations_1.0.0_Setup.exe
```

Instalação silenciosa (útil para repetir o teste):

```powershell
.\artifacts\Windows.Configurations_1.0.0_Setup.exe /VERYSILENT /NORESTART
```

Confirme depois:

- O app está em `C:\Program Files\Windows Configurations\`.
- Há atalho no menu Iniciar.
- O `Windows.Configurations.json` só é criado na primeira instalação; reinstalação **não** sobrescreve um JSON que já exista.
- O aplicativo abre e aparece na bandeja.

Para desinstalar: **Aplicativos** do Windows, ou o atalho de desinstalação no menu Iniciar.

## Observações

- `publish\` e `artifacts\` estão no `.gitignore`; não precisam ser commitados.
- No GitHub, o workflow `.github/workflows/release.yml` instala o Inno Setup 6 via `winget` (`JRSoftware.InnoSetup`) e gera a release automaticamente no push em `main`.
