using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
//using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidApp2.Database;
using RecView = Android.Support.V7.Widget.RecyclerView;
using LinLayMan = Android.Support.V7.Widget.LinearLayoutManager;
using AndroidApp2.UI;
using System.Threading.Tasks;

namespace AndroidApp2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public Android.Support.V7.Widget.RecyclerView mRecyclerView1 { get; private set; }
        public Android.Support.V7.Widget.LinearLayoutManager mLayoutManager { get; private set; }
        internal RecAdapter mAdapter { get; private set; }

        public static MainActivity Current;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            mRecyclerView1 = FindViewById<RecView>(Resource.Id.recyclerView1);
            mLayoutManager = new LinLayMan(this);
            mRecyclerView1.SetLayoutManager(mLayoutManager);
            mAdapter = new UI.RecAdapter(this);
            mRecyclerView1.SetAdapter(mAdapter);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            Current = this;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }
            if (id == Resource.Id.action_test)
            {
                ThreadTest();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ThreadTest()
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(100);
                    //var r = RealmDb.GetRealmInstance();
                    
                    //RealmDb.Instance.AddMessage("Test msg");
                    RealmDb.AddMessage("Test msg");

                }
                catch (Exception exp)
                {
                    System.Diagnostics.Debug.WriteLine(exp.Message);
                }
            });
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //View view = (View) sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

            UI.Input.InputTxt(this, "Enter msg", (s) => {
                //RealmDb.Instance.AddMessage(s);
                RealmDb.AddMessage(s);
            });
            
        }
	}
}

