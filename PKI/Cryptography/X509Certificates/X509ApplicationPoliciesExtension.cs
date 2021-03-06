﻿using System.Linq;
using System.Collections.Generic;
using SysadminsLV.Asn1Parser;

namespace System.Security.Cryptography.X509Certificates {
	/// <summary>
	/// Represents a Microsoft's proprietary <strong>Application Policies</strong> extension which is another
	/// implementation of <strong>Enhanced Key Usage</strong> extension.
	/// </summary>
	public sealed class X509ApplicationPoliciesExtension : X509Extension {
		readonly Oid oid = new Oid("1.3.6.1.4.1.311.21.10");
		readonly List<Oid> oids = new List<Oid>();

		internal X509ApplicationPoliciesExtension(Byte[] rawData, Boolean critical)
            : base("1.3.6.1.4.1.311.21.10", rawData, critical) {
			if (rawData == null) { throw new ArgumentNullException("rawData"); }
			m_decode(rawData);
		}
		
		/// <summary>
		/// Initializes a new instance of the <strong>X509ApplicationPoliciesExtension</strong> class.
		/// </summary>
		public X509ApplicationPoliciesExtension() { Oid = oid; }
		/// <summary>
		/// Initializes a new instance of the <strong>X509ApplicationPoliciesExtension</strong> class using an
		/// <see cref="AsnEncodedData"/> object and a value that identifies whether the extension is critical.
		/// </summary>
		/// <param name="applicationPolicies">The encoded data to use to create the extension.</param>
		/// <param name="critical"><strong>True</strong> if the extension is critical; otherwise, <strong>False</strong>.</param>
		/// <exception cref="ArgumentException">The data in the <strong>applicationPolicies</strong> parameter is not valid extension value.</exception>
		public X509ApplicationPoliciesExtension(AsnEncodedData applicationPolicies, Boolean critical) : this(applicationPolicies.RawData, critical) { }
		/// <summary>
		/// Initializes a new instance of the <strong>X509ApplicationPoliciesExtension</strong> class from an array of application
		/// policy object identifiers (OID) and a value that identifies whether the extension is critical.
		/// </summary>
		/// <param name="applicationPolicies">A collection of application policy OIDs.</param>
		/// <param name="critical"><strong>True</strong> if the extension is critical; otherwise, <strong>False</strong>.</param>
		/// <exception cref="ArgumentNullException"><strong>applicationPolicies</strong> parameter is null.</exception>
		public X509ApplicationPoliciesExtension(OidCollection applicationPolicies, Boolean critical) {
			if (applicationPolicies == null || applicationPolicies.Count == 0) { throw new ArgumentNullException("applicationPolicies"); }
			m_initialize(applicationPolicies, critical);
		}

		/// <summary>
		/// Gets a collection of application policy object identifiers associated with extension.
		/// </summary>
		public OidCollection ApplicationPolicies {
			get {
				OidCollection aoids = new OidCollection();
				foreach (Oid OID in oids) {
					aoids.Add(OID);
				}
				return aoids;
			}
		}

		void m_initialize(OidCollection applicationPolicies, Boolean critical) {
			Oid = oid;
			Critical = critical;
			List<Byte> rawData = new List<Byte>();
			foreach (Oid aoid in applicationPolicies.Cast<Oid>().Where(aoid => !String.IsNullOrEmpty(aoid.Value))) {
				oids.Add(aoid);
				rawData.AddRange(Asn1Utils.Encode(Asn1Utils.EncodeObjectIdentifier(aoid), 48));
			}
			RawData = Asn1Utils.Encode(rawData.ToArray(), 48);
		}
		void m_decode(Byte[] rawData) {
			Asn1Reader asn = new Asn1Reader(rawData);
			if (asn.Tag != 48) { throw new ArgumentException("The data is invalid."); }
			asn.MoveNext();
			do {
				oids.Add(Asn1Utils.DecodeObjectIdentifier(asn.GetPayload()));
			} while (asn.MoveNextCurrentLevel());
		}
	}
}
