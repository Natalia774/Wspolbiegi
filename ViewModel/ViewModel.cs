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

            _model.IsAnimatingChanged += Model_IsAnimatingChanged;

            AddCommand = new RelayCommand(StartSimulation, CanStartSimulation);
            RunCommand = new RelayCommand(StartAnimation, CanStartAnimation);
            StopCommand = new RelayCommand(StopAnimation, CanStopAnimation);
        }

        private void Model_IsAnimatingChanged(object sender, EventArgs e)
        {
            ((RelayCommand)RunCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
        }

        public bool IsAnimating => _model.IsAnimating;

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

        public void StartAnimation()
        {
            if (!IsAnimating)
            {
                _model.StartBallAnimation();
            }
        }

        public void StopAnimation()
        {
            if (IsAnimating)
            {
                _model.StopBallAnimation();
            }
        }

        private bool CanStartSimulation()
        {
            return !_model.IsAnimating;
        }

        private bool CanStartAnimation()
        {
            return !_model.IsAnimating;
        }

        private bool CanStopAnimation()
        {
            return _model.IsAnimating;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}