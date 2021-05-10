using PVCase_Task.ViewModels;
using System;
using System.Windows.Input;

namespace PVCase_Task.Commands {

    class GenerateCommand : ICommand {

        private MainViewModel _ViewModel;

        public GenerateCommand(MainViewModel viewModel) {
            _ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return _ViewModel.CanUpdate;
        }

        public void Execute(object parameter) {
            _ViewModel.Generate();
        }
    }
}
