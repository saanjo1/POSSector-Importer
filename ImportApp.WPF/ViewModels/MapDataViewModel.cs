﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.Resources;
using ModalControl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapDataViewModel : BaseViewModel
    {
        private Notifier _notifier;


        [ObservableProperty]
        private List<string> currentSheets;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string selectedSheet;

        private IExcelDataService _excelDataService;
        private readonly ImportDataViewModel _importArticleViewModel;
        private readonly SettingsViewModel _setingsViewModel;
        private ConcurrentDictionary<string, string> _myDictionary;


        [ObservableProperty]
        private MapColumnViewModel? mColumnModel;

        [ObservableProperty]
        private bool isMapped;

        //public MapDataViewModel(IExcelDataService excelDataService, ImportDataViewModel importArticleViewModel, MapColumnViewModel mapColumnViewModel, Notifier notifier, ConcurrentDictionary<string, string> myDictionary)
        //{
        //    _excelDataService = excelDataService;
        //    articleQ = mapColumnViewModel;
        //    _importArticleViewModel = importArticleViewModel;
        //    CurrentSheets = _excelDataService.ListSheetsFromFile().Result;
        //    SelectedSheet = CurrentSheets[0];
        //    _notifier = notifier;
        //    _myDictionary = myDictionary;
        //}


        public MapDataViewModel(IExcelDataService excelDataService, SettingsViewModel setingsViewModel, Notifier notifier, ConcurrentDictionary<string, string> myDictionary)
        {
            _excelDataService = excelDataService;
            _setingsViewModel = setingsViewModel;
            _myDictionary = myDictionary;
            _notifier = notifier;
            LoadSheet();
        }

        public void LoadSheet()
        {
            if (_myDictionary != null && _myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value))
            {
                CurrentSheets = _excelDataService.ListSheetsFromFile(value).Result;
                SelectedSheet = CurrentSheets[0];
            }
            else
            {
                _notifier.ShowError(Translations.SelectSheetError);
                Cancel();
            }
        }


        [RelayCommand]
        public void Cancel()
        {
            _setingsViewModel.Cancel();
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        public void Save()
        {
            if (SelectedSheet != null)
            {

                if (_myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value1) == false)
                {
                    bool success = _myDictionary.TryAdd(Translations.CurrentExcelSheet, SelectedSheet);
                    if (success)
                        _notifier.ShowInformation(Translations.SheetSelectedSuccessfully);
                    else
                        _notifier.ShowError(Translations.ErrorMessage);
                }
                else
                {
                    _myDictionary.TryGetValue(Translations.CurrentExcelSheet, out string value);

                    bool success = _myDictionary.TryUpdate(Translations.CurrentExcelSheet, SelectedSheet, value);
                    if (success)
                        _notifier.ShowInformation(Translations.SheetSelectedSuccessfully);
                    else
                        _notifier.ShowError(Translations.ErrorMessage);
                }
            }
            else
            {
                _notifier.ShowError(Translations.ErrorMessage);
            }

            Cancel();
        }


        private bool CanSave()
=> !string.IsNullOrWhiteSpace(SelectedSheet);
    }
}
