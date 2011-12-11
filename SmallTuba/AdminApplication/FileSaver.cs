// -----------------------------------------------------------------------
// <copyright file="FileSaver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AdminApplication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using SmallTuba.Entities;
    using SmallTuba.PdfGenerator;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileSaver
    {
        private string path;

        public FileSaver(string path, string pollingVenueName)
        {
            this.path = Directory.CreateDirectory(path+"\\"+pollingVenueName).FullName;
        }

        public void SaveVoterList(List<Person> persons, string electionName, string electionDate)
        {
            Dictionary<string, VoterList> voterlists = this.CreateVoterListsForVenue(persons, electionName, electionDate);
            this.AddVotersToVoterlists(persons, voterlists);
            this.SaveVoterListsToDisk(voterlists);
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
            persons.Sort(Person.CprSort());
            foreach (var person in persons)
            {
                voterlists[person.PollingTable].AddVoter(person);
            }          
        }

        private void SaveVoterListsToDisk(Dictionary<string, VoterList> voterlists)
        {
            foreach (var pollingTabel in voterlists.Keys)
            {
                voterlists[pollingTabel].SaveToDisk(this.path + "\\" + "VoterListTabel"+pollingTabel + ".pdf");
            }
        }

        public void SavePollingCards(PollingVenue pollingVenue, string electionName, string electionDate)
        {
            PollingCards pollingCards = new PollingCards(electionName, electionDate, "09.00 - 20.00");

            foreach (var person in pollingVenue.Persons)
            {
                pollingCards.CreatePollingCard(person, pollingVenue.MunicipalityAddress, pollingVenue.PollingVenueAddress);
            }
            pollingCards.SaveToDisk(this.path+"\\PollingCards.pdf");
        }

        public void SaveVoters(List<Person> persons)
        {
            StreamWriter sw = new StreamWriter(this.path+"\\Voters.csv", false);
            sw.WriteLine("FirstName;LastName;Cpr;VoterId;");
            
            foreach (var person in persons)
            {
                sw.WriteLine(person.FirstName+";"+person.LastName+";"+person.Cpr+";"+person.VoterId);
            }
            sw.Close();
        }

    }
}
