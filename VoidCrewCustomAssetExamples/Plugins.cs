using BepInEx;
using BepInEx.Logging;
using CG.Client.PlayerData;
using CG.Client.UserData;
using CG.Ship.Object;
using Client.Player.Interactions;
using HarmonyLib;
using ResourceAssets;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VoidManager;
using VoidManager.Content;
using VoidManager.MPModChecks;

namespace VoidCrewCustomAssetExamples
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.USERS_PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "N/A")]
        private void Awake()
        {
            Log = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }


    public class VoidManagerPlugin : VoidPlugin
    {
        static AssetBundle VCCATAssetBundle = null;
        public static Texture2D MiddleFingerProjectionTexture = null;
        public static Texture2D SnowmanProjectionTexture = null;

        public static string MiddleFingerProjectionUniqueString = $"{MyPluginInfo.PLUGIN_GUID}.Middlefinger";
        public static GUIDUnion MiddleFingerProjectionGUID;

        public static string SnowmanUniqueString = $"{MyPluginInfo.PLUGIN_GUID}.Snowman2x2";
        public static GUIDUnion SnowmanProjectionGUID;

        public VoidManagerPlugin()
        {
            //Load assets from bundle.
            VCCATAssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("VoidCrewCustomAssetExamples.helmetprojection"));
            MiddleFingerProjectionTexture = VCCATAssetBundle.LoadAsset<Texture2D>("Assets/Texture2D/MiddleFinger.png");
            SnowmanProjectionTexture = VCCATAssetBundle.LoadAsset<Texture2D>("Assets/Texture2D/Snowman2x2.png");



            // Register MF GUID.
            MiddleFingerProjectionGUID = Registration.GenerateAndRegisterGUID(MiddleFingerProjectionUniqueString);
            BepinPlugin.Log.LogInfo($"Generated Helmet ProjectionGUID {MiddleFingerProjectionGUID}");

            // Create Projection Cosmetic (The helmet projection 'asset') and load/assign asset.
            ProjectionCosmetic MFProjectionCosmetic = ProjectionCosmetics.CreateProjectionAsset(MiddleFingerProjectionGUID, MiddleFingerProjectionTexture);

            // Create ContextInfo.
            ContextInfo MFContext = ContextInfo.Create(MFProjectionCosmetic.CreateIcon(), "Ancient Greeting", "While researching historic archives, we found extensive usage the balled fist with digitus medius extended. We beleive the wealthy class used this sign when gifting poultry to the poor.");

            // Create asset def, ref, for later registration.
            ProjectionCosmeticDef MFprojectionDef = ProjectionCosmetics.CreateProjectionAssetDef(MiddleFingerProjectionGUID, MFProjectionCosmetic, MFContext);

            // Attempt register asset with cosmetic container.
            bool success = ProjectionCosmeticContainer.Instance.TryRegisterAsset(MFprojectionDef);
            BepinPlugin.Log.LogInfo($"{(success ? "Succesfully registered" : "Failed to register")} MF helmet projection");

            // Create unlockable options.
            UnlockOptions UOAlways = new UnlockOptions();
            UOAlways.UnlockCriteria = UnlockCriteriaType.Always;

            UnlockItemDef MFUID = Unlocks.CreateUnlockItemDef(MiddleFingerProjectionGUID, UOAlways, RarityType.Common);

            UnlockContainer.Instance.TryRegisterAsset(MFUID);



            // Register Snowman GUID.
            SnowmanProjectionGUID = Registration.GenerateAndRegisterGUID(SnowmanUniqueString);

            BepinPlugin.Log.LogInfo($"Generated Helmet ProjectionGUID {SnowmanProjectionGUID}");

            List<AnimationModifier> SnowmanAnimationModifiers = new() { 
                new TextureSheetAnimationModifier() { 
                    columnImages = 2, 
                    rowImages = 2, targetFPS = 1, 
                    lingerFrameIndex = 3, 
                    lingerDuration = 4 
                } 
            };

            // Create Projection Cosmetic (The helmet projection 'asset') and load/assign asset.
            ProjectionCosmetic SnowmanProjectionCosmetic = ProjectionCosmetics.CreateProjectionAsset(SnowmanProjectionGUID, SnowmanProjectionTexture, SnowmanAnimationModifiers);

            // Create Context Info
            ContextInfo SnowmanContext = ContextInfo.Create(SnowmanProjectionCosmetic.CreateIcon(), "Snowman", "Building holiday cheer!");

            // Create asset def, ref, for later registration.
            ProjectionCosmeticDef SnowmanProjectionDef = ProjectionCosmetics.CreateProjectionAssetDef(SnowmanProjectionGUID, SnowmanProjectionCosmetic, SnowmanContext);

            // Attempt register asset with cosmetic container.
            success = ProjectionCosmeticContainer.Instance.TryRegisterAsset(SnowmanProjectionDef)
            BepinPlugin.Log.LogInfo($"{(success ? "Succesfully registered" : "Failed to register")} snowman helmet projection");

            // Create UnlockItem Def/Ref
            UnlockItemDef SnowmanProjectionUnlockDef = Unlocks.CreateUnlockItemDef(SnowmanProjectionGUID, UOAlways, RarityType.Common);

            UnlockContainer.Instance.TryRegisterAsset(SnowmanProjectionUnlockDef);
        }

        public override MultiplayerType MPType => MultiplayerType.All;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;
    }
}