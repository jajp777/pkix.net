﻿using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using PKI.Utils.CLRExtensions;
using SysadminsLV.Asn1Parser;

namespace System.Security.Cryptography.X509Certificates {
	/// <summary>
	///		Represents a single alternative name used in <see cref="X509SubjectAlternativeNamesExtension"/> and
	///		<see cref="X509IssuerAlternativeNamesExtension"/> classes.
	///		This class supports all alternative name types defined in
	///		<see href="http://tools.ietf.org/html/rfc5280">RFC 5280</see> and supports two Microsoft
	///		proprietary alternative names: <strong>Guid</strong> and <strong>User Principal Name</strong> (<i>UPN</i>).
	/// </summary>
	public class X509AlternativeName {

		///  <summary>
		/// 	Initializes a new instance of the <strong>X509AlternativeName</strong> class by using alternative name
		/// 	type and alternative name value.
		///  </summary>
		///  <param name="type">
		/// 	Specifies the type of the alternative name contained in the <strong>value</strong> parameter.
		///  </param>
		///  <param name="value">
		/// 	Specifies the alternative name value. For acceptable value types see <strong>Remarks</strong> section.
		///  </param>
		/// <exception cref="ArgumentNullException">
		///		<strong>value</strong> parameter is null reference.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// 	The constructor cannot be used for <strong>OtherName</strong> type.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// 	The value does not fall within the expected type. For acceptable value types see <strong>Remarks</strong> section.
		/// </exception>
		/// <remarks>
		/// 	The following table defines alternative name type and value type mappings:
		/// <list type="table">
		/// 	<listheader>
		/// 		<term>Alternative Name Type</term>
		/// 		<description>Value Type</description>
		/// 	</listheader>
		/// 	<item>
		/// 		<term>OtherName</term>
		/// 		<description>Not supported in this constructor.</description>
		/// 	</item>
		/// 	<item>
		/// 	<term>Rfc822Name</term>
		/// 		<description>The value must be a string.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>DnsName</term>
		/// 		<description>The value must be a string.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>DirectoryName</term>
		/// 		<description>
		/// 			Can be a string that represents a X.500 distinguished name, or a <see cref="X500DistinguishedName"/>
		/// 			object.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>URL</term>
		/// 		<description>Can be a string that represents an absulute or relative URL or a <see cref="Uri"/> object.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>IpAddress</term>
		/// 		<description>
		///             <para>
		/// 			Must be a string that represents either, IPv4 or IPv6 address. For IPv6 address shortcuts
		/// 			are allowed (for example, ::1).
		///             </para>
		///             <para>
		///             For X.509 Name Constraints certificate extension purposes, you must provide either, IPv4
		///             or IPv6 address with network mask. In this case, network mask must be specified as the
		///             number of bits held by mask after a slash character. For example, IPv4 network "192.168.5.0"
		///             with network mask "255.255.255.0" must be passed as "192.168.5.0/24". The same syntax is used
		///             for IPv6 networks. To specify individual IP address, network mask bust be set to 32 for IPv4
		///             addresses and 128 for IPv6 addresses.
		///             </para>
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>RegisteredId</term>
		/// 		<description>
		/// 			Can be a string that represents a registered in the <strong>IANA</strong> (<i>Internet Assigned
		/// 			Numbers Authority</i>) or <strong>ISO</strong> (<i>International Standards Organization</i>) object
		/// 			identifier, or an instance of <see cref="Oid"/> object or <see cref="Oid2"/> object that contains
		/// 			registered object identifier. 
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>Guid</term>
		/// 		<description>
		/// 			Can be a string that represents a globally unique identifier and should identify a server
		/// 			to the Active Directory replication system as a domain controller or an instance of the
		/// 			<see cref="Guid"/> object.
		/// 		</description>
		/// 	</item>
		/// 	<item>
		/// 		<term>UserPrincipalName</term>
		/// 		<description>
		/// 			Must be a string that is a user logon name in email address format.
		/// 		</description>
		///		</item>
		/// </list>
		/// </remarks>
		public X509AlternativeName(X509AlternativeNamesEnum type, Object value) {
			if (value == null) { throw new ArgumentNullException("value"); }
			if (type == X509AlternativeNamesEnum.OtherName) { throw new InvalidOperationException("Invalid constructor."); }
			m_initialize(type, value);
		}
		///   <summary>
		///  		Initializes a new instance of the <strong>X509AlternativeName</strong> class by using alternative name
		///  		type, alternative name value and alternative name object identifier. This constructor supports only
		///  		<strong>OtherName</strong> type which can be either: <strong>OtherName</strong>, <strong>Guid</strong>
		///  		or <strong>UserPrincipalName</strong>. Use additional constructor for the rest alternative name types.
		///   </summary>
		///   <param name="type">
		///  		Specifies the type of the alternative name contained in the <strong>value</strong> parameter. Type can be either:
		///  		<strong>OtherName</strong>, <strong>Guid</strong> or <strong>UserPrincipalName</strong>.
		///   </param>
		///   <param name="value">
		/// 		Specifies the alternative name value. This parameter accepts either, string or byte array that
		/// 		represents other name value.
		///   </param>
		/// <param name="oid">
		///		Specifies the object identifier of the other name.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// 		<strong>rawData</strong> value is null reference.
		///  </exception>
		///  <exception cref="InvalidOperationException">
		///  		The constructor cannot be used for <strong>OtherName</strong> type.
		///   </exception>
		///   <exception cref="ArgumentException">
		///  		The value does not fall within the expected type.
		///   </exception>
		public X509AlternativeName(X509AlternativeNamesEnum type, Object value, Oid oid) {
			if (
				type != X509AlternativeNamesEnum.OtherName &&
				type != X509AlternativeNamesEnum.Guid &&
				type != X509AlternativeNamesEnum.UserPrincipalName
				) { throw new InvalidOperationException("Invalid constructor."); }
			if (value == null) { throw new ArgumentNullException("value"); }
			if (oid == null || String.IsNullOrEmpty(oid.Value)) { throw new ArgumentNullException("oid"); }
			encodeOtherName(value, oid);
		}
		///  <summary>
		///  Initializes a new instance of the <strong>X509AlternativeName</strong> class by using a ASN.1-encoded
		/// 	byte array that represents a particular alternative name.
		///  </summary>
		///  <param name="rawData">ASN.1-encoded byte array that represents a particular alternative name.</param>
		/// <exception cref="ArgumentNullException"><strong>rawData</strong> is null reference.</exception>
		/// <exception cref="ArgumentException">
		///		The data in the <strong>rawData</strong> argument is invalid or alternative name type cannot be determined.
		/// </exception>
		public X509AlternativeName(Byte[] rawData) {
			if (rawData == null) { throw new ArgumentNullException("rawData"); }
			decodeFromRawData(rawData);
		}

