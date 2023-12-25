using System;
using System.IO;
using UnityEngine;
using System.Reflection;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Object = UnityEngine.Object;
using System.Xml.Linq;

namespace TateBracken
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "TateBracken";
        private const string modName = "Andrew Tate Bracken Mod";
        private const string modVersion = "1.0.0";
        public static AssetBundle tate;

        public Harmony harmonymain;
        internal ManualLogSource mls;

        void Awake()
        {
            tate = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TateBracken"));
            harmonymain = new Harmony(modGUID);
            harmonymain.PatchAll();

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Breathe air");
        }
    }
}

namespace TateBracken.Patches
{
    [HarmonyPatch]
    internal class EnemyTypes
    {
        [HarmonyPatch(typeof(FlowermanAI), "Start")]
        [HarmonyPostfix]

        public static void SummonTate(FlowermanAI __instance)
        {
            __instance.creatureAngerVoice.clip = Plugin.tate.LoadAsset<AudioClip>("bugatti.mp3");
            __instance.crackNeckSFX = Plugin.tate.LoadAsset<AudioClip>("breathe.mp3");
            __instance.crackNeckAudio.clip = Plugin.tate.LoadAsset<AudioClip>("breathe.mp3");
            Object.Destroy((Object)(object)__instance.gameObject.transform.Find("FlowermanModel").Find("LOD1").gameObject.GetComponent<SkinnedMeshRenderer>());
            GameObject val = Object.Instantiate(Plugin.tate.LoadAsset<GameObject>("tate.prefab"), __instance.gameObject.transform);
            val.transform.localPosition = new Vector3(0f, 1.5f, 0f);
        }
    }
}