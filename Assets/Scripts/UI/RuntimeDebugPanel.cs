using UnityEngine;
using TMPro;

namespace VRHearthLike.UI
{
	public class RuntimeDebugPanel : MonoBehaviour
	{
		public Gameplay.PlayerController player;
		public Gameplay.AIController ai;
		public Gameplay.TurnManager turnManager;
		public TMP_Text lines;
		public float refreshInterval = 0.25f;
		private float timer;

		private void Update()
		{
			timer += Time.deltaTime;
			if (timer < refreshInterval) return;
			timer = 0f;
			if (lines == null || player == null || ai == null || turnManager == null) return;
			lines.text =
				$"Turn: {(turnManager.IsPlayerTurn ? "Player" : "AI")}\n" +
				$"Player Mana: {player.mana?.currentMana}/{player.mana?.maxMana}\n" +
				$"AI Mana: {ai.mana?.currentMana}/{ai.mana?.maxMana}\n" +
				$"Player HP: {player.hero?.currentHealth}\n" +
				$"AI HP: {ai.hero?.currentHealth}\n" +
				$"Player Hand: {player.hand?.Count}";
		}
	}
}


