using System;
using Xamarin.Forms;

namespace MyClassRoom

{
	public class MCR_MainPageCS : MCR_MasterDetailPage
	{
		MasterPageCS masterPage;

		public MCR_MainPageCS()
		{
			masterPage = new MCR_MainPageCS();
			Master = masterPage;
			Detail = new NavigationPage (new ContactsPageCS ());

			masterPage.ListView.ItemSelected += OnItemSelected;

			if (Device.OS == TargetPlatform.Windows)
            {
				Master.Icon = "swap.png";
			}
		}

		void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null) {
				Detail = new NavigationPage ((Page)Activator.CreateInstance (item.TargetType));
				masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}
}
