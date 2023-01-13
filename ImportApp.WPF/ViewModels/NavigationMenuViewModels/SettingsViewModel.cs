using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.EntityFramework.Services;
using ImportApp.WPF.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class SettingsViewModel : BaseViewModel
    {

        private IDiscountDataService _discountDataService;
        private Notifier _notifier;
        private ConcurrentDictionary<string, string> _myDictionary;
        private IExcelDataService _excelDataService;


        public SettingsViewModel(IDiscountDataService discountDataService, ConcurrentDictionary<string, string> myDictionary, Notifier notifier, IExcelDataService excelDataService)
        {
            _discountDataService = discountDataService;
            _myDictionary = myDictionary;
            _notifier = notifier;
            _excelDataService = excelDataService;
        }

        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SelectSheetCommand))]
        [NotifyCanExecuteChangedFor(nameof(UploadExcelFileCommand))]
        private bool isOpen;


        [ObservableProperty]
        private SelectExcelSheetModalViewModel sheetDataModel;


        [ObservableProperty]
        private string dBConnection;


        public bool SheetCheck()
        {
            if (IsOpen)
                return false;
            return true;
        }


        [RelayCommand(CanExecute = nameof(SheetCheck))]
        public void UploadExcelFile()
        {
            string excelFile = _excelDataService.OpenDialog().Result;

            try
            {
                if (excelFile != null)
                {
                    if(_myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value) == false)
                    {
                        bool success = _myDictionary.TryAdd(Translations.CurrentExcelFile, excelFile);

                        if (success)
                            _notifier.ShowSuccess(Translations.SelectedExcelFile);
                        else
                            _notifier.ShowError(Translations.ErrorMessage);
                    }
                    else
                    {
                        _myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value1);
                        bool success = _myDictionary.TryUpdate(Translations.CurrentExcelFile, excelFile, value1);

                        if (value1 == excelFile)
                            _notifier.ShowInformation(Translations.UpdatedSameFile);

                        if (success && value1 != excelFile)
                            _notifier.ShowInformation(Translations.UpdatedExcelFile);
                    }
                }
            }
            catch (System.Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);

            }
        }


        [RelayCommand(CanExecute = nameof(SheetCheck))]
        public void SelectSheet()
        {
            try
            {
                IsOpen = true;
                this.SheetDataModel = new SelectExcelSheetModalViewModel(_excelDataService, this, _notifier, _myDictionary);

            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            if (IsOpen)
                IsOpen = false;
        }

    }
}
