using UnityEngine;

namespace VRHearthLike.Cards
{

	[CreateAssetMenu(fileName = "New Card", menuName = "VR Card Game/Card Asset", order = 1)]
	public class CardAsset : ScriptableObject
	{
			[Header("Basic Info")]
	public string cardName;
	[TextArea(2, 3)]
	public string description;
	
	// Property for backward compatibility
	public string displayName => cardName;
		public Sprite cardImage;
		public int manaCost;
		
			[Header("Minion Info")]
	public int attack;
	public int maxHealth;
	public bool hasCharge; // Can attack the turn it's played
	
	// Property for backward compatibility
	public int health => maxHealth;
		public bool hasTaunt; // Must be attacked before the enemy can target the hero
		public GameObject minionPrefab; // Prefab to spawn for this minion
		
		[Header("Spell Info")]
		public bool isSpell;
		public GameObject spellEffectPrefab; // Recommended: use a prefab instead of a script name
		
		[Header("VR Interaction")]
		public AudioClip cardSound;
		public ParticleSystem cardEffect;
	}
}
