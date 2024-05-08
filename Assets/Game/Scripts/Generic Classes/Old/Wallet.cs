using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Forms.Wallets
{
    public class Wallet
    {
        private int _amount;
        private int _maxAmount;
        private GameObject _walletObject;
        private VisualElement _walletUI;
        private WalletScriptable _walletScriptable;

        public int GetAmount() { return _amount; }
        public WalletScriptable GetWalletType() { return _walletScriptable; }

        public Wallet(WalletScriptable _data, int _startingAmount)
        {
            _walletScriptable = _data;

            _amount = _startingAmount;
            _maxAmount = _data.MaxAmount;

            _walletObject = _data.CreateObject(
                GameManager.Instance.Player.transform, new Vector2(0, 0));

            _walletUI = _walletObject.GetComponent<UIDocument>().rootVisualElement[0];
            _walletUI.style.alignSelf = Align.FlexEnd;
            UpdateUI();
        }

        /// <summary>
        /// Returns -1 if there is not enough Space, 
        /// else returns the new amount
        /// </summary>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public int AddAmount(int _amount)
        {
            if (_amount + this._amount > _maxAmount)
            {
                this._amount = _maxAmount;
                UpdateUI();
                return -1;
            }
            this._amount += _amount;
            UpdateUI();
            return this._amount;
        }

        /// <summary>
        /// Returns -1 if there is not enough Money, 
        /// else returns the new amount
        /// </summary>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public int RemoveAmount(int _amount)
        {
            if (_amount > this._amount)
            {
                Debug.Log("Not Enough Money");
                return -1;
            }

            this._amount -= _amount;
            UpdateUI();
            return this._amount;
        }

        private void UpdateUI()
        {
            _walletUI.Q<Label>("Amount").text = _amount.ToString("N0");
        }

        public void Destroy()
        {
            GameObject.Destroy(_walletObject);
        }
    }
}
