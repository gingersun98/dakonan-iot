using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Samples.Purchasing.Core.InAppPurchase
{
    public class InAppPurchase : MonoBehaviour
    {
        StoreController m_StoreController;

        [Header("Products")]
        // Your subscription ID. It should match the id of your subscription in your store.
        public string[] consumableProductId;
        public string[] nonConsumableProductId;
        public string[] subscriptionProductId;
        [Header("Price Display")]
        public PriceDisplay[] listOfPrices;

        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔽🔽🔽🔽🔽🔽🔽
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔽🔽🔽🔽🔽🔽🔽
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔽🔽🔽🔽🔽🔽🔽
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔽🔽🔽🔽🔽🔽🔽

        void GrantPurchases(string id)
        {
            switch (id)
            {
                case "template : productID yang dimasukan":
                    // function untuk memberi items, dll.
                    //IAPManager.Instance.GiveCoins(100);
                    break;
            }
        }

        // Maps subscription product IDs to their effect handlers - GANTI MASING MASING EFFECT DI SINI
        private Dictionary<string, Action<bool>> subscriptionEffects = new Dictionary<string, Action<bool>>
        {
            /*
            { "subscriptionTemplateID", isActive =>
            {
                if (isActive)
                {
                    SUBSCRIPTION IS ACTIVE()
                } else
                {
                    SUBSCRIPTION IS INACTIVE()
                }
            } },
            */
            // add more subscriptions easily with this template
        };


        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔼🔼🔼🔼🔼🔼🔼
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔼🔼🔼🔼🔼🔼🔼
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔼🔼🔼🔼🔼🔼🔼
        // UNTUK MENGGANTI EFFECT SETIAP PEMBELIAN DISINI - 🔼🔼🔼🔼🔼🔼🔼






        // INI UNTUK TECHNICAL, SEBAIKNYA JANGAN DISENTUH, GANTI JIKA ADA BUG - 🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽
        
        void Awake()
        {
            InitializeIAP();
        }

        async void InitializeIAP()
        {
            m_StoreController = UnityIAPServices.StoreController();

            m_StoreController.OnPurchasePending += OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            m_StoreController.OnCheckEntitlement += OnCheckEntitlement;
            m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
            m_StoreController.OnPurchaseFailed += OnPurchaseFailed;
            m_StoreController.OnProductsFetched += OnProductsFetched;
            m_StoreController.OnProductsFetchFailed += OnProductsFetchFailed;

            await m_StoreController.Connect();
            FetchProducts();
            CheckSubscription();
        }

        void FetchProducts()
        {
            var products = new List<ProductDefinition> { };
            foreach (string consumable in consumableProductId)
            {
                products.Add(new ProductDefinition(consumable, ProductType.Consumable));
            }
            foreach (string nonConsumable in nonConsumableProductId)
            {
                products.Add(new ProductDefinition(nonConsumable, ProductType.NonConsumable));
            }
            foreach (string subscription in subscriptionProductId)
            {
                products.Add(new ProductDefinition(subscription, ProductType.Subscription));
            }
            m_StoreController.OnProductsFetched += OnProductsFetched;
            m_StoreController.OnProductsFetchFailed += OnProductsFetchFailed;
            m_StoreController.FetchProducts(products);
            UpdatePricesUI();
        }

        void OnPurchasePending(PendingOrder order)
        {
            m_StoreController.ConfirmPurchase(order);
        }

        void OnPurchaseConfirmed(Order order)
        {
            var product = GetFirstProductInOrder(order);
            if (product is null)
            {
                Debug.Log("Could not find product in order.");
                return;
            }

            //Add the purchased product to the players inventory - INI DIGANTI SESUAI GAME MASING MASING >_<
            GrantPurchases(product.definition.id);

            Debug.Log($"Purchase complete - Product: {product.definition.id}");
            CheckSubscription();
        }

        void OnPurchaseFailed(FailedOrder order)
        {
            var product = GetFirstProductInOrder(order);
            if (product == null)
            {
                Debug.Log("Could not find product in failed order.");
            }

            Debug.Log($"Purchase failed - Product: '{product?.definition.id}'," +
                      $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                      + $"Purchase Failure Details: {order.Details}");
        }

        void CheckSubscription()
        {
            foreach (string subscription in subscriptionProductId)
            {
                var product = m_StoreController.GetProducts().FirstOrDefault(p => p.definition.id == subscription);
                if (product == null)
                {
                    Debug.LogError($"Subscription product '{subscription}' not found in store.");
                    continue;
                }
                m_StoreController.CheckEntitlement(product);
            }
        }

        void OnStoreDisconnected(StoreConnectionFailureDescription storeConnectionFailureDescription)
        {
            Debug.Log($"Store disconnected. Reason: {storeConnectionFailureDescription}");
            UpdatePricesUI();
            // Optionally, update UI
        }

        void OnProductsFetchFailed(ProductFetchFailed productFetchFailed)
        {
            Debug.Log($"Product fetch failed. Reason: {productFetchFailed.FailureReason}");
            UpdatePricesUI();
            // Optionally, update UI or retry fetching products
        }

        void OnProductsFetched(List<Product> products)
        {
            Debug.Log("Products successfully fetched from the store.");
            UpdatePricesUI();
            CheckSubscription();
            // Optionally, update UI or refresh product list
        }

        void OnCheckEntitlement(Entitlement entitlement)
        {
            string id = entitlement.Product.definition.id;
            bool isEntitled = entitlement.Status == EntitlementStatus.FullyEntitled;

            if (!subscriptionEffects.TryGetValue(id, out var effect))
            {
                print($"Unknown subscription: {id}. Add it to your subscriptionEffects dictionary.");
                return;
            }

            effect(isEntitled);
        }

        public void BuyProduct(string id)
        {
            m_StoreController.PurchaseProduct(id);
        }

        Product GetFirstProductInOrder(Order order)
        {
            return order.CartOrdered.Items().First()?.Product;
        }

        void UpdatePricesUI()
        {
            if (m_StoreController == null)
            {
                foreach (PriceDisplay display in listOfPrices)
                {
                    display.targetText.text = "Offline";
                }
                return;
            }

            foreach (var display in listOfPrices)
            {
                var product = m_StoreController.GetProductById(display.productId);

                if (product?.metadata != null)
                    display.targetText.text = product.metadata.localizedPriceString;
                else
                    display.targetText.text = "N/A";
            }
        }
    }
}

[System.Serializable]
public class PriceDisplay
{
    public TextMeshProUGUI targetText;
    public string productId;
}
