﻿namespace DwarfClicker.UI.ShopPanel
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using TMPro;
	using DwarfClicker.Core;
	using UnityEngine.Purchasing;
	using Engine.Manager;
	using Engine.Utils;
    using DwarfClicker.UI.PopUp;

    public class ShopPanel : MonoBehaviour
	{
		#region Fields
		[SerializeField] private ShopController _shopController = null;
        [SerializeField] private PopUpWindowController _popup = null;
        [SerializeField] private BonusProgressBar _bonusProgressBar = null; 

		[Header("Mithril pack Texts")]
		[SerializeField] private List<TextMeshProUGUI> _mithrilTitles = null;
		[SerializeField] private List<TextMeshProUGUI> _mithrilDescs = null;
		[SerializeField] private List<TextMeshProUGUI> _mithrilPrices = null;

		[Header("Bonus Texts")]
		[SerializeField] private TextMeshProUGUI _bonusTitle = null;
		[SerializeField] private TextMeshProUGUI _bonusDesc = null;
		[SerializeField] private TextMeshProUGUI _bonusPrice = null;

		private bool _isInit = false;
		#endregion Fields

		#region Methods
		private void Init()
		{
			for (int i = 0; i < _mithrilDescs.Count; i++)
			{
				Product mithrilPack = _shopController.Store.products.WithID("MITHRIL_PACK_" + (i+1).ToString());
				_mithrilTitles[i].text = mithrilPack.metadata.localizedTitle;
				_mithrilDescs[i].text = mithrilPack.metadata.localizedDescription;
				_mithrilPrices[i].text = mithrilPack.metadata.localizedPrice.ToString() + mithrilPack.metadata.isoCurrencyCode;

			}
			Product noAds = _shopController.Store.products.WithID("NO_ADS");
			_bonusTitle.text = noAds.metadata.localizedTitle;
			_bonusDesc.text = noAds.metadata.localizedDescription;
			if (JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
			{
				_bonusPrice.text = "Bought";
			}
			else
			{
				_bonusPrice.text = noAds.metadata.localizedPrice.ToString() + noAds.metadata.isoCurrencyCode;
			}
            _bonusProgressBar.UpdateBar();
			_isInit = true;
		}

		private void OnEnable()
		{
			if (_isInit == false)
			{
				Init();
			}
		}

		public void QuitPanel()
		{
			gameObject.SetActive(false);
		}

		public void BuyMithrilPack(int productId)
		{
			if (_shopController.IsInitialized())
			{
				Product product = _shopController.Store.products.WithID("MITHRIL_PACK_" + productId.ToString());

				if (product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product:" + product.definition.id.ToString()));
					_shopController.Store.InitiatePurchase(product);
					_shopController.OnPurchaseComplete += GainMithril;
                    _popup.Display(2, "Transaction complete !\nCongatulations !");
                }
				else
				{
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}

		public void BuyBonus(string ID)
		{
			if (_shopController.IsInitialized())
			{
				Product product = _shopController.Store.products.WithID(ID);

				if (product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product:" + product.definition.id.ToString()));
					_shopController.Store.InitiatePurchase(product);
					_shopController.OnPurchaseComplete += SetUpBonus;
                }
				else
				{
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}

        public void AddPermanentGoldBonus()
        {
            JSonManager.Instance.PlayerProfile._goldMultiplierBonus += DatabaseManager.Instance.PermanentBonus.PermanentGoldBonusAdd;
            _popup.Display(1, "Transaction complete !\nCongatulations !");
        }

        public void AddPermanentResBonus()
        {
            JSonManager.Instance.PlayerProfile._resourcesMultiplierBonus += DatabaseManager.Instance.PermanentBonus.PermanentResBonusAdd;
            _popup.Display(1, "Transaction complete !\nCongatulations !");
        }

        public void AddPermanentToolBonus()
        {
            JSonManager.Instance.PlayerProfile._toolsMultiplierBonus += DatabaseManager.Instance.PermanentBonus.PermanentToolBonusAdd;
            _popup.Display(1, "Transaction complete !\nCongatulations !");
        }

        private void GainMithril(Product product)
		{
			JSonManager.Instance.PlayerProfile.Mithril += (int)product.definition.payout.quantity;
		}

		private void SetUpBonus(Product product)
		{
			PlayerProfile profile = JSonManager.Instance.PlayerProfile;

			switch (product.definition.id)
			{
				case "NO_ADS":
					profile._noMoreAdsBonus = true;
                    _popup.Display(2, "Transaction complete !\nCongatulations !");
                    break;
				default:
					break;
			}
		}

        public void OnElixirButtonClick()
        {
            _shopController.LaunchElixirAd();
        }

        public void OnSongButtonClick()
        {
            _shopController.LaunchSongAd();
            _bonusProgressBar.UpdateBar();
        }

        #endregion Methods

    }
}