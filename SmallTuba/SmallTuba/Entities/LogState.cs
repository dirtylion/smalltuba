
namespace SmallTuba.Entities
{
	public class LogState
	{
		private PersonState voter;

		private string action;

		public LogState(PersonState voter, string action)
		{
			this.voter = voter;
			this.action = action;
		}

		public PersonState Voter
		{
			get { return voter; }
		}

		public override string ToString()
		{
			// Needs the TimeConverter class to uncomment this
		   //  return voter.Time.ToLocalTime().Hour + ":" + voter.Time.ToLocalTime().Minute + " " + voter.FirstName + " " +
			 //      voter.LastName + " was " + action;

			return "";
		}
	}
}
