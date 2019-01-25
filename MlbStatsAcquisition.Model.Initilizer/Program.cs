using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model.Initilizer
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var context = new MlbStatsContext())
			{
				var init = new MlbStatsContextInitilizer();
				init.InitializeDatabase(context);
			}
		}
	}
}
