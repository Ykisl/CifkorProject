using CifkorApp.DogBreeds.Model;
using CifkorApp.Screen;
using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Zenject;

namespace CifkorApp.DogBreeds
{
    public class DogBreedsScreenView : LoadableScreenView
    {
        [SerializeField] private Transform _breedViewRoot;
        [SerializeField] private GameObject _loadingIndicator;

        private DogBreedView.Factory _breedViewFactory;

        private Dictionary<DogBreedView, DogBreedDataModel> _breedsViewItems = new Dictionary<DogBreedView, DogBreedDataModel>();

        [Inject]
        private void Construct(DogBreedView.Factory breedViewFactory)
        {
            _breedViewFactory = breedViewFactory;
        }

        public void SetBreedsData(IList<DogBreedDataModel> breedsData)
        {
            ClearBreedsData();

            for(int i = 0; i < breedsData.Count; i++)
            {
                var breedModel = breedsData[i];

                var viewItem = _breedViewFactory.Create(breedModel);
                _breedsViewItems.Add(viewItem, breedModel);

                viewItem.SetIndex(i+1);
                viewItem.SetIsLoading(false);

                viewItem.OnItemClicked += HandleBreedViewItemClicked;

                var viewTransform = viewItem.transform;
                viewTransform.SetParent(_breedViewRoot);
                viewTransform.localScale = Vector3.one;
                viewTransform.SetSiblingIndex(i);

                viewItem.gameObject.SetActive(true);
            }

            UpdateLoadingView();
        }

        public void ClearBreedsData()
        {
            if(_breedsViewItems.Count <= 0)
            {
                return;
            }

            foreach(var breedViewItem in _breedsViewItems.Keys)
            {
                breedViewItem.OnItemClicked -= HandleBreedViewItemClicked;
                breedViewItem.Dispose();
            }

            _breedsViewItems.Clear();
            UpdateLoadingView();
        }

        protected override void UpdateLoadingView()
        {
            var isLoadingVisible = _isLoading && _breedsViewItems.Count <= 0;
            _loadingIndicator.SetActive(isLoadingVisible);
        }

        private void HandleBreedViewItemClicked(DogBreedView view)
        {

        }
    }
}
