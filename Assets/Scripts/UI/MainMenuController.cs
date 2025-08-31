using UnityEngine;
using TMPro;

namespace VRHearthLike.UI
{
	public class MainMenuController : MonoBehaviour
	{
		public Managers.GameManager gameManager;
		public TMP_Text title;
		public GameObject menuRoot;

		public void Show()
		{
			if (menuRoot != null) menuRoot.SetActive(true);
		}

		public void Hide()
		{
			if (menuRoot != null) menuRoot.SetActive(false);
		}

		public void OnStartButton()
		{
			if (gameManager != null)
			{
				gameManager.BeginNewMatch(true);
			}
			Hide();
		}

		public void OnQuitButton()
		{
			Application.Quit();
		}
	}
}