		/// <summary>
		/// Gets the type of alternative name contained in the <see cref="Value"/> property.
		/// </summary>
		public X509AlternativeNamesEnum Type { get; private set; }
		/// <summary>
		/// Gets an object idientifier of the other name. For common names, this property returns null.
		/// </summary>
		public Oid OID { get; private set; }
		/// <summary>
		/// Gets textual representation of the alternative name.
		/// </summary>
		public String Value { get; private set; }
		/// <summary>
		/// Gets ASN.1-encoded alternative name value in the byte array form.
		/// </summary>
		public Byte[] RawData { get; private set; }

		// constructor initializers
		void m_initialize(X509AlternativeNamesEnum type, Object value) {
			Type = type;
			encodeFromValue(value);
		}

		// main encoder
		void encodeFromValue(Object value) {
			switch (Type) {
				case X509AlternativeNamesEnum.Rfc822Name:
					encodeEmailName((String)value);
					break;
				case X509AlternativeNamesEnum.DnsName:
					encodeDnsName((String)value);
					break;
				case X509AlternativeNamesEnum.DirectoryName:
					encodeDirectoryName(value);
					break;
				case X509AlternativeNamesEnum.URL:
					encodeUrl(value);
					break;
				case X509AlternativeNamesEnum.IpAddress:
					encodeIpAddress((String)value);
					break;
				case X509AlternativeNamesEnum.RegisteredId:
					encodeRegisteredId(value);
					break;
				case X509AlternativeNamesEnum.Guid:
					encodeGuid(value);
					break;
				case X509AlternativeNamesEnum.UserPrincipalName:
					encodeUPN((String)value);
					break;
				default: throw new ArgumentException();
			}
		}
		// encoders
		void encodeOtherName(Object value, Oid oid) {
			Byte[] rawBytes;
			if (value as String != null) {
				rawBytes = Encoding.UTF8.GetBytes((String)value);
			} else if (value as Byte[] != null) {
				rawBytes = (Byte[])value;
			} else {
				throw new ArgumentException("Input data must be either string or byte array.");
			}
			Asn1Type tag;
			switch (oid.Value) {
				// UPN
				case "1.3.6.1.4.1.311.20.2.3":
					tag = Asn1Type.UTF8String;
					Type = X509AlternativeNamesEnum.UserPrincipalName;
					break;
				// Guid
				case "1.3.6.1.4.1.311.25.1":
					tag = Asn1Type.OCTET_STRING;
					Type = X509AlternativeNamesEnum.Guid;
					break;
				// Other name;
				default:
					Value = String.Empty;
					foreach (Byte B in rawBytes) {
						Value += String.Format("{0:x2}", B) + " ";
					}
					Value = Value.Trim();
					tag = Asn1Type.OCTET_STRING;
					Type = X509AlternativeNamesEnum.OtherName;
					break;
			}
			if (String.IsNullOrEmpty(Value)) {
				Value = Encoding.UTF8.GetString(rawBytes);
			}
			OID = oid;
			RawData = Asn1Utils.Encode(rawBytes, (Byte)tag);
			RawData = Asn1Utils.Encode(RawData, 160);
			List<Byte> tempBytes = new List<Byte>(Asn1Utils.EncodeObjectIdentifier(oid));
			tempBytes.AddRange(RawData);
			RawData = Asn1Utils.Encode(tempBytes.ToArray(), 160);
		}
		void encodeEmailName(String value) {
			try {
				//MailAddress address = new MailAddress(value);
				Value = value;
				RawData = Asn1Utils.Encode(Encoding.UTF8.GetBytes(Value), 129);
			} catch { throw new ArgumentException("The string is not valid Rfc822 name."); }
		}
		void encodeDnsName(String value) {
			try {
				Value = value;
				RawData = Asn1Utils.Encode(Encoding.UTF8.GetBytes(Value), 130);
			} catch { throw new ArgumentException("The string is not valid DNS name"); }
		}
		void encodeDirectoryName(Object value) {
			X500DistinguishedName name;
			if (value as String != null) {
				try {
					name = new X500DistinguishedName((String)value);
				} catch { throw new ArgumentException("The string is not valid X.500 name."); }
			} else {
				try {
					name = new X500DistinguishedName((X500DistinguishedName)value);
				} catch { throw new ArgumentException("The string is not valid X500DistinguishedName object."); }
			}
			Value = name.Name;
			RawData = Asn1Utils.Encode(name.RawData, 164);
		}
		void encodeUrl(Object value) {
			Uri url;
			if (value as String != null) {
				try {
					url = new Uri((String)value);
				} catch { throw new ArgumentException("The string is not valid URL."); }
			} else {
				try {
					url = (Uri)value;
				} catch { throw new ArgumentException("The string is not valid Uri object."); }
			}
			Value = url.AbsoluteUri;
			RawData = Asn1Utils.Encode(Encoding.UTF8.GetBytes(Value), 134);
		}
		void encodeIpAddress(String value) {
            Boolean ipv4 = value.Contains('.');
			List<Byte> bytes = new List<Byte>();
            String[] tokens = value.Split('/');
            value = tokens[0];
            String netMask = String.Empty;
			try {
                bytes.AddRange(IPAddress.Parse(value).GetAddressBytes());
			} catch {
				throw new ArgumentException("The specified value is not valid IPv4 or IPv6 address.");
			}
            if (tokens.Length == 2) {
                netMask = "/" + tokens[1];
                var maskLength = Convert.ToByte(tokens[1]);
                if (ipv4 && maskLength > 32) {
                    throw new ArgumentException("The IPv4 netmask value is invalid.");
                }
                if (!ipv4 && maskLength > 128) {
                    throw new ArgumentException("The IPv6 netmask value is invalid.");
                }
                if (ipv4) {
                    Int32 bits = 32 - maskLength;
                    Int32 mask = ~(0 | ((Int32)Math.Pow(2, bits) - 1));
                    bytes.AddRange(BitConverter.GetBytes(mask).Reverse());
                } else {
                    // ipv6
                    Int32 maskBits = maskLength;
                    Int32 shiftBits = 128 - maskBits;
                    BigInteger mask = ((BigInteger)Math.Pow(2, maskBits) - 1) << shiftBits;
                    bytes.AddRange(mask.ToLittleEndianByteArray().Skip(1).Take(16).ToArray());
                }
            }
            Value = IPAddress.Parse(value) + netMask;
			RawData = Asn1Utils.Encode(bytes.ToArray(), 135);
		}
		void encodeRegisteredId(Object value) {
			Asn1Reader asn;
			switch (value.GetType().FullName) {
				case "System.String":
					Value = (String)value;
					Oid oid = new Oid((String)value);
					asn = new Asn1Reader(Asn1Utils.EncodeObjectIdentifier(oid));
					Value = oid.Value;
					break;
				case "System.Security.Oid":
					asn = new Asn1Reader(Asn1Utils.EncodeObjectIdentifier((Oid)value));
					Value = ((Oid)value).Value;
					break;
				case "System.Security.Oid2":
					asn = new Asn1Reader(Asn1Utils.EncodeObjectIdentifier(new Oid(((Oid2)value).Value)));
					Value = ((Oid2)value).Value;
					break;
				default: throw new ArgumentException("The input data is not valid registered ID.");
			}
			RawData = Asn1Utils.Encode(asn.GetPayload(), 136);
		}
		void encodeGuid(Object value) {
			Guid guid;
			if (value.GetType().FullName == "System.Guid") {
				guid = (Guid)value;
			} else {
				try {
					guid = new Guid((String)value);
					Value = (new Guid(RawData)).ToString();
				} catch { throw new ArgumentException("Input string is not valid Guid string."); }
			}
			encodeOtherName(guid.ToByteArray(), new Oid("1.3.6.1.4.1.311.25.1"));
		}
		void encodeUPN(String value) {
			try {
				//MailAddress address = new MailAddress(value);
				Value = value;
				encodeOtherName(Encoding.UTF8.GetBytes(value), new Oid("1.3.6.1.4.1.311.20.2.3"));
			} catch { throw new ArgumentException("The string is not valid user principal name."); }
		}

