using System;

namespace SocialCapital.Services.AddressBookImport.Converters
{
	public class ConverterException : Exception
	{
		public ConverterException (string msg) : base (msg){
		}
	}
}

