using Contacts.Classes.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contacts.Classes.Database
{
    internal interface IDatabaseInteraction
    {/// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
     /// <returns>Returns true once the database has been validated</returns>
        void VerifyDatabaseIntegrity();

        #region Contact Lens Manipulation

        /// <summary>Adds a new contact insertion to the database.</summary>
        /// <param name="newContact">Contact insertion to be added</param>
        Task<bool> AddContact(Contact newContact);

        /// <summary>Loads all contact insertions from the database.</summary>
        /// <returns>All contact insertions</returns>
        Task<List<Contact>> LoadContacts();

        #endregion Contact Lens Manipulation
    }
}