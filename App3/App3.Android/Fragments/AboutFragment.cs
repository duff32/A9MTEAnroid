using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using ClassLibrary;

namespace App3.Droid
{
    public class AboutFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {
        public static AboutFragment NewInstance() =>
            new AboutFragment { Arguments = new Bundle() };

        public AboutViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here            
        }

        Button learnMoreButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_about, container, false);

            //var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
            //string stringFaculty = prefs.GetString("Faculty", string.Empty);
            //int facultyId = 0;
            //if (!string.IsNullOrEmpty(stringFaculty))
            //   facultyId = Convert.ToInt32(stringFaculty);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            //spinner.SetSelection(facultyId);
            var x = spinner.SelectedItemId;

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    Context, Resource.Array.faculty_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            ViewModel = new AboutViewModel();
            return view;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Byla vybrána {0}", spinner.GetItemAtPosition(e.Position));
            var x = spinner.SelectedItemId;
            var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("Faculty", spinner.SelectedItemId.ToString());
            editor.Commit();

            Toast.MakeText(Context, toast, ToastLength.Long).Show();
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        public void BecameVisible()
        {

        }
    }
}
