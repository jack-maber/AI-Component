using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    public Text displayName;
    public Image displayImage;
    public Image baseImage;

    string sceneToLoad = "";
    Button btn;

    public void LoadButton(Map_Data mapDat)
    {
        btn = GetComponent<Button>();

        displayName.text = mapDat.displayName;
        sceneToLoad = mapDat.sceneName;
        displayImage.overrideSprite = mapDat.displayImage;

        btn.onClick.AddListener(Pressed);
    }

    public void Pressed()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
