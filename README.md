# Unity UI Test Project

This project implements a simple UI-driven main menu with responsive layout, animated bottom navigation, and debug tools for testing UI states.

------------------------------------------------------------------------------------------------------------------------------------------------------
## Main Menu Overview

The main menu consists of:

- A full-screen background image with resolution-safe scaling
- A bottom navigation bar with 5 interactive buttons
- UI animations for transitions and state changes
- A debug panel for testing UI behavior
- Responsive Background

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

------------------------------------------------------------------------------------------------------------------------------------------------------
## Screen System

A reusable UI screen framework built to standardise screen behaviour, transitions, and instantiation across the project.

### Architecture

Screens are built on a shared base class and prefab that defines consistent lifecycle behaviour:

- Blurred background layer for visual focus
- Appear/Idle/Disappear animations

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

### Blur is implemented via a custom shader:

Shader:
[UIBlur.shader](Unity%20Project/Assets/Art/Shaders/Blur/UIBlur.shader)

### Screen Trigger Utility

A lightweight helper was added to decouple UI buttons from screen logic.

Script:
[ScreenOpener.cs](Unity%20Project/Assets/Scripts/Screens/ScreenOpener.cs)

Provides a simple entry point for UI interactions, calling the ScreenManager to open the appropriate screen. Designed for direct button hookups.

Summary

The system is designed to:

- Enforce consistent screen behaviour
- Reduce duplication via a shared base class and prefab
- Centralise screen control through a manager
- Simplify UI integration through a lightweight trigger layer
