﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GameEventType : MlbStatsEntity
	{
		public byte GameEventTypeID { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
	}
}
