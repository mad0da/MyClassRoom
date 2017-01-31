using System.Collections.Generic;
using Xamarin.Forms;

namespace myClassroom
{
	public partial class MasterPage : ContentPage
	{
		public ListView ListView { get { return listView; } }

		public MasterPage ()
		{
			InitializeComponent ();

			var masterPageItems = new List<MasterPageItem> ();
			masterPageItems.Add (new MasterPageItem {
				Title = "myProfile",
				IconSource = "Education icons by Dreamstale (58).png",
				TargetType = typeof(ContactsPage)
			});
			masterPageItems.Add (new MasterPageItem {
				Title = "myPlanner",
				IconSource = "Education icons by Dreamstale (33)",
				TargetType = typeof(TodoListPage)
			});
			masterPageItems.Add (new MasterPageItem {
				Title = "mySettings",
				IconSource = "Education icons by Dreamstale (22).png",
				TargetType = typeof(ReminderPage)
			});
            masterPageItems.Add(new MasterPageItem
            {
                Title = "mySupport",
                IconSource = "Education icons by Dreamstale (59).png",
                TargetType = typeof(ReminderPage)
            });


            listView.ItemsSource = masterPageItems;
		}
	}
}
