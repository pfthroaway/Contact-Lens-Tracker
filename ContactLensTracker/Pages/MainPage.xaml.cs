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
            DateTime replacementDate = DateTimeHelper.Parse(DateNewContact.SelectedDate);
            switch (CmbLength.SelectedItem.ToString())
            {
                case "1 week":
                    replacementDate = replacementDate.AddDays(7);
                    break;

                case "2 weeks":
                    replacementDate = replacementDate.AddDays(14);
                    break;

                case "30 days":
                    replacementDate = replacementDate.AddDays(30);
                    break;
            }
            foreach (Side side in sides)
                await AppState.AddContact(new Contact(DateTimeHelper.Parse(DateNewContact.SelectedDate), side, replacementDate));
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
            CmbLength.Items.Add("1 week");
            CmbLength.Items.Add("2 weeks");
            CmbLength.Items.Add("30 days");
            CmbLength.SelectedIndex = 0;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) => AppState.CalculateScale(Grid);

        private void DateNewContact_SelectedDateChanged(object sender, SelectionChangedEventArgs e) =>
            ToggleButtons(DateNewContact.Text.Length > 0);

        #endregion Page-Manipulation Methods
    }
}