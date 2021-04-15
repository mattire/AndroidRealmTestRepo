using Android.Views;
using Android.Widget;

namespace AndroidApp2.UI
{
    class RecVH : Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        TextView mTextView;
        private Database.Message mMsg;

        public RecVH(View itemView) : base(itemView)
        {
            MTextView = (TextView) itemView;
            MTextView.Click += MTextView_Click;
        }

        private TextView MTextView { get => mTextView; set => mTextView = value; }

        public void SetValue(Database.Message msg) {
            mMsg = msg;
            MTextView.Text = mMsg.Text;
            mMsg.PropertyChanged += MMsg_PropertyChanged;
        }

        private void MMsg_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MTextView.Text = mMsg.Text;
        }

        private void MTextView_Click(object sender, System.EventArgs e)
        {
            var txt = MTextView.Text;
            Input.InputTxt(MainActivity.Current, "Update msg", (s) =>
            {
                Database.RealmDb.Instance.RealmInstance.Write(() =>
                {
                    mMsg.Text = s;
                });
                //RecAdapter.Current.NotifyDataSetChanged();
            }, txt);
        }
    }
}