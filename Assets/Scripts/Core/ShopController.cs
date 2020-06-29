namespace DwarfClicker.Core
{
    using DwarfClicker.UI.ShopPanel;
    using Engine.Manager;
    using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Purchasing;
	using UnityEngine.UI;

	public class ShopController : MonoBehaviour, IStoreListener
	{
		private static IStoreController m_StoreController;          // The Unity Purchasing system.
		private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
		private static Product test_product = null;

		private Boolean return_complete = true;

		public IStoreController Store { get { return m_StoreController; } }

		#region Events
		private Action<Product> _onPurchaseComplete = null;

		public event Action<Product> OnPurchaseComplete
		{
			add
			{
				_onPurchaseComplete -= value;
				_onPurchaseComplete += value;
			}
			remove
			{
				_onPurchaseComplete -= value;
			}
		}
		#endregion Events

		void Start()
		{

			// If we haven't set up the Unity Purchasing reference
			if (m_StoreController == null)
			{
				// Begin to configure our connection to Purchasing
				InitializePurchasing();
			}
			MyDebug("Complete = " + return_complete.ToString());
            foreach (Product product in m_StoreController.products.all)
            {
                Debug.Log("Title : " + product.metadata.localizedTitle);
                Debug.Log("Price : " + product.metadata.localizedPrice);
                Debug.Log("Description : " + product.metadata.localizedDescription);
            }
            //Debug.Log(m_StoreController.products.all);
        }

		public void InitializePurchasing()
		{
			if (IsInitialized())
			{
				return;
			}

			//var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			/*builder.AddProduct(GOLD_50, ProductType.Consumable);
			builder.AddProduct(NO_ADS, ProductType.NonConsumable);
			builder.AddProduct(SUB1, ProductType.Subscription);*/

			StandardPurchasingModule module = StandardPurchasingModule.Instance();
			ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
			ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
			IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, catalog);

			UnityPurchasing.Initialize(this, builder);
            
		}

		public bool IsInitialized()
		{
			return m_StoreController != null && m_StoreExtensionProvider != null;
		}

		public void CompletePurchase()
		{
			if (test_product == null)
				MyDebug("Cannot complete purchase, product not initialized.");
			else
			{
				m_StoreController.ConfirmPendingPurchase(test_product);
				MyDebug("Completed purchase with " + test_product.transactionID.ToString());
                if (test_product.definition.id == "tea")
                {
                    JSonManager.Instance.PlayerProfile.Resources["purpleMithril"].UpdateCount(1);
                }
                
            }

		}

		public void ToggleComplete()
		{
			return_complete = !return_complete;
			MyDebug("Complete = " + return_complete.ToString());

		}
		public void RestorePurchases()
		{
			m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
				if (result)
				{
					MyDebug("Restore purchases succeeded.");
				}
				else
				{
					MyDebug("Restore purchases failed.");
				}
			});
		}

		void BuyProductID(string productId)
		{
			if (IsInitialized())
			{
				Product product = m_StoreController.products.WithID(productId);

				if (product != null && product.availableToPurchase)
				{
					MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
					m_StoreController.InitiatePurchase(product);
				}
				else
				{
					MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				MyDebug("BuyProductID FAIL. Not initialized.");
			}
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			MyDebug("OnInitialized: PASS");

			m_StoreController = controller;
			m_StoreExtensionProvider = extensions;
		}


		public void OnInitializeFailed(InitializationFailureReason error)
		{
			// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
			MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
		}


		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			test_product = args.purchasedProduct;



			//MyDebug(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

			if (return_complete)
			{
				MyDebug(string.Format("ProcessPurchase: Complete. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));
				if (_onPurchaseComplete != null)
					_onPurchaseComplete(test_product);
				return PurchaseProcessingResult.Complete;
			}
			else
			{
				MyDebug(string.Format("ProcessPurchase: Pending. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));
				return PurchaseProcessingResult.Pending;
			}

		}


		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		}

		private void MyDebug(string debug)
		{
			Debug.Log(debug);
		}

        public void LaunchElixirAd()
        {

            if (!JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
            {
                MonetizationManager.Instance.ShowAd();
                MonetizationManager.Instance.AdFinished += RedeemElixirReward;
            }
            else
            {
                RedeemElixirReward();
            }

        }

        public void RedeemElixirReward()
        {
            JSonManager.Instance.PlayerProfile.Mithril += 1;
        }

        public void LaunchSongAd(BonusProgressBar bonusProgressBar)
        {

            if (!JSonManager.Instance.PlayerProfile._noMoreAdsBonus)
            {
                MonetizationManager.Instance.ShowAd();
                MonetizationManager.Instance.AdFinished += RedeemFreeSongBonus;
                MonetizationManager.Instance.AdFinished += bonusProgressBar.UpdateBar;
            }
            else
            {
                RedeemFreeSongBonus();
                bonusProgressBar.UpdateBar();
            }
        }

        private void RedeemFreeSongBonus()
        {
            JSonManager.Instance.PlayerProfile._bonusTimeRemaining += DatabaseManager.Instance.ConsumableBonusData.BonusTimeByAd;
            JSonManager.Instance.PlayerProfile._bonusTimeRemaining
                = Mathf.Clamp(JSonManager.Instance.PlayerProfile._bonusTimeRemaining, 0, DatabaseManager.Instance.ConsumableBonusData.BonusTimeMax);
        }
    }
}