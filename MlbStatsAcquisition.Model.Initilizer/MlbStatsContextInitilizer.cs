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

			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.FirstBase,  Name = "First Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.SecondBase, Name = "Second Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.ThirdBase,  Name = "Third Base" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.HomePlate,  Name = "Home Plate" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.LeftField,  Name = "Left Field" });
			context.UmpireTypes.Add(new RefUmpireType { UmpireType = UmpireType.RightField, Name = "Right Field" });

			context.SaveChanges();
		}
	}
}
