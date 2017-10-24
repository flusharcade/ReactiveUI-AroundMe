
namespace ReactiveUIAroundMe.Portable.MVVM
{
	using System;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public class ObservableObject : INotifyPropertyChanged
	{
		#region Public Events

		/// <summary>
		/// The property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Protected Methods

		/// <summary>
		/// Sets the property.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		/// <param name="referenceProperty">Reference property.</param>
		/// <param name="newProperty">New property.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected void RaiseAndSetIfChanged<T>(string propertyName, ref T referenceProperty, T newProperty)
		{
			if (!newProperty.Equals(referenceProperty))
			{
				referenceProperty = newProperty;
			}

			OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}
