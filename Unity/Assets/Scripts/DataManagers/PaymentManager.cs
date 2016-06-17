using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataManagers
{
	public class PaymentManager:Tools.XSingleton<PaymentManager>
	{

		public const string FILE_NAME= "__PAYMENT_FILE.json";

		public PaymentManager ()
		{
			
		}

		private List<string> _payamentHistory = new List<string>();


		public void Add(string str)
		{
			_payamentHistory.Add (str);
		}


		public void Tick()
		{

			if (_payamentHistory.Count == 0)
				return;
			foreach (var i in _payamentHistory) {
			   
				GamePlayerManager.Singleton.DoPaymentBuyKey (i); 
			}

			_payamentHistory.Clear ();

		}
	}
}

