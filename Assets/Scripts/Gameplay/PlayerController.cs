using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{
	public class PlayerController : PlayerAgent
	{
		public BoardManager board;
		public TurnManager turnManager;

		private CardAsset pendingCard;
		private CardInHand pendingCardView;
		private Minion selectedAttacker;

		public void NotifyCardSelected(CardInHand view)
		{
			if (!turnManager.IsPlayerTurn) return;
			pendingCard = view.Data;
			pendingCardView = view;
			view.SetVisible(false);
		}

		public bool TryPlacePendingCard(BoardSlot slot)
		{
			if (!turnManager.IsPlayerTurn) return false;
			if (pendingCard == null || slot == null || !slot.isPlayerSide || !slot.IsEmpty) return false;
			if (!mana.Spend(pendingCard.manaCost)) return false;
			int idx = board.GetIndexOfSlot(slot, true);
			if (idx >= 0 && board.TryPlaceMinion(true, idx, pendingCard.minionPrefab, pendingCard.attack, pendingCard.health, out var minion))
			{
				RemoveCard(pendingCard);
				pendingCard = null;
				if (pendingCardView != null) Destroy(pendingCardView.gameObject);
				pendingCardView = null;
				return true;
			}
			// 放置失败，返还卡面显示
			CancelPendingCard();
			return false;
		}

		public void CancelPendingCard()
		{
			if (pendingCardView != null)
			{
				pendingCardView.SetVisible(true);
			}
			pendingCard = null;
			pendingCardView = null;
		}

		public void SelectAttacker(Minion minion)
		{
			if (!turnManager.IsPlayerTurn) return;
			if (minion == null) return;
			selectedAttacker = minion;
		}

		public void TryAttackTarget(Minion targetMinion)
		{
			if (!turnManager.IsPlayerTurn) return;
			if (selectedAttacker == null || targetMinion == null) return;
			selectedAttacker.Attack(targetMinion);
			selectedAttacker = null;
		}

		public void TryAttackHero(Hero targetHero)
		{
			if (!turnManager.IsPlayerTurn) return;
			if (selectedAttacker == null || targetHero == null) return;
			targetHero.ReceiveDamage(selectedAttacker.CurrentAttack);
			selectedAttacker.ReceiveDamage(0); // 占位，若需要反伤可改
			selectedAttacker = null;
		}
	}
}


