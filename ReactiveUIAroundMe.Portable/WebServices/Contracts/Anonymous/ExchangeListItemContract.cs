// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EReactiveUIAroundMeListItemContract.cs" company="Champion Data">
//   Copyright (c) 2015 Champion Data All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using System;

	using Newtonsoft.Json;

	/// <summary>
	/// EReactiveUIAroundMe list item contract.
	/// </summary>
	public class EReactiveUIAroundMeListItemContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		[JsonProperty(PropertyName = "city", NullValueHandling = NullValueHandling.Ignore)]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the name of the company.
		/// </summary>
		/// <value>The name of the company.</value>
		[JsonProperty(PropertyName = "companyName", NullValueHandling = NullValueHandling.Ignore)]
		public string CompanyName { get; set; }

		/// <summary>
		/// Gets or sets the company route identifier.
		/// </summary>
		/// <value>The company route identifier.</value>
		[JsonProperty(PropertyName = "companyRouteId", NullValueHandling = NullValueHandling.Ignore)]
		public string CompanyRouteId { get; set; }

		/// <summary>
		/// Gets or sets the created.
		/// </summary>
		/// <value>The created.</value>
		[JsonProperty(PropertyName = "created", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		[JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the eReactiveUIAroundMe number.
		/// </summary>
		/// <value>The eReactiveUIAroundMe number.</value>
		[JsonProperty(PropertyName = "eReactiveUIAroundMeNumber", NullValueHandling = NullValueHandling.Ignore)]
		public int EReactiveUIAroundMeNumber { get; set; }

		/// <summary>
		/// Gets or sets the eReactiveUIAroundMe version.
		/// </summary>
		/// <value>The eReactiveUIAroundMe version.</value>
		[JsonProperty(PropertyName = "eReactiveUIAroundMeVersion", NullValueHandling = NullValueHandling.Ignore)]
		public int EReactiveUIAroundMeVersion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.WebServices.EReactiveUIAroundMeListItemContract"/>
		/// geo location retrieved.
		/// </summary>
		/// <value><c>true</c> if geo location retrieved; otherwise, <c>false</c>.</value>
		[JsonProperty(PropertyName = "geoLocationRetrieved", NullValueHandling = NullValueHandling.Ignore)]
		public bool GeoLocationRetrieved { get; set; }

		/// <summary>
		/// Gets or sets the home phone.
		/// </summary>
		/// <value>The home phone.</value>
		[JsonProperty(PropertyName = "homePhone", NullValueHandling = NullValueHandling.Ignore)]
		public string HomePhone { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the image identifier.
		/// </summary>
		/// <value>The image identifier.</value>
		[JsonProperty(PropertyName = "imageId", NullValueHandling = NullValueHandling.Ignore)]
		public string ImageId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.WebServices.EReactiveUIAroundMeListItemContract"/>
		/// info available.
		/// </summary>
		/// <value><c>true</c> if info available; otherwise, <c>false</c>.</value>
		[JsonProperty(PropertyName = "infoAvailable", NullValueHandling = NullValueHandling.Ignore)]
		public bool InfoAvailable { get; set; }

		/// <summary>
		/// Gets or sets the mobile phone.
		/// </summary>
		/// <value>The mobile phone.</value>
		[JsonProperty(PropertyName = "mobilePhone", NullValueHandling = NullValueHandling.Ignore)]
		public string MobilePhone { get; set; }

		/// <summary>
		/// Gets or sets the postcode.
		/// </summary>
		/// <value>The postcode.</value>
		[JsonProperty(PropertyName = "postcode", NullValueHandling = NullValueHandling.Ignore)]
		public string Postcode { get; set; }

		/// <summary>
		/// Gets or sets the sequence.
		/// </summary>
		/// <value>The sequence.</value>
		[JsonProperty(PropertyName = "sequence", NullValueHandling = NullValueHandling.Ignore)]
		public int Sequence { get; set; }

		/// <summary>
		/// Gets or sets the street.
		/// </summary>
		/// <value>The street.</value>
		[JsonProperty(PropertyName = "street", NullValueHandling = NullValueHandling.Ignore)]
		public string Street { get; set; }

		/// <summary>
		/// Gets or sets the suburb.
		/// </summary>
		/// <value>The suburb.</value>
		[JsonProperty(PropertyName = "suburb", NullValueHandling = NullValueHandling.Ignore)]
		public string Suburb { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		/// <value>The country.</value>
		[JsonProperty(PropertyName = "country", NullValueHandling = NullValueHandling.Ignore)]
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the work phone.
		/// </summary>
		/// <value>The work phone.</value>
		[JsonProperty(PropertyName = "workPhone", NullValueHandling = NullValueHandling.Ignore)]
		public string WorkPhone { get; set; }

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		[JsonProperty(PropertyName = "distance", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Distance { get; set; }

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		[JsonProperty(PropertyName = "latitude", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		[JsonProperty(PropertyName = "longitude", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Longitude { get; set; }

		/// <summary>
		/// Gets or sets the bearing.
		/// </summary>
		/// <value>The bearing.</value>
		[JsonProperty(PropertyName = "bearing", NullValueHandling = NullValueHandling.Ignore)]
		public double Bearing { get; set; }
	}
}
