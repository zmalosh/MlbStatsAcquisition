﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class JobType
	{
		public int JobTypeID { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public int? SortOrder { get; set; }
	}
}