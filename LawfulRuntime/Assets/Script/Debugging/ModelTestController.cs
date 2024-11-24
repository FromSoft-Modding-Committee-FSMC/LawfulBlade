using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lawful.Resource;

public class ModelTestController : MonoBehaviour
{
    [Header("References (External)")]
    [SerializeField] GameObject modelObject;
    [SerializeField] MeshFilter modelMeshFilter;
    [SerializeField] MeshRenderer modelMeshRenderer;

    [Header("Text Fields")]
    [SerializeField] TextMeshProUGUI modelCountField;
    [SerializeField] TextMeshProUGUI modelInfoField;

    Quaternion targetRotation;

    static string gameDataPath = Path.Combine(ResourceManager.GamePath, "DATA");

    List<string> combinedModelFiles;
    string[] mdoModelFiles;
    string[] mdlModelFiles;
    string[] msmModelFiles;
    string[] mhmModelFiles;

    int currentModelIndex = 0;
    ulong currentModelName;
    ModelResource currentModelResource;

    void Awake()
    {
        targetRotation = modelObject.transform.rotation;

        // Find our model files...
        mdoModelFiles = Directory.GetFiles(gameDataPath, "*.mdo", SearchOption.AllDirectories);
        mdlModelFiles = Directory.GetFiles(gameDataPath, "*.mdl", SearchOption.AllDirectories);
        msmModelFiles = Directory.GetFiles(gameDataPath, "*.msm", SearchOption.AllDirectories);
        mhmModelFiles = Directory.GetFiles(gameDataPath, "*.mhm", SearchOption.AllDirectories);

        combinedModelFiles = new List<string>();
        combinedModelFiles.AddRange(mdoModelFiles);
        combinedModelFiles.AddRange(mdlModelFiles);
        combinedModelFiles.AddRange(msmModelFiles);
        combinedModelFiles.AddRange(mhmModelFiles);

        modelCountField.text = $"MODEL COUNT: [MDO = {mdoModelFiles.Length}, MDL = {mdlModelFiles.Length}, MSM = {msmModelFiles.Length}, MHM = {mhmModelFiles.Length}]";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            targetRotation = modelObject.transform.rotation;

        // If the left mouse button is held down, we want to track it's moved position...
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePositionDelta.sqrMagnitude != 0)
            {
                Vector3 rotByMouse = new Vector3(Input.mousePositionDelta.y, -Input.mousePositionDelta.x, 0);
                targetRotation = (Quaternion.Euler(rotByMouse) * targetRotation);
            }
        }

        modelObject.transform.rotation = Quaternion.RotateTowards(modelObject.transform.rotation, targetRotation, 5.625f / 2f);
    }

    public void OnNextModel() =>
        currentModelIndex = (++currentModelIndex) % combinedModelFiles.Count;

    public void OnPreviousModel() =>
        currentModelIndex = (--currentModelIndex) % combinedModelFiles.Count;

    public void OnLoadModel()
    {
        string modelPath = Path.GetRelativePath(ResourceManager.GamePath, combinedModelFiles[currentModelIndex]);
        ResourceManager.LoadAsync<ModelResource>(modelPath, OnLoadModelComplete);
    }

    void OnLoadModelComplete(ulong resourceName)
    {
        currentModelName     = resourceName;
        currentModelResource = ResourceManager.Get<ModelResource>(resourceName);
        
        modelMeshFilter.mesh = currentModelResource.Get();

        // Load Materials per mesh
        Material[] meshMats = new Material[modelMeshFilter.mesh.subMeshCount];
        for (int i = 0; i < meshMats.Length; ++i)
            meshMats[i] = modelMeshRenderer.material;
        modelMeshRenderer.materials = meshMats;

        modelInfoField.text = $"CURRENT MODEL NAME: {Path.GetFileName(combinedModelFiles[currentModelIndex])}\n\n" +
            $"MESH NUM = {modelMeshFilter.mesh.subMeshCount}\nVERT NUM = {modelMeshFilter.mesh.vertices.Length}\nFACE NUM = {modelMeshFilter.mesh.triangles.Length}";
    }
}
