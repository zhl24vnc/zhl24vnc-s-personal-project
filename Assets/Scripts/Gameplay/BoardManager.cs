using System.Collections.Generic;
using UnityEngine;
using VRHearthLike.Core;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public class BoardManager : MonoBehaviour
	{
		[Header("Player Side Slots (Left to Right - 8 slots)")]
		public BoardSlot[] playerSlots = new BoardSlot[GameConfig.BoardSlotCount];
		
		[Header("AI Side Slots (Left to Right - 8 slots)")]
		public BoardSlot[] aiSlots = new BoardSlot[GameConfig.BoardSlotCount];
		
		// Properties for backward compatibility
		public IReadOnlyList<BoardSlot> PlayerSlots => playerSlots;
		public IReadOnlyList<BoardSlot> EnemySlots => aiSlots;
		
		// Finds the first empty slot on the specified side
		public bool TryGetFirstEmptySlot(Side side, out BoardSlot slot)
		{
			BoardSlot[] slots = side == Side.Player ? playerSlots : aiSlots;
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i] != null && slots[i].IsEmpty)
				{
					slot = slots[i];
					return true;
				}
			}
			slot = null;
			return false;
		}
		
		// Attempts to place a minion on the board for the specified side
		public bool TryPlaceMinion(Side side, CardAsset asset, VRPlayer owner, out Minion minion)
		{
			minion = null;
			if (asset == null || asset.minionPrefab == null || owner == null) return false;
			if (!TryGetFirstEmptySlot(side, out BoardSlot slot)) return false;
			return slot.SpawnMinion(asset, owner, out minion);
		}
		
		// Method for backward compatibility
		public bool TryPlaceMinion(bool isPlayerSide, int slotIndex, GameObject minionPrefab, int attack, int health, out Minion minion)
		{
			minion = null;
			if (slotIndex < 0 || slotIndex >= GameConfig.BoardSlotCount) return false;
			
			BoardSlot[] slots = isPlayerSide ? playerSlots : aiSlots;
			if (slots[slotIndex] == null || !slots[slotIndex].IsEmpty) return false;
			
			// Create a temporary CardAsset for the minion
			var tempAsset = ScriptableObject.CreateInstance<CardAsset>();
			tempAsset.attack = attack;
			tempAsset.maxHealth = health;
			tempAsset.minionPrefab = minionPrefab;
			
			// Find the owner (this is a simplified approach)
			VRPlayer owner = FindObjectOfType<VRPlayer>();
			
			return slots[slotIndex].SpawnMinion(tempAsset, owner, out minion);
		}
		
		// Method for backward compatibility
		public int GetIndexOfSlot(BoardSlot slot, bool isPlayerSide)
		{
			BoardSlot[] slots = isPlayerSide ? playerSlots : aiSlots;
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i] == slot) return i;
			}
			return -1;
		}
	}
}


