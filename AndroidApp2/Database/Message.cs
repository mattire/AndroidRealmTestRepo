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

using Realms;

namespace AndroidApp2.Database
{
    class Message : RealmObject, IMessage
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
    }

    class MessageWrpr : IMessage
    {
        public Message Msg { get; set; }

        public string Id
        {
            get
            {
                return ((IMessage)Msg).Id;
            }
            set
            {
                ((IMessage)Msg).Id = value;
            }
        }

        public string Text
        {
            get
            {
                return ((IMessage)Msg).Text;
            }
            set
            {
                ((IMessage)Msg).Text = value;
            }
        }

    }
}