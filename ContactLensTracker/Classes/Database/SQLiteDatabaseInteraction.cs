using Contacts.Classes.Entities;
using Contacts.Enums;
using Extensions;
using Extensions.DatabaseHelp;
using Extensions.DataTypeHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Contacts.Classes.Database
{
    internal class SQLiteDatabaseInteraction : IDatabaseInteraction
    {
        // ReSharper disable once InconsistentNaming
        private const string _DATABASENAME = "Contacts.sqlite";

        private readonly string _con = $"Data Source = {_DATABASENAME}; foreign keys = TRUE; Version = 3;";

        #region Database Interaction

        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        public void VerifyDatabaseIntegrity() => Functions.VerifyFileIntegrity(
            Assembly.GetExecutingAssembly().GetManifestResourceStream($"Contacts.{_DATABASENAME}"), _DATABASENAME);

        #endregion Database Interaction

        #region Contact Lens Manipulation

        /// <summary>Adds a new contact insertion to the database.</summary>
        /// <param name="newContact">Contact insertion to be added</param>
        public async Task<bool> AddContact(Contact newContact)
        {
            SQLiteCommand cmd = new SQLiteCommand
            {
                CommandText = "INSERT INTO Contacts([Date], [Side])" +
                              "VALUES(@date, @side)"
            };
            cmd.Parameters.AddWithValue("@date", newContact.DateToString);
            cmd.Parameters.AddWithValue("@side", newContact.SideToString);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Loads all contact insertions from the database.</summary>
        /// <returns>All contact insertions</returns>
        public async Task<List<Contact>> LoadContacts()
        {
            List<Contact> allContacts = new List<Contact>();
            DataSet ds = await SQLite.FillDataSet("SELECT * FROM Contacts", _con);
            if (ds.Tables[0].Rows.Count > 0)
            {
                allContacts.AddRange(from DataRow dr in ds.Tables[0].Rows select new Contact(DateTimeHelper.Parse(dr["Date"]), EnumHelper.Parse<Side>(dr["Side"].ToString())));
                allContacts = allContacts.OrderByDescending(contact => contact.Date)
                    .ThenBy(contact => contact.SideToString).ToList();
            }

            return allContacts;
        }

        #endregion Contact Lens Manipulation
    }
}