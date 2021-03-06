﻿namespace System.Security.Cryptography.X509Certificates {
	/// <summary>
	///		<strong>X509IssuerAlternativeNamesExtension</strong> represents a X.509 issuer alternative names extension.
	///		The issuer alternative name extension allows identities to be bound to the issuer of the certificate.
	///		Issuer alternative names are not processed as part of the certification path validation algorithm.
	///		That is, issuer alternative names are not used in name chaining and name constraints are not enforced.
	/// </summary>
	public sealed class X509IssuerAlternativeNamesExtension : X509Extension {
		readonly Oid oid = new Oid("2.5.29.18");
		X509AlternativeNameCollection alternativeNames = new X509AlternativeNameCollection();

		internal X509IssuerAlternativeNamesExtension(Byte[] rawData, Boolean critical)
            : base("2.5.29.18", rawData, critical) {
			if (rawData == null) { throw new ArgumentNullException("rawData"); }
			m_decode(rawData);
		}

		/// <summary>
		///		Initializes a new instance of the <strong>X509IssuerAlternativeNamesExtension</strong> class using an
		///		<see cref="AsnEncodedData"/> object and a value that identifies whether the extension is critical.
		/// </summary>
		/// <param name="altNames">The encoded data to use to create the extension.</param>
		/// <param name="critical">
		///		<strong>True</strong> if the extension is critical; otherwise, <strong>False</strong>.
		/// </param>
		public X509IssuerAlternativeNamesExtension(AsnEncodedData altNames, Boolean critical) : this(altNames.RawData, critical) { }
		/// <summary>
		///		Initializes a new instance of the <strong>X509IssuerAlternativeNamesExtension</strong> class using a
		///		collection of alternative names and a value that identifies whether the extension is critical.
		/// </summary>
		/// <param name="altNames">A collection of alternative name objects.</param>
		/// <param name="critical">
		///		<strong>True</strong> if the extension is critical; otherwise, <strong>False</strong>.
		/// </param>
		public X509IssuerAlternativeNamesExtension(X509AlternativeNameCollection altNames, Boolean critical) {
			if (altNames.Count == 0) { throw new ArgumentException("Empty arrays are not supported."); }
			m_initizlize(altNames, critical);
		}

		/// <summary>
		/// Gets an array of alternative names.
		/// </summary>
		public X509AlternativeNameCollection AlternativeNames {
			get {
				X509AlternativeNameCollection retValue = new X509AlternativeNameCollection();
				foreach (X509AlternativeName item in alternativeNames) {
					retValue.Add(item);
				}
				return retValue;
			}
		}

		void m_initizlize(X509AlternativeNameCollection altNames, Boolean critical) {
			Critical = critical;
			Oid = oid;
			RawData = altNames.Encode();
			alternativeNames = altNames;
		}
		void m_decode(Byte[] rawData) {
            alternativeNames.Decode(rawData);
        }
	}
}
