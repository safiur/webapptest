using ConsoleAppSB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSB.Services
{
	public class DataStore : IDataStore
	{
		public void ProcessData()
		{
			Console.WriteLine("Processing data...");
		}
	}
}
