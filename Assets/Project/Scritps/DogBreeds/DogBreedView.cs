using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using CifkorApp.DogBreeds.Model;
using Zenject;
using CifkorApp.Screen;

namespace CifkorApp.DogBreeds
{
    public class DogBreedView : MonoBehaviour, IPoolable<DogBreedDataModel, IMemoryPool>, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _indexText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private GameObject _loadingIndicator;
        [Space]
        [SerializeField] private Button _itemButton;

        private IMemoryPool _pool;

        public event Action<DogBreedView> OnItemClicked;

        public class Factory : PlaceholderFactory<DogBreedDataModel, DogBreedView> { }

        #region UNITY_EVENTS

        protected virtual void OnEnable()
        {
            _itemButton.onClick.AddListener(HandleButtonClicked);
        }

        protected virtual void OnDisable()
        {
            _itemButton.onClick.RemoveListener(HandleButtonClicked);
        }

        #endregion

        public void Initialize(DogBreedDataModel model)
        {
            SetIsLoading(false);
            SetIndex(0);

            _nameText.text = model.AttributesData.Name;
        }
        public void SetIndex(int index)
        {
            _indexText.text = index.ToString();
        }

        public void SetIsLoading(bool isLoading)
        {
            _loadingIndicator.gameObject.SetActive(isLoading);
        }

        private void HandleButtonClicked()
        {
            OnItemClicked?.Invoke(this);
        }

        public void OnSpawned(DogBreedDataModel model, IMemoryPool pool)
        {
            _pool = pool;
            Initialize(model);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            OnItemClicked = null;
            _pool = null;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }
    }
}
