using UnityEditor;
using UnityEngine;

namespace RealityEngine.EditorTools
{
    /// <summary>
    /// One-shot after compile: force Place Giza Complex (rebuild + hide meadow), then self-delete.
    /// If this stalls: Assets → Refresh, then Reality Engine → Place Giza Complex.
    /// </summary>
    [InitializeOnLoad]
    static class PlaceGizaOnce
    {
        static PlaceGizaOnce()
        {
            EditorApplication.delayCall += Run;
        }

        static void Run()
        {
            try
            {
                LabLandscapeMenu.PlaceGizaComplex();
                Debug.Log(
                    "PlaceGizaOnce: PlaceGizaComplex force-rebuild done. Meadow/Terrain/Trees hidden; " +
                    "GizaPlateau + desert Oasis sand; MountainScene root left on. Self-deleting.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("PlaceGizaOnce failed (run Reality Engine -> Place Giza Complex manually): " + e);
            }
            finally
            {
                DeleteSelf();
            }
        }

        static void DeleteSelf()
        {
            string[] guids = AssetDatabase.FindAssets("PlaceGizaOnce t:MonoScript");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (string.IsNullOrEmpty(path))
                    continue;
                if (!path.EndsWith("PlaceGizaOnce.cs", System.StringComparison.OrdinalIgnoreCase))
                    continue;
                if (AssetDatabase.DeleteAsset(path))
                    Debug.Log("PlaceGizaOnce: deleted " + path);
                else
                    Debug.LogWarning("PlaceGizaOnce: could not delete " + path + " — delete manually after Place Giza Complex.");
                return;
            }
            Debug.LogWarning("PlaceGizaOnce: script asset not found to self-delete.");
        }
    }
}
