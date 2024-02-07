namespace MProj6.DataAccess.Entities
{
	public class WebHook
	{
		// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
		public class Amount
		{
			public string value { get; set; }
			public string currency { get; set; }
		}

		public class AuthorizationDetails
		{
			public string rrn { get; set; }
			public string auth_code { get; set; }
			public ThreeDSecure three_d_secure { get; set; }
		}

		public class Card
		{
			public string first6 { get; set; }
			public string last4 { get; set; }
			public string expiry_year { get; set; }
			public string expiry_month { get; set; }
			public string card_type { get; set; }
			public string issuer_country { get; set; }
		}

		public class Metadata
		{
			public string payload { get; set; }
			public string cms_name { get; set; }
		}

		public class Object
		{
			public string id { get; set; }
			public string status { get; set; }
			public Amount amount { get; set; }
			public Recipient recipient { get; set; }
			public PaymentMethod payment_method { get; set; }
			public DateTime created_at { get; set; }
			public DateTime expires_at { get; set; }
			public bool test { get; set; }
			public bool paid { get; set; }
			public bool refundable { get; set; }
			public Metadata metadata { get; set; }
			public AuthorizationDetails authorization_details { get; set; }
		}

		public class PaymentMethod
		{
			public string type { get; set; }
			public string id { get; set; }
			public bool saved { get; set; }
			public string title { get; set; }
			public Card card { get; set; }
		}

		public class Recipient
		{
			public string account_id { get; set; }
			public string gateway_id { get; set; }
		}

		public class Root
		{
			public string type { get; set; }
			public string @event { get; set; }
			public Object @object { get; set; }
		}

		public class ThreeDSecure
		{
			public bool applied { get; set; }
			public bool method_completed { get; set; }
			public bool challenge_completed { get; set; }
		}


	}
}
