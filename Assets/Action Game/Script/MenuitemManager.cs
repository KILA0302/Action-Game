using UnityEngine;
using UnityEditor;

namespace Menu
{
    public class MenuitemManager
    {
        [MenuItem("TestScene/Change CharactorMovement", false, 8)]
        public static void OpenServerConnectSetting()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Action Game/Scenes/Charactor Movement.unity");
            EditorApplication.isPlaying = false;
        }
    }
}
