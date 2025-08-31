using System;
using UnityEngine;
using VRHearthLike.Core;

namespace VRHearthLike.Gameplay
{

	public class TurnManager : MonoBehaviour
	{
		public event Action<Side> OnTurnStarted;
		public event Action<Side> OnTurnEnded;
		
		[Header("Quote")]
		public VRPlayer player;
		public VRPlayer ai;
		
		[Header("Condition")]
		public Side currentSide = Side.Player;
		
		// Property for backward compatibility
		public bool IsPlayerTurn => currentSide == Side.Player;
		
		public void BeginFirstTurn(Side first)
		{
			currentSide = first;
			StartTurn();
		}
		
		public void EndTurn()
		{
			var ended = currentSide;
			OnTurnEnded?.Invoke(ended);
			// Exchange
			currentSide = (currentSide == Side.Player) ? Side.AI : Side.Player;
			StartTurn();
		}
		
		private void StartTurn()
		{
			OnTurnStarted?.Invoke(currentSide);
			if (currentSide == Side.Player && player != null)
			{
				player.StartTurn();
			}
			else if (currentSide == Side.AI && ai != null)
			{
				ai.StartTurn();
			}
		}
	}
}


