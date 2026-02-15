using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.UI;
using HarmonyLib;
using StationeersMods.Interface;
using UnityEngine;

// CursorToggleMod - Double-tap Alt (or the user's bound key) to toggle
//                   the mouse cursor on/off instead of holding it.
//
//   Single tap  -> default hold-to-show behaviour (unchanged)
//   Double tap  -> toggle cursor on; double tap again to toggle off

[StationeersMod("CursorToggleMod", "CursorToggleMod", "1.0.0")]
public class CursorToggleMod : ModBehaviour
{
    // -- Configuration --
    /// Maximum time (seconds) between two presses to count as a double-tap.
    private const float DOUBLE_TAP_THRESHOLD = 0.35f;

    // -- State --
    private static float _lastPressTime = -1f;
    private static bool  _isToggledOn;

    /// True when the cursor is locked on via double-tap.
    public static bool IsToggledOn => _isToggledOn;

    public override void OnLoaded(ContentHandler contentHandler)
    {
        Debug.Log("[CursorToggleMod] v1.0.0 loading...");

        // Create Harmony instance and apply all [HarmonyPatch] patches in this assembly
        var harmony = new Harmony("com.jacksonthemaster.cursortoggle");
        harmony.PatchAll();

        Debug.Log("[CursorToggleMod] v1.0.0 loaded - Harmony patches applied. Double-tap your MouseControl key to toggle the cursor.");
    }

    private void Update()
    {
        // Only run while the game is actually playing
        if (GameManager.GameState != GameState.Running)
            return;

        // Detect the MouseControl key press (same key the game checks in RunGameState)
        if (!KeyManager.GetButtonDown(KeyMap.MouseControl))
            return;

        float now = Time.unscaledTime;

        if (_lastPressTime > 0f && now - _lastPressTime < DOUBLE_TAP_THRESHOLD)
        {
            // -- Double-tap detected --
            _isToggledOn = !_isToggledOn;
            _lastPressTime = -1f;   // reset so a third quick tap won't re-toggle

            if (_isToggledOn)
            {
                // Cursor will stay visible after Alt is released because
                // CursorPatches blocks SetMouseControl(false) while toggled.
                Debug.Log("[CursorToggleMod] Cursor TOGGLED ON");
            }
            else
            {
                // Normal key-up logic in RunGameState will hide the cursor
                // once the player releases the key.
                Debug.Log("[CursorToggleMod] Cursor TOGGLED OFF");
            }
        }
        else
        {
            // First tap - record time, let normal hold-to-show behaviour run
            _lastPressTime = now;
        }
    }

    /// Reset toggle state (e.g. on scene change or mod unload).
    public static void ResetToggle()
    {
        _isToggledOn   = false;
        _lastPressTime = -1f;
    }
}
