# Migraine Relief Device Controller (.NET MAUI App)

## Overview

- This repository contains code for a cross-platform (Android and iOS) mobile application that controls a migraine relief device using a Bluetooth Low Energy (BLE) connection. It is designed to work alongside a separate repository containing the Arduino code.
- The application is primarily written in C# and utilizes **.NET MAUI** to support cross-platform functionality. **Visual Studio** or **Visual Studio Code** are recommended for running and modifying the code.
- To test the application on a physical iOS device, you must use a Mac. You must also set up your own certificates via an Apple Developer Account. A tutorial can be found [here](https://www.youtube.com/watch?v=B-kZ-AgEeO8).
- The repository includes functionality for user account creation and saving/loading of user-created presets using **Firebase Cloud Firestore**.
- To use the app:
  1. Connect to a valid Arduino device by tapping the **Connect** button on the main page and selecting a device from the list.
  2. Configure time, strength, and position settings by selecting the corresponding buttons.
  3. Tap **Start Session** to begin. During the session, the position can be adjusted using the arrow buttons.
  4. Use the **Login/Signup** pages to create or access a user account. When logged in, settings can be saved and loaded via the **Presets** page.

---

## Pages

The code for various screens in the app is located in the `MauiApp1/Pages` folder.

- **UI Layouts** are defined in `.xaml` files.
- **Logic and event handling** are implemented in the corresponding `.xaml.cs` files.

### Page Descriptions

- **MainPage**
  - The default page loaded when the app starts.
  - Displays selected settings and navigation buttons.
  - Uses `MainViewModel.cs` (`MauiApp1/Classes/Models/MainViewModel.cs`) for login status updates.

- **BluetoothConnectionPage**
  - Allows users to scan for and connect to nearby BLE devices.
  - Uses `BLEViewModel.cs` (`MauiApp1/Classes/Models/BLEViewModel.cs`) to populate the device list.

- **ControllerPage**
  - Contains arrow buttons for adjusting the connected device’s position.

- **FirebaseLoginPage**
  - Contains code for logging into an existing user account.

- **FirebaseSignUpPage**
  - Contains code for creating a new user account.

- **InstructionsPage**
  - Displays usage instructions for the app.

- **PresetsPage**
  - Shows a list of saved presets that can be loaded or deleted.
  - Includes a button to save the current settings.
  - Uses `PresetsViewModel.cs` (`MauiApp1/Classes/Models/PresetsViewModel.cs`) for list logic.

- **StrengthPage**
  - Allows users to set strength and position preferences.

- **TimerPage**
  - Allows users to configure the time setting.

---

## Bluetooth Low Energy (BLE)

- BLE functionality is supported via a community repository: [dotnet-bluetooth-le](https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le).
- BLE-related code is found in:
  - `BluetoothManager.cs`
  - `BleViewModel.cs`
  - `DeviceViewModel.cs`
  - `BluetoothConnectionPage.xaml.cs`
  - `MainPage.xaml.cs`
  - `ControllerPage.xaml.cs`

> **IMPORTANT:** If the advertised name, characteristic, or service is changed in the Arduino code, you **must** update those values in both `BleViewModel.cs` and `ControllerPage.xaml.cs`.

---

## Firebase

- The app interacts with Firebase via the **Cloud Firestore** and **Firebase Authentication REST APIs**.
- Most Firebase logic is implemented in `Firebase.cs` (`MauiApp1/Classes/Firebase.cs`).

> **IMPORTANT:** To use Firebase features:
> 1. Set up your own Firebase project and database.
> 2. Update the `ApiKey` and `projectId` values at the top of `Firebase.cs`.

- The `Firebase.cs` file contains examples of JSON for sending and receiving data. These can be modified to add or remove fields as needed.

---

## Syncfusion

- The UI for this app uses **Syncfusion** for some of its widgets.
- This is implemented in various classes, such as `StrengthPage`

- Syncfusion and its documentation can be found [here](https://www.syncfusion.com/).

---

## FontAwesome

- The UI for this app uses glyphs and icons by **FontAwesome**.

- FontAwesome and its documentation can be found [here](https://fontawesome.com/start).

---

## Testing

- Unit tests are located in the `UnitTestProject` folder and written using **JUnit**.
- The tests primarily focus on Firebase features.
- BLE and UI testing must be done manually.
- Tests interact with the Firebase database but use mock secure storage and mock connectivity.

---

Let me know if you'd like help generating a `LICENSE` or CI/CD workflow section, or setting up GitHub Actions for automated testing!
