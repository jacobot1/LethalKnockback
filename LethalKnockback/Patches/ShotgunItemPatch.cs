using HarmonyLib;
using UnityEngine;

namespace LethalKnockback.Patches
{
    [HarmonyPatch(typeof(ShotgunItem))]
    internal class ShotgunItemPatch
    {
        static float currentKnockbackForce = 0f;

        [HarmonyPatch("ShootGun")]
        [HarmonyPostfix]
        static void AddKnockbackPatch(ShotgunItem __instance)
        {
            // Knock back player
            if (!__instance.safetyOn && __instance.playerHeldBy == GameNetworkManager.Instance.localPlayerController && __instance.isHeld)
            {
                currentKnockbackForce += LethalKnockbackMod.configKnockbackForce.Value;
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void ContinuousKnockbackPatch(ShotgunItem __instance)
        {
            // Continuous knockback
            if (__instance.playerHeldBy == GameNetworkManager.Instance.localPlayerController && __instance.isHeld)
            {
                __instance.playerHeldBy.externalForces += __instance.transform.forward * -currentKnockbackForce;
                currentKnockbackForce = Mathf.Lerp(currentKnockbackForce, 0f, 5f * Time.deltaTime);
            }
        }
    }
}