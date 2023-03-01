using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace PutAndHelp.DebugUI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class SceneDropdown : MonoBehaviour
    {
        TMP_Dropdown _dropdown;
        void Start()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.options.Clear();

            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (i == currentSceneIndex)
                {
                    _dropdown.options.Insert(0, new TMP_Dropdown.OptionData("Current Scene"));
                    continue;
                }
                
                string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
                var dropdownData = new TMP_Dropdown.OptionData(sceneName);
                _dropdown.options.Add(dropdownData);
               
            }
            _dropdown.onValueChanged.AddListener(index =>
            {
                int targetSceneIndex; 
                if (index + 1 <= currentSceneIndex)
                {
                    targetSceneIndex = index;
                }else
                {
                    targetSceneIndex = index + 1;
                }

                Debug.Log($"Scene  {targetSceneIndex} loaded");
                SceneManager.LoadScene(targetSceneIndex);
            });
            
            
        }

    }
}
