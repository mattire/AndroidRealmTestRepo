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

namespace AndroidApp2.UI
{

    class RecAdapter : Android.Support.V7.Widget.RecyclerView.Adapter
    {
        public RecAdapter(Activity activity)
        {
            Activity = activity;
            Current = this;
        }

        public static RecAdapter Current { get; internal set; }

        public override int ItemCount => Messages.Count();

        public IQueryable<Database.Message> Messages {
            get {
                return Database.RealmDb.Instance.Get<Database.Message>();
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

        public void Update() {
            NotifyDataSetChanged();
        }
    }
}