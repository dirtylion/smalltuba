// -----------------------------------------------------------------------
// <copyright file="FileSaver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SmallTuba.Entities;
    using SmallTuba.PdfGenerator;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileSaver
    {
        public void SaveVoterList(List<Person> persons, string path, string electionName, string electionDate)
        {
            Dictionary<string, VoterList> voterlists = this.CreateVoterListsForVenue(persons, electionName, electionDate);
            this.AddVotersToVoterlists(persons, voterlists);
            this.SaveVoterListsToDisk(path, voterlists);
        }

        private Dictionary<string, VoterList> CreateVoterListsForVenue(List<Person> persons, string electionName, string electionDate)
        {
            Dictionary<string, VoterList> voterlists = new Dictionary<string, VoterList>();
            foreach (var person in persons)
            {
                if (!voterlists.ContainsKey(person.PollingTable))
                {
                    voterlists.Add(person.PollingTable, new VoterList(50, electionName, electionDate, person.PollingTable));
                }
            }
            return voterlists;
        }

        private void AddVotersToVoterlists(List<Person> persons, Dictionary<string, VoterList> voterlists)
        {
            foreach (var person in persons)
            {
                voterlists[person.PollingTable].AddVoter(person);
            }
        }

        private void SaveVoterListsToDisk(string path, Dictionary<string, VoterList> voterlists)
        {
            foreach (var pollingTabel in voterlists.Keys)
            {
                voterlists[pollingTabel].SaveToDisk(path + "\\" + "Bord"+pollingTabel + ".pdf");
            }
        }

        public void SavePollingCards(PollingVenue pollingVenue, string path, string electionName, string electionDate)
        {
            PollingCards pollingCards = new PollingCards(electionName, electionDate, "09.00 - 20.00");

            foreach (var person in pollingVenue.Persons)
            {
                pollingCards.CreatePollingCard(person, pollingVenue.MunicipalityAddress, pollingVenue.PollingVenueAddress);
            }
            pollingCards.SaveToDisk(path);
        }

    }
}
