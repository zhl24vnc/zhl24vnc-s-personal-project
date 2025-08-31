using UnityEngine;
using System.Collections.Generic;
using VRHearthLike.Core;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{

	public class VRPlayer : MonoBehaviour
	{
		[Header("Player Identity")]
		public Side side;
		public string playerName = "Player";
		
		[Header("Stats")]
		public int maxHealth = GameConfig.HeroMaxHealth;
		public int currentHealth;
		public int maxMana = GameConfig.MaxMana;
		public int currentMana;
		public int manaThisTurn;
		
		[Header("Game State")]
		public List<CardAsset> hand = new List<CardAsset>();
		public List<Minion> minionsOnTable = new List<Minion>();
		
			[Header("UI References")]
	public UI.HealthDisplay healthDisplay;
	public UI.ManaCrystal manaCrystal;
	
	[Header("Components")]
	public ManaManager manaManager;
	public Hero hero;
	
	[Header("Deck")]
	public DeckList deckList;
	private Deck deck;
		

		public void Initialize()
		{
			currentHealth = maxHealth;
			currentMana = 0;
			manaThisTurn = 0;
			
			// Initialize components
			if (manaManager == null)
				manaManager = GetComponent<ManaManager>();
			if (hero == null)
				hero = GetComponent<Hero>();
			
			// Initialize hero
			if (hero != null)
			{
				hero.owner = this;
				hero.currentHealth = currentHealth;
				hero.maxHealth = maxHealth;
			}
			
			// Initialize mana manager
			if (manaManager != null)
			{
				manaManager.currentMana = currentMana;
				manaManager.maxMana = maxMana;
				manaManager.manaThisTurn = manaThisTurn;
			}
			
			// Initialize deck
			deck = new Deck();
			deck.InitializeFromDeckList(deckList);
			deck.Shuffle();
			
			// Update UI
			UpdateHealthDisplay();
			UpdateManaDisplay();
		}
		

		public virtual void StartTurn()
		{
			// Increase mana
			currentMana = Mathf.Min(maxMana, currentMana + 1);
			manaThisTurn = currentMana;
			
			// Update mana manager
			if (manaManager != null)
			{
				manaManager.ResetForNewTurn();
			}
			
			// Draw a card
			DrawCard();
			
			// Enable minions to attack
			foreach (var minion in minionsOnTable)
			{
				if (minion != null)
				{
					minion.OnTurnStart();
				}
			}
			
			UpdateManaDisplay();
		}
		

		public void DrawCard()
		{
			if (deck.TryDraw(out CardAsset card))
			{
				if (hand.Count < GameConfig.MaxHandSize)
				{
					hand.Add(card);
					OnCardDrawn(card);
				}
				else
				{
					// Card is burned (discarded)
					OnCardBurned(card);
				}
			}
			else
			{
				// Fatigue damage - no cards left in deck
				TakeDamage(1);
			}
		}
		

		public bool CanPlayCard(CardAsset card)
		{
			return card != null && currentMana >= card.manaCost;
		}
		

		public void PlayCard(CardAsset card, Vector3 position)
		{
			if (!CanPlayCard(card)) return;
			
			// Spend mana
			currentMana -= card.manaCost;
			
			// Remove from hand
			hand.Remove(card);
			
			// Play the card effect
			OnCardPlayed(card, position);
			
			UpdateManaDisplay();
		}
		

		public void TakeDamage(int amount)
		{
			currentHealth = Mathf.Max(0, currentHealth - amount);
			
			// Update hero
			if (hero != null)
			{
				hero.currentHealth = currentHealth;
			}
			
			UpdateHealthDisplay();
			
			if (currentHealth <= 0)
			{
				OnPlayerDefeated();
			}
		}
		

		public void Heal(int amount)
		{
			currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
			UpdateHealthDisplay();
		}
		

		private void UpdateHealthDisplay()
		{
			if (healthDisplay != null)
			{
				healthDisplay.UpdateHealth(currentHealth, maxHealth);
			}
		}
		

		private void UpdateManaDisplay()
		{
			if (manaCrystal != null)
			{
				manaCrystal.UpdateMana(currentMana, maxMana);
			}
		}
		
		#region Event Handlers

		protected virtual void OnCardDrawn(CardAsset card)
		{
			Debug.Log($"{playerName} drew: {card.cardName}");
		}
		

		protected virtual void OnCardBurned(CardAsset card)
		{
			Debug.Log($"{playerName} burned: {card.cardName}");
		}
		

		protected virtual void OnCardPlayed(CardAsset card, Vector3 position)
		{
			Debug.Log($"{playerName} played: {card.cardName}");
		}
		

		protected virtual void OnPlayerDefeated()
		{
			Debug.Log($"{playerName} has been defeated!");
		}
		#endregion
	}
}
