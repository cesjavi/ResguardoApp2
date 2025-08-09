# ResguardoApp

ResguardoApp is a simple Windows Forms application for managing a list of folders to be backed up. It allows you to select multiple folders, save the configuration, and detect removable drives, making it easier to manage your backup tasks.

## Features

-   **Add Folders**: Easily add folders to your backup list using a folder selection dialog.
-   **Remove Folders**: Remove folders you no longer need to back up.
-   **Save Configuration**: Save your list of backup folders to a `config.json` file located in the same directory as the application executable. Both the desktop application and the service expect `config.json` to reside alongside their executables.
-   **Automatic Loading**: The application automatically loads your saved configuration on startup.
-   **Detect Portable Drives**: A dedicated feature to list all connected removable drives (e.g., USB flash drives, external hard drives).
-   **Perform Backup**: Synchronize the selected folders to a chosen removable drive. The backup process is incremental, only copying new or modified files.
-   **Force Backup**: Set `forceBackupOnStart` in `config.json` to `true` to run a backup when the service starts, or call the service's public `ForceBackup` method to trigger a backup on demand.

## Requirements

-   Windows Operating System
-   .NET 6.0 SDK or later

## How to Build and Run

This project is a standard .NET WinForms application. You can build and run it using the .NET CLI or an IDE like Visual Studio.

### Using the .NET CLI

1.  **Clone the repository** or download the source code.
2.  **Open a terminal or command prompt** and navigate to the `ResguardoApp` project directory.
3.  **Run the application**:
    ```sh
    dotnet run
    ```
4.  **To build an executable**:
    ```sh
    dotnet publish -c Release --self-contained true -r win-x64
    ```
    The executable will be located in the `bin/Release/net6.0-windows/win-x64/publish/` directory.

### Using Visual Studio

1.  **Clone the repository** or download the source code.
2.  **Open the `ResguardoApp.sln` file** in Visual Studio (or open the `ResguardoApp` folder).
3.  **Set the startup project** to `ResguardoApp`.
4.  **Press `F5`** to build and run the application in debug mode, or use the "Build" menu to create a release version.

### Manual Backup Execution

-   **Run on Service Start**: Edit `config.json` and set `forceBackupOnStart` to `true`. The service will execute a backup immediately after loading the configuration.
-   **Run On Demand**: Use the `ForceBackup` method exposed by the service to perform a backup at any time without waiting for the scheduled time.
 
### Running the Windows Service

1.  **Build the service**:
    ```sh
    dotnet publish ResguardoAppService -c Release --self-contained true -r win-x64
    ```
    The published files will be in `ResguardoAppService/bin/Release/net8.0-windows/win-x64/publish/`.
2.  **Install the service** (run these commands from an elevated command prompt):
    ```cmd
    sc create ResguardoAppService binPath= "C:\\Path\\To\\ResguardoAppService.exe" start= auto
    ```
    Alternatively, you can use `InstallUtil.exe`:
    ```cmd
    InstallUtil.exe ResguardoAppService.exe
    ```
3.  **Configuration**: Place `config.json` in the same directory as `ResguardoAppService.exe`; the service reads its settings from this file.
4.  **Logging**: Set the `RESGUARDO_LOG_PATH` environment variable before starting the service to override the default log location (`%ProgramData%/ResguardoApp/`).
5.  **Start/Stop** the service:
    ```cmd
    sc start ResguardoAppService
    sc stop ResguardoAppService
    ```

### Logging

By default the service records errors in `%ProgramData%/ResguardoApp/error_resguardo_service.txt`. Set the `RESGUARDO_LOG_PATH` environment variable to change the log directory before launching the service.

**Important Note**: This is a Windows Forms application and can only be compiled and run on a Windows operating system.
