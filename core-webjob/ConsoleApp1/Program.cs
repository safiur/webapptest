using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			var sbNameSpace = "hemanttestsb";
			var sbPath = "testtopic";
			var sbPolicy = "Send";
			var sbKey = "mVvckbUWiTm7xOIpCs7mFGRtYfMUlcNVHrTfvQaWU8Q=";
			var expiry = new TimeSpan(100, 1, 1, 1);
			try
			{
				
				var serviceUri = ServiceBusEnvironment.CreateServiceUri("https", sbNameSpace, sbPath).ToString().Trim('/');

				var sas = SharedAccessSignatureTokenProvider.GetSharedAccessSignature(sbPolicy, sbKey, serviceUri, expiry);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
