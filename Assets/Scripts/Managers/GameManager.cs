using UnityEngine;
using VRHearthLike.Core;
using VRHearthLike.Gameplay;
using VRHearthLike.Cards;

namespace VRHearthLike.Managers
{

	public class GameManager : MonoBehaviour
	{
		[Header("References")]
		public VRPlayer player;
		public VRPlayer ai;
		public DeckList playerDeckList;
		public DeckList aiDeckList; // CORRECTED: Fixed typo from 'allocKList'
		public TurnManager turnManager;
		
		private readonly Deck playerDeck = new Deck();
		private readonly Deck aiRuntimeDeck = new Deck(); // CORRECTED: Fixed typo from 'allocKRuntime'
		
		public void Initialize()
		{
			// Initialize player
			if (player != null)
			{
				player.maxHealth = GameConfig.HeroMaxHealth;
				player.currentHealth = GameConfig.HeroMaxHealth;
				player.maxMana = GameConfig.MaxMana;
				player.currentMana = 0;
				player.manaThisTurn = 0;
				if (player.healthDisplay != null)
					player.healthDisplay.UpdateHealth(player.currentHealth, player.maxHealth);
				if (player.manaCrystal != null)
					player.manaCrystal.UpdateMana(player.currentMana, player.maxMana);
			}
			
			// Initialize the deck
			playerDeck.InitializeFromDeckList(playerDeckList);
			playerDeck.Shuffle();
			aiRuntimeDeck.InitializeFromDeckList(aiDeckList); // CORRECTED
			aiRuntimeDeck.Shuffle();
			
			// Opening hand draw
			for (int i = 0; i < GameConfig.InitialHandSize; i++)
			{
				if (player != null) player.DrawCard();
				if (ai != null) ai.DrawCard();
			}
			
			// Start the round
			if (turnManager != null)
			{
				turnManager.player = player;
				turnManager.ai = ai;
				Side first = (Random.Range(0, 2) == 0) ? Side.Player : Side.AI;
				turnManager.BeginFirstTurn(first);
			}
		}
		

		public void BeginNewMatch(bool playerStarts)
		{
			Initialize();
		}
	}
}


