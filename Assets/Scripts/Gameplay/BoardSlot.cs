using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public class BoardSlot : MonoBehaviour
	{
			public Minion Occupant { get; private set; }
	public bool IsEmpty { get { return Occupant == null; } }
	
	// Alias for backward compatibility
	public Minion occupiedMinion => Occupant;
	
	// Properties for backward compatibility
	public bool isPlayerSide => true; // This should be determined by the slot's position
		
		public bool SpawnMinion(CardAsset asset, VRPlayer owner, out Minion minion)
		{
			minion = null;
			if (asset == null || asset.minionPrefab == null) return false;
			
			GameObject go = Instantiate(asset.minionPrefab, transform.position, transform.rotation, transform);
			minion = go.GetComponent<Minion>();
			if (minion == null) minion = go.AddComponent<Minion>();
			minion.Initialize(asset, owner);
			Occupant = minion;
			
			// Make sure there is a target point to select from
			if (go.GetComponent<MinionTarget>() == null)
				go.AddComponent<MinionTarget>();
				
			return true;
		}
	}
}
