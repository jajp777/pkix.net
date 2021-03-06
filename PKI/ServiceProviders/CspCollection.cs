﻿using System;
using System.Collections;

namespace PKI.ServiceProviders {
	/// <summary>
	/// Represents a collection of <see cref="CspLegacy"/> objects.
	/// </summary>
	public class CspCollection : ICollection {
		private ArrayList m_list;

		/// <summary>
		/// Initializes a new instance of the <see cref="CspCollection"/> class without any <see cref="CspLegacy"/> information.
		/// </summary>
		public CspCollection() { m_list = new ArrayList(); }

		/// <summary>
		/// Gets the number of <see cref="CspLegacy"/> objects in a collection.
		/// </summary>
		public Int32 Count {
			get { return m_list.Count; }
		}

		/// <internalonly/>
		IEnumerator IEnumerable.GetEnumerator() {
			return new CspCollectionEnumerator(this);
		}
		/// <internalonly/> 
		void ICollection.CopyTo(Array array, Int32 index) {
			if (array == null) { throw new ArgumentNullException("array"); }
			if (array.Rank != 1) { throw new ArgumentException("Multidimensional arrays are not supported."); }
			if (index < 0 || index >= array.Length) { throw new ArgumentOutOfRangeException("Index is out of range."); }
			if (index + this.Count > array.Length) { throw new ArgumentException("Index is out of range."); }
			for (Int32 i = 0; i < this.Count; i++) {
				array.SetValue(this[i], index);
				index++;
			}
		}

