using BepInEx;
using BepInEx.Logging;
using CG.Client.PlayerData;
using CG.Client.UserData;
using Client.Player.Interactions;
using HarmonyLib;
using ResourceAssets;
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

            // register GUID.
            MiddleFingerProjectionGUID = Registration.GenerateAndRegisterGUID(MiddleFingerProjectionUniqueString);

            BepinPlugin.Log.LogInfo($"Generated Helmet ProjectionGUID {MiddleFingerProjectionGUID}");

            // Create asset def, ref, for later registration.
            ProjectionCosmeticDef MFprojectionDef = new ProjectionCosmeticDef();
            ProjectionCosmeticRef MFprojectionRef = new ProjectionCosmeticRef(new ResourceAssetRef(MiddleFingerProjectionGUID, string.Empty));
            MFprojectionRef._cachedPathGuid = MiddleFingerProjectionGUID;
            MFprojectionDef.Ref = MFprojectionRef;

            // Create Projection Cosmetic (The helmet projection 'asset') and load/assign asset.
            ProjectionCosmetic MFprojectionCosmetic = ScriptableObject.CreateInstance<ProjectionCosmetic>();
            MFprojectionCosmetic.assetGuid = MiddleFingerProjectionGUID;
            MFprojectionCosmetic.animationModifiers = new(); // Needs to be an empty, non null list.
            MFprojectionCosmetic.ProjectionTexture = MiddleFingerProjectionTexture;

            ProjectionCosmeticDef AssetDef = VoidManager.Content.ProjectionCosmentics.CreateProjectionAssetDef(MiddleFingerProjectionGUID, projectionCosmetic, contextInfo);

            // Attempt register asset with cosmetic container.
            if (ProjectionCosmeticContainer.Instance.TryRegisterAsset(AssetDef))
            {
                BepinPlugin.Log.LogInfo("Succesfully registered helmet projection");
            }
            else
            {
                BepinPlugin.Log.LogError("Failed to register helmet projection");
            }

            // Assign asset to ref.
            MFprojectionRef.ResourceAsset = MFprojectionCosmetic;

            // Create Context Info
            MFprojectionDef.ContextInfo = ContextInfo.Create(MFprojectionCosmetic.CreateIcon(), "Ancient Greeting", "While researching historic archives, we found extensive usage the balled fist with digitus medius extended. We beleive the wealthy class used this sign when gifting poultry to the poor.");
            
            // Attempt register asset with cosmetic container.
            if (ProjectionCosmeticContainer.Instance.TryRegisterAsset(MFprojectionDef))
            {
                BepinPlugin.Log.LogInfo("Succesfully registered helmet projection");
            }
            else
            {
                BepinPlugin.Log.LogError("Failed to register helmet projection");
            }

            //Unlock code

            // Create unlockable options.
            UnlockOptions UO = new UnlockOptions();
            UO.UnlockCriteria = UnlockCriteriaType.Always;

            // Create UnlockItem Def/Ref
            UnlockItemDef MFprojectionUnlockDef = new UnlockItemDef();
            UnlockItemRef MFprojectionUnlockRef = new UnlockItemRef(MiddleFingerProjectionGUID, string.Empty);
            MFprojectionUnlockDef.Ref = MFprojectionUnlockRef;
            MFprojectionUnlockDef.rarity = CG.Ship.Object.RarityType.Common;
            MFprojectionUnlockDef.unlockOptions = UO;

            UnlockContainer.Instance.TryRegisterAsset(MFprojectionUnlockDef);



            // register GUID.
            SnowmanProjectionGUID = Registration.GenerateAndRegisterGUID(SnowmanUniqueString);

            BepinPlugin.Log.LogInfo($"Generated Helmet ProjectionGUID {SnowmanProjectionGUID}");

            // Create asset def, ref, for later registration.
            ProjectionCosmeticDef SnowmanProjectionDef = new ProjectionCosmeticDef();
            ProjectionCosmeticRef SnowmanprojectionRef = new ProjectionCosmeticRef(new ResourceAssetRef(SnowmanProjectionGUID, string.Empty));
            SnowmanprojectionRef._cachedPathGuid = SnowmanProjectionGUID;
            SnowmanProjectionDef.Ref = SnowmanprojectionRef;

            // Create Projection Cosmetic (The helmet projection 'asset') and load/assign asset.
            ProjectionCosmetic SnowmanProjectionCosmetic = ScriptableObject.CreateInstance<ProjectionCosmetic>();
            SnowmanProjectionCosmetic.assetGuid = SnowmanProjectionGUID;
            SnowmanProjectionCosmetic.animationModifiers = new() { new TextureSheetAnimationModifier() { columnImages = 2, rowImages = 2, targetFPS = 1, lingerFrameIndex = 3, lingerDuration = 4,  } }; // Needs to be an empty, non null list.
            SnowmanProjectionCosmetic.ProjectionTexture = SnowmanProjectionTexture;

            // Assign asset to ref.
            SnowmanprojectionRef.ResourceAsset = SnowmanProjectionCosmetic;

            // Create Context Info
            SnowmanProjectionDef.ContextInfo = ContextInfo.Create(SnowmanProjectionCosmetic.CreateIcon(), "Snowman", "Building holiday cheer!");

            // Attempt register asset with cosmetic container.
            if (ProjectionCosmeticContainer.Instance.TryRegisterAsset(SnowmanProjectionDef))
            {
                BepinPlugin.Log.LogInfo("Succesfully registered helmet projection");
            }
            else
            {
                BepinPlugin.Log.LogError("Failed to register helmet projection");
            }

            //Unlock code

            // Create UnlockItem Def/Ref
            UnlockItemDef SnowmanProjectionUnlockDef = new UnlockItemDef();
            UnlockItemRef SnowmanProjectionUnlockRef = new UnlockItemRef(SnowmanProjectionGUID, string.Empty);
            SnowmanProjectionUnlockDef.Ref = SnowmanProjectionUnlockRef;
            SnowmanProjectionUnlockDef.rarity = CG.Ship.Object.RarityType.Common;
            SnowmanProjectionUnlockDef.unlockOptions = UO;

            UnlockContainer.Instance.TryRegisterAsset(SnowmanProjectionUnlockDef);
        }

        public override MultiplayerType MPType => MultiplayerType.All;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;
    }
}