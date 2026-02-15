# CursorToggle – Double-Tap to Toggle Mouse Cursor

A **StationeersLaunchPad** mod for [Stationeers](https://store.steampowered.com/app/544550/Stationeers/) that lets you **double-tap** the MouseControl key (default: **Alt**) to **toggle** the mouse cursor on/off, instead of having to hold it.

## How It Works

| Action | Behaviour |
|--------|-----------|
| **Hold Alt** (single tap) | Cursor appears while held, disappears on release *(vanilla)* |
| **Double-tap Alt** | Cursor stays visible – double-tap again to hide it |

The mod detects two presses of your MouseControl key within 0.35 seconds. On the second press it toggles "sticky cursor" mode. While toggled on, the normal key-release logic is blocked by two small Harmony patches so the cursor stays visible. Double-tap again to turn it off — the cursor will hide on key release just like normal.

## Installation

1. Install **BepInEx** + **StationeersLaunchPad**
2. Subscribe to the mod on the Steam Workshop (or drop it in your mods folder)
3. Launch the game — double-tap Alt to toggle the cursor

## Technical Details

The mod hooks into:
- `InputMouse.SetMouseControl(bool)` — blocks `SetMouseControl(false)` while toggled on
- `CursorManager.SetCursor(bool)` — blocks `SetCursor(true)` (i.e. lock/hide) while toggled on

These are lightweight **Harmony Prefix** patches that simply return `false` to skip the original method when the toggle is active.

## Feedback & Support

Love it? Hate it?
Join the SSUI Discord: https://discord.gg/8n3vN92MyJ
