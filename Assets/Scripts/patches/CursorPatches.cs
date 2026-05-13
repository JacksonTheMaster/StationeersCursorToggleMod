using Assets.Scripts;
using Assets.Scripts.UI;
using HarmonyLib;
using UnityEngine;

namespace CursorToggle
{
    // ────────────────────────────────────────────────────────────────────────────
    // Harmony Patches
    //
    // 1) Block InputMouse.SetMouseControl(false) while the cursor is toggled on.
    //    This prevents the normal key-release logic from hiding the cursor.
    //
    // 2) Block CursorManager.SetCursor(isLocked: true) while toggled on.
    //    This catches any other system that tries to re-lock the cursor
    //    (e.g. OnApplicationFocus / LockCursorDelayed).
    // ────────────────────────────────────────────────────────────────────────────

    [HarmonyPatch(typeof(InputMouse), "SetMouseControl")]
    public static class SetMouseControlPatch
    {
        /// When the cursor is toggled ON, prevent any code from calling
        /// SetMouseControl(false) which would hide the cursor.
        public static bool Prefix(bool useMouse)
        {
            if (CursorToggleMod.IsToggledOn && !useMouse)
            {
                // Swallow the call – keep cursor visible
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CursorManager), "SetCursor")]
    public static class SetCursorPatch
    {
        /// When the cursor is toggled ON, prevent SetCursor(true)
        /// from locking / hiding the cursor.
        /// (Note: in the game's code, isLocked=true means LOCK the cursor,
        ///  i.e. hide it. Confusing naming but that's how it is.)
        public static bool Prefix(bool isLocked)
        {
            if (CursorToggleMod.IsToggledOn && isLocked)
            {
                // Swallow the call – keep cursor unlocked & visible
                return false;
            }
            return true;
        }
    }
}
