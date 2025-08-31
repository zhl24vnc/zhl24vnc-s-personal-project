using UnityEngine;
using Oculus.Interaction;
using VRHearthLike.Gameplay;

namespace VRHearthLike.VR
{
	/// <summary>
	/// Oculus Interaction 系统设置指南
	/// 参考: https://developers.meta.com/horizon/reference/interaction/v77/class_oculus_interaction_ray_interactor
	/// </summary>
	public class OculusInteractionSetup : MonoBehaviour
	{
		[Header("设置步骤说明")]
		[TextArea(10, 20)]
		public string setupInstructions = @"
Oculus Interaction 系统设置步骤：

1. 在 RightInteractions 上添加组件：
   - Ray Interactor
   - Poke Interactor (可选)
   - Grab Interactor (可选)

2. 配置 Ray Interactor：
   - 设置 Max Ray Length (建议: 10)
   - 设置 Ray Origin (控制器位置)
   - 设置 Ray Direction (控制器前向)

3. 为交互对象添加组件：
   - Interactable Unity Event Wrapper
   - Collider (Box Collider 或 Sphere Collider)
   - 设置正确的 Layer

4. 设置 Layer：
   - Card Layer: 8
   - Board Layer: 9  
   - Hero Layer: 10

5. 配置 Ray Interactor 的 Interaction Mask：
   - 包含 Card Layer
   - 包含 Board Layer
   - 包含 Hero Layer

6. 添加 OculusRayInteractionController 脚本到 RightInteractions

7. 设置引用：
   - Ray Interactor
   - Player Controller
   - Hand Manager
   - Board Manager
   - Turn Manager

8. 运行 SetupCardInteractables() 和 SetupBoardInteractables() 方法
";

		[Header("自动设置")]
		public bool autoSetupOnStart = true;

		private void Start()
		{
			if (autoSetupOnStart)
			{
				AutoSetup();
			}
		}

		public void AutoSetup()
		{
			SetupLayers();
			SetupRayInteractor();
			SetupInteractables();
		}

		private void SetupLayers()
		{
			// 创建必要的层
			Debug.Log("设置交互层...");
			
			// 注意：在运行时无法创建新层，需要在 Project Settings 中手动创建
			// Layer 8: Card
			// Layer 9: Board  
			// Layer 10: Hero
		}

		private void SetupRayInteractor()
		{
			var rayInteractor = GetComponent<RayInteractor>();
			if (rayInteractor == null)
			{
				rayInteractor = gameObject.AddComponent<RayInteractor>();
			}

			// 配置 Ray Interactor
			rayInteractor.MaxRayLength = 10f;
			
			// 注意：不同版本的 Oculus Interaction 可能有不同的属性名
			// 如果 InteractionMask 不存在，可以尝试其他属性
			try
			{
				// 尝试设置交互掩码（如果属性存在）
				var interactionMaskProperty = typeof(RayInteractor).GetProperty("InteractionMask");
				if (interactionMaskProperty != null)
				{
					int cardLayer = 8;
					int boardLayer = 9;
					int heroLayer = 10;
					
					LayerMask mask = (1 << cardLayer) | (1 << boardLayer) | (1 << heroLayer);
					interactionMaskProperty.SetValue(rayInteractor, mask);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning($"无法设置 InteractionMask: {e.Message}");
			}
			
			Debug.Log("Ray Interactor 设置完成");
		}

		private void SetupInteractables()
		{
			// 为手牌添加交互组件
			var handManager = FindObjectOfType<HandManager>();
			if (handManager != null)
			{
				foreach (Transform child in handManager.transform)
				{
					SetupCardInteractable(child.gameObject);
				}
			}

			// 为棋盘格子添加交互组件
			var boardManager = FindObjectOfType<BoardManager>();
			if (boardManager != null)
			{
				foreach (var slot in boardManager.PlayerSlots)
				{
					if (slot != null)
					{
						SetupBoardSlotInteractable(slot.gameObject);
					}
				}

				foreach (var slot in boardManager.EnemySlots)
				{
					if (slot != null)
					{
						SetupBoardSlotInteractable(slot.gameObject);
					}
				}
			}

			Debug.Log("交互对象设置完成");
		}

		private void SetupCardInteractable(GameObject cardObject)
		{
			// 添加 Interactable 组件
			if (cardObject.GetComponent<InteractableUnityEventWrapper>() == null)
			{
				cardObject.AddComponent<InteractableUnityEventWrapper>();
			}

			// 添加 Collider
			if (cardObject.GetComponent<Collider>() == null)
			{
				var collider = cardObject.AddComponent<BoxCollider>();
				collider.size = new Vector3(1f, 1.4f, 0.1f); // 卡牌尺寸
			}

			// 设置层
			cardObject.layer = 8; // Card layer
		}

		private void SetupBoardSlotInteractable(GameObject slotObject)
		{
			// 添加 Interactable 组件
			if (slotObject.GetComponent<InteractableUnityEventWrapper>() == null)
			{
				slotObject.AddComponent<InteractableUnityEventWrapper>();
			}

			// 添加 Collider
			if (slotObject.GetComponent<Collider>() == null)
			{
				var collider = slotObject.AddComponent<BoxCollider>();
				collider.size = new Vector3(1f, 0.1f, 1f); // 格子尺寸
			}

			// 设置层
			slotObject.layer = 9; // Board layer
		}

		[ContextMenu("手动设置")]
		public void ManualSetup()
		{
			AutoSetup();
		}

		[ContextMenu("清除设置")]
		public void ClearSetup()
		{
			// 移除 Ray Interactor
			var rayInteractor = GetComponent<RayInteractor>();
			if (rayInteractor != null)
			{
				DestroyImmediate(rayInteractor);
			}

			// 移除 Interactable 组件
			var interactables = FindObjectsOfType<InteractableUnityEventWrapper>();
			foreach (var interactable in interactables)
			{
				DestroyImmediate(interactable);
			}

			Debug.Log("设置已清除");
		}
	}
}
