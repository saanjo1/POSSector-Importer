using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImportApp.WPF.Commands;
using ImportApp.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImportApp.WPF.State.Navigators
{
    public partial class Navigator : ObservableObject, INavigator
    {
        [ObservableProperty]
        private BaseViewModel _currentViewModel;

        
        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

       
    }
}
