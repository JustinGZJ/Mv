﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Mv.Modules.Group.MockData;

namespace Mv.Modules.Group.Controls
{
    /// <summary>
    /// Interaction logic for MessageList.xaml
    /// </summary>
    public partial class MessageList : INotifyPropertyChanged
    {
        private const double InfinitelySmall = 1e-6;

        private bool _isArrivedTop;
        private bool _isArrivedBottom;


        public MessageList()
        {
            InitializeComponent();
            Initialize();
        }



        public static readonly DependencyProperty MessagesProperty = DependencyProperty.Register("Messages", typeof(ObservableCollection<Message>), typeof(MessageList), new PropertyMetadata(null, MessagesChanged));

        private static void MessagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;

            var control = (MessageList) d;
            control.Messages.CollectionChanged += control.OnCollectionChanged;
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Message item in e.NewItems)
                {
                    PART_MessageStackPanel.Children.Add(new MessageCard
                    {
                        Message = item.Text,
                        Author = item.Author,
                        Date = item.Date,
                        HeadUri = item.HeadUri
                    });
                }
                PART_ScrollViewer.ScrollToEnd(); // Upexpected: The ScrollToEnd() method will trigger the IsArrivedTop property change, and cause the LoadPreviousMessages() method to be called. 
            }
        }

        public ObservableCollection<Message> Messages
        {
            get => (ObservableCollection<Message>)GetValue(MessagesProperty);
            set => SetValue(MessagesProperty, value);
        }


        public bool IsArrivedTop
        {
            get => _isArrivedTop;
            private set { if (SetProperty(ref _isArrivedTop, value) && value) LoadPreviousMessages(); }
        }

        public bool IsArrivedBottom
        {
            get => _isArrivedBottom;
            private set { if (SetProperty(ref _isArrivedBottom, value) && value) LoadSubsequentMessages(); }
        }




        private void Initialize()
        {
            PART_ScrollViewer.ScrollChanged += (sender, e) =>
            {
                IsArrivedTop = e.VerticalOffset < InfinitelySmall;
                IsArrivedBottom = e.ExtentHeight - e.VerticalOffset - e.ViewportHeight < InfinitelySmall;
            };
        }

        private async void LoadPreviousMessages()
        {
            await Task.Run(() => { });
            throw new NotImplementedException(nameof(LoadPreviousMessages));
        }

        private async void LoadSubsequentMessages()
        {
            await Task.Run(() => { });
            Debug.WriteLine("Gets subsequent 10 messages.");
            throw new NotImplementedException(nameof(LoadSubsequentMessages));
        }

        #region Implements INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

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
        #endregion
    }
}
