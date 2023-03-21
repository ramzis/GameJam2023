using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Finder
{
    public static T FindInterface<T>()
    {
        T result;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                result = root.GetComponentInChildren<T>();
                if (result != null) return result;
            }
        }

        return default;
    }

    public static List<T> FindInterfaces<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                results.AddRange(root.GetComponentsInChildren<T>());
            }
        }

        return results ;
    }
}
