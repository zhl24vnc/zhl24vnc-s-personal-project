using System.Collections.Generic;
using UnityEngine;
using VRHearthLike.Cards;

namespace VRHearthLike.Gameplay
{
	public class HandManager : MonoBehaviour
	{
		[SerializeField] private Transform handAnchor;
		[SerializeField] private GameObject cardInHandPrefab;
		[SerializeField] private int maxHandSize = 10;
		[SerializeField] private float cardSpacing = 0.12f;

		private readonly List<CardAsset> cardsInHand = new List<CardAsset>();
		private readonly List<CardInHand> spawnedCardViews = new List<CardInHand>();

		public IReadOnlyList<CardAsset> Cards => cardsInHand;

		public bool AddCard(CardAsset data, PlayerController owner)
		{
			if (data == null) return false;
			if (cardsInHand.Count >= maxHandSize) return false;
			cardsInHand.Add(data);
			RefreshVisuals(owner);
			return true;
		}

		public void RemoveCard(CardAsset data)
		{
			if (data == null) return;
			cardsInHand.Remove(data);
		}

		public void RefreshVisuals(PlayerController owner)
		{
			foreach (var view in spawnedCardViews)
			{
				if (view != null) Destroy(view.gameObject);
			}
			spawnedCardViews.Clear();

			if (owner == null) return; // 只有玩家侧渲染手牌

			for (int i = 0; i < cardsInHand.Count; i++)
			{
				var data = cardsInHand[i];
				var go = Instantiate(cardInHandPrefab, handAnchor);
				go.transform.localPosition = new Vector3(i * cardSpacing, 0, 0);
				var view = go.GetComponent<CardInHand>();
				if (view == null) view = go.AddComponent<CardInHand>();
				view.Initialize(data, this, owner);
				spawnedCardViews.Add(view);
			}
		}
	}
}


