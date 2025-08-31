using UnityEngine;
using UnityEngine.UI;
using VRHearthLike.Entities;

namespace VRHearthLike.UI
{
	public class MinionStatUI : MonoBehaviour
	{
		public Minion minion;
		public Text attackText;
		public Text healthText;
		public Vector3 worldOffset = new Vector3(0, 0.2f, 0);

		private Camera cam;

		private void Awake()
		{
			cam = Camera.main;
		}

		private void OnEnable()
		{
			if (minion != null)
			{
				minion.OnStatsChanged += OnStatsChanged;
				minion.OnDied += OnDied;
				Refresh();
			}
		}

		private void OnDisable()
		{
			if (minion != null)
			{
				minion.OnStatsChanged -= OnStatsChanged;
				minion.OnDied -= OnDied;
			}
		}

		private void LateUpdate()
		{
			if (cam != null && minion != null)
			{
				transform.position = minion.transform.position + worldOffset;
				transform.forward = (transform.position - cam.transform.position).normalized;
			}
		}

		private void OnStatsChanged(Minion m) { Refresh(); }
		private void OnDied(Minion m) { gameObject.SetActive(false); }

		private void Refresh()
		{
			if (attackText != null) attackText.text = minion.CurrentAttack.ToString();
			if (healthText != null) healthText.text = minion.CurrentHealth.ToString();
		}
	}
}


