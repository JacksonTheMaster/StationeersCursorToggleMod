using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.UI;
using BepInEx.Configuration;
using HarmonyLib;
using LaunchPadBooster;
using System.Collections.Generic;
using UnityEngine;

// CursorToggleMod - Tap (or double-tap) Alt (or the user's bound key) to toggle
//                   the mouse cursor on/off instead of holding it.
//
//   Single-tap mode  -> one press toggles the cursor on/off
//   Double-tap mode  -> two quick presses toggle the cursor on/off (default)
//
//   Configure via BepInEx/config/com.jacksonthemaster.cursortoggle.cfg

public class CursorToggleMod : MonoBehaviour
{
    public static readonly Mod MOD = new("CursorToggleMod", "1.1.0");

    // -- BepInEx Config --
    private static ConfigEntry<bool> _requireDoubleTap;

    // -- Configuration --
    private const float DOUBLE_TAP_THRESHOLD = 0.35f;

    // -- State --
    private static float _lastPressTime = -1f;
    private static bool  _isToggledOn;

    public static bool IsToggledOn => _isToggledOn;

    public void OnLoaded(List<GameObject> prefabs, ConfigFile config)
    {
        Debug.Log("[CursorToggleMod] v1.1.0 loading...");

        _requireDoubleTap = config.Bind(
            "General",
            "RequireDoubleTap",
            true,
            "When true (default), double-tap the MouseControl key (default Alt) to toggle the cursor.\n" +
            "When false, a single tap will toggle the cursor on/off."
        );

        Debug.Log($"[CursorToggleMod] TapMode: {(_requireDoubleTap.Value ? "Double-tap" : "Single-tap")}");

        var harmony = new Harmony("com.jacksonthemaster.cursortoggle");
        harmony.PatchAll();

        Debug.Log("[CursorToggleMod] v1.1.0 loaded - Harmony patches applied.");
    }

    private void Update()
    {
        if (GameManager.GameState != GameState.Running)
            return;

        if (!KeyManager.GetButtonDown(KeyMap.MouseControl))
            return;

        if (_requireDoubleTap != null && !_requireDoubleTap.Value)
        {
            // -- Single-tap mode --
            _isToggledOn = !_isToggledOn;
            Debug.Log($"[CursorToggleMod] Cursor {(_isToggledOn ? "TOGGLED ON" : "TOGGLED OFF")} (single-tap)");
            return;
        }

        // -- Double-tap mode --
        float now = Time.unscaledTime;

        if (_lastPressTime > 0f && now - _lastPressTime < DOUBLE_TAP_THRESHOLD)
        {
            _isToggledOn = !_isToggledOn;
            _lastPressTime = -1f;
            Debug.Log($"[CursorToggleMod] Cursor {(_isToggledOn ? "TOGGLED ON" : "TOGGLED OFF")} (double-tap)");
        }
        else
        {
            _lastPressTime = now;
        }
    }

    public static void ResetToggle()
    {
        _isToggledOn   = false;
        _lastPressTime = -1f;
    }
}