		/// <summary>
		/// Adds an <see cref="CspLegacy"/> object to the <see cref="CspCollection"/> object.
		/// </summary>
		/// <remarks>Use this method to add an <see cref="CspLegacy"/> object to an existing collection at the current location.</remarks>
		/// <param name="entry">The <see cref="CspLegacy"/> object to add to the collection.</param>
		/// <returns>The index of the added <see cref="CspLegacy"/> object.</returns>
		public Int32 Add(CspLegacy entry) { return m_list.Add(entry); }
		/// <summary>
		/// Gets an <see cref="CspLegacy"/> object from the <see cref="CspCollection"/> object.
		/// </summary>
		/// <param name="index">The location of the <see cref="CspLegacy"/> object in the collection.</param>
		/// <returns></returns>
		public CspLegacy this[Int32 index] {
			get { return m_list[index] as CspLegacy; }
		}
		/// <summary>
		/// Gets an <see cref="CspLegacy"/> object from the <see cref="CspCollection"/> object by CSP name.
		/// </summary>
		/// <param name="name">A string that represents a <see cref="CspLegacy.Name">Name</see>
		/// property.</param>
		/// <remarks>Use this property to retrieve an <see cref="CspLegacy"/> object from an <see cref="CspCollection"/>
		/// object if you know the <see cref="CspLegacy.Name">Name</see> value of the <see cref="CspLegacy"/>
		/// object. You can use the <see cref="this[int]"/> property to retrieve an <see cref="CspLegacy"/> object if you know
		/// its location in the collection</remarks>
		/// <returns>An <see cref="CspLegacy"/> object.</returns>
		public CspLegacy this[String name] {
			get {
				foreach (CspLegacy entry in m_list) {
					if (entry.Name.ToLower() == name.ToLower()) { return entry; }
				}
				return null;
			}
		}
		/// <summary>
		/// Returns an <see cref="CspCollectionEnumerator"/> object that can be used to navigate the <see cref="CspCollection"/> object
		/// </summary>
		/// <returns>An <see cref="CspLegacy"/> object.</returns>
		public CspCollectionEnumerator GetEnumerator() {
			return new CspCollectionEnumerator(this);
		}
		/// <summary>
		/// Copies the <see cref="CspCollection"/> object into an array.
		/// </summary>
		/// <param name="array">The array to copy the <see cref="CspCollection"/> object into.</param>
		/// <param name="index">The location where the copy operation starts.</param>
		public void CopyTo(CspLegacy[] array, Int32 index) {
			((ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets a value that indicates whether access to the <see cref="CspCollection"/> object is thread safe.
		/// </summary>
		/// <remarks>Returns <strong>False</strong> in all cases.</remarks>
		public bool IsSynchronized {
			get { return false; }
		}
		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="CspCollection"/> object.
		/// </summary>
		/// <remarks><see cref="CspCollection"/> is not thread safe. Derived classes can provide their own
		/// synchronized version of the <see cref="CspCollection"/> class using this property. The synchronizing
		/// code must perform operations on the <strong>SyncRoot</strong> property of the <see cref="CspCollection"/>
		/// object, not directly on the object itself. This ensures proper operation of collections that are derived from
		/// other objects. Specifically, it maintains proper synchronization with other threads that might simultaneously
		/// be modifying the <see cref="CspCollection"/> object.</remarks>
		public Object SyncRoot {
			get { return this; }
		}
	}
	/// <summary>
	/// Provides the ability to navigate through an <see cref="CspCollection"/> object.
	/// </summary>
	public class CspCollectionEnumerator : IEnumerator {
		CspCollection m_entries;
		Int32 m_current;

		CspCollectionEnumerator() { }
		internal CspCollectionEnumerator(CspCollection entries) {
			m_entries = entries;
			m_current = -1;
		}
		/// <summary>
		/// Gets the current <see cref="CspLegacy"/> object in an <see cref="CspCollection"/> object.
		/// </summary>
		/// <remarks><p>After an enumerator is created, the <see cref="MoveNext"/> method must be called to advance the
		/// enumerator to the first element of the collection before reading the value of the <strong>Current</strong> property;
		/// otherwise, <strong>Current</strong> returns a null reference (Nothing in Visual Basic) or throws an exception.</p>
		/// <p><strong>Current</strong> also returns a null reference (Nothing in Visual Basic) or throws an exception if the last
		/// call to <see cref="MoveNext"/> returns false, which indicates that the end of the collection has been reached.</p>
		/// <p><strong>Current</strong> does not move the position of the enumerator, and consecutive calls to <strong>Current</strong>
		/// return the same object, until <see cref="MoveNext"/> is called.</p></remarks>
		public CspLegacy Current {
			get { return m_entries[m_current]; }
		}

		/// <internalonly/>
		Object IEnumerator.Current {
			get { return (Object)m_entries[m_current]; }
		}
		/// <summary>
		/// Advances to the next <see cref="CspLegacy"/> object in an <see cref="CspCollection"/> object
		/// </summary>
		/// <remarks><p>After an enumerator is created, it is positioned before the first element of the collection,
		/// and the first call to the <strong>MoveNext</strong> method moves the enumerator over the first element of the collection.
		/// Subsequent calls to <strong>MoveNext</strong> advances the enumerator past subsequent items in the collection.</p>
		/// <p>After the end of the collection is passed, calls to <strong>MoveNext</strong> return <strong>False</strong>.</p>
		/// <p>An enumerator is valid as long as the collection remains unchanged. If changes are made to the collection,
		/// such as adding, modifying, or deleting elements, the enumerator becomes invalid and the next call to MoveNext
		/// throws an <see cref="InvalidOperationException"/>.</p>
		/// </remarks>
		/// <returns><strong>True</strong>, if the enumerator was successfully advanced to the next element; <strong>False</strong>,
		/// if the enumerator has passed the end of the collection.</returns>
		public bool MoveNext() {
			if (m_current == ((int)m_entries.Count - 1)) { return false; }
			m_current++;
			return true;
		}
		/// <summary>
		/// Sets an enumerator to its initial position.
		/// </summary>
		/// <remarks>The initial position of an enumerator is before the first element in the <see cref="CspCollection"/> object.
		/// An enumerator remains valid as long as the collection remains unchanged. If changes are made to the collection, such
		/// as adding, modifying, or deleting elements, the enumerator becomes invalid and the next call to the <strong>Reset</strong>
		/// method throws an <see cref="InvalidOperationException"/>.</remarks>
		public void Reset() { m_current = -1; }
	}
}