using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace BetterSteamMenu;

public static class HarmonyPatches
{
    // For information on how to use Harmony, view this documentation:
    // https://harmony.pardeike.net/articles/intro.html

    [HarmonyPatch]
    public class GorillaMetaReportPatch // this is to not hide the screen
    {
        [HarmonyPatch(typeof(GorillaMetaReport), "StartOverlay")]
        [HarmonyPostfix]
        public static void StartOverlayPostfix(GorillaMetaReport __instance)
        {
            if (Plugin.Instance.affectReportMenu.Value)
            {
                GameObject occluder = GameObject.Find("Miscellaneous Scripts/MetaReporting/ReportOccluder");
                occluder?.SetActive(false);
                GTPlayer.Instance.disableMovement = false;
            }
        }

        [HarmonyPatch(typeof(GorillaMetaReport), "ToggleLevelVisibility")]
        [HarmonyPrefix]
        public static bool ToggleVisibilityBlocker()
        {
            return !Plugin.Instance.affectReportMenu.Value;
        }
    }

    /// Here is where the actual patching starts! You don't need to mess with any of this.

    private static HarmonyLib.Harmony? _harmonyInstance;

    /// <summary>
    ///     The current instance of Harmony that is patching the assembly.
    ///     If there is no Harmony instance, it will create one and return it.
    ///     You do not need to touch this section
    /// </summary>
    private static HarmonyLib.Harmony HarmonyInstance
    {
        get
        {
            _harmonyInstance ??= new HarmonyLib.Harmony(Constants.Guid);
            return _harmonyInstance;
        }
    }

    /// <summary>
    /// Patch the assembly.
    /// </summary>
    public static void Patch()
    {
        HarmonyInstance.PatchAll();
    }

    /// <summary>
    /// Unpatch the assembly.
    /// </summary>
    public static void Unpatch()
    {
        HarmonyInstance.UnpatchSelf();
    }
}
