using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen_SetUp : MonoBehaviour
{
    public ScreenCount screenCount = ScreenCount.One_Player;
    public GameObject cameraPrefab;
    public GameObject playerUI;
    public bool setUpOnStart = false;

    PlayerManager pm;
    List<GameObject> cameras = new List<GameObject>();
    List<PlayerUI_Display> playerUIs = new List<PlayerUI_Display>();

    private void Start()
    {
        if (setUpOnStart)
            LoadScreen((int)screenCount);
    }

    public void UpdateCount(int newCount)
    {
        switch (newCount)
        {
            case 1:
                screenCount = ScreenCount.One_Player;
                break;
            case 2:
                screenCount = ScreenCount.Two_Player;
                break;
            case 3:
                screenCount = ScreenCount.Three_Player;
                break;
            case 4:
                screenCount = ScreenCount.Four_Player;
                break;
        }
    }

    public void SetUp()
    {
        pm = GameManager.GetPlayerManager();
        ClearOldCameras();
        LoadScreen(pm.activePlayers.Count);
    }

    void LoadScreen(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newCam = CreateNewCamera(i);
            cameras.Add(newCam);
            CreatePlayerCamera(i, newCam);
        }

        if(count == 3)
            cameras.Add(CreateNewCamera(3));

        SetUpUI();

        switch (count)
        {
            case (int)ScreenCount.Two_Player:
                SetUpCamera(ScreenPosition.Top_Half, cameras[0].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Bottom_Half, cameras[1].GetComponent<FixedCamera>());
                break;

            case (int)ScreenCount.Three_Player:
                SetUpCamera(ScreenPosition.Top_Left, cameras[0].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Top_Right, cameras[1].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Bottom_Left, cameras[2].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Bottom_Right, cameras[3].GetComponent<FixedCamera>());
                ChangeCameraToAction(cameras[3]);
                break;

            case (int)ScreenCount.Four_Player:
                SetUpCamera(ScreenPosition.Top_Left, cameras[0].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Top_Right, cameras[1].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Bottom_Left, cameras[2].GetComponent<FixedCamera>());
                SetUpCamera(ScreenPosition.Bottom_Right, cameras[3].GetComponent<FixedCamera>());
                break;
        }

        SetUpTargets();
        SetUpFov();
    }

    void ClearOldCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            Destroy(cameras[i]);
        }

        cameras.Clear();
    }

    void ClearUI()
    {
        for (int i = 0; i < playerUIs.Count; i++)
        {
            Destroy(playerUIs[i].gameObject);
        }

        playerUIs.Clear();
    }

    GameObject CreateNewCamera(int id)
    {
        GameObject clone = Instantiate(cameraPrefab);
        clone.name = "Camera | " + id;

        clone.transform.parent = transform;
        return clone;
    }

    void CreatePlayerCamera(int id, GameObject cameraObject)
    {
        pm.activePlayers[id].cam = cameraObject.GetComponent<FixedCamera>();
    }

    void SetUpUI()
    {
        for (int i = 0; i < pm.activePlayers.Count; i++)
        {
            GameObject clone = Instantiate(playerUI);
            PlayerUI_Display pui = clone.GetComponent<PlayerUI_Display>();

            if(pui != null)
            {
                FixedCamera fCam = cameras[i].GetComponent<FixedCamera>();

                pui.UpdatePlayerClass(pm.activePlayers[i]);
                pui.UpdateCamera(fCam.uiCam);
                pm.activePlayers[i].uiManager = pui;
                playerUIs.Add(pui);
            }
        }
    }

    void SetUpCamera(ScreenPosition screenPosition, FixedCamera cam)
    {
        switch (screenPosition)
        {
            case ScreenPosition.Top_Half:
                cam.viewCam.rect = new Rect(0, .5f, 1, .5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Bottom_Half:
                cam.viewCam.rect = new Rect(0,0,1,.5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Top_Left:
                cam.viewCam.rect = new Rect(0,.5f,.5f,.5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Top_Right:
                cam.viewCam.rect = new Rect(.5f,.5f,.5f,.5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Bottom_Left:
                cam.viewCam.rect = new Rect(0,0,.5f,.5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Bottom_Right:
                cam.viewCam.rect = new Rect(.5f, 0, .5f, .5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Top_Center:
                cam.viewCam.rect = new Rect(.25f, .5f, .5f, .5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;

            case ScreenPosition.Bottom_Center:
                cam.viewCam.rect = new Rect(.25f, 0, .5f, .5f);
                cam.uiCam.rect = cam.viewCam.rect;
                break;
        }
    }

    void SetUpTargets()
    {
        for (int i = 0; i < pm.activePlayers.Count; i++)
        {
            pm.activePlayers[i].activeCamera = cameras[i];
            FixedCamera cam = cameras[i].GetComponent<FixedCamera>();
            cam.UpdatePlayerInstance(pm.activePlayers[i]);
            cam.SetTarget(pm.activePlayers[i].activePlayerPrefab.GetComponent<ChairMotor>().spaceTracker);
            cam.camData = pm.activePlayers[i].characterProfile.camData;
        }
    }

    void SetUpFov()
    {
        for (int i = 0; i < pm.activePlayers.Count; i++)
        {
            pm.activePlayers[i].activeCamera = cameras[i];
            Camera cam = cameras[i].GetComponent<Camera>();

            CameraFOV camfov = cameras[i].AddComponent<CameraFOV>();
            camfov.cam = cam;
            pm.activePlayers[i].camFOV = camfov;

            pm.activePlayers[i].activePlayerPrefab.GetComponent<PlayerController>().camFov = camfov;
        }
    }

    public void ClearAll()
    {
        ClearOldCameras();
        ClearUI();
    }

    public void ChangeCameraToAction(GameObject cam)
    {
        FixedCamera fixedCam = cam.GetComponent<FixedCamera>();

        if (fixedCam != null)
            fixedCam.enabled = false;

        cam.AddComponent<CameraController>();
        cam.AddComponent<ActionCamera>();
    }
}