		// main decoder
		void decodeFromRawData(Byte[] rawData) {
			RawData = rawData;
			switch (rawData[0]) {
				case 129:
					decodeEmailName(); break;
				case 130:
					decodeDnsName(); break;
				case 134:
					decodeUrl(); break;
				case 135:
					decodeIpAddress(); break;
				case 136:
					decodeRegisteredId(); break;
				case 160:
					decodeOtherName(); break;
				case 164:
					decodeDirectoryName(); break;
			}
		}
		// decoders
		void decodeOtherName() {
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
				if (!asn.MoveNext()) { throw new ArgumentException("Input data is not valid OtherName."); }
				Oid oid = new Oid(Asn1Utils.DecodeObjectIdentifier(asn.GetTagRawData()));
				asn.MoveNext();
				if (asn.Tag != 160) { throw new ArgumentException("Input data is not valid OtherName."); }
				asn.MoveNext();
				OID = oid;
				switch (oid.Value) {
					// UPN
					case "1.3.6.1.4.1.311.20.2.3":
						Type = X509AlternativeNamesEnum.UserPrincipalName;
						Value = Encoding.UTF8.GetString(asn.GetPayload()); break;
					// GUID
					case "1.3.6.1.4.1.311.25.1":
						Guid guid = new Guid(asn.GetPayload());
						Type = X509AlternativeNamesEnum.Guid;
						Value = guid.ToString();
						break;
					default:
						Value = String.Empty;
						Type = X509AlternativeNamesEnum.OtherName;
						foreach (Byte B in asn.GetPayload()) {
							Value += String.Format("{0:x2}", B) + " ";
						}
						Value = Value.Trim();
						break;
				}
			} catch { throw new ArgumentException("Input data is not valid OtherName."); }
		}
		void decodeEmailName() {
			Type = X509AlternativeNamesEnum.Rfc822Name;
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
				Value = Encoding.UTF8.GetString(asn.GetPayload());
			} catch { throw new ArgumentException("Input data is not valid Rfc822 name."); }
		}
		void decodeDnsName() {
			Type = X509AlternativeNamesEnum.DnsName;
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
				Value = Encoding.UTF8.GetString(asn.GetPayload());
			} catch { throw new ArgumentException("Input data is not valid DNS name."); }
		}
		void decodeDirectoryName() {
			Type = X509AlternativeNamesEnum.DirectoryName;
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
				Value = (new X500DistinguishedName(asn.GetPayload())).Name;
			} catch { throw new ArgumentException("Input data is not valid X.500 distinguished name."); }
		}
		void decodeUrl() {
			Type = X509AlternativeNamesEnum.URL;
            Asn1Reader asn = new Asn1Reader(RawData);
            try {
                Value = new Uri(Encoding.UTF8.GetString(asn.GetPayload())).AbsoluteUri;
			} catch {
                Value = Encoding.UTF8.GetString(asn.GetPayload());
            }
		}
		void decodeIpAddress() {
			Type = X509AlternativeNamesEnum.IpAddress;
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
                Int32 takeBytes;
                Boolean maskPresented = false;
			    switch (asn.PayloadLength) {
                    case 4: takeBytes = 4; break;
                    case 16: takeBytes = 16; break;
                    case 8: takeBytes = 4; maskPresented = true; break;
                    case 32: takeBytes = 16; maskPresented = true; break;
                    default: throw new ArgumentException("Invalid IPv4 or IPv6 address length.");
			    }
				Value = new IPAddress(asn.GetPayload().Skip(0).Take(takeBytes).ToArray()).ToString();
                if (maskPresented) {
                    List<Byte> bytes = asn.GetPayload().Skip(takeBytes).Take(takeBytes).ToList();
                    if (bytes[0] > 127) { bytes.Add(0); }
                    BigInteger maskLength = new BigInteger(bytes.ToArray()).GetEnabledBitCount();
                    Value += "/" + maskLength;
                }
			} catch { throw new ArgumentException("Input data is not valid IPv4 or IPv6 address."); }
		}
		void decodeRegisteredId() {
			Type = X509AlternativeNamesEnum.RegisteredId;
			try {
				Asn1Reader asn = new Asn1Reader(RawData);
				Oid oid = Asn1Utils.DecodeObjectIdentifier(Asn1Utils.Encode(asn.GetPayload(), (Byte)Asn1Type.OBJECT_IDENTIFIER));
				Value = oid.Value;
			} catch { throw new ArgumentException("Input data is not valid registered ID."); }
		}

        /// <summary>
        /// Returns a formatted version of the Abstract Syntax Notation One (ASN.1)-encoded alternative name as a string.
        /// </summary>
        /// <param name="multiLine">
        ///		<strong>True</strong> if the return string should contain carriage returns; otherwise, <strong>False</strong>.
        /// </param>
        /// <returns>A formatted string that represents the Abstract Syntax Notation One (ASN.1)-encoded alternative name.</returns>
        public String Format(Boolean multiLine) {
			String retValue;
			const String p = "     ";
			String n = Environment.NewLine;
			switch (Type) {
				case X509AlternativeNamesEnum.OtherName:
					retValue = multiLine
						? "Other Name:" + n + p + OID.Value + "=" + Value + n
						: "Other Name:" + OID.Value + "=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.Rfc822Name:
					retValue = multiLine
						? "RFC822 Name=" + Value + n
						: "RFC822 Name=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.DnsName:
					retValue = multiLine
						? "DNS Name=" + Value + n
						: "DNS Name=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.DirectoryName:
					retValue = "Directory Address:";
					if (multiLine) {
						String[] rdns = Value.Split(new []{ ", " }, StringSplitOptions.RemoveEmptyEntries);
						retValue = rdns.Aggregate(retValue, (current, RDN) => current + (p + RDN + n));
						retValue = retValue.Trim();
					} else {
						retValue = "Directory Address:" + Value + ", ";
					}
					break;
				case X509AlternativeNamesEnum.URL:
					retValue = multiLine
						? "URL=" + Value + n
						: "URL=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.IpAddress:
					retValue = multiLine
						? "IP Address=" + Value + n
						: "IP Address=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.RegisteredId:
					retValue = multiLine
						? "Registered ID=" + Value + n
						: "Registered ID=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.Guid:
					retValue = multiLine
						? "Other Name:" + n + p + OID.FriendlyName + "=" + Value + n
						: "Other Name:" + OID.FriendlyName + "=" + Value + ", ";
					break;
				case X509AlternativeNamesEnum.UserPrincipalName:
					retValue = multiLine
						? "Other Name:" + n + p + OID.FriendlyName + "=" + Value + n
						: "Other Name:" + OID.FriendlyName + "=" + Value + ", ";
					break;
				default:
					retValue = multiLine
						? "Unknown type=" + n + p + Value + n
						: "Unknown type=" + Value + ", ";
					break;
			}
			retValue = retValue.Trim();
			return retValue[retValue.Length - 1] == ','
				? retValue.Substring(0, retValue.Length - 1)
				: retValue;
		}
		/// <summary>
		/// Displays an alternative name in text format.
		/// </summary>
		/// <returns>The alternative information.</returns>
		public override String ToString() {
			return Format(false);
		}
	}
}
