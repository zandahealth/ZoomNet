using Shouldly;
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

		#endregion
	}
}
