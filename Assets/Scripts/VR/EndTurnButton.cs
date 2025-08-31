using UnityEngine;
using VRHearthLike.Gameplay;

namespace VRHearthLike.VR
{
	public class EndTurnButton : MonoBehaviour
	{
		public TurnManager turnManager;
		public void Press()
		{
			if (turnManager != null && turnManager.IsPlayerTurn)
			{
				turnManager.EndTurn();
			}
		}
	}
}


