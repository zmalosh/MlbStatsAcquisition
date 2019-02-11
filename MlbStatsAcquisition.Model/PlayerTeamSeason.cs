﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class PlayerTeamSeason
	{
		public int PlayerID { get; set; }
		public int TeamID { get; set; }
		public int Season { get; set; }

		public virtual Player Player { get; set; }
		public virtual TeamSeason TeamSeason { get; set; }
		public virtual ICollection<PlayerHittingBoxscore> PlayerHittingBoxscores { get; set; }
	}
}