using Logic;
using Data;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Windows.Controls;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ModelAPI _model; 
        public int _numberOfBallsToAdd;

        public MainViewModel()
        {
            _model = (ModelAPI)ModelAbstractAPI.CreateModelAPI();
            AddCommand = new RelayCommand(StartSimulation);
            RunCommand = new RelayCommand(StartAnimation);
            StopCommand = new RelayCommand(StopAnimation);
        }

        public Canvas Canvas
        {
            get => _model.Canvas;
        }

        public int NumberOfBallsToAdd
        {
            get => _numberOfBallsToAdd;
            set
            {
                if (_numberOfBallsToAdd != value)
                {
                    _numberOfBallsToAdd = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand StopCommand { get; }

        private void StartSimulation()
        {
            _model.CreateEllipses(NumberOfBallsToAdd);
        }

        private void StartAnimation()
        {
            _model.StartBallAnimation();
        }

        private void StopAnimation()
        {
            _model.StopBallAnimation();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
