using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using ClassLibrary;

namespace App3.Droid
{
    [Activity(Label = "AddItemActivity", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = ".MainActivity")]
    public class AddItemActivity : Activity
    {
        FloatingActionButton saveButton;
        EditText fName, lName, email;

        public StudentViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = BrowseFragment.ViewModel;

            // Create your application here
            SetContentView(Resource.Layout.activity_add_item);
            saveButton = FindViewById<FloatingActionButton>(Resource.Id.save_button);
            fName = FindViewById<EditText>(Resource.Id.input_fname);
            lName = FindViewById<EditText>(Resource.Id.input_lname);
            email = FindViewById<EditText>(Resource.Id.input_email);

            saveButton.Click += SaveButton_Click;
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            var item = new Student
            {
                FName = fName.Text,
                LName = lName.Text,
                Email = email.Text
            };
            ViewModel.AddItemCommand.Execute(item);

            Finish();
        }
    }
}
