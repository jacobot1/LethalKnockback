using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalKnockback.Patches;

namespace LethalKnockback
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LethalKnockbackMod : BaseUnityPlugin
    {
        public const string modGUID = "com.jacobot5.LethalKnockback";
        public const string modName = "LethalKnockback";
        public const string modVersion = "1.0.1";

        // Initalize Harmony
        private readonly Harmony harmony = new Harmony(modGUID);

        // Create static instance
        private static LethalKnockbackMod Instance;

        // Configuration
        public static ConfigEntry<float> configKnockbackForce;

        // Initialize logging
        public static ManualLogSource mls;

        private void Awake()
        {
            // Ensure static instance
            if (Instance == null)
            {
                Instance = this;
            }

            // Send alive message
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("LethalKnockback has awoken.");

            // Bind configuration
            configKnockbackForce = Config.Bind("Knockback",
                                                "KnockbackForce",
                                                15f,
                                                "How much knockback force to apply. Default is 15. Feel free to break physics.");

            // Do the patching
            harmony.PatchAll(typeof(LethalKnockbackMod));
            harmony.PatchAll(typeof(ShotgunItemPatch));
        }
    }
}
