using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Services;
using ImportApp.WPF.Resources;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;
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


            Path();
        }


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SelectSheetCommand))]
        [NotifyCanExecuteChangedFor(nameof(UploadExcelFileCommand))]
        private bool isOpen;


        [ObservableProperty]
        private bool isEditOpen;

        [ObservableProperty]
        private bool isDiscountOpen;


        [ObservableProperty]
        private SelectExcelSheetModalViewModel sheetDataModel;


        [ObservableProperty]
        private SetArticlesColumnsViewModel setColumnsVM;

        [ObservableProperty]
        private SetDiscountsColumnsViewModel setDiscountsVM;

        [ObservableProperty]
        private bool selectSheetSuccess;

        [ObservableProperty]
        private bool selectFileSuccess;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SelectSheetCommand))]
        [NotifyCanExecuteChangedFor(nameof(UploadExcelFileCommand))]
        private string excelFile;

        [ObservableProperty]
        private string dBConnection;

        [ObservableProperty]
        private string serverInstance;

        [ObservableProperty]
        private string appPort;


        public bool SheetCheck()
        {
            if (IsOpen)
                return false;
            return true;
        }


        [RelayCommand(CanExecute = nameof(SheetCheck))]
        public void UploadExcelFile()
        {
             ExcelFile = _excelDataService.OpenDialog().Result;

            try
            {
                if (ExcelFile != null)
                {
                    if (_myDictionary.TryGetValue(Translations.CurrentExcelFile, out string value) == false)
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

                if (ExcelFile != null)
                    SelectFileSuccess = true;
            }
            catch (System.Exception)
            {
                _notifier.ShowError(Translations.ErrorMessage);
                SelectFileSuccess = false;

            }
        }


        [RelayCommand]
        public void SetArticlesColumns()
        {
            this.IsEditOpen = true;
            this.SetColumnsVM = new SetArticlesColumnsViewModel(this, _notifier);

        }


        [RelayCommand]
        public void SetDiscountsColumns()
        {
            this.IsDiscountOpen = true;
            this.SetDiscountsVM = new SetDiscountsColumnsViewModel(this, _notifier);

        }

        [RelayCommand(CanExecute = nameof(SheetCheck))]
        public void SelectSheet()
        {
            try
            {
                IsOpen = true;
                this.SheetDataModel = new SelectExcelSheetModalViewModel(_excelDataService, this, _notifier, _myDictionary);
                if (SheetDataModel != null && SheetDataModel.SelectedSheet != null)
                    SelectSheetSuccess = true;


            }
            catch (System.Exception)
            {
                SelectSheetSuccess = false;
                throw;
            }
        }

        [RelayCommand]
        public void Path()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int index = appDataPath.IndexOf("Roaming");
            appDataPath = appDataPath.Substring(0, index) + Translations.POSFolderPath;

            XmlDocument doc = new XmlDocument();
            doc.Load(appDataPath);

            XmlNode databaseNode = doc.SelectSingleNode(Translations.DatabasePath);
            XmlNode serverInstanceNode = doc.SelectSingleNode(Translations.ServerInstancePath);
            XmlNode portNode = doc.SelectSingleNode(Translations.PortPath);
            DBConnection = databaseNode.InnerText;
            ServerInstance = serverInstanceNode.InnerText;
            AppPort = portNode.InnerText;
        }

        [RelayCommand]
        public void Cancel()
        {
            if (IsOpen)
                IsOpen = false;
            if (IsEditOpen)
                IsEditOpen = false;
            if (IsDiscountOpen)
                IsDiscountOpen = false;
        }

    }
}
