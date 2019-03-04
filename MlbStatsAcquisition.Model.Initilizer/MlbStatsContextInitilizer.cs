using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model.Initilizer
{
	public class MlbStatsContextInitilizer : DropCreateDatabaseAlways<MlbStatsContext>
	{
		public override void InitializeDatabase(MlbStatsContext context)
		{
			base.InitializeDatabase(context);

			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.FirstBase, Name = "First Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.SecondBase, Name = "Second Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.ThirdBase, Name = "Third Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.HomePlate, Name = "Home Plate" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.LeftField, Name = "Left Field" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.RightField, Name = "Right Field" });

			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.None, CreditName = "None" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Putout, CreditName = "Putout" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Assist, CreditName = "Assist" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Assist_OF, CreditName = "Assist_OF" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Fielded, CreditName = "Fielded" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Error_Fielding, CreditName = "Error_Fielding" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Error_Throwing, CreditName = "Error_Throwing" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Error_DroppedBall, CreditName = "Error_DroppedBall" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Deflection, CreditName = "Deflection" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Touch, CreditName = "Touch" });
			context.GamePlayFieldingCreditTypes.Add(new RefGamePlayFieldingCreditType { CreditType = GamePlayFieldingCreditType.Interference, CreditName = "Interference" });

			context.SaveChanges();
		}
	}
}
