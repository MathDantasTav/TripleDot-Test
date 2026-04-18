# Unity UI Test Project

- **Android APK (quick test):**  
  [Download APK](Builds/TripleDot_UI_Test.apk?raw=true)
- **Full Unity Project (source):**  
  https://github.com/MathDantasTav/TripleDot-Test/archive/refs/heads/main.zip

This project implements a simple UI-driven main menu with responsive layout, animated bottom navigation, and debug tools for testing UI states.

---------------------------------------------------------------------------------------------------------------------------
# Main Menu Overview

The main menu consists of:

- A full-screen background image with resolution-safe scaling
- A bottom navigation bar with 5 interactive buttons
- UI animations for transitions and state changes
- A debug panel for testing UI behavior

<details>
<summary>Main Menu Details</summary>
  
### Background scaling
  
A custom script was implemented to ensure the background image scales correctly across different resolutions while preserving aspect ratio and preventing stretching.

Script:
[ImageAspectRatioFitter.cs](Unity%20Project/Assets/Scripts/UI/ImageAspectRatioFitter.cs)

### Bottom Navigation Bar

A bottom bar with 5 buttons was implemented with support for:

- Open and close animations
- Locked/Unlocking animations
- Icon animations per button

Bottom bar also contains two UnityEvents:

- ContentActivated: triggered when a button is pressed
- Closed: triggered when all bottom buttons are disabled

Controller scripts:
[BottomBarView.cs](Unity%20Project/Assets/Scripts/Main_Menu/BottomBarView.cs)
[BottomBarView.cs](Unity%20Project/Assets/Scripts/Main_Menu/BottomBarButton.cs)

### Debug Controls

A debug button in the bottom-left of the main menu provides tools to test UI behaviour, including:

- Toggling the bottom bar show/hide
- Locking/Unlocking bottom bar buttons
- Previewing bottom bar buttons icon animations

</details>

-------------------------------------------------
# Screen System

A reusable UI screen framework built to standardise screen behaviour, transitions, and instantiation across the project.
Screens are built on a shared base class and prefab that defines consistent lifecycle behaviour:

- Blurred background layer for visual focus
- Appear/Idle/Disappear animations


<details>
<summary>Screen Architecture</summary>

### Base generic Script and Prefab used by all Screens

Script:
[ScreenInstance.cs](Unity%20Project/Assets/Scripts/Screens/ScreenInstance.cs)
Prefab:
[Popup_Screen.prefab](Unity%20Project/Assets/Prefabs/Screens/Popup_Screen.prefab)

All screens are implemented as prefab variants of Popup_Screen, using either the base ScreenInstance script or a script derived from it when additional behaviour is required.

### Screen Manager

A central singleton manager handles screen lifecycle and instantiation.

Script:
[ScreensManager.cs](Unity%20Project/Assets/Scripts/Screens/ScreensManager.cs)

Responsibilities:

- Instantiate and register screens
- Manage active screen state
- Control transitions between screens

### Screen Trigger Utility

A lightweight helper was added to decouple UI buttons from screen logic.

Script:
[ScreenOpener.cs](Unity%20Project/Assets/Scripts/Screens/ScreenOpener.cs)

Provides a simple entry point for UI interactions, calling the ScreenManager to open the appropriate screen. Designed for direct button hookups.

### Blur is implemented via a custom shader:

Shader:
[UIBlur.shader](Unity%20Project/Assets/Art/Shaders/Blur/UIBlur.shader)

### UI Feedback System

[BtnFeedback.cs](Unity%20Project/Assets/Scripts/UI/BtnFeedback.cs) and [Toggle.cs](Unity%20Project/Assets/Scripts/UI/Toggle.cs) provide a generic UI feedback layer used across all buttons and toggles in the project. They handle scale, color, and sound feedback to ensure consistent interaction responses across the UI.
</details>

---------------------------------------------------------------------------------------------------------------------------
# Language System

A fully functional global localisation system powered by an external Google Sheet, providing complete runtime text translation, language switching, and editor-driven workflow updates.

<details>
<summary>Localization Implementation</summary>

### Language Selection

A language button in the Settings menu opens a popup where the user can select their preferred language.

### Data Source

Translations are managed via a [Google Sheet](https://docs.google.com/spreadsheets/d/1fwy4EqNHLJazoDyAI4jabAOXqOYztwM39k4eBC1m0wY/edit?usp=sharing)
This is the single source of truth for all localisation data.

### Core System

Script:
[LanguageManager.cs](Unity%20Project/Assets/Scripts/Language/LanguageManager.cs)

Located in the main scene under the LanguageManager object, this component handles:

- Language selection and application
- Runtime translation updates
- Integration with UI text elements
- Editor Workflow

The system includes an inspector button in LanguageManager to sync translations from the Google Sheet into the Translations ScriptableObject.

### To add a new language:

- Add it to the [Google Sheet](https://docs.google.com/spreadsheets/d/1fwy4EqNHLJazoDyAI4jabAOXqOYztwM39k4eBC1m0wY/edit?usp=sharing)
- Add the enum in [LanguageTranslations.cs](Unity%20Project/Assets/Scripts/Language/LanguageTranslations.cs)
- Click the update button in [LanguageManager.cs](Unity%20Project/Assets/Scripts/Language/LanguageManager.cs)
- Register it in [Translations](Unity%20Project/Assets/Resources/Translations.asset) (LanguagesInfo with enum + flag icon)

The language then becomes available automatically in the UI.

### Translated Text System

This system automatically handles:

- Runtime text translation updates
- Reacting to language changes
- Ensuring consistent localisation across all UI elements

Prefab: [TranslatedText.prefab](Unity%20Project/Assets/Prefabs/TranslatedText.prefab)
Script: [TextTranslator.cs](Unity%20Project/Assets/Scripts/Language/TextTranslator.cs)
</details>

------------------------------------
# Level Completed Screen

A Level Completed screen was implemented and can be triggered via a test button in the HomeScreen.

The sequence includes an initial animation with particles, UI scaling effects, and a final shine transition.

<details>
<summary>Level Completed Animations</summary>

### Scripts/Shaders used:
- [TMPBounceAnimation.cs](Unity%20Project/Assets/Scripts/Animations/TMPBounceAnimation.cs) handles an idle bounce animation for the "Level Completed!" text (implemented as a TextMeshPro text, not an image, and fully translatable)
- [TMPNumberAnimation.cs](Unity%20Project/Assets/Scripts/Animations/TMPNumberAnimation.cs) animates a numeric value from 0 to 250 during the reward sequence
- [ScrollingTiledImage.shader](Unity%20Project/Assets/Art/Shaders/Scrolling%20Tilled%20Image/ScrollingTiledImage.shader) A custom scrolling texture effect used for the background
</details>
