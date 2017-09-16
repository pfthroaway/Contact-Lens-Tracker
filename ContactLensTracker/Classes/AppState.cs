using System.Collections.Generic;
using System.Linq;
using Contacts.Pages;
using Extensions;
using Extensions.Enums;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Contacts.Classes.Database;
using Contacts.Classes.Entities;

namespace Contacts.Classes
{
    /// <summary>Represents the current state of the application.</summary>
    internal static class AppState
    {
        private static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();
        internal static List<Contact> AllContacts = new List<Contact>();

        #region Load

        /// <summary>Loads all information from the database.</summary>
        /// <returns>Returns true if successful</returns>
        internal static async Task LoadAll()
        {
            DatabaseInteraction.VerifyDatabaseIntegrity();
            AllContacts = await DatabaseInteraction.LoadContacts();
        }

        #endregion Load

        #region Contact Lens Manipulation

        /// <summary>Adds a new contact insertion to the database.</summary>
        /// <param name="newContact">Contact insertion to be added</param>
        internal static async Task<bool> AddContact(Contact newContact)
        {
            if (await DatabaseInteraction.AddContact(newContact))
            {
                AllContacts.Add(newContact);
                AllContacts = AllContacts.OrderByDescending(contact => contact.Date).ThenBy(contact => contact.SideToString).ToList();
                return true;
            }
            return false;
        }

        #endregion Contact Lens Manipulation

        #region Navigation

        /// <summary>Instance of MainWindow currently loaded</summary>
        internal static MainWindow MainWindow { get; set; }

        /// <summary>Width of the Page currently being displayed in the MainWindow</summary>
        internal static double CurrentPageWidth { get; set; }

        /// <summary>Height of the Page currently being displayed in the MainWindow</summary>
        internal static double CurrentPageHeight { get; set; }

        /// <summary>Calculates the scale needed for the MainWindow.</summary>
        /// <param name="grid">Grid of current Page</param>
        internal static void CalculateScale(Grid grid)
        {
            CurrentPageHeight = grid.ActualHeight;
            CurrentPageWidth = grid.ActualWidth;
            MainWindow.CalculateScale();

            Page newPage = MainWindow.MainFrame.Content as Page;
            if (newPage != null)
                newPage.Style = (Style)MainWindow.FindResource("PageStyle");
        }

        /// <summary>Navigates to selected Page.</summary>
        /// <param name="newPage">Page to navigate to.</param>
        internal static void Navigate(Page newPage) => MainWindow.MainFrame.Navigate(newPage);

        /// <summary>Navigates to the previous Page.</summary>
        internal static void GoBack()
        {
            if (MainWindow.MainFrame.CanGoBack)
                MainWindow.MainFrame.GoBack();
        }

        #endregion Navigation

        #region Notification Management

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        internal static void DisplayNotification(string message, string title) => Application.Current.Dispatcher.Invoke(
            () => { new Notification(message, title, NotificationButtons.OK, MainWindow).ShowDialog(); });

        /// <summary>Displays a new Notification in a thread-safe way and retrieves a boolean result upon its closing.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        /// <returns>Returns value of clicked button on Notification.</returns>
        internal static bool YesNoNotification(string message, string title) => Application.Current.Dispatcher.Invoke(() => (new Notification(message, title, NotificationButtons.YesNo, MainWindow).ShowDialog() == true));

        #endregion Notification Management
    }
}