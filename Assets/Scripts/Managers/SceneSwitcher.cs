using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Oculus.Interaction.Input;

/// <summary>
/// Scene Switching Manager
/// Supports cycling through different scenes by pressing the space key
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private List<string> sceneNames = new List<string>();
    [SerializeField] private bool enableSpaceKeySwitch = true;
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    
    [Header("VR Controller Settings")]
    [SerializeField] private bool useVRController = true;
    [SerializeField] private bool useRightController = true; // true for right controller, false for left controller
    
    [Header("Switch Settings")]
    [SerializeField] private float switchDelay = 0.5f; // Switch delay to prevent continuous key presses
    [SerializeField] private bool showDebugInfo = true;
    
    private int currentSceneIndex = 0;
    private float lastSwitchTime = 0f;
    private bool canSwitch = true;
    
    void Start()
    {
        // If no scene list is set, automatically get all scenes
        if (sceneNames.Count == 0)
        {
            LoadAllScenes();
        }
        
        // Set current scene index
        SetCurrentSceneIndex();
        
        if (showDebugInfo)
        {
            Debug.Log($"Scene switcher initialized with {sceneNames.Count} scenes");
            Debug.Log($"Current scene: {SceneManager.GetActiveScene().name}");
            if (useVRController)
            {
                string controllerSide = useRightController ? "right controller" : "left controller";
                Debug.Log($"Using VR controller: {controllerSide} A button to switch scenes");
            }
            else
            {
                Debug.Log($"Press {switchKey} key to switch scenes");
            }
        }
    }
    
    void Update()
    {
        if (!enableSpaceKeySwitch || !canSwitch)
            return;
            
        bool shouldSwitch = false;
        
        if (useVRController)
        {
            // Check VR controller A button
            shouldSwitch = CheckVRControllerAButton();
        }
        else
        {
            // Check keyboard key
            shouldSwitch = Input.GetKeyDown(switchKey);
        }
        
        if (shouldSwitch)
        {
            // Check switch delay
            if (Time.time - lastSwitchTime < switchDelay)
                return;
                
            SwitchToNextScene();
        }
    }
    
    /// <summary>
    /// Check if VR controller A button is pressed
    /// </summary>
    /// <returns>Whether A button is pressed</returns>
    private bool CheckVRControllerAButton()
    {
        try
        {
            // Try using OVR input system
            if (useRightController)
            {
                // Right controller A button
                return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
            }
            else
            {
                // Left controller A button (X button)
                return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
            }
        }
        catch (System.Exception e)
        {
            // If OVR input is not available, try using Unity's XR input
            Debug.LogWarning($"OVR input not available, trying Unity XR input: {e.Message}");
            return CheckUnityXRInput();
        }
    }
    
    /// <summary>
    /// Use Unity XR input system to check A button
    /// </summary>
    /// <returns>Whether A button is pressed</returns>
    private bool CheckUnityXRInput()
    {
        try
        {
            // Use Unity's XR input system
            if (useRightController)
            {
                // Right controller A button
                return Input.GetKeyDown(KeyCode.JoystickButton0);
            }
            else
            {
                // Left controller A button
                return Input.GetKeyDown(KeyCode.JoystickButton2);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unity XR input check failed: {e.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// Switch to the next scene
    /// </summary>
    public void SwitchToNextScene()
    {
        if (sceneNames.Count == 0)
        {
            Debug.LogWarning("No scenes available for switching!");
            return;
        }
        
        currentSceneIndex = (currentSceneIndex + 1) % sceneNames.Count;
        LoadScene(currentSceneIndex);
    }
    
    /// <summary>
    /// Switch to the previous scene
    /// </summary>
    public void SwitchToPreviousScene()
    {
        if (sceneNames.Count == 0)
        {
            Debug.LogWarning("No scenes available for switching!");
            return;
        }
        
        currentSceneIndex = (currentSceneIndex - 1 + sceneNames.Count) % sceneNames.Count;
        LoadScene(currentSceneIndex);
    }
    
    /// <summary>
    /// Switch to scene at specified index
    /// </summary>
    /// <param name="sceneIndex">Scene index</param>
    public void SwitchToScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= sceneNames.Count)
        {
            Debug.LogError($"Scene index {sceneIndex} is out of range!");
            return;
        }
        
        currentSceneIndex = sceneIndex;
        LoadScene(currentSceneIndex);
    }
    
    /// <summary>
    /// Switch to scene with specified name
    /// </summary>
    /// <param name="sceneName">Scene name</param>
    public void SwitchToScene(string sceneName)
    {
        int index = sceneNames.IndexOf(sceneName);
        if (index == -1)
        {
            Debug.LogError($"Scene not found: {sceneName}");
            return;
        }
        
        SwitchToScene(index);
    }
    
    /// <summary>
    /// Load scene
    /// </summary>
    /// <param name="sceneIndex">Scene index</param>
    private void LoadScene(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= sceneNames.Count)
            return;
            
        string sceneName = sceneNames[sceneIndex];
        
        if (showDebugInfo)
        {
            Debug.Log($"Switching to scene: {sceneName} (index: {sceneIndex})");
        }
        
        lastSwitchTime = Time.time;
        canSwitch = false;
        
        // Use async scene loading
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    /// <summary>
    /// Automatically load all scenes
    /// </summary>
    private void LoadAllScenes()
    {
        sceneNames.Clear();
        
        // Get all scenes
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"Automatically loaded {sceneNames.Count} scenes");
        }
    }
    
    /// <summary>
    /// Set current scene index
    /// </summary>
    private void SetCurrentSceneIndex()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneIndex = sceneNames.IndexOf(currentSceneName);
        
        if (currentSceneIndex == -1)
        {
            currentSceneIndex = 0;
            Debug.LogWarning($"Current scene {currentSceneName} is not in the scene list, setting to first scene");
        }
    }
    
    /// <summary>
    /// Enable/disable scene switching
    /// </summary>
    /// <param name="enabled">Whether to enable</param>
    public void SetSceneSwitchingEnabled(bool enabled)
    {
        enableSpaceKeySwitch = enabled;
        canSwitch = enabled;
    }
    
    /// <summary>
    /// Get current scene index
    /// </summary>
    /// <returns>Current scene index</returns>
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }
    
    /// <summary>
    /// Get current scene name
    /// </summary>
    /// <returns>Current scene name</returns>
    public string GetCurrentSceneName()
    {
        if (currentSceneIndex >= 0 && currentSceneIndex < sceneNames.Count)
        {
            return sceneNames[currentSceneIndex];
        }
        return SceneManager.GetActiveScene().name;
    }
    
    /// <summary>
    /// Get total number of scenes
    /// </summary>
    /// <returns>Total number of scenes</returns>
    public int GetSceneCount()
    {
        return sceneNames.Count;
    }
    
    void OnGUI()
    {
        if (!showDebugInfo)
            return;
            
        // Display debug information
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.BeginVertical("box");
        
        if (useVRController)
        {
            string controllerSide = useRightController ? "Right Controller" : "Left Controller";
            GUILayout.Label($"Scene Switcher - {controllerSide} A Button Switch");
        }
        else
        {
            GUILayout.Label($"Scene Switcher - Press {switchKey} to Switch");
        }
        GUILayout.Label($"Current Scene: {GetCurrentSceneName()} ({currentSceneIndex + 1}/{sceneNames.Count})");
        GUILayout.Label($"Switch Status: {(enableSpaceKeySwitch ? "Enabled" : "Disabled")}");
        GUILayout.Label($"VR Controller: {(useVRController ? "Enabled" : "Disabled")}");
        
        if (GUILayout.Button("Next Scene"))
        {
            SwitchToNextScene();
        }
        
        if (GUILayout.Button("Previous Scene"))
        {
            SwitchToPreviousScene();
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

