using UnityEngine;

namespace VRHearthLike.UI
{
	public class ManaCrystalRow : MonoBehaviour
	{
		[SerializeField] private GameObject crystalPrefab;
		[SerializeField] private Transform container;
		[SerializeField] private int pooledCount = 10;
		private GameObject[] pooledCrystals;

		private void Awake()
		{
			pooledCrystals = new GameObject[pooledCount];
			for (int i = 0; i < pooledCount; i++)
			{
				var go = Instantiate(crystalPrefab, container);
				go.SetActive(false);
				pooledCrystals[i] = go;
			}
		}

		public void UpdateCrystals(int current, int max)
		{
			for (int i = 0; i < pooledCount; i++)
			{
				bool withinMax = i < max;
				if (pooledCrystals[i] != null)
				{
					pooledCrystals[i].SetActive(withinMax);
					if (withinMax)
					{
						var color = i < current ? Color.cyan : Color.gray;
						var renderer = pooledCrystals[i].GetComponentInChildren<Renderer>();
						if (renderer != null && renderer.material != null)
						{
							renderer.material.color = color;
						}
					}
				}
			}
		}
	}
}


