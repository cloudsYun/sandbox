using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SandBox.Actions
{
	public enum Material
	{ 
		R1 = 1,
		R2,
		R3,
		R4
	}

	public enum Product
	{ 
		P1 = 1,
		P2,
		P3,
		P4
	}

	public enum Market
	{
		本地市场 = 1,
		区域市场,
		国内市场,
		亚洲市场,
		国际市场
	}

	public class Quotation 
	{
		public Product 产品类型 { get; set; }
		public Market 市场类型 { get; set; }
		public String 所需数量 { get; set; }
		public String 订单账期 { get; set; }
		public String 订单总价 { get; set; }
	}
	
}
