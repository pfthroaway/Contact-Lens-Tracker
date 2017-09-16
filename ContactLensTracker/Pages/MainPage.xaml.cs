using Contacts.Classes;
using Contacts.Classes.Entities;
using Contacts.Enums;
using Extensions;
using Extensions.DataTypeHelpers;
using Extensions.ListViewHelp;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Contacts.Pages
{
    /// <summary>Interaction logic for MainPage.xaml</summary>
    public partial class MainPage
    {
        private ListViewSort _sort = new ListViewSort();

        /// <summary>Adds a new contact to the database</summary>
        /// <param name="sides">Sides on which contacts are being added</param>
        private async void NewContact(params Side[] sides)
        {
            foreach (Side side in sides)
                await AppState.AddContact(new Contact(DateTimeHelper.Parse(DateNewContact.SelectedDate), side));
            RefreshItemsSource();
        }

        /// <summary>Refreshes the ItemsSource of LVContacts.</summary>
        internal void RefreshItemsSource()
        {
            LVContacts.ItemsSource = AppState.AllContacts;
            LVContacts.Items.Refresh();
        }

        /// <summary>Toggles the Buttons on the Page.</summary>
        /// <param name="enabled">Should the buttons be enabled?</param>
        private void ToggleButtons(bool enabled)
        {
            BtnAddBoth.IsEnabled = enabled;
            BtnAddLeft.IsEnabled = enabled;
            BtnAddRight.IsEnabled = enabled;
        }

        #region Click

        private void BtnAddBoth_Click(object sender, RoutedEventArgs e) => NewContact(Side.Left, Side.Right);

        private void BtnAddLeft_Click(object sender, RoutedEventArgs e) => NewContact(Side.Left);

        private void BtnAddRight_Click(object sender, RoutedEventArgs e) => NewContact(Side.Right);

        private void LVContactsColumnHeader_Click(object sender, RoutedEventArgs e) => _sort =
            Functions.ListViewColumnHeaderClick(sender, _sort, LVContacts, "#CCCCCC");

        #endregion Click

        #region Page-Manipulation Methods

        public MainPage()
        {
            InitializeComponent();
            DateNewContact.SelectedDate = DateTime.Today;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) => AppState.CalculateScale(Grid);

        private void DateNewContact_SelectedDateChanged(object sender, SelectionChangedEventArgs e) =>
            ToggleButtons(DateNewContact.Text.Length > 0);

        #endregion Page-Manipulation Methods
    }
}