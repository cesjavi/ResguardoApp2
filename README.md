# ResguardoApp

ResguardoApp is a simple Windows Forms application for managing a list of folders to be backed up. It allows you to select multiple folders, save the configuration, and detect removable drives, making it easier to manage your backup tasks.

## Features

-   **Add Folders**: Easily add folders to your backup list using a folder selection dialog.
-   **Remove Folders**: Remove folders you no longer need to back up.
-   **Save Configuration**: Save your list of backup folders to a `config.json` file. The configuration is stored in the user's local application data folder (`%LOCALAPPDATA%/ResguardoApp`).
-   **Automatic Loading**: The application automatically loads your saved configuration on startup.
-   **Detect Portable Drives**: A dedicated feature to list all connected removable drives (e.g., USB flash drives, external hard drives).
-   **Perform Backup**: Synchronize the selected folders to a chosen removable drive. The backup process is incremental, only copying new or modified files.

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

**Important Note**: This is a Windows Forms application and can only be compiled and run on a Windows operating system.