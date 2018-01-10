using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.App;
using Android.Content;
using ClassLibrary;

namespace App3.Droid
{
    public class BrowseFragment : Android.Support.V4.App.Fragment, IFragmentVisible
    {
        public static BrowseFragment NewInstance() =>
            new BrowseFragment { Arguments = new Bundle() };

        BrowseItemsAdapter adapter;
        SwipeRefreshLayout refresher;

        ProgressBar progress;
        public static StudentViewModel ViewModel { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewModel = new StudentViewModel();

            View view = inflater.Inflate(Resource.Layout.fragment_browse, container, false);
            var recyclerView =
                view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter = new BrowseItemsAdapter(Activity, ViewModel));

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeColors(Resource.Color.accent);

            progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
            progress.Visibility = ViewStates.Gone;

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            refresher.Refresh += Refresher_Refresh;
            adapter.ItemClick += Adapter_ItemClick;
            adapter.ItemLongClick += Adapter_ItemLongClick;

            if (ViewModel.Items.Count == 0)
                ViewModel.LoadItemsCommand.Execute(null);
        }

        private void Adapter_ItemLongClick(object sender, RecyclerClickEventArgs e)
        {
            var item = ViewModel.Items[e.Position];
            AlertDialog.Builder alert = new AlertDialog.Builder(Context);
            alert.SetTitle("Smazání záznamu studenta");
            alert.SetMessage($"Opravdu chcete smazat záznam o studentovi {item.LName} {item.FName}?");
            alert.SetPositiveButton("Smazat", (senderAlert, args) => {
                ViewModel.DeleteItemCommand.Execute(item);
                Toast.MakeText(Context, "Student byl smazán", ToastLength.Short).Show();
            });

            alert.SetNegativeButton("Zpět", (senderAlert, args) => {
                Toast.MakeText(Context, "Zrušeno", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public override void OnStop()
        {
            base.OnStop();
            refresher.Refresh -= Refresher_Refresh;
            adapter.ItemClick -= Adapter_ItemClick;
            adapter.ItemLongClick -= Adapter_ItemLongClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var item = ViewModel.Items[e.Position];
            var intent = new Intent(Activity, typeof(BrowseItemDetailActivity));

            intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
            Activity.StartActivity(intent);
        }

        void Refresher_Refresh(object sender, EventArgs e)
        {
            ViewModel.LoadItemsCommand.Execute(null);
            refresher.Refreshing = false;

            //Activity activity = this.Context CrossCurrentActivity.Current.Activity;
            //if(ViewModel.Items?.Count == 0)
            //    Toast.MakeText(this.Context, "Žádná data k zobrazení.", ToastLength.Long).Show();

        }

        public void BecameVisible()
        {

        }
    }

    class BrowseItemsAdapter : BaseRecycleViewAdapter
    {
        StudentViewModel viewModel;
        Activity activity;

        public BrowseItemsAdapter(Activity activity, StudentViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.activity = activity;

            this.viewModel.Items.CollectionChanged += (sender, args) =>
            {
                this.activity.RunOnUiThread(NotifyDataSetChanged);
            };
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.item_browse;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = viewModel.Items[position];
            if (item != null)
            {
                // Replace the contents of the view with that element
                var myHolder = holder as MyViewHolder;
                myHolder.TextView.Text = item.FName + " " + item.LName;
                myHolder.DetailTextView.Text = item.Email;
            }
        }

        public override int ItemCount => viewModel.Items.Count;
    }

    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public TextView DetailTextView { get; set; }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            DetailTextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);
            itemView.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }
}
