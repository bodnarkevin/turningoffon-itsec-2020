﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CaffStore.Backend.Interface.Bll.Dtos.Caff
{
	public class CaffItemDetailsDto : CaffItemDto
	{
		public CaffDataDto CaffData { get; set; }
	}
}
