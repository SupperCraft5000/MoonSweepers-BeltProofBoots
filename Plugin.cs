// SPDX-License-Identifier: GPL-3.0-or-later
using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MoonCleaners;
using UnityEngine;

namespace BeltProofBoots;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BasePlugin
{
    public const string Guid = "moonsweepers.beltproofboots";
    public const string Name = "Belt-Proof Boots";
    public const string Version = "0.1.0";

    internal static ManualLogSource Logger;
    internal static ConfigEntry<bool> Enabled;
    internal static ConfigEntry<bool> RequireGravityBoots;
    internal static ConfigEntry<float> FootReach;

    public override void Load()
    {
        Logger = Log;
        Enabled = Config.Bind("General", "Enabled", true, "Belts no longer carry you along while the condition below is met.");
        RequireGravityBoots = Config.Bind("General", "RequireGravityBoots", true,
            "Only while your gravity boots are switched on. Set to false to never be carried by belts on foot.");
        FootReach = Config.Bind("Advanced", "FootReach", 2.5f,
            new ConfigDescription("How close (metres) the belt contact point must be to your astronaut to count as standing on it.",
                new AcceptableValueRange<float>(0.5f, 10f)));
        new Harmony(Guid).PatchAll(typeof(Patches));
        Logger.LogInfo($"{Name} {Version} loaded");
    }
}

internal static class Patches
{
    static bool failed;

    /// <summary>
    /// The game asks for a belt's surface velocity with the belt collider and the contact point,
    /// but not who stands there. The local astronaut on foot is recognised by the contact point
    /// being at their position; vehicles and robots that are ridden keep moving with the belt.
    /// </summary>
    [HarmonyPatch(typeof(MoonWorld), nameof(MoonWorld.BeltSurfaceVelocity))]
    [HarmonyPostfix]
    static void SurfaceVelocity(Vector3 point, ref Vector3 velocity, bool __result)
    {
        if (!__result || failed || !Plugin.Enabled.Value) return;
        try
        {
            var session = MoonCrewSession.Instance;
            var walker = session != null ? session.local : null;
            if (walker == null || walker.Mounted || walker.Riding != null) return;
            if (Plugin.RequireGravityBoots.Value && !walker.GravityBootsOn) return;
            float reach = Plugin.FootReach.Value;
            if ((walker.transform.position - point).sqrMagnitude > reach * reach) return;
            velocity = Vector3.zero;
        }
        catch (Exception e)
        {
            failed = true;
            Plugin.Logger.LogError($"Disabled after error: {e}");
        }
    }
}
