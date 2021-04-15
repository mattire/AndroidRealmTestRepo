﻿using System;
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
using RealmClone;

namespace AndroidApp2.Database
{
    class RealmDb
    {
        private Realm _realm;

        private static readonly Lazy<RealmDb> lazy = new Lazy<RealmDb>(() => new RealmDb());

        public static RealmDb Instance { get { return lazy.Value; } }

        public Realm RealmInstance { get { return _realm; } }

        private RealmDb()
        {
            //var mThread = System.Threading.Thread.CurrentThread;
            Init();
        }

        public void Init() {
            //using (var real)
            _realm = Realm.GetInstance();
        }

        public void AddMessage(string text) {
            _realm.Write(() =>
            {
                var m = new Message() { Text = text };
                _realm.Add(m);
            });
            //AndroidApp2.UI.RecAdapter.Current.Update();
            AndroidApp2.UI.RecAdapter.Current.NotifyDataSetChanged();
        }

        public Message GetMessage(string txtStartsWith)
        {
            var m1 = _realm.All<Message>().Where(m => m.Text.StartsWith(txtStartsWith)).First();
            var cm = m1.Clone();
            return cm;
        }

        public IQueryable<T> Get<T>() where T: RealmObject
        {
            return _realm.All<T>();
        }


    }
}