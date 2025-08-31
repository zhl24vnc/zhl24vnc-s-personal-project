using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction.Input;


public class SimpleSceneSwitcher : MonoBehaviour
{
    [Header("基本设置")]
    [SerializeField] private KeyCode switchKey = KeyCode.Space;
    [SerializeField] private bool enableDebugInfo = true;
    
    [Header("VR控制器设置")]
    [SerializeField] private bool useVRController = true;
    [SerializeField] private bool useRightController = true; // true为右手柄，false为左手柄
    
    void Update()
    {
        bool shouldSwitch = false;
        
        if (useVRController)
        {
            // 检查VR控制器A键
            shouldSwitch = CheckVRControllerAButton();
        }
        else
        {
            // 检查键盘按键
            shouldSwitch = Input.GetKeyDown(switchKey);
        }
        
        if (shouldSwitch)
        {
            SwitchToNextScene();
        }
    }
    

    private bool CheckVRControllerAButton()
    {
        try
        {
            // 尝试使用OVR输入系统
            if (useRightController)
            {
                // 右手柄A键
                return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
            }
            else
            {
                // 左手柄A键 (X键)
                return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
            }
        }
        catch (System.Exception e)
        {
            // 如果OVR输入不可用，尝试使用Unity的XR输入
            Debug.LogWarning($"OVR输入不可用，尝试使用Unity XR输入: {e.Message}");
            return CheckUnityXRInput();
        }
    }
    

    private bool CheckUnityXRInput()
    {
        try
        {
            // 使用Unity的XR输入系统
            if (useRightController)
            {
                // 右手柄A键
                return Input.GetKeyDown(KeyCode.JoystickButton0);
            }
            else
            {
                // 左手柄A键
                return Input.GetKeyDown(KeyCode.JoystickButton2);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unity XR输入检查失败: {e.Message}");
            return false;
        }
    }
    

    void SwitchToNextScene()
    {
        // 获取当前场景索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // 获取总场景数
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        
        // 计算下一个场景索引（循环切换）
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes;
        
        if (enableDebugInfo)
        {
            Debug.Log($"从场景 {currentSceneIndex} 切换到场景 {nextSceneIndex}");
        }
        
        // 加载下一个场景
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    void OnGUI()
    {
        if (!enableDebugInfo)
            return;
            
        // 显示简单的调试信息
        GUILayout.BeginArea(new Rect(10, 10, 200, 80));
        GUILayout.BeginVertical("box");
        
        if (useVRController)
        {
            string controllerSide = useRightController ? "右手柄" : "左手柄";
            GUILayout.Label($"{controllerSide} A键切换场景");
        }
        else
        {
            GUILayout.Label($"按 {switchKey} 切换场景");
        }
        GUILayout.Label($"当前场景: {SceneManager.GetActiveScene().name}");
        GUILayout.Label($"场景索引: {SceneManager.GetActiveScene().buildIndex}");
        GUILayout.Label($"VR控制器: {(useVRController ? "启用" : "禁用")}");
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

