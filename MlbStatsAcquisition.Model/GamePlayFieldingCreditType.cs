using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public enum GamePlayFieldingCreditType
	{
		None = 0,
		Putout = 1,
		Assist = 10,
		Assist_OF = 11,
		Fielded = 20,
		Error_Throwing = 31,
		Error_Fielding = 32,
		Error_DroppedBall = 33,
		Deflection = 40,
		Touch = 41,
		Interference = 42
	}
	public class RefGamePlayFieldingCreditType
	{
		public GamePlayFieldingCreditType CreditType { get; set; }
		public string CreditName { get; set; }

		public virtual ICollection<GamePlayFieldingCredit> FieldingCredits { get; set; }
	}
}
