using Shouldly;
using System.Linq;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class CountryTests
	{
		#region constants

		internal const string USER_WITH_COCOS_PHONE_NUMBER = @"{
			""id"": ""KDcuGIm1QgePTO8WbOqwIQ"",
			""first_name"": ""Jane"",
			""last_name"": ""Doe"",
			""email"": ""jane.doe@example.com"",
			""type"": 2,
			""phone_numbers"": [
				{
					""code"": ""+61"",
					""country"": ""CC"",
					""label"": ""Mobile"",
					""number"": ""0891621234"",
					""verified"": true
				}
			]
		}";

		private static readonly string[] ISO_3166_ALPHA2_CODES =
		{
			"AD", "AE", "AF", "AG", "AI", "AL", "AM", "AO", "AQ", "AR",
			"AS", "AT", "AU", "AW", "AX", "AZ", "BA", "BB", "BD", "BE",
			"BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ",
			"BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD",
			"CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR",
			"CU", "CV", "CW", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM",
			"DO", "DZ", "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI",
			"FJ", "FK", "FM", "FO", "FR", "GA", "GB", "GD", "GE", "GF",
			"GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS",
			"GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU",
			"ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IR", "IS", "IT",
			"JE", "JM", "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN",
			"KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK",
			"LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME",
			"MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ",
			"MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ", "NA",
			"NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU",
			"NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM",
			"PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RS",
			"RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI",
			"SJ", "SK", "SL", "SM", "SN", "SO", "SR", "SS", "ST", "SV",
			"SX", "SY", "SZ", "TC", "TD", "TF", "TG", "TH", "TJ", "TK",
			"TL", "TM", "TN", "TO", "TR", "TT", "TV", "TW", "TZ", "UA",
			"UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI",
			"VN", "VU", "WF", "WS", "YE", "YT", "ZA", "ZM", "ZW",
		};

		#endregion

		#region Tests

		[Fact]
		public void Parse_json_user_with_cocos_keeling_islands_phone_number()
		{
			// Act
			var result = JsonSerializer.Deserialize<User>(
				USER_WITH_COCOS_PHONE_NUMBER, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.Length.ShouldBe(1);
			result.PhoneNumbers[0].Country.ShouldBe(Country.Cocos_Keeling_Islands);
		}

		[Theory]
		[InlineData("BL", Country.Saint_Barthelemy)]
		[InlineData("BQ", Country.Bonaire_Sint_Eustatius_and_Saba)]
		[InlineData("CC", Country.Cocos_Keeling_Islands)]
		[InlineData("CU", Country.Cuba)]
		[InlineData("CW", Country.Curacao)]
		[InlineData("CX", Country.Christmas_Island)]
		[InlineData("EH", Country.Western_Sahara)]
		[InlineData("HM", Country.Heard_Island_and_McDonald_Islands)]
		[InlineData("IR", Country.Islamic_Republic_of_Iran)]
		[InlineData("KP", Country.Democratic_People_s_Republic_of_Korea)]
		[InlineData("PN", Country.Pitcairn)]
		[InlineData("SD", Country.Sudan)]
		[InlineData("SH", Country.Saint_Helena_Ascension_and_Tristan_da_Cunha)]
		[InlineData("SJ", Country.Svalbard_and_Jan_Mayen)]
		[InlineData("SX", Country.Sint_Maarten)]
		[InlineData("SY", Country.Syrian_Arab_Republic)]
		public void Parse_country_code(string countryCode, Country expected)
		{
			// Act
			var result = countryCode.ToEnum<Country>();

			// Assert
			result.ShouldBe(expected);
		}

		[Fact]
		public void Every_iso_3166_alpha2_code_resolves()
		{
			// Act
			var unmapped = ISO_3166_ALPHA2_CODES
				.Where(code => !code.TryToEnum<Country>(out _))
				.ToArray();

			// Assert
			unmapped.ShouldBeEmpty();
		}

		#endregion
	}
}
