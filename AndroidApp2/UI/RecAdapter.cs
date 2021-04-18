using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidApp2.Database;
using Realms;
using DbMessage = AndroidApp2.Database.Message;

namespace AndroidApp2.UI
{

    class RecAdapter : Android.Support.V7.Widget.RecyclerView.Adapter
    {
        private IDisposable mSubscription;

        public RecAdapter(Activity activity)
        {
            Activity = activity;
            Current = this;
            SubscribeMessages();
        }


        public static RecAdapter Current { get; internal set; }

        public override int ItemCount => Messages.Count();

        public IQueryable<Database.Message> Messages {
            get {
                return Realm.GetInstance(RealmDb.Config).All<Database.Message>();
                //var r = Realm.GetInstance();
                //return r.All<Database.Message>();
                //return Database.RealmDb.Instance.Get<Database.Message>();
            }
        }

        public Activity Activity { get; }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var msg = Messages.ElementAt(position);
            var vh = holder as RecVH;
            vh.SetValue(msg);
            //vh.MTextView.Text = msg.Text;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new RecVH(new TextView(Activity));
        }

        private void SubscribeMessages()
        {
            var r = Realm.GetInstance(RealmDb.Config);
            mSubscription = r.All<DbMessage>().SubscribeForNotifications((sender, changes, errors) =>
            {
                try
                {
                    if (changes != null)
                    {
                        System.Diagnostics.Debug.WriteLine(changes.ModifiedIndices.Count());
                        System.Diagnostics.Debug.WriteLine(changes.DeletedIndices.Count());
                        System.Diagnostics.Debug.WriteLine(changes.InsertedIndices.Count());
                        System.Diagnostics.Debug.WriteLine("");

                        // Could do much detailed refresh

                        foreach (var ind in changes.DeletedIndices) { NotifyItemRemoved(ind); }
                        foreach (var ind in changes.InsertedIndices) { NotifyItemInserted(ind); }
                        foreach (var ind in changes.ModifiedIndices) { NotifyItemChanged(ind); }

                        //Messages =
                        NotifyDataSetChanged();
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            });
        }

        ~RecAdapter() {
            mSubscription.Dispose();
        }
    }
}