using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	// Is* PROPERTIES REPRESENT ALL OPTIONS FROM statGroups MLB API
	public class StatType : MlbStatsEntity
	{
		public int StatTypeID { get; set; }
		public string Lookup { get; set; }
		public string Name { get; set; }
		public bool IsCounting { get; set; }
		public string Label { get; set; }
		public bool IsFielding { get; set; }
		public bool IsCatching { get; set; }
		public bool IsPitching { get; set; }
		public bool IsHitting { get; set; }
		public bool IsRunning { get; set; }
		public bool IsGame { get; set; }
		public bool IsTeam { get; set; }
		public bool IsStreak { get; set; }

	}
}
