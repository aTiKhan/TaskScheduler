﻿using System;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Represents a wireless network profile
	/// </summary>
	internal class NetworkProfile
	{
		private const string RegPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles";

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkProfile"/> class using the GUID of the network profile.
		/// </summary>
		/// <param name="guid">The GUID of the network profile.</param>
		public NetworkProfile(Guid guid) : this(guid.ToString("B")) { }

		private NetworkProfile(string guid)
		{
			try
			{
				using (RegistryKey hk = Registry.LocalMachine.OpenSubKey(RegPath + "\\" + guid))
				{
					if (hk != null)
					{
						this.Name = hk.GetValue("ProfileName").ToString();
						this.Id = new Guid(guid);
					}
				}
			}
			catch { }
		}

		/// <summary>
		/// Gets the name of the profile.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the GUID of the profile.
		/// </summary>
		/// <value>The id.</value>
		public Guid Id { get; private set; }

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <paramref name="obj"/> parameter is null.
		/// </exception>
		public override bool Equals(object obj)
		{
			if (obj is NetworkProfile)
				return ((NetworkProfile)obj).Id == this.Id;
			else if (obj is Guid)
				return ((Guid)obj) == this.Id;
			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>
		/// Gets all local profiles.
		/// </summary>
		/// <returns>Array of <see cref="NetworkProfile"/> objects.</returns>
		public static NetworkProfile[] GetAllLocalProfiles()
		{
			using (RegistryKey hk = Registry.LocalMachine.OpenSubKey(NetworkProfile.RegPath))
			{
				if (hk != null)
				{
					NetworkProfile[] ret = new NetworkProfile[hk.SubKeyCount];
					string[] sks = hk.GetSubKeyNames();
					for (int i = 0; i < sks.Length; i++)
						ret[i] = new NetworkProfile(sks[i]);
					return ret;
				}
			}
			return new NetworkProfile[0];
		}
	}
}