# PlutoCareGames
Serious Games made in VR to support paediatric occupational therapy. Created in collaboration with the PlutoCare fourth year design project group at the University of Waterloo.

## Usage
To play these games you will need an Oculus Quest and Unit Editor version 2019.2.0f1 or later with the Android SDK build configurations installed. Configure your machine with the appropriate Android SDK's in accordance with [this](https://developer.oculus.com/documentation/unity/unity-mobileprep/) page, and ensure that the App ID in the `Oculus / Platform / Edit Settings` is already set. The rest of the project settings should already be configured to build and deploy a build for Oculus Quest.

Ensure that your Quest has developer mode enabled, and can be connected to the appropriate device with the Unity project in this repo. Windows users may need ton install the Android ADB driver.

## Building
Follow the instructions on the `OVR Build APK and Run` section of [this](https://developer.oculus.com/documentation/unity/unity-build-android-tools/) page to deploy a development build to your Oculus Quest. It should appear under `Library->Unknown Sources->PlutoCare` in your Oculus Quest virtual home menu.
