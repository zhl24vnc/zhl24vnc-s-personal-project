using UnityEngine;

/// <summary>
/// VR Interaction Interface, defines basic interaction methods for VR objects
/// </summary>
public interface IVRInteractable
{
    /// <summary>
    /// Called when ray enters the object
    /// </summary>
    void OnRayEnter();
    
    /// <summary>
    /// Called when ray exits the object
    /// </summary>
    void OnRayExit();
    
    /// <summary>
    /// Called when interaction is executed
    /// </summary>
    void OnInteract();
}

/// <summary>
/// VR Interaction Component Base Class
/// Can be attached to GameObjects to enable VR interaction
/// </summary>
public class VRInteractable : MonoBehaviour, IVRInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private string interactionPrompt = "Interact";
    
    [Header("Events")]
    [SerializeField] private UnityEngine.Events.UnityEvent onRayEnter;
    [SerializeField] private UnityEngine.Events.UnityEvent onRayExit;
    [SerializeField] private UnityEngine.Events.UnityEvent onInteract;
    
    public bool IsInteractable => isInteractable;
    public string InteractionPrompt => interactionPrompt;
    
    /// <summary>
    /// Called when ray enters the object
    /// </summary>
    public virtual void OnRayEnter()
    {
        if (!isInteractable) return;
        
        onRayEnter?.Invoke();
        Debug.Log($"Ray entered: {gameObject.name}");
    }
    
    /// <summary>
    /// Called when ray exits the object
    /// </summary>
    public virtual void OnRayExit()
    {
        if (!isInteractable) return;
        
        onRayExit?.Invoke();
        Debug.Log($"Ray exited: {gameObject.name}");
    }
    
    /// <summary>
    /// Called when interaction is executed
    /// </summary>
    public virtual void OnInteract()
    {
        if (!isInteractable) return;
        
        onInteract?.Invoke();
        Debug.Log($"Interaction executed: {gameObject.name}");
    }
    
    /// <summary>
    /// Set whether the object is interactable
    /// </summary>
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
    }
    
    /// <summary>
    /// Set the interaction prompt text
    /// </summary>
    public void SetInteractionPrompt(string prompt)
    {
        interactionPrompt = prompt;
    }
}
