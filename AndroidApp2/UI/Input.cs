using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidApp2.UI
{
    class Input
    {
        static public void InputTxt(Activity act, string title, Action<string> handleResp, string earlier = "")
        {
            //var act = MainActivity.Current;
            EditText et = new EditText(act);
            et.Text = earlier;
            Button button = new Button(act);
            LinearLayout linearLayout = new LinearLayout(act);
            linearLayout.Orientation = Orientation.Vertical;
            button.Text = "Ok";
            linearLayout.AddView(et);
            linearLayout.AddView(button);
            AlertDialog.Builder ad = new AlertDialog.Builder(act);
            ad.SetTitle(title);
            //ad.SetView(et);
            ad.SetView(linearLayout);
            var d = ad.Create();
            d.Show();

            button.Click += (s, e) => {
                act.RunOnUiThread(() => {
                    handleResp(et.Text);
                    d.Dismiss();
                });
            };

        }
    }
}