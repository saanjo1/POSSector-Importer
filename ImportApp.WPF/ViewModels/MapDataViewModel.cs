using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.Domain.Models;
using ModalControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.WPF.ViewModels
{
    [ObservableObject]
    public partial class MapDataViewModel : BaseViewModel
    {

        [ObservableProperty]
        private BaseViewModel currentModalViewModel;


        [RelayCommand]
        public void Cancel()
        {

        }



        [RelayCommand]
        public void Submit()
        {

        }






    }
}
