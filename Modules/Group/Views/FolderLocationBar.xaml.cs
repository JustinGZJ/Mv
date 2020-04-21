using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mv.Core.Interfaces;
using Mv.Core;
using Mv.Ui.Core;

namespace Mv.Modules.Group.Views
{
    /// <summary>
    /// Interaction logic for FolderLocationBar.xaml
    /// </summary>
    public partial class FolderLocationBar : INotifyPropertyChanged
    {
        private const int MaxDisplayedFolderCount = 3;

        private ICommand _locateFolderCommand; 
        private ICommand _locateRootCommand;
        private ObservableCollection<ILazyTreeNode<object>> _displayedFolders;
        private ObservableCollection<ILazyTreeNode<object>> _foldedFolders;

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty CurrentFolderProperty = DependencyProperty.Register(
                "CurrentFolder",
                typeof(ILazyTreeNode<object>),
                typeof(FolderLocationBar),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CurrentFolderChanged));

        public ILazyTreeNode<object> CurrentFolder
        {
            get => (ILazyTreeNode<object>)GetValue(CurrentFolderProperty);
            set => SetValue(CurrentFolderProperty, value);
        }

        public ObservableCollection<ILazyTreeNode<object>> DisplayedFolders
        {
            get => _displayedFolders;
            set => SetProperty(ref _displayedFolders, value);
        }
        public ObservableCollection<ILazyTreeNode<object>> FoldedFolders
        {
            get => _foldedFolders;
            set => SetProperty(ref _foldedFolders, value);
        }

        public ICommand LocateFolderCommand
        {
            get => _locateFolderCommand;
            set => SetProperty(ref _locateFolderCommand, value);
        }
        public ICommand LocateRootCommand
        {
            get => _locateRootCommand;
            set => SetProperty(ref _locateRootCommand, value);
        }


        public FolderLocationBar()
        {
            LocateFolderCommand = new RelayCommand<ILazyTreeNode<object>>(
                fileNode => CurrentFolder = fileNode,
                fileNode => CurrentFolder != fileNode);
            LocateRootCommand = new RelayCommand(
                () => CurrentFolder = CurrentFolder.Root,
                () => CurrentFolder != CurrentFolder?.Root);

            InitializeComponent();
        }


        private static void CurrentFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FolderLocationBar)d;
            var folderChain = new Stack<ILazyTreeNode<object>>();
            var temp = control.CurrentFolder;
            do
            {
                folderChain.Push(temp);
            } while ((temp = temp.Parent) != null);
            (control.DisplayedFolders, control.FoldedFolders) = ClassifyFolder(folderChain);
        }

        private static (ObservableCollection<ILazyTreeNode<object>> displayed, ObservableCollection<ILazyTreeNode<object>> folded) ClassifyFolder(Stack<ILazyTreeNode<object>> folders)
        {
            ObservableCollection<ILazyTreeNode<object>> folded = null;
            int delta = folders.Count - MaxDisplayedFolderCount;
            if (delta > 0)
            {
                folded = new ObservableCollection<ILazyTreeNode<object>>();
                for (int i = 0; i < delta; i++)
                {
                    folded.Insert(0, folders.Pop());
                }
            }
            else
            {
                folders.Pop();
            }
            return (new ObservableCollection<ILazyTreeNode<object>>(folders), folded);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void FoldedFolderPopupListBox_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                CurrentFolder = (ILazyTreeNode<object>) listBox.SelectedItem;
            }
        }
    }
}
