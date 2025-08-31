using UnityEngine;

namespace VRHearthLike.Core
{

	public static class GameConfig
	{
		public const int MaxMana = 10; // Maximum mana per round (+1 per round, capped)
		public const int BoardSlotCount = 8; // Number of squares on a single board
		public const int InitialHandSize = 3; // Number of cards drawn in the starting hand
		public const int HeroMaxHealth = 30; // Maximum hero health
		
		// Other constants
		public const int MaxHandSize = 10; // Maximum hand size
	}
}


