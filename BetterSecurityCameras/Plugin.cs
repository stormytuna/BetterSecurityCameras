using System.Collections;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterSecurityCameras
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class BetterSecurityCameras : BaseUnityPlugin
    {
        public const string ModGUID = "stormytuna.BetterSecurityCameras";
        public const string ModName = "BetterSecurityCameras";
        public const string ModVersion = "1.0.0";

        private readonly bool appliedMonitorChanges = false;

        public static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
        public static BetterSecurityCameras Instance;

        private void Awake() {
            if (Instance is null) {
                Instance = this;
                gameObject.hideFlags = (HideFlags)61;
            }

            Log.LogInfo("Better Security Cameras has awoken!");

            DontDestroyOnLoad(this);

            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
            {
                if (scene.name != "MainMenu" && scene.name != "InitScene" && scene.name != "InitSceneLaunchOptions") {
                    StartCoroutine(OnSceneLoaded());
                }
            };
        }

        private IEnumerator OnSceneLoaded() {
            yield return new WaitForSeconds(5f);

            if (appliedMonitorChanges) {
                yield break;
            }

            RenderTexture internalCameraRt = new RenderTexture(160, 120, 32, RenderTextureFormat.ARGB32);
            GameObject internalCamera = GameObject.Find("Environment/HangarShip/Cameras/ShipCamera");
            internalCamera.GetComponent<Camera>().targetTexture = internalCameraRt;
            GameObject topMonitors = GameObject.Find("Environment/HangarShip/ShipModels2b/MonitorWall/Cube");
            topMonitors.GetComponent<MeshRenderer>().materials[2].mainTexture = internalCameraRt;

            RenderTexture externalCameraRt = new RenderTexture(1600, 1200, 32, RenderTextureFormat.ARGB32);
            GameObject externalCamera = GameObject.Find("Environment/HangarShip/Cameras/FrontDoorSecurityCam/SecurityCamera");
            externalCamera.GetComponent<Camera>().targetTexture = externalCameraRt;
            GameObject bottomMonitors = GameObject.Find("Environment/HangarShip/ShipModels2b/MonitorWall/Cube.001");
            bottomMonitors.GetComponent<MeshRenderer>().materials[2].mainTexture = externalCameraRt;
            GameObject doorMonitor = GameObject.Find("Environment/HangarShip/ShipModels2b/MonitorWall/SingleScreen");
            doorMonitor.GetComponent<MeshRenderer>().materials[1].mainTexture = externalCameraRt;
        }
    }
}
