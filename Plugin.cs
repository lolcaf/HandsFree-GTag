using BepInEx;
using BepInEx.Configuration;
using GorillaLocomotion;
using System.Linq.Expressions;

namespace BetterSteamMenu;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
public class Plugin : BaseUnityPlugin
{
    /*
    I am assuming Ngbatz is here reading this
    Don't forget to check the HarmonyPatches.cs because that is where some of the code is
    The mod was originally going to be called Better Steam Menu (why the namespace has that name)
    */
    public static Plugin Instance { get; private set; }

    private bool onSteamVR
    {
        get
        {
            return NetworkSystem.Instance?.LocalPlayer?.Platform == "Steam";
        }
    }

    public ConfigEntry<bool> affectReportMenu;

    private void Start()
    {
        Instance = this;
        HarmonyPatches.Patch();
        affectReportMenu = Config.Bind("General", "Affect Report Menu", false, "Whether or not you want your hands to be free in the report menu");
    }

    private void Update()
    {
        if (onSteamVR)
            GTPlayer.Instance?.inOverlay = false; // this is what frees your hands from the report menu and the steam menu

        if (GTPlayer.Instance?.InReportMenu == true && affectReportMenu.Value == false)
            GTPlayer.Instance?.inOverlay = true;
    }

    private void OnDisable()
    {
        HarmonyPatches.Unpatch();
    }
}

public static class Constants
{
    public const string Name = "Hands Free";
    public const string Guid = "com.lolcaf.handsfree";
    public const string Version = "1.0.0";
}
