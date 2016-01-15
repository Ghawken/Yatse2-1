using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace Remote.Emby.Api
{
	
	public class Director
	{
		// ATTRIBUTES
		[XmlAttribute("id")]
		public int id  { get; set; }
		
		[XmlAttribute("tag")]
		public string tag { get; set; }
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
		public Director()
		{}
	}
}
