using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using ClassLibrary;
using System;

namespace App3.Droid
{
    [Activity(Label = "Details", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = ".MainActivity")]
    public class BrowseItemDetailActivity : BaseActivity
    {
        FloatingActionButton saveButton;
        EditText fName, lName, email;
        Student item;
        /// <summary>
        /// Specify the layout to inflace
        /// </summary>
        protected override int LayoutResource => Resource.Layout.activity_item_details;

        public StudentViewModel ViewModel { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var data = Intent.GetStringExtra("data");

            item = Newtonsoft.Json.JsonConvert.DeserializeObject<Student>(data);
            ViewModel = BrowseFragment.ViewModel;

            //SetContentView(Resource.Layout.activity_item_details);
            saveButton = FindViewById<FloatingActionButton>(Resource.Id.save_button);
            fName = FindViewById<EditText>(Resource.Id.input_fname);
            fName.Text = item.FName;
            lName = FindViewById<EditText>(Resource.Id.input_lname);
            lName.Text = item.LName;
            email = FindViewById<EditText>(Resource.Id.input_email);
            email.Text = item.Email;

            SupportActionBar.Title = item.FName + " " + item.LName;
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var oldItem = new Student { Id = item.Id, FName = item.FName, LName = item.LName, Email = item.Email };
            item.FName = fName.Text;
            item.LName = lName.Text;
            item.Email = email.Text;
            ViewModel.UpdateItemCommand.Execute(new Tuple<Student,Student>(oldItem,item));

            Finish();
        }

        //void SaveButton_Click(object sender, EventArgs e)
        //{
        //    var item = new Student
        //    {
        //        FName = fName.Text,
        //        LName = lName.Text,
        //        Email = email.Text
        //    };
        //    ViewModel.UpdateItemCommand.Execute(item);

        //    Finish();
        //}

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
